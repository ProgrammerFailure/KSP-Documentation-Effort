using System;
using System.Collections.Generic;
using System.ComponentModel;
using ns20;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns21;

public class SettingsInputBinding : MonoBehaviour
{
	public enum BindingType
	{
		Key,
		Axis
	}

	public enum BindingVariant
	{
		[Description("#autoLOC_472265")]
		Primary,
		[Description("#autoLOC_900751")]
		Secondary
	}

	public InputBindingModes inputBindingMode;

	public KeyBinding key;

	public AxisBinding axis;

	public TextMeshProUGUI textTitle;

	public TextMeshProUGUI textDescription;

	public TextMeshProUGUI textCurrentAssignment;

	public LayoutGroup layoutModes;

	public Button buttonAccept;

	public Button buttonCancel;

	public Button clearAssignment;

	public SettingsInputBindingMode bindingModePrefab;

	public EventData<bool> OnFinished = new EventData<bool>("OnFinished");

	public static KeyCode[] AllKeys = Enum.GetValues(typeof(KeyCode)) as KeyCode[];

	public static List<KeyCode> UnbindableKeys = new List<KeyCode>
	{
		KeyCode.Mouse0,
		KeyCode.Mouse1,
		KeyCode.Mouse2
	};

	public List<KeyCode> keyValues = new List<KeyCode>(AllKeys.Length);

	public int deviceCount = 11;

	public int axisCount = 20;

	public float[] initialAxisValues;

	public string[] joynames;

	public BindingType bindingType { get; set; }

	public BindingVariant bindingVariant { get; set; }

	public object binding { get; set; }

	public string bindingName { get; set; }

	public bool bindingModes { get; set; }

	public KeyBinding keyBinding => binding as KeyBinding;

	public AxisBinding axisBinding => binding as AxisBinding;

	public void Start()
	{
		EventSystem.current.SetSelectedGameObject(null, null);
		buttonAccept.onClick.AddListener(OnAccept);
		buttonCancel.onClick.AddListener(OnCancel);
		clearAssignment.onClick.AddListener(OnClear);
		textTitle.text = bindingName + " (" + bindingVariant.displayDescription() + ")";
		if (bindingType == BindingType.Axis)
		{
			SetupAxis();
		}
		else
		{
			SetupKey();
		}
		SetupModes();
	}

	public void SetupModes()
	{
		if (bindingModes)
		{
			SettingsInputBindingMode settingsInputBindingMode = UnityEngine.Object.Instantiate(bindingModePrefab);
			settingsInputBindingMode.transform.SetParent(layoutModes.transform, worldPositionStays: false);
			settingsInputBindingMode.text.text = Localizer.Format("#autoLOC_472284");
			settingsInputBindingMode.radioButton.SetState((inputBindingMode & InputBindingModes.Staging) != 0);
			settingsInputBindingMode.radioButton.onToggle.AddListener(OnModeStaging);
			SettingsInputBindingMode settingsInputBindingMode2 = UnityEngine.Object.Instantiate(bindingModePrefab);
			settingsInputBindingMode2.transform.SetParent(layoutModes.transform, worldPositionStays: false);
			settingsInputBindingMode2.text.text = Localizer.Format("#autoLOC_472290");
			settingsInputBindingMode2.radioButton.SetState((inputBindingMode & InputBindingModes.Docking_Translation) != 0);
			settingsInputBindingMode2.radioButton.onToggle.AddListener(OnModeDockingTranslation);
			SettingsInputBindingMode settingsInputBindingMode3 = UnityEngine.Object.Instantiate(bindingModePrefab);
			settingsInputBindingMode3.transform.SetParent(layoutModes.transform, worldPositionStays: false);
			settingsInputBindingMode3.text.text = Localizer.Format("#autoLOC_472296");
			settingsInputBindingMode3.radioButton.SetState((inputBindingMode & InputBindingModes.Docking_Rotation) != 0);
			settingsInputBindingMode3.radioButton.onToggle.AddListener(OnModeDockingRotation);
		}
	}

