using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.GuiHis

{
    // http://192.168.2.21:9403/His/PhaInput
    //入库接口
    [SugarTable("PhaInput")]
    public class PhaInput
    {
        public decimal PlanNo { get; set; } // 入库计划流水号
        public string BillCode { get; set; } // 采购单号
        public string StockNo { get; set; } // 采购流水号
        public int SerialCode { get; set; } // 序号
        public string DrugDeptCode { get; set; } // 库存科室
        public string GroupCode { get; set; } // 批次号
        public string InType { get; set; } // 入库类型
        public string Class3MeaningCode { get; set; } // 入库分类
        public string DrugCode { get; set; } // 药品编码
        public string TradeName { get; set; } // 药品商品名
        public string Specs { get; set; } // 规格
        public string PackUnit { get; set; } // 包装单位
        public int PackQty { get; set; } // 包装数
        public string MinUnit { get; set; } // 最小单位
        public string BatchNo { get; set; } // 批号
        public DateTime ValidDate { get; set; } // 有效期
        public string ProducerCode { get; set; } // 生产厂家
        public string CompanyCode { get; set; } // 供货单位代码
        public decimal RetailPrice { get; set; } // 零售价
        public decimal WholesalePrice { get; set; } // 批发价
        public decimal PurchasePrice { get; set; } // 购入价
        public decimal InNum { get; set; } // 入库数量
        public decimal RetailCost { get; set; } // 零售金额
        public decimal WholesaleCost { get; set; } // 批发金额
        public decimal PurchaseCost { get; set; } // 购入金额
        public string SpecialFlag { get; set; } // 特殊标记
        public string InState { get; set; } // 入库状态
        public decimal ApplyNum { get; set; } // 申请入库量
        public string ApplyOperCode { get; set; } // 申请入库操作员
        public DateTime ApplyDate { get; set; } // 申请入库日期
        public decimal ExamNum { get; set; } // 审批数量
        public string ExamOperCode { get; set; } // 审批人
        public DateTime ExamDate { get; set; } // 审批日期
        public string ApproveOperCode { get; set; } // 核准人
        public DateTime ApproveDate { get; set; } // 核准日期
        public string PlaceCode { get; set; } // 货位码
        public string InvoiceNo { get; set; } // 发票号
        public string OperCode { get; set; } // 操作员
        public DateTime? OperDate { get; set; } // 操作日期
        public string Mark { get; set; } // 备注
        public string ExtCode1 { get; set; } // 扩展字段1
        public string ExtCode2 { get; set; } // 扩展字段2
        public decimal PurcharsePriceFirsttime { get; set; } // 一般入库时的购入价
        public string IsTenderOffer { get; set; } // 招标标记
        public DateTime? InvoiceDate { get; set; } // 发票上的发票日期
        public DateTime ProductionDate { get; set; } // 生产日期
        public string ApproveInfo { get; set; } // 批文信息
        public string TracCode { get; set; } // 药品追溯码
        public string CaseCode { get; set; } // 箱码
    }


    public class PhaInputReturn
    {
        public string code { get; set; }

        public string msg { get; set; }
    }
}
