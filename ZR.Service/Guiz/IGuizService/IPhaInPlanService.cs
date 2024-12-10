using ZR.Model.GuiHis;
using ZR.Model.GuiHis.Dto;
namespace ZR.Service.Guiz.IGuizService
{
    /// <summary>
    /// 入库计划service接口
    /// </summary>
    public interface IPhaInPlanService : IBaseService<PhaInPlan>
    {
        PagedInfo<PhaInPlanDto> GetList(PhaInPlanQueryDto parm);

        PhaInPlan GetInfo(decimal PlanNo);


        PhaInPlan AddPhaInPlan(PhaInPlan parm);
        int UpdatePhaInPlan(PhaInPlan parm);

        bool TruncatePhaInPlan();

        (string, object, object) ImportPhaInPlan(List<PhaInPlan> list);

        PagedInfo<PhaInPlanDto> ExportList(PhaInPlanQueryDto parm);
    }
}
