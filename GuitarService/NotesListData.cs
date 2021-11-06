using System;
using System.Collections.Generic;

namespace GuitarService
{
    public sealed class NotesListData : BaseDataClass
    {

        public NotesListData(string Name, string Dummy) : base()
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
                if (Dummy != string.Empty)
                {
                    Data.Append(Dummy);
                    Data.Append(",");
                }
                List<string> tmpList = new List<string>(GuitarData.TONES_LIST);
                tmpList.RemoveRange(12, 12);
                foreach (string str in tmpList)
                {
                    SetData(str);
                }
                Data.Append("];");
                Data.AppendLine();
                JSCode = Data.ToString().Replace(",];", "];");
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
                Data.Append(string.Format("{0},", tone));
            }
            catch (Exception ex)
            {
                IDataLogger dataLogger = DataLogger.GetInstance;
                dataLogger.WriteMsg(ex.ToString());
            }
        }
    }
}
