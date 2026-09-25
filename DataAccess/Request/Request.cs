using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Request
{
    public class Request
    {
        public List<Content> Contents { get; set; } = [];
    }

    public class Content
    {
        public List<Part> Parts { get; set; } = [];
    }

    public class Part
    {
        public string Text { get; set; } = string.Empty;
    }
}
