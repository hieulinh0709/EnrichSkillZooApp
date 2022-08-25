using System;
using Zoo.EventHelper;
using Zoo.Interface;
using Zoo.ZooManagement;

namespace Zoo
{
    public class Pig : Carnivore, IGluttony
    {
        public override string Sound { get; set; } = "Éc";

        public Pig() {}
        public Pig(Cage cage)
        {
            _cage = cage;
            _cage.HaveFoodEvent += (s,e) => 
            {
                HaveFoodEvent haveFoodEvent = (HaveFoodEvent)e;
                Food = haveFoodEvent.Food;
                if (Food == Food.Meat || Food == Food.Seed)
                {
                    FightForFood();
                    Eat();
                }
            };
            MakeSoundEvent += (s, e) =>
            {
                MakeSoundEvent makeSoundEvent = (MakeSoundEvent)e;

                makeSoundEvent.Animal.Speak(makeSoundEvent.Sound);

            };
        }

        public Pig(Guid code, string name)
        {
            Code = code;
            Name = name;
        }
        public override void Eat()
        {
            Console.WriteLine($"Pig-{Name} đang ăn {Food}");
        }
        public void FightForFood()
        {
            Bite(_cage.animals[0]);
        }
    }
}
