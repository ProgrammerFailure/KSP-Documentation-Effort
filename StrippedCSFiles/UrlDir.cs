using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UrlDir
{
	[Flags]
	public enum DirectoryType
	{
		Parts = 1,
		Internals = 2,
		GameData = 3
	}

	public enum FileType
	{
		Unknown,
		Config,
		Texture,
		Model,
		Audio,
		Assembly,
		AssetBundle
	}

	[Serializable]
	public class ConfigDirectory
	{
		public string directory;

		public string urlRoot;

		public DirectoryType type;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ConfigDirectory()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ConfigDirectory(string urlRoot, string directory, DirectoryType type)
		{
			throw null;
		}
	}

	public class ConfigFileType : IEnumerable
	{
		public FileType type;

		public List<string> extensions;

		public int Count
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ConfigFileType()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ConfigFileType(FileType fileType)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ConfigFileType(FileType fileType, string[] extensions)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public IEnumerator GetEnumerator()
		{
			throw null;
		}
	}

	public class UrlIdentifier
	{
		[HideInInspector]
		[SerializeField]
		private string _url;

		[SerializeField]
		[HideInInspector]
		private string[] _urlSplit;

		[SerializeField]
		[HideInInspector]
		private int _urlDepth;

		public string url
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

		public string[] urlSplit
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public int urlDepth
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public string this[int index]
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public UrlIdentifier()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public UrlIdentifier(string url)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void ConstructURL(string url)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static string[] UrlSplit(string url)
		{
			throw null;
		}
	}

	public class UrlConfig
	{
		[SerializeField]
		private string _name;

		[SerializeField]
		private string _type;

		[SerializeField]
		private ConfigNode _config;

		[HideInInspector]
		[SerializeField]
		private UrlFile _parent;

		public string name
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public string type
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public ConfigNode config
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

		public UrlFile parent
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public string url
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public UrlConfig(UrlFile parent, ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static List<UrlConfig> CreateNodeList(UrlDir parentDir, UrlFile parent)
		{
			throw null;
		}
	}

	public class UrlFile
	{
		[SerializeField]
		private string _name;

		[SerializeField]
		private FileType _fileType;

		[SerializeField]
		private string _path;

		[SerializeField]
		private string _fileExtension;

		[SerializeField]
		private DateTime _fileTime;

		[SerializeField]
		[HideInInspector]
		private UrlDir _root;

		[HideInInspector]
		[SerializeField]
		private UrlDir _parent;

		[SerializeField]
		private List<UrlConfig> _configs;

		public string name
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public FileType fileType
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public string fullPath
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public string fileExtension
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public DateTime fileTime
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public UrlDir root
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public UrlDir parent
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public List<UrlConfig> configs
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public string url
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public UrlFile(UrlDir parent, FileInfo info)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool Exists(string url)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool Exists(UrlIdentifier url, int index)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public UrlConfig GetConfig(string name)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool ContainsConfig(string name)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void ConfigureFile(ConfigFileType[] fileConfig)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SaveConfigs()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AddConfig(ConfigNode newConfig)
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_AllDirectories_003Ed__57 : IEnumerable<UrlDir>, IEnumerable, IEnumerator<UrlDir>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private UrlDir _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public UrlDir _003C_003E4__this;

		private UrlDir _003Cchild_003E5__2;

		private int _003Ci_003E5__3;

		private int _003CiC_003E5__4;

		private IEnumerator<UrlDir> _003C_003E7__wrap4;

		UrlDir IEnumerator<UrlDir>.Current
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
		public _003Cget_AllDirectories_003Ed__57(int _003C_003E1__state)
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		IEnumerator<UrlDir> IEnumerable<UrlDir>.GetEnumerator()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_AllFiles_003Ed__59 : IEnumerable<UrlFile>, IEnumerable, IEnumerator<UrlFile>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private UrlFile _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public UrlDir _003C_003E4__this;

		private IEnumerator<UrlDir> _003C_003E7__wrap1;

		private UrlDir _003Cchild_003E5__3;

		private int _003Cj_003E5__4;

		private int _003CjC_003E5__5;

		UrlFile IEnumerator<UrlFile>.Current
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
		public _003Cget_AllFiles_003Ed__59(int _003C_003E1__state)
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		IEnumerator<UrlFile> IEnumerable<UrlFile>.GetEnumerator()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_AllConfigFiles_003Ed__61 : IEnumerable<UrlFile>, IEnumerable, IEnumerator<UrlFile>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private UrlFile _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public UrlDir _003C_003E4__this;

		private IEnumerator<UrlDir> _003C_003E7__wrap1;

		private UrlDir _003Cchild_003E5__3;

		private int _003Cj_003E5__4;

		private int _003CjC_003E5__5;

		UrlFile IEnumerator<UrlFile>.Current
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
		public _003Cget_AllConfigFiles_003Ed__61(int _003C_003E1__state)
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		IEnumerator<UrlFile> IEnumerable<UrlFile>.GetEnumerator()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_AllConfigs_003Ed__63 : IEnumerable<UrlConfig>, IEnumerable, IEnumerator<UrlConfig>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private UrlConfig _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public UrlDir _003C_003E4__this;

		private IEnumerator<UrlFile> _003C_003E7__wrap1;

		private UrlFile _003Cfile_003E5__3;

		private int _003Cj_003E5__4;

		private int _003CjC_003E5__5;

		UrlConfig IEnumerator<UrlConfig>.Current
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
		public _003Cget_AllConfigs_003Ed__63(int _003C_003E1__state)
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		IEnumerator<UrlConfig> IEnumerable<UrlConfig>.GetEnumerator()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFiles_003Ed__64 : IEnumerable<UrlFile>, IEnumerable, IEnumerator<UrlFile>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private UrlFile _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public UrlDir _003C_003E4__this;

		private FileType type;

		public FileType _003C_003E3__type;

		private IEnumerator<UrlFile> _003C_003E7__wrap1;

		UrlFile IEnumerator<UrlFile>.Current
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
		public _003CGetFiles_003Ed__64(int _003C_003E1__state)
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		IEnumerator<UrlFile> IEnumerable<UrlFile>.GetEnumerator()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetConfigs_003Ed__65 : IEnumerable<UrlConfig>, IEnumerable, IEnumerator<UrlConfig>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private UrlConfig _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool recursive;

		public bool _003C_003E3__recursive;

		public UrlDir _003C_003E4__this;

		private string typeName;

		public string _003C_003E3__typeName;

		private IEnumerator<UrlConfig> _003C_003E7__wrap1;

		private UrlFile _003Cfile_003E5__3;

		private int _003Ci_003E5__4;

		private int _003CiC_003E5__5;

		private int _003Cj_003E5__6;

		private int _003CjC_003E5__7;

		UrlConfig IEnumerator<UrlConfig>.Current
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
		public _003CGetConfigs_003Ed__65(int _003C_003E1__state)
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		IEnumerator<UrlConfig> IEnumerable<UrlConfig>.GetEnumerator()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetConfigs_003Ed__66 : IEnumerable<UrlConfig>, IEnumerable, IEnumerator<UrlConfig>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private UrlConfig _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool recursive;

		public bool _003C_003E3__recursive;

		public UrlDir _003C_003E4__this;

		private string typeName;

		public string _003C_003E3__typeName;

		private string name;

		public string _003C_003E3__name;

		private IEnumerator<UrlConfig> _003C_003E7__wrap1;

		private UrlFile _003Cfile_003E5__3;

		private int _003Ci_003E5__4;

		private int _003CiC_003E5__5;

		private int _003Cj_003E5__6;

		private int _003CjC_003E5__7;

		UrlConfig IEnumerator<UrlConfig>.Current
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
		public _003CGetConfigs_003Ed__66(int _003C_003E1__state)
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		IEnumerator<UrlConfig> IEnumerable<UrlConfig>.GetEnumerator()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw null;
		}
	}

	public const string configExtension = "cfg";

	[SerializeField]
	private string _name;

	[SerializeField]
	[HideInInspector]
	private UrlDir _root;

	[SerializeField]
	[HideInInspector]
	private UrlDir _parent;

	[SerializeField]
	private List<UrlDir> _children;

	[SerializeField]
	private List<UrlFile> _files;

	[SerializeField]
	private string _path;

	[SerializeField]
	private DirectoryType _type;

	private static string selectiveExpansionstring;

	private static bool selectiveExpansionLoad;

	private static string selectedExpansionFolders;

	private static string[] selectedExpansions;

	private static string limitedPartsLoadString;

	private static bool limitedPartsLoad;

	private static string disabledPartFoldersString;

	private static string[] disabledPartsFolders;

	private static bool logSkippedGameDataLoading;

	private static bool setupSelectiveLoadingVars;

	public string name
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public UrlDir root
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public UrlDir parent
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<UrlDir> children
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<UrlFile> files
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string path
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public DirectoryType type
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string url
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public IEnumerable<UrlDir> AllDirectories
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[IteratorStateMachine(typeof(_003Cget_AllDirectories_003Ed__57))]
		get
		{
			throw null;
		}
	}

	public IEnumerable<UrlFile> AllFiles
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[IteratorStateMachine(typeof(_003Cget_AllFiles_003Ed__59))]
		get
		{
			throw null;
		}
	}

	public IEnumerable<UrlFile> AllConfigFiles
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[IteratorStateMachine(typeof(_003Cget_AllConfigFiles_003Ed__61))]
		get
		{
			throw null;
		}
	}

	public IEnumerable<UrlConfig> AllConfigs
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[IteratorStateMachine(typeof(_003Cget_AllConfigs_003Ed__63))]
		get
		{
			throw null;
		}
	}

	public static string ApplicationRootPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UrlDir(ConfigDirectory[] dirConfig, ConfigFileType[] fileConfig)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UrlDir(UrlDir root, ConfigDirectory rootInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UrlDir(UrlDir parent, DirectoryInfo info)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static UrlDir()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Create(UrlDir parent, DirectoryInfo info)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool DirectoryExists(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UrlDir GetDirectory(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UrlDir GetDirectory(UrlIdentifier id, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UrlDir CreateDirectory(string urlDir)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UrlDir CreateDirectory(UrlIdentifier id, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool FileExists(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UrlFile GetFile(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UrlFile GetFile(UrlIdentifier id, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ConfigExists(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UrlConfig GetConfig(string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UrlConfig GetConfig(UrlIdentifier id, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CGetFiles_003Ed__64))]
	public IEnumerable<UrlFile> GetFiles(FileType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CGetConfigs_003Ed__65))]
	public IEnumerable<UrlConfig> GetConfigs(string typeName, bool recursive = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CGetConfigs_003Ed__66))]
	public IEnumerable<UrlConfig> GetConfigs(string typeName, string name, bool recursive = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string CreateApplicationPath(string relativePath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string StripExtension(string filename, string extension)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PathCombine(string a, string b)
	{
		throw null;
	}
}
