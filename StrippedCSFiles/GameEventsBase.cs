using System.Runtime.CompilerServices;

public class GameEventsBase
{
	public static bool debugEvents;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameEventsBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static GameEventsBase()
	{
		throw null;
	}
}
