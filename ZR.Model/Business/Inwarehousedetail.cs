
namespace ZR.Model.Business
{
    /// <summary>
    /// 添加条目详细
    /// </summary>
    [SugarTable("t_inwarehousedetail")]
    public class Inwarehousedetail
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
        public string ProductCode { get; set; }
        /// <summary>
        /// 入库数量 
        /// </summary>
        public int InwarehouseQty { get; set; }

        /// <summary>
        /// 备注 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 入库单ID 
        /// </summary>
        public int InwarehouseId { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        public string SerialNum { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 批次号
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
        /// 产地 extCode1
        /// </summary>
        public string InName { get; set; }
        /// <summary>
        /// 购入价（最小单位价格）
        /// </summary>
        public decimal MixBuyPrice { get; set; }
        /// <summary>
        /// 最小单位零售价
        /// </summary>
        public decimal MixOutPrice { get; set; }

        public string Tstars { get; set; }

    }
}