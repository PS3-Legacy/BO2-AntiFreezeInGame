using System;
using System.Linq;
using System.Windows.Forms;
using PS3Lib;
using System.Diagnostics;

namespace AntiFreezeInGame_By_BISOON
{
    public partial class frmMain : Form
    {

     /* ****************************
        

                  By
                BISOON
                   &
                 IM.83


       ***************************** */
        PS3API PS3 = new PS3API();
        string inGame = "Not In Game";
        int dangerousPlayer = 0;
        bool ClientInGame()
        {
            return PS3.Extension.ReadBool(0x1CB68E8);
        }
        string GetClientName(int Client)
        {
            string str = PS3.Extension.ReadString(0x36524C14 + 0x4E180 * (uint)Client); return str;
        }

        void SetClientName(int Client)
        {
            string clientName = GetClientName(Client).Replace("^","");
            PS3.Extension.WriteString(0x36524C14 + 0x4E180 * (uint)Client, clientName);
            PS3.Extension.WriteString(0x0178646c + 0x5808 * (uint)Client, clientName);
        }
        string GetClientClan(int Client)
        {
            return PS3.Extension.ReadString((0x0178646c + 0x6C) + 0x5808 * (uint)Client);
        }
        void SetClientClan(int Client)
        {
            string clientClan = GetClientClan(Client).Replace("^", "");
            PS3.Extension.WriteString((0x36524C14 + 0x20) + 0x4E180 * (uint)Client, clientClan);
            PS3.Extension.WriteString((0x0178646c + 0x6C) + 0x5808 * (uint)Client, clientClan);
        }
        void RunAntiName(bool True)
        {
            PS3.InitTarget();
                while (True == true)
                {
                    if (ClientInGame())
                    {
                        inGame = "In Game";
                        for (int i = 0; i < 12; i++)
                        {
                            if (GetClientName(i).Contains('^'))
                            {
                                dangerousPlayer++;
                                SetClientName(i);
                            }
                        }
                    }
                    else
                    {
                        inGame = "Not In Game";
                        dangerousPlayer = 0;
                        gs.Text = "Game Status  : " + " Safe";
                    }
                    Application.DoEvents();
                }
        }

        void RunAntiClan(bool True)
        {
            PS3.InitTarget();
            while (True == true)
            {
                if (ClientInGame())
                {
                    for (int i = 0; i < 12; i++)
                    {
                        if (GetClientClan(i).Contains('^'))
                        {
                            SetClientClan(i);
                        }
                    }
                }
                Application.DoEvents();
            }
        }
        public frmMain()
        {
            InitializeComponent();
        }

        private void conBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (conBtn.Text == "Connect / Attach")
                {
                    if (cexCh.Checked)
                        PS3.ChangeAPI(SelectAPI.ControlConsole);
                    else
                        PS3.ChangeAPI(SelectAPI.TargetManager);
                    if (PS3.ConnectTarget() && PS3.AttachProcess())
                    {
                        MessageBox.Show("Connected / Attached", "Connection", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        conBtn.Text = "Disconnect";
                    }
                    else
                    {
                        MessageBox.Show("Error", "Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (conBtn.Text == "Disconnect")
                {
                    RunAntiName(false);
                    RunAntiClan(false);
                    dangerousPlayer = 0;
                    PS3.DisconnectTarget();
                    conBtn.Text = "Connect / Attach";
                    runAntiBtn.Text = "Run AntiFreeze";
                }
            }
            catch
            {
                MessageBox.Show("Error", "Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void runAntiBtn_Click(object sender, EventArgs e)
        {
            if (runAntiBtn.Text == "Run AntiFreeze")
            {
                gs.Text = "Game Status  : " + " Safe";
                RunAntiName(true);
                RunAntiClan(true);
                runAntiBtn.Text = "Stop AntiFreeze";
                return;
            }
            gs.Text = "Game Status  : " + " Unsafe";
            runAntiBtn.Text = "Run AntiFreeze";
            RunAntiName(false);
            RunAntiClan(false);
            dangerousPlayer = 0;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Process.Start("http://arabmodding.com/ar/");
            Process.Start("http://www.youtube.com/c/bisoon");
            Process.Start("http://www.instagram.com/im.83");
        }
    }
}
