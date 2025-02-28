using Microsoft.AspNetCore.Mvc;
using ZR.Model.Business;
using ZR.Admin.WebApi.Filters;
using MiniExcelLibs;
using ZR.Model.GuiHis;
using ZR.Model.GuiHis.Dto;
using ZR.Service.Guiz.IGuizService;
using Newtonsoft.Json;
using System.Text;
using ZR.Service.Guiz;
using Org.BouncyCastle.Asn1.Ocsp;
using ZR.Service.Business.IBusinessService;
using ZR.Service.Business;
using Aliyun.OSS;
using MailKit.Search;
using Microsoft.Data.SqlClient;

//创建时间：2024-11-27
namespace ZR.Admin.WebApi.Controllers.Gui
{
    /// <summary>
    /// 出库记录
    /// </summary>
    [AllowAnonymous]

    [Route("business/PhaOut")]
    public class PhaOutController : BaseController
    {
        /// <summary>
        /// 出库记录接口
        /// </summary>
        private readonly IPhaOutService _PhaOutService;
        private readonly IOutOrderService _OutOrderService;
        private readonly IOuWarehousetService _OuWarehousetService;


        public PhaOutController(IPhaOutService PhaOutService,IOutOrderService outOrderService,IOuWarehousetService ouWarehousetService)
        {
            _PhaOutService = PhaOutService;
                
            _OutOrderService = outOrderService;

            _OuWarehousetService= ouWarehousetService;
        }

