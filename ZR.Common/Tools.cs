using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Ocsp;
using Snowflake.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using Topsdk.Top;
using Topsdk.Top.Ability2940;
using Topsdk.Top.Ability2940.Request;
using Topsdk.Top.Ability2940.Response;

namespace ZR.Common
{
    public class Tools
    {

        public readonly static string APPKEY = "34712610";
        public readonly static string APPSECRECT = "d6e1a9eff5cb51c0a2c0044c6a3da96d";
        public readonly static string URL = "http://gw.api.taobao.com/router/rest";

        public static string Ref_id = "";
        public static ITopApiClient client = new DefaultTopApiClient(APPKEY, APPSECRECT, URL, 10000, 20000);
        public static Ability2940 apiPackage = new Ability2940(client);

        /// <summary>
        /// 要分割的字符串 eg: 1,3,10,00
        /// </summary>
        /// <param name="str"></param>
        /// <param name="split">分割的字符串</param>
        /// <returns></returns>
        public static long[] SpitLongArrary(string str, char split = ',')
        {
            if (string.IsNullOrEmpty(str)) { return Array.Empty<long>(); }
            str = str.TrimStart(split).TrimEnd(split);
            string[] strIds = str.Split(split, (char)StringSplitOptions.RemoveEmptyEntries);
            long[] infoIdss = Array.ConvertAll(strIds, s => long.Parse(s));
            return infoIdss;
        }

        public static int[] SpitIntArrary(string str, char split = ',')
        {
            if (string.IsNullOrEmpty(str)) { return Array.Empty<int>(); }
            string[] strIds = str.Split(split, (char)StringSplitOptions.RemoveEmptyEntries);
            int[] infoIdss = Array.ConvertAll(strIds, s => int.Parse(s));
            return infoIdss;
        }
        public static T[] SplitAndConvert<T>(string input, char split = ',')
        {
            if (string.IsNullOrEmpty(input)) { return Array.Empty<T>(); }
            string[] parts = input.Split(split, (char)StringSplitOptions.RemoveEmptyEntries);
            T[] result =  Array.ConvertAll(parts, s => (T)Convert.ChangeType(s, typeof(T)));
            //for (int i = 0; i < parts.Length; i++)
            //{
            //    result[i] = (T)Convert.ChangeType(parts[i], typeof(T));
            //}

            return result;
        }

