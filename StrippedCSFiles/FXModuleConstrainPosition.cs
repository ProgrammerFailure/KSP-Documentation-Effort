using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

	[KSPField]
	public bool matchRotation;

	[KSPField]
	public bool matchPosition;

	public TrackMode trackingMode;

	[KSPField]
	public string trackingModeString;

	public List<ConstrainedObject> ObjectsList;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FXModuleConstrainPosition()
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
