using System;
using System.Collections.Generic;

namespace GuitarService
{
    public sealed class NotesData : BaseDataClass
    {

        public NotesData(string Name) : base()
        {
            this.Name = Name;
        }

        public override void InitDataAndJSCode()
        {
            try
            {
                List<string> tmpList = new List<string>(GuitarData.TONES_LIST);
                tmpList.RemoveRange(12, 12);
                foreach (string str in tmpList)
                {
                    Data.AppendLine();
                    Data.Append(Name);
                    Data.Append(" ");
                    SetData(str);
                }
                Data.AppendLine();
                JSCode = Data.ToString();
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
                Data.Append(string.Format("{0} = \"{1}\";", str, GuitarData.TONE_DICT[str]));
            }
            catch (Exception ex)
            {
                IDataLogger dataLogger = DataLogger.GetInstance;
                dataLogger.WriteMsg(ex.ToString());
            }
        }
    }
}
