﻿using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Model.GuiHis.Dto;
using ZR.Service.Guiz.IGuizService;
using ZR.Model.GuiHis;


namespace ZR.Service.Guiz
{
    /// <summary>
    /// 科室Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IDrugStoreService), ServiceLifetime = LifeTime.Transient)]
    public class DrugStoreService : BaseService<DrugStore>, IDrugStoreService
    {
        /// <summary>
        /// 查询科室列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<DrugStoreDto> GetList(DrugStoreQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .ToPage<DrugStore, DrugStoreDto>(parm);

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="DrugDeptCode"></param>
        /// <returns></returns>
        public DrugStore GetInfo(string DrugDeptCode)
        {
            var response = Queryable()
                .Where(x => x.DrugDeptCode == DrugDeptCode)
                .First();

            return response;
        }

        /// <summary>
        /// 添加科室
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DrugStore AddDrugStore(DrugStore model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改科室
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateDrugStore(DrugStore model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 清空科室
        /// </summary>
        /// <returns></returns>
        public bool TruncateDrugStore()
        {
            var newTableName = $"DrugStore_{DateTime.Now:yyyyMMdd}";
            if (Queryable().Any() && !Context.DbMaintenance.IsAnyTable(newTableName))
            {
                Context.DbMaintenance.BackupTable("DrugStore", newTableName);
            }

            return Truncate();
        }
        /// <summary>
        /// 导入科室
        /// </summary>
        /// <returns></returns>
        public (string, object, object) ImportDrugStore(List<DrugStore> list)
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
        /// 导出科室
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<DrugStoreDto> ExportList(DrugStoreQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .Select((it) => new DrugStoreDto()
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
        private static Expressionable<DrugStore> QueryExp(DrugStoreQueryDto parm)
        {
            var predicate = Expressionable.Create<DrugStore>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugDeptCode), it => it.DrugDeptCode.Contains(parm.DrugDeptCode));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugDeptName), it => it.DrugDeptName.Contains(parm.DrugDeptName));
        
            return predicate;
        }
    }
}