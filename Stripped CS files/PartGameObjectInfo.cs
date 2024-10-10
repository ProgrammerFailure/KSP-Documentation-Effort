using System;
using UnityEngine;

[Serializable]
public class PartGameObjectInfo
{
	[SerializeField]
	public string name;

	[SerializeField]
	public bool status;

	public string Name => name;

	public bool Status => status;

	public PartGameObjectInfo(string name, bool status)
	{
		this.name = name;
		this.status = status;
	}
}
