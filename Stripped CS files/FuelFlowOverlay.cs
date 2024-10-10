using System.Collections;
using System.Collections.Generic;
using CompoundParts;
using ns3;
using TMPro;
using UnityEngine;
using Vectrosity;

public class FuelFlowOverlay : MonoBehaviour
{
	public static FuelFlowOverlay instance;

	public GameObject gizmoNode;

	public GameObject gizmoProvider;

	public GameObject gizmoConsumer;

	public GameObject gizmoTarget;

	public Texture lineTexture;

	public Texture lineArrowTexture;

	public Material lineMaterial;

	public SCCFlowGraphUCFinder sccFlowGraphUCFinder;

	public HashSet<Part> sccLookups;

	public Part partShowing;

	public List<GameObject> gizmoShowing;

	public List<VectorLine> lineShowing;

	public Dictionary<Part, TextMeshPro> textShowingLookup;

	public GameObject targetGizmoShowing;

	public HashSet<Part> seenVertexes = new HashSet<Part>();

	public Vector3 forwardDirection;

	public float offset;

	public const float FLOW_VIZ_RATE = 0.75f;

	public float calculateFrame;

	public Coroutine updateDaemon;

	public bool updateDaemonRunning;

	public bool IsDisplaying { get; set; }

	public void Awake()
	{
		if (instance != null)
		{
			Object.Destroy(this);
			return;
		}
		instance = this;
		GameEvents.onEditorUndo.Add(RespawnEvent);
		GameEvents.onEditorSetBackup.Add(RespawnEvent);
		GameEvents.onEditorPodSelected.Add(DespawnEvent);
		GameEvents.onEditorNewShipDialogDismiss.Add(ClearOverlay);
		GameEvents.onPartCrossfeedStateChange.Add(RespawnEvent);
		GameEvents.onPartFuelLookupStateChange.Add(RedrawEvent);
		GameEvents.onPartPriorityChanged.Add(RedrawEvent);
		GameEvents.onPartDestroyed.Add(OnPartDestroyed);
	}

	public void OnDestroy()
	{
		GameEvents.onEditorUndo.Remove(RespawnEvent);
		GameEvents.onEditorSetBackup.Remove(RespawnEvent);
		GameEvents.onEditorPodSelected.Remove(DespawnEvent);
		GameEvents.onEditorNewShipDialogDismiss.Remove(ClearOverlay);
		GameEvents.onPartCrossfeedStateChange.Remove(RespawnEvent);
		GameEvents.onPartFuelLookupStateChange.Remove(RedrawEvent);
		GameEvents.onPartPriorityChanged.Remove(RedrawEvent);
		GameEvents.onPartDestroyed.Remove(OnPartDestroyed);
	}

	public void SpawnOverlay(Part part)
	{
		if (partShowing != part)
		{
			ClearOverlay();
			InternalSpawn(part);
		}
	}

