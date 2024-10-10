using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class MemoryCache
{
	public static List<byte[]> byteCache = new List<byte[]>();

	public static List<Color32[]> colorCache = new List<Color32[]>();

	public static void CleanUp()
	{
		int i = 0;
		for (int count = byteCache.Count; i < count; i++)
		{
			byteCache[i] = null;
		}
		byteCache.Clear();
		int j = 0;
		for (int count2 = colorCache.Count; j < count2; j++)
		{
			colorCache[j] = null;
		}
		colorCache.Clear();
	}

	public static byte[] GetBytes(long nPixels)
	{
		int num = 0;
		int count = byteCache.Count;
		while (true)
		{
			if (num < count)
			{
				if (byteCache[num].Length == nPixels)
				{
					break;
				}
				num++;
				continue;
			}
			byte[] array = new byte[nPixels];
			byteCache.Add(array);
			return array;
		}
		return byteCache[num];
	}

	public static byte[] GetBytes(string filePath)
	{
		return GetBytes(new FileInfo(filePath));
	}

	public static byte[] GetBytes(FileInfo file)
	{
		byte[] bytes = GetBytes(file.Length);
		FileStream fileStream = File.OpenRead(file.FullName);
		fileStream.Read(bytes, 0, (int)file.Length);
		fileStream.Close();
		return bytes;
	}

	public static Color32[] GetColor32(long nPixels)
	{
		int num = 0;
		int count = colorCache.Count;
		while (true)
		{
			if (num < count)
			{
				if (colorCache[num].Length == nPixels)
				{
					break;
				}
				num++;
				continue;
			}
			Color32[] array = new Color32[nPixels];
			colorCache.Add(array);
			return array;
		}
		return colorCache[num];
	}
}
