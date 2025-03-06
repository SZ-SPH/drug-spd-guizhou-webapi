using Microsoft.AspNetCore.Mvc;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Service.Business.IBusinessService;
using ZR.Admin.WebApi.Filters;
using Newtonsoft.Json;
using ZR.Model.GuiHis;
using ZR.Service.Business;
using ZR.Service.Guiz.IGuizService;
using Org.BouncyCastle.Bcpg;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NPOI.SS.Formula.Functions;
using System.Text;
using ZR.Service.Guiz;
using System.Web;
using SqlSugar;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using JinianNet.JNTemplate;


//创建时间：2024-12-10
namespace ZR.Admin.WebApi.Controllers.Business
{
    /// <summary>
    /// 采购计划入库
    /// </summary>
    [Verify]
    [Route("business/TGInwarehouse")]
    public class TGInwarehouseController : BaseController
    {
        /// <summary>
        /// 采购计划入库接口
        /// </summary>
        private readonly ITGInwarehouseService _TGInwarehouseService;

        private readonly IInwarehouseService _InwarehouseService;
        private readonly IPhaInPlanService _PhaInPlanService;
        private readonly IInwarehousedetailService _inwarehousedetailService;
        private readonly ISysUserService _SysUserService;


        public TGInwarehouseController(IInwarehousedetailService inwarehousedetailService, ITGInwarehouseService TGInwarehouseService, IInwarehouseService inwarehouseService,IPhaInPlanService phaInPlanService,
               ISysUserService sysUserService)
        {
            _TGInwarehouseService = TGInwarehouseService;
            _InwarehouseService = inwarehouseService;
            _PhaInPlanService = phaInPlanService;
            _inwarehousedetailService=inwarehousedetailService; 
            _SysUserService = sysUserService;
        }

        /// <summary>
        /// 查询采购计划入库列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "tginwarehouse:list")]
        public IActionResult QueryTGInwarehouse([FromQuery] TGInwarehouseQueryDto parm)
        {
            var response = _TGInwarehouseService.GetList(parm);
            return SUCCESS(response);
        }

        [HttpPost("push")]
        public IActionResult PushInwarehouseInfoToHis([FromBody] Push parm)
        {
            var respones = _TGInwarehouseService.PushInwarehouseInfoToHis(parm);
            return SUCCESS(respones);
        }

        [HttpPost("returnPush")]
        public IActionResult returnPush([FromBody] Inwarehousedetail row)
        {
            int respones = _TGInwarehouseService.returnPush(row.Id);
            return SUCCESS(respones>0?"true":"false");
        }
        [HttpGet("AddPlan")]
        public IActionResult Addplan([FromQuery] AddDec parm)
        {
            //获取入库信息
            var ow = _InwarehouseService.GetInfo(parm.OnId);    
            var pha = _PhaInPlanService.GetInfo(decimal.Parse(parm.PlanNo));
            var o = _inwarehousedetailService.GetInfos(ow.Id);
           
            if (o.FirstOrDefault(i=>i.Tstars=="已推送")!=null)
            {
                return SUCCESS("已经存在推送的数据。请重新创建入库单");
            }
            if (ow.SupplierCode != pha.CompanyCode) return SUCCESS("供应商不同");
            if (pha.EndDate<DateTime.Now) return SUCCESS("超过截止时间");
            if (ow.InwarehouseArea!=pha.DrugDeptCode)
            {
                return SUCCESS("入库库区不同");
            }
            Inwarehousedetail d=new Inwarehousedetail();
            d.SerialNum = parm.PlanNo;
            d.BatchNo=parm.BatchNo;
            d.ApproveInfo= parm.ApproveInfo;
            d.ProductDate=parm.ProductDate;
            d.ValiDate=parm.ValiDate;
            d.InwarehouseId = ow.Id;
            d.InwarehouseQty = (int)parm.Num;
            d.DrugCode = pha.DrugCode;
            d.ProductCode = parm.ProductCode;
            d.MixOutPrice=parm.MixOutPrice;
            d.MixBuyPrice=  parm.MixBuyPrice;
            d.InName = parm.InName;
            var modal = d.Adapt<Inwarehousedetail>().ToCreate(HttpContext);
            var response = _inwarehousedetailService.AddInwarehousedetail(modal);

            //var respones = _TGInwarehouseService.PushInwarehouseInfoToHis(parm);
            return SUCCESS("成功");
        }


        /// <summary>
        /// 生成入库单
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("generateInwarehouse")]
        [ActionPermissionFilter(Permission = "tginwarehouse:generateInwarehouse")]
        public IActionResult GenerateInwarehouse([FromBody] List<InwarehouseGenerateInwarehouseDto> parm)
        {
            var response = _InwarehouseService.generateInwarehouse(parm);
            return SUCCESS(response ? "处理成功" : "处理失败");
        }


        /// <summary>
        /// 生成可选出库单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("generateSelectiveInwarehouse")]
        [ActionPermissionFilter(Permission = "tginwarehouse:generateInwarehouse")]
        public IActionResult GenerateSelectiveInwarehouse([FromBody]InwarehouseGenerateInwarehouseDto param)
        {
            var response = _InwarehouseService.generateSelectiveInwarehouse(param);
            return SUCCESS(response ? "处理成功" : "处理失败");
        }

