using System.Net;
using System.Net.NetworkInformation;
using System.Text;
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
            if (this.aTimer != null)
            {
            this.aTimer.Enabled = false;
            }

            this.aTimer = new (1000);
            ElapsedEventHandler onTimedEvent = this.SendPin;
            aTimer.Elapsed += onTimedEvent;
            this.aTimer.AutoReset = true;
            this.aTimer.Enabled = true;
            // waiter.WaitOne();
        }

        private void SendPin(Object source, ElapsedEventArgs e)
        {
            Ping ping = new();
            ping.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);
            string data = "12345678901234567890123456789012";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 1000;

            AutoResetEvent waiter = new AutoResetEvent(false);
            IPAddress ip = IPAddress.Parse(txt_ip.Text.Trim());
            PingOptions options = new PingOptions(64, true);
            ping.SendAsync(ip, timeout, buffer, options, waiter);
        }

        private void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            PingReply reply = e.Reply;
            if (reply == null) return;
            if(reply.Status == IPStatus.Success)
            {
                this.SendLog("Respuesta desde" + reply.Address.ToString() + ": Tiempo=" + reply.RoundtripTime + " Bytes=" + reply.Buffer.Length);
            }

            if(e.UserState != null)
            {
                ((AutoResetEvent)e.UserState).Set();
            }
        }

        private delegate void SendLogDelegate(string msg);

        private void SendLog(string msg)
        {
            if (txt_ip.InvokeRequired)
            {
                SendLogDelegate delegated = new SendLogDelegate(SendLog);
                txt_ip.Invoke(delegated, msg);
            }
            else
            {
                if(this.txt_log.Text == "")
                {
                    this.txt_log.Text = msg;
                }
                else
                {
                    this.txt_log.Text = msg + Environment.NewLine + this.txt_log.Text;
                }
            }
        }
    }
}