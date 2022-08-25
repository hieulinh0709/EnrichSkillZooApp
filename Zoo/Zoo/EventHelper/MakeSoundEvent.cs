using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo.EventHelper
{
    public class MakeSoundEvent : EventArgs
    {
        public BaseAnimal Animal { get; set; }
        public string Sound { get; set; }
        public MakeSoundEvent(BaseAnimal animal, string sound)
        {
            Animal = animal;
            Sound = sound;
        }
    }
}
