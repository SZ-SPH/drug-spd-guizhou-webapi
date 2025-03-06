using Microsoft.AspNetCore.Mvc;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Service.Business.IBusinessService;
using ZR.Admin.WebApi.Filters;
using MiniExcelLibs;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using NPOI.HPSF;
using NPOI.SS.Util;
using Microsoft.Data.SqlClient.DataClassification;
using NPOI.SS.Formula.Functions;
using ZR.Model.GuiHis;
using NPOI.HSSF.Model;
using NPOI.OpenXmlFormats.Dml.Diagram;
using ZR.Common;
using Oracle.ManagedDataAccess.Types;
using System.Numerics;
//创建时间：2024-12-11
namespace ZR.Admin.WebApi.Controllers.Business
{
    /// <summary>
    /// 出库单
    /// </summary>
    [Verify]
    [Route("business/OutOrder")]
    public class OutOrderController : BaseController
    {
        /// <summary>
        /// 出库单接口
        /// </summary>
        private readonly IOutOrderService _OutOrderService;
        private readonly IOuWarehousetService _OuWarehousetService;
        private readonly IOutDrugsService _OutDrugsService;

        private readonly ISysUserService _sysUserService;
        public OutOrderController(IOutOrderService OutOrderService, IOuWarehousetService ouWarehousetService, ISysUserService sysUserService, IOutDrugsService outDrugsService)
        {
            _OutOrderService = OutOrderService;
            _OuWarehousetService = ouWarehousetService;
            _sysUserService = sysUserService;
            _OutDrugsService = outDrugsService;
        }



        /// <summary>
        /// 查询出库单列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "outorder:list")]
        public IActionResult QueryOutOrder([FromQuery] OutOrderQueryDto parm)
        {
            var response = _OutOrderService.GetList(parm);
            return SUCCESS(response);
        }


