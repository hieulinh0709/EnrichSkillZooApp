using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoo.Behaviour;
using Zoo.Interface;

namespace Zoo
{
    public class Pig : Carnivore, IGluttony
    {
        public override string Code { get => base.Code; set => base.Code = value; }
        public override string Name { get => base.Name; set => base.Name = value; }
        public override bool Scary { get => base.Scary; set => base.Scary = value; }

        public Pig() {}
        public Pig(Cage cage)
        {
            _cage = cage;
            _cage.HaveFoodEvent += (s,e) => { Eat(); };
        }

        public Pig(string code, string name)
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
