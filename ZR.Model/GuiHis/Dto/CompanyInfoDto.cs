namespace ZR.Model.GuiHis.Dto
{
    /// <summary>
    /// 厂家和供应商查询对象
    /// </summary>
    public class CompanyInfoQueryDto : PagerInfo
    {
        public string FacCode { get; set; }
        public string FacName { get; set; }
        public string SpellCode { get; set; }
        public string WbCode { get; set; }
        public string CustomCode { get; set; }
        public string CompanyType { get; set; }
        public string Remark { get; set; }
        public string ValidFlag { get; set; }
    }

    /// <summary>
    /// 厂家和供应商输入输出对象
    /// </summary>
    public class CompanyInfoDto
    {
        [ExcelColumn(Name = "公司编码")]
        [ExcelColumnName("公司编码")]
        public string FacCode { get; set; }

        [ExcelColumn(Name = "公司名称")]
        [ExcelColumnName("公司名称")]
        public string FacName { get; set; }

        [ExcelColumn(Name = "公司地址")]
        [ExcelColumnName("公司地址")]
        public string Address { get; set; }

        [ExcelColumn(Name = "联系方式")]
        [ExcelColumnName("联系方式")]
        public string Relation { get; set; }

        [ExcelColumn(Name = "GMP信息")]
        [ExcelColumnName("GMP信息")]
        public string GmpInfo { get; set; }

        [ExcelColumn(Name = "GSP信息")]
        [ExcelColumnName("GSP信息")]
        public string GspInfo { get; set; }

        [ExcelColumn(Name = "拼音码")]
        [ExcelColumnName("拼音码")]
        public string SpellCode { get; set; }

        [ExcelColumn(Name = "五笔码")]
        [ExcelColumnName("五笔码")]
        public string WbCode { get; set; }

        [ExcelColumn(Name = "自定义码")]
        [ExcelColumnName("自定义码")]
        public string CustomCode { get; set; }

        [ExcelColumn(Name = "公司类别：0－生产厂家，1－供销商")]
        [ExcelColumnName("公司类别：0－生产厂家，1－供销商")]
        public string CompanyType { get; set; }

        [ExcelColumn(Name = "开户银行")]
        [ExcelColumnName("开户银行")]
        public string OpenBank { get; set; }

        [ExcelColumn(Name = "开户账号")]
        [ExcelColumnName("开户账号")]
        public string OpenAccounts { get; set; }

        [ExcelColumn(Name = "政策扣率")]
        [ExcelColumnName("政策扣率")]
        public string ActualRate { get; set; }

        [ExcelColumn(Name = "备注")]
        [ExcelColumnName("备注")]
        public string Remark { get; set; }

        [ExcelColumn(Name = "操作员编码")]
        [ExcelColumnName("操作员编码")]
        public string OperCode { get; set; }

        [ExcelColumn(Name = "操作日期", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("操作日期")]
        public DateTime? OperDate { get; set; }

        [ExcelColumn(Name = "有效性 0 无效 1 有效")]
        [ExcelColumnName("有效性 0 无效 1 有效")]
        public string ValidFlag { get; set; }



        [ExcelColumn(Name = "公司类别：0－生产厂家，1－供销商")]
        public string CompanyTypeLabel { get; set; }
    }
}