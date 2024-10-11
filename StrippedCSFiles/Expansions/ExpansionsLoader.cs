using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;

namespace Expansions;

public class ExpansionsLoader : LoadingSystem
{
	internal class ExpansionInfo
	{
		public string MasterBundleFilePath
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

		public ExpansionSO PersistentObject
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

		public bool Installed
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

		public string Name
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public string DisplayName
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public string FolderName
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public string Version
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public ProtoCrewMember.KerbalSuit[] KerbalSuits
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public string MasterBundleFileName
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public string MasterBundleDirectory
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ExpansionInfo(string masterBundleFilePath, ExpansionSO scriptableObject, bool isInstalled)
		{
			throw null;
		}
	}

	[Serializable]
	public struct SupportedExpansion
	{
		public string expansionName;

		public string minimumVersion;

		public string maximumVersion;
	}

	[CompilerGenerated]
	private sealed class _003CLoadExpansions_003Ed__21 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ExpansionsLoader _003C_003E4__this;

		private float _003CstartTime_003E5__2;

		private FileInfo[] _003CexpansionFiles_003E5__3;

		private int _003Ci_003E5__4;

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
		public _003CLoadExpansions_003Ed__21(int _003C_003E1__state)
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

	[CompilerGenerated]
	private sealed class _003CInitializeExpansion_003Ed__22 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ExpansionsLoader _003C_003E4__this;

		public string expansionFile;

		private RSACryptoServiceProvider _003Cverifier_003E5__2;

		private byte[] _003ChashBytes_003E5__3;

		private AssetBundle _003CexpansionSOBundle_003E5__4;

		private AssetBundleRequest _003CrequestSO_003E5__5;

		private ExpansionSO _003CmasterBundleSO_003E5__6;

		private bool _003CallBundlesVerified_003E5__7;

		private UnityWebRequest _003CwebRequest_003E5__8;

		private int _003Cindex_003E5__9;

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
		public _003CInitializeExpansion_003Ed__22(int _003C_003E1__state)
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
		private void _003C_003Em__Finally1()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	public static readonly string expansionsMasterExtension;

	public static readonly string expansionsPublicKey;

	private static Dictionary<string, ExpansionInfo> expansionsInfo;

	public SupportedExpansion[] supportedExpansions;

	private List<string> expansionsThatFailedToLoad;

	private float progressDelta;

	private bool isReady;

	private string progressTitle;

	private float progressFraction;

	public static ExpansionsLoader Instance
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
	public ExpansionsLoader()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ExpansionsLoader()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsReady()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ProgressTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override float ProgressFraction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void StartLoad()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CLoadExpansions_003Ed__21))]
	protected IEnumerator LoadExpansions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CInitializeExpansion_003Ed__22))]
	private IEnumerator InitializeExpansion(string expansionFile)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool InitPublicKeyCryptoProvider(out RSACryptoServiceProvider verifier)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool VerifyHashSignature(RSACryptoServiceProvider verifier, byte[] hashBytes, string signature)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool VerifyHashSignature(byte[] hashBytes, string signature)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ReportInstalledExpansions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsExpansionInstalled(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static bool IsExpansionInstalled(string name, byte[] hashBytes, string signature)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsExpansionAnyKerbalSuitInstalled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsExpansionKerbalSuitInstalled(ProtoCrewMember.KerbalSuit suit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetExpansionDirectory(string expansionName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsInstalled(List<string> expansions)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsAnyExpansionInstalled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static List<ExpansionInfo> GetInstalledExpansions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static string GetInstalledExpansionsString(string separator = "\n", bool sepAtStart = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetExpansionVersion(string expansionName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal List<string> GetExpansionsThatFailedToLoad()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static byte[] BuildMissionFileHash(string filePath, string fileName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static byte[] GetMissionVerificationStrings(ConfigNode node, out string signature)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static ConfigNode FindMissionNode(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static byte[] BuildMissionFileHash(ConfigNode missionNode)
	{
		throw null;
	}
}
