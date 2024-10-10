using System;
using System.IO;
using UnityEngine;

public class KSPLog : MonoBehaviour
{
	public delegate void OnMemoryLogUpdated(string log);

	[SerializeField]
	public bool logToFile = true;

	[SerializeField]
	public string logFilename = "KSP.log";

	[SerializeField]
	public int flushEvery = 20;

	public int flushCount;

	public StreamWriter fileStream;

	public bool localTimeZoneFailed;

	[SerializeField]
	public bool logToMemory = true;

	[SerializeField]
	public int memoryLogSize = 128;

	public string[] memoryLog;

	public int memoryLogIndex;

	public int memoryLogLength;

	public string lastError = "No errors or exceptions have been thrown.";

	public string lastStackTrace = "N/A";

	public OnMemoryLogUpdated onMemoryLogUpdated;

	public static Application.LogCallback logCallback;

	public static KSPLog Instance { get; set; }

	public int MemoryLogSize
	{
		get
		{
			return memoryLogSize;
		}
		set
		{
			memoryLogSize = value;
			memoryLog = new string[memoryLogSize];
			memoryLogLength = 0;
			memoryLogIndex = 0;
		}
	}

	public string[] MemoryLog => memoryLog;

	public int MemoryLogIndex => memoryLogIndex;

	public int MemoryLogLength => memoryLogLength;

	public string LastError => lastError;

	public string LastStackTrace => lastStackTrace;

	public void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Debug.LogError("KSPLog: Instance already exists");
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	public void Start()
	{
		if (base.transform == base.transform.root)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		Application.logMessageReceivedThreaded += LogCallback;
		if (logToFile)
		{
			AddLogCallback(LogToFile);
			flushCount = 0;
			fileStream = new StreamWriter(logFilename, append: false);
			LogHeader();
		}
		if (logToMemory)
		{
			AddLogCallback(LogToMemory);
			MemoryLogSize = memoryLogSize;
			memoryLog = new string[memoryLogSize];
		}
		if (GameSettings.LOG_ERRORS_TO_SCREEN || GameSettings.LOG_EXCEPTIONS_TO_SCREEN)
		{
			AddLogCallback(LogToScreen);
		}
	}

	public void OnDestroy()
	{
		if (fileStream != null)
		{
			fileStream.Flush();
			fileStream.Close();
		}
		if (memoryLog != null)
		{
			memoryLog = null;
		}
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void LogHeader()
	{
		if (VersioningBase.Instance != null)
		{
			Debug.Log("******* Log Initiated for " + Versioning.Title + " - " + Versioning.VersionString + " " + Versioning.Language + " *******");
			fileStream.WriteLine(Versioning.Title + " - " + Versioning.VersionString + " " + Versioning.Language);
			fileStream.WriteLine();
			fileStream.WriteLine();
		}
		else
		{
			Debug.Log("******* Log Initiated *******");
		}
		fileStream.WriteLine("OS: " + SystemInfo.operatingSystem);
		fileStream.WriteLine("CPU: " + SystemInfo.processorType + " (" + SystemInfo.processorCount + ")");
		fileStream.WriteLine("RAM: " + SystemInfo.systemMemorySize);
		fileStream.WriteLine("GPU: " + SystemInfo.graphicsDeviceName + " (" + SystemInfo.graphicsMemorySize + "MB)");
		fileStream.WriteLine("SM: " + SystemInfo.graphicsShaderLevel + " (" + SystemInfo.graphicsDeviceVersion + ")");
		LogRenderTextureFormats();
		fileStream.WriteLine();
		fileStream.WriteLine();
		fileStream.WriteLine("Log started: " + KSPUtil.SystemDateTime.DateTimeNow().ToString("ddd, MMM dd, yyyy HH:mm:ss"));
		fileStream.WriteLine();
		fileStream.WriteLine();
		fileStream.Flush();
	}

	public void LogRenderTextureFormats()
	{
		RenderTextureFormat[] array = (RenderTextureFormat[])Enum.GetValues(typeof(RenderTextureFormat));
		string text = "";
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			if (SystemInfo.SupportsRenderTextureFormat(array[i]))
			{
				if (text != string.Empty)
				{
					text += ", ";
				}
				text += array[i];
			}
		}
		fileStream.WriteLine("RT Formats: " + text);
	}

