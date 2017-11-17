using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CarHailing.Base
{
    public class WriteStreamHelper
    {
        string file = "D:\\pay\\result.txt";
        string files = "D:\\pay\\err.txt";

        /// <summary>
        /// 字符串写到文件
        /// </summary>
        /// <param name="file">文件目录</param>
        /// <param name="data">数据</param>
        public void WriteStream(string file, string data)
        {
            //Utils.writeFile("接口回调", resParam); //通知返回参数写入result.txt文本文件。
            //HttpRequest request = new HttpRequest("", "", "");
            //request = HttpContext.Current.Request;
            //data = request.InputStream.ToString();// Request.InputStream;
            //FileStream fileStream = new FileStream(Environment.CurrentDirectory + "\\result.txt", FileMode.Append);
            FileStream fileStream = new FileStream(file, FileMode.Append);
            StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Default);
            streamWriter.Write(data + "\r\n");
            streamWriter.Flush();
            streamWriter.Close();
            fileStream.Close();
        }
    }
}
