using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns25;

public class DebugScreenConsole : MonoBehaviour
{
	public delegate void OnConsoleCommand(string arg);

	[Serializable]
	public class ConsoleCommand
	{
		public string command { get; set; }

		public OnConsoleCommand onCommand { get; set; }

		public string help { get; set; }

		public ConsoleCommand()
		{
		}

		public ConsoleCommand(string command, OnConsoleCommand onCommand, string help = null)
		{
			this.command = command;
			this.onCommand = onCommand;
			this.help = ((!string.IsNullOrEmpty(help)) ? help : "No help for this command is available.");
		}
	}

	public TextMeshQueue textMeshQueue;

	public ScrollRect scrollRect;

	public Button submitButton;

	public TMP_InputField inputField;

	public static int bufferSize = 128;

	public const float bracketSaturation = 0.5f;

	public const float bracketValue = 0.95f;

	public const string LockID = "DebugConsole";

	public bool _inputLocked;

	public const string closeColor = "</color>";

	public Dictionary<string, string> bracketedColorCodes = new Dictionary<string, string>();

	public static List<ConsoleCommand> consoleCommands = new List<ConsoleCommand>();

	public bool InputLocked
	{
		get
		{
			return _inputLocked;
		}
		set
		{
			if (_inputLocked != value)
			{
				_inputLocked = value;
				if (_inputLocked)
				{
					InputLockManager.SetControlLock(ControlTypes.ALLBUTTARGETING, "DebugConsole");
				}
				else
				{
					InputLockManager.RemoveControlLock("DebugConsole");
				}
			}
		}
	}

	public static int CommandCount => consoleCommands.Count;

	public void Awake()
	{
		InitializeLog();
		GameEvents.onGameSceneLoadRequested.Add(OnGameSceneLoadRequested);
		OnGameSceneLoadRequested(HighLogic.LoadedScene);
	}

	public void Start()
	{
		KSPLog instance = KSPLog.Instance;
		instance.onMemoryLogUpdated = (KSPLog.OnMemoryLogUpdated)Delegate.Combine(instance.onMemoryLogUpdated, new KSPLog.OnMemoryLogUpdated(OnMemoryLogUpdated));
		inputField.onEndEdit.AddListener(OnInputEndEdit);
		submitButton.onClick.AddListener(OnSubmitclick);
		bufferSize = GameSettings.CONSOLE_BUFFER_SIZE;
	}

	public void OnEnable()
	{
		StartCoroutine(ScrollDown());
	}

	public void OnDisable()
	{
		InputLocked = false;
	}

	public void OnDestroy()
	{
		if (KSPLog.Instance != null)
		{
			KSPLog instance = KSPLog.Instance;
			instance.onMemoryLogUpdated = (KSPLog.OnMemoryLogUpdated)Delegate.Remove(instance.onMemoryLogUpdated, new KSPLog.OnMemoryLogUpdated(OnMemoryLogUpdated));
		}
		InputLocked = false;
		GameEvents.onGameSceneLoadRequested.Remove(OnGameSceneLoadRequested);
	}

	public void OnGameSceneLoadRequested(GameScenes scene)
	{
	}

	public void Update()
	{
		InputLocked = inputField != null && inputField.isFocused;
	}

	public IEnumerator ScrollDown()
	{
		yield return null;
		if (scrollRect != null)
		{
			scrollRect.verticalNormalizedPosition = 0f;
		}
		EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
		inputField.OnPointerClick(new PointerEventData(EventSystem.current));
	}

	public void SubmitCommand(string submitString)
	{
		if (submitString.Length == 0)
		{
			return;
		}
		inputField.text = string.Empty;
		Debug.Log(submitString);
		char c = submitString[0];
		if (c == '/' || c == '\\')
		{
			string text = submitString.Substring(1);
			if (!ParseInputString(text))
			{
				Debug.Log("[DebugConsole]: Command " + c + "\"" + text + "\" was not recognized. Please use " + c + "help for a full list of commands.");
			}
		}
		StartCoroutine(ScrollDown());
	}

