using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigTable
{
    public class ProductTesting:TableEntity
    {
        public String TestDate //用作Partition Key
        {
            get
            {
                return base.PartitionKey;
            }
            set
            {
                base.PartitionKey = value;

            }
        }
        public String ProductSn //用作Row key
        {
            get
            {
                return base.RowKey;
            }
            set
            {
                base.RowKey = value;
            }
        }
        public String Result { get; set; }//检验结果
        public String Name { get; set; }
    }
}
