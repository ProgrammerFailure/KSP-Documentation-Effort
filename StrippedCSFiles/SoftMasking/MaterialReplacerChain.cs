using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SoftMasking;

public class MaterialReplacerChain : IMaterialReplacer
{
	private readonly List<IMaterialReplacer> _replacers;

	public int order
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
	public MaterialReplacerChain(IEnumerable<IMaterialReplacer> replacers, IMaterialReplacer yetAnother)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Material Replace(Material material)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Initialize()
	{
		throw null;
	}
}
