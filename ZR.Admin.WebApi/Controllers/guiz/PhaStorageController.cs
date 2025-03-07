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
using MySqlConnector;
using System.Net.Http;
using Org.BouncyCastle.Asn1.Mozilla;
using System.Threading.Tasks;

//创建时间：2024-11-27
namespace ZR.Admin.WebApi.Controllers.Gui
{
    /// <summary>
    /// 库存
    /// </summary>
    [AllowAnonymous]
    [Route("business/PhaStorage")]
    public class PhaStorageController : BaseController
    {
        /// <summary>
        /// 库存接口
        /// </summary>
        private readonly IPhaStorageService _PhaStorageService;
        private readonly IDrugStoreService _DrugStoreService;
        private readonly IDepartmentsService _DepartmentsService;

        private readonly ILogger<PhaStorageController> _logger;

        public PhaStorageController(ILogger<PhaStorageController> logger, IPhaStorageService PhaStorageService, IDrugStoreService drugStoreService, IDepartmentsService departmentsService)
        {
            _PhaStorageService = PhaStorageService;
            _DrugStoreService = drugStoreService;
            _logger = logger;
            _DepartmentsService = departmentsService;
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
            var list = _PhaStorageService.exGetList(parm).Result;
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







        [HttpPost("TongBu")]
        public async Task<IActionResult> TongBu()
        {
            try
            {
                _PhaStorageService.TruncatePhaStoragesss();
                var departments = _DepartmentsService.GetAll();
                int num = 0;
                List<PhaStorage> list = new();
                foreach (var item in departments)
                {
                    int drugDeptCode;
                    if (!int.TryParse(item.DeptCode?.Trim(), out drugDeptCode))
                    {
                        continue;
                    }
                    reqPhaStorage x = await SendRequestsAsync(drugDeptCode);
                    if (x == null || x.data == null)
                    {
                        continue;
                    }
                    else
                    {
                        list.AddRange(x.data);
                    }
                }
                if (list.Count > 0) 
                {
                    num=_PhaStorageService.MIXAddPhaStorage(list);
                 }
                return SUCCESS("true");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in TongBu method.");
                return StatusCode(500, ex);
            }
        }

        private async Task<reqPhaStorage> SendRequestsAsync(int PhaStorageInQuery)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(60);
                // 构建 URL，包括查询参数
                string url = $"http://192.168.2.21:9403/His/GetPhaStorage?drugDeptCode={PhaStorageInQuery}";
                // 将对象序列化为 JSON
                var json = JsonConvert.SerializeObject(null);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                // 发送 POST 请求
                HttpResponseMessage response = await client.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent); // 输出响应内容
                if (response.IsSuccessStatusCode)
                {             
                    return JsonConvert.DeserializeObject<reqPhaStorage>(responseContent);
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode}, Message: {response.ReasonPhrase}, Response: {responseContent}");
                }
            }           
        }

        [HttpGet("allprice")]
        public IActionResult allprice([FromQuery]string code)
        {
            //reqAllprice reqAllprice = new reqAllprice();            
            if (string.IsNullOrEmpty(code))
            {
                return SUCCESS(0);
            }
            var list= _PhaStorageService.GetALLme(code);
 

            return SUCCESS(list);


        }
        public class reqAllprice
        {
            public decimal? Buyprice;
            public decimal? OutPrice;
        }


    }
}