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
    /// 采购退货Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IDrugInventoryService), ServiceLifetime = LifeTime.Transient)]
    public class DrugInventoryService : BaseService<DrugInventory>, IDrugInventoryService
    {
        /// <summary>
        /// 查询采购退货列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<DrugInventoryDto> GetList(DrugInventoryQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable().LeftJoin<Departments>((it, o)=> it.DrugDeptCode==o.DeptCode)
                .LeftJoin<CompanyInfo>((it, o,p)=> it.CompanyCode==p.FacCode)
                .Where(predicate.ToExpression())
                .Where((it, o, p) => string.IsNullOrEmpty(parm.DrugDeptName) || o.DeptName.Contains(parm.DrugDeptName.Trim()))
                .Where((it, o, p) => string.IsNullOrEmpty(parm.CompanyName) || p.FacName.Contains(parm.CompanyName.Trim()))
                .Select((it, o, p) =>
                 new DrugInventoryDto{
                   InBillCode = it.InBillCode.SelectAll(),
                   DrugDeptName = o.DeptName,
                   CompanyName = p.FacName,
                })
                .OrderBy(it => it.ApplyDate, OrderByType.Desc)
                .ToPage(parm);

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DrugInventory GetInfo(int Id)
        {
            var response = Queryable()
                //.Where(x => x.Id == Id)
                .First();

            return response;
        }
        public DrugInventory GetInfos(long? InBillCode)
        {
            var response = Queryable()
                .Where(x => x.InBillCode == InBillCode)
                .First();

            return response;
        }
        
        /// <summary>
        /// 添加采购退货
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DrugInventory AddDrugInventory(DrugInventory model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改采购退货
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateDrugInventory(DrugInventory model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 清空采购退货
        /// </summary>
        /// <returns></returns>
        public bool TruncateDrugInventory()
        {
            var newTableName = $"DrugInventory_{DateTime.Now:yyyyMMdd}";
            if (Queryable().Any() && !Context.DbMaintenance.IsAnyTable(newTableName))
            {
                Context.DbMaintenance.BackupTable("DrugInventory", newTableName);
            }
            
            return Truncate();
        }
        /// <summary>
        /// 导入采购退货
        /// </summary>
        /// <returns></returns>
        public (string, object, object) ImportDrugInventory(List<DrugInventory> list)
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
        /// 导出采购退货
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<DrugInventoryDto> ExportList(DrugInventoryQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .Select((it) => new DrugInventoryDto()
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
        private static Expressionable<DrugInventory> QueryExp(DrugInventoryQueryDto parm)
        {
            var predicate = Expressionable.Create<DrugInventory>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugName), it => it.TradeName.Contains(parm.DrugName.Trim()));
            return predicate;
        }
    }
}