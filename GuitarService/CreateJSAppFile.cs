using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GuitarService
{
    public sealed class CreateJSAppFile
    {
        private List<string> DataList;

        public CreateJSAppFile(List<BaseDataClass> baseDataList)
        {
            DataList = new List<string>();
            if (baseDataList != null)
            {
                foreach (BaseDataClass item in baseDataList)
                {
                    item.InitDataAndJSCode();
                    DataList.Add(item.JSCode);
                }
            }
        }

        public void CreateAppFile()
        {
            try
            {
                File.WriteAllText(GuitarData.APP_PATH, string.Empty);
                WriteFile(GetDataForAppFile(GetDataFromTemplate(GuitarData.APP_1_PATH), GetDataFromTemplate(GuitarData.APP_2_PATH)));
            }
            catch (Exception ex)
            {
                IDataLogger dataLogger = DataLogger.GetInstance;
                dataLogger.WriteMsg(ex.ToString());
            }
        }

        private void WriteFile(string dataStr)
        {
            try
            {
                File.WriteAllText(GuitarData.APP_PATH, dataStr);
            }
            catch (Exception ex)
            {
                IDataLogger dataLogger = DataLogger.GetInstance;
                dataLogger.WriteMsg(ex.ToString());
            }
        }

        private string GetDataForAppFile(string dataApp1, string dataApp2)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append(dataApp1);
                sb.AppendLine();
                sb.AppendLine();
                sb.Append("// asp.net generated content - start");
                sb.AppendLine();
                foreach (var item in DataList)
                {
                    sb.Append(item);
                }
                sb.AppendLine();
                sb.Append("// asp.net generated content - end");
                sb.AppendLine();
                sb.AppendLine();
                sb.Append(dataApp2);
            }
            catch (Exception ex)
            {
                IDataLogger dataLogger = DataLogger.GetInstance;
                dataLogger.WriteMsg(ex.ToString());
            }
            return sb.ToString();
        }

        private string GetDataFromTemplate(string path)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    sb.Append(sr.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                IDataLogger dataLogger = DataLogger.GetInstance;
                dataLogger.WriteMsg(ex.ToString());
            }
            return sb.ToString();
        }
    }
}
