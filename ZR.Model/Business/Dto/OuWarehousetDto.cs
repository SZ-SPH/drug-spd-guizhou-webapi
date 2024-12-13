
namespace ZR.Model.Business.Dto
{
    /// <summary>
    /// 出库药品详情查询对象
    /// </summary>
    public class OuWarehousetQueryDto : PagerInfo 
    {
        public int? OutorderID { get; set; }
        public string DrugDeptCode { get; set; }
        public long? OutBillCode { get; set; }
        public string GroupCode { get; set; }
        public string DrugCode { get; set; }
        public string TradeName { get; set; }
    }

    /// <summary>
    /// 出库药品详情输入输出对象
    /// </summary>
    public class OuWarehousetDto
    {
        [Required(ErrorMessage = "Id不能为空")]
        [ExcelColumn(Name = "Id")]
        [ExcelColumnName("Id")]
        public int Id { get; set; }

        [ExcelColumn(Name = "OutorderID")]
        [ExcelColumnName("OutorderID")]
        public int? OutorderID { get; set; }

        [ExcelColumn(Name = "出库科室编码")]
        [ExcelColumnName("出库科室编码")]
        public string DrugDeptCode { get; set; }

        [ExcelColumn(Name = "出库单流水号")]
        [ExcelColumnName("出库单流水号")]
        public long? OutBillCode { get; set; }

        [ExcelColumn(Name = "序号")]
        [ExcelColumnName("序号")]
        public int? SerialCode { get; set; }

        [ExcelColumn(Name = "批次号")]
        [ExcelColumnName("批次号")]
        public string GroupCode { get; set; }

        [ExcelColumn(Name = "出库单据号")]
        [ExcelColumnName("出库单据号")]
        public string OutListCode { get; set; }

        [ExcelColumn(Name = "出库类型")]
        [ExcelColumnName("出库类型")]
        public string OutType { get; set; }

        [ExcelColumn(Name = "出库分类")]
        [ExcelColumnName("出库分类")]
        public string Class3MeaningCode { get; set; }

        [ExcelColumn(Name = "入库单号")]
        [ExcelColumnName("入库单号")]
        public long? InBillCode { get; set; }

        [ExcelColumn(Name = "入库单序号")]
        [ExcelColumnName("入库单序号")]
        public int? InSerialCode { get; set; }

        [ExcelColumn(Name = "入库单据号")]
        [ExcelColumnName("入库单据号")]
        public string InListCode { get; set; }

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

        [ExcelColumn(Name = "出库数量")]
        [ExcelColumnName("出库数量")]
        public decimal OutNum { get; set; }

        [ExcelColumn(Name = "零售金额")]
        [ExcelColumnName("零售金额")]
        public decimal SaleCost { get; set; }

        [ExcelColumn(Name = "批发金额")]
        [ExcelColumnName("批发金额")]
        public decimal TradeCost { get; set; }

        [ExcelColumn(Name = "购入金额")]
        [ExcelColumnName("购入金额")]
        public decimal ApproveCost { get; set; }

        [ExcelColumn(Name = "出库后库存数量")]
        [ExcelColumnName("出库后库存数量")]
        public decimal StoreNum { get; set; }

        [ExcelColumn(Name = "出库后库存总金额")]
        [ExcelColumnName("出库后库存总金额")]
        public decimal StoreCost { get; set; }

        [ExcelColumn(Name = "特殊标记")]
        [ExcelColumnName("特殊标记")]
        public string SpecialFlag { get; set; }

        [ExcelColumn(Name = "出库状态")]
        [ExcelColumnName("出库状态")]
        public string OutState { get; set; }

        [ExcelColumn(Name = "申请出库量")]
        [ExcelColumnName("申请出库量")]
        public decimal ApplyNum { get; set; }

        [ExcelColumn(Name = "申请出库人")]
        [ExcelColumnName("申请出库人")]
        public string ApplyOpercode { get; set; }

        [ExcelColumn(Name = "申请出库日期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("申请出库日期")]
        public DateTime? ApplyDate { get; set; }

        [ExcelColumn(Name = "审批数量")]
        [ExcelColumnName("审批数量")]
        public decimal ExamNum { get; set; }

        [ExcelColumn(Name = "审批人")]
        [ExcelColumnName("审批人")]
        public string ExamOpercode { get; set; }

        [ExcelColumn(Name = "审批日期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("审批日期")]
        public DateTime? ExamDate { get; set; }

        [ExcelColumn(Name = "核准人")]
        [ExcelColumnName("核准人")]
        public string ApproveOpercode { get; set; }

        [ExcelColumn(Name = "核准日期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("核准日期")]
        public DateTime? ApproveDate { get; set; }

        [ExcelColumn(Name = "货位号")]
        [ExcelColumnName("货位号")]
        public string PlaceCode { get; set; }

        [ExcelColumn(Name = "退库数量")]
        [ExcelColumnName("退库数量")]
        public decimal ReturnNum { get; set; }

        [ExcelColumn(Name = "摆药单号")]
        [ExcelColumnName("摆药单号")]
        public string DrugedBill { get; set; }

        [ExcelColumn(Name = "制剂序号")]
        [ExcelColumnName("制剂序号")]
        public string MedId { get; set; }

        [ExcelColumn(Name = "领药单位编码")]
        [ExcelColumnName("领药单位编码")]
        public string DrugStorageCode { get; set; }

        [ExcelColumn(Name = "处方号")]
        [ExcelColumnName("处方号")]
        public string RecipeNo { get; set; }

        [ExcelColumn(Name = "处方流水号")]
        [ExcelColumnName("处方流水号")]
        public int? SequenceNo { get; set; }

        [ExcelColumn(Name = "签字人")]
        [ExcelColumnName("签字人")]
        public string SignPerson { get; set; }

        [ExcelColumn(Name = "领药人")]
        [ExcelColumnName("领药人")]
        public string GetPerson { get; set; }

        [ExcelColumn(Name = "冲账标志")]
        [ExcelColumnName("冲账标志")]
        public string StrikeFlag { get; set; }

        [ExcelColumn(Name = "备注")]
        [ExcelColumnName("备注")]
        public string Mark { get; set; }

        [ExcelColumn(Name = "操作员")]
        [ExcelColumnName("操作员")]
        public string OperCode { get; set; }

        [ExcelColumn(Name = "操作日期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("操作日期")]
        public DateTime? OperDate { get; set; }

        [ExcelColumn(Name = "是否药房向药柜出库记录")]
        [ExcelColumnName("是否药房向药柜出库记录")]
        public string ArkFlag { get; set; }

        [ExcelColumn(Name = "药柜发药出库单流水号")]
        [ExcelColumnName("药柜发药出库单流水号")]
        public long? ArkBillCode { get; set; }

        [ExcelColumn(Name = "出库记录发生时间", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("出库记录发生时间")]
        public DateTime? OutDate { get; set; }

        [ExcelColumn(Name = "申请单流水号")]
        [ExcelColumnName("申请单流水号")]
        public long? ApplyNumber { get; set; }



        [ExcelColumn(Name = "出库类型")]
        public string OutTypeLabel { get; set; }
    }
}