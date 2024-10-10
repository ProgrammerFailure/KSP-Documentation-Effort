public interface IPartCostModifier
{
	float GetModuleCost(float defaultCost, ModifierStagingSituation sit);

	ModifierChangeWhen GetModuleCostChangeWhen();
}
