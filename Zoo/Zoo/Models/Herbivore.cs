using System;

namespace Zoo
{
    public abstract class Herbivore : Animal
    {
        public override string Code { get => base.Code; set => base.Code = value; }
        public override string Name { get => base.Name; set => base.Name = value; }
        public override bool Scary { get => base.Scary; set => base.Scary = value; }
        public bool LikeSeed { get; set; }

        public override void Eat()
        {
            Console.WriteLine($"Herbivore eating... - {Name}");
        }
    }
}
