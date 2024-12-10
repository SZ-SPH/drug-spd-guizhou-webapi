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
    /// 入库详情
    /// </summary>
    [Verify]
    [Route("business/PhaInput")]
    public class PhaInputController : BaseController
    {
        /// <summary>
        /// 入库详情接口
        /// </summary>
        private readonly IPhaInputService _PhaInputService;

        public PhaInputController(IPhaInputService PhaInputService)
        {
            _PhaInputService = PhaInputService;
        }

        /// <summary>
        /// 查询入库详情列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "phainput:list")]
        public IActionResult QueryPhaInput([FromQuery] PhaInputQueryDto parm)
        {
            var response = _PhaInputService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询入库详情详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "phainput:query")]
        public IActionResult GetPhaInput(int Id)
        {
            var response = _PhaInputService.GetInfo(Id);

            var info = response.Adapt<PhaInputDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加入库详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "phainput:add")]
        [Log(Title = "入库详情", BusinessType = BusinessType.INSERT)]
        public IActionResult AddPhaInput([FromBody] PhaInputDto parm)
        {
            var modal = parm.Adapt<PhaInput>().ToCreate(HttpContext);

            var response = _PhaInputService.AddPhaInput(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新入库详情
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "phainput:edit")]
        [Log(Title = "入库详情", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdatePhaInput([FromBody] PhaInputDto parm)
        {
            var modal = parm.Adapt<PhaInput>().ToUpdate(HttpContext);
            var response = _PhaInputService.UpdatePhaInput(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除入库详情
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "phainput:delete")]
        [Log(Title = "入库详情", BusinessType = BusinessType.DELETE)]
        public IActionResult DeletePhaInput([FromRoute] string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);

            return ToResponse(_PhaInputService.Delete(idArr));
        }

        /// <summary>
        /// 导出入库详情
        /// </summary>
        /// <returns></returns>
        [Log(Title = "入库详情", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "phainput:export")]
        public IActionResult Export([FromQuery] PhaInputQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _PhaInputService.ExportList(parm).Result;
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
        [ActionPermissionFilter(Permission = "phainput:delete")]
        [HttpDelete("clean")]
        public IActionResult Clear()
        {
            if (!HttpContext.IsAdmin())
            {
                return ToResponse(ResultCode.FAIL, "操作失败");
            }
            return SUCCESS(_PhaInputService.TruncatePhaInput());
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "入库详情导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false)]
        [ActionPermissionFilter(Permission = "phainput:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<PhaInputDto> list = new();
            using (var stream = formFile.OpenReadStream())
            {
                list = stream.Query<PhaInputDto>(startCell: "A1").ToList();
            }

            return SUCCESS(_PhaInputService.ImportPhaInput(list.Adapt<List<PhaInput>>()));
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
            var result = DownloadImportTemplate(new List<PhaInputDto>() { }, "PhaInput");
            return ExportExcel(result.Item2, result.Item1);
        }

    }
}