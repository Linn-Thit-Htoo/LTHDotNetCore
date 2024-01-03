using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTHDotNetCore.ConsoleApp.Models
{
    public class BlogDataModel
    {
        public int Blog_Id { get; set; }
        public string Blog_Title { get; set; }
        public string Blog_Author { get; set; }
        public string Blog_Content { get; set; }

        public static implicit operator BlogDataModel(List<BlogDataModel> v)
        {
            throw new NotImplementedException();
        }
    }
}
