using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;

namespace SurvivorSeriesUpdater
{
    public partial class MainWindow : MetroForm
    {
        public MainWindow()
        {
            InitializeComponent();
            StyleManager = metroStyleManager1;
            metroTabControl1.SelectedTab = metroTabPage1;
            metroComboBox1.SelectedIndex = 0;
            metroComboBox2.SelectedIndex = 0;
        }

        private bool _rethoughtnews = false;
        private bool _liveupdate = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            Task.Delay(1000, CancellationToken.None);
            LoadOneByOne();
        }

        private void LoadOneByOne()
        {
            Task.Delay(500, CancellationToken.None);
            PullingData();
            Task.Delay(500, CancellationToken.None);
            PullingAssemblyData();
            Task.Delay(500, CancellationToken.None);
            PullDataForBugTracker();
            Task.Delay(500, CancellationToken.None);
            PullDataForRequestsShared();
            Task.Delay(500, CancellationToken.None);
            PullingTrackerData();
        }

        private void PullingAssemblyData()
        {
            try
            {
                var client = new WebClient();
                var ssashe =
                    client.DownloadString(
                        "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/SSAshe");
                metroLabel8.Text = ssashe;

                var ssryze =
                    client.DownloadString(
                        "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/SSRyze");
                metroLabel9.Text = ssryze;

                var ssbrand =
                    client.DownloadString(
                        "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/SSBrand");
                metroLabel10.Text = ssbrand;

                var ssmalzahar =
                    client.DownloadString(
                        "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/SSMalzahar");
                metroLabel11.Text = ssmalzahar;

                var ssaio =
                    client.DownloadString(
                        "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/SSAIO");
                metroLabel12.Text = ssaio;

                var ssirelia =
                    client.DownloadString(
                        "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/SSIrelia");
                metroLabel13.Text = ssirelia;

                var sshscamera =
                    client.DownloadString(
                        "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/SShsCamera");
                metroLabel15.Text = sshscamera;

                var rethoughtlib =
                    client.DownloadString(
                        "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/RethoughtLib");
                metroLabel29.Text = rethoughtlib;

                var rethoughtcamera =
                    client.DownloadString(
                        "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/RethoughtCamera");
                metroLabel31.Text = rethoughtcamera;

                var rethoughtirelia =
                    client.DownloadString(
                        "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/RethoughtIrelia");
                metroLabel33.Text = rethoughtirelia;
            }
            catch (Exception)
            {
                MetroMessageBox.Show(this,
                    "Error Handler [E1309]: Report this error code in the joduska.me thread if you see this message.");
            }
        }

