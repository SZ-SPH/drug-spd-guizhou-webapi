using ZR.Model.Business.Dto;
using ZR.Model.Business;
using static ZR.Service.Business.OuWarehousetService;

namespace ZR.Service.Business.IBusinessService
{
    /// <summary>
    /// 出库药品详情service接口
    /// </summary>
    public interface IOuWarehousetService : IBaseService<OuWarehouset>
    {
        PagedInfo<OuWarehousetDto> GetList(OuWarehousetQueryDto parm);
        DAYprice DAYGetList(OuWarehousetQueryDto parm);
        OuWarehouset GetInfo(int Id);
        List<OuWarehouset> EList(int Id);

        OuWarehouset AddOuWarehouset(OuWarehouset parm);
        int UpdateOuWarehouset(OuWarehouset parm);
        
        bool TruncateOuWarehouset();

        (string, object, object) ImportOuWarehouset(List<OuWarehouset> list);

        PagedInfo<OuWarehousetDtos> ExportList(OuWarehousetQueryDto parm);
    }
}
