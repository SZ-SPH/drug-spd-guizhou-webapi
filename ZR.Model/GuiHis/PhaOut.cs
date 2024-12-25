using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;



namespace ZR.Model.GuiHis
{
    //http://192.168.2.21:9403/His/GetPhaOutList?beginTime=2024-01-01&endTime=2024-01-31
    //出库
    public class PhaOutInQuery
    {
        public DateTime beginTime { get; set; }
        public DateTime endTime { get; set; }
    }
    /// <summary>
    /// 出库记录
    /// </summary>
    [SugarTable("PhaOut")]
    public class PhaOut
    {
        /// <summary>
        /// 出库科室编码
        /// </summary>
        [SugarColumn(ColumnName = "drugDeptCode")]
        public string DrugDeptCode { get; set; }


        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]

        /// <summary>
        /// 出库单流水号
        /// </summary>
        public long OutBillCode { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(ColumnName = "serialCode")]
        public int SerialCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        [SugarColumn(ColumnName = "groupCode")]
        public string GroupCode { get; set; }

        /// <summary>
        /// 出库单据号
        /// </summary>
        [SugarColumn(ColumnName = "outListCode")]
        public string OutListCode { get; set; }

        /// <summary>
        /// 出库类型
        /// </summary>
        [SugarColumn(ColumnName = "outType")]
        public string OutType { get; set; }

        /// <summary>
        /// 出库分类
        /// </summary>
        [SugarColumn(ColumnName = "class3MeaningCode")]
        public string Class3MeaningCode { get; set; }

        /// <summary>
        /// 入库单号
        /// </summary>
        [SugarColumn(ColumnName = "inBillCode")]
        public long? InBillCode { get; set; }

        /// <summary>
        /// 入库单序号
        /// </summary>
        [SugarColumn(ColumnName = "inSerialCode")]
        public int? InSerialCode { get; set; }

        /// <summary>
        /// 入库单据号
        /// </summary>
        [SugarColumn(ColumnName = "inListCode")]
        public string InListCode { get; set; }

        /// <summary>
        /// 药品编码
        /// </summary>
        [SugarColumn(ColumnName = "drugCode")]
        public string DrugCode { get; set; }

        /// <summary>
        /// 药品商品名
        /// </summary>
        [SugarColumn(ColumnName = "tradeName")]
        public string TradeName { get; set; }

        /// <summary>
        /// 药品类别
        /// </summary>
        [SugarColumn(ColumnName = "drugType")]
        public string DrugType { get; set; }

