using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Service.Business.IBusinessService;

namespace ZR.Service.Business
{
    /// <summary>
    /// 出库单Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IOutOrderService), ServiceLifetime = LifeTime.Transient)]
    public class OutOrderService : BaseService<OutOrder>, IOutOrderService
    {
        /// <summary>
        /// 查询出库单列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<OutOrderDto> GetList(OutOrderQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .ToPage<OutOrder, OutOrderDto>(parm);

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public OutOrder GetInfo(int Id)
        {
            var response = Queryable()
                .Where(x => x.Id == Id)
                .First();

            return response;
        }

        /// <summary>
        /// 添加出库单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OutOrder AddOutOrder(OutOrder model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改出库单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateOutOrder(OutOrder model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 清空出库单
        /// </summary>
        /// <returns></returns>
        public bool TruncateOutOrder()
        {
            var newTableName = $"OutOrder_{DateTime.Now:yyyyMMdd}";
            if (Queryable().Any() && !Context.DbMaintenance.IsAnyTable(newTableName))
            {
                Context.DbMaintenance.BackupTable("OutOrder", newTableName);
            }
            
            return Truncate();
        }
        /// <summary>
        /// 导入出库单
        /// </summary>
        /// <returns></returns>
        public (string, object, object) ImportOutOrder(List<OutOrder> list)
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
        /// 导出出库单
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<OutOrderDto> ExportList(OutOrderQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .Select((it) => new OutOrderDto()
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
        private static Expressionable<OutOrder> QueryExp(OutOrderQueryDto parm)
        {
            var predicate = Expressionable.Create<OutOrder>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.OutOrderCode), it => it.OutOrderCode == parm.OutOrderCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.InpharmacyId), it => it.InpharmacyId == parm.InpharmacyId);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.OutWarehouseID), it => it.OutWarehouseID == parm.OutWarehouseID);
            predicate = predicate.AndIF(parm.OutBillCode != null, it => it.OutBillCode == parm.OutBillCode);
            return predicate;
        }
    }
}