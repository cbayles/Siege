using System;

namespace Siege.Repository.Mapping.Conventions.Formatters
{
    public class Formatter<T>
    {
        private Func<T, string> formatter;

        public Formatter(){}
        
        public void FormatAs(Func<T, string> formatter)
        {
            this.formatter = formatter;
        }

        public string Format(T property)
        {
            if(formatter == null) throw new Exception("No formatter has been specified.");
            return this.formatter(property);
        }
    }
}