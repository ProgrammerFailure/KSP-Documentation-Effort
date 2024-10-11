using System.Runtime.CompilerServices;
using UnityEngine;

public static class MaterialPropertyExtensions
{
	public abstract class MaterialProperty
	{
		protected string Name;

		[MethodImpl(MethodImplOptions.NoInlining)]
		protected MaterialProperty()
		{
			throw null;
		}

		public abstract void Apply(ref MaterialPropertyBlock block);
	}

	private static MaterialPropertyBlock block;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static MaterialPropertyExtensions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetupProperties(this Renderer renderer, params MaterialProperty[] properties)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void UpdateProperties(this Renderer renderer, params MaterialProperty[] properties)
	{
		throw null;
	}
}
