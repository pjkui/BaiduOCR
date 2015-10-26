using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Web;
using System.Windows.Forms;

namespace BaiduOCR
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {


        /// <summary>
        /// 发送HTTP请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="param">请求的参数</param>
        /// <returns>请求结果</returns>
        public static string request(string url, string param)
        {
            string strURL = url;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "POST";
            // 添加header
            request.Headers.Add("apikey", "b8bbb6b2b741371f0deab5f59901926d");
            request.ContentType = "application/x-www-form-urlencoded";
            string paraUrlCoded = param;
            byte[] payload;
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            request.ContentLength = payload.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(payload, 0, payload.Length);
            writer.Close();
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream s;
            s = response.GetResponseStream();
            string StrDate = "";
            string strValue = "";
            StreamReader Reader = new StreamReader(s, Encoding.UTF8);
            while ((StrDate = Reader.ReadLine()) != null)
            {
                strValue += StrDate + "\r\n";
            }
            return strValue;
        }


        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void btn_recognize_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "E://";
            openFileDialog.Filter = "图像文件|*.jpg";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            string img_path = @"E:\test.jpg";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                img_path = openFileDialog.FileName;
                
            }  
            
            var strBase64Pic = System.Web.HttpUtility.UrlEncode(Convert.ToBase64String(File.ReadAllBytes(img_path)));
            img_show.Source = new BitmapImage(new Uri(img_path, UriKind.Absolute));
            string url = "http://apis.baidu.com/apistore/idlocr/ocr";
            //strBase64Pic = "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";
            //string param = "fromdevice=pc&clientip=10.10.10.0&detecttype=LocateRecognize&languagetype=CHN_ENG&imagetype=1&image=/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDABMNDxEPDBMREBEWFRMXHTAfHRsbHTsqLSMwRj5KSUU+RENNV29eTVJpU0NEYYRiaXN3fX59S12Jkoh5kW96fXj/2wBDARUWFh0ZHTkfHzl4UERQeHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHj/wAARCAAfACEDAREAAhEBAxEB/8QAGAABAQEBAQAAAAAAAAAAAAAAAAQDBQb/xAAjEAACAgICAgEFAAAAAAAAAAABAgADBBESIRMxBSIyQXGB/8QAFAEBAAAAAAAAAAAAAAAAAAAAAP/EABQRAQAAAAAAAAAAAAAAAAAAAAD/2gAMAwEAAhEDEQA/APawEBAQEBAgy8i8ZTVV3UY6V1eU2XoWDDZB19S646Gz39w9fkKsW1r8Wm2yo1PYis1be0JG9H9QNYCAgc35Cl3yuVuJZl0cB41rZQa32dt2y6OuOiOxo61vsLcVblxaVyXD3hFFjL6La7I/sDWAgICAgICB/9k=";
            string param = "fromdevice=pc&clientip=183.192.95.68&detecttype=LocateRecognize&languagetype=CHN_ENG&imagetype=1&image=" + strBase64Pic;
            string result = request(url, param);
            //MessageBox.Show(result);
            JObject objResult = JObject.Parse(result);
            string errMsg = (string)objResult.GetValue("errMsg");
            if (errMsg == "success")
            {

                try
                {

                    object retData = objResult.GetValue("retData");
                    string data = objResult["retData"][0]["word"].ToString();

                    //return data format
                    //{
                    //    "errNum": "0",
                    //    "errMsg": "success",
                    //    "querySign": "3845925467,2370020290",
                    //    "retData": [
                    //        {
                    //            "rect": {
                    //                "left": "0",
                    //                "top": "0",
                    //                "width": "33",
                    //                "height": "31"
                    //            },
                    //            "word": "  8"
                    //        }
                    //    ]
                    //}
                    System.Windows.MessageBox.Show("识别结果为：" + data);
                    tb_result.Text = data;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("return data error!");
                    tb_result.Text = "";
                }
            }
            else
            {
                System.Windows.MessageBox.Show(errMsg);
                tb_result.Text = "";
            }
        }
        //    }
        //}
    }
}
