
namespace ZR.Model.Business
{
    /// <summary>
    /// 采购退货
    /// </summary>
    [SugarTable("OutDrugs")]
    public class OutDrugs
    {
        /// <summary>
        /// OutorderID 
        /// </summary>
        public int? OutorderID { get; set; }

        /// <summary>
        /// 库存科室 0-全部部门 
        /// </summary>
        public string DrugDeptCode { get; set; }

        /// <summary>
        /// 入库单流水号 
        /// </summary>
        public long? InBillCode { get; set; }

        /// <summary>
        /// 序号 
        /// </summary>
        public int? SerialCode { get; set; }

        /// <summary>
        /// 批次号 
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 入库单据号 
        /// </summary>
        public string InListCode { get; set; }

        /// <summary>
        /// 入库类型 
        /// </summary>
        public string InType { get; set; }

        /// <summary>
        /// 入库分类 
        /// </summary>
        public string Class3MeaningCode { get; set; }

        /// <summary>
        /// 出库单号 
        /// </summary>
        public long? OutBillCode { get; set; }

        /// <summary>
        /// 出库单序号 
        /// </summary>
        public int? OutSerialCode { get; set; }

        /// <summary>
        /// 出库单据号 
        /// </summary>
        public string OutListCode { get; set; }

        /// <summary>
        /// 药品编码 
        /// </summary>
        public string DrugCode { get; set; }

        /// <summary>
        /// 药品商品名 
        /// </summary>
        public string TradeName { get; set; }

        /// <summary>
        /// 药品类别 
        /// </summary>
        public string DrugType { get; set; }

        /// <summary>
        /// 药品性质 
        /// </summary>
        public string DrugQuality { get; set; }

        /// <summary>
        /// 规格 
        /// </summary>
        public string Specs { get; set; }

        /// <summary>
        /// 包装单位 
        /// </summary>
        public string PackUnit { get; set; }

        /// <summary>
        /// 包装数 
        /// </summary>
        public int? PackQty { get; set; }

        /// <summary>
        /// 最小单位 
        /// </summary>
        public string MinUnit { get; set; }

        /// <summary>
        /// 显示的单位标记 
        /// </summary>
        public string ShowFlag { get; set; }

        /// <summary>
        /// 显示的单位 
        /// </summary>
        public string ShowUnit { get; set; }

        /// <summary>
        /// 批号 
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 有效期 
        /// </summary>
        public DateTime? ValidDate { get; set; }

        /// <summary>
        /// 生产厂家 
        /// </summary>
        public string ProducerCode { get; set; }

        /// <summary>
        /// 供货单位代码 
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        /// 零售价 
        /// </summary>
        public decimal RetailPrice { get; set; }

        /// <summary>
        /// 批发价 
        /// </summary>
        public decimal WholesalePrice { get; set; }

        /// <summary>
        /// 购入价 
        /// </summary>
        public decimal PurchasePrice { get; set; }

        /// <summary>
        /// 退货数量（负数） 
        /// </summary>
        public decimal InNum { get; set; }

        /// <summary>
        /// 零售金额 
        /// </summary>
        public decimal RetailCost { get; set; }

        /// <summary>
        /// 批发金额 
        /// </summary>
        public decimal WholesaleCost { get; set; }

        /// <summary>
        /// 购入金额 
        /// </summary>
        public decimal PurchaseCost { get; set; }

        /// <summary>
        /// 入库后库存数量 
        /// </summary>
        public decimal StoreCum { get; set; }

        /// <summary>
        /// 入库后库存总金额 
        /// </summary>
        public decimal StoreCost { get; set; }

        /// <summary>
        /// 特殊标记，1是，0否 
        /// </summary>
        public string SpecialFlag { get; set; }

        /// <summary>
        /// 入库状态，0-申请，1-审批（发票入库），2-核准 
        /// </summary>
        public string InState { get; set; }

        /// <summary>
        /// 申请入库量 
        /// </summary>
        public decimal ApplyNum { get; set; }

        /// <summary>
        /// 申请入库操作员 
        /// </summary>
        public string ApplyOpercode { get; set; }

        /// <summary>
        /// 申请入库日期 
        /// </summary>
        public DateTime? ApplyDate { get; set; }

        /// <summary>
        /// 审批数量 
        /// </summary>
        public decimal ExamNum { get; set; }

        /// <summary>
        /// 审批人（药库发票入库人） 
        /// </summary>
        public string ExamOpercode { get; set; }

        /// <summary>
        /// 审批日期（药库发票入库日期） 
        /// </summary>
        public DateTime? ExamDate { get; set; }

        /// <summary>
        /// 核准人 
        /// </summary>
        public string ApproveOpercode { get; set; }

        /// <summary>
        /// 核准日期 
        /// </summary>
        public DateTime? ApproveDate { get; set; }

        /// <summary>
        /// 货位码 
        /// </summary>
        public string PlaceCode { get; set; }

        /// <summary>
        /// 退库数量 
        /// </summary>
        public decimal ReturnNum { get; set; }

        /// <summary>
        /// 申请序号 
        /// </summary>
        public long? ApplyNumber { get; set; }

        /// <summary>
        /// 制剂序号－生产序号或检验序号 
        /// </summary>
        public string MedId { get; set; }

        /// <summary>
        /// 发票号 
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 送药单流水号 
        /// </summary>
        public string DeliveryNo { get; set; }

        /// <summary>
        /// 招标序号－招标单的序号 
        /// </summary>
        public string TenderNo { get; set; }

        /// <summary>
        /// 实际扣率 
        /// </summary>
        public decimal ActualRate { get; set; }

        /// <summary>
        /// 现金标志 
        /// </summary>
        public string CashFlag { get; set; }

        /// <summary>
        /// 供货商结存付款状态 0 未付 1 未全付 2 付清 
        /// </summary>
        public string PayState { get; set; }

        /// <summary>
        /// 操作员 
        /// </summary>
        public string OperCode { get; set; }

        /// <summary>
        /// 操作日期 
        /// </summary>
        public DateTime? OperDate { get; set; }

        /// <summary>
        /// 备注 
        /// </summary>
        public string Mark { get; set; }

        /// <summary>
        /// 扩展字段 
        /// </summary>
        public string ExtCode { get; set; }

        /// <summary>
        /// 扩展字段1 存储草药产地 
        /// </summary>
        public string ExtCode1 { get; set; }

        /// <summary>
        /// 扩展字段2 存储生产日期 
        /// </summary>
        public string ExtCode2 { get; set; }

        /// <summary>
        /// 一般入库时的购入价 
        /// </summary>
        public decimal PurcharsePriceFirsttime { get; set; }

        /// <summary>
        /// 招标标记，1是，0否 
        /// </summary>
        public string IsTenderOffer { get; set; }

        /// <summary>
        /// 发票上的发票日期 
        /// </summary>
        public DateTime? InvoiceDate { get; set; }

        /// <summary>
        /// 入库时间，即实际入库发生时间 
        /// </summary>
        public DateTime? InDate { get; set; }

        /// <summary>
        /// 源科室（供货单位）类别 1 院内科室 2 供货单位 3 扩展 
        /// </summary>
        public string SourceCompanyType { get; set; }

        /// <summary>
        /// 生产日期 
        /// </summary>
        public DateTime? ProductionDate { get; set; }

    }
}