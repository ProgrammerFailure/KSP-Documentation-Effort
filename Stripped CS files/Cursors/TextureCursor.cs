using System;
using UnityEngine;

namespace Cursors;

[Serializable]
public class TextureCursor : CustomCursor
{
	public Texture2D texture;

	public Vector2 hotspot;

	public override void SetCursor()
	{
		Cursor.SetCursor(texture, hotspot, CursorMode.Auto);
	}

	public override void Unset()
	{
	}
}