        /// <summary>
        /// 根据日期获取星期几
        /// </summary>
        public static string GetWeekByDate(DateTime dt)
        {
            var day = new[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            return day[Convert.ToInt32(dt.DayOfWeek.ToString("d"))];
        }

        /// <summary>
        /// 得到这个月的第几周
        /// </summary>
        /// <param name="daytime">年月日</param>
        /// <returns>传递过来的时间是第几周</returns>
        public static int GetWeekNumInMonth(DateTime daytime)
        {
            int dayInMonth = daytime.Day;
            //本月第一天
            DateTime firstDay = daytime.AddDays(1 - daytime.Day);
            //本月第一天是周几
            int weekday = (int)firstDay.DayOfWeek == 0 ? 7 : (int)firstDay.DayOfWeek;
            //本月第一周有几天
            int firstWeekEndDay = 7 - (weekday - 1);
            //当前日期和第一周之差
            int diffday = dayInMonth - firstWeekEndDay;
            diffday = diffday > 0 ? diffday : 1;
            //当前是第几周,如果整除7就减一天
            int weekNumInMonth = ((diffday % 7) == 0
                ? (diffday / 7 - 1)
                : (diffday / 7)) + 1 + (dayInMonth > firstWeekEndDay ? 1 : 0);
            return weekNumInMonth;
        }
        /// <summary>
        /// 判断一个字符串是否为url
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsUrl(string str)
        {
            try
            {
                string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
                return Regex.IsMatch(str, Url);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public static bool CheckUserName(string str)
        {
            return str!=null;
            //try
            //{
            //    string rg = @"^[a-z][a-z0-9-_]*$";
            //    return Regex.IsMatch(str, rg);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return false;
            //}
        }

        /// <summary>
        /// 计算密码强度
        /// </summary>
        /// <param name="password">密码字符串</param>
        /// <returns></returns>
        public static bool PasswordStrength(string password)
        {
            //空字符串强度值为0
            if (string.IsNullOrEmpty(password)) return false;

            //字符统计
            int iNum = 0, iLtt = 0, iSym = 0;
            foreach (char c in password)
            {
                if (c >= '0' && c <= '9') iNum++;
                else if (c >= 'a' && c <= 'z') iLtt++;
                else if (c >= 'A' && c <= 'Z') iLtt++;
                else iSym++;
            }

            if (iLtt == 0 && iSym == 0) return false; //纯数字密码
            if (iNum == 0 && iLtt == 0) return false; //纯符号密码
            if (iNum == 0 && iSym == 0) return false; //纯字母密码

            if (password.Length >= 6 && password.Length < 16) return true;//长度不大于6的密码

            if (iLtt == 0) return true; //数字和符号构成的密码
            if (iSym == 0) return true; //数字和字母构成的密码
            if (iNum == 0) return true; //字母和符号构成的密码

            return true; //由数字、字母、符号构成的密码
        }
        ///<summary>
        ///生成随机字符串 
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
        public static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            System.Security.Cryptography.RandomNumberGenerator.Create().GetBytes(b);
            Random r = new(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }

        public static object Getentinfo(string rennanem = "")
        {
            //ITopApiClient client = new DefaultTopApiClient(APPKEY, APPSECRECT, URL, 10000, 20000);
            //Ability2940 apiPackage = new Ability2940(client);
            var request = new AlibabaAlihealthDrugtraceTopYljgQueryGetentinfoRequest();
            //-- 佛山市南海区第五人民医院(佛山市南海区大沥医院)
            request.EntName = rennanem;
            var response = apiPackage.AlibabaAlihealthDrugtraceTopYljgQueryGetentinfo(request);
            if (response.isSuccess())
            {
                return response.Result.Model.RefEntId;
            }
            else
            {
                return JObject.Parse(response.SubCode).ToString();

            }
        }
        /// <summary>
        /// 获取码信息
        /// </summary>
        /// <param name="codes">多个码用逗号拼接的字符串</param>
        /// <returns></returns>
        public static AlibabaAlihealthDrugtraceTopYljgQueryCodedetailResponse codedetail(string codes = "")
        {
            //ITopApiClient client = new DefaultTopApiClient(APPKEY, APPSECRECT, URL, 10000, 20000);
            //Ability2940 apiPackage = new Ability2940(client);
            string[] numbers = codes.Split(',');
            List<string> code = new List<string>();
            // 将分隔后的每个字符串添加到列表中
            foreach (string number in numbers)
            {
                code.Add(number);
            }
            //"ent_id": "00000000000017495183",
            var request = new AlibabaAlihealthDrugtraceTopYljgQueryCodedetailRequest();
            request.RefEntId = (string?)Getentinfo("佛山市南海区第五人民医院(佛山市南海区大沥医院)");
            Ref_id = request.RefEntId;
            //81797370314342290453
            request.Codes = new List<string>();
            request.Codes = code;
            var response = apiPackage.AlibabaAlihealthDrugtraceTopYljgQueryCodedetail(request);
            return response;
        }

        public static List<Dictionary<string, object>> CodeInOneWay(string codes)
        {
            List<string> code = codes.Split(',').ToList();
            //"ent_id": "00000000000017495183",
            var request = new AlibabaAlihealthDrugtraceTopYljgQueryCodedetailRequest();
            request.RefEntId = (string?)Getentinfo("佛山市南海区第五人民医院(佛山市南海区大沥医院)");
            request.Codes = code;
            var response = apiPackage.AlibabaAlihealthDrugtraceTopYljgQueryCodedetail(request);
            var totalResponse = GetSubCodesInfoByCode(response);
            return totalResponse;
        }


        private static List<Dictionary<string, object>> GetSubCodesInfoByCode(AlibabaAlihealthDrugtraceTopYljgQueryCodedetailResponse response)
        {
            var isSuccess = response.Result.ResponseSuccess;
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            if (isSuccess.GetValueOrDefault())
            {
                var models = response.Result.Models;
                foreach (var item in models)
                {
                    Dictionary<string, object> itemDict = new Dictionary<string, object>();
                    var currentProductInfo = item.CodeProduceInfoDTO.ProduceInfoList[0];
                    //溯源码（父玛）
                    itemDict.Add("code", item.Code);
                    //数量
                    itemDict.Add("pkg_amount", currentProductInfo.PkgAmount);
                    //批号
                    itemDict.Add("batch_no", currentProductInfo.BatchNo);
                    //有效到期
                    itemDict.Add("exipre_date", currentProductInfo.ExpireDate);
                    //生产日期
                    itemDict.Add("produce_date", currentProductInfo.ProduceDateStr);
                    //有效到
                    itemDict.Add("expire", item.DrugEntBaseDTO.Exprie);
                    //许可证号
                    itemDict.Add("license_no", item.DrugEntBaseDTO.ApprovalLicenceNo);
                    //药品ID
                    itemDict.Add("drug_ent_base_id", item.DrugEntBaseDTO.DrugEntBaseInfoId);
                    //企业名称
                    itemDict.Add("ent_name", item.PUserEntDTO.EntName);
                    //企业名称
                    itemDict.Add("ref_ent_id", item.PUserEntDTO.RefEntId);
                    //药品名称
                    itemDict.Add("physic_name", item.DrugEntBaseDTO.PhysicName);
                    //药品类别
                    itemDict.Add("physic_type_desc", item.DrugEntBaseDTO.PhysicTypeDesc);
                    //药品类别
                    itemDict.Add("pkg_spec_crit", item.DrugEntBaseDTO.PkgSpecCrit);
                    //药品剂型
                    itemDict.Add("prepn_spec", item.DrugEntBaseDTO.PrepnSpec);
                    //药品剂型描述
                    itemDict.Add("prepn_type_spec", item.DrugEntBaseDTO.PrepnTypeDesc);

                    //中码或者大码
                    if (item.PackageLevel.Equals("2") || item.PackageLevel.Equals("3"))
                    {
                        AlibabaAlihealthDrugtraceTopYljgQueryRelationResponse relationRes = relation(item.Code);
                        if (relationRes != null)
                        {
                            var codeRelationList = relationRes.Result.ModelList[0].CodeRelationList;
                            var formatRelationList = codeRelationList.Where(x => x.CodePackLevel == "1").ToList();
                            itemDict.Add("sub_code", formatRelationList);
                        }
                        else if (relationRes == null)
                        {
                            var dict = AddOutBill(item.Code);
                            var msg = dict.GetValueOrDefault("msg").ToString();
                            if (msg == "调用成功")
                            {
                                return GetSubCodesInfoByCode(response);
                            }
                        }
                    }
                    list.Add(itemDict);
                }
            }
            return list;
        }
       
        public static AlibabaAlihealthDrugtraceTopYljgQueryRelationResponse relation(string code)
        {
            Ref_id = (string?)Getentinfo("佛山市南海区第五人民医院(佛山市南海区大沥医院)");

            var request = new AlibabaAlihealthDrugtraceTopYljgQueryRelationRequest();
            request.RefEntId = Ref_id;
            request.Code = code;
            request.DesRefEntId = Ref_id;

            var response = apiPackage.AlibabaAlihealthDrugtraceTopYljgQueryRelation(request);

            if (response.isSuccess())
            {
                return response;
            }
            else
            {
                return response;
            }
        }


        public class SourceTracing
        {
            public int Id { get; set; }
            public string BillCode { get; set; }
            public string Codes { get; set; }

        }

        public static long GenerateSnowCode()
        {
            var worker = new IdWorker(2, 8);
            long id = worker.NextId();
            return id;
        }

        public static Dictionary<string, object> AddOutBill(string codes)
        {
            Ref_id = (string?)Getentinfo("佛山市南海区第五人民医院(佛山市南海区大沥医院)");
            SourceTracing sourceTracing = new SourceTracing();
            sourceTracing.Codes = codes;
            //sourceTracing.BillCode =;
            var request = new AlibabaAlihealthDrugtraceTopYljgUploadinoutbillRequest();
            //"CGRK"+
            long id = GenerateSnowCode();
            string uniqueBillCode = $"BC{id}";
            //创建 
            SourceTracing parm = new SourceTracing();
            parm.Codes = codes;
            parm.BillCode = uniqueBillCode;
            //var modal = parm.Adapt<SourceTracing>().ToCreate(HttpContext);
            //var tr = _SourceTracingService.AddSourceTracing(modal);
            request.BillCode = uniqueBillCode;
            request.BillTime = DateTime.Now;
            request.BillType = 102;
            request.PhysicType = 3;
            request.RefUserId = Ref_id;
            //发货企业
            request.FromUserId = Ref_id;
            //收货企业
            request.ToUserId = Ref_id;
            var cd = new List<string>();
            request.TraceCodes = cd;
            cd.Add(codes);
            request.ClientType = "2";

            var dict = new Dictionary<string, object>();
            var response = apiPackage.AlibabaAlihealthDrugtraceTopYljgUploadinoutbill(request);
            if (response.isSuccess())
            {
                dict.Add("msg", response.MsgInfo);
            }
            else
            {
                dict.Add("msg", response.MsgInfo);
            }
            dict.Add("bill_code", uniqueBillCode);
            return dict;
        }


    }
}
