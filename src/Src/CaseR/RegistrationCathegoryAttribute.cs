using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseR;

/// <summary>
/// Specifies an category name for <seealso cref="IUseCaseInteractor{TRequest, TResponse}" /> or <seealso cref="IDomainEventHandler{TEvent}" /> derived class.
/// </summary>
/// <remarks>Can be applied multiple times to a class.</remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RegistrationCathegoryAttribute : Attribute
{
    /// <summary>
    /// Interactor cathegory name.
    /// </summary>
    public string CathegoryName
    {
        get;
    }

    /// <summary>
    /// Initializes a new instance of the RegistrationCathegory class with the specified category name.
    /// </summary>
    /// <param name="cathegoryName">The category name for the attribute.</param>
    public RegistrationCathegoryAttribute(string cathegoryName)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(cathegoryName);

        this.CathegoryName = cathegoryName;
    }
}
