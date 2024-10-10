using System.Text;

namespace Smooth.Pools;

public static class StringBuilderPool
{
	public static readonly Pool<StringBuilder> _Instance = new Pool<StringBuilder>(() => new StringBuilder(), delegate(StringBuilder sb)
	{
		sb.Length = 0;
	});

	public static Pool<StringBuilder> Instance => _Instance;
}
