
namespace ZR.Model.Business.Dto
{
    /// <summary>
    /// 采购退货查询对象
    /// </summary>
    public class OutDrugsQueryDto : PagerInfo 
    {
        public int? OutorderID { get; set; }
        public string drugname { get; set; }

        
    }

    /// <summary>
    /// 采购退货输入输出对象
    /// </summary>
    public class OutDrugsDto
    {
        [ExcelColumn(Name = "OutorderID")]
        [ExcelColumnName("OutorderID")]
        public int? OutorderID { get; set; }

        [ExcelColumn(Name = "库存科室 0-全部部门")]
        [ExcelColumnName("库存科室 0-全部部门")]
        public string DrugDeptCode { get; set; }

        [ExcelColumn(Name = "入库单流水号")]
        [ExcelColumnName("入库单流水号")]
        public long? InBillCode { get; set; }

        [ExcelColumn(Name = "序号")]
        [ExcelColumnName("序号")]
        public int? SerialCode { get; set; }

        [ExcelColumn(Name = "批次号")]
        [ExcelColumnName("批次号")]
        public string GroupCode { get; set; }

        [ExcelColumn(Name = "入库单据号")]
        [ExcelColumnName("入库单据号")]
        public string InListCode { get; set; }

        [ExcelColumn(Name = "入库类型")]
        [ExcelColumnName("入库类型")]
        public string InType { get; set; }

        [ExcelColumn(Name = "入库分类")]
        [ExcelColumnName("入库分类")]
        public string Class3MeaningCode { get; set; }

        [ExcelColumn(Name = "出库单号")]
        [ExcelColumnName("出库单号")]
        public long? OutBillCode { get; set; }

        [ExcelColumn(Name = "出库单序号")]
        [ExcelColumnName("出库单序号")]
        public int? OutSerialCode { get; set; }

        [ExcelColumn(Name = "出库单据号")]
        [ExcelColumnName("出库单据号")]
        public string OutListCode { get; set; }

        [ExcelColumn(Name = "药品编码")]
        [ExcelColumnName("药品编码")]
        public string DrugCode { get; set; }

        [ExcelColumn(Name = "药品商品名")]
        [ExcelColumnName("药品商品名")]
        public string TradeName { get; set; }

        [ExcelColumn(Name = "药品类别")]
        [ExcelColumnName("药品类别")]
        public string DrugType { get; set; }

        [ExcelColumn(Name = "药品性质")]
        [ExcelColumnName("药品性质")]
        public string DrugQuality { get; set; }

        [ExcelColumn(Name = "规格")]
        [ExcelColumnName("规格")]
        public string Specs { get; set; }

        [ExcelColumn(Name = "包装单位")]
        [ExcelColumnName("包装单位")]
        public string PackUnit { get; set; }

        [ExcelColumn(Name = "包装数")]
        [ExcelColumnName("包装数")]
        public int? PackQty { get; set; }

        [ExcelColumn(Name = "最小单位")]
        [ExcelColumnName("最小单位")]
        public string MinUnit { get; set; }

        [ExcelColumn(Name = "显示的单位标记")]
        [ExcelColumnName("显示的单位标记")]
        public string ShowFlag { get; set; }

        [ExcelColumn(Name = "显示的单位")]
        [ExcelColumnName("显示的单位")]
        public string ShowUnit { get; set; }

        [ExcelColumn(Name = "批号")]
        [ExcelColumnName("批号")]
        public string BatchNo { get; set; }

