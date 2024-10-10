using System;
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

		public AntennaInfo()
		{
		}

		public AntennaInfo(double pow, DoubleCurve range, bool combine)
		{
			power = pow;
			rangeCurve = range;
			combined = combine;
		}

		public void Update(double pow, DoubleCurve curve, bool combine)
		{
			power = pow;
			rangeCurve = curve;
			combined = combine;
		}
	}

	[SerializeField]
	public string _name = "";

	[SerializeField]
	public string _displayname = "";

	public Vector3d _position;

	public double distanceOffset;

	public bool isHome;

	public bool isControlSource;

	public bool isControlSourceMultiHop = true;

	public AntennaInfo antennaTransmit = new AntennaInfo();

	public AntennaInfo antennaRelay = new AntennaInfo();

	public DoubleCurve scienceCurve;

	public Action OnNetworkPreUpdate;

	public Action OnNetworkPostUpdate;

	public Func<CommNode, double> OnLinkCreateSignalModifier;

	public override string name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
		}
	}

	public override string displayName
	{
		get
		{
			return _displayname;
		}
		set
		{
			_displayname = value;
		}
	}

	public Transform transform { get; set; }

	public virtual Vector3d precisePosition
	{
		get
		{
			return _position;
		}
		set
		{
			_position = value;
		}
	}

	public override Vector3d position => transform.position;

	public CommNode()
	{
	}

	public CommNode(Transform transform)
	{
		this.transform = transform;
	}

	public override void NetworkPreUpdate()
	{
		if (transform != null)
		{
			_position = transform.position;
		}
		if (OnNetworkPreUpdate != null)
		{
			OnNetworkPreUpdate();
		}
	}

	public override void NetworkPostUpdate()
	{
		if (OnNetworkPostUpdate != null)
		{
			OnNetworkPostUpdate();
		}
	}

	public virtual double GetSignalStrengthMultiplier(CommNode b)
	{
		if (OnLinkCreateSignalModifier != null)
		{
			return OnLinkCreateSignalModifier(b);
		}
		return 1.0;
	}

	public override string ToString()
	{
		return base.ToString() + " Links=" + base.Count + " : " + (isHome ? "Home " : string.Empty) + " " + (isControlSource ? "Control " : string.Empty) + " " + (isControlSourceMultiHop ? "MultiHop " : string.Empty);
	}
}
