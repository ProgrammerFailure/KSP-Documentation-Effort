using System;
using CommNet.Network;

namespace CommNet;

public class CommLink : Link<CommNetwork, CommNode, CommLink, CommPath>
{
	public double sS;

	public SignalStrength signal;

	public HopType hopType;

	public bool aCanRelay;

	public bool bCanRelay;

	public bool bothRelay;

	public double strengthRR;

	public double strengthAR;

	public double strengthBR;

	public double signalStrength
	{
		get
		{
			return sS;
		}
		set
		{
			sS = value;
		}
	}

	public virtual void Set(CommNode a, CommNode b, double distance, double signalStrength)
	{
		base.a = a;
		base.b = b;
		cost = distance;
		Update(signalStrength);
	}

	public override void Update(double cost)
	{
		signalStrength = cost;
		signal = NodeUtilities.ConvertSignalStrength(signalStrength);
	}

	public double GetSignalStrength(bool aMustRelay, bool bMustRelay)
	{
		if (aMustRelay)
		{
			if (!bMustRelay && strengthRR <= strengthAR)
			{
				if (strengthAR == 0.0)
				{
					return strengthBR;
				}
				return strengthAR;
			}
			return strengthRR;
		}
		if (strengthRR > strengthBR)
		{
			return strengthRR;
		}
		if (strengthBR == 0.0)
		{
			return strengthAR;
		}
		return strengthBR;
	}

	public double GetSignalStrength(CommNode pathStartNode)
	{
		return GetSignalStrength(a != pathStartNode, b != pathStartNode);
	}

	public double GetBestSignal()
	{
		return Math.Max(Math.Max(strengthAR, strengthBR), strengthRR);
	}

	public void SetSignalStrength(double ss)
	{
		signalStrength = ss;
	}

	public void SetSignalStrength(bool aMustRelay, bool bMustRelay)
	{
		signalStrength = GetSignalStrength(aMustRelay, bMustRelay);
	}

	public void ZeroSignalStrength()
	{
		signalStrength = 0.0;
	}

	public override string ToString()
	{
		return string.Concat(base.ToString(), " : ", KSPUtil.LocalizeNumber(signalStrength, "F2"), " (", signal, ")");
	}
}
