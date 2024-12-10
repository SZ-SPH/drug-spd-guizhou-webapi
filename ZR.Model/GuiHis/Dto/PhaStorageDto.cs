namespace ZR.Model.GuiHis.Dto
{
    /// <summary>
    /// 库存查询对象
    /// </summary>
    public class PhaStorageQueryDto : PagerInfo
    {
        public string DrugDeptCode { get; set; }
        public string DrugCode { get; set; }
        public string TradeName { get; set; }
        public string DrugType { get; set; }
        public string PlaceCode { get; set; }
        public string ProducerCode { get; set; }
    }

    /// <summary>
    /// 库存输入输出对象
    /// </summary>
    public class PhaStorageDto
    {
        [Required(ErrorMessage = "库存科室不能为空")]
        [ExcelColumn(Name = "库存科室")]
        [ExcelColumnName("库存科室")]
        public string DrugDeptCode { get; set; }

        [Required(ErrorMessage = "药品编码不能为空")]
        [ExcelColumn(Name = "药品编码")]
        [ExcelColumnName("药品编码")]
        public string DrugCode { get; set; }

        [Required(ErrorMessage = "药品商品名不能为空")]
        [ExcelColumn(Name = "药品商品名")]
        [ExcelColumnName("药品商品名")]
        public string TradeName { get; set; }

        [ExcelColumn(Name = "规格")]
        [ExcelColumnName("规格")]
        public string Specs { get; set; }

        [ExcelColumn(Name = "药品类别")]
        [ExcelColumnName("药品类别")]
        public string DrugType { get; set; }

        [ExcelColumn(Name = "药品性质")]
        [ExcelColumnName("药品性质")]
        public string DrugQuality { get; set; }

        [ExcelColumn(Name = "参考零售价")]
        [ExcelColumnName("参考零售价")]
        public decimal RetailPrice { get; set; }

        [ExcelColumn(Name = "包装单位")]
        [ExcelColumnName("包装单位")]
        public string PackUnit { get; set; }

        [ExcelColumn(Name = "包装数")]
        [ExcelColumnName("包装数")]
        public int? PackQty { get; set; }

        [ExcelColumn(Name = "最小单位")]
        [ExcelColumnName("最小单位")]
        public string MinUnit { get; set; }

        [ExcelColumn(Name = "有效期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("有效期")]
        public DateTime? ValidDate { get; set; }

        [ExcelColumn(Name = "库存数量")]
        [ExcelColumnName("库存数量")]
        public decimal StoreSum { get; set; }

        [ExcelColumn(Name = "库存金额")]
        [ExcelColumnName("库存金额")]
        public decimal StoreCost { get; set; }

        [ExcelColumn(Name = "预扣库存数量")]
        [ExcelColumnName("预扣库存数量")]
        public decimal PreSum { get; set; }

        [ExcelColumn(Name = "预扣库存金额")]
        [ExcelColumnName("预扣库存金额")]
        public decimal PreCost { get; set; }

        [ExcelColumn(Name = "最低库存量")]
        [ExcelColumnName("最低库存量")]
        public decimal LowSum { get; set; }

        [ExcelColumn(Name = "最高库存量")]
        [ExcelColumnName("最高库存量")]
        public decimal TopSum { get; set; }

        [ExcelColumn(Name = "货位码")]
        [ExcelColumnName("货位码")]
        public string PlaceCode { get; set; }

        [ExcelColumn(Name = "日盘点标志")]
        [ExcelColumnName("日盘点标志")]
        public string DailtycheckFlag { get; set; }

        [Required(ErrorMessage = "批次号不能为空")]
        [ExcelColumn(Name = "批次号")]
        [ExcelColumnName("批次号")]
        public string GroupCode { get; set; }

        [ExcelColumn(Name = "批号")]
        [ExcelColumnName("批号")]
        public string BatchNo { get; set; }

        [Required(ErrorMessage = "生产厂家不能为空")]
        [ExcelColumn(Name = "生产厂家")]
        [ExcelColumnName("生产厂家")]
        public string ProducerCode { get; set; }



        [ExcelColumn(Name = "药品类别")]
        public string DrugTypeLabel { get; set; }
    }
}