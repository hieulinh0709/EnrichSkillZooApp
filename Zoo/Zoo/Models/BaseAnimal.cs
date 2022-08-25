using System;
using Zoo.EventHelper;
using Zoo.Interface;
using Zoo.ZooManagement;

namespace Zoo
{
    public class BaseAnimal : IAnimal
    {
        #region properties
        public event EventHandler<DetectSoundEvent> DetectSoundEvent;
        public event EventHandler<MakeSoundEvent> MakeSoundEvent;
        public virtual Guid Code { get; set; }
        public virtual string Name { get; set; }

        public virtual bool Scary { get; set; }
        public virtual string Sound { get; set; }
        public virtual bool Sounding { get; set; }
        public virtual Food Food { get; set; }
        protected Cage _cage;
        #endregion properties

        public BaseAnimal() 
        {

        }

        #region method
        /// <summary>
        /// My comment
        /// </summary>
        /// <param name="animal"></param>
        public virtual void Bite(BaseAnimal animal)
        {
            Console.WriteLine($"{this.GetType().Name} {Name} đang cắn {animal.GetType().Name} {animal.Name}");
            animal.Sounding = true;
            MakeSoundEvent?.Invoke(this, new MakeSoundEvent(animal, animal.Sound));
        }

        /// <summary>
        /// My comment
        /// </summary>
        /// <param name="animal"></param>
        public virtual void DetectSound()
        {
            foreach (var ani in _cage.animals)
            {
                if (ani.Sounding)
                {
                    DetectSoundEvent?.Invoke(this, new DetectSoundEvent(ani, ani.Sound));
                }
            }
        }

        public virtual void Speak(string sound)
        {
            Console.WriteLine($"{this.GetType().Name} {this.Name} kêu {Sound}...");
        }

        /// <summary>
        /// My comment
        /// </summary>
        public virtual void Eat()
        {
            Console.WriteLine("Animal eating ...");
        }

        /// <summary>
        /// My comment
        /// </summary>
        public void Sleep()
        {
            Console.WriteLine("Animal sleeping ...");
        }

        /// <summary>
        /// My comment
        /// </summary>
        public void ShowInFo()
        {
            Console.WriteLine($"{Code}\t\t\t\t | {Name}");
        }

        public virtual void Create()
        {
            bool isValid = true;
            string input = string.Empty;
            Code = Guid.NewGuid();
            do
            {
                Console.WriteLine("Nhập tên động vật: ");
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    isValid = false;
            } while (!isValid);

            Name = input;
        }
        #endregion method
    }
}
