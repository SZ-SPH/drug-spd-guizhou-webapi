using ZR.Model.GuiHis;
using ZR.Model.GuiHis.Dto;

namespace ZR.Service.Guiz.IGuizService
{
    /// <summary>
    /// 库存service接口
    /// </summary>
    public interface IPhaStorageService : IBaseService<PhaStorage>
    {
        PagedInfo<PhaStorageDto> GetList(PhaStorageQueryDto parm);

        PhaStorage GetInfo(int Id);
        PhaStorage GetisInfo(string DrugCode, string DeptCode);


        PhaStorage AddPhaStorage(PhaStorage parm);
        int UpdatePhaStorage(PhaStorage parm);

        bool TruncatePhaStorage();

        (string, object, object) ImportPhaStorage(List<PhaStorage> list);

        PagedInfo<PhaStorageDto> ExportList(PhaStorageQueryDto parm);
    }
}
