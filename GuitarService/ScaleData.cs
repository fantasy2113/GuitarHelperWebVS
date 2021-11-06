using System;
using System.Collections.Generic;
using System.Text;

namespace GuitarService
{
    public sealed class ScaleData : BaseDataClass
    {

        public ScaleData(string Name, string Dummy) : base()
        {
            this.Name = Name;
            this.Dummy = Dummy;
        }

        public override void InitDataAndJSCode()
        {
            try
            {
                Data.AppendLine();
                Data.Append(Name);
                Data.AppendLine();
                Data.Append(GuitarData.TAB);
                Data.Append(Dummy);
                Data.AppendLine();
                List<string> tmpList = new List<string>(GuitarData.TONES_LIST);
                tmpList.RemoveRange(12, 12);
                foreach (string str in tmpList)
                {
                    SetData(str);
                }
                Data.Append("};");
                Data.AppendLine();
                JSCode = Data.ToString().Replace(",],", "],");
                JSCode = JSCode.Replace("]," + Environment.NewLine + Environment.NewLine + "}" + Environment.NewLine, "]" + Environment.NewLine + "}" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                IDataLogger dataLogger = DataLogger.GetInstance;
                dataLogger.WriteMsg(ex.ToString());
            }
        }

        protected override void SetData(string tone)
        {
            try
            {
                foreach (var kvp in GuitarData.SCALES_DICT)
                {
                    Data.Append(string.Format(kvp.Key, GuitarData.TONE_DICT[tone], GetTones(kvp.Value, tone)));
                }
                Data.AppendLine();
            }
            catch (Exception ex)
            {
                IDataLogger dataLogger = DataLogger.GetInstance;
                dataLogger.WriteMsg(ex.ToString());
            }
        }

        private string GetTones(Interval interval, string tone)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                int index = GuitarData.TONES_LIST.IndexOf(tone);
                List<string> tmpList = new List<string>(GuitarData.TONES_LIST.GetRange(index, 12));
                index = 0;
                foreach (string str in tmpList)
                {
                    if (interval.GetStructure()[index])
                    {
                        sb.Append(str);
                        sb.Append(",");
                    }
                    index++;
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
