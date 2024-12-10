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
    /// 厂家和供应商Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(ICompanyInfoService), ServiceLifetime = LifeTime.Transient)]
    public class CompanyInfoService : BaseService<CompanyInfo>, ICompanyInfoService
    {
        /// <summary>
        /// 查询厂家和供应商列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<CompanyInfoDto> GetList(CompanyInfoQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .ToPage<CompanyInfo, CompanyInfoDto>(parm);

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CompanyInfo GetInfo(int Id)
        {
            var response = Queryable()
                //.Where(x => x. == Id)
                .First();

            return response;
        }

        /// <summary>
        /// 添加厂家和供应商
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CompanyInfo AddCompanyInfo(CompanyInfo model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改厂家和供应商
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateCompanyInfo(CompanyInfo model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 清空厂家和供应商
        /// </summary>
        /// <returns></returns>
        public bool TruncateCompanyInfo()
        {
            var newTableName = $"CompanyInfo_{DateTime.Now:yyyyMMdd}";
            if (Queryable().Any() && !Context.DbMaintenance.IsAnyTable(newTableName))
            {
                Context.DbMaintenance.BackupTable("CompanyInfo", newTableName);
            }

            return Truncate();
        }
        /// <summary>
        /// 导入厂家和供应商
        /// </summary>
        /// <returns></returns>
        public (string, object, object) ImportCompanyInfo(List<CompanyInfo> list)
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
        /// 导出厂家和供应商
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<CompanyInfoDto> ExportList(CompanyInfoQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .Select((it) => new CompanyInfoDto()
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
        private static Expressionable<CompanyInfo> QueryExp(CompanyInfoQueryDto parm)
        {
            var predicate = Expressionable.Create<CompanyInfo>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.FacCode), it => it.FacCode == parm.FacCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.FacName), it => it.FacName.Contains(parm.FacName));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.SpellCode), it => it.SpellCode.Contains(parm.SpellCode));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.WbCode), it => it.WbCode.Contains(parm.WbCode));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.CustomCode), it => it.CustomCode.Contains(parm.CustomCode));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.CompanyType), it => it.CompanyType == parm.CompanyType);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.Remark), it => it.Remark == parm.Remark);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ValidFlag), it => it.ValidFlag == parm.ValidFlag);
            return predicate;
        }
    }
}