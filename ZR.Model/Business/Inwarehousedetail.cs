
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

    }
}