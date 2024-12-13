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


    /// <summary>
    /// 生成入库单DTO
    /// </summary>
    public class InwarehouseGenerateInwarehouseDto
    {
        public string BillCode { get; set; }
        public List<string> PlanNos { get; set; }
        public int StockNum { get; set; }
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

        public int PushStatu { get; set; }
        //public string[] PurchaseOrderNum { get; set; }
    }
}