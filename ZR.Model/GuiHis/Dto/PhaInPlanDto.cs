namespace ZR.Model.GuiHis.Dto
{
    /// <summary>
    /// 入库计划查询对象
    /// </summary>
    public class PhaInPlanQueryDto : PagerInfo
    {
        public decimal? PlanNo { get; set; }
        public string BillCode { get; set; }
        public string State { get; set; }
        public string PlanType { get; set; }
        public string DrugDeptCode { get; set; }
        public string DrugCode { get; set; }
        public string TradeName { get; set; }
        public DateTime? BeginPlanDate { get; set; }
        public DateTime? EndPlanDate { get; set; }
        public DateTime? BeginStockDate { get; set; }
        public DateTime? EndStockDate { get; set; }
        public string StockNo { get; set; }
        public DateTime? BeginOperDate { get; set; }
        public DateTime? EndOperDate { get; set; }
    }

    /// <summary>
    /// 入库计划输入输出对象
    /// </summary>
    public class PhaInPlanDto
    {
        [Required(ErrorMessage = "入库计划流水号不能为空")]
        [ExcelColumn(Name = "入库计划流水号")]
        [ExcelColumnName("入库计划流水号")]
        public decimal PlanNo { get; set; }
        public string ApproveDate { get; set; }
        public string ApproveEmpl { get; set; }
        public decimal StockNum { get; set; }
        [Required(ErrorMessage = "采购单号不能为空")]
        [ExcelColumn(Name = "采购单号")]
        [ExcelColumnName("采购单号")]
        public string BillCode { get; set; }

        [ExcelColumn(Name = "单据状态 0 计划单，1 采购单")]
        [ExcelColumnName("单据状态 0 计划单，1 采购单")]
        public string State { get; set; }

        [ExcelColumn(Name = "计划类型0手工计划，1警戒线，2消耗，3时间，4日消耗")]
        [ExcelColumnName("计划类型0手工计划，1警戒线，2消耗，3时间，4日消耗")]
        public string PlanType { get; set; }

        [Required(ErrorMessage = "科室编码不能为空")]
        [ExcelColumn(Name = "科室编码")]
        [ExcelColumnName("科室编码")]
        public string DrugDeptCode { get; set; }

        [Required(ErrorMessage = "药品编码不能为空")]
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
        public decimal RetailPrice { get; set; }

        [ExcelColumn(Name = "参考批发价")]
        [ExcelColumnName("参考批发价")]
        public decimal WholesalePrice { get; set; }

        [ExcelColumn(Name = "最新购入价")]
        [ExcelColumnName("最新购入价")]
        public decimal PurchasePrice { get; set; }

        [ExcelColumn(Name = "包装单位")]
        [ExcelColumnName("包装单位")]
        public string PackUnit { get; set; }

        [ExcelColumn(Name = "包装数量")]
        [ExcelColumnName("包装数量")]
        public decimal PackQty { get; set; }

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
        public decimal StoreNum { get; set; }

        [ExcelColumn(Name = "全院库存总和")]
        [ExcelColumnName("全院库存总和")]
        public decimal StoreTotsum { get; set; }

        [ExcelColumn(Name = "全院出库总量")]
        [ExcelColumnName("全院出库总量")]
        public decimal OutputSum { get; set; }

        [ExcelColumn(Name = "计划入库量")]
        [ExcelColumnName("计划入库量")]
        public decimal PlanNum { get; set; }

        [ExcelColumn(Name = "计划人")]
        [ExcelColumnName("计划人")]
        public string PlanEmpl { get; set; }

        [ExcelColumn(Name = "计划日期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("计划日期")]
        public DateTime? PlanDate { get; set; }

        [ExcelColumn(Name = "采购人")]
        [ExcelColumnName("采购人")]
        public string StockEmpl { get; set; }

        [ExcelColumn(Name = "采购日期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("采购日期")]
        public DateTime? StockDate { get; set; }

        [ExcelColumn(Name = "采购流水号")]
        [ExcelColumnName("采购流水号")]
        public string StockNo { get; set; }

        [ExcelColumn(Name = "作废、替代计划单流水号")]
        [ExcelColumnName("作废、替代计划单流水号")]
        public string ReplacePlanNo { get; set; }

        [ExcelColumn(Name = "备注")]
        [ExcelColumnName("备注")]
        public string Mark { get; set; }

        [ExcelColumn(Name = "操作员")]
        [ExcelColumnName("操作员")]
        public string OperCode { get; set; }

        [ExcelColumn(Name = "操作日期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("操作日期")]
        public DateTime? OperDate { get; set; }

        [ExcelColumn(Name = "扩展字段")]
        [ExcelColumnName("扩展字段")]
        public string ExtendField { get; set; }



        [ExcelColumn(Name = "单据状态 0 计划单，1 采购单")]
        public string StateLabel { get; set; }
    }
}