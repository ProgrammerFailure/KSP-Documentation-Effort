using System.Runtime.CompilerServices;

namespace Cursors;

public abstract class CustomCursor
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	protected CustomCursor()
	{
		throw null;
	}

	public abstract void SetCursor();

	public abstract void Unset();
}
