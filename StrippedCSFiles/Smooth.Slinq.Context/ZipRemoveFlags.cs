using System;

namespace Smooth.Slinq.Context;

[Flags]
public enum ZipRemoveFlags
{
	None = 0,
	Left = 1,
	Right = 2,
	Both = 3
}