	public void LogCallback(string logString, string stackTrace, LogType type)
	{
		if (logCallback != null)
		{
			logCallback(logString, stackTrace, type);
		}
	}

	public void LogToFile(string logString, string stackTrace, LogType type)
	{
		string text = TimeString();
		try
		{
			switch (type)
			{
			case LogType.Error:
				fileStream.WriteLine("[ERR " + text + "] " + logString);
				fileStream.WriteLine(ConvertLineEndings(stackTrace, "\t"));
				lastError = "Error: " + logString;
				lastStackTrace = stackTrace;
				break;
			case LogType.Assert:
				fileStream.WriteLine("[ASR " + text + "] " + logString);
				break;
			case LogType.Warning:
				fileStream.WriteLine("[WRN " + text + "] " + logString);
				break;
			case LogType.Log:
				fileStream.WriteLine("[LOG " + text + "] " + logString);
				break;
			case LogType.Exception:
				fileStream.WriteLine("[EXC " + text + "] " + logString);
				fileStream.WriteLine(ConvertLineEndings(stackTrace, "\t"));
				lastError = "Exception: " + logString;
				lastStackTrace = stackTrace;
				break;
			}
			if (GameSettings.LOG_INSTANT_FLUSH || flushCount >= flushEvery)
			{
				fileStream.Flush();
				flushCount = -1;
			}
			flushCount++;
		}
		catch (Exception ex)
		{
			if (!(ex is ObjectDisposedException))
			{
				Debug.LogError(ex.Message);
			}
		}
	}

	public void LogToMemory(string logString, string stackTrace, LogType type)
	{
		if (memoryLog != null)
		{
			string text;
			switch (type)
			{
			case LogType.Error:
				text = (lastError = "Error: " + logString);
				lastStackTrace = stackTrace;
				break;
			case LogType.Assert:
				text = "Assert: " + logString;
				break;
			case LogType.Warning:
				text = "Warning: " + logString;
				break;
			default:
				text = logString;
				break;
			case LogType.Exception:
				text = (lastError = "Exception: " + logString);
				lastStackTrace = stackTrace;
				break;
			}
			memoryLog[memoryLogIndex] = text;
			if (memoryLogLength < memoryLogSize)
			{
				memoryLogLength++;
			}
			memoryLogIndex++;
			if (memoryLogIndex == memoryLogSize)
			{
				memoryLogIndex = 0;
			}
			if (onMemoryLogUpdated != null)
			{
				onMemoryLogUpdated(text);
			}
		}
	}

	public void LogToScreen(string logString, string stackTrace, LogType type)
	{
		if (((type == LogType.Error && GameSettings.LOG_ERRORS_TO_SCREEN) || (type == LogType.Exception && GameSettings.LOG_EXCEPTIONS_TO_SCREEN)) && !(ScreenMessages.Instance == null))
		{
			ScreenMessages.PostScreenMessage(string.Format("<size=-3><color={0}>{1}</color></size>", (type == LogType.Error) ? "orange" : "red", logString), 5f, ScreenMessageStyle.UPPER_RIGHT);
		}
	}

	public static string ConvertLineEndings(string input, string addToStart = "")
	{
		return (addToStart + input).Replace("\n", "\r\n" + addToStart).TrimEnd();
	}

	public string TimeString()
	{
		return KSPUtil.SystemDateTime.DateTimeNow().ToString("HH:mm:ss.fff");
	}

	public static void AddLogCallback(Application.LogCallback method)
	{
		logCallback = (Application.LogCallback)Delegate.Combine(logCallback, method);
	}

	public static void RemoveLogCallback(Application.LogCallback method)
	{
		logCallback = (Application.LogCallback)Delegate.Remove(logCallback, method);
	}

	public void SetScreenLogging(bool logToScreen)
	{
		if (logToScreen)
		{
			AddLogCallback(LogToScreen);
		}
		else
		{
			RemoveLogCallback(LogToScreen);
		}
	}
}
