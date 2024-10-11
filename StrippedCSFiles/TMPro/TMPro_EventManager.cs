using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro;

public static class TMPro_EventManager
{
	public static readonly FastAction<object, Compute_DT_EventArgs> COMPUTE_DT_EVENT;

	public static readonly FastAction<bool, Material> MATERIAL_PROPERTY_EVENT;

	public static readonly FastAction<bool, TMP_FontAsset> FONT_PROPERTY_EVENT;

	public static readonly FastAction<bool, Object> SPRITE_ASSET_PROPERTY_EVENT;

	public static readonly FastAction<bool, TextMeshPro> TEXTMESHPRO_PROPERTY_EVENT;

	public static readonly FastAction<GameObject, Material, Material> DRAG_AND_DROP_MATERIAL_EVENT;

	public static readonly FastAction<bool> TEXT_STYLE_PROPERTY_EVENT;

	public static readonly FastAction<TMP_ColorGradient> COLOR_GRADIENT_PROPERTY_EVENT;

	public static readonly FastAction TMP_SETTINGS_PROPERTY_EVENT;

	public static readonly FastAction<bool, TextMeshProUGUI> TEXTMESHPRO_UGUI_PROPERTY_EVENT;

	public static readonly FastAction OnPreRenderObject_Event;

	public static readonly FastAction<Object> TEXT_CHANGED_EVENT;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static TMPro_EventManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ON_PRE_RENDER_OBJECT_CHANGED()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ON_MATERIAL_PROPERTY_CHANGED(bool isChanged, Material mat)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ON_FONT_PROPERTY_CHANGED(bool isChanged, TMP_FontAsset font)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ON_SPRITE_ASSET_PROPERTY_CHANGED(bool isChanged, Object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ON_TEXTMESHPRO_PROPERTY_CHANGED(bool isChanged, TextMeshPro obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ON_DRAG_AND_DROP_MATERIAL_CHANGED(GameObject sender, Material currentMaterial, Material newMaterial)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ON_TEXT_STYLE_PROPERTY_CHANGED(bool isChanged)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ON_COLOR_GRAIDENT_PROPERTY_CHANGED(TMP_ColorGradient gradient)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ON_TEXT_CHANGED(Object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ON_TMP_SETTINGS_CHANGED()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ON_TEXTMESHPRO_UGUI_PROPERTY_CHANGED(bool isChanged, TextMeshProUGUI obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ON_COMPUTE_DT_EVENT(object Sender, Compute_DT_EventArgs e)
	{
		throw null;
	}
}
