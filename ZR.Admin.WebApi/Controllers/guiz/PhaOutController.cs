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
    /// 出库记录
    /// </summary>
    [Verify]
    [Route("business/PhaOut")]
    public class PhaOutController : BaseController
    {
        /// <summary>
        /// 出库记录接口
        /// </summary>
        private readonly IPhaOutService _PhaOutService;

        public PhaOutController(IPhaOutService PhaOutService)
        {
            _PhaOutService = PhaOutService;
        }

        /// <summary>
        /// 查询出库记录列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "phaout:list")]
        public IActionResult QueryPhaOut([FromQuery] PhaOutQueryDto parm)
        {
            var response = _PhaOutService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询出库记录详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "phaout:query")]
        public IActionResult GetPhaOut(int Id)
        {
            var response = _PhaOutService.GetInfo(Id);

            var info = response.Adapt<PhaOutDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加出库记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "phaout:add")]
        [Log(Title = "出库记录", BusinessType = BusinessType.INSERT)]
        public IActionResult AddPhaOut([FromBody] PhaOutDto parm)
        {
            var modal = parm.Adapt<PhaOut>().ToCreate(HttpContext);

            var response = _PhaOutService.AddPhaOut(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新出库记录
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "phaout:edit")]
        [Log(Title = "出库记录", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdatePhaOut([FromBody] PhaOutDto parm)
        {
            var modal = parm.Adapt<PhaOut>().ToUpdate(HttpContext);
            var response = _PhaOutService.UpdatePhaOut(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除出库记录
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "phaout:delete")]
        [Log(Title = "出库记录", BusinessType = BusinessType.DELETE)]
        public IActionResult DeletePhaOut([FromRoute] string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);

            return ToResponse(_PhaOutService.Delete(idArr));
        }

        /// <summary>
        /// 导出出库记录
        /// </summary>
        /// <returns></returns>
        [Log(Title = "出库记录", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "phaout:export")]
        public IActionResult Export([FromQuery] PhaOutQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _PhaOutService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "出库记录", "出库记录");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 清空出库记录
        /// </summary>
        /// <returns></returns>
        [Log(Title = "出库记录", BusinessType = BusinessType.CLEAN)]
        [ActionPermissionFilter(Permission = "phaout:delete")]
        [HttpDelete("clean")]
        public IActionResult Clear()
        {
            if (!HttpContext.IsAdmin())
            {
                return ToResponse(ResultCode.FAIL, "操作失败");
            }
            return SUCCESS(_PhaOutService.TruncatePhaOut());
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "出库记录导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false)]
        [ActionPermissionFilter(Permission = "phaout:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<PhaOutDto> list = new();
            using (var stream = formFile.OpenReadStream())
            {
                list = stream.Query<PhaOutDto>(startCell: "A1").ToList();
            }

            return SUCCESS(_PhaOutService.ImportPhaOut(list.Adapt<List<PhaOut>>()));
        }

        /// <summary>
        /// 出库记录导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "出库记录模板", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            var result = DownloadImportTemplate(new List<PhaOutDto>() { }, "PhaOut");
            return ExportExcel(result.Item2, result.Item1);
        }

    }
}