using ZR.Model.GuiHis;
using ZR.Model.GuiHis.Dto;
namespace ZR.Service.Guiz.IGuizService
{
    /// <summary>
    /// 药品service接口
    /// </summary>
    public interface IGuiDrugService : IBaseService<GuiDrug>
    {
        PagedInfo<GuiDrugDto> GetList(GuiDrugQueryDto parm);

        GuiDrug GetInfo(string DrugTermId);


        GuiDrug AddGuiDrug(GuiDrug parm);
        int UpdateGuiDrug(GuiDrug parm);

        bool TruncateGuiDrug();

        (string, object, object) ImportGuiDrug(List<GuiDrug> list);

        PagedInfo<GuiDrugDto> ExportList(GuiDrugQueryDto parm);
    }
}
