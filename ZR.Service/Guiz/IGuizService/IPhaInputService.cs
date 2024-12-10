using ZR.Model.GuiHis;
using ZR.Model.GuiHis.Dto;
namespace ZR.Service.Guiz.IGuizService
{
    /// <summary>
    /// 入库详情service接口
    /// </summary>
    public interface IPhaInputService : IBaseService<PhaInput>
    {
        PagedInfo<PhaInputDto> GetList(PhaInputQueryDto parm);

        PhaInput GetInfo(int Id);


        PhaInput AddPhaInput(PhaInput parm);
        int UpdatePhaInput(PhaInput parm);

        bool TruncatePhaInput();

        (string, object, object) ImportPhaInput(List<PhaInput> list);

        PagedInfo<PhaInputDto> ExportList(PhaInputQueryDto parm);
    }
}
