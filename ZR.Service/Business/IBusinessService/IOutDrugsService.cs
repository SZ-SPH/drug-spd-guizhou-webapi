using ZR.Model.Business.Dto;
using ZR.Model.Business;

namespace ZR.Service.Business.IBusinessService
{
    /// <summary>
    /// 采购退货service接口
    /// </summary>
    public interface IOutDrugsService : IBaseService<OutDrugs>
    {
        PagedInfo<OutDrugsDto> GetList(OutDrugsQueryDto parm);

        OutDrugs GetInfo(int Id);
         List<OutDrugs> EList(int Id);
        
          

    OutDrugs AddOutDrugs(OutDrugs parm);
        int UpdateOutDrugs(OutDrugs parm);
        
        bool TruncateOutDrugs();

        (string, object, object) ImportOutDrugs(List<OutDrugs> list);

        PagedInfo<OutDrugsDto> ExportList(OutDrugsQueryDto parm);
    }
}
