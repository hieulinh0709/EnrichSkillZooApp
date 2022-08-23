using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zoo.EventHandler;
using Zoo.EventHelper;
using Zoo.ZooManagement;

namespace Zoo
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding("UTF-8");
            CageHandler cageHandler = new CageHandler();
            AnimalHandler animalHandler = new AnimalHandler();
            MenuManager menuManager = new MenuManager(cageHandler, animalHandler);

            ZooManager zooManager = new ZooManager();
            List<Cage> cages = zooManager.InitCages();

            Console.WriteLine("============== DS Lồng ==============");
            foreach (var cage in cages)
            {
                cage.ShowInfo();
            }
            //cages[0].TimeForEat(1);

            //hiển thị màn hình chức năng cho Cage
            menuManager.MenuCage();

            UserInput userInput = new UserInput();
            userInput.inputEvent += (sender, e) =>
            {
                InputData intput = (InputData)e;

                do
                {
                    switch (intput.Data)
                    {
                        case 1:
                            cages.Add(menuManager.HandleCreateCage());
                            break;
                        case 2:
                            menuManager.HandleSelectCage(cages);
                            break;
                        case 3:
                            //CreateCage();
                            break;
                        default:
                            break;
                    }
                } while (intput.Data == 4);
                
            };
            userInput.Input();
        }

    }

}
