using System;
using System.Collections.Generic;
using System.Linq;

namespace Zoo
{
    public class ZooManager
    {
        public ZooManager() { }

        public Cage HandleCreateCage()
        {
            Cage cage = new Cage();

            cage.Create();

            return cage;
        }

        public List<Cage> RemoveCage(List<Cage> cages, Guid code)
        {
            var cage = FindCage(cages, code);
            bool removed = cages.Remove(cage);
            return cages;
        }

        public void RemoveAnimalFromCage(Cage cage, Guid code)
        {
            BaseAnimal animal = cage.animals.Where(a => a.Code == code).FirstOrDefault();

            if (animal == null)
                throw new ArgumentNullException("Animal không tồn tại");

            cage.DeleteAnimal(animal);
        }
        public void AddAnimalToCage(Cage cage, int chose)
        {
            BaseAnimal animal = null;
            switch (chose)
            {
                case 1:
                    animal = new Wolf(cage);
                    animal.Create();
                    break;
                case 2:
                    animal = new Pig(cage);
                    animal.Create();
                    break;
                case 3:
                    animal = new Monkey(cage);
                    animal.Create();
                    break;
                case 4:
                    animal = new Parrot(cage);
                    animal.Create();
                    break;
                default:
                    return;
            }

            cage.AddAnimal(animal);
        }

        public Cage FindCage(List<Cage> cages, Guid cageCode)
        {
            return cages.Where(c => c.Code.ToString().Equals(cageCode.ToString())).FirstOrDefault();
        }
        public void ShowInfoCages(List<Cage> cages)
        {
            Console.WriteLine("\n=========DANH SÁCH LỒNG==========");
            Console.WriteLine("Mã lồng\t\t\t\t\tTên Lòng");
            foreach (var cage in cages)
            {
                cage.ShowInfo();
            }
        }
        public List<Cage> InitCages()
        {
            List<Cage> cages = new List<Cage>();

            Cage cage = new Cage(Guid.NewGuid(), "Lồng Người Yêu Củ");
            Cage cage2 = new Cage(Guid.NewGuid(), "Lồng Người Yêu Mới");
            
            cage.animals = InitAnimalsForCage(cage);
            cages.Add(cage);

            cage2.animals = InitAnimalsForCage2(cage2);
            cages.Add(cage2);

            return cages;
        }

        public List<BaseAnimal> InitAnimalsForCage(Cage cage)
        {
            List<BaseAnimal> animals = new List<BaseAnimal>();
            Wolf wolf = new Wolf(cage);
            wolf.Code = Guid.NewGuid();
            wolf.Name = "Gear";

            Pig pig = new Pig(cage);
            pig.Code = Guid.NewGuid();
            pig.Name = "Peppa";

            Monkey monkey = new Monkey(cage);
            monkey.Code = Guid.NewGuid();
            monkey.Name = "Molly";

            Parrot parrot = new Parrot(cage);
            parrot.Code = Guid.NewGuid();
            parrot.Name = "Dona";
            animals.AddRange(new List<BaseAnimal> { monkey, parrot, wolf, pig, });

            return animals;
        }
        public List<BaseAnimal> InitAnimalsForCage2(Cage cage)
        {
            List<BaseAnimal> animals = new List<BaseAnimal>();
            Monkey monkey = new Monkey(cage);
            monkey.Code = Guid.NewGuid();
            monkey.Name = "Molly";
            Parrot parrot = new Parrot(cage);
            parrot.Code = Guid.NewGuid();
            parrot.Name = "Dona";
            animals.AddRange(new List<BaseAnimal> { monkey, parrot });

            return animals;
        }
    }
}
