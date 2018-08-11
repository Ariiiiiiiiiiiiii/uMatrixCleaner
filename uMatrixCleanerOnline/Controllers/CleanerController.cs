using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.InteropServices;
using uMatrixCleaner;
using System.Collections;

namespace uMatrixCleanerOL.Controllers
{
    [Produces("application/json")]
    [Route("api/Cleaner")]
    public class CleanerController : Controller
    {
        

        // GET: api/Default
        [HttpGet]
        public IEnumerable<string> Get()
            
        {

        return new string[] { "value1", "value2" };
        }

        // GET: api/Default/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {

            //string strs1 = System.IO.File.WriteAllText(value1, @"uMatrixRules-2018-08-09.txt");
            return "value";
        }

        // POST: api/Default
        [HttpPost]
        public string Post(string value,int para1,int para2)
        {
            System.IO.File.WriteAllText("input.txt", value);
       
            int merge = para1; 
            int delete = para2;
            String a = UsingExe(value,merge,delete);
            //Console.Write(a);
            //return $"{a.Split('\n').Length} rules available."
            string strs1 = System.IO.File.ReadAllText(@"output.txt");
            //return strs1;
            //return $"{strs1.Split('\n','\r')}";
            string[] sArray1 = strs1.Split('\n');
            ArrayList list = new ArrayList(sArray1);
            string strs2 = string.Join("", (string[])list.ToArray(typeof(string)));
            string[] sArray2 = strs2.Split('\r');
            ArrayList list2 = new ArrayList(sArray2);
            string strs3 = string.Join("    ", (string[])list2.ToArray(typeof(string)));
            //foreach(string i in sArray)
            //    Console.WriteLine(i.ToString());


            return strs3;
        }

        // PUT: api/Default/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        
        public string UsingExe(string value,int para1,int para2)
        {
            
            Process process = new Process();
            
            process.StartInfo.FileName = "dotnet";//应用程序名字
            process.StartInfo.WorkingDirectory = @"D:\uMatrixCleaner\uMatrixCleaner\bin\Debug\netcoreapp2.0";//应用程序所在路径
            string s ="--MergeThreshold " + para1 + ' ' + "--RandomDelete " + para2+ " D:\\uMatrixCleaner\\uMatrixCleanerOnline\\input.txt " + "D:\\uMatrixCleaner\\uMatrixCleanerOnline\\output.txt";
            // string s= @"uMatrixCleaner.dll D:\uMatrixCleaner\uMatrixCleanerOnline\input.txt " + "--MergeThreshold " + para1 + ' ' + "--RandomDelete " + para2;
            process.StartInfo.Arguments = "uMatrixCleaner.dll " + s;//传入的参数，用空格分隔，如果参数本身带有空格就用""括起来
            //process.StartInfo.Arguments = "dotnet.exe uMatrixCleaner.dll";
            process.Start();

            process.WaitForExit();
            return "123\n456\n789";
        }
    }
    
    }

