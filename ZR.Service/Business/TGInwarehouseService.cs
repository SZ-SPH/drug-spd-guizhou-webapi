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
                        ValidDate = DateTime.Now,
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
                        ApplyDate = DateTime.Now,
                        ExamNum = int.Parse(ti.StockNum),
                        ExamOperCode = "",
                        ExamDate = DateTime.Now,
                        ApproveOperCode = "",
                        ApproveDate = DateTime.Now,
                        OperCode = "",
                        OperDate = DateTime.Now,
                        Mark = ti.Mark,
                        PurcharsePriceFirsttime = decimal.Parse(ti.PurchasePrice),
                        IsTenderOffer = "0",
                        //待定
                        ProductionDate = DateTime.Now,
                        //待定
                        ApproveInfo = "",
                        SerialNum = id.SerialNum,
                        BatchId = id.BatchId,
                    })
                    .Single();
                    InwarhouseHisDTO HisDTO = new InwarhouseHisDTO();
                    HisDTO.PlanNo = int.Parse(planNoItem);
                    HisDTO.BillCode = PlanNoCorrespondingItem.BillCode;
                    HisDTO.StockNo = PlanNoCorrespondingItem.StockNo;
                    HisDTO.SerialCode = 1;
                    HisDTO.DrugDeptCode = PlanNoCorrespondingItem.DrugDeptCode;
                    HisDTO.GroupCode = PlanNoCorrespondingItem.BatchId;
                    HisDTO.InType = PlanNoCorrespondingItem.InType;
                    HisDTO.Class3MeaningCode = PlanNoCorrespondingItem.Class3MeaningCode;
                    HisDTO.DrugCode = PlanNoCorrespondingItem.DrugCode;
                    HisDTO.TradeName = PlanNoCorrespondingItem.TradeName;
                    HisDTO.Specs = PlanNoCorrespondingItem.Specs;
                    HisDTO.PackUnit = PlanNoCorrespondingItem.PackUnit;
                    HisDTO.PackQty = PlanNoCorrespondingItem.PackQty;
                    HisDTO.MinUnit = PlanNoCorrespondingItem.MinUnit;
                    HisDTO.BatchNo = PlanNoCorrespondingItem.BatchNo;
                    HisDTO.ValidDate = PlanNoCorrespondingItem.ValidDate;
                    HisDTO.ProducerCode = PlanNoCorrespondingItem.ProducerCode;
                    HisDTO.CompanyCode = PlanNoCorrespondingItem.CompanyCode;
                    HisDTO.RetailPrice = PlanNoCorrespondingItem.RetailPrice;
                    HisDTO.WholesalePrice = PlanNoCorrespondingItem.WholesalePrice;
                    HisDTO.PurchasePrice = PlanNoCorrespondingItem.PurchasePrice;
                    HisDTO.InNum = PlanNoCorrespondingItem.InNum;
                    HisDTO.RetailCost = PlanNoCorrespondingItem.RetailCost;
                    HisDTO.WholesaleCost = PlanNoCorrespondingItem.WholesaleCost;
                    HisDTO.PurchaseCost = PlanNoCorrespondingItem.PurchaseCost;
                    HisDTO.SpecialFlag = PlanNoCorrespondingItem.SpecialFlag;
                    HisDTO.InState = PlanNoCorrespondingItem.InState;
                    HisDTO.ApplyNum = PlanNoCorrespondingItem.ApplyNum;
                    HisDTO.ApplyOperCode = PlanNoCorrespondingItem.ApplyOperCode;
                    HisDTO.ExamDate = PlanNoCorrespondingItem.ExamDate;
                    HisDTO.ApproveOperCode = PlanNoCorrespondingItem.ApproveOperCode;
                    HisDTO.ApproveDate = PlanNoCorrespondingItem.ApproveDate;
                    HisDTO.OperCode = PlanNoCorrespondingItem.OperCode;
                    HisDTO.PurcharsePriceFirsttime = PlanNoCorrespondingItem.PurcharsePriceFirsttime;
                    HisDTO.IsTenderOffer = PlanNoCorrespondingItem.IsTenderOffer;
                    HisDTO.ProductionDate = PlanNoCorrespondingItem.ProductionDate;
                    HisDTO.ApproveInfo = PlanNoCorrespondingItem.ApproveInfo;
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