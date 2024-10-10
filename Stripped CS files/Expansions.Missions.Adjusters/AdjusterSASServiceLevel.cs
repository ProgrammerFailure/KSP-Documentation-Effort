using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions.Adjusters;

public class AdjusterSASServiceLevel : AdjusterSASBase
{
	[MEGUI_NumberRange(displayFormat = "N0", maxValue = 3f, resetValue = "0", guiName = "#autoLOC_8100259")]
	public int sasServiceLevel;

	public AdjusterSASServiceLevel()
	{
		guiName = "SAS service level";
	}

	public AdjusterSASServiceLevel(MENode node)
		: base(node)
	{
		guiName = "#autoLOC_8100260";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100261", sasServiceLevel));
	}

	public override int ApplySASServiceLevelAdjustment(int oldSASServiceLevel)
	{
		return sasServiceLevel;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "sasServiceLevel")
		{
			return Localizer.Format("#autoLOC_8100262", sasServiceLevel);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("sasServiceLevel", sasServiceLevel);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("sasServiceLevel", ref sasServiceLevel);
	}
}