        /// <summary>
        /// 追加入库单明细
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("AppendSelectiveInwarehouse")]
        [ActionPermissionFilter(Permission = "tginwarehouse:generateInwarehouse")]
        public IActionResult AppendSelectiveInwarehouse([FromBody]List<AppendInwarehouseDetail> param)
        {
            var response = _InwarehouseService.AppendSelectiveInwarehouse(param);
            return SUCCESS(response ? "处理成功" : "处理失败");
        }


        /// <summary>
        /// 查询采购计划入库详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "tginwarehouse:query")]
        public IActionResult GetTGInwarehouse(string Id)
        {
            var response = _TGInwarehouseService.GetInfo(Id);
            
            var info = response.Adapt<TGInwarehouseDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加采购计划入库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "tginwarehouse:add")]
        [Log(Title = "采购计划入库", BusinessType = BusinessType.INSERT)]
        public IActionResult AddTGInwarehouse([FromBody] TGInwarehouseDto parm)
        {
            var modal = parm.Adapt<TGInwarehouse>().ToCreate(HttpContext);

            var response = _TGInwarehouseService.AddTGInwarehouse(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新采购计划入库
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "tginwarehouse:edit")]
        [Log(Title = "采购计划入库", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateTGInwarehouse([FromBody] InwarhouseDetailDTO parm)
        {
            //var modal = parm.Adapt<TGInwarehouse>().ToUpdate(HttpContext);
            var response = _TGInwarehouseService.UpdateTGInwarehouse(parm);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除采购计划入库
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "tginwarehouse:delete")]
        [Log(Title = "采购计划入库", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteTGInwarehouse([FromRoute]string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);
          

            return ToResponse(_TGInwarehouseService.Delete(idArr));
        }

        /// <summary>
        /// 导出采购计划入库
        /// </summary>
        /// <returns></returns>
        [Log(Title = "采购计划入库", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "tginwarehouse:export")]
        public IActionResult Export([FromQuery] TGInwarehouseQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _TGInwarehouseService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "采购计划入库", "采购计划入库");
            return ExportExcel(result.Item2, result.Item1);
        }

        static string ConvertToChinese(decimal? amount)
        {
            if (amount == null)
                return "";

            if (amount < 0)
                return "负" + ConvertToChinese(-amount);

            string[] units = { "", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰", "仟" };
            string[] digits = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };

            long integralPart = (long)amount; // 整数部分
            int fractionalPart = (int)((amount.Value - integralPart) * 100); // 小数部分（分）

            string integralStr = ConvertIntegralToChinese(integralPart, units, digits);
            string fractionalStr = ConvertFractionalToChinese(fractionalPart, digits);

            // 如果没有小数部分，添加“整”
            if (fractionalPart == 0)
            {
                return integralStr + "元整";
            }

            return integralStr + "元" + fractionalStr;
        }

        static string ConvertIntegralToChinese(long integralPart, string[] units, string[] digits)
        {
            if (integralPart == 0)
                return digits[0];

            string result = "";
            int unitPos = 0; // 单位位置
            bool zero = false; // 是否存在零

            while (integralPart > 0)
            {
                int digit = (int)(integralPart % 10);
                if (digit > 0)
                {
                    result = digits[digit] + units[unitPos] + result;
                    zero = false;
                }
                else if (!zero)
                {
                    result = digits[0] + result;
                    zero = true;
                }

                integralPart /= 10;
                unitPos++;
            }

            result = result.TrimEnd('零'); // 去掉末尾的零
            return result.TrimStart('零'); // 去掉开头的零
        }

        static string ConvertFractionalToChinese(int fractionalPart, string[] digits)
        {
            if (fractionalPart == 0)
                return "";

            string result = "";
            int jiao = fractionalPart / 10;
            int fen = fractionalPart % 10;

            if (jiao > 0)
                result += digits[jiao] + "角";
            if (fen > 0)
                result += digits[fen] + "分";

            return result;
        }

        /// <summary>
        /// 入库导出
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("Exportss")]
        public async Task<IActionResult> Exportss([FromQuery] List<int> parm)
        {
            //按照入库单信息 进行仓库 药品进行分类
            List<rk> alleout = new();
            List<rkdrugs> lEin = new();
            //var username = HttpContextExtension.GetName(App.HttpContext);
            //Controller获取id
            var id = HttpContext.GetUId();
              var username = _SysUserService.GetById(id).NickName;
            //Controller获取用户名
            //var userName = HttpContext.get;

            //获取登录信息
            foreach (var item in parm)
            {
                reqIn reqIn = new reqIn();
                lEin = new();
                rk rk = new rk();
                var list = _InwarehouseService.GetInfoff(item);
                reqIn.spdInputId = item.ToString();
                var ow = _inwarehousedetailService.GetInfos(item);

                rk.DeptCode = list.InwarehouseArea;
                decimal? SumApp = 0;
                decimal? SumSale = 0;
                rk.InType = "一般入库";
                rk.Code = list.InwarehouseNum;
                rk.Mark = list.Remark;
                rk.BillCode = list.BillCode;
                rk.SupplierName = list.SupplierName;
                rk.GTime = list.PushTime?.ToString("yyyy-MM-dd HH:mm:ss");
                rk.NowgetTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");              
                foreach (var ss in ow)
                 {
                        rkdrugs rkdrugs = new();
                        rkdrugs.TradeName = ss.TradeName;
                        rkdrugs.Specs = ss.Specs;
                        rkdrugs.PackUnit = ss.PackUnit;
                        rkdrugs.PackQty = (ss.InwarehouseQty/ decimal.Parse(ss.PackQty));
                        rkdrugs.PurchasePrice = (Math.Floor(ss.MixBuyPrice * 10000) / 10000).ToString("F4");
                        rkdrugs.ApproveCost = (ss.MixBuyPrice * rkdrugs.PackQty).ToString();
                        rkdrugs.RetailPrice = ss.MixOutPrice.ToString();
                        rkdrugs.SaleCost = (ss.MixOutPrice * rkdrugs.PackQty).ToString();
                        rkdrugs.ProducerCode = ss.ProducerName.ToString();
                        rkdrugs.ValidDate = ss.ValiDate;
                        rkdrugs.BatchNo = ss.BatchNo;
                        SumApp += ss.MixBuyPrice * rkdrugs.PackQty;
                        SumSale += ss.MixOutPrice * rkdrugs.PackQty;
                        lEin.Add(rkdrugs);
                        reqIn.planNo = ss.PlanNo;
                        reqIn.drugCode = ss.DrugCode;
                    }
                    reqIn.companyCode = list.SupplierCode;
                    rk.Drugs = lEin;
                    rk.SumApproveCost = SumApp.ToString();
                    rk.SumSaleCost = SumSale.ToString();
                    rk.ChinaSumApproveCost = ConvertToChinese(SumApp).ToString();
                    rk.ChinaSumSaleCost = ConvertToChinese(SumSale).ToString();
                    rk.num = ow.Count().ToString();
                int numbers = lEin.Count();
                int number = numbers / 30;
                int number1 = numbers % 30;
                if (number1 > 0)
                {
                    number = number + 1;
                }
                rk.Page = 1;
                rk.AllPage = number;
                rk.Code = await PlanInList(reqIn);
                alleout.Add(rk);
                    
            }

            // 创建工作簿
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("入库单");

            int rownums = 0;
            // 设置列宽
            sheet.SetColumnWidth(0, 15 * 256);
            sheet.SetColumnWidth(1, 10 * 256);
            sheet.SetColumnWidth(2, 6 * 256);
            sheet.SetColumnWidth(3, 6 * 256);
            sheet.SetColumnWidth(4, 7 * 256);
            sheet.SetColumnWidth(5, 7 * 256);
            sheet.SetColumnWidth(6, 7 * 256);
            sheet.SetColumnWidth(7, 7 * 256);
            sheet.SetColumnWidth(8, 12 * 256);
            sheet.SetColumnWidth(9, 15 * 256);
            sheet.SetColumnWidth(10, 7 * 256);
            sheet.SetColumnWidth(11, 7 * 256);
            // 设置打印页边距
            sheet.SetMargin(MarginType.LeftMargin, 0.1);  // 左边距，单位：英寸
            sheet.SetMargin(MarginType.RightMargin, 0.1); // 右边距，单位：英寸
            // 创建表头样式
            ICellStyle headerStyle = workbook.CreateCellStyle();
            headerStyle.Alignment = HorizontalAlignment.Center;
            headerStyle.VerticalAlignment = VerticalAlignment.Center;
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 10;
            headerStyle.SetFont(font);
            // 创建单元格样式（带自动换行）


            ICellStyle headerStyles = workbook.CreateCellStyle();
            headerStyles.Alignment = HorizontalAlignment.Center;
            headerStyles.VerticalAlignment = VerticalAlignment.Center;
            IFont fonts = workbook.CreateFont();
            fonts.FontHeightInPoints = 16;
            fonts.Boldweight = (short)FontBoldWeight.Bold;
            headerStyles.SetFont(fonts);


            // 创建单元格样式
            ICellStyle borderStyle = workbook.CreateCellStyle();
            borderStyle.BorderTop = BorderStyle.Thin; // 上边框
            borderStyle.BorderBottom = BorderStyle.Thin; // 下边框
            borderStyle.BorderLeft = BorderStyle.Thin; // 左边框
            borderStyle.BorderRight = BorderStyle.Thin; // 右边框


            ICellStyle wrapTextStyle = workbook.CreateCellStyle();
            wrapTextStyle.BorderTop = BorderStyle.Thin; // 上边框
            wrapTextStyle.BorderBottom = BorderStyle.Thin; // 下边框
            wrapTextStyle.BorderLeft = BorderStyle.Thin; // 左边框
            wrapTextStyle.BorderRight = BorderStyle.Thin; // 右边框

            wrapTextStyle.WrapText = true; // 启用自动换行
            wrapTextStyle.ShrinkToFit = true; // 启用字体缩小以适应单元格
            wrapTextStyle.VerticalAlignment = VerticalAlignment.Center;
            IFont fontss = workbook.CreateFont();
            fontss.FontHeightInPoints = 8;
            wrapTextStyle.SetFont(fontss);

            ICellStyle wrapTextStyleS = workbook.CreateCellStyle();
            wrapTextStyleS.WrapText = true; // 启用自动换行
            wrapTextStyleS.ShrinkToFit = true; // 启用字体缩小以适应单元格
            wrapTextStyleS.VerticalAlignment = VerticalAlignment.Center;
            IFont fonstss = workbook.CreateFont();
            fonstss.FontHeightInPoints = 8;
            wrapTextStyleS.SetFont(fonstss);


            ICellStyle TopborderStyle = workbook.CreateCellStyle();
            TopborderStyle.BorderTop = BorderStyle.Thin; // 下边框       
            TopborderStyle.WrapText = true; // 启用自动换行
            TopborderStyle.ShrinkToFit = true; // 启用字体缩小以适应单元格
            TopborderStyle.VerticalAlignment = VerticalAlignment.Center;
            TopborderStyle.SetFont(fonstss);
            ICellStyle EndborderStyle = workbook.CreateCellStyle();
            EndborderStyle.BorderBottom = BorderStyle.Thin; // 下边框
            EndborderStyle.WrapText = true; // 启用自动换行
            EndborderStyle.ShrinkToFit = true; // 启用字体缩小以适应单元格
            EndborderStyle.VerticalAlignment = VerticalAlignment.Center;
            EndborderStyle.SetFont(fonstss);
            ICellStyle lefttopborderStyle = workbook.CreateCellStyle();
            lefttopborderStyle.BorderTop = BorderStyle.Thin; // 上边框
            lefttopborderStyle.BorderLeft = BorderStyle.Thin; // 左边框
            lefttopborderStyle.WrapText = true; // 启用自动换行
            lefttopborderStyle.ShrinkToFit = true; // 启用字体缩小以适应单元格
            lefttopborderStyle.VerticalAlignment = VerticalAlignment.Center;
            lefttopborderStyle.SetFont(fonstss);
            ICellStyle leftbtnborderStyle = workbook.CreateCellStyle();
            leftbtnborderStyle.BorderBottom = BorderStyle.Thin; // 下边框
            leftbtnborderStyle.BorderLeft = BorderStyle.Thin; // 右边框
            leftbtnborderStyle.WrapText = true; // 启用自动换行
            leftbtnborderStyle.ShrinkToFit = true; // 启用字体缩小以适应单元格
            leftbtnborderStyle.VerticalAlignment = VerticalAlignment.Center;
            leftbtnborderStyle.SetFont(fonstss);
            ICellStyle rightbtnborderStyle = workbook.CreateCellStyle();
            rightbtnborderStyle.BorderBottom = BorderStyle.Thin; // 下边框
            rightbtnborderStyle.BorderRight = BorderStyle.Thin; // 右边框
            rightbtnborderStyle.WrapText = true; // 启用自动换行
            rightbtnborderStyle.ShrinkToFit = true; // 启用字体缩小以适应单元格
            rightbtnborderStyle.VerticalAlignment = VerticalAlignment.Center;
            rightbtnborderStyle.SetFont(fonstss);
            ICellStyle righttopborderStyle = workbook.CreateCellStyle();
            righttopborderStyle.BorderTop = BorderStyle.Thin; // 下边框
            righttopborderStyle.BorderRight = BorderStyle.Thin; // 右边框
            righttopborderStyle.WrapText = true; // 启用自动换行
            righttopborderStyle.ShrinkToFit = true; // 启用字体缩小以适应单元格
            righttopborderStyle.VerticalAlignment = VerticalAlignment.Center;
            righttopborderStyle.SetFont(fonstss);
            ICellStyle borderStyles = workbook.CreateCellStyle();
            borderStyles.BorderTop = BorderStyle.Thin; // 上边框
            borderStyles.BorderBottom = BorderStyle.Thin; // 下边框
            borderStyles.WrapText = true; // 启用自动换行
            borderStyles.ShrinkToFit = true; // 启用字体缩小以适应单元格
            borderStyles.VerticalAlignment = VerticalAlignment.Center;
            borderStyles.SetFont(fonstss);
            for (int i = 0; i < alleout.Count; i++)
            {

                // 第一行: 表头
                IRow row0 = sheet.CreateRow(rownums);
                row0.HeightInPoints = 12;
                ICell cellTitle = row0.CreateCell(0);
                cellTitle.SetCellValue("黔南州人民医院" + alleout[i].DeptCode);
                cellTitle.CellStyle = headerStyle;
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 0, 10));

                rownums++;
                IRow row00 = sheet.CreateRow(rownums);
                row00.HeightInPoints = 20;
                ICell cellTitles = row00.CreateCell(0);
                cellTitles.SetCellValue("入     库     验     收     单");
                cellTitles.CellStyle = headerStyles;
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 0, 10));
                rownums++;


                IRow row1 = sheet.CreateRow(rownums);
                ICell cerow1 = row1.CreateCell(0);
                cerow1.SetCellValue($"第{alleout[i].Page}页/共{alleout[i].AllPage}页");
                cerow1.CellStyle = wrapTextStyleS;
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 0, 2));

                ICell cerow18 = row1.CreateCell(8);
                cerow18.SetCellValue($"入库类型:{alleout[i].InType}");
                cerow18.CellStyle = wrapTextStyleS;
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 8, 10));


                rownums++;

                // 第三行: 领取部门等信息
                IRow row2 = sheet.CreateRow(rownums);
                ICell cerow20 = row2.CreateCell(0);
                cerow20.SetCellValue($"单号:{alleout[i].Code}");
                cerow20.CellStyle = wrapTextStyleS;
               
                ICell cerow28 = row2.CreateCell(8);
                cerow28.SetCellValue($"备注:{alleout[i].Mark}");
                cerow28.CellStyle = wrapTextStyleS;
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 0, 5));
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 8, 10));

                rownums++;

                IRow row3 = sheet.CreateRow(rownums);
                ICell cerow30 = row3.CreateCell(0);
                cerow30.SetCellValue($"供货单位:{alleout[i].SupplierName}");
                cerow30.CellStyle = wrapTextStyleS;
                ICell cerow34 = row3.CreateCell(4);
                cerow34.SetCellValue($"入库时间:{alleout[i].GTime}");
                cerow34.CellStyle = wrapTextStyleS;
                ICell cerow38 = row3.CreateCell(8);
                cerow38.SetCellValue($"发票号:{alleout[i].BillCode}");
                cerow38.CellStyle = wrapTextStyleS;

                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 0, 3));

                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 4, 7));
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 8, 10));

                //sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 3));
                //sheet.AddMergedRegion(new CellRangeAddress(2, 2, 9, 13));
                rownums++;

                // 第四行: 药品表头
                IRow row4 = sheet.CreateRow(rownums);
                row4.HeightInPoints = 18;
                string[] headers = { "药品名称", "规格", "单位", "数量", "批价", "批价金额", "零价", "零售金额", "生产厂家", "批号", "有效期"};
                for (int f = 0; f < headers.Length; f++)
                {
                    row4.CreateCell(f).SetCellValue(headers[f]);
                    ICell cell = row4.CreateCell(f);
                    cell.SetCellValue(headers[f]);
                    cell.CellStyle = wrapTextStyle;
                }
                rownums++;
                int p = alleout[i].Drugs.Count + rownums;
                int ass = rownums;
                //int rownums = 1; // 数据起始行号
                int currentPageDrugCount = 0;

                // 第五行及以下: 药品数据
                for (int f = ass; f < p; f++)
                {
                    if (alleout[i].Page < alleout[i].AllPage && currentPageDrugCount == 30)
                    {
                        // 合计行
                        IRow prow8 = sheet.CreateRow(rownums);
                        prow8.HeightInPoints = 25;
                        ICell pcerow80 = prow8.CreateCell(0);
                        pcerow80.SetCellValue($"当前页{30}笔/共{alleout[i].Drugs.Count}笔");
                        pcerow80.CellStyle = wrapTextStyle;
                        ICell pcerow81 = prow8.CreateCell(1);
                        pcerow81.SetCellValue($"批价总金额:{alleout[i].SumApproveCost}");
                        pcerow81.CellStyle = wrapTextStyle;
                        ICell pcerow86 = prow8.CreateCell(6);
                        pcerow86.SetCellValue($"零售总金额:{alleout[i].SumSaleCost}");
                        pcerow86.CellStyle = wrapTextStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 1, 5));
                        sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 6, 10));

                        rownums++;
                        ass++;
                        f++;
                        p++;

                        // 大写金额行
                        IRow prow9 = sheet.CreateRow(rownums);
                        prow9.HeightInPoints = 25;

                        ICell pcerow90 = prow9.CreateCell(1);
                        pcerow90.SetCellValue($"大写批价金额:{alleout[i].ChinaSumSaleCost}");
                        pcerow90.CellStyle = wrapTextStyle;
                        ICell pcerow91 = prow9.CreateCell(6);
                        pcerow91.SetCellValue($"大写零售金额:{alleout[i].ChinaSumApproveCost}");
                        pcerow91.CellStyle = wrapTextStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 1, 5));
                        sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 6, 10));

                        rownums++;
                        ass++;
                        f++;
                        p++;

                        // 底部签字行
                        IRow prow10 = sheet.CreateRow(rownums);
                        prow10.HeightInPoints = 30;

                        ICell pcerow100 = prow10.CreateCell(0);
                        pcerow100.SetCellValue($"发药人");
                        pcerow100.CellStyle = wrapTextStyle;
                        ICell pcerow102 = prow10.CreateCell(2);
                        pcerow102.SetCellValue($"复核");
                        pcerow102.CellStyle = wrapTextStyle;
                        ICell pcerow103 = prow10.CreateCell(5);
                        pcerow103.SetCellValue($"主任");
                        pcerow103.CellStyle = wrapTextStyle;
                        ICell pcerow104 = prow10.CreateCell(7);
                        pcerow104.SetCellValue($"经手人");
                        pcerow104.CellStyle = wrapTextStyle;
                        ICell pcerow105 = prow10.CreateCell(9);
                        pcerow105.SetCellValue($"打印时间  {alleout[i].NowgetTime}");
                        pcerow105.CellStyle = wrapTextStyle;

                        sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 9, 10));

                        for (int col = 0; col < 11; col++)
                        {
                            // 获取或创建第8行、第9行和第10行的单元格
                            ICell pcell8 = prow8.GetCell(col) ?? prow8.CreateCell(col);
                            ICell pcell9 = prow9.GetCell(col) ?? prow9.CreateCell(col);
                            ICell pcell10 = prow10.GetCell(col) ?? prow10.CreateCell(col);

                            if (col == 0)
                            {
                                pcell8.CellStyle = lefttopborderStyle;
                                pcell9.CellStyle = leftbtnborderStyle;
                                ICellStyle bordersStyles = workbook.CreateCellStyle();
                                bordersStyles.BorderTop = BorderStyle.Thin; // 上边框
                                bordersStyles.BorderBottom = BorderStyle.Thin; // 下边框
                                bordersStyles.BorderLeft = BorderStyle.Thin; // 下边框
                                bordersStyles.WrapText = false; // 启用自动换行
                                bordersStyles.ShrinkToFit = true; // 启用字体缩小以适应单元格
                                bordersStyles.VerticalAlignment = VerticalAlignment.Center;
                                bordersStyles.SetFont(fonstss);
                                pcell10.CellStyle = bordersStyles;
                            }
                            else if (col == 10)
                            {
                                pcell8.CellStyle = righttopborderStyle;
                                pcell9.CellStyle = rightbtnborderStyle;
                                ICellStyle bordersStyles = workbook.CreateCellStyle();
                                bordersStyles.BorderTop = BorderStyle.Thin; // 上边框
                                bordersStyles.BorderBottom = BorderStyle.Thin; // 下边框
                                bordersStyles.BorderRight = BorderStyle.Thin; // 下边框
                                bordersStyles.BorderTop = BorderStyle.Thin; // 上边框
                                bordersStyles.BorderBottom = BorderStyle.Thin; // 下边框
                                bordersStyles.BorderLeft = BorderStyle.Thin; // 下边框
                                bordersStyles.WrapText = false; // 启用自动换行
                                bordersStyles.ShrinkToFit = true; // 启用字体缩小以适应单元格
                                bordersStyles.VerticalAlignment = VerticalAlignment.Center;
                                bordersStyles.SetFont(fonstss);
                                pcell10.CellStyle = bordersStyles;
                            }
                            else
                            {
                                pcell8.CellStyle = TopborderStyle; // 上边框
                                pcell9.CellStyle = EndborderStyle; // 下边框
                                pcell10.CellStyle = borderStyles; // 下边框
                            }
                        }

                        rownums++;
                        ass++;
                        f++;
                        p++;

                        currentPageDrugCount = 0;
                        alleout[i].Page += 1;

                        IRow qrow2 = sheet.CreateRow(rownums);
                        rownums++;
                        ass++;
                        p++;
                        f++;
                        //插入分页分页
                        sheet.SetRowBreak(rownums);
                        rownums++;
                        p++;
                        ass++;
                        f++;
                        // 插入2行空白行（可以根据需要设置行高或样式，这里直接创建空行）
                        IRow qrow1 = sheet.CreateRow(rownums);
                        ICell qcerow1 = qrow1.CreateCell(0);
                        qcerow1.SetCellValue($"第{alleout[i].Page}页/共{alleout[i].AllPage}页");
                        qcerow1.CellStyle = wrapTextStyleS;
                        sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 0, 2));
                        rownums++;
                        p++;
                        ass++;
                        f++;
                    }

                    currentPageDrugCount++;


                    IRow row = sheet.CreateRow(f);

                    row.HeightInPoints = 28;
                    var dr = alleout[i].Drugs[rownums - ass];
                    // 创建并设置单元格值，同时应用边框样式
                    //ICell cell0 = row.CreateCell(0);
                    //cell0.SetCellValue($"{dr.TradeName}");
                    //cell0.CellStyle = wrapTextStyle;
                    ICellStyle TradeNamecell = CloneCellStyle(workbook, wrapTextStyle);
                    SetCellValueAutoFit(row, 0, dr.TradeName, TradeNamecell, fontss, sheet, 0, 28);


                    //ICell cell1 = row.CreateCell(1);
                    //cell1.SetCellValue($"{dr.Specs}");
                    //cell1.CellStyle = wrapTextStyle;
                    ICellStyle Specscell = CloneCellStyle(workbook, wrapTextStyle);
                    SetCellValueAutoFit(row, 1, dr.Specs, Specscell, fontss, sheet, 1, 28);


                    ICell cell2 = row.CreateCell(2);
                    cell2.SetCellValue($"{dr.PackUnit}");
                    cell2.CellStyle = wrapTextStyle;

                    ICell cell3 = row.CreateCell(3);
                    cell3.SetCellValue($"{dr.PackQty}");
                    cell3.CellStyle = wrapTextStyle;

                    ICell cell4 = row.CreateCell(4);
                    cell4.SetCellValue($"{dr.PurchasePrice}");
                    cell4.CellStyle = wrapTextStyle;

                    ICell cell5 = row.CreateCell(5);
                    cell5.SetCellValue($"{dr.ApproveCost}");
                    cell5.CellStyle = wrapTextStyle;

                    ICell cell6 = row.CreateCell(6);
                    cell6.SetCellValue($"{dr.RetailPrice}");
                    cell6.CellStyle = wrapTextStyle;

                    ICell cell7 = row.CreateCell(7);
                    cell7.SetCellValue($"{dr.SaleCost}");
                    cell7.CellStyle = wrapTextStyle;


                    // 设置规格列
                    //ICell specCell = row.CreateCell(8);
                    //specCell.SetCellValue($"{dr.ProducerCode}");
                    //specCell.CellStyle = wrapTextStyle; // 应用自动换行样式
                    ICellStyle ProducerCodecell = CloneCellStyle(workbook, wrapTextStyle);
                    SetCellValueAutoFit(row, 8, dr.ProducerCode, ProducerCodecell, fontss, sheet, 8, 28);

                    //specCell.CellStyle = borderStyle; // 也可以应用边框样式

                    ICell cell9 = row.CreateCell(9);
                    cell9.SetCellValue($"{dr.BatchNo}");
                    cell9.CellStyle = wrapTextStyle;

                    //ICell cell10 = row.CreateCell(10);
                    //cell10.SetCellValue($"{dr.ValidDate}");
                    //cell10.CellStyle = wrapTextStyle;
                    ICellStyle ValidDatecell = CloneCellStyle(workbook, wrapTextStyle);
                    SetCellValueAutoFit(row, 10, dr.ValidDate, ValidDatecell, fontss, sheet, 10, 28);
                    rownums++;
                }

                // 合计行
                IRow row8 = sheet.CreateRow(rownums);
                row8.HeightInPoints = 25;
                ICell cerow80 = row8.CreateCell(0);
                cerow80.SetCellValue($"当前张{alleout[i].Drugs.Count}笔/共{alleout[i].Drugs.Count}笔");
                cerow80.CellStyle = wrapTextStyle;
                ICell cerow81 = row8.CreateCell(1);
                cerow81.SetCellValue($"批价总金额:{alleout[i].SumApproveCost}");
                cerow81.CellStyle = wrapTextStyle;
                ICell cerow86 = row8.CreateCell(6);
                cerow86.SetCellValue($"零售总金额:{alleout[i].SumSaleCost}");
                cerow86.CellStyle = wrapTextStyle;
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 1, 5));
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 6, 10));

                rownums++;

                // 大写金额行
                IRow row9 = sheet.CreateRow(rownums);
                row9.HeightInPoints = 25;

                ICell cerow90 = row9.CreateCell(1);
                cerow90.SetCellValue($"大写批价金额:{alleout[i].ChinaSumSaleCost}");
                cerow90.CellStyle = wrapTextStyle;
                ICell cerow91 = row9.CreateCell(6);
                cerow91.SetCellValue($"大写零售金额:{alleout[i].ChinaSumApproveCost}");
                cerow91.CellStyle = wrapTextStyle;
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 1, 5));
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 6, 10));




                rownums++;
                // 底部签字行
                IRow row10 = sheet.CreateRow(rownums);
                row10.HeightInPoints =30;

                ICell cerow100 = row10.CreateCell(0);      
                cerow100.SetCellValue($"负责人：                          采购人：                       验收人：                            审核人：                       入库人：{username}                       {alleout[i].NowgetTime}");
                cerow100.CellStyle = wrapTextStyle;
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 0, 10));


                for (int col = 0; col < 11; col++)
                {
                    // 获取或创建第8行、第9行和第10行的单元格
                    ICell cell8 = row8.GetCell(col) ?? row8.CreateCell(col);
                    ICell cell9 = row9.GetCell(col) ?? row9.CreateCell(col);
                    ICell cell10 = row10.GetCell(col) ?? row10.CreateCell(col);

                    if (col == 0)
                    {
                        cell8.CellStyle = lefttopborderStyle;
                        cell9.CellStyle = leftbtnborderStyle;
                        ICellStyle bordersStyles = workbook.CreateCellStyle();
                        bordersStyles.BorderTop = BorderStyle.Thin; // 上边框
                        bordersStyles.BorderBottom = BorderStyle.Thin; // 下边框
                        bordersStyles.BorderLeft = BorderStyle.Thin; // 下边框
                        bordersStyles.WrapText = true; // 启用自动换行
                        bordersStyles.ShrinkToFit = true; // 启用字体缩小以适应单元格
                        bordersStyles.VerticalAlignment = VerticalAlignment.Center;
                        bordersStyles.SetFont(fonstss);
                        cell10.CellStyle = bordersStyles;
                    }
                    else if (col == 10)
                    {
                        cell8.CellStyle = righttopborderStyle;
                        cell9.CellStyle = rightbtnborderStyle;
                        ICellStyle bordersStyles = workbook.CreateCellStyle();
                        bordersStyles.BorderTop = BorderStyle.Thin; // 上边框
                        bordersStyles.BorderBottom = BorderStyle.Thin; // 下边框
                        bordersStyles.BorderRight = BorderStyle.Thin; // 下边框
                        bordersStyles.BorderTop = BorderStyle.Thin; // 上边框
                        bordersStyles.BorderBottom = BorderStyle.Thin; // 下边框
                        bordersStyles.BorderLeft = BorderStyle.Thin; // 下边框
                        bordersStyles.WrapText = true; // 启用自动换行
                        bordersStyles.ShrinkToFit = true; // 启用字体缩小以适应单元格
                        bordersStyles.VerticalAlignment = VerticalAlignment.Center;
                        bordersStyles.SetFont(fonstss);
                        cell10.CellStyle = bordersStyles;
                    }
                    else
                    {
                        cell8.CellStyle = TopborderStyle; // 上边框
                        cell9.CellStyle = EndborderStyle; // 下边框
                        cell10.CellStyle = borderStyles; // 下边框
                    }
                }



                //从当前行开始 无记录 每次执行到时判断  当大于时 20.5时 就开始 rowsnum++;


                //sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 0, 2));

                rownums++;
                rownums++;
                rownums++;
                //var footer = sheet.Header;
                //footer.Left = "当前第 &P 页，共 &N 页";

                ////footer.Center = "";
                //sheet.SetRowBreak(rownums++);

            }


            IWebHostEnvironment webHostEnvironment = (IWebHostEnvironment)App.ServiceProvider.GetService(typeof(IWebHostEnvironment));
            string sFileName = $"入库单{DateTime.Now:MM-dd-HHmmss}.xlsx";
            string fullPath = Path.Combine(webHostEnvironment.WebRootPath, sFileName);

            //MiniExcel.SaveAsByTemplateAsync(list, sheetName: sheetName);
            // 保存到文件
            using (FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);

            }

            workbook.Close();

            return ExportExcel(fullPath, sFileName);
        }




        public async Task<string> PlanInList(reqIn reqIn)
        {
                List<resPutList> result = await SendRequestsAsync(reqIn); // 使用 await 等待异步操作完成
            string nf = "";
            if (result!=null)
            {
                nf = result[0].inListCode;
            }
               return nf;
           
        }

        private async Task<List<resPutList>> SendRequestsAsync(reqIn requestData)
        {
            using (var client = new HttpClient())
            {
                // 构建 URL，包括查询参数
                string url = $"http://192.168.1.95:7800/His/GetPhaInPutList";
                // 将对象序列化为 JSON
                // 构建查询参数
                var queryParams = new Dictionary<string, string>();
                //queryParams.Add("beginTime", requestData.beginTime.ToString("yyyy-MM-dd HH:mm:ss")); // 假设服务器期望的日期格式
                //queryParams.Add("endTime", requestData.endTime.ToString("yyyy-MM-dd HH:mm:ss"));
                queryParams.Add("drugDeptCode", requestData.drugDeptCode);
                queryParams.Add("drugCode", requestData.drugCode);
                queryParams.Add("companyCode", requestData.companyCode);
                queryParams.Add("planNo", requestData.planNo);
                queryParams.Add("billCode", requestData.billCode);
                queryParams.Add("spdInputId", requestData.spdInputId);

                
                // 将查询参数添加到 URL
                var builder = new UriBuilder(url);
                var query = HttpUtility.ParseQueryString(string.Empty);
                foreach (var param in queryParams)
                {
                    query[param.Key] = param.Value;
                }
                builder.Query = query.ToString();
                url = builder.ToString();
                HttpResponseMessage response = await client.GetAsync(url);
                //// 将 reqIn 对象序列化为 JSON
                //var json = JsonConvert.SerializeObject(requestData);
                //var content = new StringContent(json, Encoding.UTF8, "application/json");
                //// 发送 POST 请求
                //HttpResponseMessage response = await client.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent); // 输出响应内容
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<resPutList>>(responseContent);
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode}, Message: {response.ReasonPhrase}, Response: {responseContent}");
                }
            }
        }
        void SetCellValueAutoFit(
      IRow row,
      int cellIndex,
      string text,
      ICellStyle style,
      IFont baseFont, // 这里传入原始字体
      ISheet sheet,
      int columnIndex,
      int fixedRowHeightInPoints)
        {
            IWorkbook workbook = sheet.Workbook;
            ICell cell = row.CreateCell(cellIndex);
            cell.SetCellValue(text);

            // **关键：创建一个独立的字体，避免影响外部样式**
            IFont font = workbook.CreateFont();
            font.FontName = baseFont.FontName;
            font.Boldweight = baseFont.Boldweight;
            font.Color = baseFont.Color;
            font.IsItalic = baseFont.IsItalic;
            font.Underline = baseFont.Underline;

            int maxFontSize = 10;
            int minFontSize = 2;
            font.FontHeightInPoints = (short)maxFontSize;

            // **关键：克隆 style，防止修改外部 wrapTextStyle**
            ICellStyle newStyle = workbook.CreateCellStyle();
            newStyle.CloneStyleFrom(style);
            newStyle.SetFont(font);

            // 计算列宽所能容纳的字符数
            int colWidthIn256 = (int)sheet.GetColumnWidth(columnIndex);
            int colWidthInChars = colWidthIn256 / 256;
            decimal estimatedCharWidth = 1.5M; // 估算：一个字符大约占 2 个单位宽度
            int maxCharsPerLine = (int)(colWidthInChars / estimatedCharWidth);

            // 自动换行
            List<string> WrapText(string str, int maxLen)
            {
                List<string> lines = new List<string>();
                for (int i = 0; i < str.Length; i += maxLen)
                {
                    int len = Math.Min(maxLen, str.Length - i);
                    lines.Add(str.Substring(i, len));
                }
                return lines;
            }

            int currentFontSize = maxFontSize;
            while (currentFontSize >= minFontSize)
            {
                int estimatedLineHeight = currentFontSize + 2;
                List<string> wrappedLines = WrapText(text, maxCharsPerLine);
                int requiredHeight = wrappedLines.Count * estimatedLineHeight;

                if (requiredHeight <= fixedRowHeightInPoints)
                {
                    break;
                }
                else
                {
                    currentFontSize--;
                    font.FontHeightInPoints = (short)currentFontSize;
                    newStyle.SetFont(font);
                }
            }

            // 设置固定行高
            row.HeightInPoints = fixedRowHeightInPoints;
            cell.CellStyle = newStyle;
        }


        public ICellStyle CloneCellStyle(IWorkbook workbook, ICellStyle sourceStyle)
        {
            // 创建新的样式
            ICellStyle newStyle = workbook.CreateCellStyle();

            // 复制常见的样式属性
            newStyle.Alignment = sourceStyle.Alignment;
            newStyle.BorderBottom = sourceStyle.BorderBottom;
            newStyle.BorderLeft = sourceStyle.BorderLeft;
            newStyle.BorderRight = sourceStyle.BorderRight;
            newStyle.BorderTop = sourceStyle.BorderTop;
            newStyle.BottomBorderColor = sourceStyle.BottomBorderColor;
            newStyle.DataFormat = sourceStyle.DataFormat;
            newStyle.FillBackgroundColor = sourceStyle.FillBackgroundColor;
            newStyle.FillForegroundColor = sourceStyle.FillForegroundColor;
            newStyle.FillPattern = sourceStyle.FillPattern;
            newStyle.Indention = sourceStyle.Indention;
            newStyle.IsLocked = sourceStyle.IsLocked;
            newStyle.LeftBorderColor = sourceStyle.LeftBorderColor;
            newStyle.RightBorderColor = sourceStyle.RightBorderColor;
            newStyle.Rotation = sourceStyle.Rotation;
            newStyle.ShrinkToFit = sourceStyle.ShrinkToFit;
            newStyle.VerticalAlignment = sourceStyle.VerticalAlignment;
            newStyle.WrapText = sourceStyle.WrapText;

            // 确保创建一个独立的字体对象
            IFont sourceFont = workbook.GetFontAt(sourceStyle.FontIndex);
            IFont newFont = workbook.CreateFont();

            newFont.FontName = sourceFont.FontName;
            newFont.FontHeight = sourceFont.FontHeight;
            newFont.FontHeightInPoints = sourceFont.FontHeightInPoints;
            newFont.Boldweight = sourceFont.Boldweight;
            newFont.Color = sourceFont.Color;
            newFont.IsItalic = sourceFont.IsItalic;
            newFont.Underline = sourceFont.Underline;

            // 重要：确保新字体是独立的，防止共享
            newStyle.SetFont(newFont);
            return newStyle;
        }
    }
}