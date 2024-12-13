using Microsoft.AspNetCore.Mvc;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Service.Business.IBusinessService;
using ZR.Admin.WebApi.Filters;
using MiniExcelLibs;

//创建时间：2024-12-11
namespace ZR.Admin.WebApi.Controllers.Business
{
    /// <summary>
    /// 出库单
    /// </summary>
    [Verify]
    [Route("business/OutOrder")]
    public class OutOrderController : BaseController
    {
        /// <summary>
        /// 出库单接口
        /// </summary>
        private readonly IOutOrderService _OutOrderService;

        public OutOrderController(IOutOrderService OutOrderService)
        {
            _OutOrderService = OutOrderService;
        }

        /// <summary>
        /// 查询出库单列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "outorder:list")]
        public IActionResult QueryOutOrder([FromQuery] OutOrderQueryDto parm)
        {
            var response = _OutOrderService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询出库单详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "outorder:query")]
        public IActionResult GetOutOrder(int Id)
        {
            var response = _OutOrderService.GetInfo(Id);
            
            var info = response.Adapt<OutOrderDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加出库单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "outorder:add")]
        [Log(Title = "出库单", BusinessType = BusinessType.INSERT)]
        public IActionResult AddOutOrder([FromBody] OutOrderDto parm)
        {
            var modal = parm.Adapt<OutOrder>().ToCreate(HttpContext);

            var response = _OutOrderService.AddOutOrder(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新出库单
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "outorder:edit")]
        [Log(Title = "出库单", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateOutOrder([FromBody] OutOrderDto parm)
        {
            var modal = parm.Adapt<OutOrder>().ToUpdate(HttpContext);
            var response = _OutOrderService.UpdateOutOrder(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除出库单
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "outorder:delete")]
        [Log(Title = "出库单", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteOutOrder([FromRoute]string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);

            return ToResponse(_OutOrderService.Delete(idArr));
        }

        /// <summary>
        /// 导出出库单
        /// </summary>
        /// <returns></returns>
        [Log(Title = "出库单", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "outorder:export")]
        public IActionResult Export([FromQuery] OutOrderQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _OutOrderService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "出库单", "出库单");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 清空出库单
        /// </summary>
        /// <returns></returns>
        [Log(Title = "出库单", BusinessType = BusinessType.CLEAN)]
        [ActionPermissionFilter(Permission = "outorder:delete")]
        [HttpDelete("clean")]
        public IActionResult Clear()
        {
            if (!HttpContextExtension.IsAdmin(HttpContext))
            {
                return ToResponse(ResultCode.FAIL, "操作失败");
            }
            return SUCCESS(_OutOrderService.TruncateOutOrder());
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "出库单导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false)]
        [ActionPermissionFilter(Permission = "outorder:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<OutOrderDto> list = new();
            using (var stream = formFile.OpenReadStream())
            {
                list = stream.Query<OutOrderDto>(startCell: "A1").ToList();
            }

            return SUCCESS(_OutOrderService.ImportOutOrder(list.Adapt<List<OutOrder>>()));
        }

        /// <summary>
        /// 出库单导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "出库单模板", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            var result = DownloadImportTemplate(new List<OutOrderDto>() { }, "OutOrder");
            return ExportExcel(result.Item2, result.Item1);
        }

    }
}