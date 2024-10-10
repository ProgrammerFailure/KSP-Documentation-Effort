using Expansions;
using Expansions.Missions;
using Expansions.Serenity;
using ns11;
using ns16;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns19;

public class CrewWidget : MissionSummaryWidget
{
	public ProtoCrewMember crew;

	public bool isTourist;

	public bool isOrangeCrew;

	public bool isFemale;

	public float xpGained;

	public int levelsGained;

	public int newLevel;

	public Image crewIcon;

	public TextMeshProUGUI statusField;

	public TextMeshProUGUI roleField;

	public ImgText xpEarnedField;

	public ImgText lvlProgressField;

	public Sprite[] lvlImages;

	public Sprite crewIconMaleStandard;

	public Sprite crewIconMaleOrange;

	public Sprite crewIconMaleTourist;

	public Sprite crewIconFemaleStandard;

	public Sprite crewIconFemaleOrange;

	public Sprite crewIconFemaleTourist;

	public static CrewWidget Create(ProtoCrewMember crew, MissionRecoveryDialog mrd)
	{
		CrewWidget component = Object.Instantiate(AssetBase.GetPrefab("WidgetRecoveredCrew")).GetComponent<CrewWidget>();
		component.Init(mrd);
		component.crew = crew;
		component.isTourist = crew.type == ProtoCrewMember.KerbalType.Tourist;
		component.isOrangeCrew = crew.veteran;
		component.isFemale = crew.gender == ProtoCrewMember.Gender.Female;
		FlightLog flightLog = crew.careerLog.CreateCopy();
		flightLog.MergeWith(crew.flightLog);
		float num = KerbalRoster.CalculateExperience(crew.careerLog);
		float num2 = ((HighLogic.CurrentGame.Mode != Game.Modes.MISSION) ? KerbalRoster.CalculateExperience(flightLog) : num);
		component.xpGained = num2 - num;
		int num3 = KerbalRoster.CalculateExperienceLevel(num);
		component.newLevel = KerbalRoster.CalculateExperienceLevel(num2);
		component.levelsGained = component.newLevel - num3;
		return component;
	}

	public void UpdateFields()
	{
		header.text = crew.name;
		if (isTourist)
		{
			roleField.text = Localizer.Format("#autoLOC_476080");
		}
		else
		{
			roleField.text = crew.experienceTrait.TypeName;
		}
		crewIcon.sprite = selectSprite();
		if (HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().KerbalExperienceEnabled(HighLogic.CurrentGame.Mode))
		{
			xpEarnedField.gameObject.SetActive(value: true);
			lvlProgressField.gameObject.SetActive(value: true);
			xpEarnedField.text = "";
			lvlProgressField.text = "";
			if (xpGained > 0f)
			{
				xpEarnedField.text = Localizer.Format("#autoLOC_476118", xpGained.ToString("F0"));
				if (levelsGained > 0)
				{
					lvlProgressField.text = Localizer.Format("#autoLOC_476122", newLevel.ToString());
				}
				else
				{
					lvlProgressField.text = "";
				}
			}
			else
			{
				xpEarnedField.text = Localizer.Format("#autoLOC_476131");
			}
			lvlProgressField.sprite = lvlImages[newLevel];
		}
		else
		{
			xpEarnedField.gameObject.SetActive(value: false);
			lvlProgressField.gameObject.SetActive(value: false);
		}
		statusField.text = Localizer.Format("#autoLOC_476143");
	}

	public Sprite selectSprite()
	{
		Texture texture = null;
		if (crew.SpritePath == null)
		{
			string kerbalIconSuitPath = crew.GetKerbalIconSuitPath();
			texture = ((kerbalIconSuitPath.Contains("vintage") && ExpansionsLoader.IsExpansionInstalled("MakingHistory")) ? MissionsUtils.METexture(kerbalIconSuitPath + ".tif") : ((!kerbalIconSuitPath.Contains("future") || !ExpansionsLoader.IsExpansionInstalled("Serenity")) ? AssetBase.GetTexture(kerbalIconSuitPath) : SerenityUtils.SerenityTexture(kerbalIconSuitPath + ".tif")));
		}
		else
		{
			texture = GameDatabase.Instance.GetTexture(crew.SpritePath, asNormalMap: false);
		}
		return Sprite.Create(texture as Texture2D, new Rect(0f, 0f, texture.width, texture.height), Vector2.one * 0.5f);
	}
}
