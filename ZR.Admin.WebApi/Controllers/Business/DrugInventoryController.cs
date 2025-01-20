using Microsoft.AspNetCore.Mvc;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Service.Business.IBusinessService;
using ZR.Admin.WebApi.Filters;
using MiniExcelLibs;
using Newtonsoft.Json;
using ZR.Model.GuiHis;
using ZR.Service.Business;

//创建时间：2024-12-28
namespace ZR.Admin.WebApi.Controllers.Business
{
    /// <summary>
    /// 采购退货
    /// </summary>
    [Verify]
    [Route("business/DrugInventory")]
    public class DrugInventoryController : BaseController
    {
        /// <summary>
        /// 采购退货接口
        /// </summary>
        private readonly IDrugInventoryService _DrugInventoryService;
        private readonly IOutOrderService _OutOrderService;
        private readonly IOuWarehousetService _OuWarehousetService;
        private readonly IOutDrugsService _OutDrugsService ;

        public DrugInventoryController(IDrugInventoryService DrugInventoryService, IOutOrderService outOrderService, IOuWarehousetService ouWarehousetService, IOutDrugsService outDrugsService)
        {
            _DrugInventoryService = DrugInventoryService;
            _OutOrderService = outOrderService;
            _OuWarehousetService = ouWarehousetService;
            _OutDrugsService = outDrugsService;
        }

