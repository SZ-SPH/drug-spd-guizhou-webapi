using Microsoft.AspNetCore.Mvc;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Service.Business.IBusinessService;
using ZR.Admin.WebApi.Filters;
using MiniExcelLibs;

//创建时间：2024-12-23
namespace ZR.Admin.WebApi.Controllers.Business
{
    /// <summary>
    /// 货位药品
    /// </summary>
    [Verify]
    [Route("business/LocationDrug")]
    public class LocationDrugController : BaseController
    {
        /// <summary>
        /// 货位药品接口
        /// </summary>
        private readonly ILocationDrugService _LocationDrugService;

        public LocationDrugController(ILocationDrugService LocationDrugService)
        {
            _LocationDrugService = LocationDrugService;
        }

        /// <summary>
        /// 查询货位药品列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "locationdrug:list")]
        public IActionResult QueryLocationDrug([FromQuery] LocationDrugQueryDto parm)
        {
            var response = _LocationDrugService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询货位药品详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "locationdrug:query")]
        public IActionResult GetLocationDrug(int Id)
        {
            var response = _LocationDrugService.GetInfo(Id);
            
            var info = response.Adapt<LocationDrugDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加货位药品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "locationdrug:add")]
        [Log(Title = "货位药品", BusinessType = BusinessType.INSERT)]
        public IActionResult AddLocationDrug([FromBody] LocationDrugDto parm)
        {
            var modal = parm.Adapt<LocationDrug>().ToCreate(HttpContext);

            var response = _LocationDrugService.AddLocationDrug(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新货位药品
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "locationdrug:edit")]
        [Log(Title = "货位药品", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateLocationDrug([FromBody] LocationDrugDto parm)
        {
            var modal = parm.Adapt<LocationDrug>().ToUpdate(HttpContext);
            var response = _LocationDrugService.UpdateLocationDrug(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除货位药品
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "locationdrug:delete")]
        [Log(Title = "货位药品", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteLocationDrug([FromRoute]string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);

            return ToResponse(_LocationDrugService.Delete(idArr));
        }

        /// <summary>
        /// 导出货位药品
        /// </summary>
        /// <returns></returns>
        [Log(Title = "货位药品", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "locationdrug:export")]
        public IActionResult Export([FromQuery] LocationDrugQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _LocationDrugService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "货位药品", "货位药品");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 清空货位药品
        /// </summary>
        /// <returns></returns>
        [Log(Title = "货位药品", BusinessType = BusinessType.CLEAN)]
        [ActionPermissionFilter(Permission = "locationdrug:delete")]
        [HttpDelete("clean")]
        public IActionResult Clear()
        {
            if (!HttpContextExtension.IsAdmin(HttpContext))
            {
                return ToResponse(ResultCode.FAIL, "操作失败");
            }
            return SUCCESS(_LocationDrugService.TruncateLocationDrug());
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "货位药品导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false)]
        [ActionPermissionFilter(Permission = "locationdrug:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<LocationDrugDto> list = new();
            using (var stream = formFile.OpenReadStream())
            {
                list = stream.Query<LocationDrugDto>(startCell: "A1").ToList();
            }

            return SUCCESS(_LocationDrugService.ImportLocationDrug(list.Adapt<List<LocationDrug>>()));
        }

        /// <summary>
        /// 货位药品导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "货位药品模板", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            var result = DownloadImportTemplate(new List<LocationDrugDto>() { }, "LocationDrug");
            return ExportExcel(result.Item2, result.Item1);
        }

    }
}