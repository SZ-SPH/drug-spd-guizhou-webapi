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
    /// 厂家和供应商
    /// </summary>
    //[Verify]
    [AllowAnonymous]
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
        /// <param name="facCode"></param>
        /// <returns></returns>
        [HttpGet("{facCode}")]
        [ActionPermissionFilter(Permission = "companyinfo:query")]
        public IActionResult GetCompanyInfo(string facCode)
        {
            var response = _CompanyInfoService.GetInfo(facCode);

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


        //http://192.168.1.95:7801/roc/curr-web/api/v1/curr/pharmaceutical/company?facCode=2002&compantyType=1&validFlag=0
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
                CompanyInres companyInres = new CompanyInres();
                var x = await SendRequestsAsync(companyInres);
                foreach (var item in x.Data)
                {
                    var nu = _CompanyInfoService.GetInfo(item.FacCode);
                    if (nu != null)
                    {
                        var modal = item.Adapt<CompanyInfo>().ToUpdate(HttpContext);
                        _CompanyInfoService.UpdateCompanyInfo(modal);
                    }
                    else if (nu == null)
                    {
                        var modal = item.Adapt<CompanyInfo>().ToCreate(HttpContext);
                        _CompanyInfoService.AddCompanyInfo(modal);
                    }
                }

                return SUCCESS("true");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<CompanyResponse> SendRequestsAsync(CompanyInres companyInres)
        {
            using (var client = new HttpClient())
            {
                string url = "http://192.168.1.95:7801/roc/curr-web/api/v1/curr/pharmaceutical/company";
                var json = JsonConvert.SerializeObject(companyInres);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                // 获取响应内容
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    // 解析 JSON 响应
                    var apiResponse = JsonConvert.DeserializeObject<CompanyResponse>(responseContent);
                    return apiResponse; // 返回 ApiResponse 对象
                }
                else
                {
                    // 处理错误
                    throw new Exception($"Error: {response.StatusCode}, Message: {response.ReasonPhrase}, Response: {responseContent},Json:{json}");
                }
            }
        }

    }
}