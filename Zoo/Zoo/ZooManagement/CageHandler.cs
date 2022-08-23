using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo.ZooManagement
{
    public class CageHandler
    {
        public Cage FindCage(List<Cage> cages, string cageCode)
        {
            return cages.Where(c => c.Code.Equals(cageCode)).FirstOrDefault();
        }

        public void AddAnimalToCage(Cage cage, int chose)
        {
            Animal animal = null;
            switch (chose)
            {
                case 1:
                    animal = new Wolf(cage);
                    animal.Code = "wolf003";
                    animal.Name = "wolf 3";
                    break;
                case 2:

                    animal = new Pig(cage);
                    animal.Code = "pig003";
                    animal.Name = "pig 3";
                    break;
                case 3:
                    animal = new Monkey(cage);
                    animal.Code = "mokey003";
                    animal.Name = "monkey 3";
                    break;
                case 4:
                    animal = new Parrot(cage);
                    animal.Code = "parrot003";
                    animal.Name = "parrot 3";
                    break;
                default:
                    return;
            }

            cage.AddAnimal(animal);
        }
    }
}
