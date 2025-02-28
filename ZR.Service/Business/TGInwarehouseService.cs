using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Service.Business.IBusinessService;
using System.Text;
using ZR.Model.GuiHis;
using Microsoft.AspNetCore.Http;
using Infrastructure;
namespace ZR.Service.Business
{
    /// <summary>
    /// 采购计划入库Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(ITGInwarehouseService), ServiceLifetime = LifeTime.Transient)]
    public class TGInwarehouseService : BaseService<TGInwarehouse>, ITGInwarehouseService
    {

        public HttpContext context;
        /// <summary>
        /// 查询采购计划入库列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<TGInwarehouseDto> GetList(TGInwarehouseQueryDto parm)
        {
         
            var predicate = QueryExp(parm);
            //未过期 当前时间小于截至时间
            if (parm.EndState == "0")
            {
              predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.EndState), it => it.EndDate >= DateTime.Now);
            }
            else if (parm.EndState == "1") 
            {
                predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.EndState), it => it.EndDate < DateTime.Now);
            }

            var response = Queryable()
                .LeftJoin<VPhainPlan>((it,s)=>it.PlanNo==s.PlanNo)
                .LeftJoin<Departments>((it,s,p)=>it.DrugDeptCode==p.DeptCode)                
                .Where(predicate.ToExpression())
                .Select((it,s,p) => new TGInwarehouse()
                {
                    DrugDeptCode = p.DeptName,
                    Qty=s.Qty
                }, true)         
                .OrderBy(it=>it.PlanDate,OrderByType.Desc)
                .ToPage<TGInwarehouse, TGInwarehouseDto>(parm);

            return response;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public TGInwarehouse GetInfo(string Id)
        {
            var response = Queryable()
                .Where(x => x.PlanNo.ToString() == Id)
                .First();

            return response;
        }

        /// <summary>
        /// 添加采购计划入库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public TGInwarehouse AddTGInwarehouse(TGInwarehouse model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改采购计划入库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateTGInwarehouse(InwarhouseDetailDTO model)
        {
            return Context.Updateable<Inwarehousedetail>().SetColumns(it => new Inwarehousedetail()
            {
                BatchNo = model.BatchNo,
                ValiDate = model.ValiDate,
                ApproveInfo = model.ApproveInfo,
                ProductDate = model.ProductDate,
            }).Where(it => it.DrugCode == model.DrugCode).ExecuteCommand();
        }

        public object PushInwarehouseInfoToHis(Push parm)
        {
            //按照采购单号分组推送
            int n = 0;
            var username = HttpContextExtension.GetName(App.HttpContext);
            List<Inwarehouse> iwarehouseList = Context.Queryable<Inwarehouse>().Where(it => parm.BillCodes.Contains(it.InwarehouseNum)).ToList();
          
            foreach (var inwarehouseListItem in iwarehouseList)
            {
                if (inwarehouseListItem.PushStatu== "已推送")
                {
                    return n;
                }
                List<Inwarehousedetail> inwarehouseDetailList = Context.Queryable<Inwarehousedetail>().Where(it => it.InwarehouseId == inwarehouseListItem.Id).ToList();
                List<InwarhouseHisDTO> pushHisList = new List<InwarhouseHisDTO>();
                pushHisList.Clear();
                int serialNum = 1;
                DateTime dateTime = DateTime.Now;
                foreach (var group in inwarehouseDetailList)
                {

                    InwarhouseDetailDTO PlanNoCorrespondingItem = Context.Queryable<Inwarehousedetail>()
                    .LeftJoin<TGInwarehouse>((id, ti) => id.SerialNum == ti.PlanNo.ToString())
                    .LeftJoin<Inwarehouse>((id, ti, i) => i.Id == id.InwarehouseId).Where(id => id.Id == group.Id)
                   .Select((id, ti, i) => new InwarhouseDetailDTO
                   {
                       spdInputId=i.Id.ToString(),
                       PlanNo = (ti.PlanNo),
                       BillCode = ti.BillCode,
                       StockNo = ti.StockNo,
                       SerialCode = serialNum,
                       DrugDeptCode = ti.DrugDeptCode,
                       GroupCode = id.BatchId,
                       InType = "01",
                       Class3MeaningCode = "11",
                       DrugCode = ti.DrugCode,
                       TradeName = ti.TradeName,
                       Specs = ti.Specs,
                       PackUnit = ti.PackUnit,
                       PackQty = int.Parse(ti.PackQty),
                       MinUnit = ti.MinUnit,
                       BatchNo = id.BatchNo,
                       //有效期暂待定
                       ValidDate = DateTime.Parse(id.ValiDate).ToString("yyyy-MM-dd HH:mm:ss"),
                       ProducerCode = id.ProductCode,
                       CompanyCode = i.SupplierCode,
                       RetailPrice = id.MixOutPrice,
                       WholesalePrice = decimal.Parse(ti.WholesalePrice),
                       PurchasePrice = id.MixBuyPrice,
                       InNum = id.InwarehouseQty,
                       RetailCost = id.MixOutPrice * (id.InwarehouseQty / decimal.Parse(ti.PackQty)),
                       WholesaleCost = decimal.Parse(ti.WholesalePrice) * (id.InwarehouseQty / decimal.Parse(ti.PackQty)),
                       PurchaseCost = id.MixBuyPrice * (id.InwarehouseQty / decimal.Parse(ti.PackQty)),
                       SpecialFlag = "0",
                       InState = "2",
                       ApplyNum = int.Parse(ti.StockNum),
                       ApplyOperCode = username,
                       ApplyDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                       ExamNum = int.Parse(ti.StockNum),
                       ExamOperCode = username,
                       ExamDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                       ApproveOperCode = "",
                       ApproveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                       OperCode = username,
                       OperDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                       Mark = ti.Mark,
                       PurcharsePriceFirsttime = decimal.Parse(ti.PurchasePrice),
                       IsTenderOffer = "0",
                       ProductionDate = DateTime.Parse(id.ProductDate).ToString("yyyy-MM-dd HH:mm:ss"),  
                       //待定
                       ApproveInfo = id.ApproveInfo,
                       SerialNum = id.SerialNum,
                       BatchId = id.BatchId,
                       //发票日期
                       InvoiceDate = i.BillTime,
                       InvoiceNo = i.BillCode,
                       ExtCode1 = id.InName,

                   })
                    .Single();
                    InwarhouseHisDTO HisDTO = new InwarhouseHisDTO();
                    HisDTO.spdInputId=PlanNoCorrespondingItem.spdInputId;
                    HisDTO.extCode1 = PlanNoCorrespondingItem.ExtCode1;
                    HisDTO.planNo = PlanNoCorrespondingItem.PlanNo;
                    HisDTO.mark = PlanNoCorrespondingItem.Mark;
                    HisDTO.billCode = PlanNoCorrespondingItem.BillCode;
                    HisDTO.stockNo = PlanNoCorrespondingItem.StockNo;
                    HisDTO.serialCode = PlanNoCorrespondingItem.SerialCode;
                    HisDTO.drugDeptCode = PlanNoCorrespondingItem.DrugDeptCode;
                    HisDTO.groupCode = PlanNoCorrespondingItem.BatchId;
                    HisDTO.inType = PlanNoCorrespondingItem.InType;
                    HisDTO.class3MeaningCode = PlanNoCorrespondingItem.Class3MeaningCode;
                    HisDTO.drugCode = PlanNoCorrespondingItem.DrugCode;
                    HisDTO.applyDate = PlanNoCorrespondingItem.ApplyDate;
                    HisDTO.tradeName = PlanNoCorrespondingItem.TradeName;
                    HisDTO.specs = PlanNoCorrespondingItem.Specs;
                    HisDTO.packUnit = PlanNoCorrespondingItem.PackUnit;
                    HisDTO.packQty = PlanNoCorrespondingItem.PackQty;
                    HisDTO.minUnit = PlanNoCorrespondingItem.MinUnit;
                    HisDTO.batchNo = PlanNoCorrespondingItem.BatchNo;
                    HisDTO.validDate = PlanNoCorrespondingItem.ValidDate;
                    HisDTO.producerCode = PlanNoCorrespondingItem.ProducerCode;
                    HisDTO.companyCode = PlanNoCorrespondingItem.CompanyCode;
                    HisDTO.retailPrice = PlanNoCorrespondingItem.RetailPrice;
                    HisDTO.wholesalePrice = PlanNoCorrespondingItem.WholesalePrice;
                    HisDTO.purchasePrice = PlanNoCorrespondingItem.PurchasePrice;
                    HisDTO.inNum = PlanNoCorrespondingItem.InNum;
                    HisDTO.retailCost = PlanNoCorrespondingItem.RetailCost;
                    HisDTO.wholesaleCost = PlanNoCorrespondingItem.WholesaleCost;
                    HisDTO.purchaseCost = PlanNoCorrespondingItem.PurchaseCost;
                    HisDTO.specialFlag = PlanNoCorrespondingItem.SpecialFlag;
                    HisDTO.inState = PlanNoCorrespondingItem.InState;
                    HisDTO.applyNum = PlanNoCorrespondingItem.ApplyNum;
                    HisDTO.applyOperCode = PlanNoCorrespondingItem.ApplyOperCode;
                    HisDTO.examDate = PlanNoCorrespondingItem.ExamDate;
                    HisDTO.examNum = PlanNoCorrespondingItem.ExamNum;
                    HisDTO.approveOperCode = PlanNoCorrespondingItem.ApproveOperCode;
                    HisDTO.approveDate = PlanNoCorrespondingItem.ApproveDate;
                    HisDTO.operCode = PlanNoCorrespondingItem.OperCode;
                    HisDTO.purcharsePriceFirsttime = PlanNoCorrespondingItem.PurcharsePriceFirsttime;
                    HisDTO.isTenderOffer = PlanNoCorrespondingItem.IsTenderOffer;
                    HisDTO.productionDate = PlanNoCorrespondingItem.ProductionDate;
                    HisDTO.approveInfo = PlanNoCorrespondingItem.ApproveInfo;
                    HisDTO.operDate = PlanNoCorrespondingItem.OperDate;
                    HisDTO.invoiceDate = PlanNoCorrespondingItem.InvoiceDate;
                    HisDTO.invoiceNo = PlanNoCorrespondingItem.InvoiceNo;
                    pushHisList.Add(HisDTO);
                    serialNum++;

                    //var jsonData = JsonConvert.SerializeObject(pushHisList);
                    //var response = PostData(postUrl, pushHisList);
                    if (string.IsNullOrEmpty(HisDTO.validDate)
                       || string.IsNullOrEmpty(HisDTO.companyCode)
                       || string.IsNullOrEmpty(HisDTO.productionDate)
                       || string.IsNullOrEmpty(HisDTO.batchNo))
                    {
                        //输出 为空
                        Context.Updateable<Inwarehousedetail>().SetColumns(it => new Inwarehousedetail()
                        {
                            Tstars = "存在有（有效期，生产厂家，生产日期，批号）其中数据为空，不能推送"
                        }).Where(it => it.Id == group.Id).ExecuteCommand();
                        continue;
                    }
                    else if (DateTime.Parse(HisDTO.validDate) < DateTime.Now.Date)
                        { 
                            //输出错误信息
                            Context.Updateable<Inwarehousedetail>().SetColumns(it => new Inwarehousedetail()
                            {
                                Tstars = "有效期小于当前日期，不能推送"
                            }).Where(it => it.Id == group.Id).ExecuteCommand();
                            continue;

                    }
                    else
                    {
                        Context.Updateable<Inwarehousedetail>().SetColumns(it => new Inwarehousedetail()
                        {
                            Tstars = ""
                        }).Where(it => it.Id == group.Id).ExecuteCommand();
                        continue;
                    }


                }
                string postUrl = $"http://192.168.1.95:7800/His/PhaInput";
                using (var http = new HttpClient())
                {
                    var jsonData = JsonConvert.SerializeObject(pushHisList);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = http.PostAsync(postUrl, content).Result;
                    response.EnsureSuccessStatusCode();
                    // 获取响应内容
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    var responseDTO = JsonConvert.DeserializeObject<InwarhouseHisResponseDTO>(responseBody);
                    if ("0".Equals(responseDTO.Code))
                    {                     
                        Context.Updateable<Inwarehouse>().SetColumns(it => new Inwarehouse()
                        {
                            PushStatu = "已推送",
                            PushTime = dateTime
                        }).Where(it => it.Id == inwarehouseListItem.Id).ExecuteCommand();
                        n++;
                        Context.Updateable<Inwarehousedetail>().SetColumns(it => new Inwarehousedetail()
                        {
                            Tstars = "已推送"
                        }).Where(it => it.InwarehouseId == inwarehouseListItem.Id).ExecuteCommand();
                    }
                    else
                    {
                        Context.Updateable<Inwarehouse>().SetColumns(it => new Inwarehouse()
                        {
                            PushStatu = "推送失败！原因是" + responseDTO.Msg
                        }).Where(it => it.Id == inwarehouseListItem.Id).ExecuteCommand();
                    }
                }
            };



            return n;


        }

       
        public int returnPush(int ids)
        {
           int n= 0;

           var username = HttpContextExtension.GetName(App.HttpContext);
            // 先去
            List<Inwarehousedetail> inwarehouseDetailList = Context.Queryable<Inwarehousedetail>().Where(it => it.Id==ids).ToList();
            foreach (var group in inwarehouseDetailList)
            {
                List<InwarhouseHisDTO> pushHisList = new List<InwarhouseHisDTO>();
                pushHisList.Clear();
                int serialNum = 1;
                InwarhouseDetailDTO PlanNoCorrespondingItem = Context.Queryable<Inwarehousedetail>()
                .LeftJoin<TGInwarehouse>((id, ti) => id.SerialNum == ti.PlanNo.ToString())
                .LeftJoin<Inwarehouse>((id, ti, i) => i.Id == id.InwarehouseId).Where(id => id.Id == group.Id)
                .Select((id, ti, i) => new InwarhouseDetailDTO
                {
                    PlanNo = (ti.PlanNo),
                    BillCode = ti.BillCode,
                    StockNo = ti.StockNo,
                    SerialCode = serialNum,
                    DrugDeptCode = ti.DrugDeptCode,
                    GroupCode = id.BatchId,
                    InType = "01",
                    Class3MeaningCode = "11",
                    DrugCode = ti.DrugCode,
                    TradeName = ti.TradeName,
                    Specs = ti.Specs,
                    PackUnit = ti.PackUnit,
                    PackQty = int.Parse(ti.PackQty),
                    MinUnit = ti.MinUnit,
                    BatchNo = id.BatchNo,
                    //有效期暂待定
                    ValidDate =DateTime.Parse(id.ValiDate).ToString("yyyy-MM-dd HH:mm:ss"),
                    ProducerCode = id.ProductCode,
                    CompanyCode = i.SupplierCode,
                    RetailPrice = id.MixOutPrice,
                    WholesalePrice = decimal.Parse(ti.WholesalePrice),
                    PurchasePrice = id.MixBuyPrice,
                    InNum = id.InwarehouseQty,
                    RetailCost = id.MixOutPrice *( id.InwarehouseQty / decimal.Parse(ti.PackQty)),
                    WholesaleCost = decimal.Parse(ti.WholesalePrice) * (id.InwarehouseQty / decimal.Parse(ti.PackQty)),
                    PurchaseCost = id.MixBuyPrice * (id.InwarehouseQty / decimal.Parse(ti.PackQty)),
                    SpecialFlag = "0",
                    InState = "2",
                    ApplyNum = int.Parse(ti.StockNum),
                    ApplyOperCode = "",
                    ApplyDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ExamNum = int.Parse(ti.StockNum),
                    ExamOperCode = "",
                    ExamDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ApproveOperCode = "",
                    ApproveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    OperCode = username,
                    OperDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Mark = ti.Mark,
                    PurcharsePriceFirsttime = decimal.Parse(ti.PurchasePrice),
                    IsTenderOffer = "0",
                    ProductionDate =id.ProductDate,
                    //待定
                    ApproveInfo = id.ApproveInfo,
                    SerialNum = id.SerialNum,
                    BatchId = id.BatchId,
                    //发票日期
                    InvoiceDate =i.BillTime,
                    InvoiceNo = i.BillCode,
                    ExtCode1 = id.InName,

                })
                .Single();
                InwarhouseHisDTO HisDTO = new InwarhouseHisDTO();
                HisDTO.extCode1 = PlanNoCorrespondingItem.ExtCode1;
                HisDTO.planNo = PlanNoCorrespondingItem.PlanNo;
                HisDTO.mark = PlanNoCorrespondingItem.Mark;
                HisDTO.billCode = PlanNoCorrespondingItem.BillCode;
                HisDTO.stockNo = PlanNoCorrespondingItem.StockNo;
                HisDTO.serialCode = PlanNoCorrespondingItem.SerialCode;
                HisDTO.drugDeptCode = PlanNoCorrespondingItem.DrugDeptCode;
                HisDTO.groupCode = PlanNoCorrespondingItem.BatchId;
                HisDTO.inType = PlanNoCorrespondingItem.InType;
                HisDTO.class3MeaningCode = PlanNoCorrespondingItem.Class3MeaningCode;
                HisDTO.drugCode = PlanNoCorrespondingItem.DrugCode;
                HisDTO.applyDate = PlanNoCorrespondingItem.ApplyDate;
                HisDTO.tradeName = PlanNoCorrespondingItem.TradeName;
                HisDTO.specs = PlanNoCorrespondingItem.Specs;
                HisDTO.packUnit = PlanNoCorrespondingItem.PackUnit;
                HisDTO.packQty = PlanNoCorrespondingItem.PackQty;
                HisDTO.minUnit = PlanNoCorrespondingItem.MinUnit;
                HisDTO.batchNo = PlanNoCorrespondingItem.BatchNo;
                HisDTO.validDate = PlanNoCorrespondingItem.ValidDate;
                HisDTO.producerCode = PlanNoCorrespondingItem.ProducerCode;
                HisDTO.companyCode = PlanNoCorrespondingItem.CompanyCode;
                HisDTO.retailPrice = PlanNoCorrespondingItem.RetailPrice;
                HisDTO.wholesalePrice = PlanNoCorrespondingItem.WholesalePrice;
                HisDTO.purchasePrice = PlanNoCorrespondingItem.PurchasePrice;
                HisDTO.inNum = PlanNoCorrespondingItem.InNum;
                HisDTO.retailCost = PlanNoCorrespondingItem.RetailCost;
                HisDTO.wholesaleCost = PlanNoCorrespondingItem.WholesaleCost;
                HisDTO.purchaseCost = PlanNoCorrespondingItem.PurchaseCost;
                HisDTO.specialFlag = PlanNoCorrespondingItem.SpecialFlag;
                HisDTO.inState = PlanNoCorrespondingItem.InState;
                HisDTO.applyNum = PlanNoCorrespondingItem.ApplyNum;
                HisDTO.applyOperCode = PlanNoCorrespondingItem.ApplyOperCode;
                HisDTO.examDate = PlanNoCorrespondingItem.ExamDate;
                HisDTO.examNum=PlanNoCorrespondingItem.ExamNum;
                HisDTO.approveOperCode = PlanNoCorrespondingItem.ApproveOperCode;
                HisDTO.approveDate = PlanNoCorrespondingItem.ApproveDate;
                HisDTO.operCode = PlanNoCorrespondingItem.OperCode;
                HisDTO.purcharsePriceFirsttime = PlanNoCorrespondingItem.PurcharsePriceFirsttime;
                HisDTO.isTenderOffer = PlanNoCorrespondingItem.IsTenderOffer;
                HisDTO.productionDate = PlanNoCorrespondingItem.ProductionDate;
                HisDTO.approveInfo = PlanNoCorrespondingItem.ApproveInfo;
                HisDTO.operDate = PlanNoCorrespondingItem.OperDate;
                HisDTO.invoiceDate = PlanNoCorrespondingItem.InvoiceDate;
                HisDTO.invoiceNo = PlanNoCorrespondingItem.InvoiceNo;
                pushHisList.Add(HisDTO);
                string postUrl = $"http://192.168.1.95:7800/His/PhaInput";
                //var jsonData = JsonConvert.SerializeObject(pushHisList);
                //var response = PostData(postUrl, pushHisList);
                var f = new InwarhouseHisResponseDTO();
              
                if (string.IsNullOrEmpty(HisDTO.validDate)
                    || string.IsNullOrEmpty(HisDTO.companyCode)
                    || string.IsNullOrEmpty(HisDTO.productionDate)
                    || string.IsNullOrEmpty(HisDTO.batchNo))
                {
                    //输出 为空
                    Context.Updateable<Inwarehousedetail>().SetColumns(it => new Inwarehousedetail()
                    {
                        Tstars = "存在有（有效期，生产厂家，生产日期，批号）其中数据为空，不能推送"
                    }).Where(it => it.Id == group.Id).ExecuteCommand();
                    continue;

                }
                else
                {
                        if (DateTime.Parse(HisDTO.validDate) < DateTime.Now.Date)
                        {
                        //输出错误信息
                        Context.Updateable<Inwarehousedetail>().SetColumns(it => new Inwarehousedetail()
                        {
                            Tstars = "有效期小于当前日期，不能推送"
                        }).Where(it => it.Id == group.Id).ExecuteCommand();
                        continue;
                        }     
                }
           
                using (var http = new HttpClient())
                {
                    var jsonData = JsonConvert.SerializeObject(pushHisList);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = http.PostAsync(postUrl, content).Result;
                    response.EnsureSuccessStatusCode();
                    // 获取响应内容
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    var responseDTO = JsonConvert.DeserializeObject<InwarhouseHisResponseDTO>(responseBody);
                    f = responseDTO;
                }
                if ("0".Equals(f.Code))
                {
                    Context.Updateable<Inwarehousedetail>().SetColumns(it => new Inwarehousedetail()
                    {
                        Tstars = "已推送"
                    }).Where(it => it.Id == group.Id).ExecuteCommand();
                    n++;
                }
                else
                {
                    Context.Updateable<Inwarehousedetail>().SetColumns(it => new Inwarehousedetail()
                    {
                        Tstars = "推送失败！原因是" + f.Msg
                    }).Where(it => it.Id == group.Id).ExecuteCommand();
                }
                serialNum++;
            };

            return n;
        }

        private string PostData(string Url, object Data)
        {
            using (var http = new HttpClient())
            {
                var jsonData = JsonConvert.SerializeObject(Data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = http.PostAsync(Url, content).Result;
                response.EnsureSuccessStatusCode();
                // 获取响应内容
                string responseBody = response.Content.ReadAsStringAsync().Result;
                return responseBody;
            }
        }

        /// <summary>
        /// 导出采购计划入库
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<TGInwarehouseDto> ExportList(TGInwarehouseQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .Select((it) => new TGInwarehouseDto()
                {
                    StateLabel = it.State.GetConfigValue<Model.System.SysDictData>("sys_inwarehouse_state"),
                    PlanTypeLabel = it.PlanType.GetConfigValue<Model.System.SysDictData>("sys_inwarehouse_plantype"),
                }, true)
                .ToPage(parm);

            return response;
        }

        /// <summary>
        /// 查询导出表达式
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private static Expressionable<TGInwarehouse> QueryExp(TGInwarehouseQueryDto parm)
        {
            var predicate = Expressionable.Create<TGInwarehouse>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.State), it => it.State == parm.State);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.BillCode), it => it.BillCode == parm.BillCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.PlanType), it => it.PlanType == parm.PlanType);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.planNo), it => it.PlanNo == decimal.Parse(parm.planNo.Trim()));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.Pname), it => it.CompanyName.Contains(parm.Pname));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugCode), it => it.DrugCode.Contains(parm.DrugCode));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugName), it => it.TradeName.Contains(parm.DrugName));
            return predicate;
        }
    }
}