using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace KSPAchievements;

public class CrewRef : IConfigNode
{
	private List<ProtoCrewMember> crews;

	public List<ProtoCrewMember> Crews
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

	public bool HasAny
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CrewRef()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CrewRef FromVessel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CrewRef FromProtoVessel(ProtoVessel pv)
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