	public void OnInputEndEdit(string submitString)
	{
		if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			InputLocked = false;
			return;
		}
		SubmitCommand(inputField.text);
		inputField.ActivateInputField();
	}

	public void OnSubmitclick()
	{
		SubmitCommand(inputField.text);
		InputLocked = false;
	}

	public void InitializeLog()
	{
		int num = Mathf.Min(KSPLog.Instance.MemoryLogLength, bufferSize);
		int num2 = KSPLog.Instance.MemoryLogIndex - num;
		if (num2 < 0)
		{
			num2 += KSPLog.Instance.MemoryLogSize;
		}
		for (int num3 = bufferSize - 1; num3 >= 0; num3--)
		{
			if (num2 >= KSPLog.Instance.MemoryLogSize)
			{
				num2 -= KSPLog.Instance.MemoryLogSize;
			}
			textMeshQueue.AddLine(FormatLogEntry(KSPLog.Instance.MemoryLog[num2]));
			num2++;
		}
	}

	public void OnMemoryLogUpdated(string newLog)
	{
		textMeshQueue.AddLine(FormatLogEntry(newLog));
		textMeshQueue.RemoveLine();
		if (scrollRect != null && scrollRect.verticalNormalizedPosition <= 0f && base.enabled && base.gameObject.activeInHierarchy)
		{
			StartCoroutine(ScrollDown());
		}
	}

	public string FormatLogEntry(string original)
	{
		int num = -1;
		int num2 = -1;
		int num3 = -1;
		int i = 0;
		for (int length = original.Length; i < length; i++)
		{
			switch (original[i])
			{
			case ']':
				if (num3 >= 0)
				{
					num2 = i;
				}
				break;
			case '[':
				num3 = i;
				break;
			case ':':
				num = i;
				break;
			}
			if (num >= 0 || (num3 >= 0 && num2 >= 0))
			{
				break;
			}
		}
		if (num3 >= 0 && num2 > num3)
		{
			string bracketedColorCode = GetBracketedColorCode(original.Substring(num3, num2 - num3));
			original = original.Insert(num3, bracketedColorCode);
			original = original.Insert(num2 + 1 + bracketedColorCode.Length, "</color>");
		}
		if (num < 0)
		{
			return original;
		}
		return original.Substring(0, num) switch
		{
			"Exception" => "<b><color=#ff0000ff>" + original + "</color></b>", 
			"Assert" => "<b><color=#00ff00ff>" + original + "</color></b>", 
			"Warning" => "<b><color=#ffff00ff>" + original + "</color></b>", 
			"Error" => "<b><color=#ff8000ff>" + original + "</color></b>", 
			_ => original, 
		};
	}

	public string GetBracketedColorCode(string bracket)
	{
		string value = string.Empty;
		if (bracketedColorCodes.TryGetValue(bracket, out value))
		{
			return value;
		}
		ColorHSV colorHSV = new ColorHSV((float)new KSPRandom(bracket.GetHashCode_Net35()).NextDouble(), 0.5f, 0.95f);
		value = "<color=#" + ColorUtility.ToHtmlStringRGB(colorHSV.ToColor()) + ">";
		bracketedColorCodes.Add(bracket, value);
		return value;
	}

	public static bool ParseInputString(string str)
	{
		str = str.Trim();
		int num = str.IndexOf(' ');
		if (num == -1)
		{
			ConsoleCommand command = GetCommand(str);
			if (command != null && command.onCommand != null)
			{
				command.onCommand(string.Empty);
				return true;
			}
		}
		else
		{
			string command2 = str.Substring(0, num).Trim();
			string arg = str.Substring(num).Trim();
			ConsoleCommand command3 = GetCommand(command2);
			if (command3 != null && command3.onCommand != null)
			{
				command3.onCommand(arg);
				return true;
			}
		}
		return false;
	}

	public static void AddConsoleCommand(string command, OnConsoleCommand onCommand, string help = null)
	{
		ConsoleCommand command2 = GetCommand(command);
		if (command2 != null)
		{
			Debug.LogError("DebugConsole: Cannot add command as '" + command + "' already exists");
			return;
		}
		command2 = new ConsoleCommand(command, onCommand, help);
		consoleCommands.Add(command2);
	}

	public static void RemoveConsoleCommand(string command)
	{
		ConsoleCommand command2 = GetCommand(command);
		if (command2 != null)
		{
			consoleCommands.Remove(command2);
		}
	}

	public static ConsoleCommand GetCommand(int command)
	{
		return consoleCommands[command];
	}

	public static ConsoleCommand GetCommand(string command)
	{
		int num = 0;
		int count = consoleCommands.Count;
		while (true)
		{
			if (num < count)
			{
				if (consoleCommands[num].command == command)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return consoleCommands[num];
	}
}
