using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaBotLibrary.AI
{
    interface IRecognizer
    {
        string RecognizeTopic(byte[] imageBytes);
    }
}
