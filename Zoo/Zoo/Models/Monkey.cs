using System;
using Zoo.EventHelper;
using Zoo.Interface;
using Zoo.ZooManagement;

namespace Zoo
{
    public class Monkey : Omnivore, IIntelligent
    {
        public override Guid Code { get => base.Code; set => base.Code = value; }
        public override string Name { get => base.Name; set => base.Name = value; }
        public override bool Scary { get => base.Scary; set => base.Scary = value; }
        public override string Sound { get; set; } = "khỉ khỉ";
        public Monkey() {}
        public Monkey(Cage cage)
        {
            _cage = cage;
            _cage.HaveFoodEvent += (s,e) =>
            {
                HaveFoodEvent haveFoodEvent = (HaveFoodEvent)e;
                Food = haveFoodEvent.Food;
                if (Food == Food.Seed || Food == Food.Meat)
                {
                    Eat();
                }
                DetectSound();
            };
            MakeSoundEvent += (s, e) =>
            {
                MakeSoundEvent makeSoundEvent = (MakeSoundEvent)e;

                makeSoundEvent.Animal.Speak(makeSoundEvent.Sound);
            };
            DetectSoundEvent += MimicPublisher;
        }

        public void MimicPublisher(object sender, EventArgs e)
        {
            DetectSoundEvent mimicEvent = (DetectSoundEvent)e;
            Copy(mimicEvent.Sound);
            //UnSubscribeEvent();
        }

        public Monkey(Guid code, string name)
        {
            Code = code;
            Name = name;
        }
        public override void Eat()
        {
            Console.WriteLine($"Monkey-{Name} đang ăn {Food}");
        }
        public override void Speak(string sound)
        {
            Console.WriteLine($"Monkey-{Name} kêu {sound}");
        }
        public void Copy(string skill)
        {
            Speak(skill);
        }
        public void UnSubscribeEvent()
        {
            DetectSoundEvent -= MimicPublisher;
        }
    }
}
