public interface IAnalyticOverheatModule
{
	void OnOverheat(double amout = 1.0);

	double GetFlux();
}
