using ns2;
using ns9;

namespace ns12;

public class TooltipController_CrewAC : TooltipController
{
	public Tooltip_TitleAndText prefab;

	public string titleString = "";

	public string descriptionString = "";

	public bool showTooltip;

	public static string cacheAutoLOC_445733;

	public static string cacheAutoLOC_445747;

	public void SetTooltip(ProtoCrewMember pcm, string noHireTitle = "", string noHireDescription = "")
	{
		titleString = pcm.name + " (" + pcm.experienceTrait.Title + ")";
		descriptionString = string.Empty;
		showTooltip = false;
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().KerbalExperienceEnabled(HighLogic.CurrentGame.Mode))
		{
			showTooltip = true;
			string text = pcm.experienceTrait.Description;
			string descriptionEffects = pcm.experienceTrait.DescriptionEffects;
			if (!string.IsNullOrEmpty(descriptionEffects))
			{
				text = (string.IsNullOrEmpty(text) ? "" : (text + "\n\n"));
				text += cacheAutoLOC_445733;
				text += descriptionEffects;
			}
			if (pcm.experience > 0f)
			{
				string text2 = KerbalRoster.GenerateExperienceLog(pcm.careerLog);
				if (!string.IsNullOrEmpty(text2))
				{
					text = (string.IsNullOrEmpty(text) ? "" : (text + "\n\n"));
					text += cacheAutoLOC_445747;
					text += text2;
					if (pcm.ExtraExperience != 0f)
					{
						text += Localizer.Format("#autoLOC_445750", pcm.ExtraExperience.ToString("F0"));
					}
				}
			}
			descriptionString += text;
		}
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().GKerbalLimits)
		{
			showTooltip = true;
			descriptionString += Localizer.Format("#autoLOC_445760", ProtoCrewMember.GToleranceMult(pcm).ToString("F2"), ProtoCrewMember.MaxSustainedG(pcm).ToString("F1"));
		}
		if (!string.IsNullOrEmpty(noHireTitle) && !string.IsNullOrEmpty(noHireDescription))
		{
			showTooltip = true;
			descriptionString = descriptionString + "\n\n<b>" + noHireTitle + "</b>\n" + noHireDescription;
		}
		if (pcm.inactive)
		{
			showTooltip = true;
			descriptionString += Localizer.Format("#autoLOC_445772", KSPUtil.PrintDateNew(pcm.inactiveTimeEnd, includeTime: true));
		}
	}

	public override bool OnTooltipAboutToSpawn()
	{
		return showTooltip;
	}

	public override void OnTooltipSpawned(Tooltip instance)
	{
		Tooltip_TitleAndText obj = instance as Tooltip_TitleAndText;
		obj.title.text = titleString;
		obj.label.text = descriptionString;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_445733 = Localizer.Format("#autoLOC_445733");
		cacheAutoLOC_445747 = Localizer.Format("#autoLOC_445747");
	}
}
