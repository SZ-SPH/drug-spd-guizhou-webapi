namespace ZR.Model.GuiHis.Dto
{
    /// <summary>
    /// 药品查询对象
    /// </summary>
    public class GuiDrugQueryDto : PagerInfo
    {
        public string DrugtermId { get; set; }
        public string EnglishFormal { get; set; }
        public string RegularName { get; set; }
        public string RegularSpellCode { get; set; }
        public string RegularWbCode { get; set; }
        public string SpellCode { get; set; }
        public string TradeName { get; set; }
        public string WbCode { get; set; }
    }

    /// <summary>
    /// 药品输入输出对象
    /// </summary>
    public class GuiDrugDto
    {
        [ExcelColumn(Name = "累计天数")]
        [ExcelColumnName("累计天数")]
        public string AdditiveDays { get; set; }

        [ExcelColumn(Name = "累计数量")]
        [ExcelColumnName("累计数量")]
        public string AdditiveQty { get; set; }

        [ExcelColumn(Name = "是否抗生素")]
        [ExcelColumnName("是否抗生素")]
        public string AntibioticsFlag { get; set; }

        [ExcelColumn(Name = "抗生素等级")]
        [ExcelColumnName("抗生素等级")]
        public string AntibioticsLv { get; set; }

        [ExcelColumn(Name = "备用基本剂量")]
        [ExcelColumnName("备用基本剂量")]
        public string BakBaseDose { get; set; }

        [ExcelColumn(Name = "备用剂量单位")]
        [ExcelColumnName("备用剂量单位")]
        public string BakDoseUnit { get; set; }

        [ExcelColumn(Name = "基本剂量")]
        [ExcelColumnName("基本剂量")]
        public string BaseDose { get; set; }

        [ExcelColumn(Name = "儿童用药限制标志")]
        [ExcelColumnName("儿童用药限制标志")]
        public string ChildFlag { get; set; }

        [ExcelColumn(Name = "高危药标志")]
        [ExcelColumnName("高危药标志")]
        public string DangerFlag { get; set; }

        [ExcelColumn(Name = "默认频次编码")]
        [ExcelColumnName("默认频次编码")]
        public string DefFreqCode { get; set; }

        [ExcelColumn(Name = "默认每次计量")]
        [ExcelColumnName("默认每次计量")]
        public string DefOnceDose { get; set; }

        [ExcelColumn(Name = "默认给药途径编码")]
        [ExcelColumnName("默认给药途径编码")]
        public string DefUsageCode { get; set; }

        [ExcelColumn(Name = "默认给药途径名称")]
        [ExcelColumnName("默认给药途径名称")]
        public string DefUsageName { get; set; }

        [ExcelColumn(Name = "剂型")]
        [ExcelColumnName("剂型")]
        public string DoseModel { get; set; }

        [ExcelColumn(Name = "剂型名称")]
        [ExcelColumnName("剂型名称")]
        public string DoseModelName { get; set; }

        [ExcelColumn(Name = "剂量单位")]
        [ExcelColumnName("剂量单位")]
        public string DoseUnit { get; set; }

        [ExcelColumn(Name = "药品性质")]
        [ExcelColumnName("药品性质")]
        public string DrugQuality { get; set; }

        [ExcelColumn(Name = "药品术语编码")]
        [ExcelColumnName("药品术语编码")]
        public string DrugtermId { get; set; }

        [ExcelColumn(Name = "英文名")]
        [ExcelColumnName("英文名")]
        public string EnglishFormal { get; set; }

        [ExcelColumn(Name = "英文品名")]
        [ExcelColumnName("英文品名")]
        public string EnglishName { get; set; }

        [ExcelColumn(Name = "英文通用名")]
        [ExcelColumnName("英文通用名")]
        public string EnglishRegular { get; set; }

        [ExcelColumn(Name = "计划生育药品标志")]
        [ExcelColumnName("计划生育药品标志")]
        public string FamilyplanFlag { get; set; }

        [ExcelColumn(Name = "国家编码")]
        [ExcelColumnName("国家编码")]
        public string GbCode { get; set; }

        [ExcelColumn(Name = "GMP标志")]
        [ExcelColumnName("GMP标志")]
        public string GmpFlag { get; set; }

        [ExcelColumn(Name = "默认特殊煎制法编码")]
        [ExcelColumnName("默认特殊煎制法编码")]
        public string HerbProcCode { get; set; }

        [ExcelColumn(Name = "默认特殊煎制法名称")]
        [ExcelColumnName("默认特殊煎制法名称")]
        public string HerbProcName { get; set; }

        [ExcelColumn(Name = "皮试类别编码")]
        [ExcelColumnName("皮试类别编码")]
        public string HypoKindCode { get; set; }

        [ExcelColumn(Name = "皮试类别名称")]
        [ExcelColumnName("皮试类别名称")]
        public string HypoKindName { get; set; }

        [ExcelColumn(Name = "是否皮试剂")]
        [ExcelColumnName("是否皮试剂")]
        public string HypoReagentFlag { get; set; }

        [ExcelColumn(Name = "是否需皮试")]
        [ExcelColumnName("是否需皮试")]
        public string HypoTestFlag { get; set; }

        [ExcelColumn(Name = "是否进口药")]
        [ExcelColumnName("是否进口药")]
        public string ImportFlag { get; set; }

        [ExcelColumn(Name = "住院默认取药单位等级")]
        [ExcelColumnName("住院默认取药单位等级")]
        public string InDrugUnitLv { get; set; }

        [ExcelColumn(Name = "国际编码")]
        [ExcelColumnName("国际编码")]
        public string InternationalCode { get; set; }

        [ExcelColumn(Name = "大输液标志")]
        [ExcelColumnName("大输液标志")]
        public string LiquidFlag { get; set; }

        [ExcelColumn(Name = "每天最大用量")]
        [ExcelColumnName("每天最大用量")]
        public string MaxDayDose { get; set; }

        [ExcelColumn(Name = "最大天数")]
        [ExcelColumnName("最大天数")]
        public string MaxDays { get; set; }

        [ExcelColumn(Name = "单日最大频次")]
        [ExcelColumnName("单日最大频次")]
        public string MaxFrequency { get; set; }

        [ExcelColumn(Name = "最大每次量")]
        [ExcelColumnName("最大每次量")]
        public string MaxOnceDose { get; set; }

        [ExcelColumn(Name = "中间数量")]
        [ExcelColumnName("中间数量")]
        public string MidQty { get; set; }

        [ExcelColumn(Name = "中间单位")]
        [ExcelColumnName("中间单位")]
        public string MidUnit { get; set; }

        [ExcelColumn(Name = "最小单位")]
        [ExcelColumnName("最小单位")]
        public string MinUnit { get; set; }

        [ExcelColumn(Name = "是否协定处方")]
        [ExcelColumnName("是否协定处方")]
        public string NostrumFlag { get; set; }

        [ExcelColumn(Name = "OTC标志")]
        [ExcelColumnName("OTC标志")]
        public string OtcFlag { get; set; }

        [ExcelColumn(Name = "包装数量")]
        [ExcelColumnName("包装数量")]
        public string PackQty { get; set; }

        [ExcelColumn(Name = "包装单位")]
        [ExcelColumnName("包装单位")]
        public string PackUnit { get; set; }

        [ExcelColumn(Name = "默认每次量单位等级")]
        [ExcelColumnName("默认每次量单位等级")]
        public string PefDoseUnitLv { get; set; }

        [ExcelColumn(Name = "药理作用编码")]
        [ExcelColumnName("药理作用编码")]
        public string PhaFunCode { get; set; }

        [ExcelColumn(Name = "药理作用路径")]
        [ExcelColumnName("药理作用路径")]
        public string PhaFunPath { get; set; }

        [ExcelColumn(Name = "贵重药标志")]
        [ExcelColumnName("贵重药标志")]
        public string PreciousFlag { get; set; }

        [ExcelColumn(Name = "参考价格")]
        [ExcelColumnName("参考价格")]
        public string PriceRef { get; set; }

        [ExcelColumn(Name = "生产厂家")]
        [ExcelColumnName("生产厂家")]
        public string ProducerName { get; set; }

        [ExcelColumn(Name = "门诊默认取药单位等级")]
        [ExcelColumnName("门诊默认取药单位等级")]
        public string PutDrugUnitLv { get; set; }

        [ExcelColumn(Name = "皮试剂编码")]
        [ExcelColumnName("皮试剂编码")]
        public string ReagentCode { get; set; }

        [ExcelColumn(Name = "通用名")]
        [ExcelColumnName("通用名")]
        public string RegularName { get; set; }

        [ExcelColumn(Name = "通用名拼音码")]
        [ExcelColumnName("通用名拼音码")]
        public string RegularSpellCode { get; set; }

        [ExcelColumn(Name = "通用名五笔码")]
        [ExcelColumnName("通用名五笔码")]
        public string RegularWbCode { get; set; }

        [ExcelColumn(Name = "性别限制用药标志")]
        [ExcelColumnName("性别限制用药标志")]
        public string SexClass { get; set; }

        [ExcelColumn(Name = "医保等级")]
        [ExcelColumnName("医保等级")]
        public string SiGrade { get; set; }

        [ExcelColumn(Name = "适应症")]
        [ExcelColumnName("适应症")]
        public string SiMark { get; set; }

        [ExcelColumn(Name = "规格")]
        [ExcelColumnName("规格")]
        public string Specs { get; set; }

        [ExcelColumn(Name = "商品名拼音码")]
        [ExcelColumnName("商品名拼音码")]
        public string SpellCode { get; set; }

        [ExcelColumn(Name = "拆分类型")]
        [ExcelColumnName("拆分类型")]
        public string SplitType { get; set; }

        [ExcelColumn(Name = "兴奋剂标志")]
        [ExcelColumnName("兴奋剂标志")]
        public string StimulantFlag { get; set; }

        [ExcelColumn(Name = "与每平米体表面积换算系数")]
        [ExcelColumnName("与每平米体表面积换算系数")]
        public string SurfaceFactor { get; set; }

        [ExcelColumn(Name = "术语类型编码")]
        [ExcelColumnName("术语类型编码")]
        public string TermClassId { get; set; }

        [ExcelColumn(Name = "术语类型名称")]
        [ExcelColumnName("术语类型名称")]
        public string TermClassName { get; set; }

        [ExcelColumn(Name = "商品名")]
        [ExcelColumnName("商品名")]
        public string TradeName { get; set; }

        [ExcelColumn(Name = "药品使用提示")]
        [ExcelColumnName("药品使用提示")]
        public string UseTip { get; set; }

        [ExcelColumn(Name = "商品名五笔码")]
        [ExcelColumnName("商品名五笔码")]
        public string WbCode { get; set; }

        [ExcelColumn(Name = "与每千克体重换算系数")]
        [ExcelColumnName("与每千克体重换算系数")]
        public string WeightFactor { get; set; }



        [ExcelColumn(Name = "拆分类型")]
        public string SplitTypeLabel { get; set; }
    }
}