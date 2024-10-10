using ns9;
using TMPro;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_Comet]
public class MEGUIParameterComet : MEGUICompoundParameter
{
	public TMP_InputField textSeed;

	public Button buttonRegenSeed;

	public MEGUIParameterDropdownList dropDownCometType;

	public MEGUIParameterDropdownList dropDownCometClass;

	public MEGUIParameterInputField inputName;

	public Comet comet;

	public GAPPrefabDisplay prefabDisplay;

	public Comet FieldValue
	{
		get
		{
			return comet;
		}
		set
		{
			comet = value;
			field.SetValue(value);
		}
	}

	public override void Start()
	{
		base.Start();
		if (FieldValue.persistentId == 0)
		{
			FieldValue.Randomize();
		}
		textSeed.text = FieldValue.seed.ToString();
		inputName.inputField.text = Localizer.Format(FieldValue.name);
		textSeed.onValueChanged.AddListener(InputText_NewSeed);
	}

	public override void Setup(string name, object value)
	{
		base.Setup(name, value);
		comet = value as Comet;
		dropDownCometClass = subParameters["cometClass"] as MEGUIParameterDropdownList;
		inputName = subParameters["name"] as MEGUIParameterInputField;
		dropDownCometType = subParameters["cometType"] as MEGUIParameterDropdownList;
		buttonRegenSeed.onClick.AddListener(Button_RegenerateSeed);
		dropDownCometClass.dropdownList.onValueChanged.AddListener(RefreshGapPrefab);
		textSeed.contentType = TMP_InputField.ContentType.IntegerNumber;
		textSeed.characterLimit = 9;
	}

	public void Button_RegenerateSeed()
	{
		uint randomCometSeed = Comet.GetRandomCometSeed();
		FieldValue.seed = randomCometSeed;
		textSeed.text = randomCometSeed.ToString();
		RefreshGapPrefab();
	}

	public override void DisplayGAP()
	{
		base.DisplayGAP();
		prefabDisplay = MissionEditorLogic.Instance.actionPane.GAPInitialize<GAPPrefabDisplay>();
		RefreshGapPrefab();
	}

	public void InputText_NewSeed(string inputValue)
	{
		FieldValue.seed = uint.Parse(inputValue);
		RefreshGapPrefab();
	}

	public void RefreshGapPrefab(int foo)
	{
		RefreshGapPrefab();
	}

	public void RefreshGapPrefab()
	{
		prefabDisplay.Setup(MissionsUtils.MEPrefab("Prefabs/GAP_ProceduralComet.prefab"), 10f);
		prefabDisplay.PrefabInstance.GetComponent<GAPProceduralComet>().Setup(FieldValue.seed, FieldValue.cometClass);
	}
}
