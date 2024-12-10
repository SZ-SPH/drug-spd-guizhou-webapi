using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.GuiHis
{

    [SugarTable("Departments")]
    public class Departments
    {
        public string DeptCode { get; set; }       // 科室编码
        public string DeptEname { get; set; }      // 科室英文
        public string DeptName { get; set; }       // 科室名称
        public string SpellCode { get; set; }      // 科室拼音码
        public string WbCode { get; set; }         // 科室五笔码
    }

    public class ReturnDept
    {
        public string DeptCode { get; set; } // 科室代码
        public string DeptName { get; set; } // 科室名称
        public string SpellCode { get; set; } // 拼音码
        public string WbCode { get; set; } // 五笔码
        public string DeptEname { get; set; } // 科室英文名称
        public string MediTime { get; set; } // 就诊时间
        public DateTime? CycleBegin { get; set; } // 周期开始
        public DateTime? CycleEnd { get; set; } // 周期结束
        public string RegdeptFlag { get; set; } // 挂号科室标志
        public string TatdeptFlag { get; set; } // TAT科室标志
        public string DeptPro { get; set; } // 科室性质
        public decimal? AlterMoney { get; set; } // 调整金额
        public string ExtFlag { get; set; } // 扩展标志
        public string Ext1Flag { get; set; } // 扩展标志1
        public string ValidState { get; set; } // 有效状态
        public int? SortId { get; set; } // 排序ID
        public string OperCode { get; set; } // 操作员代码
        public DateTime OperDate { get; set; } // 操作日期
        public string UserCode { get; set; } // 用户代码
        public string SimpleName { get; set; } // 简称
        public string Remark { get; set; } // 备注
        public DateTime CreateDate { get; set; } // 创建日期
        public string DeptProfCode { get; set; } // 科室专业代码
        public string DeptDesc { get; set; } // 科室描述
        public string DeptTel { get; set; } // 科室电话
        public string SubBranchCode { get; set; } // 子分支代码
        public string BranchCode { get; set; } // 分支代码
        public string DeptAddress { get; set; } // 科室地址
    }

    public class DepartmentsQuery
    {
        public string DeptCode { get; set; }              // 科室编码
        public string DeptType { get; set; }              // 类型

    }
    public class QueryResponse
    {
        public int? Code { get; set; }              // 返回状态码
        public List<ReturnDept> Data { get; set; }  // 科室信息列表
        public string Msg { get; set; }              // 返回消息

    }

}
