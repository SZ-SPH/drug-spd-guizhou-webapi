using Microsoft.AspNetCore.Mvc;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Service.Business.IBusinessService;
using ZR.Admin.WebApi.Filters;
using MiniExcelLibs;

//创建时间：2025-02-18
namespace ZR.Admin.WebApi.Controllers.Business
{
    /// <summary>
    /// 入库详情
    /// </summary>
    [Verify]
    [Route("business/TInwarehousedetail")]
    public class TInwarehousedetailController : BaseController
    {
        /// <summary>
        /// 入库详情接口
        /// </summary>
        private readonly ITInwarehousedetailService _TInwarehousedetailService;

        public TInwarehousedetailController(ITInwarehousedetailService TInwarehousedetailService)
        {
            _TInwarehousedetailService = TInwarehousedetailService;
        }

        /// <summary>
        /// 查询入库详情列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "tinwarehousedetail:list")]
        public IActionResult QueryTInwarehousedetail([FromQuery] TInwarehousedetailQueryDto parm)
        {
            var response = _TInwarehousedetailService.GetList(parm);
            return SUCCESS(response);
        }
        /// <summary>
        /// 查询入库详情列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("lists")]
        [ActionPermissionFilter(Permission = "tinwarehousedetail:list")]
        public IActionResult QueryTInwarehousedetails([FromQuery] TInwarehousedetailQueryDto parm)
        {
            var response = _TInwarehousedetailService.GetLists(parm);
            return SUCCESS(response);
        }
        
        /// <summary>
        /// 查询入库详情详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "tinwarehousedetail:query")]
        public IActionResult GetTInwarehousedetail(int Id)
        {
            var response = _TInwarehousedetailService.GetInfo(Id);
            
            var info = response.Adapt<TInwarehousedetailDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加入库详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "tinwarehousedetail:add")]
        [Log(Title = "入库详情", BusinessType = BusinessType.INSERT)]
        public IActionResult AddTInwarehousedetail([FromBody] TInwarehousedetailDto parm)
        {
            var modal = parm.Adapt<TInwarehousedetail>().ToCreate(HttpContext);

            var response = _TInwarehousedetailService.AddTInwarehousedetail(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新入库详情
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "tinwarehousedetail:edit")]
        [Log(Title = "入库详情", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateTInwarehousedetail([FromBody] TInwarehousedetailDto parm)
        {
            var modal = parm.Adapt<TInwarehousedetail>().ToUpdate(HttpContext);
            var response = _TInwarehousedetailService.UpdateTInwarehousedetail(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除入库详情
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "tinwarehousedetail:delete")]
        [Log(Title = "入库详情", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteTInwarehousedetail([FromRoute]string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);

            return ToResponse(_TInwarehousedetailService.Delete(idArr));
        }

        /// <summary>
        /// 导出入库详情
        /// </summary>
        /// <returns></returns>
        [Log(Title = "入库详情", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "tinwarehousedetail:export")]
        public IActionResult Export([FromQuery] TInwarehousedetailQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _TInwarehousedetailService.GetLists(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "入库详情", "入库详情");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 清空入库详情
        /// </summary>
        /// <returns></returns>
        [Log(Title = "入库详情", BusinessType = BusinessType.CLEAN)]
        [ActionPermissionFilter(Permission = "tinwarehousedetail:delete")]
        [HttpDelete("clean")]
        public IActionResult Clear()
        {
            if (!HttpContextExtension.IsAdmin(HttpContext))
            {
                return ToResponse(ResultCode.FAIL, "操作失败");
            }
            return SUCCESS(_TInwarehousedetailService.TruncateTInwarehousedetail());
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "入库详情导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false)]
        [ActionPermissionFilter(Permission = "tinwarehousedetail:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<TInwarehousedetailDto> list = new();
            using (var stream = formFile.OpenReadStream())
            {
                list = stream.Query<TInwarehousedetailDto>(startCell: "A1").ToList();
            }

            return SUCCESS(_TInwarehousedetailService.ImportTInwarehousedetail(list.Adapt<List<TInwarehousedetail>>()));
        }

        /// <summary>
        /// 入库详情导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "入库详情模板", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            var result = DownloadImportTemplate(new List<TInwarehousedetailDto>() { }, "TInwarehousedetail");
            return ExportExcel(result.Item2, result.Item1);
        }

    }
}