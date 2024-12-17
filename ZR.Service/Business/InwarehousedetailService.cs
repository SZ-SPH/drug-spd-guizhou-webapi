using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Service.Business.IBusinessService;

namespace ZR.Service.Business
{
    /// <summary>
    /// 添加条目详细Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IInwarehousedetailService), ServiceLifetime = LifeTime.Transient)]
    public class InwarehousedetailService : BaseService<Inwarehousedetail>, IInwarehousedetailService
    {
        /// <summary>
        /// 查询添加条目详细列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<InwarehousedetaiWithDruglDto> GetList(InwarehousedetailQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Context.Queryable<Inwarehousedetail>()
                .LeftJoin<TGInwarehouse>((id, ti) => id.DrugCode == ti.DrugCode)
                .Where((id, ti) => id.InwarehouseId == parm.InwarehouseId)
                .Select((id, ti) => new InwarehousedetaiWithDruglDto
                {
                    Id = id.Id,
                    DrugCode = id.DrugCode,
                    InwarehouseQty = id.InwarehouseQty,
                    Remark = id.Remark,
                    CreateTime = id.CreateTime,
                    InwarehouseId = id.InwarehouseId,
                    PlanNo = ti.PlanNo,
                    BillCode = ti.BillCode,
                    State = ti.State,
                    PlanType = ti.PlanType,
                    DrugDeptCode = ti.DrugDeptCode,
                    TradeName = ti.TradeName,
                    Specs = ti.Specs,
                    RetailPrice = ti.RetailPrice,
                    WholesalePrice = ti.WholesalePrice,
                    PurchasePrice = ti.PurchasePrice,
                    PackUnit = ti.PackUnit,
                    PackQty = ti.PackQty,
                    MinUnit = ti.MinUnit,
                    ProducerCode = ti.ProducerCode,
                    ProducerName = ti.ProducerName,
                    StoreNum = ti.StoreNum,
                    StoreTotsum = ti.StoreTotsum,
                    OutputSum = ti.OutputSum,
                    PlanNum = ti.PlanNum,
                    PlanEmpl = ti.PlanEmpl,
                    PlanDate = ti.PlanDate,
                    StockNum = ti.StockNum,
                    StockEmpl = ti.StockEmpl,
                    StockDate = ti.StockDate,
                    ApproveEmpl = ti.ApproveEmpl,
                    ApproveDate = ti.ApproveDate,
                    StockNo = ti.StockNo,
                })
                .ToPage(parm);
            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Inwarehousedetail GetInfo(int Id)
        {
            var response = Queryable()
                .Where(x => x.Id == Id)
                .First();

            return response;
        }

        /// <summary>
        /// 添加添加条目详细
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Inwarehousedetail AddInwarehousedetail(Inwarehousedetail model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改添加条目详细
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateInwarehousedetail(Inwarehousedetail model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 查询导出表达式
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private static Expressionable<Inwarehousedetail> QueryExp(InwarehousedetailQueryDto parm)
        {
            var predicate = Expressionable.Create<Inwarehousedetail>();

            return predicate;
        }
    }
}