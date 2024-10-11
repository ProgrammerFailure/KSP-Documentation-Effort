using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace KSP.UI;

public class UIStatePanel : MonoBehaviour
{
	[Serializable]
	public class PanelState
	{
		public string name;

		public RectTransform[] rectTransforms;

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

	[Serializable]
	public class TextState
	{
		public string name;

		public TextMeshProUGUI text;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public TextState()
		{
			throw null;
		}
	}

	public PanelState[] panelList;

	[NonSerialized]
	public PanelState CurrentState;

	public string StartState;

	public TextState[] textList;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIStatePanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetText(string key, string value, Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTextOverflowMode(string key, TextOverflowModes overflowMode, bool wordWrap = true)
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
