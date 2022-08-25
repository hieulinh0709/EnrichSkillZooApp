using System;

namespace Zoo
{
    public class Carnivore : BaseAnimal
    {
        public override Guid Code { get => base.Code; set => base.Code = value; }
        public override string Name { get => base.Name; set => base.Name = value; }
        public override bool Scary { get => base.Scary; set => base.Scary = value; }
        public bool LikeMeat { get; set; }

        public override void Eat()
        {
        }
    }
}
