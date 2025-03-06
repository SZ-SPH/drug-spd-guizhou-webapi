
namespace ZR.Model.Business.Dto
{
    /// <summary>
    /// 入库详情查询对象
    /// </summary>
    public class TInwarehousedetailQueryDto : PagerInfo 
    {
        public string DrugCode { get; set; }
        public string DrugName { get; set; }
        public DateTime? BeginCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }
        public string BatchNo { get; set; }
    }

    /// <summary>
    /// 入库详情输入输出对象
    /// </summary>
    public class TInwarehousedetailDto
    {
        [Required(ErrorMessage = "Id不能为空")]
        [ExcelColumn(Name = "Id")]
        [ExcelColumnName("Id")]
        public int Id { get; set; }

        [ExcelColumn(Name = "药品编码 关联 ")]
        [ExcelColumnName("药品编码 关联 ")]
        public string DrugCode { get; set; }

        [ExcelColumn(Name = "入库数量")]
        [ExcelColumnName("入库数量")]
        public int? InwarehouseQty { get; set; }

        [ExcelColumn(Name = "备注")]
        [ExcelColumnName("备注")]
        public string Remark { get; set; }

        [ExcelColumn(Name = "创建时间", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("创建时间")]
        public DateTime? CreateTime { get; set; }

        [ExcelColumn(Name = "入库单ID")]
        [ExcelColumnName("入库单ID")]
        public int? InwarehouseId { get; set; }

        [ExcelColumn(Name = "流水号")]
        [ExcelColumnName("流水号")]
        public string SerialNum { get; set; }

        [ExcelColumn(Name = "批号")]
        [ExcelColumnName("批号")]
        public string BatchNo { get; set; }

        [ExcelColumn(Name = "批次号 （废弃）")]
        [ExcelColumnName("批次号 （废弃）")]
        public string BatchId { get; set; }

        [ExcelColumn(Name = "批文信息")]
        [ExcelColumnName("批文信息")]
        public string ApproveInfo { get; set; }

        [ExcelColumn(Name = "采购单号")]
        [ExcelColumnName("采购单号")]
        public string PurchaseOrderNum { get; set; }

        [ExcelColumn(Name = "生产日期")]
        [ExcelColumnName("生产日期")]
        public string ProductDate { get; set; }

        [ExcelColumn(Name = "有效期")]
        [ExcelColumnName("有效期")]
        public string ValiDate { get; set; }

        [ExcelColumn(Name = "产地")]
        [ExcelColumnName("产地")]
        public string InName { get; set; }

        [ExcelColumn(Name = "MixBuyPrice")]
        [ExcelColumnName("MixBuyPrice")]
        public decimal MixBuyPrice { get; set; }

        [ExcelColumn(Name = "MixOutPrice")]
        [ExcelColumnName("MixOutPrice")]
        public decimal MixOutPrice { get; set; }

        [ExcelColumn(Name = "推送状态")]
        [ExcelColumnName("推送状态")]
        public string Tstars { get; set; }

        [ExcelColumn(Name = "生产厂家")]
        [ExcelColumnName("生产厂家")]
        public string ProductCode { get; set; }



    }
}