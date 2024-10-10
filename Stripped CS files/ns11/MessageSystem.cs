using System;
using System.Collections.Generic;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ns11;

public class MessageSystem : UIApp
{
	public class Message
	{
		public string messageTitle;

		public string message;

		public MessageSystemButton button;

		public MessageSystemButton.MessageButtonColor color;

		public MessageSystemButton.ButtonIcons icon;

		public UIListItem advancedListItem;

		public TextMeshProUGUI advancedListItemText;

		public bool IsRead;

		public bool instantiated { get; set; }

		public Message(string messageTitle, string message, MessageSystemButton.MessageButtonColor color, MessageSystemButton.ButtonIcons icon)
		{
			this.messageTitle = messageTitle;
			this.message = message;
			this.color = color;
			this.icon = icon;
			if (Ready)
			{
				Instantiate();
			}
			else
			{
				Instance.messageQueue.AddUnique(this);
			}
		}

		public void Instantiate()
		{
			if (!instantiated)
			{
				button = MessageSystemButton.InstantiateFromPrefab(Instance.buttonPrefab);
				button.SetMessage(this, Instance.OnClick, Instance.OnRightClick);
				InstantiateAdvancedItem();
				instantiated = true;
			}
		}

		public string GetRichTextMessageTitle(string messageTitle, bool IsRead = false)
		{
			string text = StringBuilderCache.Format("<color=#e5e5e5ff>");
			if (!IsRead)
			{
				text += StringBuilderCache.Format("<b>");
			}
			text += StringBuilderCache.Format(Localizer.Format(messageTitle));
			if (!IsRead)
			{
				text += StringBuilderCache.Format("</b>");
			}
			return text + StringBuilderCache.Format("</color>");
		}

