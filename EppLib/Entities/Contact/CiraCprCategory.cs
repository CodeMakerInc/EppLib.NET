using System;

namespace EppLib.Entities
{
    public class CiraCprCategory
    {
        public string CprCode { get; set; }
        public string CprDescription { get; set; }

        public CiraCprCategory(string cprCode, string cprDescription)
        {
            CprCode = cprCode;
            CprDescription = cprDescription;
        }

        public override bool Equals(object obj)
        {
            return CprCode.Equals(((CiraCprCategory)obj).CprCode);
        }
    }
}
