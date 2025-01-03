using Microsoft.AspNetCore.Mvc;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Service.Business.IBusinessService;
using ZR.Admin.WebApi.Filters;
using MiniExcelLibs;
using ZR.Service.Business;
using Newtonsoft.Json;
using System.Text;
using Org.BouncyCastle.Asn1.Ocsp;

//创建时间：2024-09-14
namespace ZR.Admin.WebApi.Controllers.Business
{
    /// <summary>
    /// 医嘱
    /// </summary>
    [Verify]
    [Route("business/MedicalAdvice")]
    public class MedicalAdviceController : BaseController
    {
        /// <summary>
        /// 医嘱接口
        /// </summary>
        private readonly IMedicalAdviceService _MedicalAdviceService;
        private readonly ICodeDetailsService _CodeDetailsService;


        public MedicalAdviceController(IMedicalAdviceService MedicalAdviceService,
            ICodeDetailsService CodeDetailsService)
        {
            _MedicalAdviceService = MedicalAdviceService;
            _CodeDetailsService = CodeDetailsService;
        }


        /// <summary>
        /// 查询医嘱信息列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "medicaladvice:list")]
        public IActionResult QueryMedicalAdvice([FromQuery] MedicalAdviceQueryDto parm)
        {
            var response = _MedicalAdviceService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询医嘱信息详情
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        [HttpGet("{OrderId}")]
        [ActionPermissionFilter(Permission = "medicaladvice:query")]
        public IActionResult GetMedicalAdvice(int OrderId)
        {
            var response = _MedicalAdviceService.GetInfo(OrderId);

            var info = response.Adapt<MedicalAdviceDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加医嘱信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "medicaladvice:add")]
        [Log(Title = "医嘱信息", BusinessType = BusinessType.INSERT)]
        public IActionResult AddMedicalAdvice([FromBody] MedicalAdviceDto parm)
        {
            var modal = parm.Adapt<MedicalAdvice>().ToCreate(HttpContext);

            var response = _MedicalAdviceService.AddMedicalAdvice(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新医嘱信息
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "medicaladvice:edit")]
        [Log(Title = "医嘱信息", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateMedicalAdvice([FromBody] MedicalAdviceDto parm)
        {
            var modal = parm.Adapt<MedicalAdvice>().ToUpdate(HttpContext);
            var response = _MedicalAdviceService.UpdateMedicalAdvice(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除医嘱信息
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "medicaladvice:delete")]
        [Log(Title = "医嘱信息", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteMedicalAdvice([FromRoute] string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);

            return ToResponse(_MedicalAdviceService.Delete(idArr));
        }

        /// <summary>
        /// 导出医嘱信息
        /// </summary>
        /// <returns></returns>
        [Log(Title = "医嘱信息", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "medicaladvice:export")]
        public IActionResult Export([FromQuery] MedicalAdviceQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _MedicalAdviceService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "医嘱信息", "医嘱信息");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 清空医嘱信息
        /// </summary>
        /// <returns></returns>
        [Log(Title = "医嘱信息", BusinessType = BusinessType.CLEAN)]
        [ActionPermissionFilter(Permission = "medicaladvice:delete")]
        [HttpDelete("clean")]
        public IActionResult Clear()
        {
            if (!HttpContextExtension.IsAdmin(HttpContext))
            {
                return ToResponse(ResultCode.FAIL, "操作失败");
            }
            return SUCCESS(_MedicalAdviceService.TruncateMedicalAdvice());
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "医嘱信息导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false)]
        [ActionPermissionFilter(Permission = "medicaladvice:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<MedicalAdviceDto> list = new();
            using (var stream = formFile.OpenReadStream())
            {
                list = stream.Query<MedicalAdviceDto>(startCell: "A1").ToList();
            }

            return SUCCESS(_MedicalAdviceService.ImportMedicalAdvice(list.Adapt<List<MedicalAdvice>>()));
        }

        /// <summary>
        /// 医嘱信息导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "医嘱信息模板", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            var result = DownloadImportTemplate(new List<MedicalAdviceDto>() { }, "MedicalAdvice");
            return ExportExcel(result.Item2, result.Item1);
        }

  

        /// <summary>
        /// PDA根据HIS医嘱查询医嘱列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("PDAList")]
        [ActionPermissionFilter(Permission = "medicaladvice:list")]
        public IActionResult PdaQueryMedicalAdviceByHisId([FromQuery] MedicalAdviceQueryDto parm)
        {
            var response = _MedicalAdviceService.PdaQueryMedicalAdviceByHisId(parm);
            return SUCCESS(response);
        }







        /// <summary>
        /// 医嘱回补追溯码
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddRequestZsm")]
        [ActionPermissionFilter(Permission = "warehousereceipt:add")]
        [Log(Title = "医嘱", BusinessType = BusinessType.INSERT)]
        public async Task<IActionResult> AddRequestZsmAsync([FromBody] Med parmlist)
        {
            //获取到医嘱 从 code 表查询 相关的医嘱

            List<ZsmItem> requests = new List<ZsmItem>();
            RequestPayload list = new();

            //查询
            var response = _CodeDetailsService.outGetList(parmlist.Id);

            for (int i = 0; i < response.Count; i++)
            {
                ZsmItem zsm = new ZsmItem();
                zsm.Zsm = response[i].Code;
                requests.Add(zsm);
            }
            list.Fymx_Id = parmlist.Id.ToString();
            list.Zsm_List = requests;

            string result = await AddRequestZsmAsync(list);


            return SUCCESS("result");


        }

        private async Task<string> AddRequestZsmAsync(RequestPayload requests)
        {
            using (HttpClient client = new HttpClient())
            {
                // 设置请求的URL
                string url = "http://127.0.0.1:8080/xtHisService/xyxtSendAction!addRecordZxm.do";

                // 将 requests 转换为 JSON 字符串
                string json = JsonConvert.SerializeObject(requests); // 或者使用 System.Text.Json.JsonSerializer.Serialize(requests)

                // 创建 HttpContent
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // 发送 POST 请求
                HttpResponseMessage response = await client.PostAsync(url, content);

                // 检查响应
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // 处理错误
                    throw new Exception($"Error: {response.StatusCode}, Message: {response.ReasonPhrase}");
                }
            }
        }
    }
}