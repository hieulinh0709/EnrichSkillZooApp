using System;
using Zoo.EventHelper;
using Zoo.Interface;
using Zoo.ZooManagement;

namespace Zoo
{
    public class Wolf : Carnivore, IGluttony
    {
        public override Guid Code { get => base.Code; set => base.Code = value; }
        public override string Name { get => base.Name; set => base.Name = value; }
        public override bool Scary { get => base.Scary; set => base.Scary = value; }
        public override string Sound { get; set; } = "Hú";

        public Wolf() { }
        public Wolf(Cage cage)
        {
            _cage = cage;
            _cage.HaveFoodEvent += (s, e) => 
            {
                HaveFoodEvent haveFoodEvent = (HaveFoodEvent)e;
                Food = haveFoodEvent.Food;

                if (Food == Food.Meat)
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

        public Wolf(Guid code, string name)
        {
            Code = code;
            Name = name;
        }

        public override void Eat()
        {
            Console.WriteLine($"Wolf-{Name} đang ăn {Food}");
        }

        public void FightForFood()
        {
            Bite(_cage.animals[1]);
        }

        public override void Create()
        {
            bool isValid = true;
            string input = string.Empty;
            Code = Guid.NewGuid();
            do
            {
                Console.WriteLine("Nhập tên động vật: ");
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    isValid = false;
            } while (!isValid);

            Name = input;
        }
    }
}
