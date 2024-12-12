using System.Xml;
//using AutoMapper;
static class ConfigFileRW
{

    public static string? selectedPrinter { get; set; }

    public static string? DefaultPhyPrinter { get; set; }
    public static string? folderPath { get; set; }
    public static string? fileFilter { get; set; }

    public static string XMLFolder { get; set; }
    public static string XPSFolder { get; set; }
    public static string JSONFolder { get; set; }

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

                    DefaultPhyPrinter = configNode.SelectSingleNode("DefaultPhyPrinter").InnerText;
                    selectedPrinter = configNode.SelectSingleNode("selectedPrinter").InnerText;


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

                    configNode.SelectSingleNode("DefaultPhyPrinter").InnerText = DefaultPhyPrinter;
                    configNode.SelectSingleNode("selectedPrinter").InnerText = selectedPrinter;
                    configNode.SelectSingleNode("ExecutablePath").InnerText = ExecutablePath;


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
