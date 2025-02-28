using ZR.Model.Business.Dto;
using ZR.Model.Business;

namespace ZR.Service.Business.IBusinessService
{
    /// <summary>
    /// 入库详情service接口
    /// </summary>
    public interface ITInwarehousedetailService : IBaseService<TInwarehousedetail>
    {
        PagedInfo<TInwarehousedetailDto> GetList(TInwarehousedetailQueryDto parm);
        PagedInfo<InwarehousedetaiWithDruglDto> GetLists(TInwarehousedetailQueryDto parm);

        
        TInwarehousedetail GetInfo(int Id);


        TInwarehousedetail AddTInwarehousedetail(TInwarehousedetail parm);
        int UpdateTInwarehousedetail(TInwarehousedetail parm);
        
        bool TruncateTInwarehousedetail();

        (string, object, object) ImportTInwarehousedetail(List<TInwarehousedetail> list);

        PagedInfo<TInwarehousedetailDto> ExportList(TInwarehousedetailQueryDto parm);
    }
}
