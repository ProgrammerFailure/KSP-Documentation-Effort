using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro;

public class TMP_UpdateRegistry
{
	public static TMP_UpdateRegistry s_Instance;

	public readonly List<ICanvasElement> m_LayoutRebuildQueue = new List<ICanvasElement>();

	public Dictionary<int, int> m_LayoutQueueLookup = new Dictionary<int, int>();

	public readonly List<ICanvasElement> m_GraphicRebuildQueue = new List<ICanvasElement>();

	public Dictionary<int, int> m_GraphicQueueLookup = new Dictionary<int, int>();

	public static TMP_UpdateRegistry instance
	{
		get
		{
			if (s_Instance == null)
			{
				s_Instance = new TMP_UpdateRegistry();
			}
			return s_Instance;
		}
	}

	public TMP_UpdateRegistry()
	{
		Canvas.willRenderCanvases += PerformUpdateForCanvasRendererObjects;
	}

	public static void RegisterCanvasElementForLayoutRebuild(ICanvasElement element)
	{
		instance.InternalRegisterCanvasElementForLayoutRebuild(element);
	}

	public bool InternalRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
	{
		int instanceID = (element as Object).GetInstanceID();
		if (m_LayoutQueueLookup.ContainsKey(instanceID))
		{
			return false;
		}
		m_LayoutQueueLookup[instanceID] = instanceID;
		m_LayoutRebuildQueue.Add(element);
		return true;
	}

	public static void RegisterCanvasElementForGraphicRebuild(ICanvasElement element)
	{
		instance.InternalRegisterCanvasElementForGraphicRebuild(element);
	}

	public bool InternalRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
	{
		int instanceID = (element as Object).GetInstanceID();
		if (m_GraphicQueueLookup.ContainsKey(instanceID))
		{
			return false;
		}
		m_GraphicQueueLookup[instanceID] = instanceID;
		m_GraphicRebuildQueue.Add(element);
		return true;
	}

	public void PerformUpdateForCanvasRendererObjects()
	{
		for (int i = 0; i < m_LayoutRebuildQueue.Count; i++)
		{
			instance.m_LayoutRebuildQueue[i].Rebuild(CanvasUpdate.Prelayout);
		}
		if (m_LayoutRebuildQueue.Count > 0)
		{
			m_LayoutRebuildQueue.Clear();
			m_LayoutQueueLookup.Clear();
		}
		for (int j = 0; j < m_GraphicRebuildQueue.Count; j++)
		{
			instance.m_GraphicRebuildQueue[j].Rebuild(CanvasUpdate.PreRender);
		}
		if (m_GraphicRebuildQueue.Count > 0)
		{
			m_GraphicRebuildQueue.Clear();
			m_GraphicQueueLookup.Clear();
		}
	}

	public void PerformUpdateForMeshRendererObjects()
	{
		Debug.Log("Perform update of MeshRenderer objects.");
	}

	public static void UnRegisterCanvasElementForRebuild(ICanvasElement element)
	{
		instance.InternalUnRegisterCanvasElementForLayoutRebuild(element);
		instance.InternalUnRegisterCanvasElementForGraphicRebuild(element);
	}

	public void InternalUnRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
	{
		int instanceID = (element as Object).GetInstanceID();
		instance.m_LayoutRebuildQueue.Remove(element);
		m_GraphicQueueLookup.Remove(instanceID);
	}

	public void InternalUnRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
	{
		int instanceID = (element as Object).GetInstanceID();
		instance.m_GraphicRebuildQueue.Remove(element);
		m_LayoutQueueLookup.Remove(instanceID);
	}
}