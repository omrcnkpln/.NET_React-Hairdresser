using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hair.Core.Models.System
{
    public enum Pages
    {
        API = 0,
        SPA = 1,
        MOBILE = 2,
    }

    public class Page : BaseEntity
    {
        public string? Name { get; set; }
        public Pages PageType { get; set; }
        public string? Url { get; set; }
        public string? ControllerName { get; set; }
        public string? ActionName { get; set; }
    }
}
