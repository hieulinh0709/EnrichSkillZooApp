using System;

namespace Zoo
{
    public abstract class Omnivore : Animal
    {
        public override string Code { get => base.Code; set => base.Code = value; }
        public override string Name { get => base.Name; set => base.Name = value; }
        public override bool Scary { get => base.Scary; set => base.Scary = value; }

        public override void Eat()
        {
            Console.WriteLine($"Omnivore eating... - {Name}");
        }
    }
}
