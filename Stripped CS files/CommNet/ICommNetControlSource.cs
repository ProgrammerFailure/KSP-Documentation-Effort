namespace CommNet;

public interface ICommNetControlSource
{
	string name { get; }

	void UpdateNetwork();

	VesselControlState GetControlSourceState();

	bool IsCommCapable();
}
