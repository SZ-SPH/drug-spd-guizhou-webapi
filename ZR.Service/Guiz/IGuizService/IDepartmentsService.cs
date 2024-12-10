using ZR.Model.GuiHis;
using ZR.Model.GuiHis.Dto;

namespace ZR.Service.Guiz.IGuizService
{
    /// <summary>
    /// 科室service接口
    /// </summary>
    public interface IDepartmentsService : IBaseService<Departments>
    {
        PagedInfo<DepartmentsDto> GetList(DepartmentsQueryDto parm);

        Departments GetInfo(string DeptCode);


        Departments AddDepartments(Departments parm);
        int UpdateDepartments(Departments parm);

        bool TruncateDepartments();

        (string, object, object) ImportDepartments(List<Departments> list);

        PagedInfo<DepartmentsDto> ExportList(DepartmentsQueryDto parm);
    }
}
