using System;

namespace Cursors;

[Serializable]
public class CursorItem
{
	public CustomCursor defaultCursor;

	public CustomCursor leftClickCursor;

	public CustomCursor rightClickCursor;

	public CursorItem(CustomCursor defaultCursor, CustomCursor leftClickCursor, CustomCursor rightClickCursor)
	{
		this.defaultCursor = defaultCursor;
		this.leftClickCursor = leftClickCursor;
		this.rightClickCursor = rightClickCursor;
	}
}
