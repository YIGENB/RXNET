using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace RX.Web
{
    /// <summary>
    /// 文件下载有以下四种方式, 大文件下载的处理方法：将文件分块下载。
    /// Response.OutputStream.Write
    /// Response.TransmitFile
    /// Response.WriteFile
    /// Response.BinaryWrite
    /// </summary>
    public static class Download
    {
        static HttpResponse Response = HttpContext.Current.Response;

        public static void DownloadByOutputStreamBlock(this Stream stream, string fileName)
        {
            using (stream)
            {
                //将流的位置设置到开始位置。
                stream.Position = 0;
                //块大小
                long ChunkSize = 102400;
                //建立100k的缓冲区
                byte[] buffer = new byte[ChunkSize];
                //已读字节数
                long dataLengthToRead = stream.Length;

                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition",
                    string.Format("attachment; filename={0}", HttpUtility.UrlPathEncode(fileName)));

                while (dataLengthToRead > 0 && Response.IsClientConnected)
                {
                    int lengthRead = stream.Read(buffer, 0, Convert.ToInt32(ChunkSize));//读取的大小
                    Response.OutputStream.Write(buffer, 0, lengthRead);
                    Response.Flush();
                    Response.Clear();
                    dataLengthToRead -= lengthRead;
                }
                Response.Close();
            }
        }

        public static void DownloadByTransmitFile(this string filePath, string fielName)
        {
            FileInfo info = new FileInfo(filePath);
            long fileSize = info.Length;
            Response.Clear();
            Response.ContentType = "application/x-zip-compressed";
            Response.AddHeader("Content-Disposition",
                string.Format("attachment;filename={0}", HttpUtility.UrlPathEncode(fielName)));
            //不指明Content-Length用Flush的话不会显示下载进度  
            Response.AddHeader("Content-Length", fileSize.ToString());
            Response.TransmitFile(filePath, 0, fileSize);
            Response.Flush();
            Response.Close();
        }

        public static void DownloadByWriteFile(this string filePath, string fileName)
        {
            FileInfo info = new FileInfo(filePath);
            long fileSize = info.Length;
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition",
                string.Format("attachment;filename={0}", HttpUtility.UrlPathEncode(fileName)));

            //指定文件大小  
            Response.AddHeader("Content-Length", fileSize.ToString());
            Response.WriteFile(filePath, 0, fileSize);
            Response.Flush();
            Response.Close();
        }

        public static void DownloadByOutputStreamBlock(this string filePath, string fileName)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                //指定块大小  
                long chunkSize = 102400;
                //建立一个100K的缓冲区  
                byte[] buffer = new byte[chunkSize];
                //已读的字节数  
                long dataToRead = stream.Length;

                //添加Http头  
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition",
                    string.Format("attachment;filename={0}", HttpUtility.UrlPathEncode(fileName)));
                Response.AddHeader("Content-Length", dataToRead.ToString());

                while (dataToRead > 0 && Response.IsClientConnected)
                {
                    int length = stream.Read(buffer, 0, Convert.ToInt32(chunkSize));
                    Response.OutputStream.Write(buffer, 0, length);
                    Response.Flush();
                    Response.Clear();
                    dataToRead -= length;
                }
                Response.Close();
            }
        }

        public static void DownloadByBinary(this string filePath, string fileName)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                //指定块大小  
                long chunkSize = 102400;
                //建立一个100K的缓冲区  
                byte[] bytes = new byte[chunkSize];
                //已读的字节数  
                long dataToRead = stream.Length;

                //添加Http头  
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition",
                    string.Format("attachment;filename={0}", HttpUtility.UrlPathEncode(fileName)));

                Response.AddHeader("Content-Length", bytes.Length.ToString());
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.Close();
            }
        }

        public static void DownloadByBinaryBlock(this string filePath, string fileName)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                //指定块大小  
                long chunkSize = 102400;
                //建立一个100K的缓冲区  
                byte[] buffer = new byte[chunkSize];
                //已读的字节数  
                long dataToRead = stream.Length;

                //添加Http头  
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition",
                    string.Format("attachment;filename={0}", HttpUtility.UrlPathEncode(fileName)));
                Response.AddHeader("Content-Length", dataToRead.ToString());

                while (dataToRead > 0 && Response.IsClientConnected)
                {
                    int length = stream.Read(buffer, 0, Convert.ToInt32(chunkSize));
                    Response.BinaryWrite(buffer);
                    Response.Flush();
                    Response.Clear();

                    dataToRead -= length;
                }
                Response.Close();
            }
        }

        ///
        /// 下载文件
        ///
        /// 文件物理地址
        public static void DownloadFile(this string filename)
        {
            int intStart = filename.LastIndexOf("\\") + 1;
            string saveFileName = filename.Substring(intStart, filename.Length - intStart);

            System.IO.FileInfo fi = new System.IO.FileInfo(filename);
            string fileextname = fi.Extension;
            string DEFAULT_CONTENT_TYPE = "application/unknown";
            RegistryKey regkey, fileextkey;
            string filecontenttype;
            try
            {
                regkey = Registry.ClassesRoot;
                fileextkey = regkey.OpenSubKey(fileextname);
                filecontenttype = fileextkey.GetValue("Content Type", DEFAULT_CONTENT_TYPE).ToString();
            }
            catch
            {
                filecontenttype = DEFAULT_CONTENT_TYPE;
            }

            Response.Clear();
            Response.Charset = "utf-8";
            Response.Buffer = true;
            Response.ContentEncoding = System.Text.Encoding.UTF8;

            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(saveFileName, System.Text.Encoding.UTF8));
            Response.ContentType = filecontenttype;

            Response.WriteFile(filename);
            Response.Flush();
            Response.Close();

            Response.End();
        }
    }
}
