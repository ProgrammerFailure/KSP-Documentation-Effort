using System.Runtime.CompilerServices;
using System.Text;

namespace Smooth.Pools;

public static class StringBuilderPool
{
	private static readonly Pool<StringBuilder> _Instance;

	public static Pool<StringBuilder> Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static StringBuilderPool()
	{
		throw null;
	}
}
