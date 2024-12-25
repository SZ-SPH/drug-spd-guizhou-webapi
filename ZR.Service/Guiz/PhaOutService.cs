using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Model.GuiHis.Dto;
using ZR.Service.Guiz.IGuizService;
using ZR.Model.GuiHis;
using JinianNet.JNTemplate;
using System.ComponentModel;


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
                .LeftJoin<Departments>((p, d1) => p.DrugDeptCode == d1.DeptCode)
                .LeftJoin<Departments>((p, d1, d2) => p.DrugStorageCode == d2.DeptCode)
                .Where(predicate.ToExpression())
         .Where((p, d1, d2) => string.IsNullOrEmpty(parm.DrugDeptName) || d1.DeptName.Contains(parm.DrugDeptName)) // 使用 DeptName 过滤
        .Where((p, d1, d2) => string.IsNullOrEmpty(parm.DrugStorageName) || d2.DeptName.Contains(parm.DrugStorageName))
                .Select((p, d1, d2) => new PhaOutDto
                {
                    OutBillCode = p.OutBillCode.SelectAll(),
                    DrugDeptName = d1.DeptName,
                    DrugStorageName = d2.DeptName,
                })
                .ToPage(parm);
    
            return response;
        }

    //多表查询存在别名不一致,请把Where中的it改成p就可以了，特殊需求可以使用.Select((x, y)=>new{ id=x.id,name=y.name
    //}).MergeTable().Orderby(xxx=>xxx.Id)功能将Select中的多表结果集变成单表，这样就可以不限制别名一样
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

        private static Expressionable<PhaOutDto> QueryExps(PhaOutQueryDto parm)
        {
            var predicate = Expressionable.Create<PhaOutDto>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugDeptCode), p => p.DrugDeptCode == parm.DrugDeptCode);
            predicate = predicate.AndIF(parm.OutBillCode != null, p => p.OutBillCode == parm.OutBillCode);
            predicate = predicate.AndIF(parm.SerialCode != null, p => p.SerialCode == parm.SerialCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.GroupCode), p => p.GroupCode == parm.GroupCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.OutListCode), p => p.OutListCode == parm.OutListCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.OutType), p => p.OutType == parm.OutType);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.Class3MeaningCode), p => p.Class3MeaningCode == parm.Class3MeaningCode);
            predicate = predicate.AndIF(parm.InBillCode != null, p => p.InBillCode == parm.InBillCode);
            predicate = predicate.AndIF(parm.InSerialCode != null, p => p.InSerialCode == parm.InSerialCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.InListCode), p => p.InListCode == parm.InListCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugCode), p => p.DrugCode == parm.DrugCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.TradeName), p => p.TradeName.Contains(parm.TradeName));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugType), p => p.DrugType == parm.DrugType);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ProducerCode), p => p.ProducerCode == parm.ProducerCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.CompanyCode), p => p.CompanyCode == parm.CompanyCode);
            //predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugDeptName), p => p.DrugDeptName.Contains(parm.DrugDeptName));
            //predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugStorageName), p => p.DrugStorageName.Contains(parm.DrugStorageName));

            return predicate;
        }
    }
}