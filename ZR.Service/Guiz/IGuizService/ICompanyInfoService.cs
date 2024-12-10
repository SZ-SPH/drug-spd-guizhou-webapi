using ZR.Model.GuiHis;
using ZR.Model.GuiHis.Dto;

namespace ZR.Service.Guiz.IGuizService
{
    /// <summary>
    /// 厂家和供应商service接口
    /// </summary>
    public interface ICompanyInfoService : IBaseService<CompanyInfo>
    {
        PagedInfo<CompanyInfoDto> GetList(CompanyInfoQueryDto parm);

        CompanyInfo GetInfo(string facCode);


        CompanyInfo AddCompanyInfo(CompanyInfo parm);
        int UpdateCompanyInfo(CompanyInfo parm);

        bool TruncateCompanyInfo();

        (string, object, object) ImportCompanyInfo(List<CompanyInfo> list);

        PagedInfo<CompanyInfoDto> ExportList(CompanyInfoQueryDto parm);
    }
}
