
namespace ZR.Model.Business.Dto
{
    /// <summary>
    /// 采购计划入库查询对象
    /// </summary>
    public class TGInwarehouseQueryDto : PagerInfo 
    {
        public string State { get; set; }
        public string PlanType { get; set; }
        public string BillCode { get; set; }
        public string Pname { get; set; }
        public string planNo { get; set; }
        public string DrugName { get; set; }
        public string DrugCode { get; set; }
        
        public string EndState { get; set; }
        public List<string> BillCodes { get; set; }
    }
    public class Push
    {
        public List<string> BillCodes { get; set; }
    }
    

    public class AddDec
    {
        public string PlanNo { get; set; }
        public string BatchNo { get; set; }
        public string ValiDate { get; set; }
        public string ProductDate { get; set; }
        public string ApproveInfo { get; set; }
        public decimal Num { get; set; }
        public decimal MixBuyPrice { get; set; }
        public decimal MixOutPrice { get; set; }
        public string ProductCode { get; set; }

        
        public int OnId { get; set; }
    }
    public class InwarhouseDetailDTO
    {
        public int PlanNo { get; set; } // 入库计划流水号
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
        public string ValidDate { get; set; } // 有效期
        public string ProducerCode { get; set; } // 生产厂家
        public string CompanyCode { get; set; } // 供货单位代码
        public decimal RetailPrice { get; set; } // 零售价
        public decimal WholesalePrice { get; set; } // 批发价
        public decimal PurchasePrice { get; set; } // 购入价
        public int InNum { get; set; } // 入库数量
        public decimal RetailCost { get; set; } // 零售金额
        public decimal WholesaleCost { get; set; } // 批发金额
        public decimal PurchaseCost { get; set; } // 购入金额
        public string SpecialFlag { get; set; } // 特殊标记
        public string InState { get; set; } // 入库状态
        public int ApplyNum { get; set; } // 申请入库量
        public string ApplyOperCode { get; set; } // 申请入库操作员
        public string ApplyDate { get; set; } // 申请入库日期
        public int ExamNum { get; set; } // 审批数量
        public string ExamOperCode { get; set; } // 审批人
        public string ExamDate { get; set; } // 审批日期
        public string ApproveOperCode { get; set; } // 核准人
        public string ApproveDate { get; set; } // 核准日期
        public string PlaceCode { get; set; } // 货位码
        public string InvoiceNo { get; set; } // 发票号
        public string OperCode { get; set; } // 操作员
        public string OperDate { get; set; } // 操作日期
        public string Mark { get; set; } // 备注
        public string ExtCode1 { get; set; } // 扩展字段1
        public string ExtCode2 { get; set; } // 扩展字段2
        public decimal PurcharsePriceFirsttime { get; set; } // 一般入库时的购入价
        public string IsTenderOffer { get; set; } // 招标标记
        public string InvoiceDate { get; set; } // 发票上的发票日期
        public string ProductionDate { get; set; } // 生产日期
        public string ApproveInfo { get; set; } // 批文信息
        public string TracCode { get; set; } // 药品追溯码
        public string CaseCode { get; set; } // 箱码
        public string Remark { get; set; }
        public string SerialNum { get; set; }
        public string BatchId { get; set; }
        public string DrugType { get; set; }
        public string DrugQuality { get; set; }
        public string ProductDate { get; set; }
        public string ValiDate { get; set; }
    }

    public class InwarhouseHisResponseDTO
    {
        public string Code { get; set; }

        public string Msg { get; set; }
    }

    public class InwarhouseHisDTO
    {
        public int planNo { get; set; } // 入库计划流水号
        public string billCode { get; set; } // 采购单号
        public string stockNo { get; set; } // 采购流水号
        public int serialCode { get; set; } // 序号
        public string drugDeptCode { get; set; } // 库存科室
        public string groupCode { get; set; } // 批次号
        public string inType { get; set; } // 入库类型
        public string class3MeaningCode { get; set; } // 入库分类
        public string drugCode { get; set; } // 药品编码
        public string tradeName { get; set; } // 药品商品名
        public string specs { get; set; } // 规格
        public string packUnit { get; set; } // 包装单位
        public int packQty { get; set; } // 包装数
        public string minUnit { get; set; } // 最小单位
        public string batchNo { get; set; } // 批号
        public string validDate { get; set; } // 有效期
        public string producerCode { get; set; } // 生产厂家
        public string companyCode { get; set; } // 供货单位代码
        public decimal retailPrice { get; set; } // 零售价
        public decimal wholesalePrice { get; set; } // 批发价
        public decimal purchasePrice { get; set; } // 购入价
        public int inNum { get; set; } // 入库数量
        public decimal retailCost { get; set; } // 零售金额
        public decimal wholesaleCost { get; set; } // 批发金额
        public decimal purchaseCost { get; set; } // 购入金额
        public string specialFlag { get; set; } // 特殊标记
        public string inState { get; set; } // 入库状态
        public int applyNum { get; set; } // 申请入库量
        public string applyOperCode { get; set; } // 申请入库操作员
        public string applyDate { get; set; } // 申请入库日期
        public int examNum { get; set; } // 审批数量
        public string examOperCode { get; set; } // 审批人
        public string examDate { get; set; } // 审批日期
        public string approveOperCode { get; set; } // 核准人
        public string approveDate { get; set; } // 核准日期
        public string placeCode { get; set; } // 货位码
        public string invoiceNo { get; set; } // 发票号
        public string operCode { get; set; } // 操作员
        public string operDate { get; set; } // 操作日期
        public string mark { get; set; } // 备注
        public string extCode1 { get; set; } // 扩展字段1
        public string extCode2 { get; set; } // 扩展字段2
        public decimal purcharsePriceFirsttime { get; set; } // 一般入库时的购入价
        public string isTenderOffer { get; set; } // 招标标记
        public string invoiceDate { get; set; } // 发票上的发票日期
        public string productionDate { get; set; } // 生产日期
        public string approveInfo { get; set; } // 批文信息
        //public string tracCode { get; set; } // 药品追溯码
        //public string caseCode { get; set; } // 箱码
    }

