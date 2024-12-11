using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.GuiHis
{
    //http://192.168.1.95:7801/roc/order-service/api/v1/order/order-term/drugstore/queryDrugStoreList

    public class ApiDrugStore
    {
        public int Code { get; set; } // 响应状态码
        public string Msg { get; set; } // 响应消息
        public List<DrugStore> Data { get; set; } // 数据列表
    }
    [SugarTable("DrugStore")]
    public class DrugStore
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]

        public string DrugDeptCode { get; set; } // 药房代码
        public string DrugDeptName { get; set; } // 药房名称
        public int StoreSum { get; set; } // 库存总量
        public int PreoutSum { get; set; } // 预出总量
    }

}