        /// <summary>
        /// 药品性质
        /// </summary>
        [SugarColumn(ColumnName = "drugQuality")]
        public string DrugQuality { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        [SugarColumn(ColumnName = "specs")]
        public string Specs { get; set; }

        /// <summary>
        /// 包装单位
        /// </summary>
        [SugarColumn(ColumnName = "packUnit")]
        public string PackUnit { get; set; }

        /// <summary>
        /// 包装数
        /// </summary>
        [SugarColumn(ColumnName = "packQty")]
        public int? PackQty { get; set; }

        /// <summary>
        /// 最小单位
        /// </summary>
        [SugarColumn(ColumnName = "minUnit")]
        public string MinUnit { get; set; }

        /// <summary>
        /// 显示的单位标记
        /// </summary>
        [SugarColumn(ColumnName = "showFlag")]
        public string ShowFlag { get; set; }

        /// <summary>
        /// 显示的单位
        /// </summary>
        [SugarColumn(ColumnName = "showUnit")]
        public string ShowUnit { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        [SugarColumn(ColumnName = "batchNo")]
        public string BatchNo { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        [SugarColumn(ColumnName = "validDate")]
        public DateTime? ValidDate { get; set; }

        /// <summary>
        /// 生产厂家
        /// </summary>
        [SugarColumn(ColumnName = "producerCode")]
        public string ProducerCode { get; set; }

        /// <summary>
        /// 供货单位代码
        /// </summary>
        [SugarColumn(ColumnName = "companyCode")]
        public string CompanyCode { get; set; }

        /// <summary>
        /// 零售价
        /// </summary>
        [SugarColumn(ColumnName = "retailPrice")]
        public decimal? RetailPrice { get; set; }

        /// <summary>
        /// 批发价
        /// </summary>
        [SugarColumn(ColumnName = "wholesalePrice")]
        public decimal? WholesalePrice { get; set; }

        /// <summary>
        /// 购入价
        /// </summary>
        [SugarColumn(ColumnName = "purchasePrice")]
        public decimal? PurchasePrice { get; set; }

        /// <summary>
        /// 出库数量
        /// </summary>
        [SugarColumn(ColumnName = "outNum")]
        public decimal? OutNum { get; set; }

        /// <summary>
        /// 零售金额
        /// </summary>
        [SugarColumn(ColumnName = "saleCost")]
        public decimal? SaleCost { get; set; }

        /// <summary>
        /// 批发金额
        /// </summary>
        [SugarColumn(ColumnName = "tradeCost")]
        public decimal? TradeCost { get; set; }

        /// <summary>
        /// 购入金额
        /// </summary>
        [SugarColumn(ColumnName = "approveCost")]
        public decimal? ApproveCost { get; set; }

        /// <summary>
        /// 出库后库存数量
        /// </summary>
        [SugarColumn(ColumnName = "storeNum")]
        public decimal? StoreNum { get; set; }

        /// <summary>
        /// 出库后库存总金额
        /// </summary>
        [SugarColumn(ColumnName = "storeCost")]
        public decimal? StoreCost { get; set; }

        /// <summary>
        /// 特殊标记
        /// </summary>
        [SugarColumn(ColumnName = "specialFlag")]
        public string SpecialFlag { get; set; }

        /// <summary>
        /// 出库状态
        /// </summary>
        [SugarColumn(ColumnName = "outState")]
        public string OutState { get; set; }

        /// <summary>
        /// 申请出库量
        /// </summary>
        [SugarColumn(ColumnName = "applyNum")]
        public decimal? ApplyNum { get; set; }

        /// <summary>
        /// 申请出库人
        /// </summary>
        [SugarColumn(ColumnName = "applyOpercode")]
        public string ApplyOpercode { get; set; }

        /// <summary>
        /// 申请出库日期
        /// </summary>
        [SugarColumn(ColumnName = "applyDate")]
        public DateTime? ApplyDate { get; set; }

        /// <summary>
        /// 审批数量
        /// </summary>
        [SugarColumn(ColumnName = "examNum")]
        public decimal? ExamNum { get; set; }

        /// <summary>
        /// 审批人
        /// </summary>
        [SugarColumn(ColumnName = "examOpercode")]
        public string ExamOpercode { get; set; }

        /// <summary>
        /// 审批日期
        /// </summary>
        [SugarColumn(ColumnName = "examDate")]
        public DateTime? ExamDate { get; set; }

        /// <summary>
        /// 核准人
        /// </summary>
        [SugarColumn(ColumnName = "approveOpercode")]
        public string ApproveOpercode { get; set; }

        /// <summary>
        /// 核准日期
        /// </summary>
        [SugarColumn(ColumnName = "approveDate")]
        public DateTime? ApproveDate { get; set; }

        /// <summary>
        /// 货位号
        /// </summary>
        [SugarColumn(ColumnName = "placeCode")]
        public string PlaceCode { get; set; }

        /// <summary>
        /// 退库数量
        /// </summary>
        [SugarColumn(ColumnName = "returnNum")]
        public decimal? ReturnNum { get; set; }

        /// <summary>
        /// 摆药单号
        /// </summary>
        [SugarColumn(ColumnName = "drugedBill")]
        public string DrugedBill { get; set; }

        /// <summary>
        /// 制剂序号
        /// </summary>
        [SugarColumn(ColumnName = "medId")]
        public string MedId { get; set; }

        /// <summary>
        /// 领药单位编码
        /// </summary>
        [SugarColumn(ColumnName = "drugStorageCode")]
        public string DrugStorageCode { get; set; }

        /// <summary>
        /// 处方号
        /// </summary>
        [SugarColumn(ColumnName = "recipeNo")]
        public string RecipeNo { get; set; }

        /// <summary>
        /// 处方流水号
        /// </summary>
        [SugarColumn(ColumnName = "sequenceNo")]
        public int? SequenceNo { get; set; }

        /// <summary>
        /// 签字人
        /// </summary>
        [SugarColumn(ColumnName = "signPerson")]
        public string SignPerson { get; set; }

        /// <summary>
        /// 领药人
        /// </summary>
        [SugarColumn(ColumnName = "getPerson")]
        public string GetPerson { get; set; }

        /// <summary>
        /// 冲账标志
        /// </summary>
        [SugarColumn(ColumnName = "strikeFlag")]
        public string StrikeFlag { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "mark")]
        public string Mark { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        [SugarColumn(ColumnName = "operCode")]
        public string OperCode { get; set; }

        /// <summary>
        /// 操作日期
        /// </summary>
        [SugarColumn(ColumnName = "operDate")]
        public DateTime? OperDate { get; set; }

        /// <summary>
        /// 是否药房向药柜出库记录
        /// </summary>
        [SugarColumn(ColumnName = "arkFlag")]
        public string ArkFlag { get; set; }

        /// <summary>
        /// 药柜发药 出库单流水号
        /// </summary>
        [SugarColumn(ColumnName = "arkBillCode")]
        public long? ArkBillCode { get; set; }

        /// <summary>
        /// 出库记录发生时间
        /// </summary>
        [SugarColumn(ColumnName = "outDate")]
        public DateTime? OutDate { get; set; }

        /// <summary>
        /// 申请单流水号
        /// </summary>
        [SugarColumn(ColumnName = "applyNumber")]
        public long? ApplyNumber { get; set; }
    }

    [SugarTable("PhaOutView")]

    public class PhaOutDtoView
    {

        public string DrugDeptName { get; set; }
        public string DrugStorageName { get; set; }
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

