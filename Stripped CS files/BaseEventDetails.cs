using System.Collections;
using UnityEngine;

public class BaseEventDetails
{
	public enum Sender
	{
		AUTO,
		STAGING,
		const_2,
		USER
	}

	public Hashtable data;

	public ICollection Keys => data.Keys;

	public ICollection Values => data.Values;

	public BaseEventDetails(Sender sender)
	{
		data = new Hashtable();
	}

	public void Set(string name, object value)
	{
		data.Add(name, value);
	}

	public void Set(string name, int value)
	{
		data.Add(name, value);
	}

	public void Set(string name, float value)
	{
		data.Add(name, value);
	}

	public void Set(string name, string value)
	{
		data.Add(name, value);
	}

	public void Set(string name, bool value)
	{
		data.Add(name, value);
	}

	public void Set(string name, GameObject value)
	{
		data.Add(name, value);
	}

	public void Set<T>(string name, T value)
	{
		data.Add(name, value);
	}

	public T Get<T>(string name)
	{
		if (data.ContainsKey(name))
		{
			return (T)data[name];
		}
		PDebug.Error("Invalid key: " + name);
		return default(T);
	}

	public object Get(string name)
	{
		if (data.ContainsKey(name))
		{
			return data[name];
		}
		PDebug.Error("Invalid key: " + name);
		return null;
	}

	public int GetInt(string name)
	{
		if (data.ContainsKey(name))
		{
			return (int)data[name];
		}
		PDebug.Error("Invalid key: " + name);
		return 0;
	}

	public float GetFloat(string name)
	{
		if (data.ContainsKey(name))
		{
			return (float)data[name];
		}
		PDebug.Error("Invalid key: " + name);
		return 0f;
	}

	public string GetString(string name)
	{
		if (data.ContainsKey(name))
		{
			return (string)data[name];
		}
		PDebug.Error("Invalid key: " + name);
		return "";
	}

	public bool GetBool(string name)
	{
		if (data.ContainsKey(name))
		{
			return (bool)data[name];
		}
		PDebug.Error("Invalid key: " + name);
		return false;
	}

	public GameObject GetGameObject(string name)
	{
		if (data.ContainsKey(name))
		{
			return (GameObject)data[name];
		}
		PDebug.Error("Invalid key: " + name);
		return null;
	}
}
