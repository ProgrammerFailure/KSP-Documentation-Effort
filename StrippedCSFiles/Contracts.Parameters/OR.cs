using System;
using System.Runtime.CompilerServices;

namespace Contracts.Parameters;

[Serializable]
public class OR : ContractParameter
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public OR()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetHashString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnParameterStateChange(ContractParameter p)
	{
		throw null;
	}
}
