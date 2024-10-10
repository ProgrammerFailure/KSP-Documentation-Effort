using System.Collections.Generic;
using UnityEngine;

public class InternalPart : MonoBehaviour
{
	public Part part;

	public string internalName;

	public List<InternalProp> props = new List<InternalProp>();

	public void Load(ConfigNode node)
	{
		if (props.Count > 0)
		{
			int count = props.Count;
			while (count-- > 0)
			{
				Object.Destroy(props[count]);
			}
		}
		int i = 0;
		for (int count2 = node.nodes.Count; i < count2; i++)
		{
			ConfigNode configNode = node.nodes[i];
			if (configNode.name == "PROP" && configNode.HasValue("name"))
			{
				InternalProp internalProp = PartLoader.GetInternalProp(node.GetValue("name"));
				if (!(internalProp == null))
				{
					internalProp.transform.parent = base.transform;
					internalProp.Load(node);
				}
			}
		}
	}

	public void Save(ConfigNode node)
	{
		int i = 0;
		for (int count = props.Count; i < count; i++)
		{
			props[i].Save(node.AddNode("PROP"));
		}
	}

	public InternalProp AddProp(ConfigNode node)
	{
		if (!node.HasValue("name"))
		{
			Debug.LogError("Cannot add a Module because ConfigNode contains no module name");
			return null;
		}
		InternalProp internalProp = PartLoader.GetInternalProp(node.GetValue("name"));
		if (internalProp == null)
		{
			return null;
		}
		internalProp.Load(node);
		return internalProp;
	}

	public virtual void OnAwake()
	{
	}

	public virtual void OnStart()
	{
	}

	public virtual void OnActive()
	{
	}

	public virtual void OnInactive()
	{
	}

	public virtual void OnUpdate()
	{
	}

	public virtual void OnFixedUpdate()
	{
	}
}
