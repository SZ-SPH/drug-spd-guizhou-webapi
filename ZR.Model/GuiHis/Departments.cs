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

    public class QueryResponse
    {
        public int? Code { get; set; }              // 返回状态码
        public List<Departments> Data { get; set; }  // 科室信息列表
        public string Msg { get; set; }              // 返回消息

    }

}