    /// <summary>
    /// 采购计划入库输入输出对象
    /// </summary>
    public class TGInwarehouseDto
    {
        public DateTime? EndDate { get; set; } // 生成出库单 0未生成 1已生成
        public string ApproveInfo { get; set; }

        public string CompanyCode { get; set; } // 生成出库单 0未生成 1已生成
        public string CompanyName { get; set; } // 生成出库单 0未生成 1已生成
        [ExcelColumn(Name = "自增ID")]
        [ExcelColumnName("自增ID")]
        public int? Id { get; set; }

        [ExcelColumn(Name = "入库计划流水号")]
        [ExcelColumnName("入库计划流水号")]
        public string PlanNo { get; set; }

        [ExcelColumn(Name = "采购单号")]
        [ExcelColumnName("采购单号")]
        public string BillCode { get; set; }

        [ExcelColumn(Name = "单据状态 0 计划单，1 采购单")]
        [ExcelColumnName("单据状态 0 计划单，1 采购单")]
        public string State { get; set; }

        [ExcelColumn(Name = "计划类型0手工计划，1警戒线，2消耗，3时间，4日消耗")]
        [ExcelColumnName("计划类型0手工计划，1警戒线，2消耗，3时间，4日消耗")]
        public string PlanType { get; set; }

        [ExcelColumn(Name = "科室编码")]
        [ExcelColumnName("科室编码")]
        public string DrugDeptCode { get; set; }

        [ExcelColumn(Name = "药品编码")]
        [ExcelColumnName("药品编码")]
        public string DrugCode { get; set; }

        [ExcelColumn(Name = "药品名称")]
        [ExcelColumnName("药品名称")]
        public string TradeName { get; set; }

        [ExcelColumn(Name = "规格")]
        [ExcelColumnName("规格")]
        public string Specs { get; set; }

        [ExcelColumn(Name = "参考零售价")]
        [ExcelColumnName("参考零售价")]
        public string RetailPrice { get; set; }

        [ExcelColumn(Name = "参考批发价")]
        [ExcelColumnName("参考批发价")]
        public string WholesalePrice { get; set; }

        [ExcelColumn(Name = "最新购入价")]
        [ExcelColumnName("最新购入价")]
        public string PurchasePrice { get; set; }

        [ExcelColumn(Name = "包装单位")]
        [ExcelColumnName("包装单位")]
        public string PackUnit { get; set; }

        [ExcelColumn(Name = "包装数量")]
        [ExcelColumnName("包装数量")]
        public string PackQty { get; set; }

        [ExcelColumn(Name = "最小单位")]
        [ExcelColumnName("最小单位")]
        public string MinUnit { get; set; }

        [ExcelColumn(Name = "生产厂家编码")]
        [ExcelColumnName("生产厂家编码")]
        public string ProducerCode { get; set; }

        [ExcelColumn(Name = "生产厂家名称")]
        [ExcelColumnName("生产厂家名称")]
        public string ProducerName { get; set; }

        [ExcelColumn(Name = "本科室库存数量")]
        [ExcelColumnName("本科室库存数量")]
        public string StoreNum { get; set; }

        [ExcelColumn(Name = "全院库存总和")]
        [ExcelColumnName("全院库存总和")]
        public string StoreTotsum { get; set; }

        [ExcelColumn(Name = "全院出库总量")]
        [ExcelColumnName("全院出库总量")]
        public string OutputSum { get; set; }

        [ExcelColumn(Name = "计划入库量")]
        [ExcelColumnName("计划入库量")]
        public string PlanNum { get; set; }

        [ExcelColumn(Name = "计划人")]
        [ExcelColumnName("计划人")]
        public string PlanEmpl { get; set; }

        [ExcelColumn(Name = "计划日期")]
        [ExcelColumnName("计划日期")]
        public string PlanDate { get; set; }

        [ExcelColumn(Name = "采购数量")]
        [ExcelColumnName("采购数量")]
        public string StockNum { get; set; }

        [ExcelColumn(Name = "采购人")]
        [ExcelColumnName("采购人")]
        public string StockEmpl { get; set; }

        [ExcelColumn(Name = "采购日期")]
        [ExcelColumnName("采购日期")]
        public string StockDate { get; set; }

        [ExcelColumn(Name = "审批人")]
        [ExcelColumnName("审批人")]
        public string ApproveEmpl { get; set; }

        [ExcelColumn(Name = "审批时间")]
        [ExcelColumnName("审批时间")]
        public string ApproveDate { get; set; }

        [ExcelColumn(Name = "采购流水号")]
        [ExcelColumnName("采购流水号")]
        public string StockNo { get; set; }
        public decimal? Qty { get; set; }

        [ExcelIgnore]
        public string ReplacePlanNo { get; set; }

        [ExcelIgnore]
        public string Mark { get; set; }

        [ExcelIgnore]
        public string OperCode { get; set; }

        [ExcelIgnore]
        public string OperDate { get; set; }

        [ExcelIgnore]
        public string ExtendField { get; set; }



        [ExcelColumn(Name = "单据状态 0 计划单，1 采购单")]
        public string StateLabel { get; set; }
        [ExcelColumn(Name = "计划类型0手工计划，1警戒线，2消耗，3时间，4日消耗")]
        public string PlanTypeLabel { get; set; }

        public string Status { get; set; }
    }
}