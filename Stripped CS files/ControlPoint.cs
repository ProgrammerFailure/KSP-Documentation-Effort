using System;
using UnityEngine;

[Serializable]
public class ControlPoint
{
	[SerializeField]
	public string name;

	[SerializeField]
	public Vector3 orientation;

	[SerializeField]
	public Transform transform;

	[SerializeField]
	public string displayName;

	public ControlPoint()
	{
	}

	public ControlPoint(string name, string displayName, Transform transform, Vector3 orientation)
		: this()
	{
		this.name = name;
		this.displayName = displayName;
		this.transform = transform;
		this.orientation = orientation;
	}

	public void Load(ConfigNode node)
	{
		name = node.GetValue("name");
		displayName = node.GetValue("displayName");
		orientation = KSPUtil.ParseVector3(node.GetValue("orientation"));
	}
}
