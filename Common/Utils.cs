using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public static class Utils
    { /// <summary>
      /// 生成随机字符
      /// </summary>
      /// <returns></returns>
        public static string GetRandom(int length)
        {
            string s = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_"; //自己定义符号
            string r = string.Empty;
            Random random = new Random();
            Enumerable.Repeat<int>(0, length).ToList().ForEach(x => r += s[random.Next(s.Length)]);
            return r;
        }

        /// <summary>
        /// 随机生成字符串数字
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomNum(int length)
        {
            var num = string.Empty;
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                num += rd.Next(0, 10).ToString();
            }
            return num;
        }
    }
}
