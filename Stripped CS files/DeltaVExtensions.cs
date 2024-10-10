public static class DeltaVExtensions
{
	public static T GetSwitchedValue<T>(this DeltaVSituationOptions situation, T asl, T actual, T vac)
	{
		return situation switch
		{
			DeltaVSituationOptions.SeaLevel => asl, 
			DeltaVSituationOptions.Altitude => actual, 
			_ => vac, 
		};
	}
}
