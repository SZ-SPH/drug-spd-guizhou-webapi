using ZR.Model.GuiHis;
using ZR.Model.GuiHis.Dto;

namespace ZR.Service.Guiz.IGuizService
{
    /// <summary>
    /// 科室service接口
    /// </summary>
    public interface IDrugStoreService : IBaseService<DrugStore>
    {
        PagedInfo<DrugStoreDto> GetList(DrugStoreQueryDto parm);

        DrugStore GetInfo(string DrugDeptCode);


        DrugStore AddDrugStore(DrugStore parm);
        int UpdateDrugStore(DrugStore parm);

        bool TruncateDrugStore();

        (string, object, object) ImportDrugStore(List<DrugStore> list);

        PagedInfo<DrugStoreDto> ExportList(DrugStoreQueryDto parm);
    }
}
