using ZR.Model.Business.Dto;
using ZR.Model.Business;
using Infrastructure.Model;

namespace ZR.Service.Business.IBusinessService
{
    /// <summary>
    /// 入库主单service接口
    /// </summary>
    public interface IInwarehouseService : IBaseService<Inwarehouse>
    {
        PagedInfo<InwarehouseDto> GetList(InwarehouseQueryDto parm);

        Inwarehouse GetInfo(int Id);


        Inwarehouse AddInwarehouse(Inwarehouse parm);
        int UpdateInwarehouse(Inwarehouse parm);
        bool generateInwarehouse(List<InwarehouseGenerateInwarehouseDto> parm);
        int DeleteInwarehouse(string idArr);
        bool generateSelectiveInwarehouse(InwarehouseGenerateInwarehouseDto param);
        bool AppendSelectiveInwarehouse(List<AppendInwarehouseDetail> param);
    }
}
