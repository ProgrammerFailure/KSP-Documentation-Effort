using System.Runtime.CompilerServices;
using UnityEngine;

namespace EdyCommonTools;

[ExecuteInEditMode]
public class ScreenNotes : MonoBehaviour
{
	[TextArea(2, 40)]
	public string text;

	[Range(6f, 100f)]
	public int size;

	public Color color;

	public Vector2 screenPosition;

	[Space(5f)]
	public Font font;

	private GUIStyle m_smallStyle;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScreenNotes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnValidate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateTextProperties()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGUI()
	{
		throw null;
	}
}
