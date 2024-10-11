using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro;

[Serializable]
public class TMP_Sprite : TMP_TextElement
{
	public string name;

	public int hashCode;

	public int unicode;

	public Vector2 pivot;

	public Sprite sprite;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_Sprite()
	{
		throw null;
	}
}
