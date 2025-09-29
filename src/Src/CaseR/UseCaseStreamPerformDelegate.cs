using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseR;

/// <summary>
/// Represents a delegate that performs an operation asynchronously and returns a stream of responses.
/// </summary>
/// <typeparam name="TRequest">The type of the request parameter passed to the operation.</typeparam>
/// <typeparam name="TResponse">The type of the response elements returned by the operation.</typeparam>
/// <param name="request">The request object containing the input data for the operation. Cannot be <see langword="null"/>.</param>
/// <returns>An asynchronous stream of <typeparamref name="TResponse"/> objects representing the results of the operation. The
/// stream may yield zero or more elements.</returns>
public delegate IAsyncEnumerable<TResponse> UseCaseStreamPerformDelegate<TRequest, TResponse>(TRequest request);

