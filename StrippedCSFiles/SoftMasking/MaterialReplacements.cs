using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SoftMasking;

internal class MaterialReplacements
{
	private class MaterialOverride
	{
		private int _useCount;

		public Material original
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

		public Material replacement
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
		public MaterialOverride(Material original, Material replacement)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Material Get()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool Release()
		{
			throw null;
		}
	}

	private readonly IMaterialReplacer _replacer;

	private readonly Action<Material> _applyParameters;

	private readonly List<MaterialOverride> _overrides;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MaterialReplacements(IMaterialReplacer replacer, Action<Material> applyParameters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Material Get(Material original)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Release(Material replacement)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ApplyAll()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DestroyAllAndClear()
	{
		throw null;
	}
}
