using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class Versioning : VersioningBase
{
	[CompilerGenerated]
	private sealed class _003CcheckForUpdate_003Ed__67 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Versioning _003C_003E4__this;

		private bool _003CdlPatcher_003E5__2;

		private string _003CpatcherPath_003E5__3;

		private UnityWebRequest _003Cpost_003E5__4;

		private UnityWebRequest _003CpatcherChecker_003E5__5;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CcheckForUpdate_003Ed__67(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	public static Versioning fetch;

	public string titleShort;

	public string title;

	public int versionMajor;

	public int versionMinor;

	public int revision;

	public int experimental;

	public int buildID;

	public bool beta;

	public bool prerelease;

	public bool isReleaseBuild;

	private string versionString;

	[SerializeField]
	public List<string> distributionPlatformNames;

	private static string architecture;

	internal static bool WinX64;

	private string gameVersionUrl;

	private string patcherHashURL;

	private string patcherDLRoot;

	private string testUrl;

	public bool test;

	public static string Language;

	private static string distributionName;

	private int latestMajor;

	private int latestMinor;

	private int latestRev;

	private int latestExp;

	private bool latestBeta;

	public static bool promptNewVersion;

	private bool forcePrompt;

	private bool canUpdate;

	private bool isPatch;

	private string promptText;

	public GUISkin newVersionWindowSkin;

	public bool dontShowAgain;

	private bool doAnotherUpdate;

	public static int version_major
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static int version_minor
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static int Revision
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static int Experimental
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static int BuildID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool isBeta
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool isPrerelease
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool IsSteam
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string TitleShort
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string Title
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool IsReleaseBuild
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string VersionString
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string DistributionName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Versioning()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Versioning()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetVersion()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetVersionStringWithExperimental()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetVersionStringWithPrerelease()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetVersionStringFull()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Version GetVersionFromString(string stringVersion)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CheckForUpdates()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CcheckForUpdate_003Ed__67))]
	private IEnumerator checkForUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static byte[] FileMD5Bytes(string file)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string FileMD5String(string file)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string BytesToHex(byte[] b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePatcher()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetOSCode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetUpdateChannel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PatcherDownloaded(object sender, AsyncCompletedEventArgs e)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void drawNewVersionWindow(int id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CancelUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void QuitAndStartPatcher()
	{
		throw null;
	}
}
