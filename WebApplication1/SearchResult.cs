using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    public class SearchResult
    {
        String title;

        public String Title
        {
            get { return title; }
            set { title = value; }
        }
        String url;

        public String Url
        {
            get { return url; }
            set { url = value; }
        }
    }
}