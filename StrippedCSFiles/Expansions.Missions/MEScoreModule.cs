using System;
using System.Runtime.CompilerServices;

namespace Expansions.Missions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class MEScoreModule : Attribute
{
	public Type[] AllowedSystems
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEScoreModule(params Type[] allowedSystems)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsModuleAllowed(Type module)
	{
		throw null;
	}
}
