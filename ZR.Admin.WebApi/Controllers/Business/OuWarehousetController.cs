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
    /// 出库药品详情
    /// </summary>
    [Verify]
    [Route("business/OuWarehouset")]
    public class OuWarehousetController : BaseController
    {
        /// <summary>
        /// 出库药品详情接口
        /// </summary>
        private readonly IOuWarehousetService _OuWarehousetService;

        public OuWarehousetController(IOuWarehousetService OuWarehousetService)
        {
            _OuWarehousetService = OuWarehousetService;
        }

        /// <summary>
        /// 查询出库药品详情列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "ouwarehouset:list")]
        public IActionResult QueryOuWarehouset([FromQuery] OuWarehousetQueryDto parm)
        {
            var response = _OuWarehousetService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询出库药品详情详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "ouwarehouset:query")]
        public IActionResult GetOuWarehouset(int Id)
        {
            var response = _OuWarehousetService.GetInfo(Id);
            
            var info = response.Adapt<OuWarehousetDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加出库药品详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "ouwarehouset:add")]
        [Log(Title = "出库药品详情", BusinessType = BusinessType.INSERT)]
        public IActionResult AddOuWarehouset([FromBody] OuWarehousetDto parm)
        {
            var modal = parm.Adapt<OuWarehouset>().ToCreate(HttpContext);

            var response = _OuWarehousetService.AddOuWarehouset(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新出库药品详情
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "ouwarehouset:edit")]
        [Log(Title = "出库药品详情", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateOuWarehouset([FromBody] OuWarehousetDto parm)
        {
            var modal = parm.Adapt<OuWarehouset>().ToUpdate(HttpContext);
            var response = _OuWarehousetService.UpdateOuWarehouset(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除出库药品详情
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "ouwarehouset:delete")]
        [Log(Title = "出库药品详情", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteOuWarehouset([FromRoute]string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);

            return ToResponse(_OuWarehousetService.Delete(idArr));
        }

        /// <summary>
        /// 导出出库药品详情
        /// </summary>
        /// <returns></returns>
        [Log(Title = "出库药品详情", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "ouwarehouset:export")]
        public IActionResult Export([FromQuery] OuWarehousetQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _OuWarehousetService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "出库药品详情", "出库药品详情");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 清空出库药品详情
        /// </summary>
        /// <returns></returns>
        [Log(Title = "出库药品详情", BusinessType = BusinessType.CLEAN)]
        [ActionPermissionFilter(Permission = "ouwarehouset:delete")]
        [HttpDelete("clean")]
        public IActionResult Clear()
        {
            if (!HttpContextExtension.IsAdmin(HttpContext))
            {
                return ToResponse(ResultCode.FAIL, "操作失败");
            }
            return SUCCESS(_OuWarehousetService.TruncateOuWarehouset());
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "出库药品详情导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false)]
        [ActionPermissionFilter(Permission = "ouwarehouset:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<OuWarehousetDto> list = new();
            using (var stream = formFile.OpenReadStream())
            {
                list = stream.Query<OuWarehousetDto>(startCell: "A1").ToList();
            }

            return SUCCESS(_OuWarehousetService.ImportOuWarehouset(list.Adapt<List<OuWarehouset>>()));
        }

        /// <summary>
        /// 出库药品详情导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "出库药品详情模板", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            var result = DownloadImportTemplate(new List<OuWarehousetDto>() { }, "OuWarehouset");
            return ExportExcel(result.Item2, result.Item1);
        }

    }
}