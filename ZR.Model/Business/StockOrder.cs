namespace ZR.Model.Business
{
    /// <summary>
    /// ������
    /// </summary>
    [SugarTable("StockOrder")]
    public class StockOrder
    {
        /// <summary>
        /// Id 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// ��ⵥid 
        /// </summary>
        public int? ReceiptId { get; set; }

        /// <summary>
        /// ҩƷid 
        /// </summary>
        public int? DrugId { get; set; }

        /// <summary>
        /// �������� 
        /// </summary>
        public int? DeliveryQuantity { get; set; }

        /// <summary>
        /// ����ҽԺ 
        /// </summary>
        public string DeliveryHospital { get; set; }

        /// <summary>
        /// ���͵�ַ 
        /// </summary>
        public string DeliveryAddress { get; set; }

        /// <summary>
        /// ��ע 
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// ����ʱ�� 
        /// </summary>
        public string DeliveryTime { get; set; }

        /// <summary>
        /// ������ 
        /// </summary>
        public string DeliveryPerson { get; set; }

    }
}