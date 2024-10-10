using System.Collections.Generic;
using Experience;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class CrewCreationDialog : MonoBehaviour
{
	public delegate void CreateCallback(ProtoCrewMember kerbal);

	public delegate void CancelledCallback();

	public TMP_InputField kerbalName;

	public Toggle maleToggle;

	public Toggle femaleToggle;

	public TMP_Dropdown kerbalRole;

	public Slider kerbalExperience;

	public Slider kerbalCourage;

	public Slider kerbalStupidity;

	public Toggle veteranToggle;

	public Toggle badassToggle;

	public CreateCallback OnCrewCreate;

	public CancelledCallback OnDialogCancelled;

	[SerializeField]
	public Button btnCreate;

	[SerializeField]
	public Button btnRandom;

	public ProtoCrewMember crewToEdit;

	public List<ProtoCrewMember> tempCrewList;

	public KerbalRoster currentCrewRoster;

	public void Setup(CreateCallback onCrewCreate, CancelledCallback onDialogCancelled, List<ProtoCrewMember> tempCrewList = null, KerbalRoster currentCrewRoster = null)
	{
		OnCrewCreate = onCrewCreate;
		OnDialogCancelled = onDialogCancelled;
		this.tempCrewList = tempCrewList;
		this.currentCrewRoster = currentCrewRoster;
	}

	public void Start()
	{
		btnCreate.onClick.AddListener(OnButtonCreate);
		btnRandom.onClick.AddListener(OnButtonRandom);
		kerbalRole.onValueChanged.AddListener(OnRoleChanged);
		List<string> list = new List<string>();
		int i = 0;
		for (int count = GameDatabase.Instance.ExperienceConfigs.Categories.Count; i < count; i++)
		{
			list.Add(GameDatabase.Instance.ExperienceConfigs.Categories[i].Title);
		}
		kerbalRole.AddOptions(list);
		if (crewToEdit == null)
		{
			ResetValues();
		}
	}

	public void OnDestroy()
	{
		btnCreate.onClick.RemoveListener(OnButtonCreate);
		btnRandom.onClick.RemoveListener(OnButtonRandom);
		kerbalRole.onValueChanged.RemoveListener(OnRoleChanged);
	}

	public void OnRoleChanged(int selected)
	{
		ExperienceTraitConfig experienceTraitConfig = GameDatabase.Instance.ExperienceConfigs.Categories[selected];
		veteranToggle.gameObject.SetActive(experienceTraitConfig.Name != "Tourist");
		badassToggle.gameObject.SetActive(experienceTraitConfig.Name != "Tourist");
	}

	public void OnButtonCreate()
	{
		string text = kerbalName.text + " " + CrewGenerator.GetLastName();
		bool flag = false;
		if (crewToEdit == null || (crewToEdit != null && crewToEdit.name != text))
		{
			if (tempCrewList != null)
			{
				for (int i = 0; i < tempCrewList.Count; i++)
				{
					if (CrewGenerator.RemoveLastName(tempCrewList[i].name) == kerbalName.text)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				if (currentCrewRoster != null)
				{
					if (currentCrewRoster.Exists(text))
					{
						flag = true;
					}
				}
				else if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.CrewRoster.Exists(text))
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			if (crewToEdit == null)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8002103", kerbalName.text), 10f);
				kerbalName.text = CrewGenerator.RemoveLastName(CrewGenerator.GetRandomName(ProtoCrewMember.Gender.Male));
				return;
			}
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8002102", kerbalName.text), 10f);
			kerbalName.text = CrewGenerator.RemoveLastName(crewToEdit.name);
		}
		ProtoCrewMember protoCrewMember = crewToEdit ?? new ProtoCrewMember((kerbalRole.value == 3) ? ProtoCrewMember.KerbalType.Tourist : ProtoCrewMember.KerbalType.Crew, text);
		if (!flag && crewToEdit != null && text != crewToEdit.name)
		{
			crewToEdit.ChangeName(text);
		}
		ExperienceTraitConfig experienceTraitConfig = GameDatabase.Instance.ExperienceConfigs.Categories[kerbalRole.value];
		protoCrewMember.type = ((!(experienceTraitConfig.Name != "Tourist")) ? ProtoCrewMember.KerbalType.Tourist : ProtoCrewMember.KerbalType.Crew);
		protoCrewMember.gender = ((!maleToggle.isOn) ? ProtoCrewMember.Gender.Female : ProtoCrewMember.Gender.Male);
		protoCrewMember.trait = experienceTraitConfig.Name;
		KerbalRoster.SetExperienceTrait(protoCrewMember, protoCrewMember.trait);
		protoCrewMember.experienceLevel = (int)kerbalExperience.value;
		KerbalRoster.SetExperienceLevel(protoCrewMember, protoCrewMember.experienceLevel, MissionEditorLogic.Instance.EditorMission.situation.gameParameters, Game.Modes.MISSION);
		protoCrewMember.courage = kerbalCourage.value;
		protoCrewMember.stupidity = kerbalStupidity.value;
		protoCrewMember.veteran = experienceTraitConfig.Name != "Tourist" && veteranToggle.isOn;
		protoCrewMember.isBadass = experienceTraitConfig.Name != "Tourist" && badassToggle.isOn;
		OnCrewCreate(protoCrewMember);
		if (crewToEdit == null)
		{
			kerbalName.text = CrewGenerator.RemoveLastName(CrewGenerator.GetRandomName(ProtoCrewMember.Gender.Male));
		}
	}

	public void OnButtonCancel()
	{
		OnDialogCancelled();
		Dismiss();
	}

	public void OnButtonRandom()
	{
		kerbalName.text = CrewGenerator.RemoveLastName(CrewGenerator.GetRandomName((!maleToggle.isOn) ? ProtoCrewMember.Gender.Female : ProtoCrewMember.Gender.Male));
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
		ResetValues();
		btnCreate.GetComponentInChildren<TextMeshProUGUI>().text = Localizer.Format("#autoLOC_900341");
		crewToEdit = null;
	}

	public void Show(ProtoCrewMember crew)
	{
		base.gameObject.SetActive(value: true);
		SetValues(crew);
		btnCreate.GetComponentInChildren<TextMeshProUGUI>().text = Localizer.Format("#autoLOC_900498");
		crewToEdit = crew;
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	public void Dismiss()
	{
		ResetValues();
		Hide();
	}

	public void SetValues(ProtoCrewMember crew)
	{
		kerbalName.text = CrewGenerator.RemoveLastName(crew.name);
		maleToggle.isOn = crew.gender == ProtoCrewMember.Gender.Male;
		femaleToggle.isOn = crew.gender == ProtoCrewMember.Gender.Female;
		int i = 0;
		for (int count = GameDatabase.Instance.ExperienceConfigs.Categories.Count; i < count; i++)
		{
			if (GameDatabase.Instance.ExperienceConfigs.Categories[i].Name.Equals(crew.trait))
			{
				kerbalRole.value = i;
				break;
			}
		}
		kerbalExperience.value = crew.experienceLevel;
		kerbalCourage.value = crew.courage;
		kerbalStupidity.value = crew.stupidity;
		veteranToggle.isOn = crew.veteran;
		badassToggle.isOn = crew.isBadass;
	}

	public void ResetValues()
	{
		kerbalName.text = CrewGenerator.RemoveLastName(CrewGenerator.GetRandomName(ProtoCrewMember.Gender.Male));
		maleToggle.isOn = true;
		kerbalRole.value = 0;
		kerbalExperience.value = 0f;
		kerbalCourage.value = 0f;
		kerbalStupidity.value = 0f;
		veteranToggle.isOn = false;
		badassToggle.isOn = false;
	}
}