        [ExcelColumn(Name = "有效期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("有效期")]
        public DateTime? ValidDate { get; set; }

        [ExcelColumn(Name = "生产厂家")]
        [ExcelColumnName("生产厂家")]
        public string ProducerCode { get; set; }

        [ExcelColumn(Name = "供货单位代码")]
        [ExcelColumnName("供货单位代码")]
        public string CompanyCode { get; set; }

        [ExcelColumn(Name = "零售价")]
        [ExcelColumnName("零售价")]
        public decimal RetailPrice { get; set; }

        [ExcelColumn(Name = "批发价")]
        [ExcelColumnName("批发价")]
        public decimal WholesalePrice { get; set; }

        [ExcelColumn(Name = "购入价")]
        [ExcelColumnName("购入价")]
        public decimal PurchasePrice { get; set; }

        [ExcelColumn(Name = "退货数量（负数）")]
        [ExcelColumnName("退货数量（负数）")]
        public decimal InNum { get; set; }

        [ExcelColumn(Name = "零售金额")]
        [ExcelColumnName("零售金额")]
        public decimal RetailCost { get; set; }

        [ExcelColumn(Name = "批发金额")]
        [ExcelColumnName("批发金额")]
        public decimal WholesaleCost { get; set; }

        [ExcelColumn(Name = "购入金额")]
        [ExcelColumnName("购入金额")]
        public decimal PurchaseCost { get; set; }

        [ExcelColumn(Name = "入库后库存数量")]
        [ExcelColumnName("入库后库存数量")]
        public decimal StoreCum { get; set; }

        [ExcelColumn(Name = "入库后库存总金额")]
        [ExcelColumnName("入库后库存总金额")]
        public decimal StoreCost { get; set; }

        [ExcelColumn(Name = "特殊标记，1是，0否")]
        [ExcelColumnName("特殊标记，1是，0否")]
        public string SpecialFlag { get; set; }

        [ExcelColumn(Name = "入库状态，0-申请，1-审批（发票入库），2-核准")]
        [ExcelColumnName("入库状态，0-申请，1-审批（发票入库），2-核准")]
        public string InState { get; set; }

        [ExcelColumn(Name = "申请入库量")]
        [ExcelColumnName("申请入库量")]
        public decimal ApplyNum { get; set; }

        [ExcelColumn(Name = "申请入库操作员")]
        [ExcelColumnName("申请入库操作员")]
        public string ApplyOpercode { get; set; }

        [ExcelColumn(Name = "申请入库日期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("申请入库日期")]
        public DateTime? ApplyDate { get; set; }

        [ExcelColumn(Name = "审批数量")]
        [ExcelColumnName("审批数量")]
        public decimal ExamNum { get; set; }

        [ExcelColumn(Name = "审批人（药库发票入库人）")]
        [ExcelColumnName("审批人（药库发票入库人）")]
        public string ExamOpercode { get; set; }

        [ExcelColumn(Name = "审批日期（药库发票入库日期）", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("审批日期（药库发票入库日期）")]
        public DateTime? ExamDate { get; set; }

        [ExcelColumn(Name = "核准人")]
        [ExcelColumnName("核准人")]
        public string ApproveOpercode { get; set; }

        [ExcelColumn(Name = "核准日期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("核准日期")]
        public DateTime? ApproveDate { get; set; }

        [ExcelColumn(Name = "货位码")]
        [ExcelColumnName("货位码")]
        public string PlaceCode { get; set; }

        [ExcelColumn(Name = "退库数量")]
        [ExcelColumnName("退库数量")]
        public decimal ReturnNum { get; set; }

        [ExcelColumn(Name = "申请序号")]
        [ExcelColumnName("申请序号")]
        public long? ApplyNumber { get; set; }

        [ExcelColumn(Name = "制剂序号－生产序号或检验序号")]
        [ExcelColumnName("制剂序号－生产序号或检验序号")]
        public string MedId { get; set; }

        [ExcelColumn(Name = "发票号")]
        [ExcelColumnName("发票号")]
        public string InvoiceNo { get; set; }

        [ExcelColumn(Name = "送药单流水号")]
        [ExcelColumnName("送药单流水号")]
        public string DeliveryNo { get; set; }

        [ExcelColumn(Name = "招标序号－招标单的序号")]
        [ExcelColumnName("招标序号－招标单的序号")]
        public string TenderNo { get; set; }

        [ExcelColumn(Name = "实际扣率")]
        [ExcelColumnName("实际扣率")]
        public decimal ActualRate { get; set; }

        [ExcelColumn(Name = "现金标志")]
        [ExcelColumnName("现金标志")]
        public string CashFlag { get; set; }

        [ExcelColumn(Name = "供货商结存付款状态 0 未付 1 未全付 2 付清")]
        [ExcelColumnName("供货商结存付款状态 0 未付 1 未全付 2 付清")]
        public string PayState { get; set; }

        [ExcelColumn(Name = "操作员")]
        [ExcelColumnName("操作员")]
        public string OperCode { get; set; }

        [ExcelColumn(Name = "操作日期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("操作日期")]
        public DateTime? OperDate { get; set; }

        [ExcelColumn(Name = "备注")]
        [ExcelColumnName("备注")]
        public string Mark { get; set; }

        [ExcelColumn(Name = "扩展字段")]
        [ExcelColumnName("扩展字段")]
        public string ExtCode { get; set; }

        [ExcelColumn(Name = "扩展字段1 存储草药产地")]
        [ExcelColumnName("扩展字段1 存储草药产地")]
        public string ExtCode1 { get; set; }

        [ExcelColumn(Name = "扩展字段2 存储生产日期")]
        [ExcelColumnName("扩展字段2 存储生产日期")]
        public string ExtCode2 { get; set; }

        [ExcelColumn(Name = "一般入库时的购入价")]
        [ExcelColumnName("一般入库时的购入价")]
        public decimal PurcharsePriceFirsttime { get; set; }

        [ExcelColumn(Name = "招标标记，1是，0否")]
        [ExcelColumnName("招标标记，1是，0否")]
        public string IsTenderOffer { get; set; }

        [ExcelColumn(Name = "发票上的发票日期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("发票上的发票日期")]
        public DateTime? InvoiceDate { get; set; }

        [ExcelColumn(Name = "入库时间，即实际入库发生时间", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("入库时间，即实际入库发生时间")]
        public DateTime? InDate { get; set; }

        [ExcelColumn(Name = "源科室（供货单位）类别 1 院内科室 2 供货单位 3 扩展")]
        [ExcelColumnName("源科室（供货单位）类别 1 院内科室 2 供货单位 3 扩展")]
        public string SourceCompanyType { get; set; }

        [ExcelColumn(Name = "生产日期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("生产日期")]
        public DateTime? ProductionDate { get; set; }



        [ExcelColumn(Name = "入库类型")]
        public string InTypeLabel { get; set; }
    }
}