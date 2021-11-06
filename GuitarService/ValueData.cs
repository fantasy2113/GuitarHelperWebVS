using System;

namespace GuitarService
{
    public sealed class ValueData : BaseDataClass
    {

        public ValueData() : base() { }

        public override void InitDataAndJSCode()
        {
            try
            {
                foreach (string str in GuitarData.VALUES_LIST)
                {
                    SetData(str);
                    Data.AppendLine();
                }
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
            }
            catch (Exception ex)
            {
                IDataLogger dataLogger = DataLogger.GetInstance;
                dataLogger.WriteMsg(ex.ToString());
            }
        }
    }
}