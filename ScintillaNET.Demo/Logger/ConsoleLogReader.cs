using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace ScintillaNET.Demo
{
    public class ConsoleLogReader
    {
        private static string configFilePath = System.IO.Path.GetFullPath(Path.Combine(@"C:\Users\Luca\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\Modding\BepInEx\LogOutput.log"));
        private int skipLines = 0;

        public ConsoleLogReader()
        {

        }
        public System.Collections.ObjectModel.Collection<PSObject> GetConfigLog()
        {
            PowerShell psInstance = PowerShell.Create();
            psInstance.AddScript("Get-Content " + configFilePath + " | Select -Skip " + skipLines);
            var configContent = psInstance.Invoke();

            if (IsFileInUse(configFilePath) == false)
            {
                skipLines = 0;
            }

            if (configContent.Count > 0)
            {

                if (configContent != null)
                {
                    skipLines += configContent.Count;
                }

                psInstance.Dispose();
                return configContent;

            }
            psInstance.Dispose();
            return null;
        }

        private static bool IsFileInUse(string sFile)
        {
            try
            {
                File.WriteAllText(sFile, string.Empty);
                using (FileStream fs = File.Open(sFile, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {

                    fs.Close();
                }
            }
            catch (Exception)
            {
                return true;
            }
            return false;
        }
    }
}