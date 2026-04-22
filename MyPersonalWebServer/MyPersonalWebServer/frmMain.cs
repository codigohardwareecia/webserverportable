using MyPersonalWebServer.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPersonalWebServer
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            LoadWebView();
        }

        public async void LoadWebView()
        {
            await webView21.EnsureCoreWebView2Async(null);

            webView21.CoreWebView2.Navigate("http://localhost:5000/index.html");
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            HomeController.OnSendCommand += (valor) =>
            {
                this.Invoke(new Action(() =>
                {
                    lblStatus.Text = $"Recebido da Web: {valor}";
                }));
            };
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            DataShare.Instance.Nome = "Dados vindos do Window Forms";
        }
    }
}
