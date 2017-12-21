using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zycie.CustomLoggers
{
    public class FileLogger : LogBase
    {
        public string filePath = @"IDGLog.txt";
            public override void Log(string message)
            {
                using (StreamWriter streamWriter = new StreamWriter(filePath))
                {
                    streamWriter.WriteLine(message);
                    streamWriter.Close();
                }
            }
    }
}
