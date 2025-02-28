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
    /// 库存Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IPhaStorageService), ServiceLifetime = LifeTime.Transient)]
    public class PhaStorageService : BaseService<PhaStorage>, IPhaStorageService
    {
        /// <summary>
        /// 查询库存列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<PhaStorageDto> GetList(PhaStorageQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .LeftJoin<Departments>((it, p) => it.DrugDeptCode == p.DeptCode)
                .LeftJoin<CompanyInfo>((it,p,s)=>it.ProducerCode==s.FacCode)
                //.LeftJoin<GuiDrug>((it, p,s) => it.DrugCode == s.DrugTermId)
                .Where(predicate.ToExpression())
                .Where((it, p) => string.IsNullOrEmpty(parm.DrugDeptCode)|| p.DeptName.Contains(parm.DrugDeptCode))
                .Where((it, p,s) => string.IsNullOrEmpty(parm.ProducerCode) || s.FacName.Contains(parm.ProducerCode))

                .Select((it, p,s) => new PhaStorage
                   {
                      StoreSum=it.StoreSum/it.PackQty,
                       DrugDeptCode = p.DeptName,
                       ProducerCode=s.FacName
                       //DrugCode=s.TradeName
                   },true)
                .ToPage<PhaStorage, PhaStorageDto>(parm);

            return response;
        }
        public PagedInfo<PhaStorageDtos> exGetList(PhaStorageQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .LeftJoin<Departments>((it, p) => it.DrugDeptCode == p.DeptCode)
                .LeftJoin<CompanyInfo>((it, p, s) => it.ProducerCode == s.FacCode)
                //.LeftJoin<GuiDrug>((it, p,s) => it.DrugCode == s.DrugTermId)
                .Where(predicate.ToExpression())
                .Where((it, p) => string.IsNullOrEmpty(parm.DrugDeptCode) || p.DeptName.Contains(parm.DrugDeptCode))
                   .Where((it, p, s) => string.IsNullOrEmpty(parm.ProducerCode) || s.FacName.Contains(parm.ProducerCode))
                .Select((it, p, s) => new PhaStorage
                {
                    DrugDeptCode = p.DeptName,
                    ProducerCode = s.FacName
                    //DrugCode=s.TradeName
                }, true)
                .ToPage<PhaStorage, PhaStorageDtos>(parm);

            return response;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public PhaStorage GetInfo(int Id)
        {
            var response = Queryable()
                //.Where(x => x.Id == Id)
                .First();

            return response;
        }

        /// <summary>
        /// 添加库存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PhaStorage AddPhaStorage(PhaStorage model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }
        public int MIXAddPhaStorage(List<PhaStorage> model)
        {
            return MixAdd(model);
        }
        
        /// <summary>
        /// 修改库存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdatePhaStorage(PhaStorage model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 清空库存
        /// </summary>
        /// <returns></returns>
        public bool TruncatePhaStorage()
        {
            var newTableName = $"PhaStorage_{DateTime.Now:yyyyMMdd}";
            if (Queryable().Any() && !Context.DbMaintenance.IsAnyTable(newTableName))
            {
                Context.DbMaintenance.BackupTable("PhaStorage", newTableName);
            }

            return Truncate();
        }

        public bool TruncatePhaStoragesss()
        {
            //var newTableName = $"PhaStorage_{DateTime.Now:yyyyMMdd}";
            //if (Queryable().Any() && !Context.DbMaintenance.IsAnyTable(newTableName))
            //{
            //    Context.DbMaintenance.BackupTable("PhaStorage", newTableName);
            //}
            return Truncate();
        }
        /// <summary>
        /// 导入库存
        /// </summary>
        /// <returns></returns>
        public (string, object, object) ImportPhaStorage(List<PhaStorage> list)
        {
            var x = Context.Storageable(list)
                .SplitInsert(it => !it.Any())
                .SplitError(x => x.Item.DrugDeptCode.IsEmpty(), "库存科室不能为空")
                .SplitError(x => x.Item.DrugCode.IsEmpty(), "药品编码不能为空")
                .SplitError(x => x.Item.TradeName.IsEmpty(), "药品商品名不能为空")
                .SplitError(x => x.Item.GroupCode.IsEmpty(), "批次号不能为空")
                .SplitError(x => x.Item.ProducerCode.IsEmpty(), "生产厂家不能为空")
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
        public List<PhaStorage> GetALL(string DeptCode)
        {
            var response = Queryable()
               .Where(x => x.DrugDeptCode == DeptCode)
               .ToList();

            return response;
        }
        public decimal? GetALLme(string DeptCode)
        {
            var response = Queryable()
                 .LeftJoin<Departments>((it, p) => p.DeptCode == it.DrugDeptCode)
                 .Where((it, p) => p.DeptName == DeptCode)
                 .Sum((it)=>it.StoreCost)
                ;
             

            //var subQuery = Context.Queryable<PhaStorage>()
            //    .LeftJoin<Departments>((it, p) => p.DeptCode == it.DrugDeptCode)
            //    .Where((it, p) => p.DeptName == DeptCode)
            //    .GroupBy(it => new { it.DrugCode, it.StoreCost })
            //    .Select(it => new { it.DrugCode, it.StoreCost });

            //// 主查询：对分组后的结果计算总价值
            //var totalStockValue = Context.Queryable(subQuery)
            //   .Sum(x => x.StoreCost);





            return response;
        }


        /// <summary>
        /// 导出库存
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<PhaStorageDto> ExportList(PhaStorageQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .Select((it) => new PhaStorageDto()
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
        private static Expressionable<PhaStorage> QueryExp(PhaStorageQueryDto parm)
        {
            var predicate = Expressionable.Create<PhaStorage>();

            //predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugDeptCode), it => it.DrugDeptCode == parm.DrugDeptCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugCode), it => it.DrugCode == parm.DrugCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.TradeName), it => it.TradeName.Contains(parm.TradeName));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugType), it => it.DrugType == parm.DrugType);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.PlaceCode), it => it.PlaceCode == parm.PlaceCode);
            //predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ProducerCode), it => it.ProducerCode == parm.ProducerCode);
            return predicate;
        }

        public PhaStorage GetisInfo(string DrugCode,string DeptCode)
        {
            var response = Queryable()
               .Where(x => x.DrugCode == DrugCode&&x.DrugDeptCode==DeptCode)
               .First();

            return response;


        }
    }
}