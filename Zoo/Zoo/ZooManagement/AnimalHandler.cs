using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo.ZooManagement
{
    public class AnimalHandler
    {
        public void ShowInfoAnimals(List<Animal> animals)
        {
            Console.WriteLine("\n=========DANH SÁCH ĐỘNG VẬT==========");
            foreach (var animal in animals)
            {
                animal.ShowInFo();
            }
        }
    }
}
