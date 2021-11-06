using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuitarService
{
    public sealed class TuningData : BaseDataClass
    {

        public TuningData(string Name) : base()
        {
            this.Name = Name;
        }

        public override void InitDataAndJSCode()
        {
            try
            {
                Data.AppendLine();
                Data.Append(Name);
                Data.AppendLine();
                foreach (string str in GuitarData.TUNINGS_LIST)
                {
                    SetData(str);
                }
                Data.AppendLine();
                Data.Append("}");
                Data.AppendLine();
                JSCode = Data.ToString().Replace("]," + Environment.NewLine + Environment.NewLine + "}" + Environment.NewLine, "]" + Environment.NewLine + "}" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                IDataLogger dataLogger = DataLogger.GetInstance;
                dataLogger.WriteMsg(ex.ToString());
            }
        }

        protected override void SetData(string str)
        {
            try
            {
                Data.Append(GuitarData.TAB + String.Format("\"{0}\": [{1}],", str, GetTones(str)).Replace(",]", "]"));
                Data.AppendLine();
            }
            catch (Exception ex)
            {
                IDataLogger dataLogger = DataLogger.GetInstance;
                dataLogger.WriteMsg(ex.ToString());
            }
        }

        private string GetTones(string tone)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                tone = tone.Remove(0, 3);
                List<string> tmpList = tone.Split(' ').ToList();
                tmpList.Reverse();
                foreach (string str in tmpList)
                {
                    sb.Append(str.ToUpper());
                    sb.Append(",");
                }
            }
            catch (Exception ex)
            {
                IDataLogger dataLogger = DataLogger.GetInstance;
                dataLogger.WriteMsg(ex.ToString());
            }
            return sb.ToString().Replace("#", "_SHARP").Replace("BB", "B_FLAT");
        }
    }
}
