using System;
using System.Collections.Generic;
using Zoo.EventHelper;

namespace Zoo
{
    public class Cage
    {
        public event EventHandler<HaveFoodEvent> HaveFoodEvent;
        public string Code { get; set; }
        public string Name { get; set; }
        public List<Animal> animals { get; set; }
        public Cage()
        {

        }
        public Cage(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public void TimeForEat(int food)
        {
            Console.WriteLine("dang cho an ...");
            HaveFoodEvent?.Invoke(this, new HaveFoodEvent(food));
        }

        public void Select()
        {
            Console.WriteLine("Select Cage");
        }

        public void Create()
        {
            Console.WriteLine("Create Cage");
        }
        public void Delete()
        {
            Console.WriteLine("Delete Cage");
        }

        public void AddAnimal(Animal animal)
        {
            animals.Add(animal);
        }

        public void ShowInfo()
        {
            Console.WriteLine($"{Code}, {Name}");
        }

    }
}
