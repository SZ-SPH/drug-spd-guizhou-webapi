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
    /// 入库计划
    /// </summary>
    [Verify]
    [Route("business/PhaInPlan")]
    public class PhaInPlanController : BaseController
    {
        /// <summary>
        /// 入库计划接口
        /// </summary>
        private readonly IPhaInPlanService _PhaInPlanService;

        public PhaInPlanController(IPhaInPlanService PhaInPlanService)
        {
            _PhaInPlanService = PhaInPlanService;
        }

        /// <summary>
        /// 查询入库计划列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "phainplan:list")]
        public IActionResult QueryPhaInPlan([FromQuery] PhaInPlanQueryDto parm)
        {
            var response = _PhaInPlanService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询入库计划详情
        /// </summary>
        /// <param name="PlanNo"></param>
        /// <returns></returns>
        [HttpGet("{PlanNo}")]
        [ActionPermissionFilter(Permission = "phainplan:query")]
        public IActionResult GetPhaInPlan(decimal PlanNo)
        {
            var response = _PhaInPlanService.GetInfo(PlanNo);

            var info = response.Adapt<PhaInPlanDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加入库计划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "phainplan:add")]
        [Log(Title = "入库计划", BusinessType = BusinessType.INSERT)]
        public IActionResult AddPhaInPlan([FromBody] PhaInPlanDto parm)
        {
            var modal = parm.Adapt<PhaInPlan>().ToCreate(HttpContext);

            var response = _PhaInPlanService.AddPhaInPlan(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新入库计划
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "phainplan:edit")]
        [Log(Title = "入库计划", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdatePhaInPlan([FromBody] PhaInPlanDto parm)
        {
            var modal = parm.Adapt<PhaInPlan>().ToUpdate(HttpContext);
            var response = _PhaInPlanService.UpdatePhaInPlan(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除入库计划
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "phainplan:delete")]
        [Log(Title = "入库计划", BusinessType = BusinessType.DELETE)]
        public IActionResult DeletePhaInPlan([FromRoute] string ids)
        {
            var idArr = Tools.SplitAndConvert<decimal>(ids);

            return ToResponse(_PhaInPlanService.Delete(idArr));
        }

        /// <summary>
        /// 导出入库计划
        /// </summary>
        /// <returns></returns>
        [Log(Title = "入库计划", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "phainplan:export")]
        public IActionResult Export([FromQuery] PhaInPlanQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _PhaInPlanService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "入库计划", "入库计划");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 清空入库计划
        /// </summary>
        /// <returns></returns>
        [Log(Title = "入库计划", BusinessType = BusinessType.CLEAN)]
        [ActionPermissionFilter(Permission = "phainplan:delete")]
        [HttpDelete("clean")]
        public IActionResult Clear()
        {
            if (!HttpContext.IsAdmin())
            {
                return ToResponse(ResultCode.FAIL, "操作失败");
            }
            return SUCCESS(_PhaInPlanService.TruncatePhaInPlan());
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "入库计划导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false)]
        [ActionPermissionFilter(Permission = "phainplan:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<PhaInPlanDto> list = new();
            using (var stream = formFile.OpenReadStream())
            {
                list = stream.Query<PhaInPlanDto>(startCell: "A1").ToList();
            }

            return SUCCESS(_PhaInPlanService.ImportPhaInPlan(list.Adapt<List<PhaInPlan>>()));
        }

        /// <summary>
        /// 入库计划导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "入库计划模板", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            var result = DownloadImportTemplate(new List<PhaInPlanDto>() { }, "PhaInPlan");
            return ExportExcel(result.Item2, result.Item1);
        }

    }
}