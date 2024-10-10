namespace Smooth.Compare;

public static class EventExtensions
{
	public static string ToStringCached(this ComparerType comparerType)
	{
		return comparerType switch
		{
			ComparerType.EqualityComparer => "Equality Comparer", 
			ComparerType.Comparer => "Sort Order Comparer", 
			_ => "Unknown", 
		};
	}
}
