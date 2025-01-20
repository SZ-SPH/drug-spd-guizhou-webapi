using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Service.Business.IBusinessService;
using Aliyun.OSS;
using ZR.Model.GuiHis;

namespace ZR.Service.Business
{
    /// <summary>
    /// 采购退货Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IOutDrugsService), ServiceLifetime = LifeTime.Transient)]
    public class OutDrugsService : BaseService<OutDrugs>, IOutDrugsService
    {
        /// <summary>
        /// 查询采购退货列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<OutDrugsDto> GetList(OutDrugsQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                //.OrderBy("OutorderID asc")
                .Where(predicate.ToExpression())
                .ToPage<OutDrugs, OutDrugsDto>(parm);

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public OutDrugs GetInfo(int Id)
        {
            var response = Queryable()
                //.Where(x => x.Id == Id)
                .First();

            return response;
        }

        /// <summary>
        /// 添加采购退货
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OutDrugs AddOutDrugs(OutDrugs model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改采购退货
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateOutDrugs(OutDrugs model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 清空采购退货
        /// </summary>
        /// <returns></returns>
        public bool TruncateOutDrugs()
        {
            var newTableName = $"OutDrugs_{DateTime.Now:yyyyMMdd}";
            if (Queryable().Any() && !Context.DbMaintenance.IsAnyTable(newTableName))
            {
                Context.DbMaintenance.BackupTable("OutDrugs", newTableName);
            }
            
            return Truncate();
        }
        /// <summary>
        /// 导入采购退货
        /// </summary>
        /// <returns></returns>
        public (string, object, object) ImportOutDrugs(List<OutDrugs> list)
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
        public PagedInfo<OutDrugsDto> ExportList(OutDrugsQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .Select((it) => new OutDrugsDto()
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
        private static Expressionable<OutDrugs> QueryExp(OutDrugsQueryDto parm)
        {
            var predicate = Expressionable.Create<OutDrugs>();

            predicate = predicate.AndIF(parm.OutorderID != null, it => it.OutorderID == parm.OutorderID);
            predicate = predicate.AndIF(parm.drugname != null, it => it.TradeName.Contains(parm.drugname.Trim()));

            
            return predicate;
        }

        public List<OutDrugs> EList(int Id)
        {
            var response = Queryable().LeftJoin<CompanyInfo>((it, p) => it.ProducerCode == p.FacCode)
              .Where(it => it.OutorderID == Id).Select((it, p) => new OutDrugs()
              {
                  ProducerCode = p.FacName
              }, true).ToList();
            return response;
        }
    }
}