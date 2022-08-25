using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoo.ZooManagement;

namespace Zoo.EventHelper
{
    public class HaveFoodEvent : EventArgs
    {
        public Food Food { get; set; }
        public HaveFoodEvent(Food input)
        {
            Food = input;
        }
    }
}