        private void PullingTrackerData()
        {
            try
            {
                var client = new WebClient();
                client.DownloadStringAsync(new Uri(
                    "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/ProgressTracker/Project1"));
                client.DownloadStringCompleted += (sender, args) => { metroTextBox5.Text = args.Result; };

                var client2 = new WebClient();
                client2.DownloadStringAsync(new Uri(
                    "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/ProgressTracker/Project1Progress"));
                client2.DownloadStringCompleted +=
                    (sender, args) => { metroProgressBar1.Value = Convert.ToInt32(args.Result); };

                var client3 = new WebClient();
                client3.DownloadStringAsync(new Uri(
                    "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/ProgressTracker/Project2"));
                client3.DownloadStringCompleted += (sender, args) => { metroTextBox6.Text = args.Result; };

                var client4 = new WebClient();
                client4.DownloadStringAsync(new Uri(
                    "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/ProgressTracker/Project2Progress"));
                client4.DownloadStringCompleted +=
                    (sender, args) => { metroProgressBar2.Value = Convert.ToInt32(args.Result); };

                var client5 = new WebClient();
                client5.DownloadStringAsync(new Uri(
                    "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/ProgressTracker/Project3"));
                client5.DownloadStringCompleted += (sender, args) => { metroTextBox7.Text = args.Result; };

                var client6 = new WebClient();
                client6.DownloadStringAsync(new Uri(
                    "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/ProgressTracker/Project3Progress"));
                client6.DownloadStringCompleted +=
                    (sender, args) => { metroProgressBar3.Value = Convert.ToInt32(args.Result); };

                var client7 = new WebClient();
                client7.DownloadStringAsync(new Uri(
                    "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/ProgressTracker/Project4"));
                client7.DownloadStringCompleted += (sender, args) => { metroTextBox10.Text = args.Result; };

                var client8 = new WebClient();
                client8.DownloadStringAsync(new Uri(
                    "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/ProgressTracker/Project4Progress"));
                client8.DownloadStringCompleted +=
                    (sender, args) => { metroProgressBar4.Value = Convert.ToInt32(args.Result); };

                var client9 = new WebClient();
                client9.DownloadStringAsync(new Uri(
                    "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/ProgressTracker/Project5"));
                client9.DownloadStringCompleted += (sender, args) => { metroTextBox9.Text = args.Result; };

                var client10 = new WebClient();
                client10.DownloadStringAsync(new Uri(
                    "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/ProgressTracker/Project5Progress"));
                client10.DownloadStringCompleted +=
                    (sender, args) => { metroProgressBar5.Value = Convert.ToInt32(args.Result); };

                var client11 = new WebClient();
                client11.DownloadStringAsync(new Uri(
                    "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/ProgressTracker/Project6"));
                client11.DownloadStringCompleted += (sender, args) => { metroTextBox8.Text = args.Result; };

                var client12 = new WebClient();
                client12.DownloadStringAsync(new Uri(
                    "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/ProgressTracker/Project6Progress"));
                client12.DownloadStringCompleted +=
                    (sender, args) => { metroProgressBar6.Value = Convert.ToInt32(args.Result); };
            }
            catch (Exception)
            {
                MetroMessageBox.Show(this,
                    "Error Handler [E1309]: Report this error code in the joduska.me thread if you see this message.");
            }
        }

        public void PullDataForRequestsShared()
        {
            try
            {
                var client = new WebClient();
                client.DownloadStringAsync(new Uri(
                    "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/ChampionRequests"));
                client.DownloadStringCompleted += (sender, args) =>
                {
                    var reworkedtext = args.Result.Replace("..", Environment.NewLine);
                    metroTextBox2.Text = reworkedtext;
                    metroProgressSpinner3.Visible = false;
                };

                var client2 = new WebClient();
                client2.DownloadStringAsync(new Uri(
                    "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/AssemblyImprovements"));
                client2.DownloadStringCompleted += (sender, args) =>
                {
                    var reworkedtext2 = args.Result.Replace("..", Environment.NewLine);
                    metroTextBox3.Text = reworkedtext2;
                    metroProgressSpinner4.Visible = false;
                };
            }
            catch (Exception)
            {
                metroProgressSpinner3.Visible = false;
                metroProgressSpinner4.Visible = false;
                MetroMessageBox.Show(this,
                    "Error Handler [E1309]: Report this error code in the joduska.me thread if you see this message.");
            }
        }

        public void PullDataForBugTracker()
        {
            try
            {
                var client = new WebClient();
                client.DownloadStringAsync(
                    new Uri("https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/BugTracker"));
                client.DownloadStringCompleted += (sender, args) =>
                {
                    var reworkedtext = args.Result.Replace("..", Environment.NewLine);
                    metroTextBox4.Text = reworkedtext;
                    metroProgressSpinner2.Visible = false;
                };
            }
            catch (Exception)
            {
                MetroMessageBox.Show(this,
                    "Error Handler [E1309]: Report this error code in the joduska.me thread if you see this message.");
            }
        }

        public void PullingData()
        {
            try
            {
                var client = new WebClient();
                client.DownloadStringAsync(
                    new Uri("https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/News"));
                client.DownloadStringCompleted += (sender, args) =>
                {
                    var reworkedtext = args.Result.Replace("..", Environment.NewLine);
                    metroTextBox1.Text = reworkedtext;
                    metroProgressSpinner1.Visible = false;
                };
            }
            catch (Exception)
            {
                MetroMessageBox.Show(this,
                    "Error Handler [E1309]: Report this error code in the joduska.me thread if you see this message.");
            }
        }

