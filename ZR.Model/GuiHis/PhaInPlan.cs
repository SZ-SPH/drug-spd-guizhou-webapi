using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ZR.Model.GuiHis
{
    //http://192.168.2.21:9403/His/GetPhaInPlanList?beginTime=2024-01-01&endTime=2024-01-31
    //入库
    public class PhaInPlanInQuery
    {
        public DateTime beginTime { get; set; }
        public DateTime endTime { get; set; }
    }

    [SugarTable("PhaInPlan")]
    public class PhaInPlan
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public decimal PlanNo { get; set; } // 入库计划流水号
        public string ApproveDate {  get; set; }
        public string ApproveEmpl { get; set; }
        public decimal StockNum {  get; set; }
        public string BillCode { get; set; } // 采购单号

        public string State { get; set; } // 单据状态 0 计划单，1 采购单

        public string PlanType { get; set; } // 计划类型0手工计划，1警戒线，2消耗，3时间，4日消耗

        public string DrugDeptCode { get; set; } // 科室编码

        public string DrugCode { get; set; } // 药品编码

        public string TradeName { get; set; } // 药品名称

        public string Specs { get; set; } // 规格

        public decimal? RetailPrice { get; set; } // 参考零售价

        public decimal? WholesalePrice { get; set; } // 参考批发价

        public decimal? PurchasePrice { get; set; } // 最新购入价

        public string PackUnit { get; set; } // 包装单位

        public decimal? PackQty { get; set; } // 包装数量

        public string MinUnit { get; set; } // 最小单位

        public string ProducerCode { get; set; } // 生产厂家编码

        public string ProducerName { get; set; } // 生产厂家名称

        public decimal? StoreNum { get; set; } // 本科室库存数量
        public decimal? StoreTotsum { get; set; } // 全院库存总和
        public decimal? OutputSum { get; set; } // 全院出库总量
        public decimal? PlanNum { get; set; } // 计划入库量
        public string PlanEmpl { get; set; } // 计划人

        public DateTime? PlanDate { get; set; } // 计划日期

        public string StockEmpl { get; set; } // 采购人

        public DateTime? StockDate { get; set; } // 采购日期

        public string StockNo { get; set; } // 采购流水号

        public string ReplacePlanNo { get; set; } // 作废、替代计划单流水号

        public string Mark { get; set; } // 备注

        public string OperCode { get; set; } // 操作员

        public DateTime? OperDate { get; set; } // 操作日期

        public string ExtendField { get; set; } // 扩展字段
        public string Status { get; set; } // 生成出库单 0未生成 1已生成
    }

}

