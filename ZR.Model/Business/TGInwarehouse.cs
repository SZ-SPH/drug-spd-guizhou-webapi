
namespace ZR.Model.Business
{
    /// <summary>
    /// 采购计划入库
    /// </summary>
    [SugarTable("t_plan_Inwarehouse")]
    public class TGInwarehouse
    {
        /// <summary>
        /// 自增ID 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int? Id { get; set; }

        /// <summary>
        /// 入库计划流水号 
        /// </summary>
        public string PlanNo { get; set; }

        /// <summary>
        /// 采购单号 
        /// </summary>
        public string BillCode { get; set; }

        /// <summary>
        /// 单据状态 0 计划单，1 采购单 
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 计划类型0手工计划，1警戒线，2消耗，3时间，4日消耗 
        /// </summary>
        public string PlanType { get; set; }

        /// <summary>
        /// 科室编码 
        /// </summary>
        public string DrugDeptCode { get; set; }

        /// <summary>
        /// 药品编码 
        /// </summary>
        public string DrugCode { get; set; }

        /// <summary>
        /// 药品名称 
        /// </summary>
        public string TradeName { get; set; }

        /// <summary>
        /// 规格 
        /// </summary>
        public string Specs { get; set; }

        /// <summary>
        /// 参考零售价 
        /// </summary>
        public string RetailPrice { get; set; }

        /// <summary>
        /// 参考批发价 
        /// </summary>
        public string WholesalePrice { get; set; }

        /// <summary>
        /// 最新购入价 
        /// </summary>
        public string PurchasePrice { get; set; }

        /// <summary>
        /// 包装单位 
        /// </summary>
        public string PackUnit { get; set; }

        /// <summary>
        /// 包装数量 
        /// </summary>
        public string PackQty { get; set; }

        /// <summary>
        /// 最小单位 
        /// </summary>
        public string MinUnit { get; set; }

        /// <summary>
        /// 生产厂家编码 
        /// </summary>
        public string ProducerCode { get; set; }

        /// <summary>
        /// 生产厂家名称 
        /// </summary>
        public string ProducerName { get; set; }

        /// <summary>
        /// 本科室库存数量 
        /// </summary>
        public string StoreNum { get; set; }

        /// <summary>
        /// 全院库存总和 
        /// </summary>
        public string StoreTotsum { get; set; }

        /// <summary>
        /// 全院出库总量 
        /// </summary>
        public string OutputSum { get; set; }

        /// <summary>
        /// 计划入库量 
        /// </summary>
        public string PlanNum { get; set; }

        /// <summary>
        /// 计划人 
        /// </summary>
        public string PlanEmpl { get; set; }

        /// <summary>
        /// 计划日期 
        /// </summary>
        public string PlanDate { get; set; }

        /// <summary>
        /// 采购数量 
        /// </summary>
        public string StockNum { get; set; }

        /// <summary>
        /// 采购人 
        /// </summary>
        public string StockEmpl { get; set; }

        /// <summary>
        /// 采购日期 
        /// </summary>
        public string StockDate { get; set; }

        /// <summary>
        /// 审批人 
        /// </summary>
        public string ApproveEmpl { get; set; }

        /// <summary>
        /// 审批时间 
        /// </summary>
        public string ApproveDate { get; set; }

        /// <summary>
        /// 采购流水号 
        /// </summary>
        public string StockNo { get; set; }

        /// <summary>
        /// 作废、替代计划单流水号 多条时以 '|' 分割 对作废计划单 存储新合并计划单流水号 对新合并计划单 存储原计划单流水号 
        /// </summary>
        public string ReplacePlanNo { get; set; }

        /// <summary>
        /// 备注 
        /// </summary>
        public string Mark { get; set; }

        /// <summary>
        /// 操作员 
        /// </summary>
        public string OperCode { get; set; }

        /// <summary>
        /// 操作日期 
        /// </summary>
        public string OperDate { get; set; }

        /// <summary>
        /// 扩展字段 
        /// </summary>
        public string ExtendField { get; set; }

        /// <summary>
        /// 状态 0 未生成出库单 1 已生成出库单 2 已推送
        /// </summary>
        public string Status { get; set; }

    }
}