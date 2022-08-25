using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoo.EventHandler;
using Zoo.ZooManagement;

namespace Zoo.EventHelper
{
    public class UserInput
    {
        public event EventHandler<InputData> inputEvent;

        public void Input()
        {
            int i = 999;
            do
            {
                MenuManager menuManager = new MenuManager();
                menuManager.MenuCage();
                string input = Console.ReadLine();
                bool isOK = Int32.TryParse(input, out i);
                inputEvent?.Invoke(this, new InputData(i));

            }
            while (i != 4);
        }
    }
}
