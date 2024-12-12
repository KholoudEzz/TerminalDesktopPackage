using Microsoft.Win32;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Printing;
using System.Security.Principal;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RashidConfiguration
{
    public partial class Configuration : Form
    {
        public Configuration()
        {
            InitializeComponent();
        }


        string DefaultPhyPrinter = "";
        string configFilePath = "";
        string XMLPrinterPath = "";
        string BasePath = "";
        string RashidSilentName = "Tools\\TerminalDesktopSilence.exe";
        string RashidPrinterName = "Tools\\TerminalDesktop.exe";

        static string ReadRegistryValue(string ConfigkeyPath, string valueName)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(ConfigkeyPath))
                {
                    if (key != null)
                    {

                        object o = key.GetValue(valueName);
                        if (o != null)
                        {
                            return o.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading registry: {ex.Message}");
            }

            return "";
        }


        private void SaveSelectionBtn_Click(object sender, EventArgs e)
        {
            // the selected printer
            if (BothRBtn.Checked)
            {
                ConfigFileRW.selectedPrinter = "BothPrinter";
                ConfigFileRW.ExecutablePath = BasePath + RashidPrinterName;
                ExecutablePathUpdate(ConfigFileRW.ExecutablePath);
            }
            else if (PhyPrinterRBtn.Checked)
            {
                ConfigFileRW.ExecutablePath = BasePath + RashidPrinterName;
                ExecutablePathUpdate(ConfigFileRW.ExecutablePath);
            }
            else if (RPrinter_RBtn.Checked)
            {
                ConfigFileRW.selectedPrinter = "RashidPrinter";
                ConfigFileRW.ExecutablePath = BasePath + RashidPrinterName;
                ExecutablePathUpdate(ConfigFileRW.ExecutablePath);
            }
            else if (BothSilentRBtn.Checked)
            {
                ConfigFileRW.selectedPrinter = "BothPrinterSilent";
                ConfigFileRW.ExecutablePath = BasePath + RashidSilentName;
                ExecutablePathUpdate(ConfigFileRW.ExecutablePath);
            }


            //DefaultPhyPrinter 
            ConfigFileRW.DefaultPhyPrinter = PrintersCBox.Text;



            try
            {



                string ret = ConfigFileRW.SaveFromXml(configFilePath);

                if (ret == "Failed")
                    MessageBox.Show("Failed to save Configuration.");

                MessageBox.Show("Rashid Configurations saved successfully.", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save Configuration .");
            }
        }


        static bool IsRunningAsAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);
            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }

        static void RunAsAdministrator()
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = Application.ExecutablePath, // Path to your executable
                UseShellExecute = true,
                Verb = "runas" // Causes the process to run with elevated privileges
            };

            try
            {
                Process.Start(processInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to restart the application with administrator privileges: {ex.Message}");
            }

            // Close the current instance of the application
            Application.Exit();
        }
        public void CloseRunningInstance()
        {
            // Get the current process name
            string currentProcessName = Process.GetCurrentProcess().ProcessName;

            // Get all processes with the same name
            Process[] processes = Process.GetProcessesByName(currentProcessName);

            // Terminate all other instances of the application
            foreach (Process process in processes)
            {
                // Ensure not to terminate the current process
                if (process.Id != Process.GetCurrentProcess().Id)
                {
                    process.Kill(); // Terminate the process
                }
            }
        }
        private void Configuration_Load(object sender, EventArgs e)
        {
            CloseRunningInstance();

#if !DEBUG

            if (!IsRunningAsAdministrator())
            {
                // Restart the application with elevated privileges
                RunAsAdministrator();
                return;
            }


#endif



            string registryAppKey = @"SOFTWARE\Rasheed";
            string registryCPName = "ConfigPath";
            string registryPPValue = "PrinterPath";

            configFilePath = ReadRegistryValue(registryAppKey, registryCPName);
            XMLPrinterPath = ReadRegistryValue(registryAppKey, registryPPValue);

            BasePath = Directory.GetParent(configFilePath).FullName;
            if (!BasePath.EndsWith("\\"))
            {
                BasePath += "\\";
            }


            if (string.IsNullOrEmpty(configFilePath))
            {
                Console.WriteLine("Configuration file path not found in the registry.");
                return;
            }

            ConfigFileRW.LoadFromXml(configFilePath);


            // Load the selected printer from the app config file
            if (ConfigFileRW.selectedPrinter != "")
            {
                string selectedPrinter = ConfigFileRW.selectedPrinter;
                switch (selectedPrinter)
                {
                    case "RashidPrinter":
                        RPrinter_RBtn.Checked = true;
                        break;
                    case "PhysicalPrinter":
                        PhyPrinterRBtn.Checked = true;
                        break;
                    case "BothPrinter":
                        BothRBtn.Checked = true;
                        break;
                    case "BothPrinterSilent":
                        BothSilentRBtn.Checked = true;
                        break;
                    default:
                        RPrinter_RBtn.Checked = false;
                        PhyPrinterRBtn.Checked = false;
                        BothRBtn.Checked = false;
                        BothSilentRBtn.Checked = false;
                        break;
                }
            }

            FillPrintersCombo(ConfigFileRW.DefaultPhyPrinter);


        }
        private void FillPrintersCombo(string DefaultPhyPrinter)
        {
            // Add list of installed printers found to the combo box.
            // The pkInstalledPrinters string will be used to provide the display string.
            string pkInstalledPrinters;
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
                PrintersCBox.Items.Add(pkInstalledPrinters);
            }
            PrintersCBox.SelectedItem = DefaultPhyPrinter;
        }



        private void PrintersCBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConfigFileRW.DefaultPhyPrinter = PrintersCBox.Text;
        }

        private void RPrinter_RBtn_CheckedChanged(object sender, EventArgs e)
        {

        }

        public static void RunCommandSilently(string command, string arguments)
        {
            try
            {
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    CreateNoWindow = true, // Do not create a console window
                    UseShellExecute = false, // Do not use the OS shell
                    RedirectStandardOutput = true, // Redirect standard output
                    RedirectStandardError = true // Redirect standard error
                };

                using (Process process = new Process())
                {
                    process.StartInfo = processInfo;
                    process.Start();

                    // Optionally read the output and error streams
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(output))
                    {
                        Console.WriteLine("Output: " + output);
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine("Error: " + error);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        private void ExecutablePathUpdate(string ExecutablePathStr)
        {
            string command = XMLPrinterPath;

            string arguments = $"/configure \"clearsteps\"";
            RunCommandSilently(command, arguments);


            arguments = $"/configure \"addstep=1,\\\"{ExecutablePathStr}\\\"\"";
            RunCommandSilently(command, arguments);


        }
    }

}

