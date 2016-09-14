using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ForYou.translate
{
    public class file_translate
    {
        /// <summary>
        /// 创建HttpWebRequest
        /// </summary>
        /// <returns></returns>
        public static HttpWebRequest create_request(string servlet_path)
        {
            try
            {
                Uri url = new Uri(servlet_path);
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
                return webrequest;
            }catch
            {
                return null;
            }
        }
        /// <summary>
        /// 将string转为byte数组
        /// </summary>
        /// <param name="input">输入一个字符串</param>
        /// <returns></returns>
        public static byte[] utf8_tran(string input)
        {
            try
            {
                UTF8Encoding utf8 = new UTF8Encoding(false);
                byte[] input_byte = utf8.GetBytes(input);
                string sb_input = "";
                foreach (byte b in input_byte) sb_input = sb_input + (string.Format("%{0:X}", b));//中文需要再次进行编码
                byte[] send_buffer = utf8.GetBytes(sb_input);//将文件名转为utf-8格式进行传输
                return send_buffer;
            }catch
            {
                return null;
            }
        }

        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="filepath">待发送的文件的路径</param>
        /// <returns></returns>

        public static bool sendfile(string filepath)
        {
            FileStream fileStream = null;
            Stream requestStream = null;
            try
            {
                FileInfo fi = new FileInfo(filepath);

               //获取传送的文件名
                var send_buffer = utf8_tran(fi.Name);
                if (send_buffer == null)
                {
                    return false;
                }
                var webrequest = create_request(ForYou.translate.server_info.file_servlet);//将接收文件的servlet的url得到
                if (webrequest == null)
                {
                    return false;
                }
                webrequest.Method = "POST";
                fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                webrequest.ContentLength = (fileStream.Length + 4 + send_buffer.Length);//将文件大小，四个字节存放大小，文件名
                requestStream = webrequest.GetRequestStream();
                byte[] buffer = new Byte[fileStream.Length + 4 + send_buffer.Length];

                //将文件名大小放入网络流
                int i = 0;
                buffer[0] = (byte)(send_buffer.Length);
                buffer[1] = (byte)(send_buffer.Length >> 8);
                buffer[2] = (byte)(send_buffer.Length >> 16);
                buffer[3] = (byte)(send_buffer.Length >> 24);
                for (i = 0; i < send_buffer.Length; i++)
                {
                    buffer[i + 4] = send_buffer[i];
                }

                //将打包好的数据到服务器
                int bytesRead = 0;
                bytesRead = fileStream.Read(buffer, i + 4, (int)fileStream.Length);
                while (bytesRead > 0)
                {
                    requestStream.Write(buffer, 0, bytesRead + i + 4);
                    bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                }
                requestStream.Close();
                return true;
            }catch
            {
                return false;
            }finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
                if (requestStream!=null)
                {
                    requestStream.Close();
                }
            }
        }

        /// <summary>
        /// 发送json数据
        /// </summary>
        /// <param name="all_info">用于存储数据的类</param>
        /// <returns></returns>

        public static bool sendword(List<send_info> all_info)
        {
            Stream requestStream = null;
            WebResponse webresponse = null;
            StreamReader mystream = null;
            try
            {
                string jsonstring = json_translate.Serialize<send_info>(all_info);//取得序列化之后的字符串
                var send_buffer = utf8_tran(jsonstring);
                if (send_buffer == null)
                {
                    return false;
                }
                var webrequest = create_request(ForYou.translate.server_info.word_servlet);
                if (webrequest == null)
                {
                    return false;
                }
                webrequest.Method = "POST";
                webrequest.ContentType = "application/x-www-form-urlencoded";//发送表单 而text/html 发送流
                webrequest.ContentLength = send_buffer.Length;//必须设置这个长度
                requestStream = webrequest.GetRequestStream();
                requestStream.Write(send_buffer, 0, send_buffer.Length);
                requestStream.Close();
                webresponse = webrequest.GetResponse();
                mystream = new StreamReader(webresponse.GetResponseStream(), Encoding.UTF8);
                //textBox2.Text = HttpUtility.UrlDecode(mystream.ReadToEnd(), Encoding.UTF8);
                //解析服务器发回的数据添加system.web
                string json_str = HttpUtility.UrlDecode(mystream.ReadToEnd(), Encoding.UTF8);
                //解析json串
                List<send_info> get_infolist = json_translate.JSONStringToList<send_info>(json_str);
                //获取list中的所有值
                foreach (var get_info in get_infolist)
                {
                    //得到list中的所有值
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (requestStream != null)
                {
                    requestStream.Close();
                }
                if (webresponse!=null)
                {
                    webresponse.Close();
                }
                if (mystream!=null)
                {
                    mystream.Close();
                }
            }
        }

        /// <summary>
        /// 下载文件openorcreate
        /// </summary>
        /// <param name="store_path">下载之后，存储的路径</param>
        /// <param name="init_path">要下载的文件的url</param>
        /// <returns></returns>

        public static bool downloadfile(string store_path,string init_path)
        {
            Stream reader = null;
            FileStream stream = null;
            WebResponse response = null;
            try
            {
                var request = create_request(ForYou.translate.server_info.getfile_url+init_path);
                if (request == null)
                {
                    return false;
                }
                response = request.GetResponse();
                reader = response.GetResponseStream();
                stream = File.Open(store_path, FileMode.OpenOrCreate, FileAccess.Write);
                byte[] buf = new byte[512];
                int count = 0;
                while ((count = reader.Read(buf, 0, buf.Length)) > 0)
                {
                    stream.Write(buf, 0, count);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
        }
    }
}
