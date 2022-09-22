using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Timers;
using Timer = System.Timers.Timer;

namespace inspectnet
{
    public partial class Form1 : Form
    {
        private Timer aTimer;
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_inspect_Click(object sender, EventArgs e)
        {
            this.IPAddressAdditionalInfo();
            this.aTimer = new (1000);
            aTimer.Elapsed += OnTimedEvent;
            this.aTimer.AutoReset = true;
            this.aTimer.Enabled = true;

        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Ping p1 = new();
            PingReply PR = p1.Send(txt_ip.Text);
            this.SendLog("Respuesta " + txt_ip.Text + ": " + PR.Status.ToString());
            p1.Send(txt_ip.Text);
        }

        private void SendLog(string msg)
        {
            if(txt_log.Text == "")
            {
                txt_log.Text = msg;
            }
            else
            {
                txt_log.Text = txt_log.Text + Environment.NewLine + msg;
            }
        }

        private void IPAddressAdditionalInfo()
        {
            try
            {
                this.SendLog("SupportsIPv4: " + Socket.OSSupportsIPv4);
                this.SendLog("SupportsIPv6: " + Socket.OSSupportsIPv6);
            }
            catch (Exception e)
            {
                this.SendLog("[IPAddresses] Exception: " + e.ToString());
            }
        }
    }
}