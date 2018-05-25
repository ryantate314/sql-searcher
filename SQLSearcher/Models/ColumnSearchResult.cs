using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLSearcher.Models
{
    class ColumnSearchResult
    {
        public string Database { get; set; }
        public string Schema { get; set; }
        public string Table { get; set; }
        public string Column { get; set; }
        public string Type { get; set; }
        public string Reason { get; set; }
        public int? CharacterLength { get; set; }
        public bool IsNullable { get; set; }
        public bool IsPrimaryKey { get; set; }

        public string FK_Constraint { get; set; }
        public string PK_Table { get; set; }
        public string PK_Column { get; set; }

        public string DisplayType
        {
            get
            {
                if (Type == "varchar" || Type == "char")
                {
                    return String.Format("{0}({1}) {2}", Type, CharacterLength, IsNullable ? "" : "not null");
                }
                else
                {
                    return String.Format("{0} {1}", Type, IsNullable ? "" : "not null");
                }
            }
        }

        public bool IsForeignKey
        {
            get
            {
                return !String.IsNullOrEmpty(FK_Constraint);
            }
        }

        public string FKString
        {
            get
            {
                string result = "";
                if (IsForeignKey)
                {
                    result = String.Format("References {0}.{1}", PK_Table, PK_Column);
                }
                return result;
            }
        }
    }
}
