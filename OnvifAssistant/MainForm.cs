using OnvifIPClib.entity;
using OnvifIPClib.lib;
using OnvifIPClib.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnvifAssistant
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }     

        private void Form1_Load(object sender, EventArgs e)
        {
            IpcLoginForm.OnMessage += IpcLoginForm_OnMessage;
        }

        private void IpcLoginForm_OnMessage(object sender, OnvifIPClib.entity.IPC ipc)
        {
            string rtspUrl = OnvifMethod.getRtspUrl(ipc.ip, ipc.port,ipc.user,ipc.password);
            Debug.WriteLine(rtspUrl);
            VideoView vv = new VideoView();
            vv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
          | System.Windows.Forms.AnchorStyles.Left)
          | System.Windows.Forms.AnchorStyles.Right)));
            this.Controls.Add(vv);
            vv.play(rtspUrl);
        }

        private void OnvifMethod_OnMessage(object sender, OnvifIPClib.entity.IPC e)
        {           
            listBoxIPC.Items.Add(e.ip + ":" + e.port);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            listBoxIPC.Items.Clear();
            OnvifMethod.OnMessage += OnvifMethod_OnMessage;
            OnvifMethod.GetIpCameras();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process tt = System.Diagnostics.Process.GetProcessById(System.Diagnostics.Process.GetCurrentProcess().Id);
            tt.Kill();
        }

        private void listBoxIPC_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string ipAddr = listBoxIPC.SelectedItem.ToString();
            Console.WriteLine(ipAddr);
            string[] temp = ipAddr.Split(':');
            if (temp.Length < 2)
            {
                return;
            }
            IPC ipc = new IPC();
            ipc.ip = temp[0];
            ipc.port = CommonUtils.str2int(temp[1], -1);
            ipc.user = "admin";

            IpcLoginForm loginForm = new IpcLoginForm();
            loginForm.ipc = ipc;
            loginForm.Show();
          
        }
    }
}
