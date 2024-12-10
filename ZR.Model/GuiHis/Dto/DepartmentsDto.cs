namespace ZR.Model.GuiHis.Dto
{
    /// <summary>
    /// 科室查询对象
    /// </summary>
    public class DepartmentsQueryDto : PagerInfo
    {
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string SpellCode { get; set; }
        public string WbCode { get; set; }
    }

    /// <summary>
    /// 科室输入输出对象
    /// </summary>
    public class DepartmentsDto
    {
        [ExcelColumn(Name = "科室编码")]
        [ExcelColumnName("科室编码")]
        public string DeptCode { get; set; }

        [ExcelColumn(Name = "科室英文")]
        [ExcelColumnName("科室英文")]
        public string DeptEname { get; set; }

        [ExcelColumn(Name = "科室名称")]
        [ExcelColumnName("科室名称")]
        public string DeptName { get; set; }

        [ExcelColumn(Name = "科室拼音码")]
        [ExcelColumnName("科室拼音码")]
        public string SpellCode { get; set; }

        [ExcelColumn(Name = "科室五笔码")]
        [ExcelColumnName("科室五笔码")]
        public string WbCode { get; set; }



    }
}