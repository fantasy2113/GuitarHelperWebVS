using System;
using System.Text;

namespace GuitarService
{
    public abstract class BaseDataClass
    {
        public string JSCode { get; protected set; }
        protected StringBuilder Data;
        protected string Name;
        protected string Dummy;

        public BaseDataClass()
        {
            Data = new StringBuilder();
            Name = string.Empty;
            Dummy = string.Empty;
            JSCode = string.Empty;
        }

        public abstract void InitDataAndJSCode();

        protected abstract void SetData(string str);
    }
}
