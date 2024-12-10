namespace ZR.Model.GuiHis.Dto
{
    /// <summary>
    /// 入库详情查询对象
    /// </summary>
    public class PhaInputQueryDto : PagerInfo
    {
        public decimal? PlanNo { get; set; }
        public string BillCode { get; set; }
        public string StockNo { get; set; }
        public int? SerialCode { get; set; }
        public string DrugDeptCode { get; set; }
        public string GroupCode { get; set; }
        public string InType { get; set; }
        public string DrugCode { get; set; }
        public string TradeName { get; set; }
    }

    /// <summary>
    /// 入库详情输入输出对象
    /// </summary>
    public class PhaInputDto
    {
        [Required(ErrorMessage = "入库计划流水号不能为空")]
        [ExcelColumn(Name = "入库计划流水号")]
        [ExcelColumnName("入库计划流水号")]
        public decimal PlanNo { get; set; }

        [Required(ErrorMessage = "采购单号不能为空")]
        [ExcelColumn(Name = "采购单号")]
        [ExcelColumnName("采购单号")]
        public string BillCode { get; set; }

        [Required(ErrorMessage = "采购流水号不能为空")]
        [ExcelColumn(Name = "采购流水号")]
        [ExcelColumnName("采购流水号")]
        public string StockNo { get; set; }

        [Required(ErrorMessage = "序号不能为空")]
        [ExcelColumn(Name = "序号")]
        [ExcelColumnName("序号")]
        public int SerialCode { get; set; }

        [Required(ErrorMessage = "库存科室不能为空")]
        [ExcelColumn(Name = "库存科室")]
        [ExcelColumnName("库存科室")]
        public string DrugDeptCode { get; set; }

        [Required(ErrorMessage = "批次号不能为空")]
        [ExcelColumn(Name = "批次号")]
        [ExcelColumnName("批次号")]
        public string GroupCode { get; set; }

        [Required(ErrorMessage = "入库类型不能为空")]
        [ExcelColumn(Name = "入库类型")]
        [ExcelColumnName("入库类型")]
        public string InType { get; set; }

        [Required(ErrorMessage = "入库分类不能为空")]
        [ExcelColumn(Name = "入库分类")]
        [ExcelColumnName("入库分类")]
        public string Class3MeaningCode { get; set; }

        [Required(ErrorMessage = "药品编码不能为空")]
        [ExcelColumn(Name = "药品编码")]
        [ExcelColumnName("药品编码")]
        public string DrugCode { get; set; }

        [Required(ErrorMessage = "药品商品名不能为空")]
        [ExcelColumn(Name = "药品商品名")]
        [ExcelColumnName("药品商品名")]
        public string TradeName { get; set; }

        [Required(ErrorMessage = "规格不能为空")]
        [ExcelColumn(Name = "规格")]
        [ExcelColumnName("规格")]
        public string Specs { get; set; }

        [Required(ErrorMessage = "包装单位不能为空")]
        [ExcelColumn(Name = "包装单位")]
        [ExcelColumnName("包装单位")]
        public string PackUnit { get; set; }

        [Required(ErrorMessage = "包装数不能为空")]
        [ExcelColumn(Name = "包装数")]
        [ExcelColumnName("包装数")]
        public int PackQty { get; set; }

        [Required(ErrorMessage = "最小单位不能为空")]
        [ExcelColumn(Name = "最小单位")]
        [ExcelColumnName("最小单位")]
        public string MinUnit { get; set; }

        [Required(ErrorMessage = "批号不能为空")]
        [ExcelColumn(Name = "批号")]
        [ExcelColumnName("批号")]
        public string BatchNo { get; set; }

        [Required(ErrorMessage = "有效期不能为空")]
        [ExcelColumn(Name = "有效期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("有效期")]
        public DateTime? ValidDate { get; set; }

        [Required(ErrorMessage = "生产厂家不能为空")]
        [ExcelColumn(Name = "生产厂家")]
        [ExcelColumnName("生产厂家")]
        public string ProducerCode { get; set; }

        [Required(ErrorMessage = "供货单位代码不能为空")]
        [ExcelColumn(Name = "供货单位代码")]
        [ExcelColumnName("供货单位代码")]
        public string CompanyCode { get; set; }

        [Required(ErrorMessage = "零售价不能为空")]
        [ExcelColumn(Name = "零售价")]
        [ExcelColumnName("零售价")]
        public decimal RetailPrice { get; set; }

        [Required(ErrorMessage = "批发价不能为空")]
        [ExcelColumn(Name = "批发价")]
        [ExcelColumnName("批发价")]
        public decimal WholesalePrice { get; set; }

        [Required(ErrorMessage = "购入价不能为空")]
        [ExcelColumn(Name = "购入价")]
        [ExcelColumnName("购入价")]
        public decimal PurchasePrice { get; set; }

        [Required(ErrorMessage = "入库数量不能为空")]
        [ExcelColumn(Name = "入库数量")]
        [ExcelColumnName("入库数量")]
        public decimal InNum { get; set; }

        [Required(ErrorMessage = "零售金额不能为空")]
        [ExcelColumn(Name = "零售金额")]
        [ExcelColumnName("零售金额")]
        public decimal RetailCost { get; set; }

        [Required(ErrorMessage = "批发金额不能为空")]
        [ExcelColumn(Name = "批发金额")]
        [ExcelColumnName("批发金额")]
        public decimal WholesaleCost { get; set; }

        [Required(ErrorMessage = "购入金额不能为空")]
        [ExcelColumn(Name = "购入金额")]
        [ExcelColumnName("购入金额")]
        public decimal PurchaseCost { get; set; }

        [Required(ErrorMessage = "特殊标记不能为空")]
        [ExcelColumn(Name = "特殊标记")]
        [ExcelColumnName("特殊标记")]
        public string SpecialFlag { get; set; }

        [Required(ErrorMessage = "入库状态不能为空")]
        [ExcelColumn(Name = "入库状态")]
        [ExcelColumnName("入库状态")]
        public string InState { get; set; }

        [Required(ErrorMessage = "申请入库量不能为空")]
        [ExcelColumn(Name = "申请入库量")]
        [ExcelColumnName("申请入库量")]
        public decimal ApplyNum { get; set; }

        [Required(ErrorMessage = "申请入库操作员不能为空")]
        [ExcelColumn(Name = "申请入库操作员")]
        [ExcelColumnName("申请入库操作员")]
        public string ApplyOperCode { get; set; }

        [Required(ErrorMessage = "申请入库日期不能为空")]
        [ExcelColumn(Name = "申请入库日期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("申请入库日期")]
        public DateTime? ApplyDate { get; set; }

        [Required(ErrorMessage = "审批数量不能为空")]
        [ExcelColumn(Name = "审批数量")]
        [ExcelColumnName("审批数量")]
        public decimal ExamNum { get; set; }

        [Required(ErrorMessage = "审批人不能为空")]
        [ExcelColumn(Name = "审批人")]
        [ExcelColumnName("审批人")]
        public string ExamOperCode { get; set; }

        [Required(ErrorMessage = "审批日期不能为空")]
        [ExcelColumn(Name = "审批日期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("审批日期")]
        public DateTime? ExamDate { get; set; }

        [Required(ErrorMessage = "核准人不能为空")]
        [ExcelColumn(Name = "核准人")]
        [ExcelColumnName("核准人")]
        public string ApproveOperCode { get; set; }

        [Required(ErrorMessage = "核准日期不能为空")]
        [ExcelColumn(Name = "核准日期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("核准日期")]
        public DateTime? ApproveDate { get; set; }

        [ExcelColumn(Name = "货位码")]
        [ExcelColumnName("货位码")]
        public string PlaceCode { get; set; }

        [ExcelColumn(Name = "发票号")]
        [ExcelColumnName("发票号")]
        public string InvoiceNo { get; set; }

        [Required(ErrorMessage = "操作员不能为空")]
        [ExcelColumn(Name = "操作员")]
        [ExcelColumnName("操作员")]
        public string OperCode { get; set; }

        [ExcelColumn(Name = "操作日期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("操作日期")]
        public DateTime? OperDate { get; set; }

        [ExcelColumn(Name = "备注")]
        [ExcelColumnName("备注")]
        public string Mark { get; set; }

        [ExcelColumn(Name = "扩展字段1")]
        [ExcelColumnName("扩展字段1")]
        public string ExtCode1 { get; set; }

        [ExcelColumn(Name = "扩展字段2")]
        [ExcelColumnName("扩展字段2")]
        public string ExtCode2 { get; set; }

        [Required(ErrorMessage = "一般入库时的购入价不能为空")]
        [ExcelColumn(Name = "一般入库时的购入价")]
        [ExcelColumnName("一般入库时的购入价")]
        public decimal PurcharsePriceFirsttime { get; set; }

        [Required(ErrorMessage = "招标标记不能为空")]
        [ExcelColumn(Name = "招标标记")]
        [ExcelColumnName("招标标记")]
        public string IsTenderOffer { get; set; }

        [ExcelColumn(Name = "发票上的发票日期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("发票上的发票日期")]
        public DateTime? InvoiceDate { get; set; }

        [Required(ErrorMessage = "生产日期不能为空")]
        [ExcelColumn(Name = "生产日期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("生产日期")]
        public DateTime? ProductionDate { get; set; }

        [Required(ErrorMessage = "批文信息不能为空")]
        [ExcelColumn(Name = "批文信息")]
        [ExcelColumnName("批文信息")]
        public string ApproveInfo { get; set; }

        [ExcelColumn(Name = "药品追溯码，多追溯码用|隔开，如A|B")]
        [ExcelColumnName("药品追溯码，多追溯码用|隔开，如A|B")]
        public string TracCode { get; set; }

        [ExcelColumn(Name = "箱码")]
        [ExcelColumnName("箱码")]
        public string CaseCode { get; set; }



        [ExcelColumn(Name = "入库类型")]
        public string InTypeLabel { get; set; }
    }
}