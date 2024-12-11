using Microsoft.AspNetCore.Mvc;
using ZR.Model.Business;
using ZR.Admin.WebApi.Filters;
using MiniExcelLibs;
using ZR.Model.GuiHis;
using ZR.Model.GuiHis.Dto;
using ZR.Service.Guiz.IGuizService;
using Newtonsoft.Json;
using System.Text;
using ZR.Admin.WebApi.Controllers.Business;
using System.Reflection.Metadata.Ecma335;
using Aliyun.OSS;

//创建时间：2024-11-27
namespace ZR.Admin.WebApi.Controllers.Gui
{
    /// <summary>
    /// 药品
    /// </summary>
    [AllowAnonymous]

    [Route("business/GuiDrug")]
    public class GuiDrugController : BaseController
    {
        /// <summary>
        /// 药品接口
        /// </summary>
        private readonly IGuiDrugService _GuiDrugService;

        public GuiDrugController(IGuiDrugService GuiDrugService)
        {
            _GuiDrugService = GuiDrugService;
        }


        public object GetModel(GuiDrugInQuery guiDrugIn)
        {
            return 's';

        }


        /// <summary>
        /// 查询药品列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "guidrug:list")]
        public IActionResult QueryGuiDrug([FromQuery] GuiDrugQueryDto parm)
        {
            var response = _GuiDrugService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询药品详情
        /// </summary>
        /// <param name="DrugTermId"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "guidrug:query")]
        public IActionResult GetGuiDrug(string DrugTermId)
        {
            var response = _GuiDrugService.GetInfo(DrugTermId);

            var info = response.Adapt<GuiDrugDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加药品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "guidrug:add")]
        [Log(Title = "药品", BusinessType = BusinessType.INSERT)]
        public IActionResult AddGuiDrug([FromBody] GuiDrugDto parm)
        {
            var modal = parm.Adapt<GuiDrug>().ToCreate(HttpContext);

            var response = _GuiDrugService.AddGuiDrug(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新药品
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "guidrug:edit")]
        [Log(Title = "药品", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateGuiDrug([FromBody] GuiDrugDto parm)
        {
            var modal = parm.Adapt<GuiDrug>().ToUpdate(HttpContext);
            var response = _GuiDrugService.UpdateGuiDrug(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除药品
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "guidrug:delete")]
        [Log(Title = "药品", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteGuiDrug([FromRoute] string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);

            return ToResponse(_GuiDrugService.Delete(idArr));
        }

        /// <summary>
        /// 导出药品
        /// </summary>
        /// <returns></returns>
        [Log(Title = "药品", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "guidrug:export")]
        public IActionResult Export([FromQuery] GuiDrugQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _GuiDrugService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "药品", "药品");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 清空药品
        /// </summary>
        /// <returns></returns>
        [Log(Title = "药品", BusinessType = BusinessType.CLEAN)]
        [ActionPermissionFilter(Permission = "guidrug:delete")]
        [HttpDelete("clean")]
        public IActionResult Clear()
        {
            if (!HttpContext.IsAdmin())
            {
                return ToResponse(ResultCode.FAIL, "操作失败");
            }
            return SUCCESS(_GuiDrugService.TruncateGuiDrug());
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "药品导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false)]
        [ActionPermissionFilter(Permission = "guidrug:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<GuiDrugDto> list = new();
            using (var stream = formFile.OpenReadStream())
            {
                list = stream.Query<GuiDrugDto>(startCell: "A1").ToList();
            }

            return SUCCESS(_GuiDrugService.ImportGuiDrug(list.Adapt<List<GuiDrug>>()));
        }

        /// <summary>
        /// 药品导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "药品模板", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            var result = DownloadImportTemplate(new List<GuiDrugDto>() { }, "GuiDrug");
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
           ApiResponse mm =new ApiResponse();
            ApiResponse ff = new ApiResponse();

            try
            {
                GuiDrugInQuery guiDrugInQuery = new GuiDrugInQuery
                {
                    pageFlag = true,
                    pageSize = 10,
                    pageNum = 1
                };
                var x = await SendRequestsAsync(guiDrugInQuery);
                ff = x;
                if (x?.Data?.Total > 0) // 检查 x 和 x.Data 是否为 null
                {
                    guiDrugInQuery.pageSize = (int)x.Data.Total; // 确保 Total 是有效的
                    var y = await SendRequestsAsync(guiDrugInQuery);
                    mm = y;
                    if (y?.Data?.List != null) // 检查 y 和 y.Data.List 是否为 null
                    {
                        foreach (var item in y.Data.List)
                        {
                            var nu = _GuiDrugService.GetInfo(item.DrugTermId);
                            if (nu != null)
                            {
                                // 进行修改
                                var modal = item.Adapt<GuiDrug>().ToUpdate(HttpContext);
                                var response = _GuiDrugService.UpdateGuiDrug(modal);
                            }
                            else if (nu==null)
                            {
                                var modal = item.Adapt<GuiDrug>().ToCreate(HttpContext);
                                var response = _GuiDrugService.AddGuiDrug(modal);
                            }
                        }
                    }
                }
                return SUCCESS("true");
            }
            catch (Exception ex)
            {
                // 记录异常信息
                // 你可以使用日志工具记录异常
                return StatusCode(500, ex.Message); // 返回500错误和异常消息
            }
        }

        private async Task<ApiResponse> SendRequestsAsync(GuiDrugInQuery requests)
        {
            using (var client = new HttpClient())
            {

                string url = $"http://192.168.1.95:7801/roc/order-service/api/v1/order/order-term/drug/query?pageFlag={requests.pageFlag}&pageNum-{requests.pageNum}&pageSize={requests.pageSize}";
                //var json = JsonConvert.SerializeObject(requests);
                //var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.GetAsync(url);
                // 获取响应内容
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    // 解析 JSON 响应
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
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