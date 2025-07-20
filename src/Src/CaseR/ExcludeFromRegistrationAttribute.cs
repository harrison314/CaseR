using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseR;

/// <summary>
/// Specifies that a class should be excluded from automatic registration processes in CaseR.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class ExcludeFromRegistrationAttribute : Attribute
{
    /// <summary>
    /// Initialize a new instance of the <see cref="ExcludeFromRegistrationAttribute"/> class.
    /// </summary>
    public ExcludeFromRegistrationAttribute()
    {
        
    }
}
