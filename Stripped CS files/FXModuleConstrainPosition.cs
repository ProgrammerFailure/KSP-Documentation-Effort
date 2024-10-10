using System;
using System.Collections.Generic;
using UnityEngine;

public class FXModuleConstrainPosition : PartModule
{
	[Serializable]
	public class ConstrainedObject
	{
		public List<Transform> movers;

		public Transform target;

		public string targetName;

		public string moversName;

		public ConstrainedObject()
		{
			movers = new List<Transform>();
		}

		public void Load(ConfigNode node)
		{
			targetName = node.GetValue("targetName");
			moversName = node.GetValue("moversName");
		}

		public void Save(ConfigNode node)
		{
			node.AddValue("targetName", targetName);
			node.AddValue("moversName", moversName);
		}
	}

	public enum TrackMode
	{
		FixedUpdate,
		Update,
		LateUpdate
	}

	[KSPField]
	public bool matchRotation = true;

	[KSPField]
	public bool matchPosition = true;

	public TrackMode trackingMode;

	[KSPField]
	public string trackingModeString = "LateUpdate";

	public List<ConstrainedObject> ObjectsList;

	public void Update()
	{
		if (trackingMode == TrackMode.Update)
		{
			Track();
		}
	}

	public void FixedUpdate()
	{
		if (trackingMode == TrackMode.FixedUpdate)
		{
			Track();
		}
	}

	public void LateUpdate()
	{
		if (trackingMode == TrackMode.LateUpdate)
		{
			Track();
		}
	}

	public void Track()
	{
		int i = 0;
		for (int count = ObjectsList.Count; i < count; i++)
		{
			ConstrainedObject constrainedObject = ObjectsList[i];
			int j = 0;
			for (int count2 = constrainedObject.movers.Count; j < count2; j++)
			{
				Transform transform = constrainedObject.movers[j];
				if (matchPosition)
				{
					transform.position = constrainedObject.target.position;
				}
				if (matchRotation)
				{
					transform.rotation = constrainedObject.target.rotation;
				}
			}
		}
	}

	public void SetupObjects(ConstrainedObject co)
	{
		co.target = base.part.FindModelTransform(co.targetName);
		co.movers = new List<Transform>(base.part.FindModelTransforms(co.moversName));
		if (co.target != null && co.movers.Count >= 1)
		{
			ObjectsList.Add(co);
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (ObjectsList == null)
		{
			ObjectsList = new List<ConstrainedObject>();
		}
		trackingMode = (TrackMode)Enum.Parse(typeof(TrackMode), trackingModeString);
		if (!node.HasNode("CONSTRAINFX"))
		{
			return;
		}
		ObjectsList.Clear();
		int i = 0;
		for (int count = node.nodes.Count; i < count; i++)
		{
			ConfigNode configNode = node.nodes[i];
			string text = configNode.name;
			if (text == "CONSTRAINFX")
			{
				ConstrainedObject constrainedObject = new ConstrainedObject();
				constrainedObject.Load(configNode);
				SetupObjects(constrainedObject);
			}
		}
	}
}
