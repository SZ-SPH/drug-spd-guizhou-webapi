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
    //public class PhaInPlanInQuery
    //{
    //    public DateTime beginTime { get; set; }
    //    public DateTime endTime { get; set; }
    //}

    [SugarTable("v_phainplan")]
    public class VPhainPlan
    {
       
        public decimal PlanNo { get; set; } // 入库计划流水号
     
        public decimal? Qty { get; set; }

        //        "companyCode": "2014",
        //"companyName": "黔南神奇医药有限公司"
    }

}

