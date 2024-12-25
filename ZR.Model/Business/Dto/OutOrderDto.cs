
namespace ZR.Model.Business.Dto
{
    /// <summary>
    /// 出库单查询对象
    /// </summary>
    public class OutOrderQueryDto : PagerInfo 
    {
        public string OutOrderCode { get; set; }
        public string InpharmacyId { get; set; }
        public string OutWarehouseID { get; set; }
        public long? OutBillCode { get; set; }
    }

    /// <summary>
    /// 出库单输入输出对象
    /// </summary>
    public class OutOrderDto
    {


        /// <summary>
        /// 领取部门
        /// </summary>
        public string InpharmacyName { get; set; }
        /// <summary>
        /// 发出仓库
        /// </summary>
        public string OutWarehouseName { get; set; }


        [Required(ErrorMessage = "Id不能为空")]
        [ExcelColumn(Name = "Id")]
        [ExcelColumnName("Id")]
        public int Id { get; set; }

        [ExcelColumn(Name = "出库单据")]
        [ExcelColumnName("出库单据")]
        public string OutOrderCode { get; set; }

        [ExcelColumn(Name = "领取部门")]
        [ExcelColumnName("领取部门")]
        public string InpharmacyId { get; set; }

        [ExcelColumn(Name = "领取人")]
        [ExcelColumnName("领取人")]
        public string UseReceive { get; set; }

        [ExcelColumn(Name = "发出出库")]
        [ExcelColumnName("发出出库")]
        public string OutWarehouseID { get; set; }

        [ExcelColumn(Name = "时间", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("时间")]
        public DateTime? Times { get; set; }

        [ExcelColumn(Name = "备注")]
        [ExcelColumnName("备注")]
        public string Remarks { get; set; }

        [ExcelColumn(Name = "his出库单流水号")]
        [ExcelColumnName("his出库单流水号")]
        public long? OutBillCode { get; set; }

        [ExcelColumn(Name = "创建时间", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("创建时间")]
        public DateTime? CreateTime { get; set; }


        [ExcelColumn(Name = "创建人")]
        [ExcelColumnName("创建人")]
        public string CreateBy { get; set; }



    }
}