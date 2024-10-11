using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

[RequireComponent(typeof(Button))]
public class TrackingStationObjectButton : MonoBehaviour
{
	public Button.ButtonClickedEvent OnClickLeft;

	public Button.ButtonClickedEvent OnClickRight;

	public Image image;

	public Sprite spriteTrue;

	public Sprite spriteFalse;

	public TextMeshProUGUI textCount;

	public bool state;

	private Button button;

	private bool hover;

	private bool hoverOnLastDown;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TrackingStationObjectButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetState(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCount(int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ShowCount(bool showCount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Lock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Unlock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("UpdateState")]
	private void UpdateState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnMouseEnter(BaseEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnMouseExit(BaseEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}
}
