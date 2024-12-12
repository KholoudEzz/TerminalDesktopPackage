using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Reflection;
using System.Security.Principal;

namespace PrinterSettingsTool
{
    public partial class PrinterTool : Form
    {
        public PrinterTool()
        {
            InitializeComponent();
            //  SetTitle();
        }

        string DefaultPhyPrinter = "";
        string configFilePath = "";
        string XMLPrinterPath = "";
        string DatabaseFileName = "";

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
            try
            {




                //DataBase
                if (TBDatabaseFile.Text == "" || TBDatabaseFile.Text == "Enter File path like  c:\\dbfile.db")
                {
                    MessageBox.Show("Please choose Database Path!", "Database Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;

                }
                else
                {
                    if (!TBDatabaseFile.Text.EndsWith(".db", StringComparison.OrdinalIgnoreCase))
                    {
                        if (DatabaseFileName == "")
                        {
                            Random random = new Random();
                            int randomNumber = random.Next(0, 10);
                            DateTime now = DateTime.Now;
                            DatabaseFileName = "R" + randomNumber.ToString() + "_" + now.ToString("ddHHmm") + ".db";
                        }

                        if (!TBDatabaseFile.Text.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
                            TBDatabaseFile.Text = TBDatabaseFile.Text + "\\";
                        else if (TBDatabaseFile.Text.EndsWith("\\\\", StringComparison.OrdinalIgnoreCase))
                            TBDatabaseFile.Text = TBDatabaseFile.Text.Replace("\\\\", "\\");



                        // MessageBox.Show(TBDatabaseFile.Text +DatabaseFileName);
                    }



                }
                ConfigFileRW.DatabaseFile = TBDatabaseFile.Text + DatabaseFileName;

                //RashidPrinterOutputType
                int xps = 0;
                if (RB_XPS.Checked)
                {
                    ConfigFileRW.RashidPrinterOutputType = "XPS";
                    xps = 1;
                }
                else if (RB_XML.Checked)
                {
                    ConfigFileRW.RashidPrinterOutputType = "XML";
                    xps = 0;
                }



                //PrinterOutPath
                if (TBPrinterOutPath.Text == "")
                {
                    MessageBox.Show("Please choose Output Path Folder!", "Folder Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;

                }
                if (!TBPrinterOutPath.Text.EndsWith("\\\\"))
                {
                    TBPrinterOutPath.Text = TBPrinterOutPath.Text.TrimEnd('\\');
                    TBPrinterOutPath.Text += "\\\\";
                }
                ConfigFileRW.PrinterOutPath = TBPrinterOutPath.Text;

                if (!Directory.Exists(ConfigFileRW.PrinterOutPath))
                {
                    // If the folder doesn't exist, create it
                    Directory.CreateDirectory(ConfigFileRW.PrinterOutPath);
                    // Console.WriteLine("Folder created successfully.");
                }


                string path = TBPrinterOutPath.Text.Replace("\\\\", "");
                //Folders
                ConfigFileRW.XMLFolder = ConfigFileRW.XPSFolder = ConfigFileRW.JSONFolder = path;



                //ExecutablePath
                if (TBExecutablePath.Text == "")
                {
                    MessageBox.Show("Please choose Executable!", "Executable Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;

                }
                //if (!TBExecutablePath.Text.StartsWith('\"'))
                //    TBExecutablePath.Text = "\"" + TBExecutablePath.Text;
                //if (!TBExecutablePath.Text.EndsWith('\"'))
                //    TBExecutablePath.Text =  TBExecutablePath.Text + "\"";


                ConfigFileRW.ExecutablePath = TBExecutablePath.Text;
                RashidPrinterConfig(xps, ConfigFileRW.PrinterOutPath);
                ExecutablePathUpdate(ConfigFileRW.ExecutablePath);

                //    ExportAndImport();

                string ret = ConfigFileRW.SaveFromXml(configFilePath);

                if (ret == "Failed")
                    MessageBox.Show("Failed to save Configuration.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Rashid Configurations saved successfully.", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save Configuration.");
            }
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

        private void ChangeRashidPrinterOutputType(int xps)
        {

            string command = XMLPrinterPath;

            string arguments = $"/configure \"outputflags=11100{xps.ToString()}\"";

            RunCommandSilently(command, arguments);


        }

        private void DispatchPrinter()
        {
            string command = XMLPrinterPath;

            string arguments = $"/dispatch";
            RunCommandSilently(command, arguments);




        }

        private void ExecutablePathUpdate(string ExecutablePathStr)
        {
            string command = XMLPrinterPath;

            string arguments = $"/configure \"clearsteps\"";
            RunCommandSilently(command, arguments);


            arguments = $"/configure \"addstep=1,\\\"{ExecutablePathStr}\\\"\"";
            RunCommandSilently(command, arguments);


        }

        private void ExportAndImport()
        {

            string command = XMLPrinterPath;
            string tmpfile = "D:\\setting.dat";

            string arguments = $"/export \"{tmpfile.ToString()}\"";

            RunCommandSilently(command, arguments);

            arguments = $"/import \"{tmpfile.ToString()}\"";

            RunCommandSilently(command, arguments);

        }


        private void RashidPrinterConfig(int xps, string PrinterOutPath)
        {
            string command = XMLPrinterPath;
            string arguments1 = $"/configure ";
            string arguments2 = $" \"outputflags=11100{xps.ToString()}\" ";
            string arguments3 = $" \"outputdir={PrinterOutPath}\" ";

            string arguments = arguments1 + arguments2 + arguments3;
            RunCommandSilently(command, arguments);



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
        private void SetTitle()
        {
            string appName = "Print Settings Tool "; // Replace with your application name
            string version = "Version 1.0.0.2"; // Replace with your version number

            // Measure text width
            using (Graphics g = this.CreateGraphics())
            {
                SizeF appNameSize = g.MeasureString(appName, this.Font);
                SizeF versionSize = g.MeasureString(version, this.Font);

                // Calculate total width of app name and version
                float totalTextWidth = appNameSize.Width + versionSize.Width;

                // Get the width of the title bar (available space)
                int titleBarWidth = this.Width - SystemInformation.FrameBorderSize.Width * 2; // Adjust for window borders

                // Calculate available width for spaces
                float availableWidth = titleBarWidth - totalTextWidth;

                // Create spaces based on available width
                int spaceCount = Math.Max(0, (int)(availableWidth / g.MeasureString(" ", this.Font).Width));
                string spaces = new string(' ', spaceCount);

                // Set the form's title
                this.Text = $"{appName}{spaces}{version}";
            }
        }

        private void PrinterTool_Load(object sender, EventArgs e)
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


            DispatchPrinter();

            if (string.IsNullOrEmpty(configFilePath))
            {
                Console.WriteLine("Configuration file path not found in the registry.");
                return;
            }

            ConfigFileRW.LoadFromXml(configFilePath);





            //DataBase
            if (ConfigFileRW.DatabaseFile != "")
            {

                // Get the directory path
                TBDatabaseFile.Text = Path.GetDirectoryName(ConfigFileRW.DatabaseFile);

                // Get the filename
                DatabaseFileName = Path.GetFileName(ConfigFileRW.DatabaseFile);
                TBDatabaseFile.ForeColor = Color.Black;


                //  MessageBox.Show(DatabaseFileName);
            }

            //RashidPrinterOutputType

            if (ConfigFileRW.RashidPrinterOutputType == "XPS")
                RB_XPS.Checked = true;
            else if (ConfigFileRW.RashidPrinterOutputType == "XML")
                RB_XML.Checked = true;

            if (ConfigFileRW.RasheedFR == true)
                radioBFRYes.Checked = true;
            else if (ConfigFileRW.RasheedFR == false)
                radioBFRNo.Checked = true;


            //PrinterOutPath
            TBPrinterOutPath.Text = ConfigFileRW.PrinterOutPath;
            //Folders
            textBoxPathXML.Text = ConfigFileRW.PrinterOutPath;
            textBoxPathXPS.Text = ConfigFileRW.PrinterOutPath;
            textBoxPathJSON.Text = ConfigFileRW.PrinterOutPath;

            //ExecutablePath
            TBExecutablePath.Text = ConfigFileRW.ExecutablePath;


        }

        private void TBDatabaseFile_GotFocus(object sender, EventArgs e)
        {
            if (TBDatabaseFile.Text == "Enter File path like  c:\\dbfile.db")
            {
                TBDatabaseFile.Text = "";
                TBDatabaseFile.ForeColor = Color.Black;
            }
        }

        private void TBDatabaseFile_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBDatabaseFile.Text))
            {
                TBDatabaseFile.Text = "Enter File path like  c:\\dbfile.db";
                TBDatabaseFile.ForeColor = Color.Gray;
            }
        }

        private void buttonBrowseXML_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    textBoxPathXML.Text = folderBrowserDialog.SelectedPath;
                    ConfigFileRW.XMLFolder = textBoxPathXML.Text;

                    ConfigFileRW.XPSFolder = textBoxPathXPS.Text = folderBrowserDialog.SelectedPath;

                    ConfigFileRW.JSONFolder = textBoxPathJSON.Text = folderBrowserDialog.SelectedPath;

                }
            }
        }

