using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseR;

/// <summary>
/// Delegate for perform/execute use case interactor in pipeline.
/// </summary>
/// <typeparam name="TRequest">Request type.</typeparam>
/// <typeparam name="TResponse">Response type.</typeparam>
/// <param name="request">Request instance.</param>
/// <returns>Response task instance.</returns>
public delegate Task<TResponse> UseCasePerformDelegate<TRequest, TResponse>(TRequest request);
