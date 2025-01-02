using System.Diagnostics;
using System.Security.Principal;
using Microsoft.Win32;
using System.Xml;
using RasheedTag;
using Newtonsoft.Json;

namespace TerminalDesktopSilence
{
    public partial class SilenceTerminal : Form
    {
        static string FileToReadOrPrint = "";
        static string FileToReadOrPrintXML = "";
        static string outputFileJsonPath = "";
        static string sPrinterName = "";
        static string XMLPrinterPath = "";

        static string FullJsonStr = "";


        private UseRasheedTag URasheedTag;
        private Tag TagObj;

        public SilenceTerminal()
        {
            InitializeComponent();
            this.Opacity = 0;        // Make it invisible
            this.WindowState = FormWindowState.Minimized; // Minimize it
            this.ShowInTaskbar = false; // Remove it from the taskbar
        }

        public static void CloseRunningInstance()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

            }

        }
        static bool IsRunningAsAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);
            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }

        static void RunAsAdministrator(string arguments)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = Application.ExecutablePath, // Path to your executable
                Arguments = arguments,
                UseShellExecute = true,
                CreateNoWindow = true,
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
        private void Form1_Load(object sender, EventArgs e)
        {
            // Close any running instance of the app
            CloseRunningInstance();

            try
            {


#if !DEBUG
                if (!IsRunningAsAdministrator())
                {

                     
                    string[] args2 = Environment.GetCommandLineArgs();
                    string arguments = string.Join(" ", args2.Skip(1).Select(arg => $"\"{arg}\""));


                    //  MessageBox.Show("arguments");

                    // Restart the application with elevated privileges
                    RunAsAdministrator(arguments);
                    //  MessageBox.Show("Test3");
                    this.Close();
                }


                Console.WriteLine("Application is running with administrator privileges.");

#endif
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString() + ex.ToString());
                this.Close();
            }


            
          
           

            
            string registryAppKey = @"SOFTWARE\Rasheed";
            string registryCPName = "ConfigPath";
            string registryPPValue = "PrinterPath";


            GlobalVariables.configFilePath = ReadRegistryValue(registryAppKey, registryCPName);
            XMLPrinterPath = ReadRegistryValue(registryAppKey, registryPPValue);

           
            if (string.IsNullOrEmpty(GlobalVariables.configFilePath))
            {

                MessageBox.Show("Configuration file path not found in the registry.");

                this.Close();
            }


            Configuration.LoadFromXml(GlobalVariables.configFilePath);




            GlobalVariables.LogInFile("Load Config file :: " + GlobalVariables.configFilePath);



            if (Configuration.XMLFolder == "")
            {
                MessageBox.Show("Error Message :: Please prepare the setting from PrinterSettingsTool First");
                this.Close();
            }

            GlobalVariables.TmpRFFailedCounter = Configuration.RFFailedCounter;
            GlobalVariables.LogInFile("FailedCounter from Config file :: " + GlobalVariables.TmpRFFailedCounter);


            string[] args = Environment.GetCommandLineArgs();

            int index = args.Length;

            // Check if there are any arguments passed to the application
            if (args.Length > 1)
            {
                //  Console.WriteLine("Arguments provided:");




                if (args.Length >= 1 && args[1] != null)
                {
                    FileToReadOrPrint = args[1];
                    if (FileToReadOrPrint != "")
                    {
                        GlobalVariables.LogInFile("Start New Print with File :: " + FileToReadOrPrint);

                    }


                }
            }
            else if (FileToReadOrPrint == "")
            {

#if !DEBUG
                MessageBox.Show("There is no file to print", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();

#endif


#if DEBUG



                FileToReadOrPrint = Configuration.XMLFolder + "\\0001.xps";
                Console.WriteLine($"Set FileToReadOrPrint to {FileToReadOrPrint} beacace No arguments provided.");


#endif
            }



            CheckXPSFileCreateXML(FileToReadOrPrint);
            GlobalVariables.LogInFile("CreateXML File :: " + FileToReadOrPrintXML);



            string DbRet = BillsChainLite.CreateDatabase(GlobalVariables.configFilePath);
            GlobalVariables.LogInFile(DbRet);



            string selectedPrinter = Configuration.selectedPrinter ?? "RashidPrinter";
            if (selectedPrinter == "BothPrinterSilent")
            {
                GlobalVariables.LogInFile("Case BothPrinterSilent WithoutInterface ");
                BothPWithoutInterface();

            }
            else if (selectedPrinter == "BothPrinter")
            {
                GlobalVariables.LogInFile("Case Both Printers WithoutInterface ");
                BothPWithoutInterface();
            }
           


        }
        private void RashidPWithoutInterface()
        {

            CheckBalanceAndCreateJson();

        }
        private void PhysicalPWithoutInterface()
        {
            try
            {

                if (FileToReadOrPrint == "" || !FileToReadOrPrint.EndsWith("xps", StringComparison.OrdinalIgnoreCase))
                {

                    Console.WriteLine("Error Message ::: There is no file to print");
                    GlobalVariables.LogInFile("Error Message ::: There is no file to print");
                    return;
                }

                if (Configuration.DefaultPhyPrinter == "")
                {
                    Console.WriteLine("Error Message :: Please choose a physical printer first.");
                    return;
                }

                if (sPrinterName == "" && Configuration.DefaultPhyPrinter != null)
                {

                    sPrinterName = Configuration.DefaultPhyPrinter;

                    GlobalVariables.LogInFile("Print xps  :: " + FileToReadOrPrint + " With :: " + sPrinterName);
                    PrintXpsFile(FileToReadOrPrint, sPrinterName);


                }



            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private void BothPWithoutInterface()
        {
            PhysicalPWithoutInterface();
            CheckBalanceAndCreateJson();

        }
        public void RunCommandSilently(string command, string arguments)
        {
            try
            {
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    CreateNoWindow = true,
                    UseShellExecute = false, // Set to true to use the OS shell
                    Verb = "runas", // This will prompt for elevation
                    RedirectStandardOutput = false, // No need to redirect output if using runas
                    RedirectStandardError = false
                };

                using (Process process = new Process())
                {
                    process.StartInfo = processInfo;
                    process.Start();

                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        private void CheckXPSFileCreateXML(string InputFileName)
        {
            GlobalVariables.LogInFile("In CheckXPSFileCreateXML with " + InputFileName);

            if (InputFileName.EndsWith("xps", StringComparison.OrdinalIgnoreCase))
            {
                FileToReadOrPrintXML = InputFileName.Replace(".xps", ".xml");

                string command = XMLPrinterPath;
                string arguments = @"/convert " + " \"" + InputFileName + "\"  \"" + FileToReadOrPrintXML + "\" ";

                RunCommandSilently(command, arguments);

                GlobalVariables.LogInFile("Convert xps to xml File :: " + FileToReadOrPrintXML);
            }
        }
        string ReadRegistryValue(string ConfigkeyPath, string valueName)
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
                            return o.ToString() ?? "";
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
        private string XMLPrintReadBill()
        {

            if (FileToReadOrPrintXML != "")
            {
                string decompressed = XMLPrinter.ConvertXML(FileToReadOrPrintXML);

                outputFileJsonPath = FileToReadOrPrint + ".json";

                try
                {
                    // Serialize the string to JSON and write it to the file
                    File.WriteAllText(outputFileJsonPath, decompressed);
                    return decompressed;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return "";
                }
            }
            else
                return "";
        }
        public void PrintXpsFile(string xpsFilePath, string printerName)
        {

            string command = XMLPrinterPath;
            string arguments = $"/reprint \"{xpsFilePath}\" \"{printerName}\"";



            RunCommandSilently(command, arguments);
        }
        private int FindIfPrintedBefore(string billstr)
        {
            try
            {
                // Console.WriteLine("in FindIfPrintedBefore with " + billstr);
                Invoice jsonObj = new Invoice();
                jsonObj = JsonConvert.DeserializeObject<Invoice>(billstr);




                if (BillsChainLite.CheckIfPrintedBefore(jsonObj.invoiceNo.ToString()))
                    return 1;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                GlobalVariables.LogInFile(ex.Message);
                return -1;
            }



        }
        private void AddToDailyArchive(string billstr, bool already_Found)
        {
            try
            {


                string DailyArchivePath = Configuration.XMLFolder + "\\DailyFilesArchive.xml";

                // Create XML file if it does not exist
                if (!File.Exists(DailyArchivePath))
                {
                    XmlDocument doc = new XmlDocument();
                    XmlNode declarationNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    doc.AppendChild(declarationNode);

                    XmlNode rootNode = doc.CreateElement("DailyFilesArchive");
                    doc.AppendChild(rootNode);

                    doc.Save(DailyArchivePath);
                }

                if (File.Exists(DailyArchivePath))
                {
                    //parse json
                    Invoice jsonObj = new Invoice();
                    jsonObj = JsonConvert.DeserializeObject<Invoice>(billstr);



                    // Update the XML file with new field
                    XmlDocument doc = new XmlDocument();
                    doc.Load(DailyArchivePath);

                    XmlNode root = doc.DocumentElement;

                    XmlNode fieldNode = doc.CreateElement("Files");


                    XmlNode InvoiceNumNode = doc.CreateElement("InvoiceNum");
                    InvoiceNumNode.InnerText = jsonObj.invoiceNo.ToString();
                    fieldNode.AppendChild(InvoiceNumNode);

                    XmlNode InvoiceDateNode = doc.CreateElement("InvoiceDate");
                    InvoiceDateNode.InnerText = jsonObj.transactionDateTime.ToString();
                    fieldNode.AppendChild(InvoiceDateNode);

                    XmlNode InvoicePriceNode = doc.CreateElement("InvoicePrice");
                    InvoicePriceNode.InnerText = jsonObj.totalAmount.ToString();
                    fieldNode.AppendChild(InvoicePriceNode);


                    XmlNode xmlFileNode = doc.CreateElement("XmlFile");
                    xmlFileNode.InnerText = FileToReadOrPrintXML;
                    fieldNode.AppendChild(xmlFileNode);

                    XmlNode xpsFileNode = doc.CreateElement("XpsFile");
                    xpsFileNode.InnerText = FileToReadOrPrint;
                    fieldNode.AppendChild(xpsFileNode);



                    XmlNode alreadyPrinted = doc.CreateElement("AlreadyPrinted");
                    alreadyPrinted.InnerText = already_Found.ToString();
                    fieldNode.AppendChild(alreadyPrinted);


                    root.AppendChild(fieldNode);
                    doc.Save(DailyArchivePath);
                }
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }



        }
        private void UpdateRF(bool FastRead)
        {
            try
            {
                XmlDocument xmlDoc1 = new XmlDocument();
                using (XmlReader reader = XmlReader.Create(GlobalVariables.configFilePath))
                {

                    xmlDoc1.Load(reader); // Load the XML file
                    XmlNode configNode = xmlDoc1.SelectSingleNode("Configuration");

                    if (configNode != null)
                    {
                        configNode.SelectSingleNode("FastRead").InnerText = FastRead.ToString();
                    }


                }

                xmlDoc1.Save(GlobalVariables.configFilePath);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to UpdateFailedCounter: " + ex.StackTrace);

            }

        }
        public void UpdateRFFailedCounter()
        {
            try
            {
                XmlDocument xmlDoc1 = new XmlDocument();
                using (XmlReader reader = XmlReader.Create(GlobalVariables.configFilePath))
                {

                    xmlDoc1.Load(reader); // Load the XML file
                    XmlNode configNode = xmlDoc1.SelectSingleNode("Configuration");

                    if (configNode != null)
                    {
                        configNode.SelectSingleNode("RFFailedCounter").InnerText = GlobalVariables.TmpRFFailedCounter.ToString();
                    }


                }

                xmlDoc1.Save(GlobalVariables.configFilePath);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to UpdateFailedCounter: " + ex.StackTrace);

            }

        }
        public void UpdateXmlFolder()
        {
            try
            {
                XmlDocument xmlDoc1 = new XmlDocument();
                using (XmlReader reader = XmlReader.Create(GlobalVariables.configFilePath))
                {

                    xmlDoc1.Load(reader); // Load the XML file
                    XmlNode configNode = xmlDoc1.SelectSingleNode("Configuration");

                    if (configNode != null)
                    {
                        configNode.SelectSingleNode("XMLFolder").InnerText = GlobalVariables.defaultOutFolderPath;
                        configNode.SelectSingleNode("XPSFolder").InnerText = GlobalVariables.defaultOutFolderPath;
                        configNode.SelectSingleNode("JSONFolder").InnerText = GlobalVariables.defaultOutFolderPath;

                        Configuration.JsonFilePath = Configuration.XMLFolder = GlobalVariables.defaultOutFolderPath;





                        Random random = new Random();
                        int randomNumber = random.Next(0, 10);
                        DateTime now = DateTime.Now;
                        string DatabaseFileName = GlobalVariables.defaultOutFolderPath + "\\R" + randomNumber.ToString() + "_" + now.ToString("ddHHmm") + ".db";
                        configNode.SelectSingleNode("DatabaseFile").InnerText = DatabaseFileName;
                        Configuration.dbFilePath = DatabaseFileName;

                        string PrinterOutPath = GlobalVariables.defaultOutFolderPath + "\\\\";
                        configNode.SelectSingleNode("PrinterOutPath").InnerText = GlobalVariables.defaultOutFolderPath + "\\\\";
                        //RashidPrinterConfig(PrinterOutPath);





                    }


                }

                xmlDoc1.Save(GlobalVariables.configFilePath);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to UpdateXmlFolder: " + ex.StackTrace);

            }

        }

        private void CheckBalanceAndCreateJson()
        {
            var result = BillsChainLite.GetCurrCredit();
            if (result.HasValue)
            {
                int Credit = result.Value.CurrentBalance;
                string status = result.Value.CurrStatus;
                string PrevSignature = result.Value.Signature;

                if (status == "Signature_NotCorrect")
                {
                    GlobalVariables.LogInFile("Print Error ::: Signature NotCorrect ");
                    Console.WriteLine("Error Message :: Data is corrupted. Please contact support for assistance.");
                    return;
                }

                if (status == "Database_Error")
                {
                    GlobalVariables.LogInFile("Print Error ::: Database Error ");
                    Console.WriteLine("Error Message :: Unable to access the database. Please try again later or contact support for assistance.");
                    return;
                }



                if (Credit == 0 && status == "Signature_Correct")
                {
                    GlobalVariables.LogInFile("Print Error ::: Balance is zero ");
                    Console.WriteLine("Error Message :: Balance is zero. Please recharge your balance to continue using our services.");
                    return;
                }

                else if (Credit > 0 && status == "Signature_Correct")  //print the bill and update balance
                {

                    GlobalVariables.LogInFile("Start Print JSON File ");
                    //
                    //build json
                    //  Console.WriteLine("FileToReadOrPrintXML ::" + FileToReadOrPrintXML);


                    string billstr = XMLPrintReadBill();
                    // GlobalVariables.LogInFile("Print Error ::: " +billstr );


                    if (billstr == "")
                        Console.WriteLine("Error Message :: Filed to parse the bill");


                    int already_Found = FindIfPrintedBefore(billstr);

                    if (already_Found == -1)
                    {
                        Console.WriteLine("Error Message :: Filed to read the bill");
                        return;
                    }

                    bool retValue = false;
                    if (already_Found == 1)
                    {
                        retValue = BillsChainLite.UpdateCredit(Credit, billstr, PrevSignature); //if its found before save the same balance
                        GlobalVariables.LogInFile("Add Database Record without update balance because File Printed Before ");
                        AddToDailyArchive(billstr, true);
                        GlobalVariables.LogInFile("Add File To DailyArchive");
                    }
                    else if (already_Found == 0)
                    {
                        retValue = BillsChainLite.UpdateCredit(Credit - 1, billstr, PrevSignature); //if its new bill update balance
                        GlobalVariables.LogInFile("Add Database Record and  update balance ");
                        AddToDailyArchive(billstr, false);
                        GlobalVariables.LogInFile("Add File To DailyArchive");
                    }









                    if (retValue)
                    {

                        //    Console.WriteLine("Success Message ::: Invoice printing completed successfully and Rasheed NFC start sending data.");
                        MessageBox.Show(this ,"Invoice printing completed successfully and Rasheed NFC start sending data.", "Success Message");

                        GlobalVariables.LogInFile("Invoice printing completed successfully ");
                        GlobalVariables.LogInFile("Rasheed NFC start sending data.");



                        //  byte[] compressed = XMLPrinter.Compress(billstr);
                        //  int size = compressed.Length;
                        //  Console.WriteLine($"The size of the UTF-8 byte array is: {size} bytes");


                        byte[] compressed = XMLPrinter.Compress(billstr);



                        GlobalVariables.LogInFile("Create New Rasheed Tag  with FRFlag = " + Configuration.RasheedFR.ToString() + " and Wait to mobile response ");


                        if (URasheedTag != null)
                            URasheedTag.ReleaseTag(TagObj);


                        URasheedTag = new UseRasheedTag();


                        TagObj = URasheedTag.CreateTag(compressed, this);




                    }

                    else
                    {

                        MessageBox.Show(this, "Invoice printing failed. You can try again later", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("Error Message::: Invoice printing failed. You can try again later");
                        GlobalVariables.LogInFile("Invoice printing failed");
                    }


                }
                else if (Credit == 0 && status == "No_Data") //check recharged balance then print the bill and add the first record 
                {

                    ////////////////
                    GlobalVariables.LogInFile("First Time To Print Set Balance to 1000");
                    GlobalVariables.LogInFile("Start Print JSON File ");
                    int NewBalance = 1000;

                    string billstr = XMLPrintReadBill();
                    byte[] compressed = XMLPrinter.Compress(billstr);


                    bool retValue = BillsChainLite.UpdateCredit(NewBalance, billstr, "");

                    GlobalVariables.LogInFile("Add Database Record and  update balance ");



                    AddToDailyArchive(billstr, false);
                    GlobalVariables.LogInFile("Add File To DailyArchive");


                    if (retValue)
                    {

                        MessageBox.Show(this , "Invoice printing completed successfully and Rasheed NFC start sending data.", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        GlobalVariables.LogInFile("Invoice printing completed successfully ");
                        GlobalVariables.LogInFile("Rasheed NFC start sending data.");




                        GlobalVariables.LogInFile("Create New Rasheed Tag  with FRFlag = " + Configuration.RasheedFR.ToString() + " and Wait to mobile response ");

                        if (URasheedTag != null)
                            URasheedTag.ReleaseTag(TagObj);


                        URasheedTag = new UseRasheedTag();


                         URasheedTag.CreateTag(compressed, this);



                    }
                    else
                    {

                        MessageBox.Show(this , "Invoice printing failed. You can try again later", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("Error Message:::  Invoice printing failed. You can try again later");
                        GlobalVariables.LogInFile("Invoice printing failed");
                    }

                }


            }


        }

        public void DispatchPrinter()
        {
            string command = XMLPrinterPath;

            string arguments = $"/dispatch";
            RunCommandSilently(command, arguments);


        }
        public void TerminalClose()
        {


            DispatchPrinter();
            GlobalVariables.LogInFile("Dispatch XML Printer");

            if (URasheedTag != null)
                URasheedTag.ReleaseTag(TagObj);

            this.Close();
        }
        private void SilenceTerminal_FormClosing(object sender, FormClosingEventArgs e)
        {

            //UpdateRFFailedCounter();

            DispatchPrinter();
            GlobalVariables.LogInFile("Dispatch XML Printer");

            if (URasheedTag != null)
                URasheedTag.ReleaseTag(TagObj);

        }
    }
}
