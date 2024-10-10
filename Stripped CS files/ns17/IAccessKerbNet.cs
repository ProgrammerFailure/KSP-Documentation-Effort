using System.Collections.Generic;

namespace ns17;

public interface IAccessKerbNet
{
	Vessel GetKerbNetVessel();

	Part GetKerbNetPart();

	List<string> GetKerbNetDisplayModes();

	float GetKerbNetMinimumFoV();

	float GetKerbNetMaximumFoV();

	float GetKerbNetAnomalyChance();

	string GetKerbNetErrorState();
}
