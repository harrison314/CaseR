using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseR;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class ExcludeFromRegistrationAttribute : Attribute
{
    public ExcludeFromRegistrationAttribute()
    {
        
    }
}
