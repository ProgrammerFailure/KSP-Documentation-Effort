using System.Runtime.CompilerServices;
using UnityEngine;

public class BufferMaterialProperty : MaterialPropertyExtensions.MaterialProperty
{
	private readonly ComputeBuffer Buffer;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BufferMaterialProperty(string name, ComputeBuffer buffer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Apply(ref MaterialPropertyBlock block)
	{
		throw null;
	}
}
