using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.GuiHis
{
    //http://192.168.1.95:7801/roc/curr-web/api/v1/curr/pharmaceutical/company?facCode=2002&compantyType=1&validFlag=0
    //供应商和生产厂家
    public class CompanyInres
    {
        public string facCode { get; set; } 
        public string companyType { get; set; } 
        public string validFlag { get; set; } 

//facCode 否   VARCHAR2(10)    供货商或生产厂家编码，不传时，返回全部
//companyType 否 VARCHAR2(1) 公司类别：0－生产厂家，1－供销商
//validFlag   否 VARCHAR2(1) 有效性 0 无效 1 有效

    }

    public class CompanyResponse
    {
        public int Code { get; set; } // 响应码
        public string Msg { get; set; } // 响应消息
        public List<CompanyInfo> Data { get; set; } // 公司数据列表
    }

    [SugarTable("CompanyInfo")]

    public class CompanyInfo
    {
        public string FacCode { get; set; } // 公司编码
        public string FacName { get; set; } // 公司名称
        public string Address { get; set; } // 公司地址
        public string Relation { get; set; } // 联系方式
        public string GmpInfo { get; set; } // GMP信息
        public string GspInfo { get; set; } // GSP信息
        public string SpellCode { get; set; } // 拼音码
        public string WbCode { get; set; } // 五笔码
        public string CustomCode { get; set; } // 自定义码
        public string CompanyType { get; set; } // 公司类别：0－生产厂家，1－供销商
        public string OpenBank { get; set; } // 开户银行
        public string OpenAccounts { get; set; } // 开户账号
        public string ActualRate { get; set; } // 政策扣率
        public string Remark { get; set; } // 备注
        public string OperCode { get; set; } // 操作员编码
        public DateTime OperDate { get; set; } // 操作日期
        public string ValidFlag { get; set; } // 有效性 0 无效 1 有效
    }
}
