using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro;

public class TMP_UpdateManager
{
	private static TMP_UpdateManager s_Instance;

	private readonly List<TMP_Text> m_LayoutRebuildQueue;

	private Dictionary<int, int> m_LayoutQueueLookup;

	private readonly List<TMP_Text> m_GraphicRebuildQueue;

	private Dictionary<int, int> m_GraphicQueueLookup;

	public static TMP_UpdateManager instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected TMP_UpdateManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RegisterTextElementForLayoutRebuild(TMP_Text element)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool InternalRegisterTextElementForLayoutRebuild(TMP_Text element)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RegisterTextElementForGraphicRebuild(TMP_Text element)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool InternalRegisterTextElementForGraphicRebuild(TMP_Text element)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCameraPreCull(Camera cam)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DoRebuilds()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void UnRegisterTextElementForRebuild(TMP_Text element)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InternalUnRegisterTextElementForGraphicRebuild(TMP_Text element)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InternalUnRegisterTextElementForLayoutRebuild(TMP_Text element)
	{
		throw null;
	}
}
