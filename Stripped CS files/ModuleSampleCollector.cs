using System.Collections.Generic;
using UnityEngine;

public class ModuleSampleCollector : PartModule
{
	[KSPField(guiFormat = "S", guiActive = true, guiName = "#autoLOC_6001872")]
	public string collectedSampleName = "N/A";

	[KSPField(guiFormat = "F2", guiActive = true, guiName = "#autoLOC_6001873", guiUnits = "#autoLOC_7001412")]
	public float collectedSampleMass;

	[KSPField]
	public float sampleMassMin = 300f;

	[KSPField]
	public float sampleMassMax = 800f;

	[KSPField(isPersistant = true)]
	public bool sampleCollected;

	public PlanetarySample sample;

	public ScreenMessage warningMessage;

	public List<ModuleSampleContainer> containers;

	public bool showTransferDialog;

	public Rect transferDialogRect;

	public Vector2 scrollPos;

	public override void OnStart(StartState state)
	{
		if (state != StartState.Editor && state != 0)
		{
			base.Events["Collect"].active = !sampleCollected;
			base.Events["Discard"].active = sampleCollected;
			base.Events["Transfer"].active = sampleCollected;
			if (sampleCollected)
			{
				sample = new PlanetarySample();
				sample.sampleMass = collectedSampleMass;
				sample.sampleName = collectedSampleName;
			}
			containers = new List<ModuleSampleContainer>();
		}
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_6001890")]
	public void Collect()
	{
		if (base.vessel.LandedOrSplashed)
		{
			if (base.vessel.Landed)
			{
				sample = new PlanetarySample();
				if (!string.IsNullOrEmpty(base.vessel.landedAt))
				{
					sample.sampleName = (collectedSampleName = base.vessel.landedAt);
				}
				else
				{
					sample.sampleName = (collectedSampleName = base.vessel.mainBody.displayName);
				}
				sample.sampleMass = (collectedSampleMass = Random.Range(sampleMassMin, sampleMassMax));
			}
			else if (base.vessel.Splashed)
			{
				sample = new PlanetarySample();
				sample.sampleName = (collectedSampleName = base.vessel.mainBody.displayName + "'s Ocean");
				sample.sampleMass = (collectedSampleMass = Random.Range(sampleMassMin, sampleMassMax) / 2f);
			}
			base.Events["Collect"].active = false;
			base.Events["Transfer"].active = true;
			base.Events["Discard"].active = true;
			sampleCollected = true;
		}
		else
		{
			warningMessage = new ScreenMessage("Sample collection failed! You need to be on the ground or in the water to collect a sample.", 5f, ScreenMessageStyle.UPPER_CENTER);
			ScreenMessages.PostScreenMessage(warningMessage);
		}
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_6001891")]
	public void Discard()
	{
		sample = null;
		collectedSampleMass = 0f;
		collectedSampleName = "N/A";
		sampleCollected = false;
		base.Events["Discard"].active = false;
		base.Events["Transfer"].active = false;
		base.Events["Collect"].active = true;
	}

	public void OnGUI()
	{
		GUI.skin = HighLogic.Skin;
		if (showTransferDialog)
		{
			transferDialogRect = GUILayout.Window(8107, transferDialogRect, drawTransferDialog, "Available Containers:", GUILayout.ExpandHeight(expand: true), GUILayout.ExpandWidth(expand: true));
			if (Event.current.type == EventType.MouseUp && !transferDialogRect.Contains(Event.current.mousePosition))
			{
				showTransferDialog = false;
			}
		}
	}

	public void drawTransferDialog(int id)
	{
		scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(200f), GUILayout.Height(100f));
		foreach (ModuleSampleContainer container in containers)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(container.part.partInfo.title);
			GUILayout.FlexibleSpace();
			if (container.sampleHeld)
			{
				if (!GUILayout.Button("FULL"))
				{
				}
			}
			else if (GUILayout.Button("Transfer"))
			{
				showTransferDialog = false;
				container.TransferSample(sample);
				sample = null;
				collectedSampleMass = 0f;
				collectedSampleName = "N/A";
				sampleCollected = false;
				base.Events["Collect"].active = true;
				base.Events["Discard"].active = false;
				base.Events["Transfer"].active = false;
				break;
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
		GUI.DragWindow();
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_900235")]
	public void Transfer()
	{
		containers.Clear();
		for (int i = 0; i < base.vessel.parts.Count; i++)
		{
			ModuleSampleContainer component = base.vessel.parts[i].gameObject.GetComponent<ModuleSampleContainer>();
			if (component != null)
			{
				containers.Add(component);
			}
		}
		Vector2 vector = Input.mousePosition;
		transferDialogRect = new Rect(vector.x + 15f, vector.y - 150f, 85f, 25f);
		showTransferDialog = true;
	}
}
