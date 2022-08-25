using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo.Interface
{
    public interface IAnimal
    {
        void Eat();
        void Sleep();
        void Bite(BaseAnimal animal);
        void Speak(string sound);
        void Create();
    }
}
