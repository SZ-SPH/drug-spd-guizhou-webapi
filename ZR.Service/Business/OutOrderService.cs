using Infrastructure.Attribute;
using Infrastructure.Extensions;
using ZR.Model.Business.Dto;
using ZR.Model.Business;
using ZR.Repository;
using ZR.Service.Business.IBusinessService;
using ZR.Model.GuiHis.Dto;
using ZR.Model.GuiHis;
using static System.Net.WebRequestMethods;
using ZR.Model;

namespace ZR.Service.Business
{
    /// <summary>
    /// 出库单Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IOutOrderService), ServiceLifetime = LifeTime.Transient)]
    public class OutOrderService : BaseService<OutOrder>, IOutOrderService
    {
        /// <summary>
        /// 查询出库单列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<OutOrderDto> GetList(OutOrderQueryDto parm)
        {
            Expressionable<OutOrder> predicate = null ;
            PagedInfo<OutOrderDto> response =null;
            if (parm.OutBillCode == 100)
            {
                 predicate = QueryExp(parm);
                 response = Queryable()
               .LeftJoin<Departments>((it, d1) => it.InpharmacyId == d1.DeptCode)
               .LeftJoin<Departments>((it, d1, d2) => it.OutWarehouseID == d2.DeptCode).Where(predicate.ToExpression())
                .OrderByDescending((it) => it.CreateTime)
               .Select((it, d1, d2) => new OutOrderDto
               {
                   Id = it.Id.SelectAll(),
                   InpharmacyName = d1.DeptName,
                   OutWarehouseName = d2.DeptName,
               })
               .ToPage(parm);
            }
            else
            {
               predicate = QueryExp(parm);
               response = Queryable()
              .LeftJoin<CompanyInfo>((it, d1) => it.InpharmacyId == d1.FacCode)
              .LeftJoin<Departments>((it, d1, d2) => it.OutWarehouseID == d2.DeptCode).Where(predicate.ToExpression())
              .Where((it, d1, d2)=> string.IsNullOrEmpty(parm.compname)||d1.FacName.Contains(parm.compname.Trim()))
              .Where((it, d1, d2) => string.IsNullOrEmpty(parm.deptname) || d2.DeptName.Contains(parm.deptname.Trim()))
              .OrderByDescending((it) => it.CreateTime)
              .Select((it, d1, d2) => new OutOrderDto
              {
                  Id = it.Id.SelectAll(),
                  InpharmacyName = d1.FacName,
                  OutWarehouseName = d2.DeptName,
              })
              .ToPage(parm);

            }
          
            return response;

        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public OutOrder GetInfo(int Id)
        {
            var response = Queryable()
                .Where(x => x.Id == Id)
                .First();

            return response;
        }
        public OutOrderDto GetInfos(int Id)
        {
            var response = Queryable()
           .LeftJoin<Departments>((p, d1) => p.InpharmacyId == d1.DeptCode)
           .LeftJoin<Departments>((p, d1, d2) => p.OutWarehouseID == d2.DeptCode)
           .Select((p, d1, d2) => new OutOrderDto
           {
               Id = p.Id.SelectAll(),
               InpharmacyName = d1.DeptName,
               OutWarehouseName = d2.DeptName,
           }).Where(p => p.Id == Id)
                .First();

            return response;
        }

        public OutOrderDto GetInfoss(int Id)
        {
            var response = Queryable()
           .LeftJoin<CompanyInfo>((p, d1) => p.InpharmacyId == d1.FacCode)
           .LeftJoin<Departments>((p, d1, d2) => p.OutWarehouseID == d2.DeptCode)
           .Select((p, d1, d2) => new OutOrderDto
           {
               Id = p.Id.SelectAll(),
               InpharmacyName = d1.FacName,
               OutWarehouseName = d2.DeptName,
           }).Where(p => p.Id == Id)
                .First();

            return response;
        }
        /// <summary>
        /// 添加出库单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OutOrder AddOutOrder(OutOrder model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改出库单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateOutOrder(OutOrder model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 清空出库单
        /// </summary>
        /// <returns></returns>
        public bool TruncateOutOrder()
        {
            var newTableName = $"OutOrder_{DateTime.Now:yyyyMMdd}";
            if (Queryable().Any() && !Context.DbMaintenance.IsAnyTable(newTableName))
            {
                Context.DbMaintenance.BackupTable("OutOrder", newTableName);
            }
            
            return Truncate();
        }
        /// <summary>
        /// 导入出库单
        /// </summary>
        /// <returns></returns>
        public (string, object, object) ImportOutOrder(List<OutOrder> list)
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
        /// 导出出库单
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<OutOrderDto> ExportList(OutOrderQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .Select((it) => new OutOrderDto()
                {
                }, true)
                .ToPage(parm);

            return response;
        }
        public OutOrderDto EList(int id)
        {
            //var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(it => it.Id == id)
                .Select((it) => new OutOrderDto()
                {
                }, true);
                //.ToPage(parm);
            return (OutOrderDto)response;
        }
        /// <summary>
        /// 查询导出表达式
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private static Expressionable<OutOrder> QueryExp(OutOrderQueryDto parm)
        {
            var predicate = Expressionable.Create<OutOrder>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.OutOrderCode), it => it.OutOrderCode == parm.OutOrderCode);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.InpharmacyId), it => it.InpharmacyId == parm.InpharmacyId);
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.OutWarehouseID), it => it.OutWarehouseID == parm.OutWarehouseID);
            predicate = predicate.AndIF(parm.OutBillCode != null, it => it.OutBillCode == parm.OutBillCode);
            return predicate;
        }



        public class outList        {
            public string 名称 { get; set; }
            public string 类型值 { get; set; }
        }
        public List<outList> getintype() 
        {
            var outlist = new List<outList>
                    {
                    new outList { 名称 = "一般入库", 类型值 = "01" },
                    new outList { 名称 = "一般入库", 类型值 = "11" },
                    new outList { 名称 = "临时采购", 类型值 = "ls" },
                    new outList { 名称 = "外部入库申请", 类型值 = "11" },
                    new outList { 名称 = "外部入库申请", 类型值 = "12" },
                    new outList { 名称 = "内部入库申请", 类型值 = "02" },
                    new outList { 名称 = "内部入库申请", 类型值 = "13" },
                    new outList { 名称 = "核准入库", 类型值 = "05" },
                    new outList { 名称 = "入库核准", 类型值 = "16" },
                    new outList { 名称 = "内部入库退库申请", 类型值 = "03" },
                    new outList { 名称 = "内部入库退货申请", 类型值 = "18" },
                    new outList { 名称 = "采购退货", 类型值 = "06" },
                    new outList { 名称 = "入库退货", 类型值 = "19" },
                    new outList { 名称 = "发票补录", 类型值 = "04" },
                    new outList { 名称 = "发票入库", 类型值 = "1A" },
                    new outList { 名称 = "系统切换初始化", 类型值 = "12" },
                    new outList { 名称 = "自增加入库", 类型值 = "1C" },
                    new outList { 名称 = "科室还药", 类型值 = "20" },
                    new outList { 名称 = "特殊入库", 类型值 = "26" },
                    new outList { 名称 = "即入即出", 类型值 = "21" },
                    new outList { 名称 = "即入即出", 类型值 = "20" }
                    };
            return outlist;
        }

        public List<outList> getouttype()
        {
            var outlist = new List<outList>
                    {
                        //new outList { 名称 = "盘点(封帐,盘点)", 类型值 = "1" },
                   
                        new outList { 名称 = "科室借药", 类型值 = "20" },
                        new outList { 名称 = "特殊出库", 类型值 = "26" },
                        new outList { 名称 = "个人借药", 类型值 = "G1" },
                        new outList { 名称 = "协调用药", 类型值 = "K3" },
                        new outList { 名称 = "出库退库", 类型值 = "02" },
                        new outList { 名称 = "科室还药", 类型值 = "27" },
                        new outList { 名称 = "个人还药", 类型值 = "G2" },
                        new outList { 名称 = "出库审批", 类型值 = "04" },
                        new outList { 名称 = "出库审批", 类型值 = "25" },
                        new outList { 名称 = "报损", 类型值 = "05" },
                        new outList { 名称 = "特别出库审批", 类型值 = "11" },
                        new outList { 名称 = "特别出库核准", 类型值 = "12" },
                        new outList { 名称 = "门诊摆药", 类型值 = "M1" },
                        new outList { 名称 = "门诊退药", 类型值 = "M2" },
                        new outList { 名称 = "住院摆药", 类型值 = "Z1" },
                        new outList { 名称 = "住院退药", 类型值 = "Z2" },
                        new outList { 名称 = "自减少出库", 类型值 = "26" },
                        new outList { 名称 = "内部入库退货申请", 类型值 = "18" },
                        new outList { 名称 = "出库退库", 类型值 = "22" },
                        new outList { 名称 = "出库退货", 类型值 = "22" },
                        new outList { 名称 = "一般出库", 类型值 = "21" },
                        new outList { 名称 = "一般出库", 类型值 = "01" },
                        new outList { 名称 = "出库审批", 类型值 = "25" },
                        new outList { 名称 = "特别出库", 类型值 = "33" }
                    };
            return outlist;
        }


        public  string Byin(string zhi)
        {
            var res = getintype().Find(x => x.类型值 == zhi);
            if (res != null)
            {
                return res.名称;
            }
            else
            {
                return "";
            }
        }
        public string Byout(string zhi)
        {
            var res = getouttype().Find(x => x.类型值 == zhi);
            if (res != null)
            {
                return res.名称;
            }
            else
            {
                return "";
            }
        }
    }
}