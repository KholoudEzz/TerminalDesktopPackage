using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Xml;


using RasheedTag;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Principal;
using System.Text.Json.Serialization;
using System.Runtime.InteropServices;

namespace TerminalDesktopApp
{
    public static class GlobalVariables
    {
        //  global variables here
        public static int Version = 18;
        public static string defaultOutFolderPath = "Output";
        public static string defaultExe = "Tools\\TerminalDesktop.exe";
        static public int TmpRFFailedCounter = 0;
        public static string configFilePath = "";



        static public UseRasheedTag URasheedTag;
        static public Tag TagObj;

        public static void ConsoleRelease()
        {
            LogInFile("Console is Release");
            if (GlobalVariables.URasheedTag != null)
                GlobalVariables.URasheedTag.ReleaseTag();

          //  Program.UpdateRFFailedCounter();
            Program.DispatchPrinter();
            LogInFile("Dispatch XML Printer in Console is Release");

          


        }
        public static void LogInFile(string LogStr)
        {
            if (string.IsNullOrWhiteSpace(LogStr))
            {
                return;
            }

            try
            {
                // Append the log entry to the file
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} :TerminalVer{Version.ToString()}: {LogStr}{Environment.NewLine}";
                string currentDate = DateTime.Now.ToString("yyyyMMdd");
                string logFilePath = Configuration.XMLFolder + "\\LogFile" +currentDate+".txt";

                // Ensure the directory exists
                string directory = Path.GetDirectoryName(logFilePath) ?? "";
                if (Directory.Exists(directory))
                {
                    // Open the file with FileStream and allow multiple processes to write simultaneously
                    using (FileStream fs = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(logEntry);
                        // Console.WriteLine(logEntry);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logging failed: {ex.Message}");

            }
        }
    }


    static class Program
    {
        static string FileToReadOrPrint = "";
        static string FileToReadOrPrintXML = "";
        static string outputFileJsonPath = "";
        static string sPrinterName = "";
        static string XMLPrinterPath = "";

        static string FullJsonStr = "";


        // Delegate to define the signature of the callback
        private delegate bool ConsoleCtrlDelegate(CtrlTypes ctrlType);

        // Enum for the control signal types
        private enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        // Import SetConsoleCtrlHandler from kernel32.dll
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate handler, bool add);


