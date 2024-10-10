using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace ns8;

public class IOUtils
{
	public static string PluginRootPath
	{
		get
		{
			if (Application.platform != RuntimePlatform.OSXPlayer)
			{
				return Application.dataPath + "/../PluginData/";
			}
			return Application.dataPath + "/../../PluginData/";
		}
	}

	public static string GetFilePathFor(Type type_0, string file, Vessel flight = null)
	{
		string text = Path.Combine(PluginRootPath, AssemblyLoader.GetPathByType(type_0));
		if (flight != null)
		{
			text = Path.Combine(Path.Combine(text, "Flights"), flight.id.ToString());
			Directory.CreateDirectory(text);
		}
		file = Path.GetFileName(file);
		return Path.Combine(text, file);
	}

	public static void Cleanup(List<Guid> validFlightIDs)
	{
		foreach (DirectoryInfo flightDirectory in GetFlightDirectories(validFlightIDs.ToArray(), include: false))
		{
			Directory.Delete(flightDirectory.FullName, recursive: true);
		}
	}

	public static void RemoveFlightData(Guid flightID)
	{
		foreach (DirectoryInfo flightDirectory in GetFlightDirectories(new Guid[1] { flightID }, include: true))
		{
			Directory.Delete(flightDirectory.FullName, recursive: true);
		}
	}

	public static List<DirectoryInfo> GetFlightDirectories(Guid[] flightID, bool include)
	{
		List<DirectoryInfo> list = new List<DirectoryInfo>();
		List<string> list2 = new List<string>(flightID.Length);
		for (int i = 0; i < flightID.Length; i++)
		{
			list2.Add(flightID[i].ToString());
		}
		DirectoryInfo directoryInfo = new DirectoryInfo(PluginRootPath);
		if (!Directory.Exists(PluginRootPath))
		{
			Directory.CreateDirectory(PluginRootPath);
		}
		DirectoryInfo[] directories = directoryInfo.GetDirectories();
		for (int j = 0; j < directories.Length; j++)
		{
			DirectoryInfo[] directories2 = directories[j].GetDirectories();
			foreach (DirectoryInfo directoryInfo2 in directories2)
			{
				if (!(directoryInfo2.Name == "Flights"))
				{
					continue;
				}
				DirectoryInfo[] directories3 = directoryInfo2.GetDirectories();
				foreach (DirectoryInfo directoryInfo3 in directories3)
				{
					if ((include && list2.Contains(directoryInfo3.Name)) || (!include && !list2.Contains(directoryInfo3.Name)))
					{
						list.Add(directoryInfo3);
						break;
					}
				}
				break;
			}
		}
		return list;
	}

	public static System.IO.FileMode ConvertMode(FileMode mode)
	{
		return mode switch
		{
			FileMode.CreateNew => System.IO.FileMode.CreateNew, 
			FileMode.Create => System.IO.FileMode.Create, 
			FileMode.Open => System.IO.FileMode.Open, 
			FileMode.OpenOrCreate => System.IO.FileMode.OpenOrCreate, 
			FileMode.Truncate => System.IO.FileMode.Truncate, 
			FileMode.Append => System.IO.FileMode.Append, 
			_ => System.IO.FileMode.OpenOrCreate, 
		};
	}

	public static System.IO.SeekOrigin ConvertSeek(SeekOrigin origin)
	{
		return origin switch
		{
			SeekOrigin.Begin => System.IO.SeekOrigin.Begin, 
			SeekOrigin.Current => System.IO.SeekOrigin.Current, 
			SeekOrigin.End => System.IO.SeekOrigin.End, 
			_ => System.IO.SeekOrigin.Current, 
		};
	}

	public static byte[] SerializeToBinary(object something)
	{
		using System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
		new BinaryFormatter().Serialize(memoryStream, something);
		return memoryStream.ToArray();
	}

	public static object DeserializeFromBinary(byte[] input)
	{
		return new BinaryFormatter().Deserialize(new System.IO.MemoryStream(input));
	}

	public static Exception WrapException(Exception e)
	{
		if (e is IOException)
		{
			return new IOException(e.Message, e.Source, e.StackTrace);
		}
		return e;
	}

	public static System.IO.FileAccess ConvertAccess(FileAccess access)
	{
		return (System.IO.FileAccess)access;
	}

	public static System.IO.FileShare ConvertShare(FileShare share)
	{
		return (System.IO.FileShare)share;
	}

	public static string GetPartPathFor(Part part, string file)
	{
		string partPath = part.partInfo.partPath;
		file = Path.GetFileName(file);
		return Path.Combine(partPath, file);
	}
}
