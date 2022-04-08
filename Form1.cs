using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using JsonModel;
using JYKJ.EasyHTTPClient;
using JYKJ.IniHelp;
using System.Diagnostics;
using System.IO;
using Xunit;
// 
//=====================================================================================
//
//      Filename:  自动输入程序   
//   Description:  实现程序自动化输入，绕过禁止粘贴的网页以及程序
//       Version:  0.0.3
//       Created:  2022/4/8 
//        Author:  慧宇
//     Copyright:  @慧宇
//
//=====================================================================================
//
namespace WindowsFormsApp1
{
    
    public partial class Form1 : Form
    {
        int time;
        String version = "0.0.3";
        //
        //
        //程序初始化
        //
        //
        public Form1()
        {
            InitializeComponent();
        }
        //
       //
       //自动书写代码
       //
       //
        private  void WriteCode()
        {
            String[] data = CodeText.Text.Split('\n');
            for (int n = 0; n < data.Length; n++)
            {
                
                SendKeys.SendWait(data[n].Replace("**##**!","{{}").Replace("*#*#&&&","{}}")); 
 
            }
            CodeText.Clear();
            CodeText.ReadOnly = false;
        }
        //
        //
        //读配置项
        //
        //
        private async void ReadConfig()
        {
            string _path = Directory.GetCurrentDirectory() + @"\config.ini";
            IniHelper ini = new IniHelper(_path);
            if (ini.ReadValue("configuration", "ifread") == "")
            {
                String data = await request();
                TestModel testModel = JsonConvert.DeserializeObject<TestModel>(data);
                MessageBox.Show(testModel.Updatainformation);
                ini.WriteValue("configuration", "ifread", "yes");
            }
        }
        //
        //
        //操作textbox内的文件
        //
        //
        private void Delatekongge()
        {
            if (CodeText.Text == "") return;
            String[] data = CodeText.Text.Split('\n');
            String tmp = null;
            foreach (String i in data)
            {
                if (i.Trim() != "")
                tmp = tmp + i.Trim().Replace("{","**##**!").Replace("}","*#*#&&&").Replace("(","{(}").Replace(")","{)}").Replace("+","{+}").Replace("%","{%}") + "\r\n";
            }
           CodeText.Text = tmp;
        }
        //
        //
        //时钟操作
        //
        //
        private void timer1_Tick(object sender, EventArgs e)//时钟操作
        {
            button1.Text = time + "秒后启动";

            if (time == 0)
            {
                Thread operate = new Thread(WriteCode);
                operate.Start();
                button1.Text = "开始执行";
                time = int.Parse(numericUpDown1.Value.ToString());
                timer1.Stop();
            }
            time--;
        }
        //
        //
        //请求更新信息
        //
        //
        private async Task<string> request()//请求网页获取json数据
        {
            HttpItem item = new HttpItem()
            {
                URL = "http://2140.fak588.cn/version.json",
                Method = System.Net.Http.HttpMethod.Get,
                Allowautoredirect = false,
                Encoding = Encoding.UTF8,
            };
            var result = await item.GetHtml();
            if (result.Html.ToString() == "发送请求时出错。") Process.GetCurrentProcess().Kill();
            return result.Html;
        }
        //
        //
        //开始操作按钮被单击
        //
        //
        private void button1_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value == 0) { MessageBox.Show("您未曾设定等待时间！！！"); return; }
            Delatekongge();
            timer1.Interval = 1000;
            time = int.Parse(numericUpDown1.Value.ToString());
            MessageBox.Show("现在你有" + time.ToString() + "秒的时间修改您的输入法。");
            CodeText.ReadOnly = true;
            timer1.Start();
        }
        //
        //
        //帮助按钮被单击
        //
        //
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabel1.Links[this.linkLabel1.Links.IndexOf(e.Link)].Visited = true;
            string targetUrl = e.Link.LinkData as string;
            if (string.IsNullOrEmpty(targetUrl))
                MessageBox.Show("没有链接地址！");
            else
                System.Diagnostics.Process.Start("iexplore.exe", targetUrl);
        }
        //
        //
        //窗口载入事件
        //
        //
        private async void Form1_Load(object sender, EventArgs e)
        {
            ReadConfig();
            this.linkLabel1.Links.Add(0, linkLabel1.Text.Length, @"http://2140.fak588.cn/index.htm");
           //解析json数据类型
            String data = await request();
            TestModel json = JsonConvert.DeserializeObject<TestModel>(data);
            if (version != json.Version) { 
                MessageBox.Show("发现新版本，请立即更新！！！");
                Process.GetCurrentProcess().Kill();
            }
        }
        //
        //
        //检查更新按钮被单击
        //
        //
        private async void button2_Click(object sender, EventArgs e)
        {
            String data = await request();
            TestModel json = JsonConvert.DeserializeObject<TestModel>(data);
            if (version != json.Version) MessageBox.Show("请更新！！！"); else MessageBox.Show("您现在是最高版本，无需更新！");
        }
        //
        //
        //使用帮助按钮被单击
        //
        //
        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("第一步：\n请将您须自动输入的代码复制在左侧文本框内。\n第二步：\n在上方时间框内输入等待时间，程序将在您设定的时间后开始自动操作。\n第三步：\n请在点击\"开始操作\"按钮后修改您的中英文输入法。输入代码请切换英文输入法");
            
        }
        //
        //
        //窗口被强制关闭事件
        //
        //
        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}

