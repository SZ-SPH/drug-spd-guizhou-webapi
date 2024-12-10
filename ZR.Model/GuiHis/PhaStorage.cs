using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.GuiHis
{
    //库存
    //post 请求
    //http://192.168.2.21:9403/His/GetPhaStorage?drugDeptCode=6052
    public class PhaStorageInQuery
    {
        public string DrugDeptCode { get; set; }
        public string DrugCode { get; set; }
        public string TradeName { get; set; }
        public string ProducerName { get; set; }
        public int? StartIndex { get; set; }
        public int? EndIndex { get; set; }
    }
    [SugarTable("PhaStorage")]

    public class PhaStorage
    {
        public string DrugDeptCode { get; set; }
        public string DrugCode { get; set; }
        public string TradeName { get; set; }
        public string Specs { get; set; }
        public string DrugType { get; set; }
        public string DrugQuality { get; set; }
        public decimal? RetailPrice { get; set; }
        public string PackUnit { get; set; }
        public int? PackQty { get; set; }
        public string MinUnit { get; set; }
        public DateTime? ValidDate { get; set; }
        public decimal? StoreSum { get; set; }
        public decimal? StoreCost { get; set; }
        public decimal? PreSum { get; set; }
        public decimal? PreCost { get; set; }
        public decimal? LowSum { get; set; }
        public decimal? TopSum { get; set; }
        public string PlaceCode { get; set; }
        public string DailtycheckFlag { get; set; }
        public string GroupCode { get; set; }
        public string BatchNo { get; set; }
        public string ProducerCode { get; set; }
    }
}
