using System;
using System.Collections.Generic;
using ns2;
using ns9;
using Strategies;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

public class Administration : MonoBehaviour
{
	public class StrategyWrapper
	{
		public Strategy strategy { get; set; }

		public StrategyListItem stratListIcon { get; set; }

		public UIRadioButton button { get; set; }

		public UIRadioButton ButtonInUse
		{
			get
			{
				if (stratListIcon == null)
				{
					return button;
				}
				return stratListIcon.toggleButton;
			}
		}

		public StrategyWrapper(Strategy strategy, StrategyListItem stratListIcon)
		{
			this.strategy = strategy;
			this.stratListIcon = stratListIcon;
		}

		public StrategyWrapper(Strategy strategy, UIRadioButton button)
		{
			this.strategy = strategy;
			this.button = button;
		}

		public void OnTrue(PointerEventData data, UIRadioButton.CallType callType)
		{
			Instance.SetSelectedStrategy(this);
		}

		public void OnFalse(PointerEventData data, UIRadioButton.CallType callType)
		{
			if (callType == UIRadioButton.CallType.USER)
			{
				Instance.UnselectStrategy();
			}
		}
	}

	public static Administration Instance;

	public UIStateButton btnAcceptCancel;

	public Slider sliderCommitment;

	public RectTransform panelCommitmentSlider;

	public RawImage selectedStrategyImage;

	public TextMeshProUGUI selectedStrategyTitle;

	public TextMeshProUGUI selectedStrategyDescription;

	public UIStatePanel textPanel;

	public UIList scrollListStrategies;

	public UIList scrollListKerbals;

	public UIList scrollListActive;

	public UIListItem prefabStratListContainer;

	public UIListItem prefabStratListItem;

	public UIListItem prefabKerbalItem;

	public UIListItem prefabActiveStrat;

	public Texture defaultIcon;

	public TextMeshProUGUI activeStratCount;

	public GameObject avatarLighting;

	public PopupDialog strategyConfirmationDialog;

	public int activeStrategyCount;

	public int maxActiveStrategies = 5;

	public float maxStrategyCommitLevel = 1f;

	public StrategyWrapper SelectedWrapper { get; set; }

	public int ActiveStrategyCount => activeStrategyCount;

	public int MaxActiveStrategies => maxActiveStrategies;

	public float MaxStrategyCommitLevel => maxStrategyCommitLevel;

