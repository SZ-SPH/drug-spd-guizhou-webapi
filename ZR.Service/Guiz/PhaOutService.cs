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
    /// 出库记录Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IPhaOutService), ServiceLifetime = LifeTime.Transient)]
    public class PhaOutService : BaseService<PhaOut>, IPhaOutService
    {
        /// <summary>
        /// 查询出库记录列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<PhaOutDto> GetList(PhaOutQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .ToPage<PhaOut, PhaOutDto>(parm);

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="outBillCode"></param>
        /// <returns></returns>
        public PhaOut GetInfo(long outBillCode)
        {
            var response = Queryable()
                .Where(x => x.OutBillCode == outBillCode)
                .First();

            return response;
        }

        /// <summary>
        /// 添加出库记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PhaOut AddPhaOut(PhaOut model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改出库记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdatePhaOut(PhaOut model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 清空出库记录
        /// </summary>
        /// <returns></returns>
        public bool TruncatePhaOut()
        {
            var newTableName = $"PhaOut_{DateTime.Now:yyyyMMdd}";
            if (Queryable().Any() && !Context.DbMaintenance.IsAnyTable(newTableName))
            {
                Context.DbMaintenance.BackupTable("PhaOut", newTableName);
            }

            return Truncate();
        }
        /// <summary>
        /// 导入出库记录
        /// </summary>
        /// <returns></returns>
        public (string, object, object) ImportPhaOut(List<PhaOut> list)
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
        /// 导出出库记录
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<PhaOutDto> ExportList(PhaOutQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .Select((it) => new PhaOutDto()
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
        private static Expressionable<PhaOut> QueryExp(PhaOutQueryDto parm)
        {
            var predicate = Expressionable.Create<PhaOut>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugDeptCode), it => it.DrugDeptCode == parm.DrugDeptCode);
            predicate = predicate.AndIF(parm.OutBillCode != null, it => it.OutBillCode == parm.OutBillCode);
            predicate = predicate.AndIF(parm.SerialCode != null, it => it.SerialCode == parm.SerialCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.GroupCode), it => it.GroupCode == parm.GroupCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.OutListCode), it => it.OutListCode == parm.OutListCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.OutType), it => it.OutType == parm.OutType);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.Class3MeaningCode), it => it.Class3MeaningCode == parm.Class3MeaningCode);
            predicate = predicate.AndIF(parm.InBillCode != null, it => it.InBillCode == parm.InBillCode);
            predicate = predicate.AndIF(parm.InSerialCode != null, it => it.InSerialCode == parm.InSerialCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.InListCode), it => it.InListCode == parm.InListCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugCode), it => it.DrugCode == parm.DrugCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.TradeName), it => it.TradeName.Contains(parm.TradeName));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugType), it => it.DrugType == parm.DrugType);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ProducerCode), it => it.ProducerCode == parm.ProducerCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.CompanyCode), it => it.CompanyCode == parm.CompanyCode);
            return predicate;
        }
    }
}