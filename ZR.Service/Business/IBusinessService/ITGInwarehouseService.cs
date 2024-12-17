using ZR.Model.Business.Dto;
using ZR.Model.Business;

namespace ZR.Service.Business.IBusinessService
{
    /// <summary>
    /// 采购计划入库service接口
    /// </summary>
    public interface ITGInwarehouseService : IBaseService<TGInwarehouse>
    {
        PagedInfo<TGInwarehouseDto> GetList(TGInwarehouseQueryDto parm);

        TGInwarehouse GetInfo(string Id);


        TGInwarehouse AddTGInwarehouse(TGInwarehouse parm);

        int UpdateTGInwarehouse(TGInwarehouse parm);


        PagedInfo<TGInwarehouseDto> ExportList(TGInwarehouseQueryDto parm);
        object PushInwarehouseInfoToHis(TGInwarehouseQueryDto parm);
    }
}
