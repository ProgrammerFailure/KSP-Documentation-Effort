using System;
using System.Collections.Generic;
using Expansions.Missions.Actions;
using Expansions.Missions.Runtime;
using ns12;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Flow;

public class MEFlowUINode : MonoBehaviour
{
	public enum ButtonAction
	{
		None,
		ToggleDetails,
		Callback,
		CallbackToggle
	}

	public static UnityEngine.Object nodePrefab;

	public static UnityEngine.Object nodeTogglePrefab;

	public TMP_Text title;

	public TMP_Text description;

	public TooltipController_Text completedUT;

	public GameObject leader;

	public GameObject leaderComplete;

	public GameObject leaderEvent;

	public GameObject leaderNext;

	public GameObject leaderEnd;

	public GameObject leaderNextAndEnd;

	public GameObject leaderVessel;

	public Image iconComplete;

	public Image iconEvent;

	public Image unreachableOverlay;

	public Button titleButton;

	public Callback<PointerEventData> buttonCallback;

	public ButtonAction buttonAction;

	public Toggle toggle;

	public Callback<MEFlowUINode> toggleCallback;

	[SerializeField]
	public TooltipController_Text topLineTooltip;

	public PointerClickHandler titleButtonHandler;

	public MENode node;

	public MEFlowParser parser;

	public bool selected;

	public MENode Node => node;

	public static MEFlowUINode Create(MENode node, ButtonAction buttonAction, Callback<PointerEventData> buttonCallback, MEFlowParser parser)
	{
		GameObject obj = (GameObject)UnityEngine.Object.Instantiate(nodePrefab);
		obj.transform.localPosition = Vector3.zero;
		MEFlowUINode component = obj.GetComponent<MEFlowUINode>();
		component.buttonAction = buttonAction;
		component.buttonCallback = buttonCallback;
		component.SetNode(node);
		node.OnUpdateFlowUI = (Callback<MEFlowParser>)Delegate.Combine(node.OnUpdateFlowUI, new Callback<MEFlowParser>(component.OnUpdateFlowUI));
		component.parser = parser;
		return component;
	}

	public static MEFlowUINode Create(MENode node, Callback<MEFlowUINode> toggleCallback, ToggleGroup toggleGroup, MEFlowParser parser)
	{
		GameObject obj = (GameObject)UnityEngine.Object.Instantiate(nodeTogglePrefab);
		obj.transform.localPosition = Vector3.zero;
		MEFlowUINode component = obj.GetComponent<MEFlowUINode>();
		component.buttonAction = ButtonAction.CallbackToggle;
		component.toggleCallback = toggleCallback;
		component.SetNode(node);
		component.toggle.group = toggleGroup;
		toggleGroup.RegisterToggle(component.toggle);
		node.OnUpdateFlowUI = (Callback<MEFlowParser>)Delegate.Combine(node.OnUpdateFlowUI, new Callback<MEFlowParser>(component.OnUpdateFlowUI));
		component.parser = parser;
		return component;
	}

	public void Awake()
	{
		title.text = "";
		description.text = "";
		topLineTooltip.enabled = false;
		completedUT.gameObject.SetActive(value: false);
		completedUT.SetText("");
		if (titleButton != null)
		{
			titleButtonHandler = titleButton.GetComponent<PointerClickHandler>();
			titleButtonHandler.onPointerClick.AddListener(OnButtonClick);
		}
		if (toggle != null)
		{
			toggle.onValueChanged.AddListener(OnToggle);
		}
	}

