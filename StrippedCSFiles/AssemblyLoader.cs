using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Mono.Cecil;

public static class AssemblyLoader
{
	public class LoadedAssembyList : IEnumerable<LoadedAssembly>, IEnumerable
	{
		private List<LoadedAssembly> assemblies;

		public LoadedAssembly this[int index]
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public int Count
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LoadedAssembyList()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Type GetTypeByName(Type baseType, string name)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string GetPathByType(Type type)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LoadedAssembly Add(LoadedAssembly assembly)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool Contains(Assembly assembly)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool Contains(string assemblyName)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LoadedAssembly GetByAssembly(Assembly assembly)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public IEnumerator<LoadedAssembly> GetEnumerator()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void RemoveRange(int start, int end)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void RemoveAt(int index)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public IEnumerable<T> TSort<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> dependencies)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void Visit<T>(T item, HashSet<T> visited, List<T> sorted, Func<T, IEnumerable<T>> dependencies)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SortAssemblies()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void TypeOperation(Action<Type> action)
		{
			throw null;
		}
	}

	public class AssemblyDependency
	{
		public string name;

		public int versionMajor;

		public int versionMinor;

		public int versionRevision;

		public bool met;

		public bool requireEqualMajor;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AssemblyDependency()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AssemblyDependency(string name, int versionMajor, int versionMinor, bool reqEqual)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AssemblyDependency(string name, int versionMajor, int versionMinor, int versionRevision, bool reqEqual)
		{
			throw null;
		}
	}

	public class LoadedAssembly
	{
		public string name
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

		public int versionMajor
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

		public int versionMinor
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

		public int versionRevision
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

		public Assembly assembly
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

		public string dllName
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

		public string path
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

		public string url
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

		public string dataPath
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

		public ConfigNode assemblyNode
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

		public LoadedTypes types
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

		public LoadedTypesDictionary typesDictionary
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

		public List<AssemblyDependency> dependencies
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

		public List<LoadedAssembly> deps
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

		public bool dependenciesMet
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
		public LoadedAssembly(Assembly assembly, string path, string url, ConfigNode assemblyNode)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void LoadConfigAttributes(ConfigNode assemblyNode)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void LoadAttribute_KSPAssembly(string asmName, CustomAttribute atr)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void LoadAttribute_KSPAssemblyDependency(string asmName, CustomAttribute atr)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void LoadAttribute_KSPAssemblyDependencyEqualMajor(string asmName, CustomAttribute atr)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Load()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Unload()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void CheckDependencies(List<LoadedAssembly> loadedAssemblies)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void TypeOperation(Action<Type> action)
		{
			throw null;
		}
	}

	public class LoadedTypes : Dictionary<Type, List<Type>>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public LoadedTypes()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Add(Type baseType, Type type)
		{
			throw null;
		}
	}

	public class LoadedTypesDictionary : Dictionary<Type, Dictionary<string, Type>>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public LoadedTypesDictionary()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Add(Type baseType, Type type)
		{
			throw null;
		}
	}

	private class ProxyDomain : MarshalByRefObject
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ProxyDomain()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool GetAssembly(string AssemblyPath)
		{
			throw null;
		}
	}

	public static List<Type> loadedTypes;

	public static LoadedAssembyList loadedAssemblies;

	private static List<AssemblyInfo> availableAssemblies;

	private static bool initialized;

	private static Dictionary<Type, List<Type>> subclassesOfParentClass;

	private static Dictionary<string, byte[]> forbiddenRefs;

	private static Dictionary<Type, string> forbiddenTypes;

	private static readonly Dictionary<string, Assembly> bindingRedirect;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static AssemblyLoader()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Initialize(string[] baseTypes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Initialize(Type[] baseTypes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void LoadPluginInfo(FileInfo file, string url, ConfigNode assemblyNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void FlagDuplicatedPlugins()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool PluginNameNotDuplicate(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string PluginHighestVersionPath(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool LoadPlugin(FileInfo file, string url, ConfigNode assemblyNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Type GetClassByName(Type baseType, string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetPathByType(Type type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ClearPlugins()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<ConstructorInfo> GetModulesImplementingInterface<T>(Type[] param_types) where T : class
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<Type> GetTypesOfClassesImplementingInterface<T>() where T : class
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<Type> GetSubclassesOfParentClass(Type parentType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void LogBadAssemblyError(Exception ex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void InitializeForbidden()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddForbiddenType<T>(string errorToThrow)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddForbiddenString(string badString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void LoadAssembly(Assembly assembly, string filePath, string url, ConfigNode assemblyNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void LoadAssemblies()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool LoadExternalAssembly(string file)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ScanForBadTypeRefs(string file)
	{
		throw null;
	}
}
