using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class UIStateButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	[Serializable]
	public class ClickEventData<PointerEventData> : UnityEvent<PointerEventData>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ClickEventData()
		{
			throw null;
		}
	}

	[Serializable]
	public class ClickEvent<String> : UnityEvent<String>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ClickEvent()
		{
			throw null;
		}
	}

	[Serializable]
	public class OnValueChangeEvent<UIStateButton> : UnityEvent<UIStateButton>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public OnValueChangeEvent()
		{
			throw null;
		}
	}

	[SerializeField]
	private bool clickChangesState;

	public ButtonState[] states;

	public string startState;

	[NonSerialized]
	public string currentState;

	[NonSerialized]
	public int currentStateIndex;

	private bool stateSet;

	public Button.ButtonClickedEvent onClick;

	public ClickEvent<string> onClickState;

	public ClickEventData<PointerEventData> onClickEventData;

	public OnValueChangeEvent<UIStateButton> onValueChanged;

	public bool ClickChangesState
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Button Button
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

	public Image Image
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
	public UIStateButton()
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
	public void ToggleState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetState(int index, bool invokeChange = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetState(string name, bool invokeChange = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Enable(bool enable)
	{
		throw null;
	}
}
