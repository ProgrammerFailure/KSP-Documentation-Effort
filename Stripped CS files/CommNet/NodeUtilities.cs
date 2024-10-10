namespace CommNet;

public static class NodeUtilities
{
	public static SignalStrength ConvertSignalStrength(double signalStrength)
	{
		if (signalStrength > 0.75)
		{
			return SignalStrength.Green;
		}
		if (signalStrength > 0.5)
		{
			return SignalStrength.Yellow;
		}
		if (signalStrength > 0.25)
		{
			return SignalStrength.Orange;
		}
		if (signalStrength > 1E-09)
		{
			return SignalStrength.Red;
		}
		return SignalStrength.None;
	}
}