	public void Start()
	{
		byte[] hashBytes = null;
		string signature = null;
		if (node != null && node.mission != null)
		{
			hashBytes = node.mission.HashBytes;
			signature = node.mission.Signature;
		}
		if (!ExpansionsLoader.IsExpansionInstalled("MakingHistory", hashBytes, signature))
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	public void OnDestroy()
	{
		if (node != null)
		{
			MENode mENode = node;
			mENode.OnUpdateFlowUI = (Callback<MEFlowParser>)Delegate.Remove(mENode.OnUpdateFlowUI, new Callback<MEFlowParser>(OnUpdateFlowUI));
		}
		if (titleButtonHandler != null)
		{
			titleButtonHandler.onPointerClick.RemoveListener(OnButtonClick);
		}
		if (toggle != null)
		{
			if (toggle.group != null)
			{
				toggle.group.UnregisterToggle(toggle);
			}
			toggle.onValueChanged.AddListener(OnToggle);
		}
	}

	public void OnToggle(bool st)
	{
		if (st && !selected)
		{
			OnButtonClick(null);
			selected = true;
		}
		if (!st && selected)
		{
			selected = false;
		}
	}

	public void OnButtonClick(PointerEventData data)
	{
		switch (buttonAction)
		{
		case ButtonAction.ToggleDetails:
			if (data != null && data.button == PointerEventData.InputButton.Left)
			{
				description.gameObject.SetActive(!description.gameObject.activeSelf);
			}
			break;
		case ButtonAction.Callback:
			if (buttonCallback != null)
			{
				buttonCallback(data);
			}
			break;
		case ButtonAction.CallbackToggle:
			if (toggleCallback != null)
			{
				toggleCallback(this);
			}
			break;
		case ButtonAction.None:
			break;
		}
	}

	public void SetNode(MENode node)
	{
		this.node = node;
		title.text = Localizer.Format(node.Title);
		if (node.HasBeenActivated)
		{
			completedUT.gameObject.SetActive(value: true);
			completedUT.SetText("");
			if (node.IsActiveAndPendingVesselLaunch)
			{
				completedUT.SetText("#autoLOC_8006063");
				completedUT.GetComponent<UIStateImage>().SetState("CREATEVESSEL");
			}
			else
			{
				completedUT.SetText("@ " + KSPUtil.PrintDateCompact(node.activatedUT, includeTime: true));
				completedUT.GetComponent<UIStateImage>().SetState("TIMESTAMP");
			}
		}
		else
		{
			completedUT.gameObject.SetActive(value: false);
			completedUT.SetText("");
		}
		SetNodeInfo();
	}

	public void SetNodeInfo()
	{
		if ((HighLogic.LoadedSceneIsGame && HighLogic.CurrentGame.Mode == Game.Modes.MISSION) || (HighLogic.LoadedScene == GameScenes.TRACKSTATION && HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER))
		{
			string text = "";
			if (!string.IsNullOrEmpty(node.description))
			{
				text += StringBuilderCache.Format(Localizer.Format(node.description) + "\n");
			}
			for (int i = 0; i < node.testGroups.Count; i++)
			{
				TestGroup testGroup = node.testGroups[i];
				if (!string.IsNullOrEmpty(testGroup.title))
				{
					text += StringBuilderCache.Format(Localizer.Format(testGroup.title) + "\n");
				}
				for (int j = 0; j < testGroup.testModules.Count; j++)
				{
					ITestModule testModule = testGroup.testModules[j];
					if (!string.IsNullOrEmpty(testModule.GetAppObjectiveInfo()))
					{
						text = ((!Localizer.Tags.ContainsKey(testModule.GetAppObjectiveInfo().Replace("\n", ""))) ? (text + StringBuilderCache.Format(Localizer.Format(testModule.GetAppObjectiveInfo()))) : (text + StringBuilderCache.Format(Localizer.Format(testModule.GetAppObjectiveInfo().Replace("\n", "")) + "\n")));
					}
				}
			}
			for (int k = 0; k < node.actionModules.Count; k++)
			{
				text = ((!Localizer.Tags.ContainsKey(node.actionModules[k].GetAppObjectiveInfo().Replace("\n", ""))) ? (text + StringBuilderCache.Format(Localizer.Format(node.actionModules[k].GetAppObjectiveInfo()))) : (text + StringBuilderCache.Format(Localizer.Format(node.actionModules[k].GetAppObjectiveInfo().Replace("\n", "")) + "\n")));
			}
			description.gameObject.SetActive(value: true);
			description.text = text;
			return;
		}
		description.gameObject.SetActive(value: false);
		int num = 0;
		MENode mENode;
		while (true)
		{
			if (num < node.dockedNodes.Count)
			{
				mENode = node.dockedNodes[num];
				if (mENode.actionModules.Count > 0 && mENode.actionModules[0].GetType() == typeof(ActionMissionScore) && mENode.actionModules[0] as ActionMissionScore != null)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		string awardedScoreDescription = (mENode.actionModules[0] as ActionMissionScore).GetAwardedScoreDescription();
		if (!string.IsNullOrEmpty(awardedScoreDescription))
		{
			description.gameObject.SetActive(value: true);
			description.text = Localizer.Format(awardedScoreDescription);
		}
	}

	public void SetStatus(MEFlowParser parser)
	{
		titleButton.interactable = buttonAction != ButtonAction.None;
		bool flag = node.isNextObjective && node.mission.isStarted;
		bool flag2 = false;
		if (node.mission.activeNode != null)
		{
			flag2 = node.mission.activeNode.IsActiveAndPendingVesselLaunch;
		}
		leaderVessel.gameObject.SetActive(node.IsVesselNode && (node.IsActiveAndPendingVesselLaunch || !node.HasBeenActivated));
		leaderComplete.gameObject.SetActive(node.HasBeenActivated && !node.IsActiveAndPendingVesselLaunch);
		leaderEvent.gameObject.SetActive(node.isEvent && (node.HasBeenActivated || parser.showEvents || MissionSystem.IsTestMode));
		leaderNext.gameObject.SetActive(flag && !node.isEndNode && !flag2);
		leaderNextAndEnd.gameObject.SetActive(flag && node.isEndNode);
		leaderEnd.gameObject.SetActive(!flag && node.isEndNode);
		leader.gameObject.SetActive(node.HasBeenActivated || node.isEvent || node.isEndNode || (flag && !flag2) || node.IsActiveAndPendingVesselLaunch || node.IsVesselNode);
		unreachableOverlay.gameObject.SetActive(!node.HasBeenActivated && !node.IsReachable);
	}

	public void OnUpdateFlowUI(MEFlowParser parser)
	{
		bool flag = (flag = (flag = (flag = parser.showNonObjectives || node.isObjective) || (node.IsVesselNode && node.activatedUT > 0.0 && node.HasPendingVesselLaunch)) && (!node.IsHiddenByEvent || parser.showEvents)) || (parser.showEvents && node.isEvent);
		if (base.gameObject == null)
		{
			return;
		}
		base.gameObject.SetActive(flag);
		if (HighLogic.LoadedSceneIsGame && HighLogic.CurrentGame.Mode == Game.Modes.MISSION)
		{
			if (node != null && node.UpdateInfoOnRefresh)
			{
				SetNodeInfo();
			}
			if (node.HasBeenActivated)
			{
				completedUT.gameObject.SetActive(value: true);
				completedUT.SetText("@ " + KSPUtil.PrintDateCompact(node.activatedUT, includeTime: true));
				completedUT.GetComponent<UIStateImage>().SetState("TIMESTAMP");
			}
			if ((node.HasBeenActivated || !node.IsReachable) && !node.IsActiveAndPendingVesselLaunch)
			{
				description.gameObject.SetActive(value: false);
			}
			buttonAction = ButtonAction.ToggleDetails;
			topLineTooltip.textString = "";
			topLineTooltip.enabled = false;
			if (node.IsVesselNode && node.IsActiveAndPendingVesselLaunch)
			{
				base.gameObject.SetActive(value: true);
				completedUT.gameObject.SetActive(value: true);
				completedUT.SetText("#autoLOC_8006063");
				completedUT.GetComponent<UIStateImage>().SetState("CREATEVESSEL");
				List<ActionCreateVessel> allActionModules = node.GetAllActionModules<ActionCreateVessel>();
				for (int i = 0; i < allActionModules.Count; i++)
				{
					if (allActionModules[i].vesselSituation.playerCreated)
					{
						allActionModules[i].CreateVesselScreenMessage();
					}
				}
				buttonCallback = OnVesselNodeButtonCallback;
				buttonAction = ButtonAction.Callback;
				topLineTooltip.textString = "#autoLOC_8006064";
				topLineTooltip.enabled = true;
			}
		}
		SetStatus(parser);
	}

	public void OnVesselNodeButtonCallback(PointerEventData data)
	{
		if (data == null)
		{
			return;
		}
		if (data.button == PointerEventData.InputButton.Left)
		{
			description.gameObject.SetActive(!description.gameObject.activeSelf);
		}
		else if (node.IsActiveAndPendingVesselLaunch)
		{
			ActionCreateVessel actionVessel = node.mission.GetActionCreateVessel(node);
			string msg = Localizer.Format("#autoLOC_8000302", actionVessel.vesselSituation.vesselName);
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("Create a vessel", msg, Localizer.Format("#autoLOC_8000303"), UISkinManager.GetSkin("KSP window 7"), 350f, new DialogGUIHorizontalLayout(new DialogGUIFlexibleSpace(), new DialogGUIButton(Localizer.Format("#autoLOC_226976"), delegate
			{
			}, 80f, 30f, true), new DialogGUIButton(Localizer.Format("#autoLOC_8006062", actionVessel.vesselSituation.location.facility.Description()), delegate
			{
				HighLogic.CurrentGame.startScene = GameScenes.EDITOR;
				HighLogic.CurrentGame.editorFacility = actionVessel.vesselSituation.location.facility;
				HighLogic.CurrentGame.Parameters.Editor.startUpMode = 0;
				GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE, GameScenes.EDITOR);
				EditorDriver.StartEditor(HighLogic.CurrentGame.editorFacility);
			}))), persistAcrossScenes: false, UISkinManager.GetSkin("KSP window 7"), isModal: false);
		}
	}
}
