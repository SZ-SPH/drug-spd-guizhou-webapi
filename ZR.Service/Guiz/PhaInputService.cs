using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Model.GuiHis.Dto;
using ZR.Service.Guiz.IGuizService;
using ZR.Model.GuiHis;


namespace ZR.Service.Guiz
{
    /// <summary>
    /// 入库详情Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IPhaInputService), ServiceLifetime = LifeTime.Transient)]
    public class PhaInputService : BaseService<PhaInput>, IPhaInputService
    {
        /// <summary>
        /// 查询入库详情列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<PhaInputDto> GetList(PhaInputQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .ToPage<PhaInput, PhaInputDto>(parm);

            return response;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public PhaInput GetInfo(int Id)
        {
            var response = Queryable()
                //.Where(x => x.Id == Id)
                .First();

            return response;
        }

        /// <summary>
        /// 添加入库详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PhaInput AddPhaInput(PhaInput model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改入库详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdatePhaInput(PhaInput model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 清空入库详情
        /// </summary>
        /// <returns></returns>
        public bool TruncatePhaInput()
        {
            var newTableName = $"PhaInput_{DateTime.Now:yyyyMMdd}";
            if (Queryable().Any() && !Context.DbMaintenance.IsAnyTable(newTableName))
            {
                Context.DbMaintenance.BackupTable("PhaInput", newTableName);
            }

            return Truncate();
        }
        /// <summary>
        /// 导入入库详情
        /// </summary>
        /// <returns></returns>
        public (string, object, object) ImportPhaInput(List<PhaInput> list)
        {
            var x = Context.Storageable(list)
                .SplitInsert(it => !it.Any())
                .SplitError(x => x.Item.PlanNo.IsEmpty(), "入库计划流水号不能为空")
                .SplitError(x => x.Item.BillCode.IsEmpty(), "采购单号不能为空")
                .SplitError(x => x.Item.StockNo.IsEmpty(), "采购流水号不能为空")
                .SplitError(x => x.Item.SerialCode.IsEmpty(), "序号不能为空")
                .SplitError(x => x.Item.DrugDeptCode.IsEmpty(), "库存科室不能为空")
                .SplitError(x => x.Item.GroupCode.IsEmpty(), "批次号不能为空")
                .SplitError(x => x.Item.InType.IsEmpty(), "入库类型不能为空")
                .SplitError(x => x.Item.Class3MeaningCode.IsEmpty(), "入库分类不能为空")
                .SplitError(x => x.Item.DrugCode.IsEmpty(), "药品编码不能为空")
                .SplitError(x => x.Item.TradeName.IsEmpty(), "药品商品名不能为空")
                .SplitError(x => x.Item.Specs.IsEmpty(), "规格不能为空")
                .SplitError(x => x.Item.PackUnit.IsEmpty(), "包装单位不能为空")
                .SplitError(x => x.Item.PackQty.IsEmpty(), "包装数不能为空")
                .SplitError(x => x.Item.MinUnit.IsEmpty(), "最小单位不能为空")
                .SplitError(x => x.Item.BatchNo.IsEmpty(), "批号不能为空")
                .SplitError(x => x.Item.ValidDate.IsEmpty(), "有效期不能为空")
                .SplitError(x => x.Item.ProducerCode.IsEmpty(), "生产厂家不能为空")
                .SplitError(x => x.Item.CompanyCode.IsEmpty(), "供货单位代码不能为空")
                .SplitError(x => x.Item.RetailPrice.IsEmpty(), "零售价不能为空")
                .SplitError(x => x.Item.WholesalePrice.IsEmpty(), "批发价不能为空")
                .SplitError(x => x.Item.PurchasePrice.IsEmpty(), "购入价不能为空")
                .SplitError(x => x.Item.InNum.IsEmpty(), "入库数量不能为空")
                .SplitError(x => x.Item.RetailCost.IsEmpty(), "零售金额不能为空")
                .SplitError(x => x.Item.WholesaleCost.IsEmpty(), "批发金额不能为空")
                .SplitError(x => x.Item.PurchaseCost.IsEmpty(), "购入金额不能为空")
                .SplitError(x => x.Item.SpecialFlag.IsEmpty(), "特殊标记不能为空")
                .SplitError(x => x.Item.InState.IsEmpty(), "入库状态不能为空")
                .SplitError(x => x.Item.ApplyNum.IsEmpty(), "申请入库量不能为空")
                .SplitError(x => x.Item.ApplyOperCode.IsEmpty(), "申请入库操作员不能为空")
                .SplitError(x => x.Item.ApplyDate.IsEmpty(), "申请入库日期不能为空")
                .SplitError(x => x.Item.ExamNum.IsEmpty(), "审批数量不能为空")
                .SplitError(x => x.Item.ExamOperCode.IsEmpty(), "审批人不能为空")
                .SplitError(x => x.Item.ExamDate.IsEmpty(), "审批日期不能为空")
                .SplitError(x => x.Item.ApproveOperCode.IsEmpty(), "核准人不能为空")
                .SplitError(x => x.Item.ApproveDate.IsEmpty(), "核准日期不能为空")
                .SplitError(x => x.Item.OperCode.IsEmpty(), "操作员不能为空")
                .SplitError(x => x.Item.PurcharsePriceFirsttime.IsEmpty(), "一般入库时的购入价不能为空")
                .SplitError(x => x.Item.IsTenderOffer.IsEmpty(), "招标标记不能为空")
                .SplitError(x => x.Item.ProductionDate.IsEmpty(), "生产日期不能为空")
                .SplitError(x => x.Item.ApproveInfo.IsEmpty(), "批文信息不能为空")
                //.WhereColumns(it => it.UserName)//如果不是主键可以这样实现（多字段it=>new{it.x1,it.x2}）
                .ToStorage();
            var result = x.AsInsertable.ExecuteCommand();//插入可插入部分;

            string msg = $"插入{x.InsertList.Count} 更新{x.UpdateList.Count} 错误数据{x.ErrorList.Count} 不计算数据{x.IgnoreList.Count} 删除数据{x.DeleteList.Count} 总共{x.TotalList.Count}";
            Console.WriteLine(msg);

            //输出错误信息               
            foreach (var item in x.ErrorList)
            {
                Console.WriteLine("错误" + item.StorageMessage);
            }
            foreach (var item in x.IgnoreList)
            {
                Console.WriteLine("忽略" + item.StorageMessage);
            }

            return (msg, x.ErrorList, x.IgnoreList);
        }

        /// <summary>
        /// 导出入库详情
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<PhaInputDto> ExportList(PhaInputQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .Select((it) => new PhaInputDto()
                {
                }, true)
                .ToPage(parm);

            return response;
        }

        /// <summary>
        /// 查询导出表达式
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private static Expressionable<PhaInput> QueryExp(PhaInputQueryDto parm)
        {
            var predicate = Expressionable.Create<PhaInput>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.BillCode), it => it.BillCode == parm.BillCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.StockNo), it => it.StockNo == parm.StockNo);
            predicate = predicate.AndIF(parm.SerialCode != null, it => it.SerialCode == parm.SerialCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugDeptCode), it => it.DrugDeptCode == parm.DrugDeptCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.GroupCode), it => it.GroupCode == parm.GroupCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.InType), it => it.InType == parm.InType);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugCode), it => it.DrugCode == parm.DrugCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.TradeName), it => it.TradeName.Contains(parm.TradeName));
            return predicate;
        }
    }
}