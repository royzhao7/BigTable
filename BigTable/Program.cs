using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BigTable
{
    class Program
    {
        static CloudTable table;
        static String blobConnectionString = "DefaultEndpointsProtocol=https;AccountName=cosmosdemo1;AccountKey=F3bNXUR7VLZKEgbRyMtX2RbCGckO6oexWaoH0SvgURaZKecmscJDTK8JNpVSYvWSNvDKTP75Cr7Xgu9DOzABjg==;TableEndpoint=https://cosmosdemo1.table.cosmos.azure.cn:443/;";
        static void Main(string[] args)
        {
            Init();
            Console.WriteLine("--------------------");
            Console.WriteLine("1.Insert");
            Console.WriteLine("2.Select");
            Console.WriteLine("3.Update");
            Console.WriteLine("4.Delete");
            Console.WriteLine("--------------------");
            Console.WriteLine("Please choice a number");
            String choice = Console.ReadLine();
            if (choice == "1")
            {
                Insert();
            }
            else if (choice == "2")
            {
                Select();
            }
            else if (choice == "3")
            {
                Update();
            }
            else if (choice == "4")
            {
                Delete();
            }
        }
        static void Init()
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(blobConnectionString);
            CloudTableClient client = account.CreateCloudTableClient();
            table = client.GetTableReference("ProductTest");
            table.CreateIfNotExists();
        }
        static void Insert()
        {
            TableOperation tbOp;
            //添加
            ProductTesting productTest = new ProductTesting();
            productTest.TestDate = HttpUtility.UrlEncode("2017/08/01");//对斜杠做编码，避免抛出异常
            productTest.ProductSn = "P0003";
            productTest.Result = "Pass";
            productTest.Name = "ProductA";
            tbOp = TableOperation.Insert(productTest);
            table.Execute(tbOp);
        }
        static void Select()
        {
            TableQuery<ProductTesting> tableQuery = table.CreateQuery<ProductTesting>();
            //IQueryable<ProductTesting> pts = from productTesting in tableQuery select productTesting;
            IQueryable<ProductTesting> pts = from productTesting in tableQuery.Where<ProductTesting>(x=>x.Result=="Pass") select productTesting;
            ProductTesting pt = pts.First<ProductTesting>();
            Console.WriteLine(HttpUtility.UrlDecode(pt.TestDate)+","+pt.ProductSn+","+pt.Result);
        }
        static void Update()
        {

            TableQuery<ProductTesting> tableQuery = table.CreateQuery<ProductTesting>();
            //IQueryable<ProductTesting> pts = from productTesting in tableQuery select productTesting;
            IQueryable<ProductTesting> pts = from productTesting in tableQuery.Where<ProductTesting>(x => x.ProductSn == "P0001") select productTesting;
            ProductTesting productTest = pts.First<ProductTesting>();
            productTest.Result="Fail";
            TableOperation tbOp;
            tbOp = TableOperation.Replace(productTest);
            table.Execute(tbOp);
        }
        static void Delete()
        {
            TableQuery<ProductTesting> tableQuery = table.CreateQuery<ProductTesting>();
            //IQueryable<ProductTesting> pts = from productTesting in tableQuery select productTesting;
            IQueryable<ProductTesting> pts = from productTesting in tableQuery.Where<ProductTesting>(x => x.ProductSn == "P0001") select productTesting;
            ProductTesting productTest = pts.First<ProductTesting>();
            TableOperation tbOp = TableOperation.Delete(productTest);
            table.Execute(tbOp);

        }
    }
}