	public void SetupKey()
	{
		ConfigNode node = new ConfigNode();
		keyBinding.Save(node);
		key = new KeyBinding();
		key.Load(node);
		keyValues.Clear();
		int i = 0;
		for (int num = AllKeys.Length; i < num; i++)
		{
			if (!UnbindableKeys.Contains(AllKeys[i]))
			{
				keyValues.Add(AllKeys[i]);
			}
		}
		for (int j = 0; j < 127; j++)
		{
			keyValues.Add((KeyCode)(j + 128));
		}
		if (bindingVariant == BindingVariant.Primary)
		{
			inputBindingMode = key.switchState;
			if (SettingsScreen.Instance.GetKeyLayoutMap(key.primary.ToString()) == "None")
			{
				textCurrentAssignment.text = Localizer.Format("#autoLOC_472324", "");
			}
			else
			{
				textCurrentAssignment.text = Localizer.Format("#autoLOC_472324", SettingsScreen.Instance.GetKeyLayoutMap(key.primary.ToString()));
			}
		}
		else
		{
			inputBindingMode = key.switchStateSecondary;
			if (SettingsScreen.Instance.GetKeyLayoutMap(key.secondary.ToString()) == "None")
			{
				textCurrentAssignment.text = Localizer.Format("#autoLOC_472329", "");
			}
			else
			{
				textCurrentAssignment.text = Localizer.Format("#autoLOC_472329", SettingsScreen.Instance.GetKeyLayoutMap(key.secondary.ToString()));
			}
		}
		textDescription.text = Localizer.Format("#autoLOC_472332", bindingName);
	}

	public void SetKey(KeyCodeExtended keyCode)
	{
		if (bindingVariant == BindingVariant.Primary)
		{
			key.primary = keyCode;
			key.switchState = inputBindingMode;
			if (SettingsScreen.Instance.GetKeyLayoutMap(key.primary.ToString()) == "None")
			{
				textCurrentAssignment.text = Localizer.Format("#autoLOC_472341", "");
			}
			else
			{
				textCurrentAssignment.text = Localizer.Format("#autoLOC_472341", SettingsScreen.Instance.GetKeyLayoutMap(key.primary.ToString()));
			}
		}
		else
		{
			key.secondary = keyCode;
			key.switchStateSecondary = inputBindingMode;
			if (SettingsScreen.Instance.GetKeyLayoutMap(key.secondary.ToString()) == "None")
			{
				textCurrentAssignment.text = Localizer.Format("#autoLOC_472347", "");
			}
			else
			{
				textCurrentAssignment.text = Localizer.Format("#autoLOC_472347", SettingsScreen.Instance.GetKeyLayoutMap(key.secondary.ToString()));
			}
		}
	}

	public void SetupAxis()
	{
		ConfigNode node = new ConfigNode();
		axisBinding.Save(node);
		axis = new AxisBinding();
		axis.Load(node);
		initialAxisValues = new float[deviceCount * axisCount];
		joynames = Input.GetJoystickNames();
		string text = "Joystick Names:";
		int i = 0;
		for (int num = joynames.Length; i < num; i++)
		{
			if (joynames[i] != string.Empty)
			{
				joynames[i] = InputDevices.TrimDeviceName(joynames[i]);
			}
			else
			{
				joynames[i] = "Joystick " + i;
			}
			text = text + "\n" + i + ": " + joynames[i];
		}
		if (bindingVariant == BindingVariant.Primary)
		{
			textCurrentAssignment.text = Localizer.Format("#autoLOC_472391", (axis.primary.idTag == "None") ? "" : axis.primary.idTag);
			inputBindingMode = axis.primary.switchState;
		}
		else
		{
			textCurrentAssignment.text = Localizer.Format("#autoLOC_472396", (axis.secondary.idTag == "None") ? "" : axis.secondary.idTag);
			inputBindingMode = axis.secondary.switchState;
		}
		textDescription.text = Localizer.Format("#autoLOC_148446", bindingName);
	}

