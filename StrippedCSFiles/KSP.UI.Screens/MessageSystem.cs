using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KSP.UI.Screens;

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

		public bool instantiated
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
		public Message(string messageTitle, string message, MessageSystemButton.MessageButtonColor color, MessageSystemButton.ButtonIcons icon)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Instantiate()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string GetRichTextMessageTitle(string messageTitle, bool IsRead = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void InstantiateAdvancedItem()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override bool Equals(object obj)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool Equals(Message m)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override int GetHashCode()
		{
			throw null;
		}
	}

	public RectTransform anchor;

	public MessageSystemButton buttonPrefab;

	public UIList listResizer;

	public int maxShowingMessages;

	public TextMeshProUGUI counterText;

	public MessageSystemPopup popupPrefab;

	[SerializeField]
	private TextMeshProUGUI launcherTextPrfab;

	[SerializeField]
	private RectTransform buttonStorage;

	[SerializeField]
	private MessageSystemAppFrame appFramePrefab;

	private MessageSystemAppFrame appFrame;

	[SerializeField]
	private PointerClickHandler deleteButton;

	private PopupDialog confirmPopup;

	[SerializeField]
	internal UIListItem advancedMessagePrefab;

	private MessageSystemButton lastClickedButton;

	private List<MessageSystemButton> messageList;

	internal List<Message> messageQueue;

	private int showingMessages;

	private bool isFlashing;

	private MessageSystemPopup popup;

	private TextMeshProUGUI launcherTextSprite;

	private RectTransform rectTransform;

	private bool _started;

	private bool advancedMessages;

	private bool initialSetupComplete;

	public static MessageSystem Instance
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

	public static bool Ready
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
	public MessageSystem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void setupMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDeleteAllMessagesClicked(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DeleteAllMessages()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool OnAppAboutToStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAppDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAppInitialized()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Reposition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RepositionDynamic()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void DisplayApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void HideApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override ApplicationLauncher.AppScenes GetAppScenes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override Vector3 GetAppScreenPos(Vector3 defaultAnchorPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadMessages(Game game)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadMessages(ConfigNode gameNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Message LoadMessage(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SaveMessages(ConfigNode gameNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SaveMessage(ConfigNode cNode, Message c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoadRequested(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoaded(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnClick(MessageSystemButton button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnRightClick(MessageSystemButton button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInputAdvMessageItem(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateLauncherButtonCount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateLauncherButtonPlayAnim()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateLauncherButtonStopAnim()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool HasNextMessage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool HasPreviousMessage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowNextMessage(bool discardSelected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowPreviousMessage(bool discardSelected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowMessage(MessageSystemButton button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowMessage(MessageSystemButton button, bool yield)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InstantiatePopup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddMessage(Message message, bool animate = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<Message> FindMessages(Func<Message, bool> where)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddMessageInternalNonDuplicate(Message message, bool playAnim, bool queue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddMessageInternal(Message message, bool playAnim, bool queue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DiscardSelected()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DiscardMessage(MessageSystemButton btn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearAllMessages()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Resize()
	{
		throw null;
	}
}
