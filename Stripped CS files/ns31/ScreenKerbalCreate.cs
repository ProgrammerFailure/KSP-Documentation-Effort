using System;
using System.Collections;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns31;

public class ScreenKerbalCreate : MonoBehaviour
{
	public TMP_InputField nameField;

	public Button randomButton;

	public Toggle maleGender;

	public Toggle femaleGender;

	public Toggle pilotRole;

	public Toggle scientistRole;

	public Toggle engineerRole;

	public Toggle touristRole;

	public Slider experienceSlider;

	public TextMeshProUGUI experienceSliderText;

	public Slider courageSlider;

	public Slider stupiditySlider;

	public Toggle veteranToggle;

	public Toggle badassToggle;

	public Button submitButton;

	public TextMeshProUGUI submitButtonText;

	public string currentName;

	public ProtoCrewMember.Gender currentGender;

	public string currentRole;

	public int experienceLevel;

	public float courageLevel;

	public float stupidityLevel;

	public bool isVeteran;

	public bool isBadass;

	public string lastName;

	public string levelString;

	public KSPRandom generator;

	public void Start()
	{
		InitializeLocals();
		RefreshControls();
		AddListeners();
		CheckForErrors();
		GameEvents.onKerbalAdded.Add(KerbalsModified);
		GameEvents.onKerbalRemoved.Add(KerbalsModified);
		GameEvents.OnCrewmemberHired.Add(KerbalsModified);
		GameEvents.OnCrewmemberSacked.Add(KerbalsModified);
		GameEvents.OnCrewmemberLeftForDead.Add(KerbalsModified);
		GameEvents.onGameStatePostLoad.Add(GameLoaded);
	}

	public void OnDestroy()
	{
		GameEvents.onKerbalAdded.Remove(KerbalsModified);
		GameEvents.onKerbalRemoved.Remove(KerbalsModified);
		GameEvents.OnCrewmemberHired.Remove(KerbalsModified);
		GameEvents.OnCrewmemberSacked.Remove(KerbalsModified);
		GameEvents.OnCrewmemberLeftForDead.Remove(KerbalsModified);
		GameEvents.onGameStatePostLoad.Remove(GameLoaded);
	}

	public void InitializeLocals()
	{
		generator = new KSPRandom(Environment.TickCount ^ Guid.NewGuid().GetHashCode());
		currentGender = ((generator.Next(2) > 0) ? ProtoCrewMember.Gender.Female : ProtoCrewMember.Gender.Male);
		currentName = CrewGenerator.GetRandomName(currentGender, generator);
		switch (generator.Next(3))
		{
		default:
			currentRole = KerbalRoster.pilotTrait;
			break;
		case 1:
			currentRole = KerbalRoster.engineerTrait;
			break;
		case 0:
			currentRole = KerbalRoster.scientistTrait;
			break;
		}
		experienceLevel = generator.Next(0, 6);
		courageLevel = Convert.ToSingle(generator.NextDouble());
		stupidityLevel = Convert.ToSingle(generator.NextDouble());
		isVeteran = generator.Next(100) == 0;
		isBadass = generator.Next(10) == 0;
		lastName = CrewGenerator.GetLastName();
		levelString = Localizer.Format("#autoLOC_6002246");
	}

	public void SetAttributes()
	{
	}

	public void Lock()
	{
		nameField.interactable = false;
		randomButton.interactable = false;
		maleGender.interactable = false;
		femaleGender.interactable = false;
		pilotRole.interactable = false;
		scientistRole.interactable = false;
		engineerRole.interactable = false;
		touristRole.interactable = false;
		experienceSlider.interactable = false;
		courageSlider.interactable = false;
		stupiditySlider.interactable = false;
		veteranToggle.interactable = false;
		badassToggle.interactable = false;
		submitButton.interactable = false;
	}

	public void Unlock()
	{
		nameField.interactable = true;
		randomButton.interactable = true;
		maleGender.interactable = true;
		femaleGender.interactable = true;
		pilotRole.interactable = true;
		scientistRole.interactable = true;
		engineerRole.interactable = true;
		touristRole.interactable = true;
		experienceSlider.interactable = true;
		courageSlider.interactable = true;
		stupiditySlider.interactable = true;
		veteranToggle.interactable = true;
		badassToggle.interactable = true;
		submitButton.interactable = true;
	}

	public void RefreshControls()
	{
		nameField.text = currentName.Replace(lastName, string.Empty);
		maleGender.isOn = currentGender == ProtoCrewMember.Gender.Male;
		femaleGender.isOn = currentGender == ProtoCrewMember.Gender.Female;
		pilotRole.isOn = currentRole == KerbalRoster.pilotTrait;
		scientistRole.isOn = currentRole == KerbalRoster.scientistTrait;
		engineerRole.isOn = currentRole == KerbalRoster.engineerTrait;
		touristRole.isOn = currentRole == KerbalRoster.touristTrait;
		experienceSlider.minValue = 0f;
		experienceSlider.maxValue = 5f;
		experienceSlider.wholeNumbers = true;
		experienceSlider.value = experienceLevel;
		experienceSliderText.text = levelString + " " + experienceLevel;
		courageSlider.minValue = 0f;
		courageSlider.maxValue = 1f;
		courageSlider.value = courageLevel;
		stupiditySlider.minValue = 0f;
		stupiditySlider.maxValue = 1f;
		stupiditySlider.value = stupidityLevel;
		veteranToggle.isOn = isVeteran;
		badassToggle.isOn = isBadass;
	}

