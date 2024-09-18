using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Service.Business.IBusinessService;

namespace ZR.Service.Business
{
    /// <summary>
    /// 医嘱Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IMedicalAdviceService), ServiceLifetime = LifeTime.Transient)]
    public class MedicalAdviceService : BaseService<MedicalAdvice>, IMedicalAdviceService
    {
        /// <summary>
        /// 查询医嘱列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<MedicalAdviceDto> GetList(MedicalAdviceQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .ToPage<MedicalAdvice, MedicalAdviceDto>(parm);

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public MedicalAdvice GetInfo(int OrderId)
        {
            var response = Queryable()
                .Where(x => x.OrderId == OrderId)
                .First();

            return response;
        }

        /// <summary>
        /// 添加医嘱
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MedicalAdvice AddMedicalAdvice(MedicalAdvice model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改医嘱
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateMedicalAdvice(MedicalAdvice model)
        {
            return Update(model, true, "修改医嘱");
        }

        /// <summary>
        /// 清空医嘱
        /// </summary>
        /// <returns></returns>
        public bool TruncateMedicalAdvice()
        {
            var newTableName = $"MEDICAL_ADVICE_{DateTime.Now:yyyyMMdd}";
            if (Queryable().Any() && !Context.DbMaintenance.IsAnyTable(newTableName))
            {
                Context.DbMaintenance.BackupTable("MEDICAL_ADVICE", newTableName);
            }

            return Truncate();
        }
        /// <summary>
        /// 导入医嘱
        /// </summary>
        /// <returns></returns>
        public (string, object, object) ImportMedicalAdvice(List<MedicalAdvice> list)
        {
            var x = Context.Storageable(list)
                .SplitInsert(it => !it.Any())
                .SplitError(x => x.Item.OrderId.IsEmpty(), "id不能为空")
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
        /// 导出医嘱
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<MedicalAdviceDto> ExportList(MedicalAdviceQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .Select((it) => new MedicalAdviceDto()
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
        private static Expressionable<MedicalAdvice> QueryExp(MedicalAdviceQueryDto parm)
        {
            var predicate = Expressionable.Create<MedicalAdvice>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.IpiRegistrationId), it => it.IpiRegistrationId == parm.IpiRegistrationId);
            predicate = predicate.AndIF(parm.DrugId != null, it => it.DrugId == parm.DrugId);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.EmployeeName), it => it.EmployeeName == parm.EmployeeName);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DepartmentChineseName), it => it.DepartmentChineseName == parm.DepartmentChineseName);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.IpiReaistrationNo), it => it.IpiReaistrationNo == parm.IpiReaistrationNo);
            return predicate;
        }
    }
}