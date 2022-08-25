using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo.EventHelper
{
    public class DetectSoundEvent : EventArgs
    {
        public BaseAnimal Animal { get; set; }
        public string Sound { get; set; }
        public DetectSoundEvent(BaseAnimal animal, string sound)
        {
            Animal = animal;
            Sound = sound;
        }
    }
}
