using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ZR.Model.Business.Dto
{
    /// <summary>
    /// 入库主单查询对象
    /// </summary>
    public class InwarehouseQueryDto : PagerInfo
    {
        public string InwarehouseNum { get; set; }

        public DateTime CreateTime { get; set; }

        public string CreateMan { get; set; }

        public string Remark { get; set; }

        public int InwarehouseDetailId { get; set; }

        public string InwarehouseArea { get; set; }

        public string PlanNo { get; set; }

        public int StockNum { get; set; }

        public string PurchaseOrderNum { get; set; }
    }


    public class AppendInwarehouseDetail
    {
        public string PlanNo { get; set; }
        public string BillCode { get; set; }
        public int StockNum { get; set; }
        public string DrugCode { get; set; }
        public string InwarehouseNum { get; set; }
    }

    /// <summary>
    /// 生成入库单DTO 废弃
    /// </summary>
    public class InwarehouseGenerateInwarehouseDto
    {
        //采购计划单号
        public string BillCode { get; set; }
        public string PurchaseNum { get; set; }
        public string inwarehouseArea { get; set; }

        public List<string> PlanNos { get; set; }
        public int StockNum { get; set; }
        public string BillTime { get; set; }
        public string BillTimeFormat { get; set; }
        public string SupplierCode { get; set; }
    }

    /// <summary>
    /// 入库主单输入输出对象
    /// </summary>
    public class InwarehouseDto
    {
        [Required(ErrorMessage = "Id不能为空")]
        public int Id { get; set; }

        public string InwarehouseNum { get; set; }

        public DateTime CreateTime { get; set; }

        public string CreateMan { get; set; }

        public string Remark { get; set; }

        public int InwarehouseDetailId { get; set; }

        public string InwarehouseArea { get; set; }

        public string PlanNo { get; set; }

        public int StockNum { get; set; }

        public string PushStatu { get; set; }
        public string BillCode { get; set; }
        public string BillTime { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        //public string[] PurchaseOrderNum { get; set; }
    }
}