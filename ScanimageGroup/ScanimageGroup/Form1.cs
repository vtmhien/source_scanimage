using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xNet;
using System.Threading;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ScanimageGroup
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        private void ReadAllFile()
        {
            string readfile = File.ReadAllText(Application.StartupPath + "\\abc.txt");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                {
                    MAIN();
                }

            }).Start();
        }
        private void MAIN()
        {
            Req http = new Req(textBox1.Text);
            string getlinktoken = http.RequestGet("https://m.facebook.com/composer/ocelot/async_loader/?publisher=feed").ToString(); ;
            string fixlinktoken = getlinktoken.Replace(@"\", "").ToString();
            string token = Regex.Match(fixlinktoken, "accessToken\":\"(.*?)\"").Groups[1].ToString();
            APIGETIMAGE(token);
            //   GetLinkVideo();
        }
        public System.Drawing.Image DownloadImageFromUrl(string imageUrl)
        {
            System.Drawing.Image image = null;

            try
            {
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;

                System.Net.WebResponse webResponse = webRequest.GetResponse();

                System.IO.Stream stream = webResponse.GetResponseStream();

                image = System.Drawing.Image.FromStream(stream);

                webResponse.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            return image;
        }
        private void GetLinkGroup()
        {
            xNet.HttpRequest http = new xNet.HttpRequest();
            string getapi = http.Get("https://graph.facebook.com/j2team.community?fields=posts&access_token=EAAAAZAw4FxQIBAE3jKY8npq2tCIFDpXk3Gs5nGU73YsN8tDtpBofM7n6gPRNddRAPOZCuql9AViRpbUAsTJ5TARvEWyLNRzenTUlAJiP3XZASeaAg6nOq0Hhf8VIS9UaWaLGrbc2Tt5uuiGSxrup6c8Ec8yKQZANqwVfIgdINFapCcQN3jAGNnFoOCC8VhwZD&limit=1000000").ToString();
            int CountImage = JObject.Parse(getapi)["posts"]["data"].Count();
            int urllink;
            string urlget = "";
            string get = "";
            for (int i = 0; i < CountImage; i++)
            {
                try
                {
                    urllink = JObject.Parse(getapi)["posts"]["data"][i].Count();
                    urlget = JObject.Parse(getapi)["posts"]["data"][i].ToString();
                    for (int x = 0; x < urllink; x++)
                    {
                        get = JObject.Parse(urlget)["actions"][x]["link"].ToString();
                        //  listBox1.Items.Add(get);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        private void APIGETIMAGE(string token)
        {
            xNet.HttpRequest http = new xNet.HttpRequest();
            string id = txtid.Text;
            string filepath = Application.StartupPath + "\\data";
            string getapi = http.Get("https://graph.facebook.com/" + id + "?fields=feed.limit(" + numericUpDown1.Value + ")%7Bfull_picture%7D&access_token=" + token).ToString();
            string nextPage = "";
            int CountImage = 0;
            int tenanh = 0;
            for (int z = 1; z <= numericUpDown2.Value; z++)
            {
                int i = 0;
                if (z > 1)
                {
                    try
                    {
                        string[] catchuoi = nextPage.Split(Convert.ToChar('?'));
                        string chuoi1 = catchuoi[0];
                        string chuoi2 = catchuoi[1];
                        nextPage = chuoi1 + $"?limit={numericUpDown1.Value - 1}&" + chuoi2;
                    }
                    catch
                    {

                    }
                    getapi = http.Get(nextPage).ToString();
                }
                try
                {
                    CountImage = JObject.Parse(getapi)["feed"]["data"].Count();
                }
                catch
                {
                    CountImage = JObject.Parse(getapi)["data"].Count();
                }

                try
                {
                    nextPage = JObject.Parse(getapi)["paging"]["next"].ToString();
                }
                catch
                {
                    nextPage = JObject.Parse(getapi)["feed"]["paging"]["next"].ToString();
                }
                string urlimage = "";
                for (; i < CountImage; i++)
                {
                    try
                    {
                        try
                        {
                            urlimage = JObject.Parse(getapi)["feed"]["data"][i]["full_picture"].ToString();
                        }
                        catch
                        {
                            urlimage = JObject.Parse(getapi)["data"][i]["full_picture"].ToString();
                        }
                        System.Drawing.Image image = DownloadImageFromUrl(urlimage.Trim());
                        string fileName = System.IO.Path.Combine(filepath, tenanh++ + ".jpg");
                        image.Save(fileName);
                        //  listBox1.Items.Add(urlimage);
                    }
                    catch
                    {

                    }
                }
            }

        }
    }
}
