using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class UIStateToggleButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	[Serializable]
	public class ButtonState
	{
		public Sprite normal;

		public Sprite highlight;

		public Sprite pressed;

		public Sprite disabled;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ButtonState()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Setup(Button button, Image image)
		{
			throw null;
		}
	}

	public enum BtnState
	{
		True,
		False
	}

	public enum ClickType
	{
		Left,
		Middle,
		Right
	}

	public ButtonState stateTrue;

	public ButtonState stateFalse;

	public Button.ButtonClickedEvent onTrue;

	public Button.ButtonClickedEvent onTrueLeft;

	public Button.ButtonClickedEvent onTrueRight;

	public Button.ButtonClickedEvent onFalse;

	public Button.ButtonClickedEvent onFalseLeft;

	public Button.ButtonClickedEvent onFalseRight;

	public Button.ButtonClickedEvent onClick;

	public Button.ButtonClickedEvent onClickLeft;

	public Button.ButtonClickedEvent onClickRight;

	[SerializeField]
	private bool autoStateChange;

	[SerializeField]
	private BtnState state;

	private BtnState previousState;

	private ButtonState currentState;

	public bool AutoStateChange
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public BtnState State
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool StateBool
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ButtonState CurrentState
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

	public EventTrigger Trigger
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

	public bool interactable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIStateToggleButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetState(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetState(BtnState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleState(int button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerClick(PointerEventData eventData)
	{
		throw null;
	}
}
