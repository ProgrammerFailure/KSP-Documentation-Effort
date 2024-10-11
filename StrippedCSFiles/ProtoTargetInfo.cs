using System;
using System.Runtime.CompilerServices;

public class ProtoTargetInfo : IConfigNode
{
	public enum Type
	{
		Vessel,
		PartModule,
		Part,
		CelestialBody,
		Generic,
		Null
	}

	public Type targetType;

	public uint partUID;

	public Guid vesselId;

	public string uniqueName;

	public int partModuleIndex;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoTargetInfo(ProtoTargetInfo pTI)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoTargetInfo(ITargetable tgt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoTargetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoTargetInfo(ConfigNode node)
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ITargetable FindTarget()
	{
		throw null;
	}
}
