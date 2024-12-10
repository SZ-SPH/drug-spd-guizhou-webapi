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
    /// 库存
    /// </summary>
    [Verify]
    [Route("business/PhaStorage")]
    public class PhaStorageController : BaseController
    {
        /// <summary>
        /// 库存接口
        /// </summary>
        private readonly IPhaStorageService _PhaStorageService;
        private readonly IDrugStoreService _DrugStoreService;


        public PhaStorageController(IPhaStorageService PhaStorageService, IDrugStoreService drugStoreService)
        {
            _PhaStorageService = PhaStorageService;
            _DrugStoreService = drugStoreService;
        }

        /// <summary>
        /// 查询库存列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "phastorage:list")]
        public IActionResult QueryPhaStorage([FromQuery] PhaStorageQueryDto parm)
        {
            var response = _PhaStorageService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询库存详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "phastorage:query")]
        public IActionResult GetPhaStorage(int Id)
        {
            var response = _PhaStorageService.GetInfo(Id);

            var info = response.Adapt<PhaStorageDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加库存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "phastorage:add")]
        [Log(Title = "库存", BusinessType = BusinessType.INSERT)]
        public IActionResult AddPhaStorage([FromBody] PhaStorageDto parm)
        {
            var modal = parm.Adapt<PhaStorage>().ToCreate(HttpContext);

            var response = _PhaStorageService.AddPhaStorage(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新库存
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "phastorage:edit")]
        [Log(Title = "库存", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdatePhaStorage([FromBody] PhaStorageDto parm)
        {
            var modal = parm.Adapt<PhaStorage>().ToUpdate(HttpContext);
            var response = _PhaStorageService.UpdatePhaStorage(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除库存
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "phastorage:delete")]
        [Log(Title = "库存", BusinessType = BusinessType.DELETE)]
        public IActionResult DeletePhaStorage([FromRoute] string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);

            return ToResponse(_PhaStorageService.Delete(idArr));
        }

        /// <summary>
        /// 导出库存
        /// </summary>
        /// <returns></returns>
        [Log(Title = "库存", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "phastorage:export")]
        public IActionResult Export([FromQuery] PhaStorageQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _PhaStorageService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "库存", "库存");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 清空库存
        /// </summary>
        /// <returns></returns>
        [Log(Title = "库存", BusinessType = BusinessType.CLEAN)]
        [ActionPermissionFilter(Permission = "phastorage:delete")]
        [HttpDelete("clean")]
        public IActionResult Clear()
        {
            if (!HttpContext.IsAdmin())
            {
                return ToResponse(ResultCode.FAIL, "操作失败");
            }
            return SUCCESS(_PhaStorageService.TruncatePhaStorage());
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "库存导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false)]
        [ActionPermissionFilter(Permission = "phastorage:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<PhaStorageDto> list = new();
            using (var stream = formFile.OpenReadStream())
            {
                list = stream.Query<PhaStorageDto>(startCell: "A1").ToList();
            }

            return SUCCESS(_PhaStorageService.ImportPhaStorage(list.Adapt<List<PhaStorage>>()));
        }

        /// <summary>
        /// 库存导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "库存模板", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            var result = DownloadImportTemplate(new List<PhaStorageDto>() { }, "PhaStorage");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        //http://192.168.2.21:9403/His/GetPhaStorage?drugDeptCode=6052
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("TongBu")]
        public async Task<IActionResult> TongBu()
        {
            try
            {
                var dep= _DrugStoreService.GetAll();
                foreach (var item in dep)
                {
                    PhaStorageInQuery phaStorageInQuery = new PhaStorageInQuery();
                    phaStorageInQuery.DrugDeptCode = item.DrugDeptCode;
                    reqPhaStorage x = await SendRequestsAsync(phaStorageInQuery);
                    foreach (var items in x.Data)
                    {
                        var nu = _PhaStorageService.GetisInfo(items.DrugCode,item.DrugDeptCode);
                            if (nu != null)
                            {
                                var modal = nu.Adapt<PhaStorage>().ToUpdate(HttpContext);
                                _PhaStorageService.UpdatePhaStorage(modal);
                            }
                            else if (nu == null)
                            {
                                var modal = nu.Adapt<PhaStorage>().ToCreate(HttpContext);
                                 _PhaStorageService.AddPhaStorage(modal);
                            }
                    }

                }
                return SUCCESS("true");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<reqPhaStorage> SendRequestsAsync(PhaStorageInQuery PhaStorageInQuery)
        {
            using (var client = new HttpClient())
            {
                string url = "http://192.168.2.21:9403/His/GetPhaStorage";
                var json = JsonConvert.SerializeObject(PhaStorageInQuery);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                // 获取响应内容
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    // 解析 JSON 响应
                    var apiResponse = JsonConvert.DeserializeObject<reqPhaStorage>(responseContent);
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