using Smooth.Algebraics;

namespace Smooth.Slinq.Context;

public delegate void Mutator<T, U>(ref U context, out Option<T> next);
