using System;
using Zoo.Interface;

namespace Zoo
{
    public abstract class Animal : IAnimal
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }

        public virtual bool Scary { get; set; }
        protected Cage _cage { get; set; }
        public Animal() {}

        public virtual void Eat()
        {
            Console.WriteLine("Animal eating ...");
        }
        public void Sleep()
        {
            Console.WriteLine("Animal sleeping ...");
        }
        public void Bite()
        {
            Console.WriteLine("Animal biting ...");
        }

        public void ShowInFo()
        {
            Console.WriteLine($"{Code} - {Name}");
        }
    }
}
