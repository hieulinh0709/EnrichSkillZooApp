using System;
using Zoo.Interface;

namespace Zoo
{
    public class Parrot : Herbivore, IIntelligent
    {
        public override string Code { get => base.Code; set => base.Code = value; }
        public override string Name { get => base.Name; set => base.Name = value; }
        public override bool Scary { get => base.Scary; set => base.Scary = value; }

        public Parrot() {}
        public Parrot(Cage cage)
        {
            _cage = cage;
            _cage.HaveFoodEvent += (s, e) => { Eat(); };
        }

        public Parrot(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public void Copy()
        {
            Console.WriteLine("FightForFood");
        }
    }
}
