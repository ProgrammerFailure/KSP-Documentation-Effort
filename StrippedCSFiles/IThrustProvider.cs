public interface IThrustProvider
{
	void OnCenterOfThrustQuery(CenterOfThrustQuery qry);

	float GetMaxThrust();

	float GetCurrentThrust();

	EngineType GetEngineType();
}
