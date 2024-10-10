public interface IScalarModule
{
	string ScalarModuleID { get; }

	float GetScalar { get; }

	bool CanMove { get; }

	EventData<float, float> OnMoving { get; }

	EventData<float> OnStop { get; }

	void SetScalar(float t);

	void SetUIRead(bool state);

	void SetUIWrite(bool state);

	bool IsMoving();
}
