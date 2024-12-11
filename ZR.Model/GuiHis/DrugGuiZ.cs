using SqlSugar;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.GuiHis
{


    //药品
    public class GuiDrugInQuery
    {
        // 查询参数
        public int? pageSize { get; set; } // 页大小
        public int? pageNum { get; set; }  // 页码
        public string orderBy { get; set; } // 排序字段
        public string orderType { get; set; } // 排序类型
        public string termClassId { get; set; } // 术语类型编码
        public string drugTermId { get; set; } // 术语编码
        public bool pageFlag { get; set; } // pageFlag为true表示分页，为false的情况下就需要传其他过滤条件

    }
    public class ApiResponse
    {
        public int? Code { get; set; } // code
        public item Data { get; set; } // data
        public string Msg { get; set; } // msg
    }

    public class item
    {
        public int? EndRow { get; set; } // endRow
        public bool? HasNextPage { get; set; } // hasNextPage
        public bool? HasPreviousPage { get; set; } // hasPreviousPage
        public bool? IsFirstPage { get; set; } // isFirstPage
        public bool? IsLastPage { get; set; } // isLastPage
        public List<GuiDrug> List { get; set; } // list
        public int? NavigateFirstPage { get; set; } // navigateFirstPage
        public int? NavigateLastPage { get; set; } // navigateLastPage
        public int? NavigatePages { get; set; } // navigatePages
        public List<int?> NavigatePageNums { get; set; } // navigatepageNums
        public int? NextPage { get; set; } // nextPage
        public int? PageNum { get; set; } // pageNum
        public int? PageSize { get; set; } // pageSize
        public int? Pages { get; set; } // pages
        public int? PrePage { get; set; } // prePage
        public int? Size { get; set; } // size
        public int? StartRow { get; set; } // startRow
        public long? Total { get; set; } // total
    }

    [SugarTable("GuiDrug")]

    public class GuiDrug
    {
        public string AdditiveDays { get; set; } // cumulative days
        public string AdditiveQty { get; set; } // cumulative quantity
        public string AntibioticsFlag { get; set; } // whether antibiotic
        public string AntibioticsLv { get; set; } // antibiotic level
        public string BakBaseDose { get; set; } // backup base dose
        public string BakDoseUnit { get; set; } // backup dose unit
        public string BaseDose { get; set; } // base dose
        public string ChildFlag { get; set; } // child medication restriction flag
        public string DangerFlag { get; set; } // high-risk drug flag
        public string DefFreqCode { get; set; } // default frequency code
        public string DefOnceDose { get; set; } // default per dose
        public string DefUsageCode { get; set; } // default administration route code
        public string DefUsageName { get; set; } // default administration route name
        public string DoseModel { get; set; } // dosage form
        public string DoseModelName { get; set; } // dosage form name
        public string DoseUnit { get; set; } // dosage unit
        public string DrugQuality { get; set; } // drug quality
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]

        public string DrugTermId { get; set; } // drug terminology code
        public string EnglishFormal { get; set; } // formal English name
        public string EnglishName { get; set; } // English product name
        public string EnglishRegular { get; set; } // English common name
        public string FamilyPlanFlag { get; set; } // family planning drug flag
        public string GbCode { get; set; } // national code
        public string GmpFlag { get; set; } // GMP flag
        public string HerbProcCode { get; set; } // default special preparation method code
        public string HerbProcName { get; set; } // default special preparation method name
        public string HypoKindCode { get; set; } // skin test category code
        public string HypoKindName { get; set; } // skin test category name
        public string HypoReagentFlag { get; set; } // whether skin test reagent
        public string HypoTestFlag { get; set; } // whether skin test required
        public string ImportFlag { get; set; } // whether imported drug
        public string InDrugUnitLv { get; set; } // inpatient default drug unit level
        public string InternationalCode { get; set; } // international code
        public string LiquidFlag { get; set; } // large infusion flag
        public string MaxDayDose { get; set; } // maximum daily dosage
        public string MaxDays { get; set; } // maximum days
        public string MaxFrequency { get; set; } // maximum frequency per day
        public string MaxOnceDose { get; set; } // maximum single dose
        public string MidQty { get; set; } // intermediate quantity
        public string MidUnit { get; set; } // intermediate unit
        public string MinUnit { get; set; } // minimum unit
        public string NostrumFlag { get; set; } // whether agreed prescription
        public string OtcFlag { get; set; } // OTC flag
        public string PackQty { get; set; } // packaging quantity
        public string PackUnit { get; set; } // packaging unit
        public string PefDoseUnitLv { get; set; } // default per dose unit level
        public string PhaFunCode { get; set; } // pharmacological action code
        public string PhaFunPath { get; set; } // pharmacological action path
        public string PreciousFlag { get; set; } // precious drug flag
        public string PriceRef { get; set; } // reference price
        public string ProducerName { get; set; } // manufacturer
        public string PutDrugUnitLv { get; set; } // outpatient default drug unit level
        public string ReagentCode { get; set; } // skin test reagent code
        public string RegularName { get; set; } // common name
        public string RegularSpellCode { get; set; } // common name pinyin code
        public string RegularWbCode { get; set; } // common name Wubi code
        public string SexClass { get; set; } // gender restriction medication flag
        public string SiGrade { get; set; } // medical insurance grade
        public string SiMark { get; set; } // indication
        public string Specs { get; set; } // specifications
        public string SpellCode { get; set; } // product name pinyin code
        public string SplitType { get; set; } // split type
        public string StimulantFlag { get; set; } // stimulant flag
        public string SurfaceFactor { get; set; } // surface area conversion factor
        public string TermClassId { get; set; } // terminology type code
        public string TermClassName { get; set; } // terminology type name
        public string TradeName { get; set; } // trade name
        public string UseTip { get; set; } // medication usage tips
        public string WbCode { get; set; } // product name Wubi code
        public string WeightFactor { get; set; } // weight conversion factor
    }

}
