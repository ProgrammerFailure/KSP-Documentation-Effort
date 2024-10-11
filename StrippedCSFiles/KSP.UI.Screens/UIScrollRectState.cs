using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class UIScrollRectState : MonoBehaviour
{
	[Serializable]
	public class PanelState
	{
		public ScrollRect scrollRect;

		public string name;

		public RectTransform rectTransform;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PanelState()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Enable()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Disable()
		{
			throw null;
		}
	}

	public PanelState[] panelList;

	[NonSerialized]
	public PanelState CurrentState;

	public string StartState;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIScrollRectState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetState(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetState(string name)
	{
		throw null;
	}
}
