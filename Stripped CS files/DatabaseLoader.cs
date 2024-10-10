using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DatabaseLoader<T> where T : class
{
	public bool successful { get; set; }

	public T obj { get; set; }

	public List<string> extensions { get; set; }

	public DatabaseLoader()
	{
		obj = null;
		successful = false;
		object[] customAttributes = GetType().GetCustomAttributes(inherit: true);
		int num = 0;
		int num2 = customAttributes.Length;
		DatabaseLoaderAttrib databaseLoaderAttrib;
		while (true)
		{
			if (num < num2)
			{
				databaseLoaderAttrib = customAttributes[num] as DatabaseLoaderAttrib;
				if (databaseLoaderAttrib != null)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		extensions = new List<string>(databaseLoaderAttrib.extensions);
	}

	public virtual IEnumerator Load(UrlDir.UrlFile urlFile, FileInfo file)
	{
		yield break;
	}

	public virtual void CleanUp()
	{
	}
}
