using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.Business
{
    /// <summary>
    /// 入库主单
    /// </summary>
    [SugarTable("t_inwarehouse")]
    public class Inwarehouse
    {
        /// <summary>
        /// Id 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 入库单号 
        /// </summary>
        public string InwarehouseNum { get; set; }

        /// <summary>
        /// 创建时间 
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建人 
        /// </summary>
        public string CreateMan { get; set; }

        /// <summary>
        /// 备注 
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 入库库区 
        /// </summary>
        public string InwarehouseArea { get; set; }

        /// <summary>
        /// 采购计划流水号 
        /// </summary>
        public string PlanNo { get; set; }

        /// <summary>
        /// 采购数量
        /// </summary>
        public int StockNum { get; set; }

        /// <summary>
        /// 采购订单号
        /// </summary>
        public string PurchaseOrderNum { get; set; }

        /// <summary>
        /// 推送状态
        /// </summary>
        public string PushStatu { get; set; }

    }
}
