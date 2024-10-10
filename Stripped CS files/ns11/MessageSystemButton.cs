using System;
using ns2;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

public class MessageSystemButton : MonoBehaviour
{
	public delegate void OnClick(MessageSystemButton button);

	public delegate void OnRightClick(MessageSystemButton button);

	public enum MessageButtonColor
	{
		const_0 = 1,
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
	public Image btnUnread;

	public OnClick onClick;

	public OnRightClick onRightClick;

	[NonSerialized]
	public MessageSystem.Message message;

	public string current;

	public bool over;

	public void SetAsRead()
	{
		btnUnread.gameObject.SetActive(value: false);
	}

	public static MessageSystemButton InstantiateFromPrefab(MessageSystemButton prefab)
	{
		return UnityEngine.Object.Instantiate(prefab);
	}

	public void SetMessage(MessageSystem.Message message, OnClick onClick, OnRightClick onRightClick)
	{
		this.message = message;
		this.onClick = onClick;
		this.onRightClick = onRightClick;
		stateButton.onClickEventData.RemoveAllListeners();
		stateButton.onClickEventData.AddListener(MouseInput);
		SetTexture(message.icon);
		SetButtonColor(message.color);
	}

	public void MouseInput(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			onClick(this);
		}
		else if (eventData.button == PointerEventData.InputButton.Right)
		{
			onRightClick(this);
		}
	}

	public void SetTexture(Texture texture)
	{
		sprite.texture = texture;
	}

	public void SetTexture(ButtonIcons icon)
	{
		Texture texture = null;
		switch (icon)
		{
		case ButtonIcons.DEADLINE:
			texture = iconDeadline;
			break;
		case ButtonIcons.FAIL:
			texture = iconFail;
			break;
		case ButtonIcons.COMPLETE:
			texture = iconComplete;
			break;
		case ButtonIcons.ALERT:
			texture = iconAlert;
			break;
		case ButtonIcons.MESSAGE:
			texture = iconMessage;
			break;
		case ButtonIcons.ACHIEVE:
			texture = iconAchieve;
			break;
		}
		SetTexture(texture);
	}

	public void SetButtonColor(MessageButtonColor color)
	{
		switch (color)
		{
		case MessageButtonColor.const_0:
			stateButton.SetState("red");
			break;
		case MessageButtonColor.GREEN:
			stateButton.SetState("green");
			break;
		case MessageButtonColor.BLUE:
			stateButton.SetState("blue");
			break;
		case MessageButtonColor.YELLOW:
			stateButton.SetState("yellow");
			break;
		case MessageButtonColor.ORANGE:
			stateButton.SetState("orange");
			break;
		}
	}

	public void AddInputDelegate(UnityAction<PointerEventData> del)
	{
		stateButton.onClickEventData.AddListener(del);
	}
}
