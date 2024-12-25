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
            try
            {
                Context.Ado.BeginTran();
                Inwarehouse inwarehouseItem = Context.Queryable<Inwarehouse>().Where(it => idArr.Contains(it.Id.ToString())).Single();
                Context.Updateable<PhaInPlan>().SetColumns(it => it.Status == "0").Where(it => inwarehouseItem.PlanNo.Contains(it.PlanNo.ToString())).ExecuteCommand();
                Context.Deleteable<Inwarehousedetail>().Where(it => idArr.Contains(it.InwarehouseId.ToString())).ExecuteCommand();
                int res = Context.Deleteable<Inwarehouse>().Where(it => idArr.Contains(it.Id.ToString()) && it.PushStatu != "1").ExecuteCommand();
                Context.Ado.CommitTran();
                return res;
            }
            catch (Exception e)
            {
                Context.Ado.RollbackTran();
            }
            return -1;
        }

        
        /// <summary>
        /// 追加入库单明细
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool AppendSelectiveInwarehouse(List<AppendInwarehouseDetail> param)
        {
            try 
            {
                Context.Ado.BeginTran();
                List<Inwarehousedetail> inwarehousedetailList = new List<Inwarehousedetail>();
                string inwarehouseItemNum = param[0].InwarehouseNum;
                Inwarehouse inwarehouseItem = Context.Queryable<Inwarehouse>().Where(it => it.InwarehouseNum == inwarehouseItemNum).Single();
                //更改主单应该入库数量
                int totalStockNum = param.Sum(item => item.StockNum);
                string planNos = string.Join(",", param.Select(n => n.PlanNo));
                Context.Updateable<Inwarehouse>().SetColumns(it => new Inwarehouse { StockNum = totalStockNum,PlanNo = planNos }).Where(it => it.InwarehouseNum == inwarehouseItemNum).ExecuteCommand();
                param.ForEach((item) =>
                {
                    Inwarehousedetail inwarehouseDetailItem = new Inwarehousedetail()
                    {
                        DrugCode = item.DrugCode,
                        InwarehouseQty = (item.StockNum),
                        CreateTime = DateTime.Now,
                        SerialNum = item.PlanNo,
                        InwarehouseId = inwarehouseItem.Id,
                        PurchaseOrderNum = item.BillCode
                    };
                    inwarehousedetailList.Add(inwarehouseDetailItem);
                    //更新生成状态
                    Context.Updateable<TGInwarehouse>().Where(it => it.PlanNo == int.Parse(item.PlanNo)).SetColumns(it => new TGInwarehouse { Status = "1" }).ExecuteCommand();
                });
                //添加明细
                Context.Insertable<Inwarehousedetail>(inwarehousedetailList).ExecuteCommand();
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
                PushStatu = "0",
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
                        StockNum = item.StockNum
                    };
                    Inwarehouse inwarehouse = Insertable(inwarehouseParm).ExecuteReturnEntity();
                    List<Inwarehousedetail> inDetailList = Context.Queryable<TGInwarehouse>()
                        .Where(it => item.PlanNos.Contains(it.PlanNo.ToString()))
                        .Select(it => new Inwarehousedetail
                        {
                            DrugCode = it.DrugCode,
                            InwarehouseQty = int.Parse(it.StockNum),
                            CreateTime = DateTime.Now,
                            InwarehouseId = inwarehouse.Id,
                            SerialNum = it.PlanNo.ToString()
                        })
                        .ToList();
                    Context.Insertable<Inwarehousedetail>(inDetailList).ExecuteCommand();
                    var planNoList = item.PlanNos.Select(decimal.Parse).ToList();
                    Context.Updateable<TGInwarehouse>().Where(it => planNoList.Contains(it.PlanNo)).SetColumns(it => new TGInwarehouse { Status = "1" }).ExecuteCommand();
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
            CompanyInfo supplierItem = Context.Queryable<CompanyInfo>().Where(it => it.FacCode == model.SupplierCode).Single();
            //int res = Context.Updateable<Inwarehouse>().SetColumns(it => it.SupplierName == supplierItem.FacName).Where(it => it.Id == model.Id).ExecuteCommand();
            int res = Context.Updateable<Inwarehouse>().SetColumns(it => new Inwarehouse()
            {
                SupplierName = supplierItem.FacName,
                BillCode = model.BillCode,
                BillTime = model.BillTime
            }).Where(it => it.Id == model.Id).ExecuteCommand();
            return res;
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
            return predicate;
        }
    }
}