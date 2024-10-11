using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class InternalText : MonoBehaviour
{
	public string fontName;

	public float fontSize;

	[HideInInspector]
	public TextMeshPro text;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}
}
