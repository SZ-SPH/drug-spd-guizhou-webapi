
namespace ZR.Model.Business.Dto
{
    /// <summary>
    /// 添加条目详细查询对象
    /// </summary>
    public class InwarehousedetailQueryDto : PagerInfo 
    {

        public string DrugCode { get; set; }

        public int InwarehouseQty { get; set; }

        public string Remark { get; set; }

        public DateTime CreateTime { get; set; }

        [Required(ErrorMessage = "入库单id不能为空")]
        public int InwarehouseId { get; set; }

    }

    /// <summary>
    /// 添加条目详细输入输出对象
    /// </summary>
    public class InwarehousedetailDto
    {
        [Required(ErrorMessage = "Id不能为空")]
        public int Id { get; set; }
        public string Tstars { get; set; }
        public string DrugCode { get; set; }
        public string ProductCode { get; set; }
        public int? InwarehouseQty { get; set; }

        public string Remark { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? InwarehouseId { get; set; }
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

        public string ValiDate { get; set; }

        public string ProductDate { get; set; }

        public decimal MixBuyPrice { get; set; }
        /// <summary>
        /// 最小单位零售价
        /// </summary>
        public decimal MixOutPrice { get; set; }
        public string InName { get; set; }
    }

    public class InwarehousedetaiWithDruglDto
    {

        public string ProductCode { get; set; }
        public string Tstars { get; set; }
        
        public string BatchNo { get; set; }

        public decimal MixBuyPrice { get; set; }
        /// <summary>
        /// 最小单位零售价
        /// </summary>
        public decimal MixOutPrice { get; set; }
        public string InName { get; set; }
        /// <summary>
        /// 批文信息
        /// </summary>
        public string ApproveInfo { get; set; }

        public string ValiDate { get; set; }

        public string ProductDate { get; set; }
        public int Id { get; set; }
        public string DrugCode { get; set; }
        public int InwarehouseQty { get; set; }
        public string PurChaseOrderNum { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public int InwarehouseId { get; set; }
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

    }

}