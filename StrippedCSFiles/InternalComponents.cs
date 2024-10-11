using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class InternalComponents : MonoBehaviour
{
	public List<InternalText> textPrefabs;

	public static InternalComponents Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalComponents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalText CreateText(string fontName, float fontSize, Transform position)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalText CreateText(string fontName, float fontSize, Transform position, string textString, Color color, bool enablewordWrapping, string alignment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private TextAlignmentOptions getTMPAlignment(string alignment)
	{
		throw null;
	}
}
