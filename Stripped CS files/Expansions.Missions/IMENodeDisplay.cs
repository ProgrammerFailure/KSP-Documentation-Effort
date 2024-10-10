using System.Collections.Generic;
using Expansions.Missions.Editor;

namespace Expansions.Missions;

public interface IMENodeDisplay
{
	string GetName();

	string GetDisplayName();

	void AddParameterToSAP(string parameter);

	void RemoveParameterFromSAP(string parameter);

	void AddParameterToNodeBody(string parameter);

	void AddParameterToNodeBodyAndUpdateUI(string parameter);

	void RemoveParameterFromNodeBody(string parameter);

	void RemoveParameterFromNodeBodyAndUpdateUI(string parameter);

	bool HasNodeBodyParameter(string parameter);

	bool HasSAPParameter(string parameter);

	string GetNodeBodyParameterString(BaseAPField field);

	void UpdateNodeBodyUI();

	List<IMENodeDisplay> GetInternalParametersToDisplay();

	MENode GetNode();

	string GetInfo();

	void ParameterSetupComplete();
}
