namespace CommNet;

public interface IRelayEnabler
{
	bool CanRelay();

	bool CanRelayUnloaded(ProtoPartModuleSnapshot mSnap);
}
