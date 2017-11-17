using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarHailing.Base
{
    public class CompareAndAddLogHelper
    {
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
            System.Reflection.PropertyInfo[] mPi = typeof(T).GetProperties();
            System.Reflection.PropertyInfo[] logMpi = typeof(C).GetProperties();


            for (int i = 0; i < mPi.Length; i++)
            {
                System.Reflection.PropertyInfo pi = mPi[i];

                string oldValue = pi.GetValue(t1, null).ToString();
                string newValue = pi.GetValue(t2, null).ToString();
                string oldName = pi.Name;
                if (oldValue != newValue)
                {
                    string s = oldValue + "|" + newValue;
                    //pi.SetValue(emptyLine, s);
                    for (int n = 0; n < logMpi.Length; n++)
                    {
                        System.Reflection.PropertyInfo p = logMpi[n];
                        string logDoName = p.Name;
                        if (logDoName == oldName)
                        {
                            //p.SetValue(emptyLineDoLog, s);
                            p.SetValue(c1, Convert.ChangeType(s, p.PropertyType), null);
                        }
                    }
                }
                else
                {
                    for (int n = 0; n < logMpi.Length; n++)
                    {
                        System.Reflection.PropertyInfo p = logMpi[n];
                        string logDoName = p.Name;
                        if (logDoName == oldName)
                        {
                            //p.SetValue(emptyLineDoLog, oldValue);
                            p.SetValue(c1, Convert.ChangeType("", p.PropertyType), null);
                        }
                    }
                }
            }
            return c1;
        }
    }
}
