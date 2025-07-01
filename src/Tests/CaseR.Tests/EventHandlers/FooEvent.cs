using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseR.Tests.EventHandlers;

public record FooEvent(string Name) : IDomainEvent;
