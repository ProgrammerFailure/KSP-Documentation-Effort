public interface IAnimatedModule
{
	void EnableModule();

	void DisableModule();

	bool ModuleIsActive();

	bool IsSituationValid();
}
