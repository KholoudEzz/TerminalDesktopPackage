using System.Xml;
//using AutoMapper;
static class ConfigFileRW
{

    public static string? folderPath { get; set; }
    public static string? fileFilter { get; set; }

    public static bool RasheedFR { get; set; }



    public static string XMLFolder { get; set; }
    public static string XPSFolder { get; set; }
    public static string JSONFolder { get; set; }
    
    public static string DatabaseFile { get; set; }

    public static string RashidPrinterOutputType { get; set; }
    public static string PrinterOutPath { get; set; }
    public static string ExecutablePath { get; set; }

    public static void LoadFromXml(string filePath)
    {
        try
        {
            using (XmlReader reader = XmlReader.Create(filePath))
            {
                XmlDocument xmlDoc1 = new XmlDocument();
                xmlDoc1.Load(reader); // Load the XML file
                XmlNode configNode = xmlDoc1.SelectSingleNode("Configuration");

                if (configNode != null)
                {
                    XMLFolder = configNode.SelectSingleNode("XMLFolder").InnerText;
                    XPSFolder = configNode.SelectSingleNode("XPSFolder").InnerText;
                    JSONFolder = configNode.SelectSingleNode("JSONFolder").InnerText;



                    RashidPrinterOutputType = configNode.SelectSingleNode("RashidPrinterOutputType").InnerText;
                    PrinterOutPath = configNode.SelectSingleNode("PrinterOutPath").InnerText;
                    ExecutablePath = configNode.SelectSingleNode("ExecutablePath").InnerText;
                    DatabaseFile = configNode.SelectSingleNode("DatabaseFile").InnerText;


                    RasheedFR = bool.Parse(configNode.SelectSingleNode("FastRead").InnerText);
                }
               
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred 1: " + ex.StackTrace);
        }


    }
    public static string SaveFromXml(string filePath)
    {
        try
        {
            XmlDocument xmlDoc1 = new XmlDocument();
            using (XmlReader reader = XmlReader.Create(filePath))
            {
                
                xmlDoc1.Load(reader); // Load the XML file
                XmlNode configNode = xmlDoc1.SelectSingleNode("Configuration");

                if (configNode != null)
                {
                    configNode.SelectSingleNode("XMLFolder").InnerText = XMLFolder;
                    configNode.SelectSingleNode("XPSFolder").InnerText = XPSFolder;
                    configNode.SelectSingleNode("JSONFolder").InnerText = JSONFolder;


                    configNode.SelectSingleNode("RashidPrinterOutputType").InnerText = RashidPrinterOutputType;
                    configNode.SelectSingleNode("PrinterOutPath").InnerText = PrinterOutPath;
                    configNode.SelectSingleNode("ExecutablePath").InnerText = ExecutablePath;
                    configNode.SelectSingleNode("DatabaseFile").InnerText = DatabaseFile;

                    configNode.SelectSingleNode("FastRead").InnerText = RasheedFR.ToString();
                    configNode.SelectSingleNode("RFFailedCounter").InnerText = "0";
                }
        

            }

            xmlDoc1.Save(filePath);
            return "Success";
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred 1: " + ex.StackTrace);
            return "Failed";
        }


    }
}
