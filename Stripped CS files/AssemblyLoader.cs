using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security;
using System.Text;
using Mono.Cecil;
using ns7;
using UnityEngine;

public static class AssemblyLoader
{
	public class LoadedAssembyList : IEnumerable<LoadedAssembly>, IEnumerable
	{
		public List<LoadedAssembly> assemblies;

		public LoadedAssembly this[int index] => assemblies[index];

		public int Count => assemblies.Count;

		public LoadedAssembyList()
		{
			assemblies = new List<LoadedAssembly>();
		}

		public Type GetTypeByName(Type baseType, string name)
		{
			foreach (LoadedAssembly assembly in assemblies)
			{
				if (assembly.typesDictionary.ContainsKey(baseType) && assembly.typesDictionary[baseType].ContainsKey(name))
				{
					return assembly.typesDictionary[baseType][name];
				}
			}
			return null;
		}

		public string GetPathByType(Type type)
		{
			foreach (LoadedAssembly assembly in assemblies)
			{
				Type[] types = assembly.assembly.GetTypes();
				for (int i = 0; i < types.Length; i++)
				{
					if (types[i] == type)
					{
						return assembly.dataPath;
					}
				}
			}
			return null;
		}

		public LoadedAssembly Add(LoadedAssembly assembly)
		{
			if (assemblies.Contains(assembly))
			{
				return null;
			}
			assemblies.Add(assembly);
			return assembly;
		}

		public bool Contains(Assembly assembly)
		{
			foreach (LoadedAssembly assembly2 in assemblies)
			{
				if (assembly2.assembly == assembly)
				{
					return true;
				}
			}
			return false;
		}

		public bool Contains(string assemblyName)
		{
			foreach (LoadedAssembly assembly in assemblies)
			{
				if (assembly?.name == assemblyName)
				{
					return true;
				}
			}
			return false;
		}

		public LoadedAssembly GetByAssembly(Assembly assembly)
		{
			foreach (LoadedAssembly assembly2 in assemblies)
			{
				if (assembly2.assembly == assembly)
				{
					return assembly2;
				}
			}
			return null;
		}

		public IEnumerator<LoadedAssembly> GetEnumerator()
		{
			return assemblies.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return assemblies.GetEnumerator();
		}

		public void RemoveRange(int start, int end)
		{
			for (int num = end; num >= start; num--)
			{
				assemblies.RemoveAt(num);
			}
		}

		public void RemoveAt(int index)
		{
			assemblies.RemoveAt(index);
		}

		public IEnumerable<T> TSort<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> dependencies)
		{
			List<T> list = new List<T>();
			HashSet<T> visited = new HashSet<T>();
			foreach (T item in source)
			{
				Visit(item, visited, list, dependencies);
			}
			return list;
		}

		public void Visit<T>(T item, HashSet<T> visited, List<T> sorted, Func<T, IEnumerable<T>> dependencies)
		{
			if (visited.Contains(item))
			{
				return;
			}
			visited.Add(item);
			foreach (T item2 in dependencies(item))
			{
				Visit(item2, visited, sorted, dependencies);
			}
			sorted.Add(item);
		}

