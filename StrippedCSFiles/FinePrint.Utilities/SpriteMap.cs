using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FinePrint.Utilities;

public class SpriteMap
{
	private readonly IDictionary<string, Sprite> _sprites;

	public Sprite this[string key]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SpriteMap()
	{
		throw null;
	}
}
