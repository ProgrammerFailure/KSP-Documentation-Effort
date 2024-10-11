using System.Runtime.CompilerServices;
using CommNet.Network;

namespace CommNet;

public class CommLink : Link<CommNetwork, CommNode, CommLink, CommPath>
{
	private double sS;

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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CommLink()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Set(CommNode a, CommNode b, double distance, double signalStrength)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Update(double cost)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetSignalStrength(bool aMustRelay, bool bMustRelay)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetSignalStrength(CommNode pathStartNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetBestSignal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSignalStrength(double ss)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSignalStrength(bool aMustRelay, bool bMustRelay)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ZeroSignalStrength()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}
}
