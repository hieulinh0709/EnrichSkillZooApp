using System;

namespace Zoo
{
    public abstract class Herbivore : BaseAnimal
    {
        public bool LikeSeed { get; set; }

        public override void Eat()
        {
            Console.WriteLine($"{typeof(Herbivore)} {Name} đang ăn");
        }
    }
}
