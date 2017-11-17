using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarHailing.Base
{
    public class MessageHelper
    {
        /// <summary>
        /// 成功
        /// </summary>
        public const string msgTrue = "成功！";

        /// <summary>
        /// 失败
        /// </summary>
        public const string msgFalse = "失败！";

        /// <summary>
        /// 系统错误
        /// </summary>
        public const string msgErrs = "系统错误！";

        /// <summary>
        /// 无数据
        /// </summary>
        public const string msgNull = "无数据！";

        /// <summary>
        /// 无乘客信息
        /// </summary>
        public const string msgPasserNull = "订单提交失败，无乘客信息！";

        /// <summary>
        /// 无乘客信息
        /// </summary>
        public const string msgPasserQs = "订单提交失败，乘客信息有误！";

        /// <summary>
        /// 余票不足
        /// </summary>
        public const string msgSeatNull = "订单提交失败，可用余票不足！";

        /// <summary>
        /// 无订单信息
        /// </summary>
        public const string msgOrderNull = "订单有误！";

        /// <summary>
        /// 已发车
        /// </summary>
        public const string msgIsLeave = "该班次已发车！";
    }
}
