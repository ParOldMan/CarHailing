using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace CarHailing.Base
{
    /// <summary>
    /// 数据帮助类
    /// </summary>
    public class DataHelper
    {
        #region 把对象转换为Json格式
        static readonly JavaScriptSerializer JsonOp = new JavaScriptSerializer();
        /// <summary>
        /// 把对象转换为Json格式
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">参数</param>
        /// <returns>加密后字符串</returns>
        public static string ObjToJson<T>(T obj)
        {
            return JsonOp.Serialize(obj);
        }
        #endregion

        #region 把Json转换为对象
        /// <summary>
        /// 把Json转换为对象
        /// </summary>
        /// <typeparam name="T">返回对象类型</typeparam>
        /// <param name="str">解密字符串</param>
        /// <returns>解密结果对象</returns>
        public static T JsonToObj<T>(string str)
        {
            return JsonOp.Deserialize<T>(str);
        }
        #endregion


        #region 比较普通对象内全部属性，并把修改过的属性的属性值和原属性值存入操作日志表

        /// <summary>
        /// 比较普通对象内全部属性，并把修改过的属性的属性值和原属性值存入操作日志表
        /// 注：现存入操作日志对象的值 为 oldValue | newValue ， 如果T1 T2对象位置互换，则为 newValue | oldValue
        /// </summary>
        /// <typeparam name="T">普通对象(eg:Line)</typeparam>
        /// <typeparam name="C">普通对象对应的操作日志对象(eg:LineDoLog)</typeparam>
        /// <param name="t1">old普通对象</param>
        /// <param name="t2">修改后的普通对象</param>
        /// <param name="c1">操作日志对象</param>
        /// <returns></returns>
        public static C CompareAndAddLog<T, C>(T t1, T t2, C c1)
            where T : class, new()
            where C : class, new()
        {
            try
            {
                System.Reflection.PropertyInfo[] mPi = typeof(T).GetProperties();
                System.Reflection.PropertyInfo[] logMpi = typeof(C).GetProperties();


                foreach (var pi in mPi)
                {
                    //比较,只比较值类型
                    if ((pi.PropertyType.IsValueType || pi.PropertyType.Name.StartsWith("String")))
                    {
                        string oldValue = pi.GetValue(t1, null).ToString();
                        string newValue = pi.GetValue(t2, null).ToString();
                        string oldName = pi.Name;
                        if (!oldValue.Equals(newValue))
                        {
                            string s = oldValue + "|" + newValue;
                            foreach (var p in logMpi)
                            {
                                if ((p.PropertyType.IsValueType || p.PropertyType.Name.StartsWith("String")))
                                {
                                    string logDoName = p.Name;
                                    if (logDoName.Equals(oldName))
                                    {
                                        p.SetValue(c1, s);
                                        //p.SetValue(c1, Convert.ChangeType(s, p.PropertyType), null);
                                        break;
                                    }
                                }

                            }
                        }
                        else
                        {
                            foreach (var p in logMpi)
                            {
                                if ((p.PropertyType.IsValueType || p.PropertyType.Name.StartsWith("String")))
                                {
                                    string logDoName = p.Name;
                                    if (logDoName.Equals(oldName))
                                    {
                                        p.SetValue(c1, "");
                                        //p.SetValue(c1, Convert.ChangeType("", p.PropertyType), null);
                                        break;
                                    }
                                }

                            }
                        }
                    }

                }
                return c1;
            }
            catch (Exception ex)
            {
                throw ex;
                //return null;
            }
        }
        //for (int i = 0; i < mPi.Length; i++)
        //{
        //    System.Reflection.PropertyInfo pi = mPi[i];

        //    string oldValue = pi.GetValue(t1, null).ToString();
        //    string newValue = pi.GetValue(t2, null).ToString();
        //    string oldName = pi.Name;
        //    if (!oldValue.Equals(newValue))
        //    {
        //        string s = oldValue + "|" + newValue;
        //        //pi.SetValue(emptyLine, s);
        //        for (int n = 0; n < logMpi.Length; n++)
        //        {
        //            System.Reflection.PropertyInfo p = logMpi[n];
        //            string logDoName = p.Name;
        //            if (logDoName.Equals(oldName))
        //            {
        //                //p.SetValue(emptyLineDoLog, s);
        //                p.SetValue(c1, Convert.ChangeType(s, p.PropertyType), null);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        for (int n = 0; n < logMpi.Length; n++)
        //        {
        //            System.Reflection.PropertyInfo p = logMpi[n];
        //            string logDoName = p.Name;
        //            if (logDoName.Equals(oldName))
        //            {
        //                //p.SetValue(emptyLineDoLog, oldValue);
        //                p.SetValue(c1, Convert.ChangeType("", p.PropertyType), null);
        //            }
        //        }
        //    }
        //}
        #endregion


        #region 对象间赋值

        /// <summary>
        /// 对象间赋值   
        /// </summary>
        /// 存在问题：部分类型不能进行强制转换、名称必须一致且所有子集合间及父集合间名称不能重复
        /// <typeparam name="T">传入对象</typeparam>
        /// <typeparam name="L">输出对象</typeparam>
        /// <param name="t">传入数据</param>
        /// <returns></returns>
        public static L Mapper<T, L>(T t) where L : new()
        {
            if (t == null)
            {
                return default(L);
            }
            System.Reflection.PropertyInfo[] propertiesT = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            System.Reflection.PropertyInfo[] propertiesL = typeof(L).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            L setT = new L();
            foreach (System.Reflection.PropertyInfo itemL in propertiesL)
            {
                foreach (System.Reflection.PropertyInfo itemT in propertiesT)
                {
                    if (itemL.Name == itemT.Name)
                    {
                        if (itemT.PropertyType.Name == "Guid")
                        {
                            object values = itemT.GetValue(t, null).ToString();
                            itemL.SetValue(setT, values, null);
                        }
                        else
                        {
                            object value = itemT.GetValue(t, null);
                            itemL.SetValue(setT, value, null);
                        }
                    }
                }
            }
            return setT;
        }
        #endregion

        #region 获取随机字符串（数字+小写字母+大写字母）

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="strLength">字符串长度</param>
        /// <param name="Seed">随机函数种子值</param>
        /// <returns>指定长度的随机字符串</returns>
        public static string GetRandomString(int strLength, params int[] Seed)
        {
            string strSep = ",";
            char[] chrSep = strSep.ToCharArray();
            string strChar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z"
             + ",A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] aryChar = strChar.Split(chrSep, strChar.Length);
            string strRandom = string.Empty;
            Random Rnd;
            if (Seed != null && Seed.Length > 0)
            {
                Rnd = new Random(Seed[0]);
            }
            else
            {
                Rnd = new Random();
            }
            //生成随机字符串
            for (int i = 0; i < strLength; i++)
            {
                strRandom += aryChar[Rnd.Next(aryChar.Length)];
            }
            return strRandom;
        }

        #endregion


    }
}

