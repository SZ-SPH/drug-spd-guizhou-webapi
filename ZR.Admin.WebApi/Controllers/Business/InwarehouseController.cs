using Microsoft.AspNetCore.Mvc;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Service.Business.IBusinessService;
using ZR.Admin.WebApi.Filters;
using ZR.Service.Business;

//创建时间：2024-12-10
namespace ZR.Admin.WebApi.Controllers.Business
{
    /// <summary>
    /// 入库主单
    /// </summary>
    [Verify]
    [Route("business/Inwarehouse")]
    public class InwarehouseController : BaseController
    {
        /// <summary>
        /// 入库主单接口
        /// </summary>
        private readonly IInwarehouseService _InwarehouseService;

        public InwarehouseController(IInwarehouseService InwarehouseService)
        {
            _InwarehouseService = InwarehouseService;
        }

        /// <summary>
        /// 查询入库主单列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "inwarehouse:list")]
        public IActionResult QueryInwarehouse([FromQuery] InwarehouseQueryDto parm)
        {
            var response = _InwarehouseService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询入库主单详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "inwarehouse:query")]
        public IActionResult GetInwarehouse(int Id)
        {
            var response = _InwarehouseService.GetInfo(Id);
            
            var info = response.Adapt<InwarehouseDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加入库主单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "inwarehouse:add")]
        [Log(Title = "入库主单", BusinessType = BusinessType.INSERT)]
        public IActionResult AddInwarehouse([FromBody] InwarehouseDto parm)
        {
            var modal = parm.Adapt<Inwarehouse>().ToCreate(HttpContext);

            var response = _InwarehouseService.AddInwarehouse(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新入库主单
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "inwarehouse:edit")]
        [Log(Title = "入库主单", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateInwarehouse([FromBody] InwarehouseDto parm)
        {
            var modal = parm.Adapt<Inwarehouse>().ToUpdate(HttpContext);
            var response = _InwarehouseService.UpdateInwarehouse(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除入库主单
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "inwarehouse:delete")]
        [Log(Title = "入库主单", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteInwarehouse([FromRoute]string ids)
        {  
            var idArr = Tools.SplitAndConvert<int>(ids);
            string num = "";
            foreach (var id in idArr) { 
                string res = _InwarehouseService.DeleteInwarehouse(id);
                if (res == "成功")
                {                   
                }
                else
                {
                    num = num + "," + res;
                }
            }
            if (num=="")
            {
                return SUCCESS("处理成功");
            }
            else
            {
                return SUCCESS(num);

            }
        }

    }
}