public interface IAnalyticTemperatureModifier
{
	void SetAnalyticTemperature(FlightIntegrator fi, double analyticTemp, double toBeInternal, double toBeSkin);

	double GetSkinTemperature(out bool lerp);

	double GetInternalTemperature(out bool lerp);
}
