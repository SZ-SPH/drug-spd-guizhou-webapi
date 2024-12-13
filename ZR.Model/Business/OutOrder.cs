
namespace ZR.Model.Business
{
    /// <summary>
    /// 出库单
    /// </summary>
    [SugarTable("OutOrder")]
    public class OutOrder
    {
        /// <summary>
        /// Id 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 出库单据 
        /// </summary>
        public string OutOrderCode { get; set; }

        /// <summary>
        /// 领取部门 
        /// </summary>
        public string InpharmacyId { get; set; }

        /// <summary>
        /// 领取人 
        /// </summary>
        public string UseReceive { get; set; }

        /// <summary>
        /// 发出出库 
        /// </summary>
        public string OutWarehouseID { get; set; }

        /// <summary>
        /// 时间 
        /// </summary>
        public DateTime? Times { get; set; }

        /// <summary>
        /// 备注 
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// his出库单流水号
        /// </summary>
        public long? OutBillCode { get; set; }

        /// <summary>
        /// 创建时间 
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建人 
        /// </summary>
        public string CreateBy { get; set; }

    }
}