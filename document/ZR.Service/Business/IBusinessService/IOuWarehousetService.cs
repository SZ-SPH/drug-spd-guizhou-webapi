using ZR.Model.Business.Dto;
using ZR.Model.Business;

namespace ZR.Service.Business.IBusinessService
{
    /// <summary>
    /// 出库service接口
    /// </summary>
    public interface IOuWarehousetService : IBaseService<OuWarehouset>
    {
        PagedInfo<OuWarehousetDto> GetList(OuWarehousetQueryDto parm);

        OuWarehouset GetInfo(int Id);


        OuWarehouset AddOuWarehouset(OuWarehouset parm);
        int UpdateOuWarehouset(OuWarehouset parm);
        
        bool TruncateOuWarehouset();

        (string, object, object) ImportOuWarehouset(List<OuWarehouset> list);

        PagedInfo<OuWarehousetDto> ExportList(OuWarehousetQueryDto parm);
    }
}
