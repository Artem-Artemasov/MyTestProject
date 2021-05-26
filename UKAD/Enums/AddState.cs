using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UKAD.Enums
{
    public enum AddState:byte
    {
        AddAsNew = 0,
        AddAsAllLocation = 1,
        ExistWithoutTime = 2,
        ExistNormal = 3,
    }
}
