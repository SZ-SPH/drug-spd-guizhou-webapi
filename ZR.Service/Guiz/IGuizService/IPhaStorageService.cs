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
        PagedInfo<PhaStorageDtos> exGetList(PhaStorageQueryDto parm);

        
        PhaStorage GetInfo(int Id);
        PhaStorage GetisInfo(string DrugCode, string DeptCode);


        PhaStorage AddPhaStorage(PhaStorage parm);
        int MIXAddPhaStorage(List<PhaStorage> parm);

        int UpdatePhaStorage(PhaStorage parm);

        bool TruncatePhaStorage();
        /// <returns></returns>
        bool TruncatePhaStoragesss();

        (string, object, object) ImportPhaStorage(List<PhaStorage> list);

        PagedInfo<PhaStorageDto> ExportList(PhaStorageQueryDto parm);
        List<PhaStorage> GetALL(string DeptCode);
       decimal? GetALLme(string DeptCode);

    }
}
