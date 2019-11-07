using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Info.Common
{
    public static class Utils
    {  /// <summary>
       /// 获取时间毫秒数
       /// 提取为通用方法
       /// </summary>
       /// <returns></returns>
        public static long GetTimeStamp()
        {
            //return (long)(DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds);
            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(DateTime.Now.AddHours(-8) - Jan1st1970).TotalMilliseconds;

        }
        /// <summary>
        /// 时间戳转换为日期（时间戳单位秒）
        /// </summary>
        /// <param name="TimeStamp"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(long timeStamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return start.AddMilliseconds(timeStamp).AddHours(8);
        }

    }
}
