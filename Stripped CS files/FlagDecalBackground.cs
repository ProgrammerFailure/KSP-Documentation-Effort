using System;
using System.Collections.Generic;
using Expansions;
using ns9;
using UnityEngine;

public class FlagDecalBackground : FlagDecal, IThumbnailSetup, IPartMassModifier, IPartCostModifier
{
	[KSPField(isPersistant = true)]
	[SerializeField]
	public string currentflagUrl = "";

	[UI_Toggle(affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6006059")]
	public bool displayingPortrait = true;

	[KSPField(guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6006059")]
	[UI_Label]
	public string displayingPortraitLabel = "";

	[UI_ChooseOption(affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6006057")]
	public int flagSize;

	[KSPField(guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6006057")]
	[UI_Label]
	public string flagSizeLabel = "";

	[KSPField(guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6006057")]
	[UI_Label]
	public string sizeLocked = "#autoLOC_8003622";

	[KSPField(isPersistant = true)]
	public int placementID;

	[SerializeField]
	public List<FlagMesh> flagMeshes;

	public UI_ChooseOption uI_ChooseOption;

	public UI_Toggle uI_FlagOrientationToggle;

	public FlagNameManager flagNameInfo;

	public int flagSizeOffset;

	public bool variantEventSubscribed;

	public float mass;

	public float cost;

	public bool updateFlagTexturebySym;

	public bool hasSurfaceAttachedParts;

	public override void OnAwake()
	{
		base.OnAwake();
		if (flagMeshes == null)
		{
			flagMeshes = new List<FlagMesh>();
		}
		sizeLocked = Localizer.Format("#autoLOC_8003622");
	}

	public override void OnStart(StartState state)
	{
		if (HighLogic.fetch != null && HighLogic.LoadedSceneIsEditor)
		{
			uI_ChooseOption = base.Fields["flagSize"].uiControlEditor as UI_ChooseOption;
			UI_ChooseOption obj = uI_ChooseOption;
			obj.onFieldChanged = (Callback<BaseField, object>)Delegate.Combine(obj.onFieldChanged, new Callback<BaseField, object>(EnableCurrentFlagMesh));
			UI_ChooseOption obj2 = uI_ChooseOption;
			obj2.onSymmetryFieldChanged = (Callback<BaseField, object>)Delegate.Combine(obj2.onSymmetryFieldChanged, new Callback<BaseField, object>(EnableCurrentFlagMesh));
			uI_FlagOrientationToggle = base.Fields["displayingPortrait"].uiControlEditor as UI_Toggle;
			uI_FlagOrientationToggle.enabledText = "#autoLOC_6006060";
			uI_FlagOrientationToggle.disabledText = "#autoLOC_6006061";
			base.Fields["displayingPortrait"].OnValueModified += DisplayingPortrait_OnValueModified;
			UI_Toggle uI_Toggle = uI_FlagOrientationToggle;
			uI_Toggle.onSymmetryFieldChanged = (Callback<BaseField, object>)Delegate.Combine(uI_Toggle.onSymmetryFieldChanged, new Callback<BaseField, object>(DisplayingPortrait_OnValueSymmetryModified));
			base.Events["ToggleFlag"].guiActiveEditor = false;
			Part obj3 = base.part;
			obj3.OnEditorDetach = (Callback)Delegate.Combine(obj3.OnEditorDetach, new Callback(OnPartDetached));
		}
		LoadFlagsTransformAndRenderer();
		UpdateUIChooseOptions();
		if (base.part.variants != null && !variantEventSubscribed)
		{
			GameEvents.onVariantApplied.Add(OnVariantApplied);
			variantEventSubscribed = true;
		}
		if (HighLogic.LoadedSceneIsEditor && string.IsNullOrEmpty(currentflagUrl))
		{
			currentflagUrl = EditorLogic.FlagURL;
		}
		if (HighLogic.LoadedSceneIsEditor || HighLogic.LoadedSceneIsFlight || (ExpansionsLoader.IsExpansionInstalled("MakingHistory") && HighLogic.LoadedSceneIsMissionBuilder))
		{
			updateFlag(currentflagUrl);
		}
		if (isMirrored)
		{
			FlipTexture();
		}
		hasSurfaceAttachedParts = HasSurfaceAttachedParts();
		GameEvents.onEditorPartEvent.Add(OnEditorPartEvent);
	}

	public new void OnDestroy()
	{
		if (uI_ChooseOption != null)
		{
			UI_ChooseOption obj = uI_ChooseOption;
			obj.onFieldChanged = (Callback<BaseField, object>)Delegate.Remove(obj.onFieldChanged, new Callback<BaseField, object>(EnableCurrentFlagMesh));
		}
		base.Fields["displayingPortrait"].OnValueModified -= DisplayingPortrait_OnValueModified;
		if (uI_ChooseOption != null)
		{
			UI_ChooseOption obj2 = uI_ChooseOption;
			obj2.onSymmetryFieldChanged = (Callback<BaseField, object>)Delegate.Remove(obj2.onSymmetryFieldChanged, new Callback<BaseField, object>(EnableCurrentFlagMesh));
		}
		if (uI_FlagOrientationToggle != null)
		{
			UI_Toggle uI_Toggle = uI_FlagOrientationToggle;
			uI_Toggle.onSymmetryFieldChanged = (Callback<BaseField, object>)Delegate.Remove(uI_Toggle.onSymmetryFieldChanged, new Callback<BaseField, object>(DisplayingPortrait_OnValueSymmetryModified));
		}
		GameEvents.onVariantApplied.Remove(OnVariantApplied);
		GameEvents.onEditorPartEvent.Remove(OnEditorPartEvent);
		if (HighLogic.LoadedSceneIsEditor && base.part != null)
		{
			Part obj3 = base.part;
			obj3.OnEditorDetach = (Callback)Delegate.Remove(obj3.OnEditorDetach, new Callback(OnPartDetached));
		}
	}

	public override void updateFlag(string flagURL)
	{
		currentflagUrl = flagURL;
		flagTexture = GameDatabase.Instance.GetTexture(flagURL, asNormalMap: false);
		if (flagTexture == null)
		{
			Debug.LogWarning("[FlagDecalBackground Warning!]: Flag URL is given as " + currentflagUrl + ", but no texture found in database with that name", base.gameObject);
		}
		EnableCurrentFlagMesh(null, null);
	}

	public override void OnInventoryModeDisable()
	{
		if (base.part.variants != null && !variantEventSubscribed)
		{
			GameEvents.onVariantApplied.Add(OnVariantApplied);
			variantEventSubscribed = true;
		}
		if (flagMeshes == null)
		{
			flagMeshes = new List<FlagMesh>();
		}
		LoadFlagsTransformAndRenderer();
		if (HighLogic.LoadedSceneIsEditor || HighLogic.LoadedSceneIsFlight || (ExpansionsLoader.IsExpansionInstalled("MakingHistory") && HighLogic.LoadedSceneIsMissionBuilder))
		{
			updateFlag(currentflagUrl);
		}
		UpdateFlagRenderers();
		if (isMirrored)
		{
			FlipTexture();
		}
	}

	public void LoadFlagsTransformAndRenderer(GameObject icon = null)
	{
		if (flagMeshes == null)
		{
			return;
		}
		bool flag = base.part != null;
		if (icon != null)
		{
			flag = false;
		}
		if (!flag && base.part == null)
		{
			return;
		}
		for (int i = 0; i < flagMeshes.Count; i++)
		{
			if (flag)
			{
				flagMeshes[i].flagTransform = base.part.FindModelTransform(flagMeshes[i].meshName);
			}
			else
			{
				flagMeshes[i].flagTransform = TransformExtension.FindChild(icon.transform, flagMeshes[i].meshName);
			}
			if (flagMeshes[i].flagTransform != null)
			{
				flagMeshes[i].flagRend = flagMeshes[i].flagTransform.GetComponent<Renderer>();
				if (flagMeshes[i].flagRend == null)
				{
					flagMeshes.RemoveAt(i);
					i--;
				}
				else
				{
					flagMeshes[i].flagRend.material.renderQueue = 2500;
				}
			}
			else
			{
				flagMeshes.RemoveAt(i);
				i--;
			}
		}
		flagNameInfo = new FlagNameManager(flagMeshes);
	}

	public void DisplayingPortrait_OnValueModified(object arg1)
	{
		EnableCurrentFlagMesh(null, null);
	}

	public void DisplayingPortrait_OnValueSymmetryModified(BaseField field, object arg1)
	{
		EnableCurrentFlagMesh(null, null);
	}

	[KSPEvent(guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6006058")]
	public void SetFlag()
	{
		if (HighLogic.LoadedSceneIsEditor || !(EditorLogic.fetch == null) || !(EditorLogic.fetch.flagBrowserButton == null))
		{
			EditorLogic.fetch.flagBrowserButton.SpawnBrowser();
			EditorLogic.fetch.flagBrowserButton.isFlagDecalBrowsing = true;
			FlagBrowser browser = EditorLogic.fetch.flagBrowserButton.browser;
			browser.OnDismiss = (Callback)Delegate.Combine(browser.OnDismiss, new Callback(UnsubscribeToFlagSelect));
			updateFlagTexturebySym = true;
			FlagBrowserButton flagBrowserButton = EditorLogic.fetch.flagBrowserButton;
			flagBrowserButton.OnFlagSelectedURL = (Callback<string>)Delegate.Combine(flagBrowserButton.OnFlagSelectedURL, new Callback<string>(SetFlagBrowserTexture));
		}
	}

	public void SetFlagBrowserTexture(string flagUrl)
	{
		if (updateFlagTexturebySym && base.part.symmetryCounterparts.Count > 0)
		{
			updateFlagTexturebySym = false;
			int index = base.part.Modules.IndexOf(this);
			for (int i = 0; i < base.part.symmetryCounterparts.Count; i++)
			{
				if (base.part.symmetryCounterparts[i].isSymmetryCounterPart(base.part))
				{
					(base.part.symmetryCounterparts[i].Modules[index] as FlagDecalBackground).SetFlagBrowserTexture(flagUrl);
				}
			}
		}
		updateFlag(flagUrl);
		UpdateFlagRenderers();
		UnsubscribeToFlagSelect();
		base.part.RefreshHighlighter();
	}

	public void UnsubscribeToFlagSelect()
	{
		GameEvents.onFlagSelect.Remove(SetFlagBrowserTexture);
		FlagBrowser browser = EditorLogic.fetch.flagBrowserButton.browser;
		browser.OnDismiss = (Callback)Delegate.Remove(browser.OnDismiss, new Callback(UnsubscribeToFlagSelect));
		FlagBrowserButton flagBrowserButton = EditorLogic.fetch.flagBrowserButton;
		flagBrowserButton.OnFlagSelectedURL = (Callback<string>)Delegate.Remove(flagBrowserButton.OnFlagSelectedURL, new Callback<string>(SetFlagBrowserTexture));
	}

	public void ToolboxSetFlagTexture(GameObject icon, string flagURL = "")
	{
		LoadFlagsTransformAndRenderer(icon);
		if (flagNameInfo.HasSizeNames(FlagOrientation.PORTRAIT, 0))
		{
			flagSize = flagNameInfo.SizeNames(FlagOrientation.PORTRAIT, 0).Count - 1;
			flagSizeOffset = 0;
		}
		else if (flagNameInfo.HasSizeNames(FlagOrientation.LANDSCAPE, 0))
		{
			flagSize = flagNameInfo.SizeNames(FlagOrientation.LANDSCAPE, 0).Count - 1;
			flagSizeOffset = 0;
		}
		else if (flagNameInfo.HasSizeNames(FlagOrientation.PORTRAIT, 1000))
		{
			flagSize = flagNameInfo.SizeNames(FlagOrientation.PORTRAIT, 1000).Count - 1;
			flagSizeOffset = 1000;
		}
		else if (flagNameInfo.HasSizeNames(FlagOrientation.LANDSCAPE, 1000))
		{
			flagSize = flagNameInfo.SizeNames(FlagOrientation.LANDSCAPE, 1000).Count - 1;
			flagSizeOffset = 1000;
		}
		updateFlag(string.IsNullOrEmpty(flagURL) ? EditorLogic.FlagURL : flagURL);
		UpdateFlagRenderers();
	}

	public void AssumeSnapshotPosition(GameObject icon, ProtoPartSnapshot partSnapshot)
	{
		string value = currentflagUrl;
		if (partSnapshot != null)
		{
			ProtoPartModuleSnapshot protoPartModuleSnapshot = partSnapshot.FindModule("FlagDecalBackground");
			if (protoPartModuleSnapshot != null)
			{
				value = protoPartModuleSnapshot.moduleValues.GetValue("currentflagUrl");
			}
		}
		else
		{
			FlagDecalBackground flagDecalBackground = null;
			if (HighLogic.LoadedSceneIsEditor && EditorLogic.fetch != null && EditorLogic.SelectedPart != null && EditorLogic.fetch.IsCurrentPartFlag)
			{
				flagDecalBackground = EditorLogic.SelectedPart.FindModuleImplementing<FlagDecalBackground>();
			}
			if (HighLogic.LoadedSceneIsFlight && EVAConstructionModeController.Instance != null && EVAConstructionModeController.Instance.evaEditor != null && EVAConstructionModeController.Instance.evaEditor.SelectedPart != null && EVAConstructionModeController.Instance.evaEditor.IsCurrentPartFlag)
			{
				flagDecalBackground = EVAConstructionModeController.Instance.evaEditor.SelectedPart.FindModuleImplementing<FlagDecalBackground>();
			}
			if ((bool)flagDecalBackground)
			{
				value = flagDecalBackground.currentflagUrl;
			}
		}
		ToolboxSetFlagTexture(icon, value);
	}

	public string ThumbSuffix(ProtoPartSnapshot partSnapshot)
	{
		string result = currentflagUrl;
		if (partSnapshot != null)
		{
			ProtoPartModuleSnapshot protoPartModuleSnapshot = partSnapshot.FindModule("FlagDecalBackground");
			if (protoPartModuleSnapshot != null)
			{
				result = protoPartModuleSnapshot.moduleValues.GetValue("currentflagUrl");
				result = result.Replace("/", "_");
			}
		}
		return result;
	}

	public void OnPartDetached()
	{
		placementID = 0;
	}

	public void UpdateUIChooseOptions()
	{
		base.Fields["flagSize"].guiActiveEditor = !hasSurfaceAttachedParts;
		base.Fields["flagSizeLabel"].guiActiveEditor = hasSurfaceAttachedParts;
		base.Fields["sizeLocked"].guiActiveEditor = hasSurfaceAttachedParts;
		if (!hasSurfaceAttachedParts)
		{
			if (uI_ChooseOption != null)
			{
				if (displayingPortrait)
				{
					uI_ChooseOption.options = flagNameInfo.SizeNames(FlagOrientation.PORTRAIT, flagSizeOffset).ToArray();
					uI_ChooseOption.display = flagNameInfo.DisplayNames(FlagOrientation.PORTRAIT, flagSizeOffset).ToArray();
				}
				else
				{
					uI_ChooseOption.options = flagNameInfo.SizeNames(FlagOrientation.LANDSCAPE, flagSizeOffset).ToArray();
					uI_ChooseOption.display = flagNameInfo.DisplayNames(FlagOrientation.LANDSCAPE, flagSizeOffset).ToArray();
				}
			}
			base.Fields["displayingPortrait"].guiActiveEditor = IsFlagOrientationCounterPartAvailable();
			base.Fields["displayingPortraitLabel"].guiActiveEditor = false;
		}
		else
		{
			displayingPortraitLabel = (displayingPortrait ? Localizer.Format("#autoLOC_6006060") : Localizer.Format("#autoLOC_6006061"));
			if (displayingPortrait)
			{
				flagSizeLabel = flagNameInfo.DisplayNames(FlagOrientation.PORTRAIT, flagSizeOffset).ToArray()[flagSize];
			}
			else
			{
				flagSizeLabel = flagNameInfo.DisplayNames(FlagOrientation.LANDSCAPE, flagSizeOffset).ToArray()[flagSize];
			}
			base.Fields["displayingPortrait"].guiActiveEditor = false;
			base.Fields["displayingPortraitLabel"].guiActiveEditor = IsFlagOrientationCounterPartAvailable();
		}
	}

	public bool IsFlagOrientationCounterPartAvailable()
	{
		int num = 0;
		FlagOrientation flagOrientation = FlagOrientation.LANDSCAPE;
		int num2 = 0;
		while (true)
		{
			if (num2 < flagMeshes.Count)
			{
				if (flagMeshes[num2].index + flagMeshes[num2].indexOffset == flagSize)
				{
					if (num == 0)
					{
						flagOrientation = flagMeshes[num2].flagOrientation;
						num++;
					}
					else if (flagOrientation != flagMeshes[num2].flagOrientation)
					{
						break;
					}
				}
				num2++;
				continue;
			}
			return false;
		}
		num++;
		return true;
	}

	public void EnableCurrentFlagMesh(BaseField field, object obj)
	{
		if (flagMeshes == null)
		{
			return;
		}
		for (int i = 0; i < flagMeshes.Count; i++)
		{
			bool flag = false;
			int index = flagMeshes[i].index;
			if (flagSize == index)
			{
				string[] array = flagMeshes[i].variantNames.Split(',');
				string value = base.part.variants?.SelectedVariant.Name;
				if (array.IndexOf(value) > -1)
				{
					mass = flagMeshes[i].mass;
					cost = flagMeshes[i].cost;
					if (displayingPortrait)
					{
						flag = flagMeshes[i].flagOrientation == FlagOrientation.PORTRAIT;
					}
					else if (flagMeshes[i].flagOrientation == FlagOrientation.LANDSCAPE)
					{
						flag = flagMeshes[i].flagOrientation == FlagOrientation.LANDSCAPE;
					}
				}
			}
			if (flag)
			{
				textureQuadRenderer = flagMeshes[i].flagRend;
				flagSizeOffset = flagMeshes[i].indexOffset;
			}
			flagMeshes[i].flagTransform.gameObject.SetActive(flag);
		}
		UpdateUIChooseOptions();
	}

	public override void OnVariantApplied(Part appliedPart, PartVariant partVariant)
	{
		if (!(base.part == appliedPart))
		{
			return;
		}
		UpdateFlagRenderers();
		if (isMirrored)
		{
			FlipTexture();
		}
		EnableCurrentFlagMesh(null, null);
		if (flagSizeOffset == 1000)
		{
			base.part.PhysicsSignificance = 0;
			base.part.dragModel = Part.DragModel.CUBE;
			if (base.part.physicalSignificance == Part.PhysicalSignificance.NONE)
			{
				base.part.PromoteToPhysicalPart();
			}
		}
		else
		{
			base.part.PhysicsSignificance = 1;
			base.part.dragModel = Part.DragModel.NONE;
			if (base.part.physicalSignificance == Part.PhysicalSignificance.FULL)
			{
				base.part.DemoteToPhysicslessPart();
			}
		}
	}

	public void UpdateFlagRenderers()
	{
		if (flagMeshes == null)
		{
			return;
		}
		for (int i = 0; i < flagMeshes.Count; i++)
		{
			if (flagMeshes[i].flagRend != null)
			{
				flagMeshes[i].flagRend.material.SetTexture("_MainTex", flagTexture);
			}
		}
	}

	public void CheckSymmetry()
	{
		EnableCurrentFlagMesh(null, null);
	}

	public bool HasSurfaceAttachedParts()
	{
		int num = 0;
		while (true)
		{
			if (num < base.part.transform.childCount)
			{
				Part component = base.part.transform.GetChild(num).GetComponent<Part>();
				if (component != null && component.attachMode == AttachModes.SRF_ATTACH)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public void OnEditorPartEvent(ConstructionEventType evt, Part p)
	{
		if (evt == ConstructionEventType.PartAttached || evt == ConstructionEventType.PartDetached)
		{
			hasSurfaceAttachedParts = HasSurfaceAttachedParts();
			UpdateUIChooseOptions();
		}
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_6006054");
	}

	public override void OnSave(ConfigNode node)
	{
		base.OnSave(node);
		if (flagMeshes != null)
		{
			for (int i = 0; i < flagMeshes.Count; i++)
			{
				ConfigNode configNode = new ConfigNode();
				configNode.name = "MESH";
				configNode.AddValue("name", flagMeshes[i].name);
				configNode.AddValue("displayName", flagMeshes[i].displayName);
				configNode.AddValue("meshName", flagMeshes[i].meshName);
				configNode.AddValue("orientation", flagMeshes[i].flagOrientation.ToString());
				configNode.AddValue("mass", flagMeshes[i].mass);
				configNode.AddValue("cost", flagMeshes[i].cost);
				configNode.AddValue("variantNames", flagMeshes[i].variantNames);
				configNode.AddValue("indexOffset", flagMeshes[i].indexOffset);
				node.AddNode(configNode);
			}
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (flagMeshes != null && flagMeshes.Count > 0)
		{
			return;
		}
		base.OnLoad(node);
		ConfigNode[] nodes = node.GetNodes("MESH");
		int num = 0;
		for (int i = 0; i < nodes.Length; i++)
		{
			string value = string.Empty;
			string value2 = string.Empty;
			string value3 = string.Empty;
			FlagOrientation value4 = FlagOrientation.LANDSCAPE;
			float value5 = 0f;
			float value6 = 0f;
			string value7 = string.Empty;
			int value8 = 0;
			nodes[i].TryGetValue("name", ref value);
			nodes[i].TryGetValue("displayName", ref value2);
			nodes[i].TryGetValue("meshName", ref value3);
			nodes[i].TryGetEnum("orientation", ref value4, FlagOrientation.LANDSCAPE);
			nodes[i].TryGetValue("mass", ref value5);
			nodes[i].TryGetValue("cost", ref value6);
			nodes[i].TryGetValue("variantNames", ref value7);
			nodes[i].TryGetValue("indexOffset", ref value8);
			if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(value2) && !string.IsNullOrEmpty(value3))
			{
				int index = FlagMeshIndex(value);
				FlagMesh flagMesh = new FlagMesh();
				flagMesh.name = value;
				flagMesh.displayName = value2;
				flagMesh.meshName = value3;
				flagMesh.index = index;
				flagMesh.flagOrientation = value4;
				if (flagMesh.flagOrientation == FlagOrientation.PORTRAIT)
				{
					num++;
				}
				flagMesh.mass = value5;
				flagMesh.cost = value6;
				flagMesh.variantNames = value7;
				flagMesh.indexOffset = value8;
				flagMeshes.Add(flagMesh);
			}
		}
		if (num <= 0)
		{
			displayingPortrait = false;
		}
	}

	public int FlagMeshIndex(string name)
	{
		int num = -1;
		for (int i = 0; i < flagMeshes.Count; i++)
		{
			if (string.Equals(flagMeshes[i].name, name))
			{
				num = flagMeshes[i].index;
				break;
			}
		}
		if (num == -1)
		{
			for (int j = 0; j < flagMeshes.Count; j++)
			{
				if (flagMeshes[j].index > num)
				{
					num = flagMeshes[j].index;
				}
			}
			num++;
		}
		return num;
	}

	public float GetModuleMass(float defaultMass, ModifierStagingSituation sit)
	{
		return mass;
	}

	public ModifierChangeWhen GetModuleMassChangeWhen()
	{
		return ModifierChangeWhen.FIXED;
	}

	public float GetModuleCost(float defaultCost, ModifierStagingSituation sit)
	{
		return cost;
	}

	public ModifierChangeWhen GetModuleCostChangeWhen()
	{
		return ModifierChangeWhen.FIXED;
	}
}
