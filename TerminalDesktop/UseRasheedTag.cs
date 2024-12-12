using RasheedTag;
using System.Xml;
using static RasheedTag.ITagStatusUpdate;

namespace TerminalDesktopApp
{
    public class UseRasheedTag : ITagStatusUpdate
    {
        public void CreateTag(byte[] JsonData, bool RasheedFR)
        {

            GlobalVariables.TagObj = new Tag(JsonData, RasheedFR);
            GlobalVariables.TagObj.Subscribe(this);


            GlobalVariables.LogInFile("::Rasheed NFC :: New Tag created" );
            Console.Read();



        }
        public void ReleaseTag()
        {
            try
            {
                GlobalVariables.LogInFile("::Rasheed NFC :: Tag Released");
                GlobalVariables.TagObj.Unsubscribe(this);

                GlobalVariables.LogInFile("::Rasheed NFC :: In Tag Released after Unsubscribe");

                GlobalVariables.TagObj.Dispose();

                GlobalVariables.LogInFile("::Rasheed NFC :: In Tag Released after Dispose");
            }
            catch (Exception ex)
            {
                GlobalVariables.LogInFile("Crash on ReleaseTag with error " + ex.Message);

            }



        }
        public void OnTagStatusChanged(ITagStatusUpdate.TagStatus status)
        {
            switch (status)
            {
                case TagStatus.ScanDevice:
                    {
                         GlobalVariables. LogInFile("::Rasheed NFC :: Scan rasheed device 🔎");
                        break;
                    }
                case TagStatus.DeviceNotFound:
                    {
                        Console.WriteLine("Error Message :: Rasheed device not found .");
                        GlobalVariables. LogInFile("::Rasheed NFC :: Rasheed device not found 🤷🏻");
                        GlobalVariables.ConsoleRelease();
                        break;
                    }
                case TagStatus.DeviceFailedToConnect:
                    {
                        Console.WriteLine("Error Message :: Failed to connect to rasheed device.");
                         GlobalVariables. LogInFile("::Rasheed NFC :: Failed to connect to rasheed device 😢");
                        GlobalVariables.ConsoleRelease();
                        break;
                    }
                case TagStatus.DeviceConnected:
                    {
                         GlobalVariables. LogInFile("::Rasheed NFC :: Rasheed connected 🤝");
                        break;
                    }
                case TagStatus.WaitingMobile:
                    {
                         GlobalVariables. LogInFile("::Rasheed NFC :: I'm waiting a mobile");
                        break;
                    }
                case TagStatus.MobileConnected:
                    {
                         GlobalVariables. LogInFile("::Rasheed NFC :: Mobile connected");
                        break;
                    }
                case TagStatus.TransmissionSuccess:
                    {
                        Console.WriteLine("Success Message :: Rasheed NFC has successfully completed sending Invoice data .");
                         GlobalVariables. LogInFile("::Rasheed NFC ::  Success! 🎉");
                      //  GlobalVariables.TmpRFFailedCounter = 0;
                        GlobalVariables.ConsoleRelease();
                        break;
                    }
                case TagStatus.TransmissionInProgress:
                    {
                         GlobalVariables. LogInFile("::Rasheed NFC :: InProgress... 🕒️");
                        break;
                    }
                case TagStatus.MobileLost:
                    {
                        Console.WriteLine("Error Message :: Rasheed NFC Mobile connection Lost.");
                         GlobalVariables. LogInFile("::Rasheed NFC ::  MobileLost! ❌ ");
                       // GlobalVariables.TmpRFFailedCounter++;
                        GlobalVariables.ConsoleRelease();
                        break;

                    }
                case TagStatus.TransmissionFailed:
                    {
                        Console.WriteLine("Error Message :: Rasheed NFC Failed to send Invoice data .");
                         GlobalVariables. LogInFile("::Rasheed NFC ::  Failed! ❌ ");
                         //GlobalVariables.TmpRFFailedCounter++;
                        GlobalVariables.ConsoleRelease();


                        break;
                    }
            }
        }

      
   


    }
}
