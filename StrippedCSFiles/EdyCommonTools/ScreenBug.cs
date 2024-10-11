using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace EdyCommonTools;

[ExecuteInEditMode]
public class ScreenBug : MonoBehaviour
{
	public bool hideAtEditor;

	public bool sideBySide;

	public bool showText;

	public string[] text;

	public GUIStyle style;

	public bool showBug;

	public Texture2D bug;

	public float bugSize;

	[FormerlySerializedAs("borderX")]
	public float marginX;

	[FormerlySerializedAs("borderY")]
	public float marginY;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScreenBug()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGUI()
	{
		throw null;
	}
}