        /// <summary>
        ///     Ashe PlaySharp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroButton1_Click(object sender, EventArgs e)
        {
            Process.Start("ps://assembly/1196");
        }

        /// <summary>
        ///     Ryze PlaySharp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroButton6_Click(object sender, EventArgs e)
        {
            Process.Start("ps://assembly/1173");
        }

        /// <summary>
        ///     Brand PlaySharp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroButton9_Click(object sender, EventArgs e)
        {
            Process.Start("ps://assembly/1169");
        }

        /// <summary>
        ///     Malzahar PlaySharp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroButton12_Click(object sender, EventArgs e)
        {
            Process.Start("ps://assembly/1157");
        }

        /// <summary>
        ///     AIO PlaySharp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroButton15_Click(object sender, EventArgs e)
        {
            Process.Start("ps://assembly/1277");
        }

        /// <summary>
        ///     Irelia PlaySharp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroButton18_Click(object sender, EventArgs e)
        {
            Process.Start("ps://assembly/1261");
        }


        /// <summary>
        ///     Malzahar GitHub
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroButton10_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/SupportExTraGoZ/LeagueSharp/blob/master/Malzahar");
        }


        /// <summary>
        ///     Brand GitHub
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroButton7_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/SupportExTraGoZ/LeagueSharp/blob/master/Brand");
        }

        /// <summary>
        ///     Ryze GitHub
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroButton4_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/SupportExTraGoZ/LeagueSharp/blob/master/Ryze");
        }

        /// <summary>
        ///     Ashe GitHub
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroButton3_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/SupportExTraGoZ/LeagueSharp/blob/master/Ashe");
        }

        /// <summary>
        ///     AIO GitHub
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroButton13_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/SupportExTraGoZ/LeagueSharp/blob/master/SurvivorSeriesAIO");
        }


        /// <summary>
        ///     Irelia GitHub
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroButton16_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/SupportExTraGoZ/LeagueSharp/blob/master/Irelia");
        }

        /// <summary>
        ///     hsCamera PlaySharp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroButton21_Click(object sender, EventArgs e)
        {
            Process.Start("ps://assembly/1247");
        }

        /// <summary>
        ///     hsCamera GitHub
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroButton19_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/SupportExTraGoZ/LeagueSharp/tree/master/hsCamera");
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            Process.Start("ls://project/SupportExTraGoZ/LeagueSharp/SurvivorAshe/");
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            Process.Start("ls://project/SupportExTraGoZ/LeagueSharp/SurvivorRyze/");
        }

        private void metroButton8_Click(object sender, EventArgs e)
        {
            Process.Start("ls://project/SupportExTraGoZ/LeagueSharp/SurvivorSeriesBrand/");
        }

        private void metroButton11_Click(object sender, EventArgs e)
        {
            Process.Start("ls://project/SupportExTraGoZ/LeagueSharp/SurvivorMalzahar/");
        }

        private void metroButton14_Click(object sender, EventArgs e)
        {
            Process.Start("ls://project/SupportExTraGoZ/LeagueSharp/SurvivorSeriesAIO/");
        }

        private void metroButton17_Click(object sender, EventArgs e)
        {
            Process.Start("ls://project/SupportExTraGoZ/LeagueSharp/SVIrelia/");
        }

        private void metroButton20_Click(object sender, EventArgs e)
        {
            Process.Start("ls://project/SupportExTraGoZ/LeagueSharp/hsCamera/");
        }

        private void metroButton22_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.joduska.me/forum/user/1177184-supportextragoz/");
        }

        private void metroButton23_Click(object sender, EventArgs e)
        {
            Process.Start(
                "https://www.joduska.me/forum/index.php?app=members&module=messaging&section=send&do=form&fromMemberID=1177184");
        }

        private void metroButton24_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/SupportExTraGoZ/Riot-API/blob/Riot-API-Get/BugTracker");
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        #region SettingsTheme

        private void metroComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var item = MetroColorStyle + metroComboBox2.SelectedItem;
            //metroStyleManager1.Style
            switch (metroComboBox2.SelectedIndex)
            {
                case 0:
                    metroStyleManager1.Style = MetroColorStyle.Blue;
                    break;
                case 1:
                    metroStyleManager1.Style = MetroColorStyle.Default;
                    break;
                case 2:
                    metroStyleManager1.Style = MetroColorStyle.Black;
                    break;
                case 3:
                    metroStyleManager1.Style = MetroColorStyle.White;
                    break;
                case 4:
                    metroStyleManager1.Style = MetroColorStyle.Silver;
                    break;
                case 5:
                    metroStyleManager1.Style = MetroColorStyle.Red;
                    break;
                case 6:
                    metroStyleManager1.Style = MetroColorStyle.Green;
                    break;
                case 7:
                    metroStyleManager1.Style = MetroColorStyle.Lime;
                    break;
                case 8:
                    metroStyleManager1.Style = MetroColorStyle.Teal;
                    break;
                case 9:
                    metroStyleManager1.Style = MetroColorStyle.Orange;
                    break;
                case 10:
                    metroStyleManager1.Style = MetroColorStyle.Brown;
                    break;
                case 11:
                    metroStyleManager1.Style = MetroColorStyle.Pink;
                    break;
                case 12:
                    metroStyleManager1.Style = MetroColorStyle.Magenta;
                    break;
                case 13:
                    metroStyleManager1.Style = MetroColorStyle.Purple;
                    break;
                default:
                    metroStyleManager1.Style = MetroColorStyle.Default;
                    break;
            }
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (metroComboBox1.SelectedIndex)
            {
                case 0:
                    metroStyleManager1.Theme = MetroThemeStyle.Dark;
                    break;
                case 1:
                    metroStyleManager1.Theme = MetroThemeStyle.Light;
                    break;
                default:
                    metroStyleManager1.Theme = MetroThemeStyle.Dark;
                    break;
            }
        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void metroButton26_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.joduska.me/forum/user/20623-rethought/");
        }

        private void metroButton25_Click(object sender, EventArgs e)
        {
            Process.Start(
                "https://www.joduska.me/forum/index.php?app=members&module=messaging&section=send&do=form&fromMemberID=20623");
        }

        private void metroButton29_Click(object sender, EventArgs e)
        {
            Process.Start("ps://assembly/1186");
        }

        private void metroButton32_Click(object sender, EventArgs e)
        {
            Process.Start("ps://assembly/1250");
        }

        private void metroButton35_Click(object sender, EventArgs e)
        {
            Process.Start("ps://assembly/1264");
        }

        private void metroButton28_Click(object sender, EventArgs e)
        {
            Process.Start("ls://project/MediaGithub/LeagueSharp/RethoughtLib/");
        }

        private void metroButton31_Click(object sender, EventArgs e)
        {
            Process.Start("ls://project/MediaGithub/LeagueSharp/Rethought Camera/");
        }

        private void metroButton34_Click(object sender, EventArgs e)
        {
            Process.Start("ls://project/MediaGithub/LeagueSharp/Rethought Irelia/");
        }

        private void metroButton27_Click(object sender, EventArgs e)
        {
            Process.Start(
                "https://github.com/MediaGithub/LeagueSharp/blob/master/LeagueSharp/RethoughtLib");
        }

        private void metroButton30_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/MediaGithub/LeagueSharp/blob/master/LeagueSharp/Rethought%20Camera");
        }

        private void metroButton33_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/MediaGithub/LeagueSharp/blob/master/LeagueSharp/Rethought%20Irelia");
        }

        private void metroButton36_Click(object sender, EventArgs e)
        {
            if (_rethoughtnews == false)
            {
                _rethoughtnews = true;
                metroLabel2.Text = ".:Rethought News:.";
                try
                {
                    var client = new WebClient();
                    client.DownloadStringAsync(
                        new Uri("https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/RethoughtNews"));
                    client.DownloadStringCompleted += (senderweb, args) =>
                    {
                        var reworkedtext = args.Result.Replace("..", Environment.NewLine);
                        metroTextBox1.Text = reworkedtext;
                        metroProgressSpinner1.Visible = false;
                    };
                }
                catch (Exception)
                {
                    MetroMessageBox.Show(this,
                        "Error Handler [E1309]: Report this error code in the joduska.me thread if you see this message.");
                }
            }
            else if (_rethoughtnews == true)
            {
                _rethoughtnews = false;
                metroLabel2.Text = ".:SurvivorSeries News:.";
                try
                {
                    var client = new WebClient();
                    client.DownloadStringAsync(
                        new Uri("https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/News"));
                    client.DownloadStringCompleted += (senderweb, args) =>
                    {
                        var reworkedtext = args.Result.Replace("..", Environment.NewLine);
                        metroTextBox1.Text = reworkedtext;
                        metroProgressSpinner1.Visible = false;
                    };
                }
                catch (Exception)
                {
                    MetroMessageBox.Show(this,
                        "Error Handler [E1309]: Report this error code in the joduska.me thread if you see this message.");
                }
            }
        }

        private void metroButton37_Click(object sender, EventArgs e)
        {
            if (_liveupdate == false)
            {
                _liveupdate = true;
                timer2.Start();
                liveUpdateLabel();
            }
            else
            {
                _liveupdate = false;
                timer2.Stop();
                liveUpdateLabel();
            }
        }

        private void liveUpdateLabel()
        {
            if (_liveupdate == false)
            {
                metroLabel35.Text = ".:Status: Not Updating:.";
                metroLabel35.Style = MetroColorStyle.Red;
            }
            else
            {
                metroLabel35.Text = ".:Status: Updating:.";
                metroLabel35.Style = MetroColorStyle.Green;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            PullingTrackerData();
        }

        private void metroButton38_Click(object sender, EventArgs e)
        {
            UpdateCheck();
        }

        public void UpdateCheck()
        {
            /*h3h3 version checker*/
            Task.Factory.StartNew(() =>
            {
                try
                {
                    using (var c = new WebClient())
                    {
                        var rawVersion =
                            c.DownloadString(
                                "https://raw.githubusercontent.com/SupportExTraGoZ/Riot-API/Riot-API-Get/ProgramVersion");
                        var match =
                            new Regex(@"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]")
                                .Match(rawVersion);

                        if (match.Success)
                        {
                            var gitVersion =
                                new Version(string.Format("{0}.{1}.{2}.{3}", match.Groups[1], match.Groups[2],
                                    match.Groups[3], match.Groups[4]));

                            if (gitVersion != typeof(Program).Assembly.GetName().Version)
                            {
                                DialogResult dr = MetroMessageBox.Show(this,
                                    "You are using OUTDATED version of SS Updater [" +
                                    gitVersion + "]\nPress OK to download the latest one!", "Updater", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                                switch (dr)
                                {
                                    case DialogResult.OK:
                                    {
                                        TryTaskDownload();
                                        break;
                                    }
                                    case DialogResult.Cancel:
                                    {
                                        break;
                                    }
                                }
                            }
                            else
                                MetroMessageBox.Show(this,
                                    "You are using the latest version of SS Updater [" +
                                    gitVersion + "]", "Updater", MessageBoxButtons.OK, MessageBoxIcon.Question);
                        }
                    }
                }
                catch (Exception)
                {
                    MetroMessageBox.Show(this,
                        "Error Handler [E1309]: Report this error code in the joduska.me thread if you see this message.");
                }
            });
        }

        private void TryTaskDownload()
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    WebClient client = new WebClient();
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                    client.DownloadFileAsync(new Uri("https://github.com/SupportExTraGoZ/Riot-API/raw/Riot-API-Get/Release.rar"), Assembly.GetExecutingAssembly().Location + @".rar");
                }
                catch (Exception)
                {
                    MetroMessageBox.Show(this, "Error Handler [E1310]: Report this error code in the joduska.me thread if you see this message.",
                    "Download Failed", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
            });
            thread.Start();
        }
        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                metroProgressBar7.Visible = true;
                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;
                metroProgressBar7.Value = int.Parse(Math.Truncate(percentage).ToString());
            });
        }
        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                metroProgressBar7.Visible = false;
                MetroMessageBox.Show(this, "The latest version of SS Updater has been succesfully downloaded!",
                    "Downloading Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        }
    }
}