        /// <summary>
        /// 查询出库单详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "outorder:query")]
        public IActionResult GetOutOrder(int Id)
        {
            var response = _OutOrderService.GetInfo(Id);

            var info = response.Adapt<OutOrderDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加出库单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "outorder:add")]
        [Log(Title = "出库单", BusinessType = BusinessType.INSERT)]
        public IActionResult AddOutOrder([FromBody] OutOrderDto parm)
        {
            var modal = parm.Adapt<OutOrder>().ToCreate(HttpContext);

            var response = _OutOrderService.AddOutOrder(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新出库单
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "outorder:edit")]
        [Log(Title = "出库单", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateOutOrder([FromBody] OutOrderDto parm)
        {
            var modal = parm.Adapt<OutOrder>().ToUpdate(HttpContext);
            var response = _OutOrderService.UpdateOutOrder(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除出库单
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{ids}")]
        [ActionPermissionFilter(Permission = "outorder:delete")]
        [Log(Title = "出库单", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteOutOrder([FromRoute] string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);

            return ToResponse(_OutOrderService.Delete(idArr));
        }

        /// <summary>
        /// 导出出库单
        /// </summary>
        /// <returns></returns>
        [Log(Title = "出库单", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "outorder:export")]
        public IActionResult Export([FromQuery] OutOrderQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _OutOrderService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "出库单", "出库单");
            string demo = "模板路径";
            string files = "文件路径";

            MiniExcel.SaveAsByTemplateAsync(demo, files, parm);

            return ExportExcel(result.Item2, result.Item1);
        }


        static string ConvertToChinese(decimal? amount)
        {
            if (amount < 0)
                return "负" + ConvertToChinese(-amount);

            string[] units = { "", "拾", "佰", "仟", "万", "亿" };
            string[] digits = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };

            long integralPart = (long)amount; // 整数部分
            int fractionalPart = (int)((amount - integralPart) * 100); // 小数部分（分）

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
                else if (!zero && result.Length > 0) // 只在需要时添加零
                {
                    result = digits[0] + result;
                    zero = true;
                }

                integralPart /= 10;
                unitPos++;
            }

            return result.TrimStart('零'); // 去掉开头的零
        }

        static string ConvertFractionalToChinese(int fractionalPart, string[] digits)
        {
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
        /// 清空出库单
        /// </summary>
        /// <returns></returns>
        [Log(Title = "出库单", BusinessType = BusinessType.CLEAN)]
        [ActionPermissionFilter(Permission = "outorder:delete")]
        [HttpDelete("clean")]
        public IActionResult Clear()
        {
            if (!HttpContextExtension.IsAdmin(HttpContext))
            {
                return ToResponse(ResultCode.FAIL, "操作失败");
            }
            return SUCCESS(_OutOrderService.TruncateOutOrder());
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "出库单导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false)]
        [ActionPermissionFilter(Permission = "outorder:import")]
        public IActionResult ImportData([FromForm(Name = "file")] IFormFile formFile)
        {
            List<OutOrderDto> list = new();
            using (var stream = formFile.OpenReadStream())
            {
                list = stream.Query<OutOrderDto>(startCell: "A1").ToList();
            }

            return SUCCESS(_OutOrderService.ImportOutOrder(list.Adapt<List<OutOrder>>()));
        }

        /// <summary>
        /// 出库单导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "出库单模板", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            var result = DownloadImportTemplate(new List<OutOrderDto>() { }, "OutOrder");
            return ExportExcel(result.Item2, result.Item1);
        }

        [Log(Title = "出库单", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("Exports")]
        [ActionPermissionFilter(Permission = "outorder:export")]
        public IActionResult Exports([FromQuery] List<int> parm)
        {
            List<EOut> alleout = new();

            List<EIn> lEin = new();
            foreach (var item in parm)
            {
                lEin = new();
                EOut eOut = new EOut();
                eOut.Demo = "黔南州人民医院";

                var list = _OutOrderService.GetInfos(item);
                eOut.DrugDeptCode = list.OutWarehouseName;
                eOut.DrugStorageCode = list.InpharmacyName;
                eOut.GetPerson = list.UseReceive;
                //Controller获取用户名
                var userName = HttpContext.GetUId();
                eOut.GetPerson = _sysUserService.SelectUserById(userName).NickName;

                eOut.Mark = list.Remarks;

                eOut.GTime = DateTime.Now.ToString("yyyy-MM-dd");
                decimal SumApp = 0;
                decimal SumSale = 0;
                var ow = _OuWarehousetService.EList(item);
                foreach (var t in ow)
                {
                    EIn eIn = new();
                    eIn.TradeName = t.TradeName;
                    eIn.Specs = t.Specs;
                    eIn.PackUnit = t.PackUnit;
                    eIn.PackQty = t.PackQty.ToString();
                    eIn.PurchasePrice = t.PurchasePrice.ToString("G0");
                    eIn.ApproveCost = t.ApproveCost.ToString("G0");
                    eIn.RetailPrice = t.RetailPrice.ToString("G0");
                    eIn.SaleCost = t.SaleCost.ToString("G0");
                    eIn.ProducerCode = t.ProducerCode.ToString();
                    eIn.ValidDate =t.ValidDate?.ToString("yyyy-MM-dd");
                    eIn.BatchNo = t.BatchNo;
                    eOut.OutType = t.OutType;
                    SumApp += t.ApproveCost;
                    SumSale += t.SaleCost;
                    lEin.Add(eIn);
                }
                eOut.Drugs = lEin;
                eOut.SumApproveCost = SumApp.ToString("G0");
                eOut.SumSaleCost = SumSale.ToString("G0");
                eOut.ChinaSumApproveCost = ConvertToChinese(SumApp).ToString();
                eOut.ChinaSumSaleCost = ConvertToChinese(SumSale).ToString();
                eOut.num = ow.Count().ToString();
                eOut.NowgetTime = DateTime.Now.ToString("yyyy-MM-dd");
                alleout.Add(eOut);
            }
            //string demo = "D:\\出库导出模板.xlsx";
            //string files = "D:\\出库单.xlsx";
            //MiniExcel.SaveAsByTemplateAsync(demo, files, eOut);

            var result = ExCod(alleout[0], "出库导出模板.xlsx", "出库单");
            //return ExportWithTemplate(alleout);
            return ExportExcel(result.Item2, result.Item1);
        }


        /// <summary>
        /// 出库导出
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>

        [HttpGet("GenerateExcel")]
        public IActionResult GenerateExcel([FromQuery] List<int> parm)
        {
            List<EOut> alleout = new();
            List<EIn> lEin = new();
            foreach (var item in parm)
            {
                lEin = new();
                EOut eOut = new EOut();
                eOut.Demo = "黔南州人民医院";
                var list = _OutOrderService.GetInfos(item);
                eOut.OutType = _OutOrderService.Byout(list.Type);
                //if (list.Type == "4") eOut.iscong = "（冲单）";

                eOut.DrugDeptCode = list.OutWarehouseName;
                eOut.DrugStorageCode = list.InpharmacyName;
                eOut.GetPerson = list.UseReceive;
                eOut.Billcode = list.OutOrderCode;
                //Controller获取用户名
                var userName = HttpContext.GetUId();
                eOut.GetPerson = list.UseReceive;
                eOut.Mark = list.Remarks;
                eOut.GTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                decimal SumApp = 0;
                decimal SumSale = 0;
                var ow = _OuWarehousetService.EList(item);
                foreach (var t in ow)
                {
                    if (!string.IsNullOrEmpty(t.OutListCode)) { eOut.Billcode = t.OutListCode; }
                    eOut.GTime = t.OutDate.ToString();
                    EIn eIn = new();
                    eIn.TradeName = t.TradeName;
                    eIn.Specs = t.Specs;
                    eIn.PackUnit = t.PackUnit;
                    eIn.PackQty = (t.OutNum / t.PackQty).ToString();
                    eIn.PurchasePrice = t.PurchasePrice.ToString("G0");
                    eIn.ApproveCost = t.ApproveCost.ToString("G0");
                    eIn.RetailPrice = t.RetailPrice.ToString("G0");
                    eIn.SaleCost = t.SaleCost.ToString("G0");
                    eIn.ProducerCode = t.ProducerCode?.ToString();
                    eIn.ValidDate = t.ValidDate?.ToString("yyyy-MM-dd");
                    eIn.BatchNo = t.BatchNo;
                    eIn.LoctionName = t.PlaceCode;
                    //eOut.OutType = t.OutType;
                    SumApp += t.ApproveCost;
                    SumSale += t.SaleCost;
                    lEin.Add(eIn);
                }
                eOut.Drugs = lEin;
                eOut.SumApproveCost = SumApp.ToString("G0");
                eOut.SumSaleCost = SumSale.ToString("G0");
                eOut.ChinaSumApproveCost = ConvertToChinese(SumApp).ToString();
                eOut.ChinaSumSaleCost = ConvertToChinese(SumSale).ToString();
                eOut.num = ow.Count().ToString();
                eOut.NowgetTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
              
                int numbers = lEin.Count();   
                int number = numbers / 30;
                int number1 = numbers % 30;
                if (number1 > 0)
                {
                    number = number + 1;
                }
                eOut.Page = 1;
                eOut.AllPage = number;

                alleout.Add(eOut);
            }
         
            int page = 0;            
            int allpage = 0;
            int counts = 0;
            int allcount = 0;         

            // 创建工作簿
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("出库单");           
            // 设置列宽
            // 设置列宽
            sheet.SetColumnWidth(0, 11 * 256);
            sheet.SetColumnWidth(1, 10 * 256);
            sheet.SetColumnWidth(2, 6 * 256);
            sheet.SetColumnWidth(3, 6 * 256);
            sheet.SetColumnWidth(4, 7 * 256);
            sheet.SetColumnWidth(5, 7 * 256);
            sheet.SetColumnWidth(6, 7 * 256);
            sheet.SetColumnWidth(7, 7 * 256);
            sheet.SetColumnWidth(8, 10 * 256);
            sheet.SetColumnWidth(9, 10 * 256);
            sheet.SetColumnWidth(10, 7 * 256);
            sheet.SetColumnWidth(11, 6 * 256);
            sheet.SetColumnWidth(12, 5 * 256);

            sheet.SetMargin(MarginType.LeftMargin, 0.1);  // 左边距，单位：英寸
            sheet.SetMargin(MarginType.RightMargin, 0.1); // 右边距，单位：英寸

            // 创建表头样式
            ICellStyle headerStyle = workbook.CreateCellStyle();
            headerStyle.Alignment = HorizontalAlignment.Center;
            headerStyle.VerticalAlignment = VerticalAlignment.Center;
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 18;
            font.Boldweight = (short)FontBoldWeight.Bold;
            headerStyle.SetFont(font);
            // 创建单元格样式（带自动换行）

            ICellStyle headerStyles = workbook.CreateCellStyle();
            headerStyles.Alignment = HorizontalAlignment.Center;
            headerStyles.VerticalAlignment = VerticalAlignment.Center;
            IFont fonts = workbook.CreateFont();
            fonts.FontHeightInPoints = 20;
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

            wrapTextStyle.VerticalAlignment = VerticalAlignment.Center;
            IFont fontss = workbook.CreateFont();
            fontss.FontHeightInPoints = 8;
            wrapTextStyle.SetFont(fontss);

            ICellStyle wrapTextStyleS = workbook.CreateCellStyle();
            wrapTextStyleS.WrapText = true; // 启用自动换行
            wrapTextStyleS.ShrinkToFit = false; // 启用字体缩小以适应单元格
            wrapTextStyleS.VerticalAlignment = VerticalAlignment.Center;
            IFont fonstss = workbook.CreateFont();
            fonstss.FontHeightInPoints = 8;
            wrapTextStyleS.SetFont(fonstss);

            ICellStyle TopborderStyle = workbook.CreateCellStyle();
            TopborderStyle.BorderTop = BorderStyle.Thin; // 下边框       
            TopborderStyle.WrapText = true; // 启用自动换行
            TopborderStyle.ShrinkToFit = false; // 启用字体缩小以适应单元格
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
            lefttopborderStyle.ShrinkToFit = false; // 启用字体缩小以适应单元格
            lefttopborderStyle.VerticalAlignment = VerticalAlignment.Center;
            lefttopborderStyle.SetFont(fonstss);

            ICellStyle leftbtnborderStyle = workbook.CreateCellStyle();
            leftbtnborderStyle.BorderBottom = BorderStyle.Thin; // 下边框
            leftbtnborderStyle.BorderLeft = BorderStyle.Thin; // 右边框
            leftbtnborderStyle.WrapText = true; // 启用自动换行
            leftbtnborderStyle.ShrinkToFit = false; // 启用字体缩小以适应单元格
            leftbtnborderStyle.VerticalAlignment = VerticalAlignment.Center;
            leftbtnborderStyle.SetFont(fonstss);
            ICellStyle rightbtnborderStyle = workbook.CreateCellStyle();
            rightbtnborderStyle.BorderBottom = BorderStyle.Thin; // 下边框
            rightbtnborderStyle.BorderRight = BorderStyle.Thin; // 右边框
            rightbtnborderStyle.WrapText = true; // 启用自动换行
            rightbtnborderStyle.ShrinkToFit = false; // 启用字体缩小以适应单元格
            rightbtnborderStyle.VerticalAlignment = VerticalAlignment.Center;
            rightbtnborderStyle.SetFont(fonstss);
            ICellStyle righttopborderStyle = workbook.CreateCellStyle();
            righttopborderStyle.BorderTop = BorderStyle.Thin; // 下边框
            righttopborderStyle.BorderRight = BorderStyle.Thin; // 右边框
            righttopborderStyle.WrapText = true; // 启用自动换行
            righttopborderStyle.ShrinkToFit = false; // 启用字体缩小以适应单元格
            righttopborderStyle.VerticalAlignment = VerticalAlignment.Center;
            righttopborderStyle.SetFont(fonstss);
            ICellStyle borderStyles = workbook.CreateCellStyle();
            borderStyles.BorderTop = BorderStyle.Thin; // 上边框
            borderStyles.BorderBottom = BorderStyle.Thin; // 下边框
            borderStyles.WrapText = true; // 启用自动换行
            borderStyles.ShrinkToFit = false; // 启用字体缩小以适应单元格
            borderStyles.VerticalAlignment = VerticalAlignment.Center;
            borderStyles.SetFont(fonstss);

            int rownums = 0;
            
            // 获取 页数是 每一张的 页数 非全部页数
            // 页数是第一页是 除去前五行 即是有35行数据时 为第一页 后续页每有35行为一页 最后则是为数据行
            
            
            for (int i = 0; i < alleout.Count; i++)
            {

                // 第一行: 表头
                IRow row0 = sheet.CreateRow(rownums);
                row0.HeightInPoints = 25;
                ICell cellTitle = row0.CreateCell(0);
                cellTitle.SetCellValue("黔南州人民医院" + alleout[i].DrugDeptCode);
                cellTitle.CellStyle = headerStyle;
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 0, 12));

                rownums++;
                IRow row00 = sheet.CreateRow(rownums);
                row00.HeightInPoints = 24;
                ICell cellTitles = row00.CreateCell(0);
                cellTitles.SetCellValue(" 出库单 ");
                cellTitles.CellStyle = headerStyles;
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 0, 12));
                rownums++;

              
                IRow row1 = sheet.CreateRow(rownums);
                ICell cerow1 = row1.CreateCell(0);
                cerow1.SetCellValue($"第{alleout[i].Page}页/共{alleout[i].AllPage}页");
                cerow1.CellStyle = wrapTextStyleS;
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 0, 2));
                if (alleout[i].OutType == "出库退货"|| alleout[i].OutType == "出库退库"|| alleout[i].OutType == "住院退药"|| alleout[i].OutType == "门诊退药")
                {
                        ICell cerow0098 = row1.CreateCell(6);
                        cerow0098.SetCellValue($"(冲单)");
                        cerow0098.CellStyle = wrapTextStyleS;
                }
        

                
                ICell cerow18 = row1.CreateCell(8);
                cerow18.SetCellValue($"出库类型:{alleout[i].OutType}");
                cerow18.CellStyle = wrapTextStyleS;
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 8, 12));
                
                
                rownums++;

                // 第三行: 领取部门等信息
                IRow row2 = sheet.CreateRow(rownums);
                ICell cerow20 = row2.CreateCell(0);
                cerow20.SetCellValue($"领取部门:{alleout[i].DrugStorageCode}");
                cerow20.CellStyle = wrapTextStyleS;
                ICell cerow24 = row2.CreateCell(4);
                cerow24.SetCellValue($"领药人:{alleout[i].GetPerson}");
                cerow24.CellStyle = wrapTextStyleS;
                ICell cerow28 = row2.CreateCell(8);
                cerow28.SetCellValue($"单号:{alleout[i].Billcode}");
                cerow28.CellStyle = wrapTextStyleS;
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 0, 3));
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 4, 7));
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 8, 12));

                rownums++;

                IRow row3 = sheet.CreateRow(rownums);
                ICell cerow30 = row3.CreateCell(0);
                cerow30.SetCellValue($"发出仓库:{alleout[i].DrugDeptCode}");
                cerow30.CellStyle = wrapTextStyleS;
                ICell cerow34 = row3.CreateCell(4);
                cerow34.SetCellValue($"领用时间:{alleout[i].GTime}");
                cerow34.CellStyle = wrapTextStyleS;
                ICell cerow38 = row3.CreateCell(8);
                cerow38.SetCellValue($"备注:{alleout[i].Mark}");
                cerow38.CellStyle = wrapTextStyleS;

                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 0, 3));

                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 4, 7));
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 8, 12));              
                rownums++;

                // 第四行: 药品表头
                IRow row4 = sheet.CreateRow(rownums);
                string[] headers = { "药品名称", "规格", "单位", "数量", "购入价", "购入金额", "零售价", "零售金额", "生产厂家", "批号", "有效期", "仓库货位","领用部门货位" };
                for (int f = 0; f < headers.Length; f++)
                {
                    row4.CreateCell(f).SetCellValue(headers[f]);
                    ICell cell = row4.CreateCell(f);
                    cell.SetCellValue(headers[f]);
                    cell.CellStyle = wrapTextStyle;
                }
                rownums++;
                int p = alleout[i].Drugs.Count + rownums ;
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
                        sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 6, 11));

                        rownums++;
                        ass++;
                        f++;
                        p++;

                        // 大写金额行
                        IRow prow9 = sheet.CreateRow(rownums);
                        ICell pcerow90 = prow9.CreateCell(1);
                        pcerow90.SetCellValue($"大写批价金额:{alleout[i].ChinaSumApproveCost}");
                        pcerow90.CellStyle = wrapTextStyle;
                        ICell pcerow91 = prow9.CreateCell(6);
                        pcerow91.SetCellValue($"大写零售金额:{alleout[i].ChinaSumSaleCost}"); 
                        pcerow91.CellStyle = wrapTextStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 1, 5));
                        sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 6, 11));

                        rownums++;
                        ass++;
                        f++;
                        p++;

                        // 底部签字行
                        IRow prow10 = sheet.CreateRow(rownums);
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

                        sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 9, 12));

                        for (int col = 0; col < 13; col++)
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
                            else if (col == 12)
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
                    ICellStyle TradeNamecell = wrapTextStyle;
                    //cell0.CellStyle = TradeNamecell;
                    SetCellValueAutoFit(row, 0, dr.TradeName, TradeNamecell, fontss, sheet, 0, 28);

                    //ICell cell1 = row.CreateCell(1);
                    //cell1.SetCellValue($"{dr.Specs}");
                 
                    ICellStyle Specscell = wrapTextStyle;
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


                    //// 设置规格列
                    //ICell specCell = row.CreateCell(8);

                    //specCell.SetCellValue($"{dr.ProducerCode}");
                    ////specCell.CellStyle = wrapTextStyle; // 应用自动换行样式

                    ICellStyle ProducerCodecell = wrapTextStyle;
                    //specCell.CellStyle = ProducerCodecell;
                    SetCellValueAutoFit(row, 8, dr.ProducerCode, ProducerCodecell, fontss, sheet, 8, 28);
                    //specCell.CellStyle = borderStyle; // 也可以应用边框样式

                    ICell cell9 = row.CreateCell(9);
                    cell9.SetCellValue($"{dr.BatchNo}");
                    cell9.CellStyle = wrapTextStyle;

                    //ICell cell10 = row.CreateCell(10);
                    //cell10.SetCellValue($"{dr.ValidDate}");
                    ICellStyle ValidDatecell = wrapTextStyle;
                    //cell10.CellStyle = ProducerCodecell;
                    SetCellValueAutoFit(row, 10, dr.ValidDate, ValidDatecell, fontss, sheet, 10, 28);

                    ICell cell11 = row.CreateCell(11);
                    cell11.SetCellValue($"");
                    cell11.CellStyle = wrapTextStyle;

                    ICell cell12 = row.CreateCell(12);
                    cell12.SetCellValue($"{dr.LoctionName}");
                    cell12.CellStyle = wrapTextStyle;
                    

                    rownums++;
                }
          


                // 合计行
                IRow row8 = sheet.CreateRow(rownums);

                ICell cerow80 = row8.CreateCell(0);
                cerow80.SetCellValue($"当前页{alleout[i].Drugs.Count%30}笔/共{alleout[i].Drugs.Count}笔");
                cerow80.CellStyle = wrapTextStyle;
                ICell cerow81 = row8.CreateCell(1);
                cerow81.SetCellValue($"批价总金额:{alleout[i].SumApproveCost}");
                cerow81.CellStyle = wrapTextStyle;
                ICell cerow86 = row8.CreateCell(6);
                cerow86.SetCellValue($"零售总金额:{alleout[i].SumSaleCost}");
                cerow86.CellStyle = wrapTextStyle;
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 1, 5));
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 6, 11));

                rownums++;

                // 大写金额行
                IRow row9 = sheet.CreateRow(rownums);
                ICell cerow90 = row9.CreateCell(1);
                cerow90.SetCellValue($"大写批价金额:{alleout[i].ChinaSumApproveCost}");
                cerow90.CellStyle = wrapTextStyle;
                ICell cerow91 = row9.CreateCell(6);
                cerow91.SetCellValue($"大写零售金额:{alleout[i].ChinaSumSaleCost}"); 
                cerow91.CellStyle = wrapTextStyle;
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 1, 5));
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 6, 11));
                rownums++;
                // 底部签字行
                IRow row10 = sheet.CreateRow(rownums);
                ICell cerow100 = row10.CreateCell(0);
                cerow100.SetCellValue($"发药人");
                cerow100.CellStyle = wrapTextStyle;
                ICell cerow102 = row10.CreateCell(2);
                cerow102.SetCellValue($"复核");
                cerow102.CellStyle = wrapTextStyle;
                ICell cerow103 = row10.CreateCell(5);
                cerow103.SetCellValue($"主任");
                cerow103.CellStyle = wrapTextStyle;
                ICell cerow104 = row10.CreateCell(7);
                cerow104.SetCellValue($"经手人");
                cerow104.CellStyle = wrapTextStyle;
                ICell cerow105 = row10.CreateCell(9);
                cerow105.SetCellValue($"打印时间  {alleout[i].NowgetTime}");     
                cerow105.CellStyle = wrapTextStyle;             
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 9, 12));
                for (int col = 0; col < 13; col++)
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
                        bordersStyles.WrapText = false; // 启用自动换行
                        bordersStyles.ShrinkToFit = true; // 启用字体缩小以适应单元格
                        bordersStyles.VerticalAlignment = VerticalAlignment.Center;
                        bordersStyles.SetFont(fonstss);
                        cell10.CellStyle = bordersStyles;
                    }
                    else if (col == 12)
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
                        bordersStyles.WrapText = false; // 启用自动换行
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

                rownums++;
                rownums++;
                rownums++;

            }

           
            IWebHostEnvironment webHostEnvironment = (IWebHostEnvironment)App.ServiceProvider.GetService(typeof(IWebHostEnvironment));
            string sFileName = $"出库单_{DateTime.Now:MM-dd-HHmmss}.xlsx";
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
        /// <summary>
        /// 退货导出
        /// </summary> 
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("GenerateExcels")]
        public IActionResult GenerateExcels([FromQuery] List<int> parm)
        {
            List<EOut> alleout = new();
            List<EIn> lEin = new();
            foreach (var item in parm)
            {
                lEin = new();
                EOut eOut = new EOut();
                eOut.Demo = "黔南州人民医院";
                var list = _OutOrderService.GetInfoss(item);
                eOut.OutType = _OutOrderService.Byin(list.Type);
                eOut.DrugDeptCode = list.OutWarehouseName;
                eOut.DrugStorageCode = list.InpharmacyName;
                eOut.GetPerson = list.UseReceive;
                eOut.Billcode = list.OutOrderCode;
                eOut.biidess = list.Billcodesf;
                 //Controller获取用户名
                 var userName = HttpContext.GetUId();
                eOut.GetPerson = _sysUserService.SelectUserById(userName).NickName;
                eOut.Mark = list.Remarks;
                eOut.GTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                decimal SumApp = 0;
                decimal SumSale = 0;
                var ow = _OutDrugsService.EList(item);
                foreach (var t in ow)
                {
                    EIn eIn = new();
                    if (!string.IsNullOrEmpty(t.InListCode))
                    {
                        eOut.Billcode = t.InListCode;
                    }
                    eIn.TradeName = t.TradeName;
                    eIn.Specs = t.Specs;
                    eIn.PackUnit = t.PackUnit;
                    eIn.PackQty = (t.InNum /  (decimal)(t.PackQty.GetValueOrDefault(0))).ToString("G0");
                    eIn.PurchasePrice = t.PurchasePrice.ToString("G0");
                    eIn.ApproveCost = t.PurchaseCost.ToString("G0");
                    eIn.RetailPrice = t.RetailPrice.ToString("G0");
                    eIn.SaleCost = t.RetailCost.ToString("G0");
                    eIn.ProducerCode = t.ProducerCode.ToString();
                    eIn.ValidDate = t.ValidDate?.ToString("yyyy-MM-dd");
                    eIn.BatchNo = t.BatchNo;
                    eOut.GTime = t.OperDate.ToString();
                   
                    //eOut.OutType = t.OutType;
                    SumApp += t.PurchaseCost;
                    SumSale += t.RetailCost;
                    lEin.Add(eIn);
                }
                eOut.Drugs = lEin;
                eOut.SumApproveCost = SumApp.ToString("G0");
                eOut.SumSaleCost = SumSale.ToString("G0");
                eOut.ChinaSumApproveCost = ConvertToChinese(SumApp).ToString();
                eOut.ChinaSumSaleCost = ConvertToChinese(SumSale).ToString();
                eOut.num = ow.Count().ToString();
                eOut.NowgetTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                int numbers = lEin.Count();
                int number = numbers / 30;
                int number1 = numbers % 30;
                if (number1 > 0)
                {
                    number = number + 1;
                }
                eOut.Page = 1;
                eOut.AllPage = number;
                alleout.Add(eOut);
            }

            // 创建工作簿
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("入库验收单（采购退货）");

            int rownums = 0;


            // 设置列宽
            sheet.SetColumnWidth(0, 15 * 256);
            sheet.SetColumnWidth(1, 10 * 256);
            sheet.SetColumnWidth(2, 6 * 256);
            sheet.SetColumnWidth(3, 6 * 256);
            sheet.SetColumnWidth(4, 8 * 256);
            sheet.SetColumnWidth(5, 8 * 256);
            sheet.SetColumnWidth(6, 8 * 256);
            sheet.SetColumnWidth(7, 8 * 256);
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
            font.FontHeightInPoints = 14;
            font.Boldweight = (short)FontBoldWeight.Bold;
            headerStyle.SetFont(font);
            // 创建单元格样式（带自动换行）


            ICellStyle headerStyles = workbook.CreateCellStyle();
            headerStyles.Alignment = HorizontalAlignment.Center;
            headerStyles.VerticalAlignment = VerticalAlignment.Center;
            IFont fonts = workbook.CreateFont();
            fonts.FontHeightInPoints = 18;
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
          // wrapTextStyle.ShrinkToFit = true; // 启用字体缩小以适应单元格

            wrapTextStyle.WrapText = true; // 启用自动换行
            wrapTextStyle.VerticalAlignment = VerticalAlignment.Center;
            IFont fontAs = workbook.CreateFont();
            fontAs.FontName = "微软雅黑";
            fontAs.FontHeightInPoints = 10;
            wrapTextStyle.SetFont(fontAs);

            IFont fontss = workbook.CreateFont();
            fontss.FontName = "微软雅黑";
            fontss.FontHeightInPoints = 10;

            ICellStyle wrapTextStyleS = workbook.CreateCellStyle();
            wrapTextStyleS.ShrinkToFit = true; // 启用字体缩小以适应单元格

            wrapTextStyleS.WrapText = true; // 启用自动换行
            wrapTextStyleS.VerticalAlignment = VerticalAlignment.Center;
            IFont fonstss = workbook.CreateFont();
            fonstss.FontHeightInPoints = 10;
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
                row0.HeightInPoints = 20;
                ICell cellTitle = row0.CreateCell(0);
                cellTitle.SetCellValue("黔南州人民医院" + alleout[i].DrugDeptCode);
                cellTitle.CellStyle = headerStyle;
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 0, 10));

                rownums++;
                IRow row00 = sheet.CreateRow(rownums);
                row00.HeightInPoints = 24;
                ICell cellTitles = row00.CreateCell(0);
                cellTitles.SetCellValue("入库验收单  (冲单)");
                cellTitles.CellStyle = headerStyles;
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 0, 10));
                rownums++;


                IRow row1 = sheet.CreateRow(rownums);
                row1.HeightInPoints = 15;

                ICell cerow1 = row1.CreateCell(0);
                cerow1.SetCellValue($"第{alleout[i].Page}页/共{alleout[i].AllPage}页");
                cerow1.CellStyle = wrapTextStyleS;
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 0, 2));
        
                ICell cerow18 = row1.CreateCell(8);
                cerow18.SetCellValue($"{alleout[i].OutType}");
                cerow18.CellStyle = wrapTextStyleS;
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 9, 10));
                rownums++;
                // 第三行: 领取部门等信息
                IRow row2 = sheet.CreateRow(rownums);
                ICell cerow20 = row2.CreateCell(0);
                cerow20.SetCellValue($"单号:{alleout[i].Billcode}");
                cerow20.CellStyle = wrapTextStyleS;

                ICell cerow28 = row2.CreateCell(8);
                cerow28.SetCellValue($"备注:{alleout[i].Mark}");
                cerow28.CellStyle = wrapTextStyleS;
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 0, 5));
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 8, 10));

                rownums++;

                IRow row3 = sheet.CreateRow(rownums);
                ICell cerow30 = row3.CreateCell(0);
                cerow30.SetCellValue($"供货单位:{alleout[i].DrugStorageCode}");
                cerow30.CellStyle = wrapTextStyleS;
                ICell cerow34 = row3.CreateCell(4);
                cerow34.SetCellValue($"日期:{alleout[i].GTime}");
                cerow34.CellStyle = wrapTextStyleS;
                ICell cerow38 = row3.CreateCell(8);
                cerow38.SetCellValue($"发票号:{alleout[i].biidess}");
                cerow38.CellStyle = wrapTextStyleS;

                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 0, 3));

                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 4, 7));
                sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 8, 10));

                //sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 3));
                //sheet.AddMergedRegion(new CellRangeAddress(2, 2, 9, 13));
                rownums++;

                // 第四行: 药品表头
                IRow row4 = sheet.CreateRow(rownums);
                ICellStyle heads = CloneCellStyle(workbook,wrapTextStyle);
                string[] headers = { "药品名称", "规格", "单位", "数量", "批价", "批价金额", "零价", "零售金额", "生产厂家", "批号", "有效期" };
                for (int f = 0; f < headers.Length; f++)
                {
                    row4.CreateCell(f).SetCellValue(headers[f]);
                    ICell cell = row4.CreateCell(f);
                    cell.SetCellValue(headers[f]);
                    cell.CellStyle = heads;
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
                        sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 6, 11));

                        rownums++;
                        ass++;
                        f++;
                        p++;

                        // 大写金额行
                        IRow prow9 = sheet.CreateRow(rownums);
                        ICell pcerow90 = prow9.CreateCell(1);
                        pcerow90.SetCellValue($"大写批价金额:{alleout[i].ChinaSumApproveCost}");
                        pcerow90.CellStyle = wrapTextStyle;
                        ICell pcerow91 = prow9.CreateCell(6);
                        pcerow91.SetCellValue($"大写零售金额:{alleout[i].ChinaSumSaleCost}"); 
                        pcerow91.CellStyle = wrapTextStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 1, 5));
                        sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 6, 11));

                        rownums++;
                        ass++;
                        f++;
                        p++;

                        // 底部签字行
                        IRow prow10 = sheet.CreateRow(rownums);
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

                        sheet.AddMergedRegion(new CellRangeAddress(rownums, rownums, 9, 12));

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
                    var dr = alleout[i].Drugs[rownums - f];
                    // 创建并设置单元格值，同时应用边框样式
                    //ICell cell0 = row.CreateCell(0);
                    //cell0.SetCellValue($"{dr.TradeName}");
                    ICellStyle TradeNamecell = CloneCellStyle(workbook, wrapTextStyle);
                    ////cell0.CellStyle = TradeNamecell;
                    SetCellValueAutoFit(row, 0, dr.TradeName, TradeNamecell, fontss, sheet, 0, 28);
           
                    //ICell cell1 = row.CreateCell(1);                    
                    //cell1.SetCellValue($"{dr.Specs}");
                    ICellStyle Specscell = CloneCellStyle(workbook, wrapTextStyle);
                    //cell1.CellStyle = Specscell;
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


                    //// 设置规格列
                    //ICell specCell = row.CreateCell(8);
                    //specCell.SetCellValue($"{dr.ProducerCode}");
                    ////specCell.CellStyle = wrapTextStyle; // 应用自动换行样式
                    ICellStyle ProducerCodecell = CloneCellStyle(workbook, wrapTextStyle);
                    //specCell.CellStyle = ProducerCodecell;
                    SetCellValueAutoFit(row, 8, dr.ProducerCode, ProducerCodecell, fontss, sheet, 8, 28);
                    

                    ICell cell9 = row.CreateCell(9);
                    cell9.SetCellValue($"{dr.BatchNo}");
                    cell9.CellStyle = wrapTextStyle;

                    //ICell cell10 = row.CreateCell(10);
                    //cell10.SetCellValue($"{dr.ValidDate}");
                    ICellStyle ValidDatecell = CloneCellStyle(workbook, wrapTextStyle);
                    //cell10.CellStyle = ValidDatecell;
                    SetCellValueAutoFit(row, 10, dr.ValidDate, ValidDatecell, fontss, sheet, 10, 28);


                    rownums++;
                }
                // 合计行
                // 合计行
                IRow row8 = sheet.CreateRow(rownums);

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
                ICell cerow100 = row10.CreateCell(0);
                cerow100.SetCellValue($"负责人：                  采购人：                  验收人：                    审核人：                 入库人：               {alleout[i].NowgetTime}");
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
                        bordersStyles.WrapText = false; // 启用自动换行
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
                        bordersStyles.WrapText = false; // 启用自动换行
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
               

                rownums++;
                rownums++;
                rownums++;
        
                ////footer.Center = "";
                //sheet.SetRowBreak(rownums++);


            }


            IWebHostEnvironment webHostEnvironment = (IWebHostEnvironment)App.ServiceProvider.GetService(typeof(IWebHostEnvironment));
            string sFileName = $"退货单_{DateTime.Now:MM-dd-HHmmss}.xlsx";
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
        // 固定列宽和行高，且数据能完全显示的实现方案
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