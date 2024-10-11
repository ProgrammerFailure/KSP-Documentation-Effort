using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

namespace TMPro;

public class TMP_UpdateRegistry
{
	private static TMP_UpdateRegistry s_Instance;

	private readonly List<ICanvasElement> m_LayoutRebuildQueue;

	private Dictionary<int, int> m_LayoutQueueLookup;

	private readonly List<ICanvasElement> m_GraphicRebuildQueue;

	private Dictionary<int, int> m_GraphicQueueLookup;

	public static TMP_UpdateRegistry instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected TMP_UpdateRegistry()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RegisterCanvasElementForLayoutRebuild(ICanvasElement element)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool InternalRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RegisterCanvasElementForGraphicRebuild(ICanvasElement element)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool InternalRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PerformUpdateForCanvasRendererObjects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PerformUpdateForMeshRendererObjects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void UnRegisterCanvasElementForRebuild(ICanvasElement element)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InternalUnRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InternalUnRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
	{
		throw null;
	}
}
