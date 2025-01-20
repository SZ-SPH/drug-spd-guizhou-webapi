using Microsoft.AspNetCore.Mvc;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Service.Business.IBusinessService;
using ZR.Admin.WebApi.Filters;
using MiniExcelLibs;

//创建时间：2024-12-30
namespace ZR.Admin.WebApi.Controllers.Business
{
    /// <summary>
    /// 采购退货
    /// </summary>
    [Verify]
    [Route("business/OutDrugs")]
    public class OutDrugsController : BaseController
    {
        /// <summary>
        /// 采购退货接口
        /// </summary>
        private readonly IOutDrugsService _OutDrugsService;

        public OutDrugsController(IOutDrugsService OutDrugsService)
        {
            _OutDrugsService = OutDrugsService;
        }

        /// <summary>
        /// 查询采购退货列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "outdrugs:list")]
        public IActionResult QueryOutDrugs([FromQuery] OutDrugsQueryDto parm)
        {
            var response = _OutDrugsService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询采购退货详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "outdrugs:query")]
        public IActionResult GetOutDrugs(int Id)
        {
            var response = _OutDrugsService.GetInfo(Id);
            
            var info = response.Adapt<OutDrugsDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加采购退货
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "outdrugs:add")]
        [Log(Title = "采购退货", BusinessType = BusinessType.INSERT)]
        public IActionResult AddOutDrugs([FromBody] OutDrugsDto parm)
        {
            var modal = parm.Adapt<OutDrugs>().ToCreate(HttpContext);

            var response = _OutDrugsService.AddOutDrugs(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新采购退货
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "outdrugs:edit")]
        [Log(Title = "采购退货", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateOutDrugs([FromBody] OutDrugsDto parm)
        {
            var modal = parm.Adapt<OutDrugs>().ToUpdate(HttpContext);
            var response = _OutDrugsService.UpdateOutDrugs(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除采购退货
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "outdrugs:delete")]
        [Log(Title = "采购退货", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteOutDrugs([FromRoute]string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);

            return ToResponse(_OutDrugsService.Delete(idArr));
        }

        /// <summary>
        /// 导出采购退货
        /// </summary>
        /// <returns></returns>
        [Log(Title = "采购退货", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "outdrugs:export")]
        public IActionResult Export([FromQuery] OutDrugsQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _OutDrugsService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "采购退货", "采购退货");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 清空采购退货
        /// </summary>
        /// <returns></returns>
        [Log(Title = "采购退货", BusinessType = BusinessType.CLEAN)]
        [ActionPermissionFilter(Permission = "outdrugs:delete")]
        [HttpDelete("clean")]
        public IActionResult Clear()
        {
            if (!HttpContextExtension.IsAdmin(HttpContext))
            {
                return ToResponse(ResultCode.FAIL, "操作失败");
            }
            return SUCCESS(_OutDrugsService.TruncateOutDrugs());
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "采购退货导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false)]
        [ActionPermissionFilter(Permission = "outdrugs:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<OutDrugsDto> list = new();
            using (var stream = formFile.OpenReadStream())
            {
                list = stream.Query<OutDrugsDto>(startCell: "A1").ToList();
            }

            return SUCCESS(_OutDrugsService.ImportOutDrugs(list.Adapt<List<OutDrugs>>()));
        }

        /// <summary>
        /// 采购退货导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "采购退货模板", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            var result = DownloadImportTemplate(new List<OutDrugsDto>() { }, "OutDrugs");
            return ExportExcel(result.Item2, result.Item1);
        }

    }
}