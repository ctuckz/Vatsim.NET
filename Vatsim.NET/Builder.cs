using System;
using System.Collections.Generic;
using System.Text;

namespace Vatsim.NET
{
    internal abstract class Builder<T>
    {
        public abstract T Build();
    }
}
