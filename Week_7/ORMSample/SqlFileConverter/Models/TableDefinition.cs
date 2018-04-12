using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlFileConverter.Models
{
    public class TableDefinition
    {
        public string TableName { get; set; }

        public IEnumerable<TableField> Fields { get; set; }
    }
}
