using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Service.Business.IBusinessService;
using ZR.Model.GuiHis;

namespace ZR.Service.Business
{
    /// <summary>
    /// 出库药品详情Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IOuWarehousetService), ServiceLifetime = LifeTime.Transient)]
    public class OuWarehousetService : BaseService<OuWarehouset>, IOuWarehousetService
    {
        /// <summary>
        /// 查询出库药品详情列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<OuWarehousetDto> GetList(OuWarehousetQueryDto parm)
        {
            var predicate = QueryExp(parm);
            //predicate = predicate.AndIF(parm.BeginCreateTime != null, it => it.CreateTime >= parm.BeginCreateTime);
            //predicate = predicate.AndIF(parm.EndCreateTime != null, it => it.CreateTime <= parm.EndCreateTime);
            var response = Queryable()
                .LeftJoin<Departments>((it, p) => it.DrugDeptCode == p.DeptCode)
                .LeftJoin<Departments>((it, p,s) => it.DrugStorageCode == s.DeptCode)
                .LeftJoin<OutOrder>((it,p,s,ad)=>it.OutorderID==ad.Id)
                .Where(predicate.ToExpression())
                .Where((it, p, s) => string.IsNullOrEmpty(parm.DrugDeptCode) || p.DeptName.Contains(parm.DrugDeptCode))
                .Where((it, p, s) =>string.IsNullOrEmpty(parm.DrugStorageCode)||s.DeptName.Contains(parm.DrugStorageCode))
                .Where((it, p, s,ad)=> ad.CreateTime >= parm.BeginCreateTime)
                .Where((it, p, s, ad) =>  ad.CreateTime <= parm.EndCreateTime)      
                .OrderByDescending((it)=>it.OperDate)
                .Select((it, p, s,ad)=>new OuWarehouset
                {
                    DrugDeptCode = p.DeptName,
                    DrugStorageCode=s.DeptName
                },true)
                .ToPage<OuWarehouset, OuWarehousetDto>(parm);

            return response;
        }
        public class DAYprice
        {
            public decimal? AllPurchasePrice {  get; set; }
            public decimal? AllRetailPrice { get; set; }

        }
        public DAYprice DAYGetList(OuWarehousetQueryDto parm)
        {
            var predicate = QueryExp(parm);
            //predicate = predicate.AndIF(parm.BeginCreateTime != null, it => it.CreateTime >= parm.BeginCreateTime);
            //predicate = predicate.AndIF(parm.EndCreateTime != null, it => it.CreateTime <= parm.EndCreateTime);
            var response = Queryable()
                .LeftJoin<Departments>((it, p) => it.DrugDeptCode == p.DeptCode)
                .LeftJoin<Departments>((it, p, s) => it.DrugStorageCode == s.DeptCode)
                .LeftJoin<OutOrder>((it, p, s, ad) => it.OutorderID == ad.Id)
                .Where(predicate.ToExpression())
                .Where((it, p, s) => string.IsNullOrEmpty(parm.DrugDeptCode) || p.DeptName.Contains(parm.DrugDeptCode))
                .Where((it, p, s) => string.IsNullOrEmpty(parm.DrugStorageCode) || s.DeptName.Contains(parm.DrugStorageCode))
                .Where((it, p, s, ad) => ad.CreateTime >= parm.BeginCreateTime)
                .Where((it, p, s, ad) => ad.CreateTime <= parm.EndCreateTime)
                .Sum((it) => (it.OutNum / it.PackQty) * it.PurchasePrice);

                var predicates = QueryExp(parm);
                //predicate = predicate.AndIF(parm.BeginCreateTime != null, it => it.CreateTime >= parm.BeginCreateTime);
                //predicate = predicate.AndIF(parm.EndCreateTime != null, it => it.CreateTime <= parm.EndCreateTime);
                var responses = Queryable()
                    .LeftJoin<Departments>((it, p) => it.DrugDeptCode == p.DeptCode)
                    .LeftJoin<Departments>((it, p, s) => it.DrugStorageCode == s.DeptCode)
                    .LeftJoin<OutOrder>((it, p, s, ad) => it.OutorderID == ad.Id)
                    .Where(predicate.ToExpression())
                    .Where((it, p, s) => string.IsNullOrEmpty(parm.DrugDeptCode) || p.DeptName.Contains(parm.DrugDeptCode))
                    .Where((it, p, s) => string.IsNullOrEmpty(parm.DrugStorageCode) || s.DeptName.Contains(parm.DrugStorageCode))
                    .Where((it, p, s, ad) => ad.CreateTime >= parm.BeginCreateTime)
                    .Where((it, p, s, ad) => ad.CreateTime <= parm.EndCreateTime)
                    .Sum((it) => (it.OutNum / it.PackQty) * it.RetailPrice);

            return new DAYprice { AllPurchasePrice = response, AllRetailPrice = responses };
        }



