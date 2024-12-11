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
    /// 药房
    /// </summary>
    [AllowAnonymous]

    [Route("business/DrugStore")]
    public class DrugStoreController : BaseController
    {
        /// <summary>
        /// 药房接口
        /// </summary>
        private readonly IDrugStoreService _DrugStoreService;

        public DrugStoreController(IDrugStoreService DrugStoreService)
        {
            _DrugStoreService = DrugStoreService;
        }

        /// <summary>
        /// 查询药房列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "drugStore:list")]
        public IActionResult QueryDrugStore([FromQuery] DrugStoreQueryDto parm)
        {
            var response = _DrugStoreService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询药房详情
        /// </summary>
        /// <param name="DrugDeptCode"></param>
        /// <returns></returns>
        [HttpGet("{DrugDeptCode}")]
        [ActionPermissionFilter(Permission = "drugStore:query")]
        public IActionResult GetDrugStore(string DrugDeptCode)
        {
            var response = _DrugStoreService.GetInfo(DrugDeptCode);

            var info = response.Adapt<DrugStoreDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加药房
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "drugStore:add")]
        [Log(Title = "药房", BusinessType = BusinessType.INSERT)]
        public IActionResult AddDrugStore([FromBody] DrugStoreDto parm)
        {
            var modal = parm.Adapt<DrugStore>().ToCreate(HttpContext);

            var response = _DrugStoreService.AddDrugStore(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新药房
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "drugStore:edit")]
        [Log(Title = "药房", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateDrugStore([FromBody] DrugStoreDto parm)
        {
            var modal = parm.Adapt<DrugStore>().ToUpdate(HttpContext);
            var response = _DrugStoreService.UpdateDrugStore(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除药房
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "drugStore:delete")]
        [Log(Title = "药房", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteDrugStore([FromRoute] string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);

            return ToResponse(_DrugStoreService.Delete(idArr));
        }

        /// <summary>
        /// 导出药房
        /// </summary>
        /// <returns></returns>
        [Log(Title = "药房", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "drugStore:export")]
        public IActionResult Export([FromQuery] DrugStoreQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _DrugStoreService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "药房", "药房");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 清空药房
        /// </summary>
        /// <returns></returns>
        [Log(Title = "药房", BusinessType = BusinessType.CLEAN)]
        [ActionPermissionFilter(Permission = "drugStore:delete")]
        [HttpDelete("clean")]
        public IActionResult Clear()
        {
            if (!HttpContext.IsAdmin())
            {
                return ToResponse(ResultCode.FAIL, "操作失败");
            }
            return SUCCESS(_DrugStoreService.TruncateDrugStore());
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "药房导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false)]
        [ActionPermissionFilter(Permission = "drugStore:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<DrugStoreDto> list = new();
            using (var stream = formFile.OpenReadStream())
            {
                list = stream.Query<DrugStoreDto>(startCell: "A1").ToList();
            }

            return SUCCESS(_DrugStoreService.ImportDrugStore(list.Adapt<List<DrugStore>>()));
        }

        /// <summary>
        /// 药房导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "药房模板", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            var result = DownloadImportTemplate(new List<DrugStoreDto>() { }, "DrugStore");
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
                var x = await SendRequestsAsync();
                foreach (var item in x.Data)
                {
                    var nu = _DrugStoreService.GetInfo(item.DrugDeptCode);
                    if (nu != null)
                    {
                        //进行修改
                        DrugStore DrugStore = new DrugStore();
                        DrugStore.DrugDeptCode = item.DrugDeptCode;
                        DrugStore.DrugDeptName = item.DrugDeptName;
                        DrugStore.StoreSum = item.StoreSum;
                        DrugStore.PreoutSum = item.PreoutSum;
                        _DrugStoreService.UpdateDrugStore(DrugStore);
                    }
                    else if (nu == null)
                    {
                        DrugStore DrugStore = new DrugStore();
                        DrugStore.DrugDeptCode = item.DrugDeptCode;
                        DrugStore.DrugDeptName = item.DrugDeptName;
                        DrugStore.StoreSum = item.StoreSum;
                        DrugStore.PreoutSum = item.PreoutSum;
                        _DrugStoreService.AddDrugStore(DrugStore);
                    }
                }

                return SUCCESS("true");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<ApiDrugStore> SendRequestsAsync()
        {
            using (var client = new HttpClient())
            {
                string url = "http://192.168.1.95:7801/roc/order-service/api/v1/order/order-term/drugstore/queryDrugStoreList";
                var json = JsonConvert.SerializeObject(null);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                // 获取响应内容
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    // 解析 JSON 响应
                    var apiResponse = JsonConvert.DeserializeObject<ApiDrugStore>(responseContent);
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