using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

[RequireComponent(typeof(Button))]
public class UIButtonToggle : MonoBehaviour
{
	public Image toggleImage;

	public Sprite spriteOn;

	public Sprite spriteOff;

	public bool startOn;

	public bool canToggleOn;

	public bool canToggleOff;

	public Button.ButtonClickedEvent onToggle;

	public Button.ButtonClickedEvent onToggleOff;

	public Button.ButtonClickedEvent onToggleOn;

	private Button button;

	private bool hasBeenSet;

	private bool _state;

	public Button ButtonCtrl
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool state
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
	public UIButtonToggle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetState(bool on)
	{
		throw null;
	}
}
