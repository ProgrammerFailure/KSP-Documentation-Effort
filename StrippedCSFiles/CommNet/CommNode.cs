using System;
using System.Runtime.CompilerServices;
using CommNet.Network;
using UnityEngine;

namespace CommNet;

public class CommNode : Node<CommNetwork, CommNode, CommLink, CommPath>
{
	public class AntennaInfo
	{
		public double power;

		public DoubleCurve rangeCurve;

		public bool combined;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AntennaInfo()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AntennaInfo(double pow, DoubleCurve range, bool combine)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Update(double pow, DoubleCurve curve, bool combine)
		{
			throw null;
		}
	}

	[SerializeField]
	protected string _name;

	[SerializeField]
	protected string _displayname;

	protected Vector3d _position;

	public double distanceOffset;

	public bool isHome;

	public bool isControlSource;

	public bool isControlSourceMultiHop;

	public AntennaInfo antennaTransmit;

	public AntennaInfo antennaRelay;

	public DoubleCurve scienceCurve;

	public Action OnNetworkPreUpdate;

	public Action OnNetworkPostUpdate;

	public Func<CommNode, double> OnLinkCreateSignalModifier;

	public override string name
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public override string displayName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public Transform transform
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public virtual Vector3d precisePosition
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public override Vector3d position
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CommNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CommNode(Transform transform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void NetworkPreUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void NetworkPostUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double GetSignalStrengthMultiplier(CommNode b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}
}
