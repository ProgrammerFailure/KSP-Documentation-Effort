using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ConstrainedObject()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Load(ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Save(ConfigNode node)
		{
			throw null;
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
	public string trackingModeString;

	[KSPField]
	public bool runInEditor;

	private bool ready;

	public List<ConstrainedObject> ObjectsList;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FXModuleLookAtConstraint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnConstructionModeUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnConstructionModeFixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Track()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupObjects(ConstrainedObject co)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}
}
