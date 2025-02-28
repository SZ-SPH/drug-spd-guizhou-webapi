
namespace ZR.Model.Business
{
    /// <summary>
    /// 入库详情
    /// </summary>
    [SugarTable("t_inwarehousedetail")]
    public class TInwarehousedetail
    {
        /// <summary>
        /// Id 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 药品编码 关联  
        /// </summary>
        public string DrugCode { get; set; }

        /// <summary>
        /// 入库数量 
        /// </summary>
        public int? InwarehouseQty { get; set; }

        /// <summary>
        /// 备注 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间 
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 入库单ID 
        /// </summary>
        public int? InwarehouseId { get; set; }

        /// <summary>
        /// 流水号 
        /// </summary>
        public string SerialNum { get; set; }

        /// <summary>
        /// 批号 
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 批次号 （废弃） 
        /// </summary>
        public string BatchId { get; set; }

        /// <summary>
        /// 批文信息 
        /// </summary>
        public string ApproveInfo { get; set; }

        /// <summary>
        /// 采购单号 
        /// </summary>
        public string PurchaseOrderNum { get; set; }

        /// <summary>
        /// 生产日期 
        /// </summary>
        public string ProductDate { get; set; }

        /// <summary>
        /// 有效期 
        /// </summary>
        public string ValiDate { get; set; }

        /// <summary>
        /// 产地 
        /// </summary>
        public string InName { get; set; }

        /// <summary>
        /// MixBuyPrice 
        /// </summary>
        public decimal MixBuyPrice { get; set; }

        /// <summary>
        /// MixOutPrice 
        /// </summary>
        public decimal MixOutPrice { get; set; }

        /// <summary>
        /// 推送状态 
        /// </summary>
        public string Tstars { get; set; }

        /// <summary>
        /// 生产厂家 
        /// </summary>
        public string ProductCode { get; set; }

    }
}