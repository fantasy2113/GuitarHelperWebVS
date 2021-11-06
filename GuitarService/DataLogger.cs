using System;
using System.IO;

namespace GuitarService
{
    public sealed class DataLogger : IDataLogger
    {
        private static DataLogger Instance;

        private DataLogger() { }

        public static DataLogger GetInstance {
            get {
                if (Instance == null)
                {
                    Instance = new DataLogger();
                }
                return Instance;
            }
        }

        public void WriteMsg(string msg)
        {
            try
            {
                string path = GuitarData.LOG_PATH;
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine("");
                    }
                }
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(string.Format("{0}: {1}", DateTime.Now.ToString(), msg));
                    sw.WriteLine();
                    sw.WriteLine();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
