using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    public class ZooManager
    {
        public ZooManager()
        {

        }
        public List<Cage> InitCages()
        {
            List<Cage> cages = new List<Cage>();

            Cage cage = new Cage("cage001", "cage name 1");
            Cage cage2 = new Cage("cage002", "cage name 2");
            
            cage.animals = InitAnimalsForCage(cage);
            cages.Add(cage);

            cage2.animals = InitAnimalsForCage2(cage2);
            cages.Add(cage2);

            return cages;
        }

        public List<Animal> InitAnimalsForCage(Cage cage)
        {
            List<Animal> animals = new List<Animal>();
            Wolf wolf = new Wolf(cage);
            wolf.Code = "wolf001";
            wolf.Name = "wolf 1";
            Pig pig = new Pig(cage);
            pig.Code = "pig001";
            pig.Name = "pig 1";
            animals.AddRange(new List<Animal> { wolf, pig });

            return animals;
        }
        public List<Animal> InitAnimalsForCage2(Cage cage)
        {
            List<Animal> animals = new List<Animal>();
            Monkey monkey = new Monkey(cage);
            monkey.Code = "mokey001";
            monkey.Name = "monkey 1";
            Parrot parrot = new Parrot(cage);
            parrot.Code = "parrot001";
            parrot.Name = "parrot 1";
            animals.AddRange(new List<Animal> { monkey, parrot });

            return animals;
        }




    }
}
