using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace aviso_tip
{
    internal class LogWritter
    {
        public string m_exePath = Assembly.GetExecutingAssembly().CodeBase;
        private int i = 1;

        public LogWritter(string logMessage) => this.LogWrite(logMessage);

        public void LogWrite(string logMessage)
        {
            this.m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter txtWriter = File.AppendText(this.m_exePath + "\\log" + this.i.ToString() + ".txt"))
                    this.Log(logMessage, (TextWriter)txtWriter);
                if (new FileInfo(this.m_exePath + "\\log" + this.i.ToString() + ".txt").Length <= 100000000L)
                    return;
                ++this.i;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (File.Exists(this.m_exePath + "\\log" + (this.i - 1).ToString() + ".txt"))
                    File.Delete(this.m_exePath + "\\log" + (this.i - 1).ToString() + ".txt");
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                TextWriter textWriter = txtWriter;
                DateTime dateTime = DateTime.Now;
                dateTime = dateTime.ToLocalTime();
                string str1 = dateTime.ToString("dd/MM/yyyy hh:mm:ss.fff");
                string str2 = logMessage;
                textWriter.WriteLine("{0} - {1}", (object)str1, (object)str2);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
