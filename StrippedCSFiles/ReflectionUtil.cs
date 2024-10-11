using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

public static class ReflectionUtil
{
	public class AttributedType<Tatt>
	{
		public Type Type
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

		public Tatt Attribute
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
		public AttributedType(Type t, Tatt attrib)
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<AttributedType<Tatt>> GetAttributedTypesInAssemblies<Tbase, Tatt>(List<Assembly> assemblies) where Tbase : class where Tatt : Attribute
	{
		throw null;
	}
}