	public void AddListeners()
	{
		nameField.onEndEdit.AddListener(OnNameEdit);
		randomButton.onClick.AddListener(OnRandomClicked);
		maleGender.onValueChanged.AddListener(OnGenderToggle);
		femaleGender.onValueChanged.AddListener(OnGenderToggle);
		pilotRole.onValueChanged.AddListener(OnClassToggle);
		scientistRole.onValueChanged.AddListener(OnClassToggle);
		engineerRole.onValueChanged.AddListener(OnClassToggle);
		touristRole.onValueChanged.AddListener(OnClassToggle);
		experienceSlider.onValueChanged.AddListener(OnExperienceLevelSet);
		courageSlider.onValueChanged.AddListener(OnCourageLevelSet);
		stupiditySlider.onValueChanged.AddListener(OnStupidityLevelSet);
		veteranToggle.onValueChanged.AddListener(OnVeteranToggle);
		badassToggle.onValueChanged.AddListener(OnBadassToggle);
		submitButton.onClick.AddListener(OnSubmitClicked);
	}

	public void KerbalsModified(ProtoCrewMember pcm)
	{
		CheckForErrors();
	}

	public void KerbalsModified(ProtoCrewMember pcm, int count)
	{
		CheckForErrors();
	}

	public void GameLoaded(ConfigNode config)
	{
		CheckForErrors();
	}

	public void CheckForErrors()
	{
		if (!HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().KerbalExperienceEnabled(HighLogic.CurrentGame.Mode))
		{
			experienceLevel = 5;
			experienceSlider.value = experienceLevel;
			experienceSliderText.text = levelString + experienceLevel;
			experienceSlider.interactable = false;
		}
		bool flag = HighLogic.CurrentGame.CrewRoster.Exists(currentName);
		int activeCrewCount = HighLogic.CurrentGame.CrewRoster.GetActiveCrewCount();
		int num = int.MaxValue;
		if (HighLogic.CurrentGame.Mode != 0)
		{
			num = GameVariables.Instance.GetActiveCrewLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.AstronautComplex));
		}
		if (!flag && activeCrewCount < num)
		{
			submitButton.interactable = true;
			submitButtonText.text = Localizer.Format("#autoLOC_900441");
			return;
		}
		submitButton.interactable = false;
		if (flag)
		{
			submitButtonText.text = Localizer.Format("#autoLOC_901066");
		}
		else
		{
			submitButtonText.text = Localizer.Format("#autoLOC_901065");
		}
	}

	public void OnNameEdit(string value)
	{
		currentName = CrewGenerator.GetFullName(value, lastName);
		CheckForErrors();
	}

	public void OnRandomClicked()
	{
		currentName = CrewGenerator.GetRandomName(currentGender, generator);
		RefreshControls();
		CheckForErrors();
	}

	public void OnGenderToggle(bool on)
	{
		if (femaleGender.isOn)
		{
			currentGender = ProtoCrewMember.Gender.Female;
		}
		else
		{
			currentGender = ProtoCrewMember.Gender.Male;
		}
	}

	public void OnClassToggle(bool on)
	{
		if (scientistRole.isOn)
		{
			currentRole = KerbalRoster.scientistTrait;
		}
		else if (engineerRole.isOn)
		{
			currentRole = KerbalRoster.engineerTrait;
		}
		else if (touristRole.isOn)
		{
			currentRole = KerbalRoster.touristTrait;
		}
		else
		{
			currentRole = KerbalRoster.pilotTrait;
		}
	}

	public void OnExperienceLevelSet(float value)
	{
		experienceLevel = Mathf.RoundToInt(value);
		experienceSliderText.text = levelString + " " + experienceLevel;
	}

	public void OnCourageLevelSet(float value)
	{
		courageLevel = value;
	}

	public void OnStupidityLevelSet(float value)
	{
		stupidityLevel = value;
	}

	public void OnVeteranToggle(bool on)
	{
		isVeteran = on;
	}

	public void OnBadassToggle(bool on)
	{
		isBadass = on;
	}

	public void OnSubmitClicked()
	{
		ProtoCrewMember protoCrewMember = new ProtoCrewMember(ProtoCrewMember.KerbalType.Crew, currentName);
		protoCrewMember.rosterStatus = ProtoCrewMember.RosterStatus.Available;
		protoCrewMember.gender = currentGender;
		protoCrewMember.trait = currentRole;
		protoCrewMember.courage = courageLevel;
		protoCrewMember.stupidity = stupidityLevel;
		protoCrewMember.veteran = isVeteran;
		protoCrewMember.isBadass = isBadass;
		protoCrewMember.hasToured = false;
		KerbalRoster.SetExperienceLevel(protoCrewMember, experienceLevel);
		if (!HighLogic.CurrentGame.CrewRoster.AddCrewMember(protoCrewMember))
		{
			Debug.LogError("Cannot create kerbal \"" + base.name + "\".");
		}
		HighLogic.fetch.StartCoroutine(SubmitConfirmation());
	}

	public IEnumerator SubmitConfirmation()
	{
		Lock();
		submitButtonText.text = Localizer.Format("#autoLOC_901067");
		yield return new WaitForSecondsRealtime(2f);
		submitButtonText.text = Localizer.Format("#autoLOC_900441");
		Unlock();
		InitializeLocals();
		RefreshControls();
		CheckForErrors();
	}
}
