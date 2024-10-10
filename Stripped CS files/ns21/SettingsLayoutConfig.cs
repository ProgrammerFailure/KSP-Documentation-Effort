using System;
using ns20;
using ns9;
using TMPro;
using UnityEngine;

namespace ns21;

public class SettingsLayoutConfig : SettingsWindow
{
	public SettingsInputGroupTitle groupTitlePrefab;

	public SettingsKeyboardLayoutOs keyboardLayoutOsPrefab;

	public SettingsKeyboardLayoutInput keyboardLayoutInputPrefab;

	public SettingsKeyboardLayoutApply keyboardLayoutApplyPrefab;

	public GameObject rebindPopUp;

	public TextMeshProUGUI rebindPopUpDescription;

	public GameObject layoutPopUp;

	public TextMeshProUGUI layoutPopUpDescription;

	public SettingsKeyboardLayoutInput keyboardLayoutInput;

	public ConfigNode currentLayout;

	public Action<bool> callback;

	public static SettingsLayoutConfig Instance { get; set; }

	public static ConfigNode LayoutConfig
	{
		get
		{
			if (Instance != null && Instance.keyboardLayoutInput != null)
			{
				return Instance.keyboardLayoutInput.LayoutConfig;
			}
			return null;
		}
	}

	public static void SaveChanges()
	{
		if (LayoutConfig != null)
		{
			Instance.keyboardLayoutInput.LoadCurrentLayout();
		}
	}

	public static void OnRevert()
	{
		if (LayoutConfig != null)
		{
			Instance.keyboardLayoutInput.CleanLayout();
		}
	}

	public void Awake()
	{
		Instance = this;
	}

	public void Start()
	{
		SettingsInputGroupTitle settingsInputGroupTitle = UnityEngine.Object.Instantiate(groupTitlePrefab);
		settingsInputGroupTitle.transform.SetParent(base.Template.layout.transform, worldPositionStays: false);
		settingsInputGroupTitle.SetTitleText(Localizer.Format("#autoLOC_6001210"));
		UnityEngine.Object.Instantiate(keyboardLayoutOsPrefab).transform.SetParent(base.Template.layout.transform, worldPositionStays: false);
		keyboardLayoutInput = UnityEngine.Object.Instantiate(keyboardLayoutInputPrefab);
		keyboardLayoutInput.transform.SetParent(base.Template.layout.transform, worldPositionStays: false);
		keyboardLayoutInput.Init(this);
		SettingsKeyboardLayoutApply settingsKeyboardLayoutApply = UnityEngine.Object.Instantiate(keyboardLayoutApplyPrefab);
		settingsKeyboardLayoutApply.transform.SetParent(base.Template.layout.transform, worldPositionStays: false);
		settingsKeyboardLayoutApply.buttonApply.onClick.AddListener(OnApply);
		rebindPopUp.transform.SetParent(SettingsScreen.Instance.popUpReset.transform.parent, worldPositionStays: false);
		layoutPopUp.transform.SetParent(SettingsScreen.Instance.popUpReset.transform.parent, worldPositionStays: false);
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void OnApply()
	{
		rebindPopUpDescription.text = Localizer.Format("#autoLOC_6001211", keyboardLayoutInput.SelectedLayout);
		rebindPopUp.SetActive(value: true);
	}

	public void ShowLayoutChangePopUp(Action<bool> popUpCallback)
	{
		callback = popUpCallback;
		layoutPopUpDescription.text = Localizer.Format("#autoLOC_6001212", keyboardLayoutInput.SelectedLayout);
		layoutPopUp.SetActive(value: true);
	}

	public void OnLayoutChange()
	{
		SettingsInputKey[] componentsInChildren = base.Template.transform.parent.GetComponentsInChildren<SettingsInputKey>(includeInactive: true);
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			componentsInChildren[i].UpdateInputSettings();
		}
	}

	public void OnRebindResult(bool result)
	{
		rebindPopUp.SetActive(value: false);
		if (result)
		{
			SettingsInputKey[] componentsInChildren = base.Template.transform.parent.GetComponentsInChildren<SettingsInputKey>(includeInactive: true);
			keyboardLayoutInput.SetKeysLayout();
			int i = 0;
			for (int num = componentsInChildren.Length; i < num; i++)
			{
				componentsInChildren[i].OnRevert();
			}
		}
	}

	public void OnLayoutResult(bool result)
	{
		layoutPopUp.SetActive(value: false);
		if (callback != null)
		{
			callback(result);
		}
	}
}
