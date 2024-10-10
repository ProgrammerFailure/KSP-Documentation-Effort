using System.Collections;
using ns12;
using ns2;
using ns9;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ns11;

[RequireComponent(typeof(PointerClickHandler))]
public class SubassemblyDropZone : TooltipController
{
	public string tooltipTitle;

	public string tooltipDefault;

	public string tooltipNoPart;

	public string tooltipNotAttachable;

	public Tooltip_TitleAndText tooltipPrefab;

	public Callback<ShipTemplate> onAddSubassembly = delegate
	{
	};

	public PointerClickHandler handler;

	public Part subassembly;

	public string subassemblyName;

	public string subassemblyDescription;

	public static SubassemblyDropZone Instance { get; set; }

	public override void Awake()
	{
		Instance = this;
		tooltipDefault = Localizer.Format("#autoLOC_455805");
		tooltipNoPart = Localizer.Format("#autoLOC_455807");
		tooltipNotAttachable = "#autoLOC_455809";
		handler = GetComponent<PointerClickHandler>();
		handler.onPointerUp.AddListener(ButtonInputDelegate);
		base.Awake();
	}

	public override void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
		base.OnDestroy();
	}

	public void ButtonInputDelegate(PointerEventData eventData)
	{
		if (EditorLogic.SelectedPart == null)
		{
			Debug.Log("SubassemblyDropZone: Needs to have a part selected to save");
			return;
		}
		if (!EditorLogic.SelectedPart.isAttachable)
		{
			Debug.Log("SubassemblyDropZone: Cannot save a part which is not attachable");
			return;
		}
		InputLockManager.SetControlLock(ControlTypes.EDITOR_SOFT_LOCK, "SubassemblyDropZone");
		subassembly = EditorLogic.SelectedPart;
		subassemblyName = Localizer.Format("#autoLOC_455855");
		subassemblyDescription = Localizer.Format("#autoLOC_455856");
		SpawnSaveDialog();
	}

	public void SpawnSaveDialog()
	{
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SubassemblySave", Localizer.Format("#autoLOC_7000024"), Localizer.Format("#autoLOC_455866"), null, new DialogGUITextInput(subassemblyName, multiline: false, 64, delegate(string n)
		{
			subassemblyName = EscapeString(n);
			return subassemblyName;
		}, 24f), new DialogGUITextInput(subassemblyDescription, multiline: true, 64, delegate(string n)
		{
			subassemblyDescription = EscapeString(n);
			return subassemblyDescription;
		}, 60f), new DialogGUIButton(Localizer.Format("#autoLOC_455877"), delegate
		{
			OnSave();
		}, dismissOnSelect: true), new DialogGUIButton(Localizer.Format("#autoLOC_455882"), delegate
		{
			StartCoroutine(EndDialogCoroutine());
		}, dismissOnSelect: true)), persistAcrossScenes: false, null).OnDismiss = delegate
		{
			StartCoroutine(EndDialogCoroutine());
		};
	}

	public void OnSave()
	{
		if (ShipConstruction.SubassemblyExists(subassemblyName))
		{
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SubAssemblyOverwrite", Localizer.Format("#autoLOC_455895", subassemblyName), Localizer.Format("#autoLOC_6004041"), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_190811"), SaveSubassembly), new DialogGUIButton(Localizer.Format("#autoLOC_900211"), SpawnSaveDialog)), persistAcrossScenes: false, HighLogic.UISkin).OnDismiss = SpawnSaveDialog;
		}
		else
		{
			SaveSubassembly();
		}
	}

	public IEnumerator EndDialogCoroutine()
	{
		yield return null;
		yield return null;
		InputLockManager.RemoveControlLock("SubassemblyDropZone");
	}

	public override void OnTooltipSpawned(Tooltip instance)
	{
		Tooltip_TitleAndText obj = instance as Tooltip_TitleAndText;
		string text = Localizer.Format(tooltipDefault);
		if (EditorLogic.SelectedPart == null)
		{
			text = text + "\n" + Localizer.Format(tooltipNoPart);
		}
		else if (!EditorLogic.SelectedPart.isAttachable)
		{
			text = text + "\n" + Localizer.Format(tooltipNotAttachable);
		}
		obj.title.text = tooltipTitle;
		obj.label.text = text;
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		UIMasterController.Instance.DespawnTooltip(this);
	}

	public override void OnDisable()
	{
	}

	public override void OnAppFocus(bool focus)
	{
	}

	public void SaveSubassembly()
	{
		SaveSubassembly(subassemblyName, subassemblyDescription, subassembly);
	}

	public void SaveSubassembly(string name, string description, Part subassemblyRootpart)
	{
		subassemblyName = name;
		subassemblyDescription = description;
		subassembly = subassemblyRootpart;
		subassemblyDescription = subassemblyDescription.Replace('{', '(');
		subassemblyDescription = subassemblyDescription.Replace('}', ')');
		subassemblyDescription = subassemblyDescription.Replace('/', ' ');
		subassemblyDescription = subassemblyDescription.Replace('\\', ' ');
		subassemblyDescription = subassemblyDescription.Replace('*', ' ');
		subassemblyDescription = subassemblyDescription.Replace('?', '.');
		subassemblyDescription = subassemblyDescription.Replace('<', ' ');
		subassemblyDescription = subassemblyDescription.Replace('>', ' ');
		subassemblyDescription = subassemblyDescription.Replace(':', ' ');
		subassemblyDescription = subassemblyDescription.Replace('\n', ' ');
		subassemblyDescription = subassemblyDescription.Replace('\r', ' ');
		subassemblyName = subassemblyName.Replace('{', '(');
		subassemblyName = subassemblyName.Replace('}', ')');
		subassemblyName = subassemblyName.Replace('/', ' ');
		subassemblyName = subassemblyName.Replace('\\', ' ');
		subassemblyName = subassemblyName.Replace('*', ' ');
		subassemblyName = subassemblyName.Replace('?', '.');
		subassemblyName = subassemblyName.Replace('<', ' ');
		subassemblyName = subassemblyName.Replace('>', ' ');
		subassemblyName = subassemblyName.Replace(':', ' ');
		subassemblyName = subassemblyName.Replace('\n', ' ');
		subassemblyName = subassemblyName.Replace('\r', ' ');
		ShipConstruct shipConstruct = new ShipConstruct(subassemblyName, subassemblyDescription, subassembly);
		int inverseStage = EditorLogic.RootPart.inverseStage;
		int count = shipConstruct.parts.Count;
		for (int i = 0; i < count; i++)
		{
			shipConstruct.parts[i].inverseStage -= inverseStage;
		}
		ShipConstruction.SaveSubassembly(shipConstruct, subassemblyName);
		if (shipConstruct.vesselDeltaV != null)
		{
			shipConstruct.vesselDeltaV.gameObject.DestroyGameObject();
		}
		EditorPartList.Instance.RefreshSubassemblies();
		subassembly = null;
		StartCoroutine(EndDialogCoroutine());
		PartCategorizer.Instance.AddSubassemblyToSelectedCategory(subassemblyName);
	}

	public string EscapeString(string str)
	{
		str = str.Replace('\r', ' ');
		str = str.Replace('\n', ' ');
		str = str.Replace('"', '\'');
		return str;
	}
}
