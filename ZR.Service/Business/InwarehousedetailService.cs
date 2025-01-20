using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Service.Business.IBusinessService;
using ZR.Model.GuiHis;
using Aliyun.OSS;

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
                .LeftJoin<TGInwarehouse>((id, ti) => id.SerialNum == ti.PlanNo.ToString())
                .LeftJoin<CompanyInfo>((id,ti,c)=>id.ProductCode==c.FacCode)
                .LeftJoin<Departments>((id, ti, c,f) => ti.DrugDeptCode == f.DeptCode)
                .Where((id, ti) => id.InwarehouseId == parm.InwarehouseId)
                .Select((id, ti,c,f) => new InwarehousedetaiWithDruglDto
                { 
                    BatchNo = id.BatchNo,
                    ApproveInfo = id.ApproveInfo,
                    ValiDate = id.ValiDate,
                    ProductDate = id.ProductDate,
                    Id = id.Id,
                    DrugCode = id.DrugCode,
                    InwarehouseQty = id.InwarehouseQty,
                    Remark = id.Remark,
                    CreateTime = id.CreateTime,
                    InwarehouseId = id.InwarehouseId,
                    PlanNo = ti.PlanNo.ToString(),
                    BillCode = ti.BillCode,
                    State = ti.State,
                    PlanType = ti.PlanType,
                    DrugDeptCode = f.DeptName,
                    TradeName = ti.TradeName,
                    Specs = ti.Specs,
                    RetailPrice = ti.RetailPrice,
                    WholesalePrice = ti.WholesalePrice,
                    PurchasePrice = ti.PurchasePrice,
                    PackUnit = ti.PackUnit,
                    PackQty = ti.PackQty,
                    MinUnit = ti.MinUnit,
                    ProducerCode = c.FacCode,
                    ProducerName = c.FacName,
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
                    InName=id.InName,
                    MixBuyPrice=id.MixBuyPrice,
                    MixOutPrice=id.MixOutPrice,
                    Tstars = id.Tstars,
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
        public List<InwarehousedetaiWithDruglDto> GetInfos(int Id)
        {
            //var response = Queryable()
            //    .Where(x => x.InwarehouseId == Id)
            //    .ToList();
            var response = Context.Queryable<Inwarehousedetail>()
                .LeftJoin<TGInwarehouse>((id, ti) => id.SerialNum == ti.PlanNo.ToString())
                .LeftJoin<CompanyInfo>((id, ti, c) => id.ProductCode == c.FacCode)
                .LeftJoin<Departments>((id, ti, c, f) => ti.DrugDeptCode == f.DeptCode)
                .LeftJoin<Inwarehouse>((id, ti, c, f,s)=>id.InwarehouseId==s.Id)
                .Where((id, ti, c, f, s) => s.Id == Id)
                .Select((id, ti, c, f) => new InwarehousedetaiWithDruglDto
                {
                    BatchNo = id.BatchNo,
                    ApproveInfo = id.ApproveInfo,
                    ValiDate = id.ValiDate,
                    ProductDate = id.ProductDate,
                    Id = id.Id,
                    DrugCode = id.DrugCode,
                    InwarehouseQty = id.InwarehouseQty,
                    Remark = id.Remark,
                    CreateTime = id.CreateTime,
                    InwarehouseId = id.InwarehouseId,
                    PlanNo = ti.PlanNo.ToString(),
                    BillCode = ti.BillCode,
                    State = ti.State,
                    PlanType = ti.PlanType,
                    DrugDeptCode = f.DeptName,
                    TradeName = ti.TradeName,
                    Specs = ti.Specs,
                    RetailPrice = ti.RetailPrice,
                    WholesalePrice = ti.WholesalePrice,
                    PurchasePrice = ti.PurchasePrice,
                    PackUnit = ti.PackUnit,
                    PackQty = ti.PackQty,
                    MinUnit = ti.MinUnit,
                    ProducerCode = c.FacCode,
                    ProducerName = c.FacName,
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
                    InName = id.InName,
                    MixBuyPrice = id.MixBuyPrice,
                    MixOutPrice = id.MixOutPrice,
                    Tstars = id.Tstars,
                }).ToList();
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

        //删除入库明细
        public int DeleteInwarehouseDetail(string ids)
        {
            try
            {
                Context.Ado.BeginTran();
                List<Inwarehousedetail> InwarehouseDetailItemList = Context.Queryable<Inwarehousedetail>().Where(it => ids.Contains(it.Id.ToString())).ToList();
                if (InwarehouseDetailItemList.Count != 0) 
                {
                    InwarehouseDetailItemList.ForEach((item) =>
                    {
                        //删除明细
                        Context.Deleteable<Inwarehousedetail>().Where(it => it.Id == item.Id).ExecuteCommand();
                        //修改planNo
                        //修改主表入库数量
                        Context.Updateable<Inwarehouse>().SetColumns(it => new Inwarehouse
                        {
                            StockNum = it.StockNum - item.InwarehouseQty,
                            PlanNo = SqlFunc.Replace(it.PlanNo, item.SerialNum, "")
                        })
                        .Where(it => it.Id == item.InwarehouseId)
                        .ExecuteCommand();
                    });
                }
                Context.Ado.CommitTran();
                return 1;
            }catch(Exception e)
            {
                Context.Ado.RollbackTran();
            }
            return -1;
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