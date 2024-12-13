using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Service.Business.IBusinessService;
using Infrastructure;

namespace ZR.Service.Business
{
    /// <summary>
    /// 入库主单Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IInwarehouseService), ServiceLifetime = LifeTime.Transient)]
    public class InwarehouseService : BaseService<Inwarehouse>, IInwarehouseService
    {
        /// <summary>
        /// 查询入库主单列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<InwarehouseDto> GetList(InwarehouseQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .OrderBy(it => it.CreateTime,OrderByType.Desc)
                .ToPage<Inwarehouse, InwarehouseDto>(parm);

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Inwarehouse GetInfo(int Id)
        {
            var response = Queryable()
                .Where(x => x.Id == Id)
                .First();

            return response;
        }

        /// <summary>
        /// 添加入库主单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Inwarehouse AddInwarehouse(Inwarehouse model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 删除入库单
        /// </summary>
        /// <param name="idArr"></param>
        /// <returns></returns>
        public int DeleteInwarehouse(string idArr)
        {
            int res = Context.Deleteable<Inwarehouse>().Where(it => idArr.Contains(it.Id.ToString())).ExecuteCommand();
            Context.Deleteable<Inwarehousedetail>().Where(it => idArr.Contains(it.InwarehouseId.ToString())).ExecuteCommand();
            return res;
        }

        //生成入库单
        public bool generateInwarehouse(List<InwarehouseGenerateInwarehouseDto> parm)
        {
            var username = HttpContextExtension.GetName(App.HttpContext);
            foreach (var item in parm)
            {
                var inwarehouseParm = new Inwarehouse()
                {
                    CreateMan = username,
                    CreateTime = DateTime.Now,
                    PlanNo = string.Join(",", item.PlanNos),
                    InwarehouseNum = $"IN{SnowFlakeSingle.Instance.getID()}",
                    PurchaseOrderNum = item.BillCode,
                    StockNum = item.StockNum
                };
                Inwarehouse inwarehouse = Insertable(inwarehouseParm).ExecuteReturnEntity();
                List<Inwarehousedetail> inDetailList = Context.Queryable<TGInwarehouse>()
                    .Where(it => item.PlanNos.Contains(it.PlanNo))
                    .Select(it => new Inwarehousedetail
                    {
                        DrugCode = it.DrugCode,
                        InwarehouseQty = int.Parse(it.StockNum),
                        CreateTime = DateTime.Now,
                        InwarehouseId = inwarehouse.Id,
                        SerialNum = it.PlanNo
                    })
                    .ToList();
                Context.Insertable<Inwarehousedetail>(inDetailList).ExecuteCommand();
                Context.Updateable<TGInwarehouse>().Where(it => string.Join(",", item.PlanNos).Contains(it.PlanNo)).SetColumns(it => new TGInwarehouse { Status = "1" }).ExecuteCommand();
            }
            return true;
        }

        /// <summary>
        /// 修改入库主单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateInwarehouse(Inwarehouse model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 查询导出表达式
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private static Expressionable<Inwarehouse> QueryExp(InwarehouseQueryDto parm)
        {
            var predicate = Expressionable.Create<Inwarehouse>();
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.PlanNo), it => it.PlanNo.Equals(parm.PlanNo));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.PurchaseOrderNum), it => it.PurchaseOrderNum.Equals(parm.PurchaseOrderNum));
            return predicate;
        }
    }
}