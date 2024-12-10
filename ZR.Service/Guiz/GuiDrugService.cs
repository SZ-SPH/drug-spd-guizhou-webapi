using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Model.GuiHis.Dto;
using ZR.Model.GuiHis;
using ZR.Service.Guiz.IGuizService;

namespace ZR.Service.Guiz
{
    /// <summary>
    /// 药品Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IGuiDrugService), ServiceLifetime = LifeTime.Transient)]
    public class GuiDrugService : BaseService<GuiDrug>, IGuiDrugService
    {
        /// <summary>
        /// 查询药品列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<GuiDrugDto> GetList(GuiDrugQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .ToPage<GuiDrug, GuiDrugDto>(parm);

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="DrugTermId"></param>
        /// <returns></returns>
        public GuiDrug GetInfo(string DrugTermId)
        {
            var response = Queryable()
                .Where(x => x.DrugTermId == DrugTermId)
                .First();

            return response;
        }

        /// <summary>
        /// 添加药品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public GuiDrug AddGuiDrug(GuiDrug model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改药品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateGuiDrug(GuiDrug model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 清空药品
        /// </summary>
        /// <returns></returns>
        public bool TruncateGuiDrug()
        {
            var newTableName = $"GuiDrug_{DateTime.Now:yyyyMMdd}";
            if (Queryable().Any() && !Context.DbMaintenance.IsAnyTable(newTableName))
            {
                Context.DbMaintenance.BackupTable("GuiDrug", newTableName);
            }

            return Truncate();
        }
        /// <summary>
        /// 导入药品
        /// </summary>
        /// <returns></returns>
        public (string, object, object) ImportGuiDrug(List<GuiDrug> list)
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
        /// 导出药品
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<GuiDrugDto> ExportList(GuiDrugQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .Select((it) => new GuiDrugDto()
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
        private static Expressionable<GuiDrug> QueryExp(GuiDrugQueryDto parm)
        {
            var predicate = Expressionable.Create<GuiDrug>();

            //predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugtermId), it => it.DrugtermId.Contains(parm.DrugtermId));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.EnglishFormal), it => it.EnglishFormal.Contains(parm.EnglishFormal));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.RegularName), it => it.RegularName.Contains(parm.RegularName));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.RegularSpellCode), it => it.RegularSpellCode.Contains(parm.RegularSpellCode));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.RegularWbCode), it => it.RegularWbCode.Contains(parm.RegularWbCode));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.SpellCode), it => it.SpellCode.Contains(parm.SpellCode));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.TradeName), it => it.TradeName.Contains(parm.TradeName));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.WbCode), it => it.WbCode.Contains(parm.WbCode));
            return predicate;
        }
    }
}