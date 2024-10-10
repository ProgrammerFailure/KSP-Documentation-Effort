using System.Collections.Generic;
using UnityEngine;

public class InputDevices : IConfigNode
{
	public Dictionary<string, int> devices;

	public InputDevices()
	{
		devices = new Dictionary<string, int>();
		if (Application.isPlaying)
		{
			InitializeDevices();
		}
	}

	public void InitializeDevices()
	{
		string[] joystickNames = Input.GetJoystickNames();
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		int num = joystickNames.Length;
		while (num-- > 0)
		{
			string text = TrimDeviceName(joystickNames[num]);
			if (string.IsNullOrEmpty(text))
			{
				text = "Joystick " + num;
				Debug.LogWarning("[InputDevices]: Device #" + num + " has no name, renaming as Joystick " + num);
			}
			dictionary[text] = num;
		}
		devices = dictionary;
		Debug.Log("[Input Devices]: " + KSPUtil.PrintCollection(devices.Keys, ", ", (string ss) => devices[ss] + ": " + ss));
	}

	public void Load(ConfigNode node)
	{
		devices.Clear();
		foreach (ConfigNode.Value value in node.values)
		{
			devices.Add(value.name.Trim(), int.Parse(value.value));
		}
		InitializeDevices();
	}

	public void Save(ConfigNode node)
	{
		foreach (string key in devices.Keys)
		{
			node.AddValue(key, devices[key]);
		}
	}

	public int GetDeviceIndex(string deviceName)
	{
		if (devices.TryGetValue(deviceName, out var value))
		{
			return value;
		}
		return -1;
	}

	public int GetDeviceListLength()
	{
		int result = 0;
		if (devices != null)
		{
			result = devices.Count;
		}
		return result;
	}

	public static string TrimDeviceName(string src)
	{
		return src.Trim();
	}
}
