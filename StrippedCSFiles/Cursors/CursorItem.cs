using System;
using System.Runtime.CompilerServices;

namespace Cursors;

[Serializable]
public class CursorItem
{
	public CustomCursor defaultCursor;

	public CustomCursor leftClickCursor;

	public CustomCursor rightClickCursor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CursorItem(CustomCursor defaultCursor, CustomCursor leftClickCursor, CustomCursor rightClickCursor)
	{
		throw null;
	}
}
