using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class MessageSystemButton : MonoBehaviour
{
	public delegate void OnClick(MessageSystemButton button);

	public delegate void OnRightClick(MessageSystemButton button);

	public enum MessageButtonColor
	{
		RED = 1,
		GREEN,
		BLUE,
		YELLOW,
		ORANGE
	}

	public enum ButtonIcons
	{
		DEADLINE = 1,
		FAIL,
		COMPLETE,
		ALERT,
		MESSAGE,
		ACHIEVE
	}

	public UIStateButton stateButton;

	public Texture iconDeadline;

	public Texture iconFail;

	public Texture iconComplete;

	public Texture iconAlert;

	public Texture iconMessage;

	public Texture iconAchieve;

	public UIListItem container;

	public RawImage sprite;

	[SerializeField]
	private Image btnUnread;

	public OnClick onClick;

	public OnRightClick onRightClick;

	[NonSerialized]
	public MessageSystem.Message message;

	private string current;

	private bool over;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MessageSystemButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetAsRead()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MessageSystemButton InstantiateFromPrefab(MessageSystemButton prefab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetMessage(MessageSystem.Message message, OnClick onClick, OnRightClick onRightClick)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInput(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTexture(Texture texture)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTexture(ButtonIcons icon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetButtonColor(MessageButtonColor color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddInputDelegate(UnityAction<PointerEventData> del)
	{
		throw null;
	}
}
