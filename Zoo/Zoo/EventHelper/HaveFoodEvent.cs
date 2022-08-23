using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo.EventHelper
{
    public class HaveFoodEvent : EventArgs
    {
        public int Food { get; set; }
        public HaveFoodEvent(int input)
        {
            Food = input;
        }
    }
}