	public void SetAxis(string axisID, string name, int axisIdx, int deviceIdx)
	{
		AxisBinding_Single axisBinding_Single = ((bindingVariant != 0) ? axis.secondary : axis.primary);
		axisBinding_Single.idTag = axisID;
		axisBinding_Single.name = name;
		if (axisIdx != -1)
		{
			axisBinding_Single.title = Localizer.Format("#autoLOC_6001495", name, axisIdx.ToString());
		}
		else
		{
			axisBinding_Single.title = name;
		}
		axisBinding_Single.deviceIdx = deviceIdx;
		axisBinding_Single.axisIdx = axisIdx;
		axisBinding_Single.switchState = inputBindingMode;
		textCurrentAssignment.text = Localizer.Format("#autoLOC_472347", axisBinding_Single.idTag);
		ResetAxisValues();
	}

	public void ResetAxisValues()
	{
		int i = 0;
		for (int num = initialAxisValues.Length; i < num; i++)
		{
			initialAxisValues[i] = Input.GetAxis("joy" + i / axisCount + "." + i % axisCount);
		}
	}

	public void OnAccept()
	{
		if (bindingType == BindingType.Key)
		{
			if (bindingVariant == BindingVariant.Primary)
			{
				key.switchState = inputBindingMode;
			}
			else
			{
				key.switchStateSecondary = inputBindingMode;
			}
			ConfigNode node = new ConfigNode();
			key.Save(node);
			keyBinding.Load(node);
		}
		else
		{
			if (bindingVariant == BindingVariant.Primary)
			{
				axis.primary.switchState = inputBindingMode;
			}
			else
			{
				axis.secondary.switchState = inputBindingMode;
			}
			ConfigNode node2 = new ConfigNode();
			axis.Save(node2);
			axisBinding.Load(node2);
		}
		OnFinished.Fire(data: true);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void OnCancel()
	{
		OnFinished.Fire(data: false);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void OnClear()
	{
		if (bindingType == BindingType.Key)
		{
			SetKey(new KeyCodeExtended());
		}
		else
		{
			SetAxis("None", "None", -1, -1);
		}
	}

	public void Update()
	{
		KeyCodeExtended keyCodeExtended;
		if (bindingType == BindingType.Axis)
		{
			for (int i = 0; i < deviceCount; i++)
			{
				for (int j = 0; j < axisCount; j++)
				{
					if (Mathf.Abs(Input.GetAxis("joy" + i + "." + j) - initialAxisValues[i * axisCount + j]) > 0.5f)
					{
						SetAxis("joy" + i + "." + j, joynames[i], j, i);
					}
				}
			}
		}
		else if (ExtendedInput.DetectKeyDown(keyValues, out keyCodeExtended))
		{
			SetKey(keyCodeExtended);
		}
	}

	public void OnModeStaging()
	{
		if ((inputBindingMode & InputBindingModes.Staging) != 0)
		{
			inputBindingMode &= ~InputBindingModes.Staging;
		}
		else
		{
			inputBindingMode |= InputBindingModes.Staging;
		}
	}

	public void OnModeDockingTranslation()
	{
		if ((inputBindingMode & InputBindingModes.Docking_Translation) != 0)
		{
			inputBindingMode &= ~InputBindingModes.Docking_Translation;
		}
		else
		{
			inputBindingMode |= InputBindingModes.Docking_Translation;
		}
	}

	public void OnModeDockingRotation()
	{
		if ((inputBindingMode & InputBindingModes.Docking_Rotation) != 0)
		{
			inputBindingMode &= ~InputBindingModes.Docking_Rotation;
		}
		else
		{
			inputBindingMode |= InputBindingModes.Docking_Rotation;
		}
	}
}
