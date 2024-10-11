using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro;

public static class TMP_DefaultControls
{
	public struct Resources
	{
		public Sprite standard;

		public Sprite background;

		public Sprite inputField;

		public Sprite knob;

		public Sprite checkmark;

		public Sprite dropdown;

		public Sprite mask;
	}

	private const float kWidth = 160f;

	private const float kThickHeight = 30f;

	private const float kThinHeight = 20f;

	private static Vector2 s_ThickElementSize;

	private static Vector2 s_ThinElementSize;

	private static Color s_DefaultSelectableColor;

	private static Color s_TextColor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static TMP_DefaultControls()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static GameObject CreateUIElementRoot(string name, Vector2 size)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static GameObject CreateUIObject(string name, GameObject parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SetDefaultTextValues(TMP_Text lbl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SetDefaultColorTransitionValues(Selectable slider)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SetParentAndAlign(GameObject child, GameObject parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SetLayerRecursively(GameObject go, int layer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static GameObject CreateScrollbar(Resources resources)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static GameObject CreateInputField(Resources resources)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static GameObject CreateDropdown(Resources resources)
	{
		throw null;
	}
}
