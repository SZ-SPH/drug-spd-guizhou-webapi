using ZR.Model.GuiHis;
using ZR.Model.GuiHis.Dto;

namespace ZR.Service.Guiz.IGuizService
{
    /// <summary>
    /// 出库记录service接口
    /// </summary>
    public interface IPhaOutService : IBaseService<PhaOut>
    {
        PagedInfo<PhaOutDto> GetList(PhaOutQueryDto parm);

        PhaOut GetInfo(int Id);


        PhaOut AddPhaOut(PhaOut parm);
        int UpdatePhaOut(PhaOut parm);

        bool TruncatePhaOut();

        (string, object, object) ImportPhaOut(List<PhaOut> list);

        PagedInfo<PhaOutDto> ExportList(PhaOutQueryDto parm);
    }
}