        /// <summary>
        /// 查询出库记录列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "phaout:list")]
        public IActionResult QueryPhaOut([FromQuery] PhaOutQueryDto parm)
        {
            var response = _PhaOutService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询出库记录详情
        /// </summary>
        /// <param name="outBillCode"></param>
        /// <returns></returns>
        [HttpGet("{outBillCode}/{gr}")]
        [ActionPermissionFilter(Permission = "phaout:query")]
        public IActionResult GetPhaOut(long outBillCode,string gr)
        {
            var response = _PhaOutService.GetInfo(outBillCode,gr);

            var info = response.Adapt<PhaOutDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 增加到出库单
        /// </summary>
        /// <param name="phaOuts"></param>
        /// <returns></returns>
        [HttpPost("Addout")]
        [ActionPermissionFilter(Permission = "outOrder:query")]
        public IActionResult Addout([FromBody] List<PhaOut> phaOuts)
        {
            // 用于存储已经处理的出库单
            
            var processedOrders = new Dictionary<string, OutOrder>();
            var codeOrders = new Dictionary<string, string>();

            OutOrder currentOrder = new OutOrder();
            List<PhaOut> pa = new List<PhaOut>();

            foreach (var item in phaOuts)
            {
                string orderKey = item.OutListCode;
                if (!codeOrders.ContainsKey(orderKey))
                {
                    pa.AddRange(_PhaOutService.Alloutcode(item.OutListCode));
                    codeOrders[orderKey] = orderKey;
                }
            }

            foreach (var item in pa)
            {
                // 创建唯一的出库单标识
                string orderKey = $"{item.DrugDeptCode}-{item.OutListCode}-{item.DrugStorageCode}-{item.OutType}";
                //OutOrder currentOrder = new OutOrder();
                // 检查是否已经存在相同的出库单
                if (!processedOrders.ContainsKey(orderKey))
                {
                    // 如果不存在，创建新的出库单
                    OutOrder outOrder = new OutOrder
                    {
                        OutOrderCode = GenerateUniqueOutOrderCode(), // 生成唯一的出库单代码
                        OutWarehouseID = item.DrugDeptCode,
                        UseReceive = item.GetPersonName,
                        OutBillCode =100,
                        InpharmacyId = item.DrugStorageCode,
                        Times = DateTime.Now
                        ,Type=item.OutType,
                    };
                    var modal = outOrder.Adapt<OutOrder>().ToCreate(HttpContext);
                    currentOrder = _OutOrderService.AddOutOrder(modal);
                    // 将出库单添加到已处理的字典中
                    processedOrders[orderKey] = outOrder;
                }
                // 获取当前出库单
                //currentOrder = processedOrders[orderKey];
                // 将 PhaOut 数据添加到 OuWarehouset 表
                var OuWarehouset = MapPhaOutToOuWarehouset(item, currentOrder.Id);
                // 保存 OuWarehouset
                _OuWarehousetService.AddOuWarehouset(OuWarehouset);
            }
            return SUCCESS("出库单处理成功");
        }

        // 生成唯一的出库单代码的示例方法
        private string GenerateUniqueOutOrderCode()
        {
            string currentTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            // 生成一个随机四位数
            Random random = new Random();
            int randomFourDigit = random.Next(1000, 10000); // 生成范围在1000到9999之间的四位数
            // 组合当前时间和随机四位数
            return $"{currentTime}{randomFourDigit}";
        }

        private OuWarehouset MapPhaOutToOuWarehouset(PhaOut phaOut, int outOrderId)
        {
            var OuWarehouset = new OuWarehouset
            {
                OutorderID = outOrderId // 设置 OutOrderId
            };

            var phaOutProperties = typeof(PhaOut).GetProperties();
            var OuWarehousetProperties = typeof(OuWarehouset).GetProperties();

            foreach (var phaOutProp in phaOutProperties)
            {
                // 查找与 PhaOut 属性同名的 OuWarehouset 属性
                var matchingProp = OuWarehousetProperties.FirstOrDefault(p => p.Name == phaOutProp.Name);
                if (matchingProp != null && matchingProp.CanWrite)
                {
                    // 复制属性值
                    matchingProp.SetValue(OuWarehouset, phaOutProp.GetValue(phaOut));
                }
            }
            return OuWarehouset;
        }

        /// <summary>
        /// 添加出库记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "phaout:add")]
        [Log(Title = "出库记录", BusinessType = BusinessType.INSERT)]
        public IActionResult AddPhaOut([FromBody] PhaOutDto parm)
        {
            var modal = parm.Adapt<PhaOut>().ToCreate(HttpContext);

            var response = _PhaOutService.AddPhaOut(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新出库记录
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "phaout:edit")]
        [Log(Title = "出库记录", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdatePhaOut([FromBody] PhaOutDto parm)
        {
            var modal = parm.Adapt<PhaOut>().ToUpdate(HttpContext);
            var response = _PhaOutService.UpdatePhaOut(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除出库记录
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "phaout:delete")]
        [Log(Title = "出库记录", BusinessType = BusinessType.DELETE)]
        public IActionResult DeletePhaOut([FromRoute] string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);

            return ToResponse(_PhaOutService.Delete(idArr));
        }

        /// <summary>
        /// 导出出库记录
        /// </summary>
        /// <returns></returns>
        [Log(Title = "出库记录", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "phaout:export")]
        public IActionResult Export([FromQuery] PhaOutQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _PhaOutService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "出库记录", "出库记录");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 清空出库记录
        /// </summary>
        /// <returns></returns>
        [Log(Title = "出库记录", BusinessType = BusinessType.CLEAN)]
        [ActionPermissionFilter(Permission = "phaout:delete")]
        [HttpDelete("clean")]
        public IActionResult Clear()
        {
            if (!HttpContext.IsAdmin())
            {
                return ToResponse(ResultCode.FAIL, "操作失败");
            }
            return SUCCESS(_PhaOutService.TruncatePhaOut());
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "出库记录导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false)]
        [ActionPermissionFilter(Permission = "phaout:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<PhaOutDto> list = new();
            using (var stream = formFile.OpenReadStream())
            {
                list = stream.Query<PhaOutDto>(startCell: "A1").ToList();
            }

            return SUCCESS(_PhaOutService.ImportPhaOut(list.Adapt<List<PhaOut>>()));
        }

        /// <summary>
        /// 出库记录导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "出库记录模板", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            var result = DownloadImportTemplate(new List<PhaOutDto>() { }, "PhaOut");
            return ExportExcel(result.Item2, result.Item1);
        }

        //http://192.168.2.21:9403/His/GetPhaInPlanList?beginTime=2024-01-01&endTime=2024-01-31
        //http://ip:port/His/GetPhaOutList
        //
        /// <summary>
        /// 同步
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("TongBu")]
        public async Task<IActionResult> TongBu()
        {
            try
            {
                PhaOutInQuery phaOutInQuery = new PhaOutInQuery
                {
                    beginTime = DateTime.Now.AddHours(-1).ToString("yyyy-M-d HH:mm:ss"), // 当前时间减去一小时并格式化
                    endTime = DateTime.Now.AddHours(1).ToString("yyyy-M-d HH:mm:ss")      // 当前时间加上一小时并格式化
                };

                var x = await SendRequestsAsync(phaOutInQuery);

                // 检查 x 是否为 null
                if (x == null || !x.Any())
                {
                    return BadRequest("未获取到数据");
                }

                foreach (var item in x)
                {
                    if (item == null) // 检查 item 是否为 null
                    {
                        continue; // 跳过 null 项
                    }

                    var nu = _PhaOutService.GetInfo(item.OutBillCode, item.GroupCode);
                    if (nu != null)
                    {
                        var modal = item.Adapt<PhaOut>().ToUpdate(HttpContext);
                        _PhaOutService.UpdatePhaOut(modal);
                    }
                    else
                    {
                        var modal = item.Adapt<PhaOut>().ToCreate(HttpContext);
                        _PhaOutService.AddPhaOut(modal);
                    }
                }
                return SUCCESS("true");
            }
            catch (Exception ex)
            {
                // 记录异常信息
                // 可以使用日志记录工具记录 ex.Message 和 ex.StackTrace
                return StatusCode(500, $"服务器内部错误: {ex.Message}");
            }
        }

        private async Task<List<PhaOut>> SendRequestsAsync(PhaOutInQuery phaOutInQuery)
        {
            using (var client = new HttpClient())
            {
                string url = $"http://192.168.1.95:7800/His/GetPhaOutList?beginTime={phaOutInQuery.beginTime:yyyy-MM-dd}&endTime={phaOutInQuery.endTime:yyyy-MM-dd}";

                HttpResponseMessage response = await client.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<List<PhaOut>>(responseContent);
                    return apiResponse ?? new List<PhaOut>(); // 确保返回一个空列表而不是 null
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode}, Message: {response.ReasonPhrase}, Response: {responseContent}");
                }
            }
        }



    }
}