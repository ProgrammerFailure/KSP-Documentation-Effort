namespace FlightUIModeControllerUtil;

public static class FlightUIModeControllerUtil
{
	public static int TransitionIndex(this TabAction ta)
	{
		return (int)ta;
	}

	public static string TransitionStateName(this TabAction ta)
	{
		return ta switch
		{
			TabAction.EXPAND => "In", 
			TabAction.COLLAPSE => "Out", 
			_ => "", 
		};
	}
}
