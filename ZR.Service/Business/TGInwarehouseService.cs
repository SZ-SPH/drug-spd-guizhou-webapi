using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Service.Business.IBusinessService;
using Microsoft.AspNetCore.Http;
using Infrastructure;
using JinianNet.JNTemplate.Nodes;
using Org.BouncyCastle.Bcpg;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Numerics;
using System;
using System.Text;
using Org.BouncyCastle.Asn1.Ocsp;
using ZR.Model.GuiHis;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;
using MailKit.Search;

namespace ZR.Service.Business
{
    /// <summary>
    /// 采购计划入库Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(ITGInwarehouseService), ServiceLifetime = LifeTime.Transient)]
    public class TGInwarehouseService : BaseService<TGInwarehouse>, ITGInwarehouseService
    {
        /// <summary>
        /// 查询采购计划入库列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<TGInwarehouseDto> GetList(TGInwarehouseQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
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

        public object PushInwarehouseInfoToHis(TGInwarehouseQueryDto parm)
        {
            //按照采购单号分组推送
            string n = "";
            List<Inwarehouse> iwarehouseList = Context.Queryable<Inwarehouse>().Where(it => parm.BillCodes.Contains(it.InwarehouseNum)).ToList();
            iwarehouseList.ForEach((inwarehouseListItem) =>
            {
                List<Inwarehousedetail> inwarehouseDetailList = Context.Queryable<Inwarehousedetail>()
                .Where(it => it.InwarehouseId == inwarehouseListItem.Id)
                .GroupBy(it => it.PurchaseOrderNum)
                .Select(it => new Inwarehousedetail
                { 
                    PurchaseOrderNum = it.PurchaseOrderNum
                })
                .ToList();
                int serialNum = 1;
                inwarehouseDetailList.ForEach((inwarehouseDetailItem) =>
                {
                    var CurrentinwarehouseDetailList = Context.Queryable<Inwarehousedetail>()
                    .Where(it => it.PurchaseOrderNum == inwarehouseDetailItem.PurchaseOrderNum && it.InwarehouseId == inwarehouseListItem.Id)
                    .ToList();
                    string SerialNumStr = string.Join(",", CurrentinwarehouseDetailList.Select(o => o.SerialNum));
                    List<InwarhouseHisDTO> pushHisList = new List<InwarhouseHisDTO>();
                    var PlanNoCorrespondingItem = Context.Queryable<Inwarehousedetail>()
                    .LeftJoin<TGInwarehouse>((id, ti) => id.SerialNum == ti.PlanNo.ToString())
                    .LeftJoin<Inwarehouse>((id, ti, i) => i.Id == id.InwarehouseId)
                    .Where((id, ti, i) => SerialNumStr.Contains(id.SerialNum) && id.InwarehouseId == inwarehouseListItem.Id)
                    //.Where((id, ti, i) => id.SerialNum.Contains(SerialNumStr))
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
                        ValidDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        ProducerCode = ti.ProducerCode,
                        CompanyCode = i.SupplierCode,
                        RetailPrice = decimal.Parse(ti.RetailPrice),
                        WholesalePrice = decimal.Parse(ti.WholesalePrice),
                        PurchasePrice = decimal.Parse(ti.PurchasePrice),
                        InNum = int.Parse(ti.StockNum),
                        RetailCost = decimal.Parse(ti.RetailPrice) * int.Parse(ti.StockNum),
                        WholesaleCost = decimal.Parse(ti.WholesalePrice) * int.Parse(ti.StockNum),
                        PurchaseCost = decimal.Parse(ti.PurchasePrice) * int.Parse(ti.StockNum),
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
                        OperCode = ti.OperCode,
                        OperDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        Mark = ti.Mark,
                        PurcharsePriceFirsttime = decimal.Parse(ti.PurchasePrice),
                        IsTenderOffer = "0",
                        ProductionDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        ApproveInfo = id.ApproveInfo,
                        SerialNum = id.SerialNum,
                        BatchId = id.BatchId,
                        //发票日期
                        InvoiceDate = DateTime.Parse(i.BillTime).ToString("yyyy-MM-dd HH:mm:ss"),
                        InvoiceNo = i.BillCode,
                    })
                    .ToList();
                    PlanNoCorrespondingItem.ForEach((item) =>
                    {
                        InwarhouseHisDTO HisDTO = new InwarhouseHisDTO();
                        HisDTO.planNo = item.PlanNo;
                        HisDTO.billCode = item.BillCode;
                        HisDTO.stockNo = item.StockNo;
                        HisDTO.serialCode = item.SerialCode;
                        HisDTO.drugDeptCode = item.DrugDeptCode;
                        HisDTO.groupCode = item.BatchId;
                        HisDTO.inType = item.InType;
                        HisDTO.class3MeaningCode = item.Class3MeaningCode;
                        HisDTO.drugCode = item.DrugCode;
                        HisDTO.tradeName = item.TradeName;
                        HisDTO.specs = item.Specs;
                        HisDTO.packUnit = item.PackUnit;
                        HisDTO.packQty = item.PackQty;
                        HisDTO.minUnit = item.MinUnit;
                        HisDTO.batchNo = item.BatchNo;
                        HisDTO.validDate = item.ValidDate;
                        HisDTO.producerCode = item.ProducerCode;
                        HisDTO.companyCode = item.CompanyCode;
                        HisDTO.retailPrice = item.RetailPrice;
                        HisDTO.wholesalePrice = item.WholesalePrice;
                        HisDTO.purchasePrice = item.PurchasePrice;
                        HisDTO.inNum = item.InNum;
                        HisDTO.retailCost = item.RetailCost;
                        HisDTO.wholesaleCost = item.WholesaleCost;
                        HisDTO.purchaseCost = item.PurchaseCost;
                        HisDTO.specialFlag = item.SpecialFlag;
                        HisDTO.inState = item.InState;
                        HisDTO.applyNum = item.ApplyNum;
                        HisDTO.applyOperCode = item.ApplyOperCode;
                        HisDTO.examDate = item.ExamDate;
                        HisDTO.approveOperCode = item.ApproveOperCode;
                        HisDTO.approveDate = item.ApproveDate;
                        HisDTO.operCode = item.OperCode;
                        HisDTO.purcharsePriceFirsttime = item.PurcharsePriceFirsttime;
                        HisDTO.isTenderOffer = item.IsTenderOffer;
                        HisDTO.productionDate = item.ProductionDate;
                        HisDTO.approveInfo = item.ApproveInfo;
                        HisDTO.operDate = item.OperDate;
                        HisDTO.invoiceDate = item.InvoiceDate;
                        pushHisList.Add(HisDTO);
                        serialNum++;
                    });
                    //发送数据
                    string postUrl = $"http://192.168.2.21:9403/His/PhaInput";
                    var jsonData = JsonConvert.SerializeObject(pushHisList);
                    var response = PostData(postUrl, pushHisList);
                    var responseDTO = JsonConvert.DeserializeObject<InwarhouseHisResponseDTO>(response);
                    n = response;
                    if ("1".Equals(responseDTO.Code))
                    {
                        Context.Updateable<Inwarehouse>().UpdateColumns(it => new Inwarehouse()
                        {
                            PushStatu = "1"
                        }).Where(it => it.InwarehouseNum == inwarehouseListItem.InwarehouseNum).ExecuteCommand();
                    }
                });
            });
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
            return predicate;
        }
    }
}