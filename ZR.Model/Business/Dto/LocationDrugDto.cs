
namespace ZR.Model.Business.Dto
{
    /// <summary>
    /// 货位药品查询对象
    /// </summary>
    public class LocationDrugQueryDto : PagerInfo 
    {
        public int? LocationId { get; set; }
        public string DrugtermId { get; set; }
    }

    /// <summary>
    /// 货位药品输入输出对象
    /// </summary>
    public class LocationDrugDto
    {
        [Required(ErrorMessage = "Id不能为空")]
        [ExcelColumn(Name = "Id")]
        [ExcelColumnName("Id")]
        public int Id { get; set; }

        [ExcelColumn(Name = "货位")]
        [ExcelColumnName("货位")]
        public int? LocationId { get; set; }

        [ExcelColumn(Name = "药品编号")]
        [ExcelColumnName("药品编号")]
        public string DrugtermId { get; set; }

        [ExcelColumn(Name = "创建时间", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("创建时间")]
        public DateTime? CreateTime { get; set; }

        [ExcelColumn(Name = "创建人")]
        [ExcelColumnName("创建人")]
        public string CreateBy { get; set; }

        [ExcelColumn(Name = "修改时间", Format = "yyyy-MM-dd HH:mm:ss", Width = 20)]
        [ExcelColumnName("修改时间")]
        public DateTime? UpdateTime { get; set; }

        [ExcelColumn(Name = "修改人")]
        [ExcelColumnName("修改人")]
        public string UpdateBy { get; set; }



    }
}