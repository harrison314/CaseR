using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseR.Tests;

public class CallAssertion
{
    private readonly List<string> calls;

    public CallAssertion()
    {
        this.calls = new List<string>();
    }

    public void AddCall(string callName)
    {
        this.calls.Add(callName);
    }

    public void AssertCall(string callName)
    {
        if (!this.calls.Contains(callName))
        {
            throw new InvalidOperationException($"Expected call '{callName}' but it was not found.");
        }
    }

    public void AssertNoCalls()
    {
        if (this.calls.Count > 0)
        {
            throw new InvalidOperationException($"Expected no calls but found: {string.Join(", ", this.calls)}");
        }
    }
}
