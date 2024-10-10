using System;
using System.Collections.Generic;
using System.IO;
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

		public Proxy()
		{
			center = Vector3.zero;
			size = Vector3.zero;
			color = Color.white;
		}
	}

	[Serializable]
	public class Prop
	{
		public string directory;

		public string configName;

		public string propName;

		public List<Proxy> proxies;

		public Prop(string directory, string configName, string propName)
		{
			this.directory = directory;
			this.configName = configName;
			this.propName = propName;
			proxies = new List<Proxy>();
		}
	}

	public static char[] charTrim = new char[4] { ' ', '\t', '\n', '\r' };

	public static char[] charDelimiters = new char[5] { ' ', '\t', '=', ',', ';' };

	public string propRootDirectory = "";

	public List<Prop> props = new List<Prop>();

	public Vector2 propScrollPosition;

	public Rect lastRect;

	public string filePath = "";

	public string filename = "PropConfig.cfg";

	public bool CreatePropInfoList(string propRoot)
	{
		props = new List<Prop>();
		DirectoryInfo directoryInfo = new DirectoryInfo(propRoot);
		if (directoryInfo == null)
		{
			Debug.Log("Cannot find prop root directory '" + propRoot + "'");
			return false;
		}
		DirectoryInfo[] directories = directoryInfo.GetDirectories();
		for (int i = 0; i < directories.Length; i++)
		{
			FileInfo[] files = directories[i].GetFiles("*.cfg");
			foreach (FileInfo file in files)
			{
				Prop prop = CreatePropInfo(file);
				if (prop != null)
				{
					props.Add(prop);
				}
			}
		}
		if (props.Count == 0)
		{
			Debug.Log("No valid props found in directory '" + propRoot + "'");
		}
		else
		{
			Debug.Log(props.Count + " props loaded successfully");
		}
		return true;
	}

	public Prop CreatePropInfo(FileInfo file)
	{
		string[] array = File.ReadAllLines(file.FullName);
		if (array != null && array.Length != 0)
		{
			Prop prop = new Prop(file.Directory.Name, file.Name, file.Directory.Name);
			bool flag = false;
			string[] array2 = array;
			foreach (string text in array2)
			{
				string text2 = text.Trim(charTrim).ToLower();
				if (!flag && text2.StartsWith("name"))
				{
					string[] array3 = text.Split(charDelimiters, StringSplitOptions.RemoveEmptyEntries);
					if (array3.Length == 2)
					{
						prop.propName = array3[1];
						flag = true;
					}
				}
				if (text2.StartsWith("proxy"))
				{
					Proxy proxy = CreateProxyInfo(file.Directory.Name, text2);
					if (proxy != null)
					{
						prop.proxies.Add(proxy);
					}
				}
			}
			if (prop.proxies.Count == 0)
			{
				Debug.Log("Config data for prop '" + file.Directory.Name + "' contains no usable proxies and is therefore invalid");
				return null;
			}
			return prop;
		}
		Debug.Log("Config data for prop '" + file.Directory.Name + "' is null or has zero length");
		return null;
	}

	public Proxy CreateProxyInfo(string propName, string proxyString)
	{
		string[] array = proxyString.Split(charDelimiters, StringSplitOptions.RemoveEmptyEntries);
		if (array != null && array.Length != 0 && array.Length >= 7)
		{
			float result = 0f;
			float result2 = 0f;
			float result3 = 0f;
			float result4 = 0f;
			float result5 = 0f;
			float result6 = 0f;
			float result7 = 1f;
			float result8 = 1f;
			float result9 = 1f;
			if (!float.TryParse(array[1], out result))
			{
				ProxyError(propName, proxyString);
				return null;
			}
			if (!float.TryParse(array[2], out result2))
			{
				ProxyError(propName, proxyString);
				return null;
			}
			if (!float.TryParse(array[3], out result3))
			{
				ProxyError(propName, proxyString);
				return null;
			}
			if (!float.TryParse(array[4], out result4))
			{
				ProxyError(propName, proxyString);
				return null;
			}
			if (!float.TryParse(array[5], out result5))
			{
				ProxyError(propName, proxyString);
				return null;
			}
			if (!float.TryParse(array[6], out result6))
			{
				ProxyError(propName, proxyString);
				return null;
			}
			if (array.Length == 10)
			{
				if (!float.TryParse(array[7], out result7))
				{
					ProxyError(propName, proxyString);
					return null;
				}
				if (!float.TryParse(array[8], out result8))
				{
					ProxyError(propName, proxyString);
					return null;
				}
				if (!float.TryParse(array[9], out result9))
				{
					ProxyError(propName, proxyString);
					return null;
				}
			}
			return new Proxy
			{
				center = new Vector3(result, result2, result3),
				size = new Vector3(result4, result5, result6),
				color = new Color(result7, result8, result9)
			};
		}
		ProxyError(propName, proxyString);
		return null;
	}

	public void ProxyError(string propName, string proxyString)
	{
		Debug.Log("Config data for prop '" + propName + "' contains an invalid proxy '" + proxyString + "'. Data must be in the format 'proxy = centerX, centerY, centerZ, sizeX, sizeY, sizeZ {, colorR, colorG, colorB }'");
	}
}
