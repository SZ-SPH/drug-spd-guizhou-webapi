
namespace ZR.Model.Business
{
    /// <summary>
    /// 出库药品详情
    /// </summary>
    [SugarTable("OuWarehouset")]
    public class OuWarehouset
    {
        /// <summary>
        /// Id 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// OutorderID 
        /// </summary>
        public int? OutorderID { get; set; }

        /// <summary>
        /// 出库科室编码 
        /// </summary>
        public string DrugDeptCode { get; set; }

        /// <summary>
        /// 出库单流水号 
        /// </summary>
        public long? OutBillCode { get; set; }

        /// <summary>
        /// 序号 
        /// </summary>
        public int? SerialCode { get; set; }

        /// <summary>
        /// 批次号 
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 出库单据号 
        /// </summary>
        public string OutListCode { get; set; }

        /// <summary>
        /// 出库类型 
        /// </summary>
        public string OutType { get; set; }

        /// <summary>
        /// 出库分类 
        /// </summary>
        public string Class3MeaningCode { get; set; }

        /// <summary>
        /// 入库单号 
        /// </summary>
        public long? InBillCode { get; set; }

        /// <summary>
        /// 入库单序号 
        /// </summary>
        public int? InSerialCode { get; set; }

        /// <summary>
        /// 入库单据号 
        /// </summary>
        public string InListCode { get; set; }

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
        /// 出库数量 
        /// </summary>
        public decimal OutNum { get; set; }

        /// <summary>
        /// 零售金额 
        /// </summary>
        public decimal SaleCost { get; set; }

        /// <summary>
        /// 批发金额 
        /// </summary>
        public decimal TradeCost { get; set; }

        /// <summary>
        /// 购入金额 
        /// </summary>
        public decimal ApproveCost { get; set; }

        /// <summary>
        /// 出库后库存数量 
        /// </summary>
        public decimal StoreNum { get; set; }

        /// <summary>
        /// 出库后库存总金额 
        /// </summary>
        public decimal StoreCost { get; set; }

        /// <summary>
        /// 特殊标记 
        /// </summary>
        public string SpecialFlag { get; set; }

        /// <summary>
        /// 出库状态 
        /// </summary>
        public string OutState { get; set; }

        /// <summary>
        /// 申请出库量 
        /// </summary>
        public decimal ApplyNum { get; set; }

        /// <summary>
        /// 申请出库人 
        /// </summary>
        public string ApplyOpercode { get; set; }

        /// <summary>
        /// 申请出库日期 
        /// </summary>
        public DateTime? ApplyDate { get; set; }

        /// <summary>
        /// 审批数量 
        /// </summary>
        public decimal ExamNum { get; set; }

        /// <summary>
        /// 审批人 
        /// </summary>
        public string ExamOpercode { get; set; }

        /// <summary>
        /// 审批日期 
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
        /// 货位号 
        /// </summary>
        public string PlaceCode { get; set; }

        /// <summary>
        /// 退库数量 
        /// </summary>
        public decimal ReturnNum { get; set; }

        /// <summary>
        /// 摆药单号 
        /// </summary>
        public string DrugedBill { get; set; }

        /// <summary>
        /// 制剂序号 
        /// </summary>
        public string MedId { get; set; }

        /// <summary>
        /// 领药单位编码 
        /// </summary>
        public string DrugStorageCode { get; set; }

        /// <summary>
        /// 处方号 
        /// </summary>
        public string RecipeNo { get; set; }

        /// <summary>
        /// 处方流水号 
        /// </summary>
        public int? SequenceNo { get; set; }

        /// <summary>
        /// 签字人 
        /// </summary>
        public string SignPerson { get; set; }

        /// <summary>
        /// 领药人 
        /// </summary>
        public string GetPerson { get; set; }

        /// <summary>
        /// 冲账标志 
        /// </summary>
        public string StrikeFlag { get; set; }

        /// <summary>
        /// 备注 
        /// </summary>
        public string Mark { get; set; }

        /// <summary>
        /// 操作员 
        /// </summary>
        public string OperCode { get; set; }

        /// <summary>
        /// 操作日期 
        /// </summary>
        public DateTime? OperDate { get; set; }

        /// <summary>
        /// 是否药房向药柜出库记录 
        /// </summary>
        public string ArkFlag { get; set; }

        /// <summary>
        /// 药柜发药出库单流水号 
        /// </summary>
        public long? ArkBillCode { get; set; }

        /// <summary>
        /// 出库记录发生时间 
        /// </summary>
        public DateTime? OutDate { get; set; }

        /// <summary>
        /// 申请单流水号 
        /// </summary>
        public long? ApplyNumber { get; set; }

    }
}