        //public decimal AllMixPrice(OuWarehousetQueryDto parm)
        //{
        //    var predicate = QueryExp(parm);

        //    var response = Queryable()
        //        .LeftJoin<Departments>((it, p) => it.DrugDeptCode == p.DeptCode)
        //        .LeftJoin<Departments>((it, p, s) => it.DrugStorageCode == s.DeptCode)
        //        .Where(predicate.ToExpression())
        //        .Where((it, p, s) => string.IsNullOrEmpty(parm.DrugDeptCode) || p.DeptName.Contains(parm.DrugDeptCode))
        //        .Where((it, p, s) => string.IsNullOrEmpty(parm.DrugStorageCode) || s.DeptName.Contains(parm.DrugStorageCode))
        //        .OrderByDescending((it) => it.OperDate)
        //        .Select((it, p, s) => new OuWarehouset
        //        {
        //            DrugDeptCode = p.DeptName,
        //            DrugStorageCode = s.DeptName
        //        }, true);


        //    return response;
        //}

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public OuWarehouset GetInfo(int Id)
        {
            var response = Queryable()
                .Where(x => x.Id == Id)
                .First();

            return response;
        }

        /// <summary>
        /// 添加出库药品详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OuWarehouset AddOuWarehouset(OuWarehouset model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改出库药品详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateOuWarehouset(OuWarehouset model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 清空出库药品详情
        /// </summary>
        /// <returns></returns>
        public bool TruncateOuWarehouset()
        {
            var newTableName = $"OuWarehouset_{DateTime.Now:yyyyMMdd}";
            if (Queryable().Any() && !Context.DbMaintenance.IsAnyTable(newTableName))
            {
                Context.DbMaintenance.BackupTable("OuWarehouset", newTableName);
            }
            
            return Truncate();
        }
        /// <summary>
        /// 导入出库药品详情
        /// </summary>
        /// <returns></returns>
        public (string, object, object) ImportOuWarehouset(List<OuWarehouset> list)
        {
            var x = Context.Storageable(list)
                .SplitInsert(it => !it.Any())
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
        /// 导出出库药品详情
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<OuWarehousetDtos> ExportList(OuWarehousetQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .Select((it) => new OuWarehousetDtos()
                {
                }, true)
                .ToPage(parm);

            return response;
        }
        public List<OuWarehouset> EList(int parm)
        {
            //var predicate = QueryExp(parm);

            var response = Queryable().LeftJoin<CompanyInfo>((it,p)=>it.ProducerCode==p.FacCode)
                .Where(it => it.OutorderID == parm).Select((it,p)=>new OuWarehouset()
                {
                    ProducerCode=p.FacName
                },true).ToList();
            return response;
        }
        /// <summary>
        /// 查询导出表达式
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private static Expressionable<OuWarehouset> QueryExp(OuWarehousetQueryDto parm)
        {
            var predicate = Expressionable.Create<OuWarehouset>();

            predicate = predicate.AndIF(parm.OutorderID != null, it => it.OutorderID == parm.OutorderID);
            //predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugDeptCode), it => it.DrugDeptCode == parm.DrugDeptCode);
            predicate = predicate.AndIF(parm.OutBillCode != null, it => it.OutBillCode == parm.OutBillCode);

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.GroupCode), it => it.GroupCode == parm.GroupCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.DrugCode), it => it.DrugCode == parm.DrugCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.TradeName), it => it.TradeName == parm.TradeName);
            return predicate;
        }
    }
}