using Microsoft.AspNetCore.Mvc;
using ZR.Model.Business;
using ZR.Admin.WebApi.Filters;
using MiniExcelLibs;
using ZR.Model.GuiHis;
using ZR.Model.GuiHis.Dto;
using ZR.Service.Guiz.IGuizService;

//创建时间：2024-11-27
namespace ZR.Admin.WebApi.Controllers.Gui
{
    /// <summary>
    /// 厂家和供应商
    /// </summary>
    [Verify]
    [Route("guiz/CompanyInfo")]
    public class CompanyInfoController : BaseController
    {
        /// <summary>
        /// 厂家和供应商接口
        /// </summary>
        private readonly ICompanyInfoService _CompanyInfoService;

        public CompanyInfoController(ICompanyInfoService CompanyInfoService)
        {
            _CompanyInfoService = CompanyInfoService;
        }

        /// <summary>
        /// 查询厂家和供应商列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "companyinfo:list")]
        public IActionResult QueryCompanyInfo([FromQuery] CompanyInfoQueryDto parm)
        {
            var response = _CompanyInfoService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询厂家和供应商详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "companyinfo:query")]
        public IActionResult GetCompanyInfo(int Id)
        {
            var response = _CompanyInfoService.GetInfo(Id);

            var info = response.Adapt<CompanyInfoDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加厂家和供应商
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "companyinfo:add")]
        [Log(Title = "厂家和供应商", BusinessType = BusinessType.INSERT)]
        public IActionResult AddCompanyInfo([FromBody] CompanyInfoDto parm)
        {
            var modal = parm.Adapt<CompanyInfo>().ToCreate(HttpContext);

            var response = _CompanyInfoService.AddCompanyInfo(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新厂家和供应商
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "companyinfo:edit")]
        [Log(Title = "厂家和供应商", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateCompanyInfo([FromBody] CompanyInfoDto parm)
        {
            var modal = parm.Adapt<CompanyInfo>().ToUpdate(HttpContext);
            var response = _CompanyInfoService.UpdateCompanyInfo(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除厂家和供应商
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "companyinfo:delete")]
        [Log(Title = "厂家和供应商", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteCompanyInfo([FromRoute] string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);

            return ToResponse(_CompanyInfoService.Delete(idArr));
        }

        /// <summary>
        /// 导出厂家和供应商
        /// </summary>
        /// <returns></returns>
        [Log(Title = "厂家和供应商", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "companyinfo:export")]
        public IActionResult Export([FromQuery] CompanyInfoQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _CompanyInfoService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "厂家和供应商", "厂家和供应商");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 清空厂家和供应商
        /// </summary>
        /// <returns></returns>
        [Log(Title = "厂家和供应商", BusinessType = BusinessType.CLEAN)]
        [ActionPermissionFilter(Permission = "companyinfo:delete")]
        [HttpDelete("clean")]
        public IActionResult Clear()
        {
            if (!HttpContext.IsAdmin())
            {
                return ToResponse(ResultCode.FAIL, "操作失败");
            }
            return SUCCESS(_CompanyInfoService.TruncateCompanyInfo());
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "厂家和供应商导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false)]
        [ActionPermissionFilter(Permission = "companyinfo:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<CompanyInfoDto> list = new();
            using (var stream = formFile.OpenReadStream())
            {
                list = stream.Query<CompanyInfoDto>(startCell: "A1").ToList();
            }

            return SUCCESS(_CompanyInfoService.ImportCompanyInfo(list.Adapt<List<CompanyInfo>>()));
        }

        /// <summary>
        /// 厂家和供应商导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "厂家和供应商模板", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            var result = DownloadImportTemplate(new List<CompanyInfoDto>() { }, "CompanyInfo");
            return ExportExcel(result.Item2, result.Item1);
        }

    }
}