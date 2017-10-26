using OnvifIPClib.entity;
using OnvifIPClib.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnvifAssistant
{
    public partial class IpcLoginForm : Form
    {
      public  IPC ipc=null;
        public static event EventHandler<IPC> OnMessage;
        public IpcLoginForm()
        {
            InitializeComponent();
           
        }
        
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ipc == null)
            {
                ipc = new IPC();
            }
            ipc.ip = tbIp.Text;
            ipc.port = CommonUtils.str2int(tbPort.Text,-1);
            ipc.user = tbIp.Text;
            ipc.password = tbIp.Text;
            if (OnMessage != null) OnMessage(this, ipc);
            this.Close();
        }

        private void IpcLoginForm_Load(object sender, EventArgs e)
        {
            if (ipc != null)
            {
                tbIp.Text = ipc.ip;
                tbPort.Text = ipc.port.ToString();
                tbUser.Text = ipc.user;
                tbPwd.Text = ipc.password;
            }
            else
            {
                Console.WriteLine("ipc is null");
            }
        }
    }
}
