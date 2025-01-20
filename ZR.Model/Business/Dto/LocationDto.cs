
namespace ZR.Model.Business.Dto
{
    /// <summary>
    /// 货位查询对象
    /// </summary>
    public class LocationQueryDto : PagerInfo 
    {
        public int? LocationNumber { get; set; }
        public string LocationName { get; set; }
        public string State { get; set; }
    }

    /// <summary>
    /// 货位输入输出对象
    /// </summary>
    public class LocationDto
    {
        [Required(ErrorMessage = "Id不能为空")]
        [ExcelColumn(Name = "Id")]
        [ExcelColumnName("Id")]
        public int Id { get; set; }

        [ExcelColumn(Name = "货位号")]
        [ExcelColumnName("货位号")]
        public int? LocationNumber { get; set; }

        [ExcelColumn(Name = "货位名称")]
        [ExcelColumnName("货位名称")]
        public string LocationName { get; set; }

        [ExcelColumn(Name = "状态")]
        [ExcelColumnName("状态")]
        public string State { get; set; }

        public string Orderby { get; set; }


        [ExcelColumn(Name = "状态")]
        public string StateLabel { get; set; }
    }
}