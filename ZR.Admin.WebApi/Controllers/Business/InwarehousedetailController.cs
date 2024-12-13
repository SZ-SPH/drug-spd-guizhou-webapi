using Microsoft.AspNetCore.Mvc;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Service.Business.IBusinessService;
using ZR.Admin.WebApi.Filters;

//创建时间：2024-12-10
namespace ZR.Admin.WebApi.Controllers.Business
{
    /// <summary>
    /// 添加条目详细
    /// </summary>
    [Verify]
    [Route("business/Inwarehousedetail")]
    public class InwarehousedetailController : BaseController
    {
        /// <summary>
        /// 添加条目详细接口
        /// </summary>
        private readonly IInwarehousedetailService _InwarehousedetailService;

        public InwarehousedetailController(IInwarehousedetailService InwarehousedetailService)
        {
            _InwarehousedetailService = InwarehousedetailService;
        }

        /// <summary>
        /// 查询添加条目详细列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "inwarehousedetail:list")]
        public IActionResult QueryInwarehousedetail([FromQuery] InwarehousedetailQueryDto parm)
        {
            var response = _InwarehousedetailService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询添加条目详细详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "inwarehousedetail:query")]
        public IActionResult GetInwarehousedetail(int Id)
        {
            var response = _InwarehousedetailService.GetInfo(Id);
            
            var info = response.Adapt<InwarehousedetailDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加添加条目详细
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "inwarehousedetail:add")]
        [Log(Title = "添加条目详细", BusinessType = BusinessType.INSERT)]
        public IActionResult AddInwarehousedetail([FromBody] InwarehousedetailDto parm)
        {
            var modal = parm.Adapt<Inwarehousedetail>().ToCreate(HttpContext);

            var response = _InwarehousedetailService.AddInwarehousedetail(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新添加条目详细
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "inwarehousedetail:edit")]
        [Log(Title = "添加条目详细", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateInwarehousedetail([FromBody] InwarehousedetailDto parm)
        {
            var modal = parm.Adapt<Inwarehousedetail>().ToUpdate(HttpContext);
            var response = _InwarehousedetailService.UpdateInwarehousedetail(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除添加条目详细
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "inwarehousedetail:delete")]
        [Log(Title = "添加条目详细", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteInwarehousedetail([FromRoute]string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);

            return ToResponse(_InwarehousedetailService.Delete(idArr));
        }

    }
}