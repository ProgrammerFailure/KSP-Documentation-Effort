using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("KSP/Prop Tools")]
[ExecuteInEditMode]
public class PropTools : MonoBehaviour
{
	[Serializable]
	public class Proxy
	{
		public Vector3 center;

		public Vector3 size;

		public Color color;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Proxy()
		{
			throw null;
		}
	}

	[Serializable]
	public class Prop
	{
		public string directory;

		public string configName;

		public string propName;

		public List<Proxy> proxies;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Prop(string directory, string configName, string propName)
		{
			throw null;
		}
	}

	private static char[] charTrim;

	private static char[] charDelimiters;

	public string propRootDirectory;

	public List<Prop> props;

	public Vector2 propScrollPosition;

	public Rect lastRect;

	public string filePath;

	public string filename;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PropTools()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PropTools()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CreatePropInfoList(string propRoot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Prop CreatePropInfo(FileInfo file)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Proxy CreateProxyInfo(string propName, string proxyString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProxyError(string propName, string proxyString)
	{
		throw null;
	}
}
