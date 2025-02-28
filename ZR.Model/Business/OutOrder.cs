
namespace ZR.Model.Business
{
    /// <summary>
    /// 出库单
    /// </summary>
    [SugarTable("OutOrder")]
    public class OutOrder
    {

        public string Type { get; set; }

        /// <summary>
        /// Id 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 出库单据 
        /// </summary>
        public string OutOrderCode { get; set; }
        public string Billcodesf { get; set; }
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


    public class EOut
    {
        public int Page { get; set; }
        public int AllPage { get; set; }
        //public string iscong { get; set; }


        public string biidess { get; set; }

        public string Demo { get; set; }
        public string Billcode { get; set; }

        /// <summary>
        /// 领取部门
        /// </summary>
        public string DrugStorageCode { get; set; }
        /// <summary>
        /// 发出仓库
        /// </summary>
        public string DrugDeptCode { get; set; }
        /// <summary>
        ///   领取人
        /// </summary>
        public string GetPerson { get; set; }
        /// <summary>
        /// 出库类型
        /// </summary>
        public string OutType { get; set; }
        /// <summary>
        /// 领用时间
        /// </summary>
        public string GTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Mark { get; set; }
        /// <summary>
        /// 总批价金额
        /// </summary>
        public string SumApproveCost { get; set; }
        /// <summary>
        /// 总零价金额
        /// </summary>
        public string SumSaleCost { get; set; }
        /// <summary>
        /// 大写总批价金额
        /// </summary>
        public string ChinaSumApproveCost { get; set; }
        /// <summary>
        /// 大写总零价金额
        /// </summary>
        public string ChinaSumSaleCost { get; set; }
        public List<EIn> Drugs { get; set; }
        public string num { get; set; }
        public string NowgetTime { get; set; }


    }

    public class EIn
    {
        /// <summary>
        ///  药品名称
        /// </summary>
        public string TradeName { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Specs { get; set; }
        /// <summary>
        /// /单位
        /// </summary>
        public string PackUnit { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string PackQty { get; set; }
        /// <summary>
        ///   购入价
        /// </summary>
        public string PurchasePrice { get; set; }
        /// <summary>
        /// 购入金额
        /// </summary>
        public string ApproveCost { get; set; }
        /// <summary>
        /// 零售价
        /// </summary>
        public string RetailPrice { get; set; }
        /// <summary>
        /// 售价金额
        /// </summary>
        public string SaleCost { get; set; }
        /// <summary>
        ///   生产厂家
        /// </summary>
        public string ProducerCode { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public string ValidDate { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 货位号
        /// </summary>
        public string LoctionName { get; set; }


    }



    public class rk
    {
        public int Page { get; set; }
        public int AllPage { get; set; }
        /// <summary>
        /// 入库类型	  一般入库
        /// </summary>
        public string InType { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        public string DeptCode { get; set; }

        ///备注
        public string Mark { get; set; }
        /// <summary>
        /// 供货单位 
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string GTime { get; set; }
        /// <summary>
        /// 发票号
        /// </summary>
        public string BillCode { get; set; }
        /// <summary>
        /// 总批价金额
        /// </summary>
        public string SumApproveCost { get; set; }
        /// <summary>
        /// 总零价金额
        /// </summary>
        public string SumSaleCost { get; set; }
        /// <summary>
        /// 大写总批价金额
        /// </summary>
        public string ChinaSumApproveCost { get; set; }
        /// <summary>
        /// 大写总零价金额
        /// </summary>
        public string ChinaSumSaleCost { get; set; }
        public string num { get; set; }
        public string NowgetTime { get; set; }
        public string Code { get; set; }

        public List<rkdrugs> Drugs { get; set; }
    }

    public class rkdrugs
    {
        /// <summary>
        ///  药品名称
        /// </summary>
        public string TradeName { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Specs { get; set; }
        /// <summary>
        /// /单位
        /// </summary>
        public string PackUnit { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal PackQty { get; set; }
        /// <summary>
        ///   购入价
        /// </summary>
        public string PurchasePrice { get; set; }
        /// <summary>
        /// 购入金额
        /// </summary>
        public string ApproveCost { get; set; }
        /// <summary>
        /// 零售价
        /// </summary>
        public string RetailPrice { get; set; }
        /// <summary>
        /// 售价金额
        /// </summary>
        public string SaleCost { get; set; }
        /// <summary>
        ///   生产厂家
        /// </summary>
        public string ProducerCode { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public string ValidDate { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        public string BatchNo { get; set; }

    }


   
}