using Microsoft.AspNetCore.Mvc;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Service.Business.IBusinessService;
using ZR.Admin.WebApi.Filters;
using Newtonsoft.Json;
using ZR.Model.GuiHis;

//创建时间：2024-12-10
namespace ZR.Admin.WebApi.Controllers.Business
{
    /// <summary>
    /// 采购计划入库
    /// </summary>
    [Verify]
    [Route("business/TGInwarehouse")]
    public class TGInwarehouseController : BaseController
    {
        /// <summary>
        /// 采购计划入库接口
        /// </summary>
        private readonly ITGInwarehouseService _TGInwarehouseService;

        private readonly IInwarehouseService _InwarehouseService;

        public TGInwarehouseController(ITGInwarehouseService TGInwarehouseService, IInwarehouseService inwarehouseService)
        {
            _TGInwarehouseService = TGInwarehouseService;
            _InwarehouseService = inwarehouseService;
        }

        /// <summary>
        /// 查询采购计划入库列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "tginwarehouse:list")]
        public IActionResult QueryTGInwarehouse([FromQuery] TGInwarehouseQueryDto parm)
        {
            var response = _TGInwarehouseService.GetList(parm);
            return SUCCESS(response);
        }

        [HttpPost("push")]
        public IActionResult PushInwarehouseInfoToHis([FromBody] TGInwarehouseQueryDto parm)
        {
            var respones = _TGInwarehouseService.PushInwarehouseInfoToHis(parm);
            return SUCCESS(respones);
        }

        /// <summary>
        /// 生成入库单
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("generateInwarehouse")]
        [ActionPermissionFilter(Permission = "tginwarehouse:generateInwarehouse")]
        public IActionResult GenerateInwarehouse([FromBody] List<InwarehouseGenerateInwarehouseDto> parm)
        {
            var response = _InwarehouseService.generateInwarehouse(parm);
            return SUCCESS(response ? "处理成功" : "处理失败");
        }


        /// <summary>
        /// 查询采购计划入库详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "tginwarehouse:query")]
        public IActionResult GetTGInwarehouse(int Id)
        {
            var response = _TGInwarehouseService.GetInfo(Id);
            
            var info = response.Adapt<TGInwarehouseDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加采购计划入库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "tginwarehouse:add")]
        [Log(Title = "采购计划入库", BusinessType = BusinessType.INSERT)]
        public IActionResult AddTGInwarehouse([FromBody] TGInwarehouseDto parm)
        {
            var modal = parm.Adapt<TGInwarehouse>().ToCreate(HttpContext);

            var response = _TGInwarehouseService.AddTGInwarehouse(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新采购计划入库
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "tginwarehouse:edit")]
        [Log(Title = "采购计划入库", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateTGInwarehouse([FromBody] TGInwarehouseDto parm)
        {
            var modal = parm.Adapt<TGInwarehouse>().ToUpdate(HttpContext);
            var response = _TGInwarehouseService.UpdateTGInwarehouse(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除采购计划入库
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "tginwarehouse:delete")]
        [Log(Title = "采购计划入库", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteTGInwarehouse([FromRoute]string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);

            return ToResponse(_TGInwarehouseService.Delete(idArr));
        }

        /// <summary>
        /// 导出采购计划入库
        /// </summary>
        /// <returns></returns>
        [Log(Title = "采购计划入库", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "tginwarehouse:export")]
        public IActionResult Export([FromQuery] TGInwarehouseQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _TGInwarehouseService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "采购计划入库", "采购计划入库");
            return ExportExcel(result.Item2, result.Item1);
        }
        ///// <summary>
        ///// 同步
        ///// </summary>
        ///// <param name=""></param>
        ///// <returns></returns>
        //[HttpGet("TongBu")]
        //public async Task<IActionResult> TongBu()
        //{
        //    try
        //    {
        //        PhaInPlanInQuery planInQuery = new PhaInPlanInQuery();
        //        planInQuery.beginTime = new DateTime(2024, 12, 1);
        //        planInQuery.endTime = DateTime.Now;
        //        var x = await SendRequestsAsync(planInQuery);
        //        foreach (var item in x)
        //        {
        //            var n = _TGInwarehouseService.GetInfo(item.PlanNo);
        //            if (n != null)
        //            {
        //                var modal = item.Adapt<TGInwarehouse>().ToUpdate(HttpContext);
        //                var response = _TGInwarehouseService.UpdateTGInwarehouse(modal);
        //            }
        //            else if (n == null)
        //            {
        //                var modal = item.Adapt<TGInwarehouse>().ToCreate(HttpContext);
        //                var response = _TGInwarehouseService.AddTGInwarehouse(modal);
        //            }
        //        }
        //        return SUCCESS("true");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        //private async Task<List<PhaInPlan>> SendRequestsAsync(PhaInPlanInQuery requests)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        //string url = "http://192.168.2.21:9403/His/GetPhaInPlanList";
        //        //var json = JsonConvert.SerializeObject(requests);
        //        //var content = new StringContent(json, Encoding.UTF8, "application/json");
        //        string url = $"http://192.168.2.21:9403/His/GetPhaInPlanList?beginTime={requests.beginTime:yyyy-MM-dd}&endTime={requests.endTime:yyyy-MM-dd}";
        //        // 发送 GET 请求
        //        HttpResponseMessage response = await client.GetAsync(url);
        //        //HttpResponseMessage response = await client.PostAsync(url, content);
        //        // 获取响应内容
        //        var responseContent = await response.Content.ReadAsStringAsync();
        //        if (response.IsSuccessStatusCode)
        //        {
        //            // 解析 JSON 响应
        //            var apiResponse = JsonConvert.DeserializeObject<List<PhaInPlan>>(responseContent);
        //            return apiResponse; // 返回 ApiResponse 对象
        //        }
        //        else
        //        {
        //            // 处理错误
        //            throw new Exception($"Error: {response.StatusCode}, Message: {response.ReasonPhrase}, Response: {responseContent}");
        //        }
        //    }
        //}
    }
}