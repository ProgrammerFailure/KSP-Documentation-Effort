using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace KSP.UI;

public class UIStateText : MonoBehaviour
{
	[Serializable]
	public class TextState
	{
		public string name;

		public string textMessage;

		public Color textColor;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public TextState()
		{
			throw null;
		}
	}

	public TextMeshProUGUI textLabel;

	public TextState[] states;

	public string startState;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIStateText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
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
