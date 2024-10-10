namespace Contracts.Agents;

public enum KeywordScore
{
	Ignore = int.MinValue,
	NegativeHigh = -2,
	Negative = -1,
	None = 0,
	Positive = 1,
	PositiveHigh = 2
}
