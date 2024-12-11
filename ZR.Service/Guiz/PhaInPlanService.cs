using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Model.GuiHis.Dto;
using ZR.Service.Guiz.IGuizService;
using ZR.Model.GuiHis;

namespace ZR.Service.Guiz
{
    /// <summary>
    /// 入库计划Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IPhaInPlanService), ServiceLifetime = LifeTime.Transient)]
    public class PhaInPlanService : BaseService<PhaInPlan>, IPhaInPlanService
    {
        /// <summary>
        /// 查询入库计划列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<PhaInPlanDto> GetList(PhaInPlanQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .ToPage<PhaInPlan, PhaInPlanDto>(parm);

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="PlanNo"></param>
        /// <returns></returns>
        public PhaInPlan GetInfo(decimal PlanNo)
        {
            var response = Queryable()
                .Where(x => x.PlanNo == PlanNo)
                .First();

            return response;
        }

        /// <summary>
        /// 添加入库计划
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PhaInPlan AddPhaInPlan(PhaInPlan model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改入库计划
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdatePhaInPlan(PhaInPlan model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 清空入库计划
        /// </summary>
        /// <returns></returns>
        public bool TruncatePhaInPlan()
        {
            var newTableName = $"PhaInPlan_{DateTime.Now:yyyyMMdd}";
            if (Queryable().Any() && !Context.DbMaintenance.IsAnyTable(newTableName))
            {
                Context.DbMaintenance.BackupTable("PhaInPlan", newTableName);
            }

            return Truncate();
        }
        /// <summary>
        /// 导入入库计划
        /// </summary>
        /// <returns></returns>
        public (string, object, object) ImportPhaInPlan(List<PhaInPlan> list)
        {
            var x = Context.Storageable(list)
                .SplitInsert(it => !it.Any())
                .SplitError(x => x.Item.PlanNo.IsEmpty(), "入库计划流水号不能为空")
                .SplitError(x => x.Item.BillCode.IsEmpty(), "采购单号不能为空")
                .SplitError(x => x.Item.DrugDeptCode.IsEmpty(), "科室编码不能为空")
                .SplitError(x => x.Item.DrugCode.IsEmpty(), "药品编码不能为空")
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
        /// 导出入库计划
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<PhaInPlanDto> ExportList(PhaInPlanQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .Select((it) => new PhaInPlanDto()
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
        private static Expressionable<PhaInPlan> QueryExp(PhaInPlanQueryDto parm)
        {
            var predicate = Expressionable.Create<PhaInPlan>();

            predicate = predicate.AndIF(parm.PlanNo>0, it => it.PlanNo == parm.PlanNo);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.BillCode), it => it.BillCode == parm.BillCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.State), it => it.State == parm.State);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.PlanType), it => it.PlanType == parm.PlanType);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugDeptCode), it => it.DrugDeptCode == parm.DrugDeptCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugCode), it => it.DrugCode == parm.DrugCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.TradeName), it => it.TradeName == parm.TradeName);
            //predicate = predicate.AndIF(parm.BeginPlanDate == null, it => it.PlanDate >= DateTime.Now.ToShortDateString().ParseToDateTime());
            predicate = predicate.AndIF(parm.BeginPlanDate != null, it => it.PlanDate >= parm.BeginPlanDate);
            predicate = predicate.AndIF(parm.EndPlanDate != null, it => it.PlanDate <= parm.EndPlanDate);
            //predicate = predicate.AndIF(parm.BeginStockDate == null, it => it.StockDate >= DateTime.Now.ToShortDateString().ParseToDateTime());
            predicate = predicate.AndIF(parm.BeginStockDate != null, it => it.StockDate >= parm.BeginStockDate);
            predicate = predicate.AndIF(parm.EndStockDate != null, it => it.StockDate <= parm.EndStockDate);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.StockNo), it => it.StockNo == parm.StockNo);
            //predicate = predicate.AndIF(parm.BeginOperDate == null, it => it.OperDate >= DateTime.Now.ToShortDateString().ParseToDateTime());
            predicate = predicate.AndIF(parm.BeginOperDate != null, it => it.OperDate >= parm.BeginOperDate);
            predicate = predicate.AndIF(parm.EndOperDate != null, it => it.OperDate <= parm.EndOperDate);
            return predicate;
        }
    }
}