namespace PreFlightTests;

public interface IPreFlightTest
{
	bool Test();

	string GetWarningTitle();

	string GetWarningDescription();

	string GetProceedOption();

	string GetAbortOption();
}
