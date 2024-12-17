using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Service.Business.IBusinessService;
using Infrastructure;
using ZR.Model.GuiHis;

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

        
        /// <summary>
        /// 追加入库单明细
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool AppendSelectiveInwarehouse(InwarehouseGenerateInwarehouseDto param)
        {
            //入参类似 billcode:IN1868586339822346240 planNos:103494,103495 
            var planNoList = param.PlanNos.Select(decimal.Parse).ToList();
            var planNoStr = string.Join(",", param.PlanNos);
            //var planNoStr = param.PlanNos.Select(it => string.Join(",", it)).ToString();
            //Inwarehouse inwarehouseItem = Context.Queryable<Inwarehouse>().Where(it => it.InwarehouseNum == param.PurchaseNum).Single();
            try 
            {
                Context.Ado.BeginTran();
                //查出当前主单
                Inwarehouse inwarehouseItem = Context.Queryable<Inwarehouse>().Where(it => it.InwarehouseNum == param.PurchaseNum).Single();
                //更改主订单状态
                if (string.IsNullOrEmpty(inwarehouseItem.PurchaseOrderNum))
                {
                    Context.Updateable<Inwarehouse>()
                    .SetColumns(it => new Inwarehouse
                    {
                        PlanNo = SqlFunc.IsNullOrEmpty(it.PlanNo) ? planNoStr : it.PlanNo + planNoStr,
                        StockNum = it.StockNum + param.StockNum,
                        PurchaseOrderNum = param.BillCode
                    })
                    .Where(it => it.InwarehouseNum == param.PurchaseNum) // 更新条件
                    .ExecuteCommand();
                }
                else
                {
                    Context.Updateable<Inwarehouse>()
                    .SetColumns(it => new Inwarehouse
                    {
                        PlanNo = it.PlanNo + planNoStr,
                        StockNum = it.StockNum + param.StockNum,
                    })
                    .Where(it => it.InwarehouseNum == param.PurchaseNum && it.PurchaseOrderNum == param.BillCode) // 更新条件
                    .ExecuteCommand();
                }
                //查出所需采购计划明细，构造数据
                List<PhaInPlan> phaInPlanList = Context.Queryable<PhaInPlan>().Where(it => planNoList.Contains(it.PlanNo)).ToList();
                List<Inwarehousedetail> inwarehousedetailList = new List<Inwarehousedetail>();
                phaInPlanList.ForEach((item) =>
                {
                    Inwarehousedetail inwarehousedetailItem = new Inwarehousedetail()
                    {
                        DrugCode = item.DrugCode,
                        InwarehouseQty = item.StockNum.ParseToInt(),
                        InwarehouseId = inwarehouseItem.Id,
                        CreateTime = DateTime.Now,
                        SerialNum = item.PlanNo.ToString()
                    };
                    inwarehousedetailList.Add(inwarehousedetailItem);
                });
                //添加明细
                Context.Insertable<Inwarehousedetail>(inwarehousedetailList).ExecuteCommand();
                //更新生成状态
                Context.Updateable<TGInwarehouse>().Where(it => planNoList.Contains(decimal.Parse(it.PlanNo))).SetColumns(it => new TGInwarehouse { Status = "1" }).ExecuteCommand();
                Context.Ado.CommitTran();
                return true;
            }
            catch(Exception e)
            {
                Context.Ado.RollbackTran();
                return false;
            }
            
        }

        /// <summary>
        /// 生成可选入库单
        /// </summary>
        /// <returns></returns>
        public bool generateSelectiveInwarehouse(InwarehouseGenerateInwarehouseDto param)
        {
            var username = HttpContextExtension.GetName(App.HttpContext);
            CompanyInfo companyInfo = Context.Queryable<CompanyInfo>().Where(it => it.FacCode == param.SupplierCode).Single();
            var inwarehouseParm = new Inwarehouse()
            {
                CreateMan = username,
                CreateTime = DateTime.Now,
                InwarehouseNum = $"IN{SnowFlakeSingle.Instance.getID()}",
                BillCode = param.BillCode,
                BillTime = param.BillTime,
                SupplierCode = param.SupplierCode,
                SupplierName = companyInfo.FacName
            };
            Inwarehouse inwarehouse = Insertable(inwarehouseParm).ExecuteReturnEntity();
            return inwarehouse != null;
        }

        //生成入库单
        public bool generateInwarehouse(List<InwarehouseGenerateInwarehouseDto> parm)
        {
            var username = HttpContextExtension.GetName(App.HttpContext);
            try
            {
                Context.Ado.BeginTran();
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
                    var planNoList = item.PlanNos.Select(decimal.Parse).ToList();
                    Context.Updateable<TGInwarehouse>().Where(it => planNoList.Contains(decimal.Parse(it.PlanNo))).SetColumns(it => new TGInwarehouse { Status = "1" }).ExecuteCommand();
                }
                Context.Ado.CommitTran();
                return true;
            }
            catch(Exception e)
            {
                Context.Ado.RollbackTran();
                return false;
            }
          
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
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.PlanNo), it => it.PlanNo.Contains(parm.PlanNo));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.PurchaseOrderNum), it => it.PurchaseOrderNum.Equals(parm.PurchaseOrderNum));
            return predicate;
        }
    }
}