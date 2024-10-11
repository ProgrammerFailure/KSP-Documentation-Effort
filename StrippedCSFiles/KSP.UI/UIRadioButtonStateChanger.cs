using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI;

[RequireComponent(typeof(UIRadioButton))]
public class UIRadioButtonStateChanger : MonoBehaviour
{
	[Serializable]
	public class RadioButtonState
	{
		public string name;

		public ButtonState stateTrue;

		public ButtonState stateFalse;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public RadioButtonState()
		{
			throw null;
		}
	}

	private UIRadioButton button;

	public RadioButtonState[] states;

	[NonSerialized]
	public string currentState;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIRadioButtonStateChanger()
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
