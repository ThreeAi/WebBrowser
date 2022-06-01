using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PWebBrowser
{
    public partial class Form1 : Form
    {
        private string strINIFile = @"C:\Users\Artem\Desktop\проги\browser\browser.ini";
        private const string HeadWindow = "[Window]";
        private const string HeadBrowser = "[Browser]";


        public Form1()
        {
            InitializeComponent();
        }

        // Обработчик события закрытия формы
        private void Form1_FormClosed(Object sender, FormClosedEventArgs e)
        {
            
            FileInfo fi = new FileInfo(strINIFile);
            StreamWriter sw = fi.CreateText();
            sw.WriteLine(HeadWindow);
            sw.WriteLine("Lft={0}", this.Left);
            sw.WriteLine("Top={0}", this.Top);
            sw.WriteLine("Wdt={0}", this.Width);
            sw.WriteLine("Hgh={0}", this.Height);
            sw.WriteLine("Sta={0}", this.WindowState);
            sw.WriteLine(HeadBrowser);
            sw.WriteLine("URL={0}", this.txtURL.Text);
            sw.Close();
        }

        // Обработчик события загрузки формы
        private void Form1_Load(object sender, EventArgs e)
        {
            int lft, top, hgh, wdt;
            string strURL = "", sLft, sTop, sHgh, sWdt, sta;
            Dictionary<string, string> window = new Dictionary<string, string>();
            Dictionary<string, string> url = new Dictionary<string, string>();
            Parameters(strINIFile, out window, out url);
            window.TryGetValue("Lft", out sLft);
            if (Int32.TryParse(sLft, out lft))
                if (lft >= -8)
                    this.Left = lft;
            window.TryGetValue("Top", out sTop);
            if (Int32.TryParse(sTop, out top))
                if (top >= -8)
                    this.Top = top;
            window.TryGetValue("Wdt", out sWdt);
            if (Int32.TryParse(sWdt, out wdt))
                if (wdt >= 400)
                    this.Width = wdt;
            window.TryGetValue("Hgh", out sHgh);
            if (Int32.TryParse(sHgh, out hgh))
                if (hgh >= 400)
                    this.Height = hgh;
            window.TryGetValue("Sta", out sta);
            switch (sta)
            {
                case "Normal":
                    this.WindowState = FormWindowState.Normal;
                    break;
                case "Maximized":
                    this.WindowState = FormWindowState.Maximized;
                    break;
                case "Minimized":
                    this.WindowState = FormWindowState.Minimized;
                    break;
            }
            url.TryGetValue("URL", out strURL);
            wbBrowser.Navigate(strURL);
            this.txtURL.Text = strURL;
        }


        
        // Обработчик события нажатия клавиши
        private void txtURL_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            // Check for the flag being set in the KeyDown event.
            if (e.KeyChar == (char)Keys.Return)
            {
                //MessageBox.Show(txtURL.Text);
                wbBrowser.Navigate(txtURL.Text);
                e.Handled = true;
            }
             
        }
        private static void Parameters(string name, out Dictionary<string, string> window, out Dictionary<string, string> url)
        {      
            window = new Dictionary<string, string>();
            url = new Dictionary<string, string>();
            Dictionary<string, string> temp = new Dictionary<string, string>();
            FileInfo fi = new FileInfo(name);
            try
            {
                StreamReader sr = fi.OpenText();
                string s;
                while (sr.EndOfStream == false)
                {
                    s = sr.ReadLine();
                    if (s.Length == 0)
                        continue;
                    if ((s == HeadBrowser) || (s == HeadWindow))
                    {
                        switch (s)
                        {
                            case HeadWindow:
                                temp = window;
                                break;
                            case HeadBrowser:
                                temp = url;
                                break;
                        }
                        continue;
                    }
                    string[] par = s.Split('=');                   
                    temp.Add(par[0], par[1]);                                       
                }  
                sr.Close();
            }           
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }             
        }       
    }
}
