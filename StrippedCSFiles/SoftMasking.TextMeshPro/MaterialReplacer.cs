using System.Runtime.CompilerServices;
using UnityEngine;

namespace SoftMasking.TextMeshPro;

[GlobalMaterialReplacer]
public class MaterialReplacer : IMaterialReplacer
{
	public int order
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MaterialReplacer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Material Replace(Material material)
	{
		throw null;
	}
}
