using RasheedTag;
using TerminalDesktopSilence;
using static RasheedTag.ITagStatusUpdate;

namespace TerminalDesktopSilence
{
    public class UseRasheedTag : ITagStatusUpdate
    {
        
        static SilenceTerminal TermDialog;
        public Tag CreateTag(byte[] JsonData, SilenceTerminal terminal)
        {

            var tag = new Tag(JsonData);
            tag.Subscribe(this);
           
            TermDialog = terminal;

            GlobalVariables.LogInFile("New Tag created...");
            return tag;

        }
        public void ReleaseTag(Tag CurrTag)
        {
            GlobalVariables.LogInFile("Tag Released ...");
            CurrTag.Unsubscribe(this);
            CurrTag.Dispose();
        }



        public void OnTagStatusChanged(ITagStatusUpdate.TagStatus status)
        {
            switch (status)
            {
                case TagStatus.ScanDevice:
                    {
                        GlobalVariables.LogInFile("Scan rasheed device 🔎");
                        break;
                    }
                case TagStatus.DeviceNotFound:
                    {
                        MessageBox.Show("Rasheed device not found .", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        GlobalVariables.LogInFile("Rasheed device not found 🤷🏻");
                        if (TermDialog != null )
                            TermDialog.TerminalClose();
                        break;
                    }
                case TagStatus.DeviceFailedToConnect:
                    {
                        MessageBox.Show("Failed to connect to rasheed device.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        GlobalVariables.LogInFile("Failed to connect to rasheed device 😢");
                        if (TermDialog != null )
                            TermDialog.TerminalClose();
                        break;
                    }
                case TagStatus.DeviceConnected:
                    {
                        GlobalVariables.LogInFile("Rasheed connected 🤝");
                        break;
                    }
                case TagStatus.WaitingMobile:
                    {
                        GlobalVariables.LogInFile("I'm waiting a mobile");
                        break;
                    }
                case TagStatus.MobileConnected:
                    {
                        GlobalVariables.LogInFile("Mobile connected");
                        break;
                    }
                case TagStatus.TransmissionSuccess:
                    {
                        MessageBox.Show("Rasheed NFC has successfully completed sending Invoice data .", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        GlobalVariables.LogInFile(" Success! 🎉");
                        // GlobalVariables.TmpRFFailedCounter = 0;
                        if (TermDialog != null)
                            TermDialog.TerminalClose();
                        break;
                    }
                case TagStatus.TransmissionInProgress:
                    {
                        GlobalVariables.LogInFile("InProgress... 🕒️");
                        break;
                    }
                case TagStatus.MobileLost:
                    {
                        MessageBox.Show("Rasheed NFC Mobile connection Lost.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        GlobalVariables.LogInFile(" MobileLost! ❌ ");


                        if (TermDialog != null )
                        {
                            //GlobalVariables.UpdateRFFailedCounter();
                            TermDialog.TerminalClose();
                        }
                        break;
                    }
                case TagStatus.TransmissionFailed:
                    {
                        MessageBox.Show("Rasheed NFC Failed to send Invoice data .", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        GlobalVariables.LogInFile(" Failed! ❌ ");
                        //GlobalVariables.TmpRFFailedCounter++;


                        if (TermDialog != null )
                        {
                            //GlobalVariables.UpdateRFFailedCounter();
                            TermDialog.TerminalClose();
                        }
                        break;
                    }
            }
        }

      
    }
}
