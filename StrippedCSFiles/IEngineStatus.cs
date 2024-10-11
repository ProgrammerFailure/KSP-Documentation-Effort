public interface IEngineStatus
{
	bool isOperational { get; }

	float normalizedOutput { get; }

	float throttleSetting { get; }

	string engineName { get; }
}
