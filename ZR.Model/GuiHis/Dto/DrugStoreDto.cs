using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Model.GuiHis.Dto

{ 
        public class DrugStoreQueryDto : PagerInfo
        {
            public string DrugDeptCode { get; set; }
            public string DrugDeptName { get; set; }
        }

        /// <summary>
        /// </summary>
        public class DrugStoreDto
    {
        public string DrugDeptCode { get; set; } // 药房代码
        public string DrugDeptName { get; set; } // 药房名称
        public int StoreSum { get; set; } // 库存总量
        public int PreoutSum { get; set; } // 预出总量

    }
}
