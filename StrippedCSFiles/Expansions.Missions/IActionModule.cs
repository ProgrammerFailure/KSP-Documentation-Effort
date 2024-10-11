using System.Collections;
using Expansions.Missions.Editor;
using UnityEngine;

namespace Expansions.Missions;

public interface IActionModule : IConfigNode, IMENodeDisplay
{
	void RunValidationWrapper(MissionEditorValidator validator);

	void Initialize(MENode node);

	IEnumerator Fire();

	string GetAppObjectiveInfo();

	Vector3 ActionLocation();

	void OnCloned(ref ActionModule actionModule);
}
