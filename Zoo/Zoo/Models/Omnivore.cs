using System;
using Zoo.EventHelper;

namespace Zoo
{
    public abstract class Omnivore : BaseAnimal
    {
        public override Guid Code { get => base.Code; set => base.Code = value; }
        public override string Name { get => base.Name; set => base.Name = value; }
        public override bool Scary { get => base.Scary; set => base.Scary = value; }

        public override void Eat()
        {
            //Console.WriteLine($"{typeof(Omnivore)} {Name} đang ăn {Food}");
        }
    }
}
