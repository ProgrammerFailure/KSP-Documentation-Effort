using UnityEngine;

namespace ns2;

public interface ITooltipController
{
	string name { get; set; }

	Tooltip TooltipPrefabType { get; set; }

	Tooltip TooltipPrefabInstance { get; set; }

	RectTransform TooltipPrefabInstanceTransform { get; set; }

	bool OnTooltipAboutToSpawn();

	void OnTooltipSpawned(Tooltip instance);

	bool OnTooltipAboutToDespawn();

	void OnTooltipDespawned(Tooltip instance);

	bool OnTooltipUpdate(Tooltip instance);
}
