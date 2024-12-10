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

        /// <summary>
        /// 出库单流水号
        /// </summary>
        [SugarColumn(ColumnName = "outBillCode")]
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
}

