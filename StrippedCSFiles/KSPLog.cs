using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class KSPLog : MonoBehaviour
{
	public delegate void OnMemoryLogUpdated(string log);

	[SerializeField]
	private bool logToFile;

	[SerializeField]
	private string logFilename;

	[SerializeField]
	private int flushEvery;

	private int flushCount;

	private StreamWriter fileStream;

	private bool localTimeZoneFailed;

	[SerializeField]
	private bool logToMemory;

	[SerializeField]
	private int memoryLogSize;

	private string[] memoryLog;

	private int memoryLogIndex;

	private int memoryLogLength;

	private string lastError;

	private string lastStackTrace;

	public OnMemoryLogUpdated onMemoryLogUpdated;

	private static Application.LogCallback logCallback;

	public static KSPLog Instance
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

	public int MemoryLogSize
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

	public string[] MemoryLog
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int MemoryLogIndex
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int MemoryLogLength
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string LastError
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string LastStackTrace
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPLog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LogHeader()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LogRenderTextureFormats()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LogCallback(string logString, string stackTrace, LogType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LogToFile(string logString, string stackTrace, LogType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LogToMemory(string logString, string stackTrace, LogType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LogToScreen(string logString, string stackTrace, LogType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string ConvertLineEndings(string input, string addToStart = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string TimeString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddLogCallback(Application.LogCallback method)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RemoveLogCallback(Application.LogCallback method)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetScreenLogging(bool logToScreen)
	{
		throw null;
	}
}
