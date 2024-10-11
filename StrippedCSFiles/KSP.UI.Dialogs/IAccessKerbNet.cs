using System.Collections.Generic;

namespace KSP.UI.Dialogs;

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
