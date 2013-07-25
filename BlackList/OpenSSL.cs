using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlackList
{
    public class OpenSSL
    {
        public static Boolean SignRequest(String OpenSSLPath, String KeyPEM, String Request, out String requestFile, out String signatureFile, String NameEventLog)
        {
            Boolean ret = true;
            requestFile = signatureFile = String.Empty;

            String RequestPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\request.xml";   
            String SignRequestPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\request.sign";
            String KeyPEMPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\key.pem";

            try
            {  
                StreamWriter swRequest = new StreamWriter(RequestPath, false, Encoding.GetEncoding("Windows-1251"));
                swRequest.Write(Request);
                swRequest.Close();

                requestFile = RequestPath;
                signatureFile = SignRequestPath;

                StreamWriter swKey = new StreamWriter(KeyPEMPath);
                swKey.Write(KeyPEM);
                swKey.Close();

                Process cmdProcess = new Process();
                cmdProcess.StartInfo.WorkingDirectory = OpenSSLPath;
                cmdProcess.StartInfo.FileName = "openssl.exe";
                cmdProcess.StartInfo.Arguments = String.Format("smime -sign -in {0} -out {1} -signer {2} -outform DER", RequestPath, SignRequestPath, KeyPEMPath);
                
                cmdProcess.StartInfo.CreateNoWindow = true;
                cmdProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                cmdProcess.Start();

                if (!cmdProcess.WaitForExit(5000))
                {
                    cmdProcess.Kill();
                    ret = false;
                }               
                
                File.Delete(KeyPEMPath);        
            }
            catch (Exception error)
            {
                EventLog.WriteEntry(NameEventLog, "Ошибка подписи: " + error.Message, EventLogEntryType.Error, 200, 001);
                ret = false;
            }

            return ret;
        }
    }
}