		public void InstantiateAdvancedItem()
		{
			advancedListItem = UnityEngine.Object.Instantiate(Instance.advancedMessagePrefab);
			Transform transform = advancedListItem.transform.Find("MessageName");
			if (transform != null)
			{
				advancedListItemText = transform.gameObject.GetComponent<TextMeshProUGUI>();
				if (advancedListItemText != null)
				{
					advancedListItemText.text = GetRichTextMessageTitle(messageTitle, IsRead);
				}
			}
			MessageSystemAdvancedButton component = advancedListItem.transform.GetComponent<MessageSystemAdvancedButton>();
			if (component != null)
			{
				switch (color)
				{
				case MessageSystemButton.MessageButtonColor.const_0:
					component.buttonImage.sprite = component.backgroundRed;
					break;
				case MessageSystemButton.MessageButtonColor.GREEN:
					component.buttonImage.sprite = component.backgroundGreen;
					break;
				case MessageSystemButton.MessageButtonColor.BLUE:
					component.buttonImage.sprite = component.backgroundBlue;
					break;
				case MessageSystemButton.MessageButtonColor.YELLOW:
					component.buttonImage.sprite = component.backgroundYellow;
					break;
				case MessageSystemButton.MessageButtonColor.ORANGE:
					component.buttonImage.sprite = component.backgroundOrange;
					break;
				}
				switch (icon)
				{
				case MessageSystemButton.ButtonIcons.DEADLINE:
					component.iconImage.texture = component.iconDeadline;
					break;
				case MessageSystemButton.ButtonIcons.FAIL:
					component.iconImage.texture = component.iconFail;
					break;
				case MessageSystemButton.ButtonIcons.COMPLETE:
					component.iconImage.texture = component.iconComplete;
					break;
				case MessageSystemButton.ButtonIcons.ALERT:
					component.iconImage.texture = component.iconAlert;
					break;
				case MessageSystemButton.ButtonIcons.MESSAGE:
					component.iconImage.texture = component.iconMessage;
					break;
				case MessageSystemButton.ButtonIcons.ACHIEVE:
					component.iconImage.texture = component.iconAchieve;
					break;
				}
			}
			PointerClickHandler component2 = advancedListItem.GetComponent<PointerClickHandler>();
			component2.onPointerClick.AddListener(Instance.MouseInputAdvMessageItem);
			component2.Data = this;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is Message message))
			{
				return false;
			}
			if (messageTitle.Equals(message.messageTitle) && this.message.Equals(message.message) && color.Equals(message.color))
			{
				return icon.Equals(message.icon);
			}
			return false;
		}

		public bool Equals(Message m)
		{
			if (m == null)
			{
				return false;
			}
			if (messageTitle.Equals(m.messageTitle) && message.Equals(m.message) && color.Equals(m.color))
			{
				return icon.Equals(m.icon);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return messageTitle.GetHashCode_Net35() ^ message.GetHashCode_Net35() ^ color.GetHashCode() ^ icon.GetHashCode();
		}
	}

	public RectTransform anchor;

	public MessageSystemButton buttonPrefab;

	public UIList listResizer;

	public int maxShowingMessages = 5;

	public TextMeshProUGUI counterText;

	public MessageSystemPopup popupPrefab;

	[SerializeField]
	public TextMeshProUGUI launcherTextPrfab;

	[SerializeField]
	public RectTransform buttonStorage;

	[SerializeField]
	public MessageSystemAppFrame appFramePrefab;

	public MessageSystemAppFrame appFrame;

	[SerializeField]
	public PointerClickHandler deleteButton;

	public PopupDialog confirmPopup;

	[SerializeField]
	public UIListItem advancedMessagePrefab;

	public MessageSystemButton lastClickedButton;

	public List<MessageSystemButton> messageList = new List<MessageSystemButton>();

	public List<Message> messageQueue = new List<Message>();

	public int showingMessages;

	public bool isFlashing;

	public MessageSystemPopup popup;

	public TextMeshProUGUI launcherTextSprite;

	public RectTransform rectTransform;

	public bool _started;

	public bool advancedMessages;

	public bool initialSetupComplete;

	public static MessageSystem Instance { get; set; }

	public static bool Ready { get; set; }

	public override void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning("MessageSystem already exist, destroying this instance");
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		rectTransform = base.transform as RectTransform;
		setupMode();
		GameEvents.onGameStatePostLoad.Add(LoadMessages);
		GameEvents.onGameStateSave.Add(SaveMessages);
		GameEvents.onGameSceneLoadRequested.Add(OnSceneLoadRequested);
		GameEvents.onLevelWasLoadedGUIReady.Add(OnSceneLoaded);
		GameEvents.OnGameSettingsApplied.Add(setupMode);
		base.Awake();
	}

	public void setupMode()
	{
		advancedMessages = GameSettings.ADVANCED_MESSAGESAPP;
		if (HighLogic.LoadedScene == GameScenes.MAINMENU)
		{
			advancedMessages = false;
		}
		if (appFrame == null)
		{
			appFrame = UnityEngine.Object.Instantiate(appFramePrefab);
			appFrame.transform.SetParent(base.transform, worldPositionStays: false);
			appFrame.transform.localPosition = Vector3.zero;
		}
		if (appFrame != null && !initialSetupComplete && HighLogic.LoadedScene != GameScenes.MAINMENU)
		{
			appFrame.Setup(base.appLauncherButton, "Messages", "#autoLOC_8006065", 225, 150);
			appFrame.AddGlobalInputDelegate(base.MouseInput_PointerEnter, base.MouseInput_PointerExit);
			deleteButton.onPointerClick.AddListener(OnDeleteAllMessagesClicked);
			appFrame.deleteHandler.onPointerClick.AddListener(OnDeleteAllMessagesClicked);
			ApplicationLauncher.Instance.AddOnRepositionCallback(appFrame.Reposition);
			initialSetupComplete = true;
		}
		if (base.appIsLive)
		{
			DisplayApp();
		}
		else if (HighLogic.LoadedScene != GameScenes.MAINMENU)
		{
			HideApp();
		}
	}

	public void OnDeleteAllMessagesClicked(PointerEventData eventData)
	{
		if (GameSettings.CONFIRM_MESSAGE_DELETION && messageList.Count > 0)
		{
			if (confirmPopup == null)
			{
				confirmPopup = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("#autoLOC_8012004", Localizer.Format("#autoLOC_8012005", messageList.Count), Localizer.Format("#autoLOC_8012004"), null, 350f, new DialogGUIButton(Localizer.Format("#autoLOC_439855"), DeleteAllMessages, dismissOnSelect: true), new DialogGUIButton(Localizer.Format("#autoLOC_439856"), null, dismissOnSelect: true), new DialogGUIToggle(!GameSettings.CONFIRM_MESSAGE_DELETION, Localizer.Format("#autoLOC_360842"), delegate(bool b)
				{
					GameSettings.CONFIRM_MESSAGE_DELETION = !b;
				})), persistAcrossScenes: false, null);
			}
			else
			{
				confirmPopup.gameObject.SetActive(value: true);
			}
		}
		else
		{
			DeleteAllMessages();
		}
	}

	public void DeleteAllMessages()
	{
		ClearAllMessages();
	}

	public override bool OnAppAboutToStart()
	{
		bool result = !_started;
		_started = true;
		return result;
	}

	public override void OnAppDestroy()
	{
		GameEvents.onGameStatePostLoad.Remove(LoadMessages);
		GameEvents.onGameStateSave.Remove(SaveMessages);
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneLoadRequested);
		GameEvents.onLevelWasLoadedGUIReady.Remove(OnSceneLoaded);
		GameEvents.OnGameSettingsApplied.Remove(setupMode);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public override void OnAppInitialized()
	{
		Debug.Log("[MessageSystem] OnAppInitialized");
		Reposition();
		UpdateLauncherButtonCount();
		HideApp();
		GameEvents.onGUIMessageSystemReady.Fire();
	}

	public override void Reposition()
	{
		Debug.Log("[MessageSystem] Reposition " + Time.timeSinceLevelLoad + " " + Time.frameCount);
		if (base.transform.parent != ApplicationLauncher.Instance.appSpace)
		{
			base.transform.SetParent(ApplicationLauncher.Instance.appSpace, worldPositionStays: false);
			base.transform.SetLocalPositionZ();
		}
		if (ApplicationLauncher.Instance.IsPositionedAtTop)
		{
			rectTransform.anchoredPosition = base.appLauncherButton.GetAnchorTopRight();
		}
		else
		{
			RepositionDynamic();
		}
	}

	public void RepositionDynamic()
	{
		if (!ApplicationLauncher.Instance.IsPositionedAtTop && base.appLauncherButton != null)
		{
			base.transform.position = new Vector3(base.appLauncherButton.GetAnchorUR().x, base.appLauncherButton.GetAnchorUR().y + rectTransform.sizeDelta.y * GameSettings.UI_SCALE * GameSettings.UI_SCALE_APPS, base.appLauncherButton.GetAnchorUR().z);
		}
	}

	public override void DisplayApp()
	{
		if (advancedMessages)
		{
			if (appFrame != null)
			{
				appFrame.gameObject.SetActive(value: true);
			}
			if (anchor != null)
			{
				anchor.gameObject.SetActive(value: false);
			}
			return;
		}
		if (anchor != null)
		{
			anchor.gameObject.SetActive(value: true);
		}
		if (appFrame != null)
		{
			appFrame.gameObject.SetActive(value: false);
		}
		Resize();
	}

	public override void HideApp()
	{
		if (appFrame != null)
		{
			appFrame.gameObject.SetActive(value: false);
		}
		if (anchor != null)
		{
			anchor.gameObject.SetActive(value: false);
		}
	}

	public override ApplicationLauncher.AppScenes GetAppScenes()
	{
		return ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.MAPVIEW | ApplicationLauncher.AppScenes.flag_5 | ApplicationLauncher.AppScenes.flag_6 | ApplicationLauncher.AppScenes.TRACKSTATION;
	}

	public override Vector3 GetAppScreenPos(Vector3 defaultAnchorPos)
	{
		return new Vector3(ApplicationLauncher.Instance.transform.position.x, defaultAnchorPos.y, defaultAnchorPos.z);
	}

	public void LoadMessages(Game game)
	{
		LoadMessages(game.config);
	}

	public void LoadMessages(ConfigNode gameNode)
	{
		Debug.Log("[MessageSystem] Load Messages");
		ClearAllMessages();
		ConfigNode configNode = gameNode.GetNode("GAME");
		if (configNode == null && gameNode.name != "GAME")
		{
			Debug.LogWarning("MessageSystem: Cannot load messages - GAME node does not exist");
			return;
		}
		if (gameNode.name == "GAME")
		{
			configNode = gameNode;
		}
		ConfigNode node = configNode.GetNode("MESSAGESYSTEM");
		if (node == null)
		{
			Debug.LogWarning("MessageSystem: Cannot load messages - MESSAGESYSTEM node does not exist");
			return;
		}
		if (node.HasValue("flash"))
		{
			isFlashing = bool.Parse(node.GetValue("flash"));
		}
		else
		{
			isFlashing = false;
		}
		ConfigNode[] nodes = node.GetNodes("MESSAGE");
		int num = nodes.Length;
		while (num-- > 0)
		{
			Message message = LoadMessage(nodes[num]);
			if (message != null)
			{
				AddMessageInternalNonDuplicate(message, playAnim: false, queue: true);
			}
		}
		UpdateLauncherButtonCount();
		if (isFlashing)
		{
			UpdateLauncherButtonPlayAnim();
		}
		else
		{
			UpdateLauncherButtonStopAnim();
		}
	}

	public Message LoadMessage(ConfigNode node)
	{
		string value = node.GetValue("title");
		string value2 = node.GetValue("message");
		Message result = new Message(color: (MessageSystemButton.MessageButtonColor)int.Parse(node.GetValue("color")), icon: (MessageSystemButton.ButtonIcons)int.Parse(node.GetValue("icon")), messageTitle: value.Replace("~n", "\n"), message: value2.Replace("~n", "\n"))
		{
			IsRead = bool.Parse(node.GetValue("read"))
		};
		if (value != null && value2 != null)
		{
			return result;
		}
		Debug.LogError("MessageSystem: Message config is invalid");
		return null;
	}

	public void SaveMessages(ConfigNode gameNode)
	{
		if (HighLogic.LoadedSceneIsGame)
		{
			Debug.Log("[MessageSystem] Save Messages");
			ConfigNode configNode = gameNode.AddNode("MESSAGESYSTEM");
			configNode.AddValue("flash", isFlashing);
			int i = 0;
			for (int count = messageList.Count; i < count; i++)
			{
				Message message = messageList[i].message;
				ConfigNode cNode = configNode.AddNode("MESSAGE");
				SaveMessage(cNode, message);
			}
		}
	}

	public void SaveMessage(ConfigNode cNode, Message c)
	{
		cNode.AddValue("title", c.messageTitle.Replace("\n", "~n"));
		cNode.AddValue("message", c.message.Replace("\n", "~n"));
		cNode.AddValue("color", (int)c.color);
		cNode.AddValue("icon", (int)c.icon);
		cNode.AddValue("read", c.IsRead.ToString());
	}

	public void OnSceneLoadRequested(GameScenes scene)
	{
		Ready = false;
		if (scene == GameScenes.MAINMENU)
		{
			ClearAllMessages();
		}
	}

	public void OnSceneLoaded(GameScenes scene)
	{
		setupMode();
		Ready = true;
		int i = 0;
		for (int count = messageQueue.Count; i < count; i++)
		{
			AddMessageInternalNonDuplicate(messageQueue[i], playAnim: false, queue: false);
		}
		messageQueue.Clear();
		UpdateLauncherButtonCount();
	}

	public void OnClick(MessageSystemButton button)
	{
		ShowMessage(button);
	}

	public void OnRightClick(MessageSystemButton button)
	{
		if (popup != null && popup.Showing && lastClickedButton == button)
		{
			popup.Hide();
		}
		lastClickedButton = button;
		DiscardSelected();
	}

	public void MouseInputAdvMessageItem(PointerEventData eventData)
	{
		if (eventData.pointerPress.GetComponent<PointerClickHandler>().Data is Message message)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				OnClick(message.button);
			}
			else if (eventData.button == PointerEventData.InputButton.Right)
			{
				OnRightClick(message.button);
			}
		}
	}

	public void UpdateLauncherButtonCount()
	{
		if (base.appLauncherButton != null)
		{
			if (launcherTextSprite == null)
			{
				launcherTextSprite = UnityEngine.Object.Instantiate(launcherTextPrfab);
				launcherTextSprite.gameObject.transform.SetParent(base.appLauncherButton.gameObject.GetChild("Image").transform, worldPositionStays: false);
			}
			launcherTextSprite.text = string.Concat(messageList.Count);
		}
	}

	public void UpdateLauncherButtonPlayAnim()
	{
		if (ApplicationLauncher.Instance != null && ApplicationLauncher.Ready && base.appLauncherButton != null)
		{
			isFlashing = true;
			base.appLauncherButton.PlayAnim(ApplicationLauncherButton.AnimatedIconType.NOTIFICATION, 5f);
		}
	}

	public void UpdateLauncherButtonStopAnim()
	{
		if (ApplicationLauncher.Instance != null && ApplicationLauncher.Ready && base.appLauncherButton != null)
		{
			isFlashing = false;
			base.appLauncherButton.StopAnim();
		}
	}

	public bool HasNextMessage()
	{
		return messageList.FindIndex((MessageSystemButton btn) => btn == lastClickedButton) + 1 != messageList.Count;
	}

	public bool HasPreviousMessage()
	{
		return messageList.FindIndex((MessageSystemButton btn) => btn == lastClickedButton) != 0;
	}

	public void ShowNextMessage(bool discardSelected)
	{
		int num = messageList.FindIndex((MessageSystemButton btn) => btn == lastClickedButton);
		MessageSystemButton btn2 = messageList.Find((MessageSystemButton btn) => btn == lastClickedButton);
		MessageSystemButton button = messageList[num + 1];
		if (discardSelected)
		{
			DiscardMessage(btn2);
		}
		ShowMessage(button);
	}

	public void ShowPreviousMessage(bool discardSelected)
	{
		int num = messageList.FindIndex((MessageSystemButton btn) => btn == lastClickedButton);
		MessageSystemButton btn2 = messageList.Find((MessageSystemButton btn) => btn == lastClickedButton);
		MessageSystemButton button = messageList[num - 1];
		if (discardSelected)
		{
			DiscardMessage(btn2);
		}
		ShowMessage(button);
	}

	public void ShowMessage(MessageSystemButton button)
	{
		lastClickedButton = button;
		bool yield = false;
		if (popup == null)
		{
			InstantiatePopup();
			yield = true;
		}
		ShowMessage(button, yield);
	}

	public void ShowMessage(MessageSystemButton button, bool yield)
	{
		if (HasNextMessage() && HasPreviousMessage())
		{
			popup.Show(button.message.messageTitle, button.message.message, DiscardSelected, ShowNextMessage, ShowPreviousMessage);
		}
		else if (!HasNextMessage() && HasPreviousMessage())
		{
			popup.Show(button.message.messageTitle, button.message.message, DiscardSelected, null, ShowPreviousMessage);
		}
		else if (HasNextMessage() && !HasPreviousMessage())
		{
			popup.Show(button.message.messageTitle, button.message.message, DiscardSelected, ShowNextMessage, null);
		}
		else if (!HasNextMessage() && !HasPreviousMessage())
		{
			popup.Show(button.message.messageTitle, button.message.message, DiscardSelected, null, null);
		}
		button.SetAsRead();
		button.message.IsRead = true;
		if (button.message.advancedListItemText != null)
		{
			button.message.advancedListItemText.text = button.message.GetRichTextMessageTitle(button.message.messageTitle, button.message.IsRead);
		}
	}

	public void InstantiatePopup()
	{
		popup = MessageSystemPopup.InstantiateFromPrefab(popupPrefab);
		popup.transform.SetParent(ApplicationLauncher.Instance.appSpace.gameObject.transform, worldPositionStays: false);
	}

	public void AddMessage(Message message, bool animate = true)
	{
		if (HighLogic.LoadedScene == GameScenes.MAINMENU)
		{
			Debug.LogWarning("[MessageSystem] is live but not accepting messages in MainMenu.");
		}
		else
		{
			AddMessageInternal(message, animate, queue: true);
		}
	}

	public List<Message> FindMessages(Func<Message, bool> where)
	{
		List<Message> list = new List<Message>();
		if (messageQueue != null && messageQueue.Count > 0)
		{
			int count = messageQueue.Count;
			while (count-- > 0)
			{
				if (messageQueue[count] != null && where(messageQueue[count]))
				{
					list.Add(messageQueue[count]);
				}
			}
		}
		if (messageList != null && messageList.Count > 0)
		{
			int count2 = messageList.Count;
			while (count2-- > 0)
			{
				if (!(messageList[count2] == null) && messageList[count2].message != null && where(messageList[count2].message))
				{
					list.Add(messageList[count2].message);
				}
			}
		}
		return list;
	}

	public void AddMessageInternalNonDuplicate(Message message, bool playAnim, bool queue)
	{
		if (!messageList.Exists((MessageSystemButton a) => a.message.Equals(message)) && !messageQueue.Exists((Message a) => a.message.Equals(message)))
		{
			AddMessageInternal(message, playAnim: false, queue);
		}
	}

	public void AddMessageInternal(Message message, bool playAnim, bool queue)
	{
		if (Ready)
		{
			message.Instantiate();
			listResizer.InsertItem(message.button.container, 0);
			appFrame.scrollList.AddItem(message.advancedListItem);
			showingMessages++;
			if (messageList.Count >= maxShowingMessages)
			{
				Transform obj = listResizer.GetUilistItemAt(maxShowingMessages).gameObject.transform;
				listResizer.RemoveItem(maxShowingMessages);
				obj.SetParent(buttonStorage, worldPositionStays: false);
			}
			messageList.Insert(0, message.button);
			UpdateLauncherButtonCount();
			if (playAnim)
			{
				UpdateLauncherButtonPlayAnim();
			}
			if (message.IsRead)
			{
				message.button.SetAsRead();
			}
			Resize();
		}
		else if (queue)
		{
			messageQueue.Add(message);
		}
	}

	public void DiscardSelected()
	{
		listResizer.RemoveItem(lastClickedButton.container, deleteItem: true);
		appFrame.scrollList.RemoveItem(lastClickedButton.message.advancedListItem, deleteItem: true);
		messageList.Remove(lastClickedButton);
		lastClickedButton = null;
		showingMessages--;
		if (messageList.Count >= maxShowingMessages)
		{
			listResizer.AddItem(messageList[maxShowingMessages - 1].container);
			if (showingMessages < maxShowingMessages)
			{
				showingMessages++;
			}
		}
		UpdateLauncherButtonCount();
		Resize();
	}

	public void DiscardMessage(MessageSystemButton btn)
	{
		int num = messageList.FindIndex((MessageSystemButton b) => b == btn);
		listResizer.RemoveItem(btn.container, deleteItem: true);
		appFrame.scrollList.RemoveItem(btn.message.advancedListItem, deleteItem: true);
		messageList.Remove(btn);
		showingMessages--;
		if (messageList.Count >= maxShowingMessages && num < maxShowingMessages)
		{
			listResizer.AddItem(messageList[maxShowingMessages - 1].container);
			if (showingMessages < maxShowingMessages)
			{
				showingMessages++;
			}
		}
		UpdateLauncherButtonCount();
		Resize();
	}

	public void ClearAllMessages()
	{
		messageList.Clear();
		listResizer.Clear(destroyElements: true);
		if (appFrame != null)
		{
			appFrame.scrollList.Clear(destroyElements: true);
		}
		buttonStorage.ClearChildrenImmediate();
		lastClickedButton = null;
		showingMessages = 0;
		UpdateLauncherButtonCount();
		Resize();
	}

	public void Resize()
	{
		if (messageList.Count > 0)
		{
			if (messageList.Count > maxShowingMessages)
			{
				counterText.text = "+ " + (messageList.Count - maxShowingMessages);
				rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 22 + maxShowingMessages * 32);
			}
			else
			{
				counterText.text = "";
				rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 16 + messageList.Count * 32);
			}
		}
		else
		{
			counterText.text = Localizer.Format("#autoLOC_6003083");
		}
		RepositionDynamic();
	}
}
