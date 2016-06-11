using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GodServer.Tools
{
    class Md5Tool
    {
        public static string GetMd5(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] md5Bytes = md5.ComputeHash(dataBytes);
            return BitConverter.ToString(md5Bytes).Replace("-", "");  //将-转换成空字符           
        }
    }
}
