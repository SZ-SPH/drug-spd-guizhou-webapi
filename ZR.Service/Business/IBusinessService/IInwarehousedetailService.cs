using ZR.Model.Business.Dto;
using ZR.Model.Business;

namespace ZR.Service.Business.IBusinessService
{
    /// <summary>
    /// 添加条目详细service接口
    /// </summary>
    public interface IInwarehousedetailService : IBaseService<Inwarehousedetail>
    {
        PagedInfo<InwarehousedetaiWithDruglDto> GetList(InwarehousedetailQueryDto parm);

        Inwarehousedetail GetInfo(int Id);


        Inwarehousedetail AddInwarehousedetail(Inwarehousedetail parm);
        int UpdateInwarehousedetail(Inwarehousedetail parm);


    }
}
