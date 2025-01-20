using ZR.Model.Business.Dto;
using ZR.Model.Business;
using static ZR.Service.Business.OutOrderService;

namespace ZR.Service.Business.IBusinessService
{
    /// <summary>
    /// 出库单service接口
    /// </summary>
    public interface IOutOrderService : IBaseService<OutOrder>
    {
        PagedInfo<OutOrderDto> GetList(OutOrderQueryDto parm);
        OutOrderDto GetInfos(int Id);
        OutOrderDto GetInfoss(int Id);
        
        OutOrder GetInfo(int Id);
      string Byin(string zhi);
        string Byout(string zhi);


        OutOrder AddOutOrder(OutOrder parm);
        int UpdateOutOrder(OutOrder parm);
        
        bool TruncateOutOrder();

        (string, object, object) ImportOutOrder(List<OutOrder> list);

        PagedInfo<OutOrderDto> ExportList(OutOrderQueryDto parm);
   
    }
}
