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
        public TGInwarehouse GetInfo(int Id)
        {
            var response = Queryable()
                .Where(x => x.Id == Id)
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
        public int UpdateTGInwarehouse(TGInwarehouse model)
        {
            return Update(model, true);
        }

        public object PushInwarehouseInfoToHis(TGInwarehouseQueryDto parm)
        {
            List<Inwarehouse> iwarehouseList = Context.Queryable<Inwarehouse>().Where(it => parm.BillCodes.Contains(it.InwarehouseNum)).ToList();
            iwarehouseList.ForEach((item) =>
            {
                List<InwarhouseHisDTO> pushHisList = new List<InwarhouseHisDTO>();
                //
                List<string> planNoListItem = item.PlanNo.Split(",").ToList();
                planNoListItem.ForEach((planNoItem) => 
                {
                    InwarhouseDetailDTO PlanNoCorrespondingItem = Context.Queryable<Inwarehousedetail>()
                    .LeftJoin<TGInwarehouse>((id,ti) => id.SerialNum == ti.PlanNo)
                    .Where((id, ti) => id.SerialNum.Equals(planNoItem))
                    .Select((id, ti) => new InwarhouseDetailDTO
                    {
                        PlanNo = int.Parse(ti.PlanNo),
                        BillCode = ti.BillCode,
                        StockNo = ti.StockNo,
                        SerialCode = 1,
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
                        //供货单位代码待定
                        CompanyCode = "",
                        RetailPrice = decimal.Parse(ti.RetailPrice),
                        WholesalePrice = decimal.Parse(ti.WholesalePrice),
                        PurchasePrice = decimal.Parse(ti.PurchasePrice),
                        InNum = int.Parse(ti.StockNum),
                        //待定
                        RetailCost = decimal.Parse(ti.RetailPrice) * int.Parse(ti.StockNum),
                        WholesaleCost = decimal.Parse(ti.WholesalePrice) * int.Parse(ti.StockNum),
                        PurchaseCost = decimal.Parse(ti.PurchasePrice) * int.Parse(ti.StockNum),
                        SpecialFlag = "0",
                        InState = "2",
                        ApplyNum = int.Parse(ti.StockNum),
                        //待定
                        ApplyOperCode = "",
                        ApplyDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        ExamNum = int.Parse(ti.StockNum),
                        ExamOperCode = "",
                        ExamDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        ApproveOperCode = "",
                        ApproveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        OperCode = "",
                        OperDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        Mark = ti.Mark,
                        PurcharsePriceFirsttime = decimal.Parse(ti.PurchasePrice),
                        IsTenderOffer = "0",
                        //待定
                        ProductionDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        //待定
                        ApproveInfo = "",
                        SerialNum = id.SerialNum,
                        BatchId = id.BatchId,
                    })
                    .Single();
                    InwarhouseHisDTO HisDTO = new InwarhouseHisDTO();
                    HisDTO.planNo = int.Parse(planNoItem);
                    HisDTO.billCode = PlanNoCorrespondingItem.BillCode;
                    HisDTO.stockNo = PlanNoCorrespondingItem.StockNo;
                    HisDTO.serialCode = 1;
                    HisDTO.drugDeptCode = PlanNoCorrespondingItem.DrugDeptCode;
                    HisDTO.groupCode = PlanNoCorrespondingItem.BatchId;
                    HisDTO.inType = PlanNoCorrespondingItem.InType;
                    HisDTO.class3MeaningCode = PlanNoCorrespondingItem.Class3MeaningCode;
                    HisDTO.drugCode = PlanNoCorrespondingItem.DrugCode;
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
                    HisDTO.approveOperCode = PlanNoCorrespondingItem.ApproveOperCode;
                    HisDTO.approveDate = PlanNoCorrespondingItem.ApproveDate;
                    HisDTO.operCode = PlanNoCorrespondingItem.OperCode;
                    HisDTO.purcharsePriceFirsttime = PlanNoCorrespondingItem.PurcharsePriceFirsttime;
                    HisDTO.isTenderOffer = PlanNoCorrespondingItem.IsTenderOffer;
                    HisDTO.productionDate = PlanNoCorrespondingItem.ProductionDate;
                    HisDTO.approveInfo = PlanNoCorrespondingItem.ApproveInfo;
                    pushHisList.Add(HisDTO);
                });
                string postUrl = $"http://192.168.2.21:9403/His/PhaInput";
                var response = PostData(postUrl, pushHisList);
                var responseDTO = JsonConvert.DeserializeObject<InwarhouseHisResponseDTO>(response);
                if ("1".Equals(responseDTO.Code)) 
                {
                    Context.Updateable<Inwarehouse>().UpdateColumns(it => new Inwarehouse()
                    {
                        PushStatu = "1"
                    }).Where(it => it.InwarehouseNum == item.InwarehouseNum).ExecuteCommand();
                }
            });
            return null;
        }

        private string PostData(string Url,Object Data)
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
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.PlanType), it => it.PlanType == parm.PlanType);
            return predicate;
        }
    }
}