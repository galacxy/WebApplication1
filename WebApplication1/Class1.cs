using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
   public class Class1
   {
       List<String> names;

       public List<String> Names
       {
           get { return names; }
           set { names = value; }
       }

       public Class1()
       {
           names = new List<string>();
       }

       public Class1(String[] nameArray)
       {
           names = new List<string>(nameArray);
       }

       public List<String> getNames(int num)
       {
           List<string> result = new List<string>();
           if (num > Names.Count)
           {
               num = Names.Count;  
           }
           var temp = Names.OrderBy(item => item).Take(num);
           foreach (var name in temp)
           {
               result.Add(name);
           }
           return result;
       }
   }
}