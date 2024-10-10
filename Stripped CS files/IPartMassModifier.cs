public interface IPartMassModifier
{
	float GetModuleMass(float defaultMass, ModifierStagingSituation sit);

	ModifierChangeWhen GetModuleMassChangeWhen();
}
