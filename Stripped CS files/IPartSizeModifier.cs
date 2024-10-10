using UnityEngine;

public interface IPartSizeModifier
{
	Vector3 GetModuleSize(Vector3 defaultSize, ModifierStagingSituation sit);

	ModifierChangeWhen GetModuleSizeChangeWhen();
}