		public void SortAssemblies()
		{
			foreach (LoadedAssembly assembly in assemblies)
			{
				assembly.CheckDependencies(assemblies);
			}
			List<LoadedAssembly> list = new List<LoadedAssembly>(TSort(assemblies, (LoadedAssembly n) => n.deps));
			assemblies.Clear();
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				if (list[i].dependenciesMet)
				{
					assemblies.Add(list[i]);
				}
			}
		}

		public void TypeOperation(Action<Type> action)
		{
			int count = loadedAssemblies.Count;
			for (int i = 0; i < count; i++)
			{
				loadedAssemblies[i].TypeOperation(action);
			}
		}
	}

	public class AssemblyDependency
	{
		public string name = "";

		public int versionMajor;

		public int versionMinor;

		public int versionRevision;

		public bool met;

		public bool requireEqualMajor;

		public AssemblyDependency()
		{
			name = "";
			versionMajor = 0;
			versionMinor = 0;
			versionRevision = 0;
			met = false;
			requireEqualMajor = false;
		}

		public AssemblyDependency(string name, int versionMajor, int versionMinor, bool reqEqual)
		{
			this.name = name;
			this.versionMajor = versionMajor;
			this.versionMinor = versionMinor;
			versionRevision = 0;
			met = false;
			requireEqualMajor = reqEqual;
		}

		public AssemblyDependency(string name, int versionMajor, int versionMinor, int versionRevision, bool reqEqual)
		{
			this.name = name;
			this.versionMajor = versionMajor;
			this.versionMinor = versionMinor;
			this.versionRevision = versionRevision;
			met = false;
			requireEqualMajor = reqEqual;
		}
	}

	public class LoadedAssembly
	{
		public string name { get; set; }

		public int versionMajor { get; set; }

		public int versionMinor { get; set; }

		public int versionRevision { get; set; }

		public Assembly assembly { get; set; }

		public string dllName { get; set; }

		public string path { get; set; }

		public string url { get; set; }

		public string dataPath { get; set; }

		public ConfigNode assemblyNode { get; set; }

		public LoadedTypes types { get; set; }

		public LoadedTypesDictionary typesDictionary { get; set; }

		public List<AssemblyDependency> dependencies { get; set; }

		public List<LoadedAssembly> deps { get; set; }

		public bool dependenciesMet { get; set; }

		public LoadedAssembly(Assembly assembly, string path, string url, ConfigNode assemblyNode)
		{
			this.assembly = assembly;
			this.path = path;
			this.url = url;
			dllName = Path.GetFileNameWithoutExtension(path);
			name = dllName;
			dataPath = Path.GetDirectoryName(path) + "/PluginData/" + dllName;
			types = new LoadedTypes();
			typesDictionary = new LoadedTypesDictionary();
			dependencies = new List<AssemblyDependency>();
			deps = new List<LoadedAssembly>();
			dependenciesMet = true;
			if (assembly == null)
			{
				foreach (CustomAttribute customAttribute in AssemblyDefinition.ReadAssembly(path).CustomAttributes)
				{
					if (customAttribute.AttributeType.Name == "KSPAssembly")
					{
						LoadAttribute_KSPAssembly(dllName, customAttribute);
					}
					else if (customAttribute.AttributeType.Name == "KSPAssemblyDependency")
					{
						LoadAttribute_KSPAssemblyDependency(dllName, customAttribute);
					}
					else if (customAttribute.AttributeType.Name == "KSPAssemblyDependencyEqualMajor")
					{
						LoadAttribute_KSPAssemblyDependencyEqualMajor(dllName, customAttribute);
					}
				}
			}
			if (assemblyNode != null)
			{
				LoadConfigAttributes(assemblyNode);
			}
		}

		public void LoadConfigAttributes(ConfigNode assemblyNode)
		{
			string value = assemblyNode.GetValue("versionMajor");
			string value2 = assemblyNode.GetValue("versionMinor");
			string value3 = assemblyNode.GetValue("versionRevision");
			string value4 = assemblyNode.GetValue("requireEqualMajor");
			if (value != null)
			{
				versionMajor = int.Parse(value);
			}
			if (value2 != null)
			{
				versionMinor = int.Parse(value2);
			}
			if (value3 != null)
			{
				versionRevision = int.Parse(value3);
			}
			ConfigNode[] nodes = assemblyNode.GetNodes("DEPENDANCY");
			foreach (ConfigNode configNode in nodes)
			{
				name = configNode.GetValue("name");
				value = configNode.GetValue("versionMajor");
				value2 = configNode.GetValue("versionMinor");
				value3 = configNode.GetValue("versionRevision");
				value4 = configNode.GetValue("requireEqualMajor");
				if (name != null)
				{
					AssemblyDependency assemblyDependency = new AssemblyDependency();
					assemblyDependency.name = name;
					if (value != null)
					{
						assemblyDependency.versionMajor = int.Parse(value);
					}
					if (value2 != null)
					{
						assemblyDependency.versionMinor = int.Parse(value2);
					}
					if (value3 != null)
					{
						assemblyDependency.versionRevision = int.Parse(value3);
					}
					if (value4 != null)
					{
						assemblyDependency.requireEqualMajor = bool.Parse(value4);
					}
				}
			}
		}

		public void LoadAttribute_KSPAssembly(string asmName, CustomAttribute atr)
		{
			if (atr.ConstructorArguments.Count != 3 && atr.ConstructorArguments.Count != 4)
			{
				UnityEngine.Debug.LogError("AssemblyLoader: KSPAssembly has incorrect number of arguments(" + atr.ConstructorArguments.Count + "), should be 3 or 4.");
				return;
			}
			name = (string)atr.ConstructorArguments[0].Value;
			versionMajor = (int)atr.ConstructorArguments[1].Value;
			versionMinor = (int)atr.ConstructorArguments[2].Value;
			if (atr.ConstructorArguments.Count == 4)
			{
				versionRevision = (int)atr.ConstructorArguments[3].Value;
			}
			UnityEngine.Debug.Log("AssemblyLoader: KSPAssembly '" + name + "' V" + versionMajor + "." + versionMinor + "." + versionRevision);
		}

		public void LoadAttribute_KSPAssemblyDependency(string asmName, CustomAttribute atr)
		{
			if (atr.ConstructorArguments.Count != 3 && atr.ConstructorArguments.Count != 4)
			{
				UnityEngine.Debug.LogError("AssemblyLoader: KSPAssemblyDependency has incorrect number of arguments(" + atr.ConstructorArguments.Count + "), should be 3 or 4.");
				return;
			}
			AssemblyDependency assemblyDependency = null;
			if (atr.ConstructorArguments.Count == 3)
			{
				assemblyDependency = new AssemblyDependency((string)atr.ConstructorArguments[0].Value, (int)atr.ConstructorArguments[1].Value, (int)atr.ConstructorArguments[2].Value, reqEqual: false);
			}
			if (atr.ConstructorArguments.Count == 4)
			{
				assemblyDependency = new AssemblyDependency((string)atr.ConstructorArguments[0].Value, (int)atr.ConstructorArguments[1].Value, (int)atr.ConstructorArguments[2].Value, (int)atr.ConstructorArguments[3].Value, reqEqual: false);
			}
			if (assemblyDependency != null)
			{
				dependencies.Add(assemblyDependency);
				UnityEngine.Debug.Log("AssemblyLoader: KSPAssemblyDependency '" + assemblyDependency.name + "' V" + assemblyDependency.versionMajor + "." + assemblyDependency.versionMinor + "." + assemblyDependency.versionRevision);
			}
		}

		public void LoadAttribute_KSPAssemblyDependencyEqualMajor(string asmName, CustomAttribute atr)
		{
			if (atr.ConstructorArguments.Count != 3 && atr.ConstructorArguments.Count != 4)
			{
				UnityEngine.Debug.LogError("AssemblyLoader: KSPAssemblyDependencyEqualMajor has incorrect number of arguments(" + atr.ConstructorArguments.Count + "), should be 3 or 4.");
				return;
			}
			AssemblyDependency assemblyDependency = null;
			if (atr.ConstructorArguments.Count == 3)
			{
				assemblyDependency = new AssemblyDependency((string)atr.ConstructorArguments[0].Value, (int)atr.ConstructorArguments[1].Value, (int)atr.ConstructorArguments[2].Value, reqEqual: true);
			}
			if (atr.ConstructorArguments.Count == 4)
			{
				assemblyDependency = new AssemblyDependency((string)atr.ConstructorArguments[0].Value, (int)atr.ConstructorArguments[1].Value, (int)atr.ConstructorArguments[2].Value, (int)atr.ConstructorArguments[3].Value, reqEqual: true);
			}
			if (assemblyDependency != null)
			{
				dependencies.Add(assemblyDependency);
				UnityEngine.Debug.Log("AssemblyLoader: KSPAssemblyDependencyEqualMajor '" + assemblyDependency.name + "' V" + assemblyDependency.versionMajor + "." + assemblyDependency.versionMinor + "." + assemblyDependency.versionRevision);
			}
		}

		public void Load()
		{
			if (assembly == null && dependenciesMet)
			{
				assembly = Assembly.LoadFrom(path);
			}
		}

		public void Unload()
		{
			assembly = null;
		}

		public void CheckDependencies(List<LoadedAssembly> loadedAssemblies)
		{
			dependenciesMet = true;
			if (dependencies.Count == 0)
			{
				return;
			}
			int num = dependencies.Count;
			dependenciesMet = false;
			foreach (AssemblyDependency dependency in dependencies)
			{
				foreach (LoadedAssembly loadedAssembly in loadedAssemblies)
				{
					if (this != loadedAssembly && loadedAssembly.name == dependency.name && ((!dependency.requireEqualMajor && loadedAssembly.versionMajor > dependency.versionMajor) || (loadedAssembly.versionMajor == dependency.versionMajor && (loadedAssembly.versionMinor > dependency.versionMinor || (loadedAssembly.versionMinor == dependency.versionMinor && loadedAssembly.versionRevision >= dependency.versionRevision)))))
					{
						dependency.met = true;
						num--;
						deps.Add(loadedAssembly);
						break;
					}
				}
				if (!dependency.met)
				{
					UnityEngine.Debug.LogWarning("AssemblyLoader: Assembly '" + name + "' has not met dependency '" + dependency.name + "' V" + dependency.versionMajor + "." + dependency.versionMinor + "." + dependency.versionRevision);
				}
			}
			if (num > 0)
			{
				UnityEngine.Debug.LogWarning("AssemblyLoader: Assembly '" + name + "' is missing " + num + " dependencies");
			}
			else
			{
				dependenciesMet = true;
			}
		}

		public void TypeOperation(Action<Type> action)
		{
			try
			{
				Type[] array = assembly.GetTypes();
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					action(array[i]);
				}
			}
			catch (Exception ex)
			{
				LogBadAssemblyError(ex);
			}
		}
	}

	public class LoadedTypes : Dictionary<Type, List<Type>>
	{
		public void Add(Type baseType, Type type)
		{
			if (!ContainsKey(baseType))
			{
				base[baseType] = new List<Type>();
			}
			base[baseType].Add(type);
		}
	}

	public class LoadedTypesDictionary : Dictionary<Type, Dictionary<string, Type>>
	{
		public void Add(Type baseType, Type type)
		{
			if (!ContainsKey(baseType))
			{
				base[baseType] = new Dictionary<string, Type>();
			}
			base[baseType][type.Name] = type;
		}
	}

	public class ProxyDomain : MarshalByRefObject
	{
		public bool GetAssembly(string AssemblyPath)
		{
			UnityEngine.Debug.Log("AssemblyLoader: Loading assembly at " + AssemblyPath);
			return LoadExternalAssembly(AssemblyPath);
		}
	}

	public static List<Type> loadedTypes;

	public static LoadedAssembyList loadedAssemblies;

	public static List<AssemblyInfo> availableAssemblies;

	public static bool initialized = false;

	public static Dictionary<Type, List<Type>> subclassesOfParentClass = new Dictionary<Type, List<Type>>();

	public static Dictionary<string, byte[]> forbiddenRefs;

	public static Dictionary<Type, string> forbiddenTypes;

	public static readonly Dictionary<string, Assembly> bindingRedirect = new Dictionary<string, Assembly>();

	public static void Initialize(string[] baseTypes)
	{
		List<Type> list = new List<Type>();
		for (int i = 0; i < baseTypes.Length; i++)
		{
			Type type = Type.GetType(baseTypes[i]);
			if (type != null)
			{
				list.Add(type);
			}
		}
		Initialize(list.ToArray());
	}

	public static void Initialize(Type[] baseTypes)
	{
		if (GameDatabase.Instance.Recompile)
		{
			UnityEngine.Debug.LogWarning("AssemblyLoader: Database Reload Skipping...");
			return;
		}
		if (initialized)
		{
			UnityEngine.Debug.LogError("AssemblyLoader: Already initialized");
			return;
		}
		initialized = true;
		loadedAssemblies = new LoadedAssembyList();
		availableAssemblies = new List<AssemblyInfo>();
		loadedTypes = new List<Type>(baseTypes);
		InitializeForbidden();
		ConfigNode configNode = new ConfigNode();
		configNode.AddValue("versionMajor", Versioning.version_major);
		configNode.AddValue("versionMinor", Versioning.version_minor);
		configNode.AddValue("versionRevision", Versioning.Revision);
		LoadAssembly(Assembly.GetExecutingAssembly(), "KSP", "KSP", configNode);
	}

	public static void LoadPluginInfo(FileInfo file, string url, ConfigNode assemblyNode)
	{
		Version aV = new Version(FileVersionInfo.GetVersionInfo(file.FullName).FileVersion);
		AssemblyInfo item = new AssemblyInfo(Path.GetFileNameWithoutExtension(file.FullName), file.FullName, aV);
		availableAssemblies.Add(item);
	}

	public static void FlagDuplicatedPlugins()
	{
		int count = availableAssemblies.Count;
		while (count-- > 0)
		{
			int index = count;
			while (index-- > 0)
			{
				if (!string.IsNullOrEmpty(availableAssemblies[count].name) && !string.IsNullOrEmpty(availableAssemblies[index].name) && availableAssemblies[count].name.Equals(availableAssemblies[index].name))
				{
					if (availableAssemblies[count].assemblyVersion.CompareTo(availableAssemblies[index].assemblyVersion) > 0)
					{
						availableAssemblies[count].isDuplicate = true;
					}
					else
					{
						availableAssemblies[index].isDuplicate = true;
					}
				}
			}
		}
		int count2 = availableAssemblies.Count;
		while (count2-- > 0)
		{
			if (availableAssemblies[count2].isDuplicate)
			{
				availableAssemblies.RemoveAt(count2);
			}
		}
	}

	public static bool PluginNameNotDuplicate(string name)
	{
		bool result = false;
		int count = availableAssemblies.Count;
		while (count-- > 0)
		{
			if (availableAssemblies[count].name.Equals(name) && availableAssemblies[count].isDuplicate)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public static string PluginHighestVersionPath(string name)
	{
		string result = "";
		int count = availableAssemblies.Count;
		while (count-- > 0)
		{
			if (availableAssemblies[count].name.Equals(name))
			{
				result = availableAssemblies[count].path;
				break;
			}
		}
		return result;
	}

	public static bool LoadPlugin(FileInfo file, string url, ConfigNode assemblyNode)
	{
		ProxyDomain proxyDomain = new ProxyDomain();
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FullName);
		if (proxyDomain.GetAssembly(file.FullName) && !loadedAssemblies.Contains(fileNameWithoutExtension))
		{
			if (!PluginNameNotDuplicate(fileNameWithoutExtension))
			{
				LoadAssembly(null, file.FullName, url, assemblyNode);
				return true;
			}
			LoadAssembly(null, PluginHighestVersionPath(fileNameWithoutExtension), url, assemblyNode);
			return true;
		}
		return false;
	}

	public static Type GetClassByName(Type baseType, string name)
	{
		return loadedAssemblies.GetTypeByName(baseType, name);
	}

	public static string GetPathByType(Type type)
	{
		return loadedAssemblies.GetPathByType(type);
	}

	public static void ClearPlugins()
	{
		loadedAssemblies.RemoveRange(1, loadedAssemblies.Count - 1);
	}

	public static List<ConstructorInfo> GetModulesImplementingInterface<T>(Type[] param_types) where T : class
	{
		Type typeFromHandle = typeof(T);
		List<ConstructorInfo> list = new List<ConstructorInfo>();
		int count = loadedAssemblies.Count;
		for (int i = 0; i < count; i++)
		{
			try
			{
				Type[] types = loadedAssemblies[i].assembly.GetTypes();
				int num = types.Length;
				for (int j = 0; j < num; j++)
				{
					Type[] interfaces = types[j].GetInterfaces();
					int num2 = interfaces.Length;
					for (int k = 0; k < num2; k++)
					{
						if (interfaces[k] == typeFromHandle)
						{
							ConstructorInfo constructor = types[j].GetConstructor(param_types);
							if (constructor != null)
							{
								list.Add(constructor);
								UnityEngine.Debug.Log("GetModulesImplementingInterface: " + types[j].Name);
							}
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogBadAssemblyError(ex);
			}
		}
		UnityEngine.Debug.Log("GetModulesImplementingInterface: Found " + list.Count + " types with " + typeFromHandle.Name + " interface in " + loadedAssemblies.Count + " assemblies.");
		return list;
	}

	public static List<Type> GetTypesOfClassesImplementingInterface<T>() where T : class
	{
		Type typeFromHandle = typeof(T);
		List<Type> list = new List<Type>();
		int count = loadedAssemblies.Count;
		for (int i = 0; i < count; i++)
		{
			try
			{
				Type[] types = loadedAssemblies[i].assembly.GetTypes();
				int num = types.Length;
				for (int j = 0; j < num; j++)
				{
					Type[] interfaces = types[j].GetInterfaces();
					int num2 = interfaces.Length;
					for (int k = 0; k < num2; k++)
					{
						if (interfaces[k] == typeFromHandle)
						{
							list.Add(types[j]);
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogBadAssemblyError(ex);
			}
		}
		UnityEngine.Debug.Log("GetTypesOfModulesImplementingInterface2: Found " + list.Count + " types with " + typeFromHandle.Name + " interface in " + loadedAssemblies.Count + " assemblies.");
		return list;
	}

	public static List<Type> GetSubclassesOfParentClass(Type parentType)
	{
		if (subclassesOfParentClass.ContainsKey(parentType))
		{
			UnityEngine.Debug.Log("GetSubclassesOfParentClass: Using cached results for " + parentType.Name);
			return subclassesOfParentClass[parentType];
		}
		List<Type> list = new List<Type>();
		int count = loadedAssemblies.Count;
		for (int i = 0; i < count; i++)
		{
			try
			{
				Type[] types = loadedAssemblies[i].assembly.GetTypes();
				int num = types.Length;
				for (int j = 0; j < num; j++)
				{
					if (types[j].IsSubclassOf(parentType))
					{
						list.Add(types[j]);
					}
				}
			}
			catch (Exception ex)
			{
				LogBadAssemblyError(ex);
			}
		}
		UnityEngine.Debug.Log("GetSubclassesOfParentClass: Found " + list.Count + " types with " + parentType.Name + " parent in " + loadedAssemblies.Count + " assemblies.");
		subclassesOfParentClass[parentType] = list;
		return list;
	}

	public static void LogBadAssemblyError(Exception ex)
	{
		string text = ex.Message;
		if (ex is ReflectionTypeLoadException)
		{
			text += "\n\nAdditional information about this exception:";
			Exception[] loaderExceptions = ((ReflectionTypeLoadException)ex).LoaderExceptions;
			foreach (Exception ex2 in loaderExceptions)
			{
				text = text + "\n\n " + ex2.ToString();
			}
		}
		UnityEngine.Debug.LogError("[AssemblyLoader] Exception when getting assembly attributes: " + text);
	}

	public static void InitializeForbidden()
	{
		forbiddenRefs = new Dictionary<string, byte[]>();
		forbiddenTypes = new Dictionary<Type, string>();
	}

	public static void AddForbiddenType<T>(string errorToThrow)
	{
		forbiddenTypes[typeof(T)] = errorToThrow;
	}

	public static void AddForbiddenString(string badString)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(badString);
		string[] array = new string[bytes.Length];
		int i = 0;
		for (int num = bytes.Length; i < num; i++)
		{
			array[i] = bytes[i].ToString("X");
		}
		forbiddenRefs.Add(badString, bytes);
		UnityEngine.Debug.Log("AssemblyLoader: Added forbidden namespace " + badString + " (" + string.Join(" ", array) + ")");
	}

	public static Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
	{
		Assembly value = null;
		if (bindingRedirect.TryGetValue(args.Name, out value))
		{
			return value;
		}
		AssemblyName assemblyName = new AssemblyName(args.Name);
		if (assemblyName.Name.EndsWith(".resources", StringComparison.InvariantCultureIgnoreCase))
		{
			return null;
		}
		Version version = assemblyName.Version;
		assemblyName.Version = null;
		try
		{
			value = Assembly.Load(assemblyName);
			if (value.GetName().Version >= version)
			{
				UnityEngine.Debug.LogFormat("ADDON BINDER: Create binding redirect: {0} => {1}", args.Name, value.FullName);
			}
			else
			{
				UnityEngine.Debug.LogWarningFormat("ADDON BINDER: Ingoring binding redirect due to incompatible versions: {0} => {1}", args.Name, value.FullName);
				value = null;
			}
		}
		catch (Exception)
		{
			UnityEngine.Debug.LogErrorFormat("ADDON BINDER: Cannot resolve assembly: {0}", assemblyName);
		}
		bindingRedirect.Add(args.Name, value);
		return value;
	}

	public static void LoadAssembly(Assembly assembly, string filePath, string url, ConfigNode assemblyNode)
	{
		try
		{
			LoadedAssembly assembly2 = new LoadedAssembly(assembly, filePath, url, assemblyNode);
			loadedAssemblies.Add(assembly2);
		}
		catch (Exception ex)
		{
			string text = ex.ToString();
			if (ex is ReflectionTypeLoadException)
			{
				text += "\n\nAdditional information about this exception:";
				Exception[] loaderExceptions = ((ReflectionTypeLoadException)ex).LoaderExceptions;
				foreach (Exception ex2 in loaderExceptions)
				{
					text = text + "\n\n " + ex2.ToString();
				}
			}
			UnityEngine.Debug.LogError("Exception when loading " + filePath + ": " + text);
		}
	}

	public static void LoadAssemblies()
	{
		UnityEngine.Debug.Log("AssemblyLoader: Loading assemblies");
		AppDomain.CurrentDomain.AssemblyResolve -= MyResolveEventHandler;
		AppDomain.CurrentDomain.AssemblyResolve += MyResolveEventHandler;
		loadedAssemblies.SortAssemblies();
		TestManager.ClearUnitTests();
		foreach (LoadedAssembly loadedAssembly in loadedAssemblies)
		{
			try
			{
				loadedAssembly.Load();
				if (loadedAssembly.assembly == null)
				{
					UnityEngine.Debug.LogError("AssemblyLoader: Failed to load assembly '" + loadedAssembly.name + "' from '" + loadedAssembly.path + "'");
					continue;
				}
				LoadedTypes loadedTypes = new LoadedTypes();
				List<Type> list = new List<Type>();
				Type[] types = loadedAssembly.assembly.GetTypes();
				foreach (Type type in types)
				{
					foreach (Type loadedType in AssemblyLoader.loadedTypes)
					{
						if (type.IsSubclassOf(loadedType) || type == loadedType)
						{
							loadedTypes.Add(loadedType, type);
						}
						if (type.IsSubclassOf(typeof(UnitTest)) && !type.IsAbstract)
						{
							list.AddUnique(type);
						}
					}
				}
				foreach (Type key in loadedTypes.Keys)
				{
					foreach (Type item in loadedTypes[key])
					{
						loadedAssembly.types.Add(key, item);
						loadedAssembly.typesDictionary.Add(key, item);
					}
				}
				foreach (Type item2 in list)
				{
					TestManager.AddTest(item2);
				}
			}
			catch (Exception ex)
			{
				string text = ex.ToString();
				if (ex is ReflectionTypeLoadException)
				{
					text += "\n\nAdditional information about this exception:";
					Exception[] loaderExceptions = ((ReflectionTypeLoadException)ex).LoaderExceptions;
					foreach (Exception ex2 in loaderExceptions)
					{
						text = text + "\n\n " + ex2.ToString();
					}
				}
				UnityEngine.Debug.LogError("AssemblyLoader: Exception loading '" + loadedAssembly.name + "': " + text);
				loadedAssembly.Unload();
			}
		}
		int count = loadedAssemblies.Count;
		while (count-- > 0)
		{
			if (loadedAssemblies[count].assembly == null)
			{
				loadedAssemblies.RemoveAt(count);
			}
		}
	}

	public static bool LoadExternalAssembly(string file)
	{
		try
		{
			byte[] array;
			using (FileStream fileStream = new FileStream(file, FileMode.Open))
			{
				array = new byte[fileStream.Length];
				fileStream.Read(array, 0, array.Length);
			}
			foreach (KeyValuePair<string, byte[]> forbiddenRef in forbiddenRefs)
			{
				byte[] value = forbiddenRef.Value;
				int i = 0;
				for (int num = array.Length; i < num; i++)
				{
					if (array[i] != value[0] || i + value.Length - 1 >= array.Length)
					{
						continue;
					}
					bool flag = false;
					for (int j = 1; j < value.Length; j++)
					{
						if (array[i + j] != value[j])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						string key = forbiddenRef.Key;
						throw new SecurityException("Assembly " + file + " tried to load " + key + "!\nPlease use KSP.IO.PluginConfiguration (in Assembly-CSharp.dll) to save/load your configuration!");
					}
				}
			}
			ScanForBadTypeRefs(file);
			return true;
		}
		catch (Exception ex)
		{
			string text = ex.ToString();
			if (ex is ReflectionTypeLoadException)
			{
				text += "\n\nAdditional information about this exception:";
				Exception[] loaderExceptions = ((ReflectionTypeLoadException)ex).LoaderExceptions;
				foreach (Exception ex2 in loaderExceptions)
				{
					text = text + "\n\n" + ex2.ToString();
				}
			}
			UnityEngine.Debug.LogError("Failed to load assembly " + file + ":\n" + text);
			return false;
		}
	}

	public static void ScanForBadTypeRefs(string file)
	{
		foreach (TypeReference typeReference in ModuleDefinition.ReadModule(file).GetTypeReferences())
		{
			foreach (Type key in forbiddenTypes.Keys)
			{
				if (key.FullName.Equals(typeReference.FullName))
				{
					throw new SecurityException("Assembly " + file + " tried to use forbidden type " + key.FullName + "!\n" + forbiddenTypes[key]);
				}
			}
		}
	}
}
