using System;

namespace CaseR.SourceGenerator;

[Flags]
internal enum UseCaseInteractorType
{
    None = 0,
    Standard = 1,
    Streaming = 2
}