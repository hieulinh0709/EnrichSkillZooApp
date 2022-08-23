using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoo.Interface;

namespace Zoo.Behaviour
{
    public class CopyBehavious : IBehaviou
    {
        public void DoBehaviours()
        {
            Console.WriteLine("Coping ...");
        }
    }
}
