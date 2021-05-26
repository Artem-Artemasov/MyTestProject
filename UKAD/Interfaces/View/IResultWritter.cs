using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UKAD.Interfaces.View
{
    public interface IResultWritter
    {
        void WriteLine(string line);
        void Write(string line);
        int GetOutputWidth();
        void ChangeCursorPositonX(int newPos);
    }
}
