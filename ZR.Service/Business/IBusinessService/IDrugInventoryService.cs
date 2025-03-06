using ZR.Model.Business.Dto;
using ZR.Model.Business;

namespace ZR.Service.Business.IBusinessService
{
    /// <summary>
    /// 采购退货service接口
    /// </summary>
    public interface IDrugInventoryService : IBaseService<DrugInventory>
    {
        PagedInfo<DrugInventoryDto> GetList(DrugInventoryQueryDto parm);

        DrugInventory GetInfo(int Id);
        DrugInventory GetInfos(long? code);


        DrugInventory AddDrugInventory(DrugInventory parm);
        int UpdateDrugInventory(DrugInventory parm);
        
        bool TruncateDrugInventory();

        (string, object, object) ImportDrugInventory(List<DrugInventory> list);

        PagedInfo<DrugInventoryDto> ExportList(DrugInventoryQueryDto parm);
    }
}