	public void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("Administration: Instance already exists.");
			base.gameObject.DestroyGameObject();
			return;
		}
		Instance = this;
		btnAcceptCancel.onClickState.AddListener(BtnInputAccept);
		avatarLighting = UnityEngine.Object.Instantiate(AssetBase.GetPrefab("Strategy_AvatarLights"));
		avatarLighting.transform.parent = base.transform;
		sliderCommitment.onValueChanged.AddListener(OnSliderCommitmentValueChanged);
	}

	public void Start()
	{
		maxActiveStrategies = GameVariables.Instance.GetActiveStrategyLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.Administration));
		maxStrategyCommitLevel = GameVariables.Instance.GetStrategyCommitRange(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.Administration));
		CreateStrategiesList(StrategySystem.Instance.SystemConfig.Departments);
		CreateActiveStratList();
		UnselectStrategy();
		UpdateStrategyCount();
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
		btnAcceptCancel.onClickState.RemoveListener(BtnInputAccept);
	}

	public void CreateStrategiesList(List<DepartmentConfig> departments)
	{
		scrollListStrategies.Clear(destroyElements: true);
		scrollListKerbals.Clear(destroyElements: true);
		float height = scrollListStrategies.transform.parent.GetComponent<RectTransform>().rect.height;
		int count = departments.Count;
		for (int i = 0; i < count; i++)
		{
			DepartmentConfig departmentConfig = departments[i];
			UIListItem uIListItem = UnityEngine.Object.Instantiate(prefabStratListContainer);
			UIList componentInChildren = uIListItem.GetComponentInChildren<UIList>();
			List<Strategy> strategies = StrategySystem.Instance.GetStrategies(departmentConfig.Name);
			List<Strategy> list = new List<Strategy>(strategies.Count);
			int count2 = strategies.Count;
			for (int j = 0; j < count2; j++)
			{
				if (!strategies[j].IsActive)
				{
					list.Add(strategies[j]);
				}
			}
			AddStrategiesListItem(componentInChildren, list);
			float preferredHeight = Math.Max(height, list.Count * 54 + 10);
			uIListItem.GetComponent<LayoutElement>().preferredHeight = preferredHeight;
			AddKerbalListItem(departmentConfig);
			scrollListStrategies.AddItem(uIListItem);
		}
	}

	public void AddStrategiesListItem(UIList itemList, List<Strategy> strategies)
	{
		int i = 0;
		for (int count = strategies.Count; i < count; i++)
		{
			Texture iconImage = strategies[i].Config.IconImage;
			if (iconImage == null)
			{
				iconImage = Instance.defaultIcon;
			}
			UIListItem uIListItem = UnityEngine.Object.Instantiate(prefabStratListItem);
			StrategyListItem component = uIListItem.GetComponent<StrategyListItem>();
			component.Initialize(iconImage, strategies[i].Title);
			string reason = "";
			StrategyWrapper strategyWrapper = new StrategyWrapper(strategies[i], component);
			component.SetupButton(strategies[i].CanBeActivated(out reason), strategyWrapper, strategyWrapper.OnTrue, strategyWrapper.OnFalse);
			itemList.AddItem(uIListItem);
		}
	}

	public void AddKerbalListItem(DepartmentConfig dep)
	{
		string headName = "<color=#" + RUIutils.ColorToHex(dep.Color) + ">" + dep.HeadName + "\n(" + dep.Title + ")</color>";
		UIListItem uIListItem = UnityEngine.Object.Instantiate(prefabKerbalItem);
		uIListItem.GetComponent<KerbalListItem>().Initialize(headName, dep.Description, dep.AvatarPrefab);
		scrollListKerbals.AddItem(uIListItem);
	}

	public void UpdateStrategyCount()
	{
		activeStratCount.text = Localizer.Format("#autoLOC_439627", activeStrategyCount, maxActiveStrategies);
	}

	public void CreateActiveStratList()
	{
		scrollListActive.Clear(destroyElements: true);
		activeStrategyCount = 0;
		StrategyWrapper wrapper = null;
		int i = 0;
		for (int count = StrategySystem.Instance.Strategies.Count; i < count; i++)
		{
			if (activeStrategyCount >= maxActiveStrategies)
			{
				break;
			}
			if (StrategySystem.Instance.Strategies[i].IsActive)
			{
				scrollListActive.AddItem(CreateActiveStratItem(StrategySystem.Instance.Strategies[i], out wrapper));
				activeStrategyCount++;
			}
		}
		UpdateStrategyCount();
	}

	public UIListItem CreateActiveStratItem(Strategy strategy, out StrategyWrapper wrapper)
	{
		UIListItem uIListItem = UnityEngine.Object.Instantiate(prefabActiveStrat);
		ActiveStrategyListItem component = uIListItem.GetComponent<ActiveStrategyListItem>();
		UIRadioButton component2 = uIListItem.GetComponent<UIRadioButton>();
		wrapper = null;
		if (strategy != null)
		{
			wrapper = new StrategyWrapper(strategy, component2);
			component2.Data = wrapper;
			component2.onTrue.AddListener(wrapper.OnTrue);
			component2.onFalse.AddListener(wrapper.OnFalse);
			if (strategy.IsActive)
			{
				Texture iconImage = wrapper.strategy.Config.IconImage;
				if (iconImage == null)
				{
					iconImage = defaultIcon;
				}
				component.Setup("<b><color=" + XKCDColors.HexFormat.KSPBadassGreen + ">" + strategy.Title + "</color></b>", strategy.Effect, iconImage as Texture2D);
			}
		}
		return uIListItem;
	}

	public StrategyWrapper AddActiveStratItem(Strategy strategy)
	{
		StrategyWrapper wrapper = null;
		scrollListActive.AddItem(CreateActiveStratItem(strategy, out wrapper));
		activeStrategyCount++;
		UpdateStrategyCount();
		return wrapper;
	}

	public void SetSelectedStrategy(StrategyWrapper wrapper)
	{
		textPanel.SetState("stratText");
		SelectedWrapper = wrapper;
		Texture iconImage = wrapper.strategy.Config.IconImage;
		if (iconImage == null)
		{
			iconImage = defaultIcon;
		}
		btnAcceptCancel.gameObject.SetActive(value: true);
		if (wrapper.strategy.IsActive)
		{
			btnAcceptCancel.SetState("cancel");
			if (wrapper.strategy.CanBeDeactivated(out var reason))
			{
				SetStrategyDescription(iconImage, wrapper.strategy.Title, wrapper.strategy.Description, wrapper.strategy.Effect, "");
			}
			else
			{
				SetStrategyDescription(iconImage, wrapper.strategy.Title, wrapper.strategy.Description, wrapper.strategy.Effect, reason);
			}
			btnAcceptCancel.Enable(wrapper.strategy.CanBeDeactivated(out reason));
		}
		else
		{
			btnAcceptCancel.SetState("accept");
			string reason = "";
			btnAcceptCancel.Enable(wrapper.strategy.CanBeActivated(out reason));
			SetStrategyDescription(iconImage, wrapper.strategy.Title, wrapper.strategy.Description, wrapper.strategy.Effect, reason);
		}
		if (wrapper.strategy.HasFactorSlider && !wrapper.strategy.IsActive)
		{
			panelCommitmentSlider.gameObject.SetActive(value: true);
			SetEffectSliderValue(wrapper.strategy.Factor, 0f, 1f);
		}
		else
		{
			panelCommitmentSlider.gameObject.SetActive(value: false);
		}
	}

	public void UnselectStrategy()
	{
		textPanel.SetState("nothing");
		SelectedWrapper = null;
		btnAcceptCancel.gameObject.SetActive(value: false);
		panelCommitmentSlider.gameObject.SetActive(value: false);
	}

	public void SetStrategyDescription(Texture image, string title, string description, string effects, string reason)
	{
		selectedStrategyImage.texture = image;
		selectedStrategyTitle.text = "<b><color=" + XKCDColors.HexFormat.KSPBadassGreen + ">" + title + "</color></b>";
		selectedStrategyDescription.text = GetStrategyDescription(description, effects, reason);
	}

	public void UpdateStrategyDescription(string title, string description, string effects, string reason)
	{
		selectedStrategyDescription.text = GetStrategyDescription(description, effects, reason);
	}

	public string GetStrategyDescription(string description, string effects, string reason)
	{
		return ((reason == string.Empty) ? string.Empty : ("<color=#ff7512>" + reason + "</color>\n\n")) + description + "\n\n" + effects;
	}

	public void BtnInputAccept(string state)
	{
		string text = "";
		if (!(state == "accept"))
		{
			if (state == "cancel" && SelectedWrapper.strategy.CanBeDeactivated(out var _))
			{
				text = "";
				if (SelectedWrapper.strategy.InitialCostFunds != 0f || SelectedWrapper.strategy.InitialCostScience != 0f || SelectedWrapper.strategy.InitialCostReputation != 0f)
				{
					text = Localizer.Format("#autoLOC_439851");
				}
				text += "\n";
				strategyConfirmationDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("StrategyConfirmation", Localizer.Format("#autoLOC_439854", text), Localizer.Format("#autoLOC_464288"), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_439855"), OnCancelConfirm), new DialogGUIButton(Localizer.Format("#autoLOC_439856"), OnPopupDismiss)), persistAcrossScenes: false, HighLogic.UISkin);
				strategyConfirmationDialog.OnDismiss = OnPopupDismiss;
			}
			return;
		}
		text = "";
		if ((SelectedWrapper.strategy.InitialCostFunds != 0f || SelectedWrapper.strategy.InitialCostScience != 0f || SelectedWrapper.strategy.InitialCostReputation != 0f) && activeStrategyCount <= maxActiveStrategies)
		{
			text = Localizer.Format("#autoLOC_439827");
			if (SelectedWrapper.strategy.InitialCostFunds != 0f)
			{
				text = text + Localizer.Format("#autoLOC_439829", SelectedWrapper.strategy.InitialCostFunds.ToString("N0")) + " ";
			}
			if (SelectedWrapper.strategy.InitialCostScience != 0f)
			{
				text = text + Localizer.Format("#autoLOC_439831", SelectedWrapper.strategy.InitialCostScience.ToString("N0")) + " ";
			}
			if (SelectedWrapper.strategy.InitialCostReputation != 0f)
			{
				text += Localizer.Format("#autoLOC_439833", SelectedWrapper.strategy.InitialCostReputation.ToString("N0"));
			}
			text += Localizer.Format("#autoLOC_439834");
		}
		text += "\n";
		string msg = Localizer.Format("#autoLOC_439838", text);
		if (SelectedWrapper.strategy.NoDuration)
		{
			msg = Localizer.Format("#autoLOC_8003399", text);
		}
		strategyConfirmationDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("StrategyConfirmation", msg, Localizer.Format("#autoLOC_464288"), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_439839"), OnAcceptConfirm), new DialogGUIButton(Localizer.Format("#autoLOC_439840"), OnPopupDismiss)), persistAcrossScenes: false, HighLogic.UISkin);
		strategyConfirmationDialog.OnDismiss = OnPopupDismiss;
	}

	public void OnAcceptConfirm()
	{
		OnPopupDismiss();
		if (SelectedWrapper.strategy.Activate())
		{
			StrategyWrapper selectedStrategy = AddActiveStratItem(SelectedWrapper.strategy);
			SetSelectedStrategy(selectedStrategy);
			CreateStrategiesList(StrategySystem.Instance.SystemConfig.Departments);
			SelectedWrapper.ButtonInUse.Value = true;
		}
		StrategySystem.Instance.StartCoroutine(CallbackUtil.DelayedCallback(2, delegate
		{
			if (!SelectedWrapper.strategy.IsActive)
			{
				UnselectStrategy();
			}
		}));
	}

	public void OnCancelConfirm()
	{
		OnPopupDismiss();
		if (SelectedWrapper.strategy.Deactivate())
		{
			UnselectStrategy();
			RedrawPanels();
		}
	}

	public void OnPopupDismiss()
	{
	}

	public void RedrawPanels()
	{
		CreateActiveStratList();
		CreateStrategiesList(StrategySystem.Instance.SystemConfig.Departments);
	}

	public void OnSliderCommitmentValueChanged(float value)
	{
		if (value < 1f)
		{
			sliderCommitment.value = 1f;
			value = 1f;
		}
		SelectedWrapper.strategy.Factor = Mathf.InverseLerp(sliderCommitment.minValue, sliderCommitment.maxValue, value);
		UpdateStrategyStats();
	}

	public void SetEffectSliderValue(float value, float min, float max)
	{
		float t = Mathf.InverseLerp(min, max, Mathf.Clamp01(value));
		sliderCommitment.value = Mathf.Lerp(sliderCommitment.minValue, sliderCommitment.maxValue, t);
	}

	public void UpdateStrategyStats()
	{
		string reason = "";
		if (SelectedWrapper.strategy.CanBeActivated(out reason))
		{
			btnAcceptCancel.Enable(enable: true);
			UpdateStrategyDescription(SelectedWrapper.strategy.Title, SelectedWrapper.strategy.Description, SelectedWrapper.strategy.Effect, "");
			SelectedWrapper.stratListIcon.UpdateButton(acceptable: true);
		}
		else
		{
			btnAcceptCancel.Enable(enable: false);
			UpdateStrategyDescription(SelectedWrapper.strategy.Title, SelectedWrapper.strategy.Description, SelectedWrapper.strategy.Effect, reason);
			SelectedWrapper.stratListIcon.UpdateButton(acceptable: false);
		}
	}
}
