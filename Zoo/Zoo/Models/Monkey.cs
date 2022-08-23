using System;
using Zoo.Interface;

namespace Zoo
{
    public class Monkey : Omnivore, IIntelligent
    {
        public override string Code { get => base.Code; set => base.Code = value; }
        public override string Name { get => base.Name; set => base.Name = value; }
        public override bool Scary { get => base.Scary; set => base.Scary = value; }

        public Monkey() {}
        public Monkey(Cage cage)
        {
            _cage = cage;
            _cage.HaveFoodEvent += (s,e) => { Eat(); };
        }

        public Monkey(string code, string name)
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
