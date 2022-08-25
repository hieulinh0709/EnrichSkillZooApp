using System;
using Zoo.EventHelper;

namespace Zoo
{
    public abstract class Omnivore : BaseAnimal
    {

        public override void Eat()
        {
            //Console.WriteLine($"{typeof(Omnivore)} {Name} đang ăn {Food}");
        }
    }
}
