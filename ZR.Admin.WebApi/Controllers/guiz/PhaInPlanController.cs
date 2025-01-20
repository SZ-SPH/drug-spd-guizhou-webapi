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
//创建时间：2024-11-27
namespace ZR.Admin.WebApi.Controllers.Gui
{
    /// <summary>
    /// 入库计划
    /// </summary>
    [AllowAnonymous]

    [Route("business/PhaInPlan")]
    public class PhaInPlanController : BaseController
    {
        /// <summary>
        /// 入库计划接口
        /// </summary>
        private readonly IPhaInPlanService _PhaInPlanService;

        public PhaInPlanController(IPhaInPlanService PhaInPlanService)
        {
            _PhaInPlanService = PhaInPlanService;
        }

        /// <summary>
        /// 查询入库计划列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "phainplan:list")]
        public IActionResult QueryPhaInPlan([FromQuery] PhaInPlanQueryDto parm)
        {
            var response = _PhaInPlanService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询入库计划详情
        /// </summary>
        /// <param name="PlanNo"></param>
        /// <returns></returns>
        [HttpGet("{PlanNo}")]
        [ActionPermissionFilter(Permission = "phainplan:query")]
        public IActionResult GetPhaInPlan(decimal PlanNo)
        {
            var response = _PhaInPlanService.GetInfo(PlanNo);

            var info = response.Adapt<PhaInPlanDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加入库计划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "phainplan:add")]
        [Log(Title = "入库计划", BusinessType = BusinessType.INSERT)]
        public IActionResult AddPhaInPlan([FromBody] PhaInPlanDto parm)
        {
            var modal = parm.Adapt<PhaInPlan>().ToCreate(HttpContext);

            var response = _PhaInPlanService.AddPhaInPlan(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新入库计划
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "phainplan:edit")]
        [Log(Title = "入库计划", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdatePhaInPlan([FromBody] PhaInPlanDto parm)
        {
            var modal = parm.Adapt<PhaInPlan>().ToUpdate(HttpContext);
            var response = _PhaInPlanService.UpdatePhaInPlan(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除入库计划
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "phainplan:delete")]
        [Log(Title = "入库计划", BusinessType = BusinessType.DELETE)]
        public IActionResult DeletePhaInPlan([FromRoute] string ids)
        {
            var idArr = Tools.SplitAndConvert<decimal>(ids);

            return ToResponse(_PhaInPlanService.Delete(idArr));
        }

        /// <summary>
        /// 导出入库计划
        /// </summary>
        /// <returns></returns>
        [Log(Title = "入库计划", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "phainplan:export")]
        public IActionResult Export([FromQuery] PhaInPlanQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _PhaInPlanService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "入库计划", "入库计划");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 清空入库计划
        /// </summary>
        /// <returns></returns>
        [Log(Title = "入库计划", BusinessType = BusinessType.CLEAN)]
        [ActionPermissionFilter(Permission = "phainplan:delete")]
        [HttpDelete("clean")]
        public IActionResult Clear()
        {
            if (!HttpContext.IsAdmin())
            {
                return ToResponse(ResultCode.FAIL, "操作失败");
            }
            return SUCCESS(_PhaInPlanService.TruncatePhaInPlan());
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "入库计划导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false)]
        [ActionPermissionFilter(Permission = "phainplan:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<PhaInPlanDto> list = new();
            using (var stream = formFile.OpenReadStream())
            {
                list = stream.Query<PhaInPlanDto>(startCell: "A1").ToList();
            }

            return SUCCESS(_PhaInPlanService.ImportPhaInPlan(list.Adapt<List<PhaInPlan>>()));
        }

        /// <summary>
        /// 入库计划导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "入库计划模板", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            var result = DownloadImportTemplate(new List<PhaInPlanDto>() { }, "PhaInPlan");
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
                PhaInPlanInQuery planInQuery = new PhaInPlanInQuery
                {
                    beginTime = DateTime.Now.AddHours(-1).ToString("yyyy-M-d HH:mm:ss"), // 当前时间减去一小时并格式化
                    endTime = DateTime.Now.AddHours(1).ToString("yyyy-M-d HH:mm:ss")      // 当前时间加上一小时并格式化
                };

                var x = await SendRequestsAsync(planInQuery);
                foreach (var item in x)
                {
                    var n = _PhaInPlanService.GetInfo(item.PlanNo);
                    if (n != null)
                    {
                        item.Qty = 0;
                        var modal = item.Adapt<PhaInPlan>().ToUpdate(HttpContext);
                        var response = _PhaInPlanService.UpdatePhaInPlan(modal);
                    }
                    else if (n == null)
                    {
                        item.Status = "0";
                        item.Qty = 0;
                        var modal = item.Adapt<PhaInPlan>().ToCreate(HttpContext);
                        var response = _PhaInPlanService.AddPhaInPlan(modal);
                    }
                }
                return SUCCESS("true");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<List<PhaInPlan>> SendRequestsAsync(PhaInPlanInQuery requests)
        {
            using (var client = new HttpClient())
            {

                //string url = "http://192.168.2.21:9403/His/GetPhaInPlanList";
                //var json = JsonConvert.SerializeObject(requests);
                //var content = new StringContent(json, Encoding.UTF8, "application/json");

                string url = $"http://192.168.2.21:9403/His/GetPhaInPlanList?beginTime={requests.beginTime:yyyy-MM-dd}&endTime={requests.endTime:yyyy-MM-dd}";

                // 发送 GET 请求
                HttpResponseMessage response = await client.GetAsync(url);

                //HttpResponseMessage response = await client.PostAsync(url, content);
                // 获取响应内容
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    // 解析 JSON 响应
                    var apiResponse = JsonConvert.DeserializeObject<List<PhaInPlan>>(responseContent);
                    return apiResponse; // 返回 ApiResponse 对象
                }
                else
                {
                    // 处理错误
                    throw new Exception($"Error: {response.StatusCode}, Message: {response.ReasonPhrase}, Response: {responseContent}");
                }
            }
        }
    }
}