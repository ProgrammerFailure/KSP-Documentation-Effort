using System;
using System.Runtime.CompilerServices;

namespace Contracts.Parameters;

[Serializable]
public class XOR : ContractParameter
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public XOR()
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
