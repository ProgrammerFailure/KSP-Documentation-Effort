using UnityEngine;

namespace Expansions.Missions;

public interface ITestNodeLabel
{
	MENode node { get; }

	bool HasNodeLabel();

	bool HasWorldPosition();

	Vector3 GetWorldPosition();

	string GetExtraText();
}