        /// <summary>
        /// 查询采购退货列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "druginventory:list")]
        public IActionResult QueryDrugInventory([FromQuery] DrugInventoryQueryDto parm)
        {
            var response = _DrugInventoryService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询采购退货详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "druginventory:query")]
        public IActionResult GetDrugInventory(int Id)
        {
            var response = _DrugInventoryService.GetInfo(Id);
            
            var info = response.Adapt<DrugInventoryDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加采购退货
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "druginventory:add")]
        [Log(Title = "采购退货", BusinessType = BusinessType.INSERT)]
        public IActionResult AddDrugInventory([FromBody] DrugInventoryDto parm)
        {
            var modal = parm.Adapt<DrugInventory>().ToCreate(HttpContext);

            var response = _DrugInventoryService.AddDrugInventory(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新采购退货
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "druginventory:edit")]
        [Log(Title = "采购退货", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateDrugInventory([FromBody] DrugInventoryDto parm)
        {
            var modal = parm.Adapt<DrugInventory>().ToUpdate(HttpContext);
            var response = _DrugInventoryService.UpdateDrugInventory(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除采购退货
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "druginventory:delete")]
        [Log(Title = "采购退货", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteDrugInventory([FromRoute]string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);

            return ToResponse(_DrugInventoryService.Delete(idArr));
        }

        /// <summary>
        /// 导出采购退货
        /// </summary>
        /// <returns></returns>
        [Log(Title = "采购退货", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "druginventory:export")]
        public IActionResult Export([FromQuery] DrugInventoryQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _DrugInventoryService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "采购退货", "采购退货");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 清空采购退货
        /// </summary>
        /// <returns></returns>
        [Log(Title = "采购退货", BusinessType = BusinessType.CLEAN)]
        [ActionPermissionFilter(Permission = "druginventory:delete")]
        [HttpDelete("clean")]
        public IActionResult Clear()
        {
            if (!HttpContextExtension.IsAdmin(HttpContext))
            {
                return ToResponse(ResultCode.FAIL, "操作失败");
            }
            return SUCCESS(_DrugInventoryService.TruncateDrugInventory());
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "采购退货导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false)]
        [ActionPermissionFilter(Permission = "druginventory:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<DrugInventoryDto> list = new();
            using (var stream = formFile.OpenReadStream())
            {
                list = stream.Query<DrugInventoryDto>(startCell: "A1").ToList();
            }

            return SUCCESS(_DrugInventoryService.ImportDrugInventory(list.Adapt<List<DrugInventory>>()));
        }

        /// <summary>
        /// 采购退货导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "采购退货模板", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            var result = DownloadImportTemplate(new List<DrugInventoryDto>() { }, "DrugInventory");
            return ExportExcel(result.Item2, result.Item1);
        }
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
                //清空表
                _DrugInventoryService.TruncateDrugInventory();


                PhaOutInQuery phaOutInQuery = new PhaOutInQuery
                {
                    beginTime = DateTime.Now.AddHours(-1).ToString("yyyy-M-d HH:mm:ss"), // 当前时间减去一小时并格式化
                    endTime = DateTime.Now.AddHours(1).ToString("yyyy-M-d HH:mm:ss")      // 当前时间加上一小时并格式化
                };

                var x = await SendRequestsAsync(phaOutInQuery);

                foreach (var item in x)
                {
                    //var nu = _DrugInventoryService.GetInfo(item.OutBillCode, item.GroupCode);
                    // if (nu == null)
                    //{
                        var modal = item.Adapt<DrugInventory>().ToCreate(HttpContext);
                        _DrugInventoryService.AddDrugInventory(modal);
                    //}
                }
                return SUCCESS("true");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
        private async Task<List<DrugInventory>> SendRequestsAsync(PhaOutInQuery phaOutInQuery)
        {
            using (var client = new HttpClient())
            {
                //http://192.168.2.21:9403/His/GetPhaGiveBack


                string url = $"http://192.168.2.21:9403/His/GetPhaGiveBack?beginTime={phaOutInQuery.beginTime:yyyy-MM-dd}&endTime={phaOutInQuery.endTime:yyyy-MM-dd}";

                // 发送 GET 请求
                HttpResponseMessage response = await client.GetAsync(url);
                //string url = "http://192.168.2.21:9403/His/GetPhaGiveBack";
                //var json = JsonConvert.SerializeObject(phaOutInQuery);
                //var content = new StringContent(json, Encoding.UTF8, "application/json");
                //HttpResponseMessage response = await client.PostAsync(url, content);
                // 获取响应内容
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    // 解析 JSON 响应
                    var apiResponse = JsonConvert.DeserializeObject<List<DrugInventory>>(responseContent);
                    return apiResponse; // 返回 ApiResponse 对象
                }
                else
                {
                    // 处理错误
                    throw new Exception($"Error: {response.StatusCode}, Message: {response.ReasonPhrase}, Response: {responseContent}");
                }
            }
        }

        /// <summary>
        /// 增加到出库单
        /// </summary>
        /// <param name="phaOuts"></param>
        /// <returns></returns>
        [HttpPost("Addout")]
        //[ActionPermissionFilter(Permission = "outOrder:query")]
        public IActionResult Addout([FromBody] List<DrugInventory> drugInventorys)
        {
            // 用于存储已经处理的出库单
            var processedOrders = new Dictionary<string, OutOrder>();
            OutOrder currentOrder = new OutOrder();

            foreach (var item in drugInventorys)
            {
                // 创建唯一的出库单标识
                string orderKey = $"{item.DrugDeptCode}-{item.CompanyCode}-{item.InvoiceNo}";
                //OutOrder currentOrder = new OutOrder();
                // 检查是否已经存在相同的出库单
                if (!processedOrders.ContainsKey(orderKey))
                {
                    // 如果不存在，创建新的出库单
                    OutOrder outOrder = new OutOrder
                    {
                        OutOrderCode = GenerateUniqueOutOrderCode(), // 生成唯一的出库单代码
                        OutWarehouseID = item.DrugDeptCode,
                        InpharmacyId = item.CompanyCode,
                        UseReceive = item.OperCode,
                        Type = "06",
                        OutBillCode = 10,
                        Billcodesf = item.InvoiceNo,
                        //InpharmacyId = item.DrugStorageCode,
                        Times = DateTime.Now
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
                _OutDrugsService.AddOutDrugs(OuWarehouset);
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

        private OutDrugs MapPhaOutToOuWarehouset(DrugInventory phaOut, int outOrderId)
        {
            var OuWarehouset = new OutDrugs
            {
                OutorderID = outOrderId // 设置 OutOrderId
            };

            var phaOutProperties = typeof(DrugInventory).GetProperties();
            var OuWarehousetProperties = typeof(OutDrugs).GetProperties();

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
    }
}