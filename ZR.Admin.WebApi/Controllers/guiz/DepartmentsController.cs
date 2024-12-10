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
    /// 科室
    /// </summary>
    [Verify]
    [Route("business/Departments")]
    public class DepartmentsController : BaseController
    {
        /// <summary>
        /// 科室接口
        /// </summary>
        private readonly IDepartmentsService _DepartmentsService;

        public DepartmentsController(IDepartmentsService DepartmentsService)
        {
            _DepartmentsService = DepartmentsService;
        }

        /// <summary>
        /// 查询科室列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "departments:list")]
        public IActionResult QueryDepartments([FromQuery] DepartmentsQueryDto parm)
        {
            var response = _DepartmentsService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询科室详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "departments:query")]
        public IActionResult GetDepartments(string DeptCode)
        {
            var response = _DepartmentsService.GetInfo(DeptCode);

            var info = response.Adapt<DepartmentsDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加科室
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "departments:add")]
        [Log(Title = "科室", BusinessType = BusinessType.INSERT)]
        public IActionResult AddDepartments([FromBody] DepartmentsDto parm)
        {
            var modal = parm.Adapt<Departments>().ToCreate(HttpContext);

            var response = _DepartmentsService.AddDepartments(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新科室
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "departments:edit")]
        [Log(Title = "科室", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateDepartments([FromBody] DepartmentsDto parm)
        {
            var modal = parm.Adapt<Departments>().ToUpdate(HttpContext);
            var response = _DepartmentsService.UpdateDepartments(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除科室
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "departments:delete")]
        [Log(Title = "科室", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteDepartments([FromRoute] string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);

            return ToResponse(_DepartmentsService.Delete(idArr));
        }

        /// <summary>
        /// 导出科室
        /// </summary>
        /// <returns></returns>
        [Log(Title = "科室", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "departments:export")]
        public IActionResult Export([FromQuery] DepartmentsQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _DepartmentsService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "科室", "科室");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 清空科室
        /// </summary>
        /// <returns></returns>
        [Log(Title = "科室", BusinessType = BusinessType.CLEAN)]
        [ActionPermissionFilter(Permission = "departments:delete")]
        [HttpDelete("clean")]
        public IActionResult Clear()
        {
            if (!HttpContext.IsAdmin())
            {
                return ToResponse(ResultCode.FAIL, "操作失败");
            }
            return SUCCESS(_DepartmentsService.TruncateDepartments());
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "科室导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false)]
        [ActionPermissionFilter(Permission = "departments:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<DepartmentsDto> list = new();
            using (var stream = formFile.OpenReadStream())
            {
                list = stream.Query<DepartmentsDto>(startCell: "A1").ToList();
            }

            return SUCCESS(_DepartmentsService.ImportDepartments(list.Adapt<List<Departments>>()));
        }

        /// <summary>
        /// 科室导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "科室模板", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            var result = DownloadImportTemplate(new List<DepartmentsDto>() { }, "Departments");
            return ExportExcel(result.Item2, result.Item1);
        }




        /// <summary>
        /// 同步 http://192.168.1.95:7801/roc/curr-web/api/v1/common/dept/query?deptCode=&deptType=
        /// </summary>
        /// <param name="parmlist"></param>
        /// <returns></returns>
        [HttpGet("TongBu")]
        public async Task<IActionResult> TongBu()
        {
            try
            {
                DepartmentsQuery departmentsQuery = new DepartmentsQuery();
                departmentsQuery.DeptCode = null;
                departmentsQuery.DeptType = null;
                var x = await SendRequestsAsync(departmentsQuery);
                foreach (var item in x.Data)
                {
                    var nu = _DepartmentsService.GetInfo(item.DeptCode);
                    if (nu != null)
                    {
                        //进行修改
                        Departments departments = new Departments();
                        departments.DeptCode = item.DeptCode;
                        departments.DeptEname = item.DeptEname;
                        departments.DeptName = item.DeptName;
                        departments.SpellCode = item.SpellCode;
                        departments.WbCode = item.WbCode;
                        _DepartmentsService.UpdateDepartments(departments);
                    }
                    else if (nu == null)
                    {
                        Departments departments = new Departments();
                        departments.DeptCode = item.DeptCode;
                        departments.DeptEname = item.DeptEname;
                        departments.DeptName = item.DeptName;
                        departments.SpellCode = item.SpellCode;
                        departments.WbCode = item.WbCode;
                        _DepartmentsService.AddDepartments(departments);
                    }
                }

                return SUCCESS("true");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<QueryResponse> SendRequestsAsync(DepartmentsQuery requests)
        {
            using (var client = new HttpClient())
            {
                string url = "http://192.168.1.95:7801/roc/curr-web/api/v1/common/dept/query";
                var json = JsonConvert.SerializeObject(requests);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                // 获取响应内容
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    // 解析 JSON 响应
                    var apiResponse = JsonConvert.DeserializeObject<QueryResponse>(responseContent);
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