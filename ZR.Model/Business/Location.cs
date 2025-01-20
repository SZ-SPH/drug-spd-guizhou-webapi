
namespace ZR.Model.Business
{
    /// <summary>
    /// 货位
    /// </summary>
    [SugarTable("location")]
    public class Location
    {
        /// <summary>
        /// Id 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public int Id { get; set; }
        public string Orderby { get; set; }

        
        /// <summary>
        /// 货位号 
        /// </summary>
        public int? LocationNumber { get; set; }

        /// <summary>
        /// 货位名称 
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// 状态 
        /// </summary>
        public string State { get; set; }

    }
}