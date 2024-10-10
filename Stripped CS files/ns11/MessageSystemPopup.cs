using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

public class MessageSystemPopup : MonoBehaviour
{
	public delegate void OnDiscard();

	public delegate void OnShowNext(bool discardSelected);

	public delegate void OnShowPrevious(bool discardSelected);

	public enum ButtonTypes
	{
		DISCARDCLOSE = 1
	}

	public TextMeshProUGUI textArea;

	public Button btnClose;

	public Button btnDiscard;

	public Button btnNext;

	public Button btnPrevious;

	public EventTrigger background;

	public TextMeshProUGUI title;

	public OnDiscard onDiscard;

	public OnShowNext onShowNext;

	public OnShowPrevious onShowPrevious;

	public bool showing;

	public bool Showing => showing;

	public void Start()
	{
		GameEvents.onGameSceneLoadRequested.Add(OnSceneLoadRequest);
		btnNext.onClick.AddListener(ShowNext);
		btnPrevious.onClick.AddListener(ShowPrevious);
		background.AddEvent(EventTriggerType.PointerClick, MouseinputClose);
		btnClose.onClick.AddListener(MouseinputClose);
		btnDiscard.onClick.AddListener(MouseinputDiscard);
	}

	public void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneLoadRequest);
	}

	public static MessageSystemPopup InstantiateFromPrefab(MessageSystemPopup prefab)
	{
		return Object.Instantiate(prefab);
	}

	public void OnSceneLoadRequest(GameScenes scene)
	{
		Object.Destroy(base.gameObject);
	}

	public void Show(string title, string text, OnDiscard onDiscard, OnShowNext onShowNext, OnShowPrevious onShowPrevious)
	{
		showing = true;
		base.gameObject.SetActive(value: true);
		this.title.text = title;
		this.onDiscard = onDiscard;
		this.onShowNext = onShowNext;
		this.onShowPrevious = onShowPrevious;
		if (onShowNext == null)
		{
			btnNext.interactable = false;
		}
		else
		{
			btnNext.interactable = true;
		}
		if (onShowPrevious == null)
		{
			btnPrevious.interactable = false;
		}
		else
		{
			btnPrevious.interactable = true;
		}
		textArea.text = text;
	}

	public void Hide()
	{
		showing = false;
		base.gameObject.SetActive(value: false);
	}

	public void MouseinputClose()
	{
		Hide();
	}

	public void MouseinputClose(BaseEventData eventData)
	{
		Hide();
	}

	public void MouseinputDiscard()
	{
		if (btnNext.interactable)
		{
			onShowNext(discardSelected: true);
			return;
		}
		if (btnPrevious.interactable)
		{
			onShowPrevious(discardSelected: true);
			return;
		}
		Hide();
		onDiscard();
	}

	public void ShowNext()
	{
		onShowNext(discardSelected: false);
	}

	public void ShowPrevious()
	{
		onShowPrevious(discardSelected: false);
	}
}
