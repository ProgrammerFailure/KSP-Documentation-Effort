public interface IOverheatDisplay
{
	float GetHeatThrottle();

	bool ModuleIsActive();

	bool IsOverheating();

	double GetCoreTemperature();

	double GetGoalTemperature();

	bool DisplayCoreHeat();
}
