using System;
using Zoo.Interface;

namespace Zoo
{
    public class Wolf : Carnivore, IGluttony
    {
        public override string Code { get => base.Code; set => base.Code = value; }
        public override string Name { get => base.Name; set => base.Name = value; }
        public override bool Scary { get => base.Scary; set => base.Scary = value; }

        public Wolf() { }
        public Wolf(Cage cage)
        {
            _cage = cage;
            _cage.HaveFoodEvent += (s, e) => { Eat(); };
        }

        public Wolf(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public void FightForFood()
        {
            Console.WriteLine("FightForFood");
        }
    }
}