        public static void RunCommandSilently(string command, string arguments)
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
        static private void CheckXPSFileCreateXML(string InputFileName)
        {
            GlobalVariables.LogInFile("In CheckXPSFileCreateXML with " + InputFileName);

            if (InputFileName.EndsWith("xps", StringComparison.OrdinalIgnoreCase))
            {
                FileToReadOrPrintXML = InputFileName.Replace(".xps", ".xml");

                string command = XMLPrinterPath;
                string arguments = @"/convert " + " \"" + InputFileName + "\"  \"" + FileToReadOrPrintXML + "\" ";

                RunCommandSilently(command, arguments);

                 GlobalVariables. LogInFile("Convert xps to xml File :: " + FileToReadOrPrintXML);
            }
        }
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
        static private string XMLPrintReadBill()
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
        static public void PrintXpsFile(string xpsFilePath, string printerName)
        {

            string command = XMLPrinterPath;
            string arguments = $"/reprint \"{xpsFilePath}\" \"{printerName}\"";



            RunCommandSilently(command, arguments);
        }
        static private int FindIfPrintedBefore(string billstr)
        {
            try
            {
               // Console.WriteLine("in FindIfPrintedBefore with " + billstr);
                Invoice jsonObj = new Invoice();
                jsonObj  = JsonConvert.DeserializeObject<Invoice>(billstr);




                if ( BillsChainLite.CheckIfPrintedBefore(jsonObj.invoiceNo.ToString()))
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
        static private void AddToDailyArchive(string billstr, bool already_Found)
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
        static private void UpdateRF(bool FastRead)
        {
            try
            {
                XmlDocument xmlDoc1 = new XmlDocument();
                using (XmlReader reader = XmlReader.Create( GlobalVariables. configFilePath))
                {

                    xmlDoc1.Load(reader); // Load the XML file
                    XmlNode configNode = xmlDoc1.SelectSingleNode("Configuration");

                    if (configNode != null)
                    {
                        configNode.SelectSingleNode("FastRead").InnerText = FastRead.ToString();
                    }


                }

                xmlDoc1.Save( GlobalVariables. configFilePath);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to UpdateFailedCounter: " + ex.StackTrace);

            }

        }
        public static void UpdateRFFailedCounter()
        {
            try
            {
                XmlDocument xmlDoc1 = new XmlDocument();
                using (XmlReader reader = XmlReader.Create( GlobalVariables. configFilePath))
                {

                    xmlDoc1.Load(reader); // Load the XML file
                    XmlNode configNode = xmlDoc1.SelectSingleNode("Configuration");

                    if (configNode != null)
                    {
                        configNode.SelectSingleNode("RFFailedCounter").InnerText =  GlobalVariables. TmpRFFailedCounter.ToString();
                    }


                }

                xmlDoc1.Save( GlobalVariables. configFilePath);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to UpdateFailedCounter: " + ex.StackTrace);

            }

        }
       public static void UpdateXmlFolder()
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

        static private void CheckBalanceAndCreateJson()
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
                     GlobalVariables. LogInFile("Print Error ::: Database Error ");
                    Console.WriteLine("Error Message :: Unable to access the database. Please try again later or contact support for assistance.");
                    return;
                }



                if (Credit == 0 && status == "Signature_Correct")
                {
                     GlobalVariables. LogInFile("Print Error ::: Balance is zero ");
                    Console.WriteLine("Error Message :: Balance is zero. Please recharge your balance to continue using our services.");
                    return;
                }

                else if (Credit > 0 && status == "Signature_Correct")  //print the bill and update balance
                {

                     GlobalVariables. LogInFile("Start Print JSON File ");
                    //
                    //build json
                  //  Console.WriteLine("FileToReadOrPrintXML ::" + FileToReadOrPrintXML);
                   

                    string billstr = XMLPrintReadBill();
                   // GlobalVariables.LogInFile("Print Error ::: " +billstr );

                    
                    if (billstr == "")
                        Console.WriteLine("Error Message :: Filed to parse the bill");


                    int  already_Found = FindIfPrintedBefore(billstr);

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

                        Console.WriteLine("Success Message ::: Invoice printing completed successfully and Rasheed NFC start sending data.");

                         GlobalVariables. LogInFile("Invoice printing completed successfully ");
                         GlobalVariables. LogInFile("Rasheed NFC start sending data.");



                        //  byte[] compressed = XMLPrinter.Compress(billstr);
                        //  int size = compressed.Length;
                        //  Console.WriteLine($"The size of the UTF-8 byte array is: {size} bytes");


                        byte[] compressed = XMLPrinter.Compress(billstr);
                        // int size = compressed.Length;
                        //   Console.WriteLine($"The size of the UTF-8 byte array is: {size} bytes");

                        //File.WriteAllBytes("D:\\JsonBytes", compressed);
                      //  byte[] compressed = File.ReadAllBytes("F:\\SimpleTerminal\\Rasheed-Desktop-2024-11-20-2035\\Rasheed-Desktop\\JsonBytes");
                      //  byte[] compressed3 = File.ReadAllBytes("F:\\SimpleTerminal\\Rasheed-Desktop-2024-11-20-2035\\Rasheed-Desktop\\JsonBytes");
                     //   byte[] compressed2 = File.ReadAllBytes("D:\\JsonBytes2");

                        //string json = XMLPrinter.Decompress(compressed);


                      /////  string json1 = XMLPrinter.Decompress(compressed1);
                      //  string json2 = XMLPrinter.Decompress(compressed2);
                        //Console.WriteLine(json);
                        //GlobalVariables.LogInFile( json);


                     //   Console.WriteLine(billstr + json1);
                     //   GlobalVariables.LogInFile(json1);


                     
                        //File.WriteAllBytes("D:\\JsonBytes_WithQR", compressed);

                        GlobalVariables. LogInFile("Create New Rasheed Tag  with FRFlag = " + Configuration.RasheedFR.ToString() + " and Wait to mobile response ");

                        if (GlobalVariables.URasheedTag != null)
                            GlobalVariables.URasheedTag.ReleaseTag();


                        GlobalVariables.URasheedTag = new UseRasheedTag();

                        if (Configuration.RFFailedCounter >= 2 && Configuration.RasheedFR == true)
                        {
                            // UpdateRF(false);
                            GlobalVariables.LogInFile("Try Without FastRead");
                            GlobalVariables.URasheedTag.CreateTag(compressed, false);
                            
                        }
                        else
                            GlobalVariables.URasheedTag.CreateTag(compressed, Configuration.RasheedFR);



         
                    }

                    else
                    {
                        Console.WriteLine("Error Message::: Invoice printing failed. You can try again later");
                         GlobalVariables. LogInFile("Invoice printing failed");
                    }


                }
                else if (Credit == 0 && status == "No_Data") //check recharged balance then print the bill and add the first record 
                {
                    
                    ////////////////
                     GlobalVariables. LogInFile("First Time To Print Set Balance to 1000");
                     GlobalVariables. LogInFile("Start Print JSON File ");
                    int NewBalance = 1000;

                    string billstr = XMLPrintReadBill();
                    byte[] compressed = XMLPrinter.Compress(billstr);


                    bool retValue = BillsChainLite.UpdateCredit(NewBalance, billstr, "");

                     GlobalVariables. LogInFile("Add Database Record and  update balance ");



                    AddToDailyArchive(billstr, false);
                     GlobalVariables. LogInFile("Add File To DailyArchive");


                    if (retValue)
                    {
                        Console.WriteLine("Success Message :::Invoice printing completed successfully and Rasheed NFC start sending data.");
                         GlobalVariables. LogInFile("Invoice printing completed successfully ");
                         GlobalVariables. LogInFile("Rasheed NFC start sending data.");




                        GlobalVariables.LogInFile("Create New Rasheed Tag  with FRFlag = " + Configuration.RasheedFR.ToString() + " and Wait to mobile response ");

                        if (GlobalVariables.URasheedTag != null)
                            GlobalVariables.URasheedTag.ReleaseTag();


                        GlobalVariables.URasheedTag = new UseRasheedTag();


                        if (Configuration.RFFailedCounter >= 2 && Configuration.RasheedFR == true)
                        {
                            // UpdateRF(false);
                            GlobalVariables.LogInFile("Try Without FastRead");
                            GlobalVariables.URasheedTag.CreateTag(compressed, false);

                        }
                        else
                            GlobalVariables.URasheedTag.CreateTag(compressed, Configuration.RasheedFR);



                    }
                    else
                    {
                        Console.WriteLine("Error Message:::  Invoice printing failed. You can try again later");
                         GlobalVariables. LogInFile("Invoice printing failed");
                    }

                }


            }


        }

        static private void RashidPWithoutInterface()
        {

            CheckBalanceAndCreateJson();

        }
        static private void PhysicalPWithoutInterface()
        {
            try
            {

                if (FileToReadOrPrint == "" || !FileToReadOrPrint.EndsWith("xps", StringComparison.OrdinalIgnoreCase))
                {

                    Console.WriteLine("Error Message ::: There is no file to print");
                     GlobalVariables. LogInFile("Error Message ::: There is no file to print");
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

                     GlobalVariables. LogInFile("Print xps  :: " + FileToReadOrPrint + " With :: " + sPrinterName);
                    PrintXpsFile(FileToReadOrPrint, sPrinterName);


                }



            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        static private void BothPWithoutInterface()
        {
            PhysicalPWithoutInterface();
            CheckBalanceAndCreateJson();

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
                FileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TerminalDesktop.exe"),
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = true, // Set to true to use the OS shell
                Verb = "runas", // This will prompt for elevation
              
            };

            try
            {
                Process.Start(processInfo);
                Console.WriteLine("Application restarted with administrator privileges.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to restart the application with administrator privileges: {ex.Message}");
            }

            Thread.Sleep(1000);
            // Close the current instance of the application
           Environment.Exit(0);



                }
        public static void CloseRunningInstance()
        {
            try
            {
                // Get the current process name
                string currentProcessName = Process.GetCurrentProcess().ProcessName;
                //Console.WriteLine("Current Process: " + currentProcessName + " with ID: " + Process.GetCurrentProcess().Id);

                // Get all processes with the same name
                Process[] processes = Process.GetProcessesByName(currentProcessName);

                // Terminate all other instances of the application
                foreach (Process process in processes)
                {
                    // Ensure not to terminate the current process
                    if (process.Id != Process.GetCurrentProcess().Id)
                    {
                        Console.WriteLine("Terminating process with ID: " + process.Id);
                        process.Kill(); // Terminate the process
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static public void DispatchPrinter()
        {
            string command = XMLPrinterPath;

            string arguments = $"/dispatch";
            RunCommandSilently(command, arguments);


        }


        private static bool OnConsoleClose(CtrlTypes ctrlType)
        {
            if (ctrlType == CtrlTypes.CTRL_CLOSE_EVENT)
            {
                GlobalVariables.LogInFile ("Console is closing. CTRL_CLOSE_EVENT.");
                if (GlobalVariables.URasheedTag != null)
                    GlobalVariables.URasheedTag.ReleaseTag();

               // UpdateRFFailedCounter();
                DispatchPrinter();
                GlobalVariables.LogInFile("Dispatch XML Printer");
            }
            return true; // Return true to indicate the signal was handled
        }



        // Main method is the entry point of the application
        static void Main(string[] args)
        {

            // Register the handler
            SetConsoleCtrlHandler(OnConsoleClose, true);


            // Display a welcome message to the user
            Console.WriteLine("Hello, welcome to Terminal DesktopApp !");

            // Close any running instance of the app
            CloseRunningInstance();

            try
            {


#if !DEBUG
                if (!IsRunningAsAdministrator())
                {
                    Console.WriteLine("Restarting application as administrator...");
                    RunAsAdministrator(string.Join(" ", args));
                    return;
                }

                Console.WriteLine("Application is running with administrator privileges.");

#endif
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString() + ex.ToString());
                Console.Read();
                return;
            }





           // Console.WriteLine("Strat Read registry");


            string registryAppKey = @"SOFTWARE\Rasheed";
            string registryCPName = "ConfigPath";
            string registryPPValue = "PrinterPath";


            GlobalVariables.configFilePath = ReadRegistryValue(registryAppKey, registryCPName);
            XMLPrinterPath = ReadRegistryValue(registryAppKey, registryPPValue);


            if (string.IsNullOrEmpty(GlobalVariables.configFilePath))
            {

                Console.WriteLine("Configuration file path not found in the registry.");
                Console.Read();
                return;
            }


            Configuration.LoadFromXml(GlobalVariables.configFilePath);




            GlobalVariables.LogInFile("Load Config file :: " + GlobalVariables.configFilePath);



            if (Configuration.XMLFolder == "")
            {
                Console.WriteLine("Error Message :: Please prepare the setting from PrinterSettingsTool First");
                return;
            }
              /*  
               {string basePath = Directory.GetParent(Directory.GetParent(GlobalVariables.configFilePath).FullName).FullName;
                if (!basePath.EndsWith("\\"))
                {
                    basePath += "\\";
                }
                GlobalVariables.defaultOutFolderPath = basePath + GlobalVariables.defaultOutFolderPath;
                GlobalVariables.LogInFile($"CreateDirectory :: {GlobalVariables.defaultOutFolderPath}");
                if (!Directory.Exists(GlobalVariables.defaultOutFolderPath))
                {
                    Directory.CreateDirectory(GlobalVariables.defaultOutFolderPath);
                   
                    if (Directory.Exists(GlobalVariables.defaultOutFolderPath))
                    {
                        Configuration.XMLFolder = GlobalVariables.defaultOutFolderPath;
                        UpdateXmlFolder();
                    }
                    else
                    {
              

                        Console.WriteLine("Error Message :: Please prepare the setting from PrinterSettingsTool First");
                        return;
                    }
                }
                else
                {
                    Configuration.XMLFolder = GlobalVariables.defaultOutFolderPath;
                    UpdateXmlFolder();

                }
              }
              */


            

            GlobalVariables.TmpRFFailedCounter = Configuration.RFFailedCounter;
            GlobalVariables.LogInFile("FailedCounter from Config file :: " + GlobalVariables.TmpRFFailedCounter);


                // Check if there are any arguments passed to the application
            if (args.Length > 0)
            {
                //  Console.WriteLine("Arguments provided:");



              
                if (args.Length >= 1 && args[0]!= null)
                {
                    FileToReadOrPrint = args[0];
                    if (FileToReadOrPrint != "")
                    {
                        GlobalVariables.LogInFile("Start New Print with File :: " + FileToReadOrPrint);

                    }


                }
            }
            else if (FileToReadOrPrint == "")
            {

#if !DEBUG
                Console.WriteLine("There is no file to Print");
                return;

#endif


#if DEBUG
           


                FileToReadOrPrint = Configuration.XMLFolder + "\\0001.xps";
                Console.WriteLine($"Set FileToReadOrPrint to {FileToReadOrPrint} beacace No arguments provided.");
            

#endif
            }



            //Console.WriteLine("Before CheckXPSFileCreateXML ");

            CheckXPSFileCreateXML(FileToReadOrPrint);
            GlobalVariables.LogInFile("CreateXML File :: " + FileToReadOrPrintXML);



            string DbRet = BillsChainLite.CreateDatabase(GlobalVariables.configFilePath);
            GlobalVariables.LogInFile(DbRet);



            string selectedPrinter = Configuration.selectedPrinter ?? "RashidPrinter";
            if (selectedPrinter == "PhysicalPrinter")
            {
                GlobalVariables.LogInFile("Case PhysicalPrinter WithoutInterface ");
                PhysicalPWithoutInterface();

            }
            else if (selectedPrinter == "BothPrinter")
            {
                GlobalVariables.LogInFile("Case Both Printers WithoutInterface ");
                BothPWithoutInterface();


            }
            else if (selectedPrinter == "RashidPrinter" || selectedPrinter == "")

            {
                GlobalVariables.LogInFile("Case RashidPrinter WithoutInterface ");
                RashidPWithoutInterface();


            }


            Console.Read();







AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
    {
        Console.WriteLine("Console is closing...");
        if (GlobalVariables.URasheedTag != null)
            GlobalVariables.URasheedTag.ReleaseTag();

        UpdateRFFailedCounter();
        DispatchPrinter();
        GlobalVariables.LogInFile("Dispatch XML Printer");



    };
   
}




        }


    }



