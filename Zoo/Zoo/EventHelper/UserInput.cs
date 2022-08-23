using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoo.EventHandler;

namespace Zoo.EventHelper
{
    public class UserInput
    {
        public event EventHandler<InputData> inputEvent;

        public void Input()
        {
            do
            {
                string input = Console.ReadLine();
                int i = Int32.Parse(input);
                inputEvent?.Invoke(this, new InputData(i));

            }
            while (false);
        }
    }
}
