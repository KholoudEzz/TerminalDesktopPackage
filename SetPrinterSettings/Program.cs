// See https://aka.ms/new-console-template for more information
using Microsoft.Win32;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Principal;

class Program
{

    public static string configFilePath = "C:\\Program Files\\MisrCompany\\RasheedTerminal\\BillConfig.xml";
    public static string DriverSetupPath ="C:\\Program Files\\MisrCompany\\RasheedTerminal\\Tools\\Driver\\Setup.exe";
    public static string XMLPrinterPath = "C:\\Program Files\\XML Printer\\xmlprn.exe";
    public static string DatabaseFileName = "";

    



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
            FileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TerminalDesktop.exe"),
            Arguments = "",
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
    static  void RunCommandSilently(string command, string arguments)
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
  
    static void DispatchPrinter()
    {
        string command = XMLPrinterPath;

        string arguments = $"/dispatch";
        RunCommandSilently(command, arguments);




    }

    static void RashidPrinterConfig( string PrinterOutPath)
    {



        if (!Directory.Exists(PrinterOutPath))
        {
            try
            {
                // إنشاء المجلد
                Directory.CreateDirectory(PrinterOutPath);

            }
            catch (Exception ex)
            {
                // في حالة حدوث أي خطأ أثناء الإنشاء
                Console.WriteLine($"error: {ex.Message}");
            }
        }

        string command = XMLPrinterPath;
        string arguments1 = $"/configure ";
        string arguments2 = $" \"outputflags=111001"+ "\" ";
        string arguments3 = $" \"outputdir={PrinterOutPath}\" ";

        string arguments = arguments1 + arguments2 + arguments3;
        RunCommandSilently(command, arguments);



    }
    static void ExecutablePathUpdate(string ExecutablePathStr)
    {
        string command = XMLPrinterPath;

        string arguments = $"/configure \"clearsteps\"";
        RunCommandSilently(command, arguments);





        arguments = $"/configure \"addstep=1,\\\"{ExecutablePathStr}\\\"\"";
        RunCommandSilently(command, arguments);


    }

    static void SetupDriver()
    {
        string command = DriverSetupPath;

        string arguments = " /q";
        RunCommandSilently(command, arguments);


    }


    static void Main(string[] args)
    {
        Console.WriteLine("Preparing printer settings .... ");

        if (!IsRunningAsAdministrator())
        {
            // Restart the application with elevated privileges
            RunAsAdministrator();
            return;
        }


        //string registryAppKey = @"SOFTWARE\Rasheed";
        //string registryCPName = "ConfigPath";
        //string registryPPValue = "PrinterPath";


        //configFilePath = ReadRegistryValue(registryAppKey, registryCPName);
        //XMLPrinterPath = ReadRegistryValue(registryAppKey, registryPPValue);




        //if (string.IsNullOrEmpty(configFilePath))
        //{
        //    Console.WriteLine("Configuration file path not found in the registry.");
        //    return;
        //}



        Thread.Sleep(3000);
        ConfigFileRW.LoadFromXml(configFilePath);


        DispatchPrinter();



        RashidPrinterConfig(ConfigFileRW.PrinterOutPath);
        ExecutablePathUpdate(ConfigFileRW.ExecutablePath);

        Console.WriteLine("Start Driver setup .... ");
        SetupDriver();





    }
}