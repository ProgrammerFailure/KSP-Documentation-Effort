using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ionic.Zip;
using UnityEngine;

public class KSPCompression : MonoBehaviour
{
	public static bool CompressDirectory(string sourcePath, string destPath, bool includeTopLevelFolder, bool overwrite = true)
	{
		if (File.Exists(destPath))
		{
			if (!overwrite)
			{
				Debug.LogError("Unable to Compress Directory to " + destPath + " as the file already exists");
				return false;
			}
			File.Delete(destPath);
		}
		try
		{
			string directoryPathInArchive = sourcePath.Substring(sourcePath.TrimEnd('/').LastIndexOf("/") + 1).TrimEnd('/');
			using ZipFile zipFile = new ZipFile(Encoding.UTF8);
			if (includeTopLevelFolder)
			{
				zipFile.AddDirectory(sourcePath, directoryPathInArchive);
			}
			else
			{
				zipFile.AddDirectory(sourcePath);
			}
			zipFile.Save(destPath);
			zipFile.Dispose();
		}
		catch (Exception ex)
		{
			Debug.LogError("Failed to Compress Directory to Zip:\n" + ex.Message);
			return false;
		}
		return true;
	}

	public static void DecompressFile(string filePath, string extractPath)
	{
		ReadOptions readOptions = new ReadOptions();
		readOptions.Encoding = Encoding.UTF8;
		using ZipFile zipFile = ZipFile.Read(filePath, readOptions);
		IEnumerator<ZipEntry> enumerator = zipFile.Entries.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				string fileName = enumerator.Current.FileName;
				if (fileName[fileName.Length - 1] == '/')
				{
					Directory.CreateDirectory(extractPath + fileName);
					continue;
				}
				using FileStream fileStream = new FileStream(extractPath + fileName, FileMode.OpenOrCreate, FileAccess.Write);
				enumerator.Current.Extract(fileStream);
				fileStream.Close();
			}
		}
		finally
		{
			enumerator.Dispose();
		}
		zipFile.Dispose();
	}

	public static List<string> GetTopLevelDirectories(string filePath)
	{
		List<string> list = new List<string>();
		string text = "";
		ReadOptions readOptions = new ReadOptions();
		readOptions.Encoding = Encoding.UTF8;
		using ZipFile zipFile = ZipFile.Read(filePath, readOptions);
		IEnumerator<ZipEntry> enumerator = zipFile.Entries.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				string fileName = enumerator.Current.FileName;
				if (fileName.IndexOf('/') > -1)
				{
					text = fileName.Substring(0, fileName.IndexOf('/'));
					list.AddUnique(text);
				}
			}
		}
		finally
		{
			enumerator.Dispose();
		}
		zipFile.Dispose();
		return list;
	}

	public static bool FilesAtRoot(string filePath)
	{
		bool result = false;
		ReadOptions readOptions = new ReadOptions();
		readOptions.Encoding = Encoding.UTF8;
		using ZipFile zipFile = ZipFile.Read(filePath, readOptions);
		IEnumerator<ZipEntry> enumerator = zipFile.Entries.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.FileName.IndexOf('/') == -1)
				{
					result = true;
					break;
				}
			}
		}
		finally
		{
			enumerator.Dispose();
		}
		zipFile.Dispose();
		return result;
	}
}
