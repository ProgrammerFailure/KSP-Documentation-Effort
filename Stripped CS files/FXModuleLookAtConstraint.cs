using System;
using System.Collections.Generic;
using UnityEngine;

public class FXModuleLookAtConstraint : PartModule
{
	[Serializable]
	public class ConstrainedObject
	{
		public List<Transform> movers;

		public Transform target;

		public string targetName;

		public string rotatorsName;

		public Vector3 rotationAxis;

		public ConstrainedObject()
		{
			movers = new List<Transform>();
		}

		public void Load(ConfigNode node)
		{
			targetName = node.GetValue("targetName");
			rotatorsName = node.GetValue("rotatorsName");
		}

		public void Save(ConfigNode node)
		{
			node.AddValue("targetName", targetName);
			node.AddValue("rotatorsName", rotatorsName);
		}
	}

	public enum TrackMode
	{
		FixedUpdate,
		Update,
		LateUpdate
	}

	public TrackMode trackingMode;

	[KSPField]
	public string trackingModeString = "LateUpdate";

	[KSPField]
	public bool runInEditor = true;

	public bool ready;

	public List<ConstrainedObject> ObjectsList;

	public override void OnStart(StartState state)
	{
		if (state != StartState.Editor)
		{
			ready = true;
		}
		if (runInEditor)
		{
			ready = true;
		}
	}

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

	public override void OnConstructionModeUpdate()
	{
		base.OnConstructionModeUpdate();
		if (trackingMode == TrackMode.Update || trackingMode == TrackMode.LateUpdate)
		{
			Track();
		}
	}

	public override void OnConstructionModeFixedUpdate()
	{
		base.OnConstructionModeFixedUpdate();
		if (trackingMode == TrackMode.FixedUpdate)
		{
			Track();
		}
	}

	public void Track()
	{
		if (!ready)
		{
			return;
		}
		int i = 0;
		for (int count = ObjectsList.Count; i < count; i++)
		{
			ConstrainedObject constrainedObject = ObjectsList[i];
			int j = 0;
			for (int count2 = constrainedObject.movers.Count; j < count2; j++)
			{
				constrainedObject.movers[j].LookAt(constrainedObject.target, base.transform.up);
			}
		}
	}

	public void SetupObjects(ConstrainedObject co)
	{
		co.target = base.part.FindModelTransform(co.targetName);
		co.movers = new List<Transform>(base.part.FindModelTransforms(co.rotatorsName));
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
		if (!node.HasNode("CONSTRAINLOOKFX"))
		{
			return;
		}
		ObjectsList.Clear();
		int i = 0;
		for (int count = node.nodes.Count; i < count; i++)
		{
			ConfigNode configNode = node.nodes[i];
			string text = configNode.name;
			if (text == "CONSTRAINLOOKFX")
			{
				ConstrainedObject constrainedObject = new ConstrainedObject();
				constrainedObject.Load(configNode);
				SetupObjects(constrainedObject);
			}
		}
	}
}
