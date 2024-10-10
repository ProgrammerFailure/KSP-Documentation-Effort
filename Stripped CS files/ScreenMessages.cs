using System.Collections.Generic;
using ns11;
using ns9;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMessages : MonoBehaviour
{
	public ScreenMessagesText textPrefab;

	public RectTransform lowerCenter;

	public RectTransform upperCenter;

	public RectTransform upperLeft;

	public RectTransform upperRight;

	public RectTransform kerbalEva;

	public bool useLifetimeGradient;

	public Gradient lifetimeGradient;

	public Color defaultColor = Color.white;

	public List<ScreenMessage> activeMessages = new List<ScreenMessage>();

	public float[] lastPendingMessageAdded = new float[5];

	public int[] pendingMessages = new int[5];

	public bool[] failedAreas = new bool[5];

	public ScreenMessage[] firstMessage = new ScreenMessage[5];

	public float[] waitTime = new float[5];

	public float MAX_INTERVAL = 0.35f;

	public float MAX_WAIT = 8f;

	public bool sceneLoadInProgress;

	public static ScreenMessages Instance { get; set; }

	public List<ScreenMessage> ActiveMessages
	{
		get
		{
			return activeMessages;
		}
		set
		{
			activeMessages = value;
		}
	}

	public void Awake()
	{
		if ((bool)Instance)
		{
			Object.Destroy(this);
			return;
		}
		Instance = this;
		kerbalEva.localPosition = new Vector3(Mathf.Clamp(GameSettings.EVA_SCREEN_MESSAGE_X, -Screen.width / 2, Screen.width / 2), Mathf.Clamp(GameSettings.EVA_SCREEN_MESSAGE_Y, -(Screen.height / 2) + 60, Screen.height / 2), 0f);
		activeMessages = new List<ScreenMessage>();
		lastPendingMessageAdded[3] = 0f;
		lastPendingMessageAdded[0] = 0f;
		lastPendingMessageAdded[1] = 0f;
		lastPendingMessageAdded[2] = 0f;
		lastPendingMessageAdded[4] = 0f;
		waitTime[3] = MAX_INTERVAL;
		waitTime[0] = MAX_INTERVAL;
		waitTime[1] = MAX_INTERVAL;
		waitTime[2] = MAX_INTERVAL;
		waitTime[4] = MAX_INTERVAL;
	}

	public void Start()
	{
		GameEvents.onGameSceneLoadRequested.Add(OnGameSceneLoadRequested);
		GameEvents.onLevelWasLoaded.Add(OnLevelLoaded);
	}

	public void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnGameSceneLoadRequested);
		GameEvents.onLevelWasLoaded.Remove(OnLevelLoaded);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void OnGameSceneLoadRequested(GameScenes scene)
	{
		DestroyMessages();
		sceneLoadInProgress = true;
	}

	public void OnLevelLoaded(GameScenes scene)
	{
		sceneLoadInProgress = false;
	}

	public void Update()
	{
		int count = activeMessages.Count;
		while (count-- > 0)
		{
			ScreenMessage screenMessage = activeMessages[count];
			if (screenMessage != null && !(screenMessage.textInstance == null) && !(screenMessage.textInstance.text == null))
			{
				if (screenMessage.startTime == 0f)
				{
					screenMessage.startTime = Time.realtimeSinceStartup;
				}
				if (Time.realtimeSinceStartup >= screenMessage.startTime + screenMessage.duration && screenMessage.textInstance.isActiveAndEnabled)
				{
					DestroyMessage(screenMessage);
				}
				else if (useLifetimeGradient && screenMessage.textInstance.isActiveAndEnabled)
				{
					float time = Mathf.Clamp01((Time.realtimeSinceStartup - screenMessage.startTime) / screenMessage.duration);
					screenMessage.textInstance.text.color = lifetimeGradient.Evaluate(time) * screenMessage.color;
				}
			}
			else
			{
				activeMessages.RemoveAt(count);
			}
		}
		firstMessage[3] = null;
		firstMessage[0] = null;
		firstMessage[1] = null;
		firstMessage[2] = null;
		firstMessage[4] = null;
		pendingMessages[3] = 0;
		pendingMessages[0] = 0;
		pendingMessages[1] = 0;
		pendingMessages[2] = 0;
		pendingMessages[4] = 0;
		failedAreas[3] = false;
		failedAreas[0] = false;
		failedAreas[1] = false;
		failedAreas[2] = false;
		failedAreas[4] = false;
		for (int i = 0; i < activeMessages.Count; i++)
		{
			ScreenMessage screenMessage2 = activeMessages[i];
			RectTransform rectTransform = screenMessage2.textInstance.transform.parent as RectTransform;
			ScreenMessageStyle screenMessageStyle = ((rectTransform == lowerCenter) ? ScreenMessageStyle.LOWER_CENTER : ((!(rectTransform == upperCenter)) ? ((rectTransform == upperLeft) ? ScreenMessageStyle.UPPER_LEFT : ((rectTransform == upperRight) ? ScreenMessageStyle.UPPER_RIGHT : ScreenMessageStyle.KERBAL_EVA)) : ScreenMessageStyle.UPPER_CENTER));
			if (!screenMessage2.textInstance.isActiveAndEnabled)
			{
				if (failedAreas[(int)screenMessageStyle])
				{
					pendingMessages[(int)screenMessageStyle]++;
					continue;
				}
				if (!screenMessage2.textInstance.gameObject.activeSelf)
				{
					screenMessage2.textInstance.gameObject.SetActive(value: true);
				}
				VerticalLayoutGroup component = rectTransform.GetComponent<VerticalLayoutGroup>();
				LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
				bool flag = false;
				if (component.preferredHeight > rectTransform.sizeDelta.y)
				{
					float num = waitTime[(int)screenMessageStyle];
					if (lastPendingMessageAdded[(int)screenMessageStyle] + num < Time.realtimeSinceStartup)
					{
						flag = component.preferredHeight - screenMessage2.textInstance.GetComponent<RectTransform>().sizeDelta.y < rectTransform.sizeDelta.y;
						i--;
						DestroyMessage(firstMessage[(int)screenMessageStyle]);
					}
					if (!flag)
					{
						if (screenMessage2.textInstance.gameObject.activeSelf)
						{
							screenMessage2.textInstance.gameObject.SetActive(value: false);
						}
						pendingMessages[(int)screenMessageStyle]++;
						failedAreas[(int)screenMessageStyle] = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					screenMessage2.startTime = Time.realtimeSinceStartup;
					lastPendingMessageAdded[(int)screenMessageStyle] = Time.realtimeSinceStartup;
				}
			}
			else if (firstMessage[(int)screenMessageStyle] == null)
			{
				firstMessage[(int)screenMessageStyle] = screenMessage2;
			}
		}
		for (int j = 0; j < 4; j++)
		{
			if (pendingMessages[j] == 0)
			{
				lastPendingMessageAdded[j] = Time.realtimeSinceStartup;
			}
			waitTime[j] = Mathf.Min(MAX_WAIT / (float)pendingMessages[j], MAX_INTERVAL);
		}
	}

	public static ScreenMessage PostScreenMessage(string message, float duration, ScreenMessageStyle style, bool persist)
	{
		ScreenMessage result = (Instance ? Instance.PostMessage(null, message, Instance.textPrefab, duration, style) : null);
		if (persist)
		{
			SendStateMessage(Localizer.Format("#autoLOC_6006000"), message, MessageSystemButton.MessageButtonColor.GREEN, MessageSystemButton.ButtonIcons.MESSAGE);
		}
		return result;
	}

	public static ScreenMessage PostScreenMessage(string message, float duration, ScreenMessageStyle style, Color color)
	{
		if (!Instance)
		{
			return null;
		}
		return Instance.PostMessage(null, message, Instance.textPrefab, duration, style, color);
	}

	public static ScreenMessage PostScreenMessage(string message, float duration, ScreenMessageStyle style)
	{
		if (!Instance)
		{
			return null;
		}
		return Instance.PostMessage(null, message, Instance.textPrefab, duration, style);
	}

	public static ScreenMessage PostScreenMessage(string message, float duration, ScreenMessageStyle style, ScreenMessage msg)
	{
		if (!Instance)
		{
			return null;
		}
		return Instance.PostMessage(msg, message, Instance.textPrefab, duration, style);
	}

	public static ScreenMessage PostScreenMessage(string message, float duration, bool persist)
	{
		ScreenMessage result = (Instance ? Instance.PostMessage(null, message, Instance.textPrefab, duration, ScreenMessageStyle.UPPER_CENTER) : null);
		if (persist)
		{
			SendStateMessage(Localizer.Format("#autoLOC_6006000"), message, MessageSystemButton.MessageButtonColor.GREEN, MessageSystemButton.ButtonIcons.MESSAGE);
		}
		return result;
	}

	public static ScreenMessage PostScreenMessage(string message, float duration)
	{
		if (!Instance)
		{
			return null;
		}
		return Instance.PostMessage(null, message, Instance.textPrefab, duration, ScreenMessageStyle.UPPER_CENTER);
	}

	public static ScreenMessage PostScreenMessage(string message, bool persist)
	{
		ScreenMessage result = (Instance ? Instance.PostMessage(null, message, Instance.textPrefab, 3f, ScreenMessageStyle.UPPER_CENTER) : null);
		if (persist)
		{
			SendStateMessage(Localizer.Format("#autoLOC_6006000"), message, MessageSystemButton.MessageButtonColor.GREEN, MessageSystemButton.ButtonIcons.MESSAGE);
		}
		return result;
	}

	public static ScreenMessage PostScreenMessage(string message)
	{
		if (!Instance)
		{
			return null;
		}
		return Instance.PostMessage(null, message, Instance.textPrefab, 3f, ScreenMessageStyle.UPPER_CENTER);
	}

	public static ScreenMessage PostScreenMessage(string message, ScreenMessage msg)
	{
		if (!Instance)
		{
			return null;
		}
		return Instance.PostMessage(msg, message, Instance.textPrefab, msg.duration, msg.style);
	}

	public static ScreenMessage PostScreenMessage(ScreenMessage msg)
	{
		if (!Instance)
		{
			return null;
		}
		return Instance.PostMessage(msg, msg.message, Instance.textPrefab, msg.duration, msg.style);
	}

	public static void RemoveMessage(ScreenMessage msg)
	{
		if (Instance != null)
		{
			Instance.DestroyMessage(msg);
		}
	}

	public static void SendStateMessage(string title, string message, MessageSystemButton.MessageButtonColor color, MessageSystemButton.ButtonIcons icon)
	{
		if (MessageSystem.Instance != null)
		{
			MessageSystem.Instance.AddMessage(new MessageSystem.Message(title, message, color, icon), animate: false);
		}
	}

	public ScreenMessage PostMessage(ScreenMessage current, string msg, ScreenMessagesText textPrefab, float duration, ScreenMessageStyle style)
	{
		return PostMessage(current, msg, textPrefab, duration, style, defaultColor);
	}

	public ScreenMessage PostMessage(ScreenMessage current, string msg, ScreenMessagesText textPrefab, float duration, ScreenMessageStyle style, Color color)
	{
		if (sceneLoadInProgress)
		{
			return null;
		}
		ScreenMessage screenMessage = null;
		int count = activeMessages.Count;
		bool flag = current != null;
		for (int i = 0; i < count; i++)
		{
			ScreenMessage screenMessage2 = activeMessages[i];
			if (screenMessage2 == null)
			{
				continue;
			}
			if (flag)
			{
				if (screenMessage2 == current || screenMessage2.message == msg)
				{
					screenMessage = screenMessage2;
					break;
				}
			}
			else if (screenMessage2.message == msg)
			{
				screenMessage = screenMessage2;
				break;
			}
		}
		if (screenMessage == null)
		{
			if (current != null)
			{
				screenMessage = current;
			}
			else
			{
				screenMessage = new ScreenMessage(msg, duration, style);
				screenMessage.color = color;
			}
			CreateTextInstance(screenMessage);
			if (!activeMessages.Contains(screenMessage))
			{
				activeMessages.Add(screenMessage);
			}
		}
		screenMessage.startTime = Time.realtimeSinceStartup;
		screenMessage.style = style;
		if (screenMessage.textInstance != null)
		{
			screenMessage.textInstance.SetText(msg);
			SetParent(screenMessage.textInstance.transform, screenMessage.style);
			screenMessage.textInstance.transform.SetAsLastSibling();
			RectTransform rectTransform = screenMessage.textInstance.transform.parent as RectTransform;
			ScreenMessageStyle screenMessageStyle = ((rectTransform == lowerCenter) ? ScreenMessageStyle.LOWER_CENTER : ((!(rectTransform == upperCenter)) ? ((rectTransform == upperLeft) ? ScreenMessageStyle.UPPER_LEFT : ((rectTransform == upperRight) ? ScreenMessageStyle.UPPER_RIGHT : ScreenMessageStyle.KERBAL_EVA)) : ScreenMessageStyle.UPPER_CENTER));
			bool flag2 = false;
			if (pendingMessages[(int)screenMessageStyle] == 0)
			{
				VerticalLayoutGroup component = rectTransform.GetComponent<VerticalLayoutGroup>();
				LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
				flag2 = component.preferredHeight <= rectTransform.sizeDelta.y;
			}
			if (!flag2)
			{
				screenMessage.textInstance.gameObject.SetActive(value: false);
			}
		}
		return screenMessage;
	}

	public void CreateTextInstance(ScreenMessage message)
	{
		if (!(message.textInstance != null))
		{
			ScreenMessagesText screenMessagesText = Object.Instantiate(textPrefab);
			SetParent(screenMessagesText.transform, message.style);
			screenMessagesText.SetText(message.message);
			screenMessagesText.text.color = message.color;
			message.textInstance = screenMessagesText;
		}
	}

	public void DestroyMessages()
	{
		int count = activeMessages.Count;
		while (count-- > 0)
		{
			DestroyMessage(activeMessages[count]);
		}
		activeMessages.Clear();
	}

	public void DestroyMessage(ScreenMessage message)
	{
		if (message != null && message.textInstance != null)
		{
			message.textInstance.text.enabled = false;
			Object.Destroy(message.textInstance.gameObject);
		}
		activeMessages.Remove(message);
	}

	public void SetParent(Transform t, ScreenMessageStyle style)
	{
		switch (style)
		{
		case ScreenMessageStyle.UPPER_CENTER:
			t.SetParent(upperCenter, worldPositionStays: false);
			break;
		case ScreenMessageStyle.UPPER_LEFT:
			t.SetParent(upperLeft, worldPositionStays: false);
			break;
		case ScreenMessageStyle.UPPER_RIGHT:
			t.SetParent(upperRight, worldPositionStays: false);
			break;
		case ScreenMessageStyle.LOWER_CENTER:
			t.SetParent(lowerCenter, worldPositionStays: false);
			break;
		case ScreenMessageStyle.KERBAL_EVA:
			t.SetParent(kerbalEva, worldPositionStays: false);
			break;
		}
	}
}
