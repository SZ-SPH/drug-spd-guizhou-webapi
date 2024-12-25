using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Service.Business.IBusinessService;

namespace ZR.Service.Business
{
    /// <summary>
    /// 货位药品Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(ILocationDrugService), ServiceLifetime = LifeTime.Transient)]
    public class LocationDrugService : BaseService<LocationDrug>, ILocationDrugService
    {
        /// <summary>
        /// 查询货位药品列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<LocationDrugDto> GetList(LocationDrugQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                //.OrderBy("Id asc")
                .Where(predicate.ToExpression())
                .ToPage<LocationDrug, LocationDrugDto>(parm);

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public LocationDrug GetInfo(int Id)
        {
            var response = Queryable()
                .Where(x => x.Id == Id)
                .First();

            return response;
        }

        /// <summary>
        /// 添加货位药品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public LocationDrug AddLocationDrug(LocationDrug model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改货位药品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateLocationDrug(LocationDrug model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 清空货位药品
        /// </summary>
        /// <returns></returns>
        public bool TruncateLocationDrug()
        {
            var newTableName = $"LocationDrug_{DateTime.Now:yyyyMMdd}";
            if (Queryable().Any() && !Context.DbMaintenance.IsAnyTable(newTableName))
            {
                Context.DbMaintenance.BackupTable("LocationDrug", newTableName);
            }
            
            return Truncate();
        }
        /// <summary>
        /// 导入货位药品
        /// </summary>
        /// <returns></returns>
        public (string, object, object) ImportLocationDrug(List<LocationDrug> list)
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
        /// 导出货位药品
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<LocationDrugDto> ExportList(LocationDrugQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .Select((it) => new LocationDrugDto()
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
        private static Expressionable<LocationDrug> QueryExp(LocationDrugQueryDto parm)
        {
            var predicate = Expressionable.Create<LocationDrug>();

            predicate = predicate.AndIF(parm.LocationId != null, it => it.LocationId == parm.LocationId);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugtermId), it => it.DrugtermId == parm.DrugtermId);
            return predicate;
        }
    }
}