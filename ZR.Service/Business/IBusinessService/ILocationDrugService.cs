using ZR.Model.Business.Dto;
using ZR.Model.Business;

namespace ZR.Service.Business.IBusinessService
{
    /// <summary>
    /// 货位药品service接口
    /// </summary>
    public interface ILocationDrugService : IBaseService<LocationDrug>
    {
        PagedInfo<LocationDrugDto> GetList(LocationDrugQueryDto parm);

        LocationDrug GetInfo(int Id);


        LocationDrug AddLocationDrug(LocationDrug parm);
        int UpdateLocationDrug(LocationDrug parm);
        
        bool TruncateLocationDrug();

        (string, object, object) ImportLocationDrug(List<LocationDrug> list);

        PagedInfo<LocationDrugDto> ExportList(LocationDrugQueryDto parm);
    }
}
