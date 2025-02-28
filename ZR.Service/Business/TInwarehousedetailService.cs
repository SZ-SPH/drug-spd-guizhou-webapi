using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Service.Business.IBusinessService;
using ZR.Model.GuiHis;

namespace ZR.Service.Business
{
    /// <summary>
    /// 入库详情Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(ITInwarehousedetailService), ServiceLifetime = LifeTime.Transient)]
    public class TInwarehousedetailService : BaseService<TInwarehousedetail>, ITInwarehousedetailService
    {
        /// <summary>
        /// 查询入库详情列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<TInwarehousedetailDto> GetList(TInwarehousedetailQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .ToPage<TInwarehousedetail, TInwarehousedetailDto>(parm);

            return response;
        }
        public PagedInfo<InwarehousedetaiWithDruglDto> GetLists(TInwarehousedetailQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .LeftJoin<TGInwarehouse>((it, ti)=>it.SerialNum==ti.PlanNo.ToString())
                //.LeftJoin<Inwarehouse>((it, ti,c) => it.InwarehouseId == ti.Id)
                .LeftJoin<CompanyInfo>((it, ti, c) => it.ProductCode == c.FacCode)
                .LeftJoin<Departments>((it, ti, c, f) => ti.DrugDeptCode == f.DeptCode)
                .Where(predicate.ToExpression())
                .OrderByDescending((it) => it.CreateTime)
                .Select((it, ti, c, f) => new InwarehousedetaiWithDruglDto
                {

                    BatchNo = it.BatchNo,
                    ApproveInfo = it.ApproveInfo,
                    ValiDate = it.ValiDate,
                    ProductDate = it.ProductDate,
                    Id = it.Id,
                    DrugCode = it.DrugCode,
                    InwarehouseQty = it.InwarehouseQty.Value,
                    Remark = it.Remark,
                    CreateTime = it.CreateTime.Value,
                    InwarehouseId = it.InwarehouseId.Value,
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
                    InName = it.InName,
                    MixBuyPrice = it.MixBuyPrice,
                    MixOutPrice = it.MixOutPrice,
                    Tstars = it.Tstars,

                })
                .ToPage(parm);

            return response;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public TInwarehousedetail GetInfo(int Id)
        {
            var response = Queryable()
                .Where(x => x.Id == Id)
                .First();

            return response;
        }

        /// <summary>
        /// 添加入库详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public TInwarehousedetail AddTInwarehousedetail(TInwarehousedetail model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改入库详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateTInwarehousedetail(TInwarehousedetail model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 清空入库详情
        /// </summary>
        /// <returns></returns>
        public bool TruncateTInwarehousedetail()
        {
            var newTableName = $"t_inwarehousedetail_{DateTime.Now:yyyyMMdd}";
            if (Queryable().Any() && !Context.DbMaintenance.IsAnyTable(newTableName))
            {
                Context.DbMaintenance.BackupTable("t_inwarehousedetail", newTableName);
            }
            
            return Truncate();
        }
        /// <summary>
        /// 导入入库详情
        /// </summary>
        /// <returns></returns>
        public (string, object, object) ImportTInwarehousedetail(List<TInwarehousedetail> list)
        {
            var x = Context.Storageable(list)
                .SplitInsert(it => !it.Any())
                //.WhereColumns(it => it.UserName)//如果不是主键可以这样实现（多字段it=>new{it.x1,it.x2}）
                .ToStorage();
            var result = x.AsInsertable.ExecuteCommand();//插入可插入部分;

            string msg = $"插入{x.InsertList.Count} 更新{x.UpdateList.Count} 错误数据{x.ErrorList.Count} 不计算数据{x.IgnoreList.Count} 删除数据{x.DeleteList.Count} 总共{x.TotalList.Count}";                    
            Console.WriteLine(msg);

            //输出错误信息               
            foreach (var item in x.ErrorList)
            {
                Console.WriteLine("错误" + item.StorageMessage);
            }
            foreach (var item in x.IgnoreList)
            {
                Console.WriteLine("忽略" + item.StorageMessage);
            }

            return (msg, x.ErrorList, x.IgnoreList);
        }

        /// <summary>
        /// 导出入库详情
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<TInwarehousedetailDto> ExportList(TInwarehousedetailQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .Select((it) => new TInwarehousedetailDto()
                {
                }, true)
                .ToPage(parm);

            return response;
        }

        /// <summary>
        /// 查询导出表达式
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private static Expressionable<TInwarehousedetail> QueryExp(TInwarehousedetailQueryDto parm)
        {
            var predicate = Expressionable.Create<TInwarehousedetail>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugCode), it => it.DrugCode == parm.DrugCode);
            predicate = predicate.AndIF(parm.BeginCreateTime != null, it => it.CreateTime >= parm.BeginCreateTime);
            predicate = predicate.AndIF(parm.EndCreateTime != null, it => it.CreateTime <= parm.EndCreateTime);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.BatchNo), it => it.BatchNo == parm.BatchNo);
            return predicate;
        }
        private static Expressionable<Inwarehousedetail> QueryExps(InwarehousedetailQueryDto parm)
        {
            var predicate = Expressionable.Create<Inwarehousedetail>();

            return predicate;
        }
    }
}