	public void RespawnOverlay(Part part)
	{
		if (!(partShowing == null) && IsDisplaying)
		{
			Part tmpPartShowing = partShowing;
			ClearOverlay();
			StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
			{
				InternalSpawn(tmpPartShowing);
			}));
		}
	}

	public void RedrawOverlay(Part part)
	{
		if (IsDisplaying && textShowingLookup.TryGetValue(part, out var value))
		{
			UpdateText(value, part);
		}
	}

	public void ClearOverlay()
	{
		if (partShowing != null)
		{
			partShowing.fuelFlowOverlayEnabled = false;
		}
		IsDisplaying = false;
		if (updateDaemon != null)
		{
			StopCoroutine(updateDaemon);
		}
		updateDaemonRunning = false;
		partShowing = null;
		if (gizmoShowing != null)
		{
			int i = 0;
			for (int count = gizmoShowing.Count; i < count; i++)
			{
				GameObject gameObject = gizmoShowing[i];
				if (gameObject != null)
				{
					Object.Destroy(gameObject);
				}
			}
			gizmoShowing.Clear();
		}
		if (lineShowing != null)
		{
			int j = 0;
			for (int count2 = lineShowing.Count; j < count2; j++)
			{
				VectorLine line = lineShowing[j];
				if (line != null && line.rectTransform != null)
				{
					VectorLine.Destroy(ref line);
				}
			}
			lineShowing.Clear();
		}
		if (textShowingLookup != null)
		{
			IEnumerator<TextMeshPro> enumerator = textShowingLookup.Values.GetEnumerator();
			while (enumerator.MoveNext())
			{
				TextMeshPro current = enumerator.Current;
				if (current != null)
				{
					Object.Destroy(current.gameObject);
				}
			}
			textShowingLookup.Clear();
		}
		if (targetGizmoShowing != null)
		{
			Object.Destroy(targetGizmoShowing);
		}
	}

	public bool isConsumer(Part part)
	{
		Calculate(part);
		return sccFlowGraphUCFinder.request.reverseEntries.ContainsKey(part);
	}

	public bool isProvider(Part part)
	{
		Calculate(part);
		return sccFlowGraphUCFinder.delivery.reverseEntries.ContainsKey(part);
	}

	public bool isConsumerOrProvider(Part part)
	{
		Calculate(part);
		if (!sccFlowGraphUCFinder.request.reverseEntries.ContainsKey(part))
		{
			return sccFlowGraphUCFinder.delivery.reverseEntries.ContainsKey(part);
		}
		return true;
	}

	public void RespawnEvent(ShipConstruct ship)
	{
		RespawnOverlay(EditorLogic.SelectedPart);
	}

	public void RespawnEvent(Part part)
	{
		RespawnOverlay(part);
	}

	public void DespawnEvent(Part part)
	{
		ClearOverlay();
	}

	public void RedrawEvent(Part part)
	{
		RedrawOverlay(part);
	}

	public void RedrawEvent(GameEvents.HostedFromToAction<bool, Part> data)
	{
		RedrawOverlay(data.to);
	}

	public void OnPartDestroyed(Part p)
	{
		if (instance != null && EditorLogic.SelectedPart != null)
		{
			RespawnOverlay(EditorLogic.SelectedPart);
		}
		else
		{
			ClearOverlay();
		}
	}

	public void Calculate(Part part)
	{
		if ((float)Time.frameCount != calculateFrame)
		{
			if (HighLogic.LoadedSceneIsEditor)
			{
				sccFlowGraphUCFinder = new SCCFlowGraphUCFinder(EditorLogic.fetch.ship.Parts);
			}
			else if (HighLogic.LoadedSceneIsFlight)
			{
				sccFlowGraphUCFinder = new SCCFlowGraphUCFinder(FlightGlobals.ActiveVessel.Parts);
			}
			calculateFrame = Time.frameCount;
		}
	}

	public void InternalSpawn(Part part)
	{
		bool flag = isConsumer(part);
		bool flag2 = isProvider(part);
		if (!flag && !flag2)
		{
			return;
		}
		IsDisplaying = true;
		VectorLine.SetCamera3D();
		updateDaemon = StartCoroutine(UpdateDaemon());
		partShowing = part;
		partShowing.fuelFlowOverlayEnabled = true;
		gizmoShowing = new List<GameObject>();
		lineShowing = new List<VectorLine>();
		textShowingLookup = new Dictionary<Part, TextMeshPro>();
		if (flag)
		{
			sccLookups = sccFlowGraphUCFinder.sccFlowGraph.GetAllRequests(part);
		}
		else
		{
			sccLookups = sccFlowGraphUCFinder.sccFlowGraph.GetAllDeliveries(part);
		}
		sccFlowGraphUCFinder.sccFlowGraph.stackFlowGraph.BuildTransformGuides();
		GameObject gameObject = Object.Instantiate(gizmoTarget);
		gameObject.name = "flowGizmo_" + part.name;
		gameObject.transform.SetParent(part.transform, worldPositionStays: false);
		gameObject.transform.localPosition = Vector3.zero;
		targetGizmoShowing = gameObject;
		seenVertexes.Clear();
		IEnumerator<Part> enumerator = sccLookups.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if ((flag && isConsumer(enumerator.Current)) || (!flag && isProvider(enumerator.Current)))
			{
				Connect(enumerator.Current, flag);
			}
		}
	}

	public bool Connect(Part part, bool request)
	{
		if (part != null && !seenVertexes.Contains(part))
		{
			seenVertexes.Add(part);
			bool result = false;
			float width = 12f;
			GameObject gameObject = null;
			if (isConsumer(part))
			{
				gameObject = Object.Instantiate(gizmoConsumer);
				result = true;
			}
			else if (isProvider(part))
			{
				CreateText(new GameObject("flowPriText_" + part.name).AddComponent<TextMeshPro>(), part);
				gameObject = Object.Instantiate(gizmoProvider);
				result = true;
			}
			else
			{
				gameObject = Object.Instantiate(gizmoNode);
			}
			gizmoShowing.Add(gameObject);
			gameObject.transform.SetParent(part.transform, worldPositionStays: false);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale *= 0.15f;
			Vertex<Part> vertex = null;
			vertex = ((!request) ? sccFlowGraphUCFinder.sccFlowGraph.stackFlowGraph.delivery.lookup[part] : sccFlowGraphUCFinder.sccFlowGraph.stackFlowGraph.request.lookup[part]);
			int i = 0;
			for (int count = vertex.Dependencies.Count; i < count; i++)
			{
				Vertex<Part> vertex2 = vertex.Dependencies[i];
				if (!seenVertexes.Contains(vertex2.Value))
				{
					bool num = Connect(vertex2.Value, request);
					bool flag = vertex2.Value.FindModuleImplementing<CModuleFuelLine>() != null;
					bool num2 = num || flag;
					bool isMoving = num2;
					Color color = (num2 ? Color.green : Color.grey);
					VectorLine vectorLine = null;
					if (vertex.transformGuide != null && vertex.transformGuide.TryGetValue(vertex2, out var value))
					{
						vectorLine = new VectorLine("flowLine_aug_" + vertex.Value.name + "->" + vertex2.Value.name, new List<Vector3>
						{
							value.Key.position,
							value.Value.position
						}, width, LineType.Discrete);
						UpdateLine(vectorLine, vertex.Value, color, isMoving);
						vectorLine.rectTransform.SetParent(part.transform, worldPositionStays: true);
						VectorLine vectorLine2 = null;
						vectorLine2 = ((!request) ? new VectorLine("flowLine_aug2_" + vertex.Value.name + "->" + vertex2.Value.name, new List<Vector3>
						{
							vertex2.Value.transform.position,
							value.Key.position
						}, width, LineType.Discrete) : new VectorLine("flowLine_aug2_" + vertex.Value.name + "->" + vertex2.Value.name, new List<Vector3>
						{
							vertex.Value.transform.position,
							value.Key.position
						}, width, LineType.Discrete));
						UpdateLine(vectorLine2, vertex.Value, color, isMoving);
						vectorLine2.rectTransform.SetParent(part.transform, worldPositionStays: true);
					}
					else
					{
						vectorLine = ((flag || isConsumer(vertex2.Value)) ? new VectorLine("flowLine_" + vertex.Value.name + "->" + vertex2.Value.name, new List<Vector3>
						{
							vertex2.Value.transform.position,
							vertex.Value.transform.position
						}, width, LineType.Discrete) : new VectorLine("flowLine_" + vertex.Value.name + "->" + vertex2.Value.name, new List<Vector3>
						{
							vertex.Value.transform.position,
							vertex2.Value.transform.position
						}, width, LineType.Discrete));
						UpdateLine(vectorLine, vertex.Value, color, isMoving);
						vectorLine.rectTransform.SetParent(part.transform, worldPositionStays: true);
					}
					if (num)
					{
						result = true;
					}
				}
			}
			return result;
		}
		return false;
	}

	public void CreateText(TextMeshPro text, Part part)
	{
		text.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		text.transform.SetParent(part.transform, worldPositionStays: true);
		text.fontSize = 1f;
		text.alignment = TextAlignmentOptions.Center;
		text.font = UISkinManager.TMPFont;
		text.fontSize = 2f;
		text.outlineWidth = 0.15f;
		UpdateText(text, part);
		textShowingLookup.Add(part, text);
	}

	public void UpdateText(TextMeshPro text, Part part)
	{
		text.SetText("p" + part.GetResourcePriority());
		forwardDirection = Camera.main.transform.forward;
		forwardDirection.y = 0f;
		text.transform.rotation = Quaternion.LookRotation(forwardDirection);
		text.transform.localPosition = Camera.main.transform.right * 0.15f + Vector3.up * 0.15f;
	}

	public void UpdateLine(VectorLine line, Part parent, Color color, bool isMoving)
	{
		line.SetColor(color);
		line.rectTransform.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		line.rectTransform.SetParent(parent.transform, worldPositionStays: true);
		line.material = lineMaterial;
		line.continuousTexture = false;
		line.textureScale = 6f;
		if (isMoving)
		{
			line.textureOffset = offset;
			line.texture = lineArrowTexture;
		}
		else
		{
			line.texture = lineTexture;
		}
		line.Draw3D();
		lineShowing.Add(line);
	}

	public IEnumerator UpdateDaemon()
	{
		yield return null;
		yield return null;
		if (updateDaemonRunning)
		{
			yield break;
		}
		updateDaemonRunning = true;
		while ((bool)this)
		{
			if (lineShowing != null)
			{
				int i = 0;
				for (int count = lineShowing.Count; i < count; i++)
				{
					lineShowing[i].Draw3D();
				}
			}
			if (textShowingLookup != null)
			{
				IEnumerator<Part> enumerator = textShowingLookup.Keys.GetEnumerator();
				IEnumerator<TextMeshPro> enumerator2 = textShowingLookup.Values.GetEnumerator();
				while (enumerator.MoveNext() && enumerator2.MoveNext())
				{
					UpdateText(enumerator2.Current, enumerator.Current);
				}
			}
			yield return new WaitForSecondsRealtime(0.1f);
		}
	}

	public void Update()
	{
		offset += Time.deltaTime * 0.75f;
		if (lineShowing == null)
		{
			return;
		}
		int i = 0;
		for (int count = lineShowing.Count; i < count; i++)
		{
			VectorLine vectorLine = lineShowing[i];
			if (vectorLine.texture == lineArrowTexture)
			{
				vectorLine.textureOffset = offset;
			}
		}
	}
}
