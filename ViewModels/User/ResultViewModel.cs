using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class ResultViewModel
    {
        public bool Isuccessed { get; set; }
         = true;

        public string Message { get; set; }
        public object Data { get; set; }
    }
}
