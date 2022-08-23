using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo.EventHandler
{
    public class InputData : EventArgs
    {
        public int Data { get; set; }
        public InputData(int input)
        {
            Data = input;
        }
    }
}
