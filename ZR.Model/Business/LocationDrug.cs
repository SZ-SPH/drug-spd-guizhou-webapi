
namespace ZR.Model.Business
{
    /// <summary>
    /// 货位药品
    /// </summary>
    [SugarTable("LocationDrug")]
    public class LocationDrug
    {
        /// <summary>
        /// Id 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 货位 
        /// </summary>
        public int? LocationId { get; set; }

        /// <summary>
        /// 药品编号 
        /// </summary>
        public string DrugtermId { get; set; }

        /// <summary>
        /// 创建时间 
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建人 
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 修改时间 
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 修改人 
        /// </summary>
        public string UpdateBy { get; set; }

    }
}