        private void buttonBrowseXPS_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    textBoxPathXPS.Text = folderBrowserDialog.SelectedPath;
                    ConfigFileRW.XPSFolder = textBoxPathXPS.Text;
                }
            }
        }

        private void buttonBrowseJSON_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    textBoxPathJSON.Text = folderBrowserDialog.SelectedPath;
                    ConfigFileRW.JSONFolder = textBoxPathJSON.Text;
                }
            }

        }

        private void RB_XML_CheckedChanged(object sender, EventArgs e)
        {
            ConfigFileRW.RashidPrinterOutputType = "XML";

        }

        private void RB_XPS_CheckedChanged(object sender, EventArgs e)
        {
            ConfigFileRW.RashidPrinterOutputType = "XPS";


        }

        private void ExecutablePathBtn_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "exe files (*.exe)|*.exe|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    TBExecutablePath.Text = openFileDialog.FileName;
                    ConfigFileRW.ExecutablePath = TBExecutablePath.Text;

                }
            }
        }

        private void PrinterOutPathBtn_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (TBPrinterOutPath.Text != null)
                    folderBrowserDialog.SelectedPath = TBPrinterOutPath.Text;
                DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    TBPrinterOutPath.Text = folderBrowserDialog.SelectedPath;
                    ConfigFileRW.JSONFolder = TBPrinterOutPath.Text;


                    ConfigFileRW.XMLFolder = textBoxPathXML.Text = folderBrowserDialog.SelectedPath;

                    ConfigFileRW.XPSFolder = textBoxPathXPS.Text = folderBrowserDialog.SelectedPath;

                    ConfigFileRW.JSONFolder = textBoxPathJSON.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void DBBrowse_Click(object sender, EventArgs e)
        {

            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (TBPrinterOutPath.Text != null)
                    folderBrowserDialog.SelectedPath = TBPrinterOutPath.Text;
                DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    TBDatabaseFile.Text = folderBrowserDialog.SelectedPath;
                    TBDatabaseFile.ForeColor = Color.Black;
                    ConfigFileRW.DatabaseFile = TBDatabaseFile.Text;

                }

            }
        }

        private void ReSetBtn_Click(object sender, EventArgs e)
        {
            DispatchPrinter();
            System.Threading.Thread.Sleep(100);
            MessageBox.Show("Reset Succeed", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void radioBFRYes_CheckedChanged(object sender, EventArgs e)
        {
            ConfigFileRW.RasheedFR = true;
        }

        private void radioBFRNo_CheckedChanged(object sender, EventArgs e)
        {
            ConfigFileRW.RasheedFR = false;
        }
    }

}

