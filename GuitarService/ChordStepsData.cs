using System;
using System.Collections.Generic;

namespace GuitarService
{
    public sealed class ChordStepsData : BaseDataClass
    {
        private List<string> StepList;

        public ChordStepsData(string Name, List<string> StepList) : base()
        {
            this.StepList = new List<string>(StepList);
            this.Name = Name;
        }

        public override void InitDataAndJSCode()
        {
            try
            {
                Data.AppendLine();
                Data.Append(Name);
                Data.Append(GuitarData.TAB);
                Data.AppendLine();
                foreach (string str in StepList)
                {
                    SetData(str);
                }
                Data.Append("};");
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
                Data.Append(str);
                Data.AppendLine();
            }
            catch (Exception ex)
            {
                IDataLogger dataLogger = DataLogger.GetInstance;
                dataLogger.WriteMsg(ex.ToString());
            }
        }
    }
}
