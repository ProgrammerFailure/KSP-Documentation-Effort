using System;
using System.Collections;
using System.Collections.Generic;
using ns2;
using ns9;
using UnityEngine;

namespace ns11;

public class ResourceDisplay : UIApp
{
	public delegate void OnShowResource(ResourceItem item);

	public delegate void OnHideResource(ResourceItem item);

	public ResourceItem itemPrefab;

	public ResourceDisplayOptions optionsPrefab;

	public ResourceDisplayOptions options;

	public Transform resourceItemTransform;

	public float resourceItemHeight;

	[HideInInspector]
	public List<ResourceItem> resourceItems;

	public List<PartResourceDefinition> resourceList;

	public bool isCreated;

	public Vessel vessel;

	public bool showStage;

	public bool panelEnabled = true;

	public bool hidden = true;

	public bool displayDisabled = true;

	[SerializeField]
	public GenericAppFrame appFramePrefab;

	public GenericAppFrame appFrame;

	public bool isDirty;

	public bool appStarted;

	public static OnShowResource onShowResource;

	public static OnHideResource onHideResource;

	public HashSet<PartResourceDefinition> resourceHolder = new HashSet<PartResourceDefinition>();

	public HashSet<Part> stageParts = new HashSet<Part>();

	public PartSet stagePartSet;

	public static ResourceDisplay Instance { get; set; }

	public override bool OnAppAboutToStart()
	{
		return true;
	}

	public override ApplicationLauncher.AppScenes GetAppScenes()
	{
		return ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.MAPVIEW;
	}

	public override Vector3 GetAppScreenPos(Vector3 defaultAnchorPos)
	{
		return defaultAnchorPos;
	}

	public override void OnAppInitialized()
	{
		Debug.Log("[ResourceDisplay] OnAppStarted(): id: " + GetInstanceID());
		if (Instance != null)
		{
			Debug.Log("ResourceDisplay already exist, destroying this instance");
			UnityEngine.Object.DestroyImmediate(base.gameObject);
			return;
		}
		Instance = this;
		GameEvents.onVesselWasModified.Add(OnVesselWasModified);
		GameEvents.onVesselChange.Add(OnVesselChanged);
		GameEvents.onVesselDestroy.Add(OnVesselDestroy);
		GameEvents.onVesselGoOffRails.Add(OnVesselGoOffRails);
		appFrame = UnityEngine.Object.Instantiate(appFramePrefab);
		appFrame.transform.SetParent(base.transform, worldPositionStays: false);
		appFrame.transform.localPosition = Vector3.zero;
		appFrame.Setup(base.appLauncherButton, base.name, Localizer.Format("#autoLOC_444646"), 300, 140, scaleHeightToContainList: true);
		appFrame.AddGlobalInputDelegate(base.MouseInput_PointerEnter, base.MouseInput_PointerExit);
		ApplicationLauncher.Instance.AddOnRepositionCallback(appFrame.Reposition);
		resourceItems = new List<ResourceItem>();
		resourceList = new List<PartResourceDefinition>();
		showStage = false;
		HideResourceList();
		displayDisabled = false;
		HideApp();
		appStarted = true;
	}

	public override void OnAppDestroy()
	{
		if (appFrame != null)
		{
			if ((bool)ApplicationLauncher.Instance)
			{
				ApplicationLauncher.Instance.RemoveOnRepositionCallback(appFrame.Reposition);
			}
			appFrame.gameObject.DestroyGameObject();
		}
		GameEvents.onVesselWasModified.Remove(OnVesselWasModified);
		GameEvents.onVesselChange.Remove(OnVesselChanged);
		GameEvents.onVesselDestroy.Remove(OnVesselDestroy);
		GameEvents.onVesselGoOffRails.Remove(OnVesselGoOffRails);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public override void DisplayApp()
	{
		if (appFrame != null)
		{
			appFrame.gameObject.SetActive(value: true);
		}
		ShowResourceList(recreate: false);
		hidden = false;
	}

	public override void HideApp()
	{
		if (appFrame != null)
		{
			appFrame.gameObject.SetActive(value: false);
		}
		HideResourceList();
		hidden = true;
	}

	public static void AddOnShowResource(OnShowResource onShow)
	{
		if (onShowResource == null)
		{
			onShowResource = onShow;
		}
		else
		{
			onShowResource = (OnShowResource)Delegate.Combine(onShowResource, onShow);
		}
	}

	public static void RemoveOnShowResource(OnShowResource onShow)
	{
		if (onShowResource != null)
		{
			onShowResource = (OnShowResource)Delegate.Remove(onShowResource, onShow);
		}
	}

	public static void AddOnHideResource(OnHideResource onHide)
	{
		if (onHideResource == null)
		{
			onHideResource = onHide;
		}
		else
		{
			onHideResource = (OnHideResource)Delegate.Combine(onHideResource, onHide);
		}
	}

	public static void RemoveOnHideResource(OnHideResource onHide)
	{
		if (onHideResource != null)
		{
			onHideResource = (OnHideResource)Delegate.Remove(onHideResource, onHide);
		}
	}

	public void SetItemSelection(ResourceItem item, bool show)
	{
		if (show)
		{
			if (onShowResource != null)
			{
				onShowResource(item);
			}
		}
		else if (onHideResource != null)
		{
			onHideResource(item);
		}
	}

	public new void Hover()
	{
		ShowResourceList(recreate: false);
	}

	public new void HoverOut()
	{
		if (hidden)
		{
			HideResourceList();
		}
	}

	public new void EnablePanel()
	{
		ShowResourceList(recreate: true);
	}

	public new void DisablePanel()
	{
		ForceHideResourceList();
	}

	public void OnVesselWasModified(Vessel v)
	{
		if (v == vessel)
		{
			isDirty = true;
		}
	}

	public void OnVesselChanged(Vessel v)
	{
		if (v != vessel)
		{
			vessel = v;
			displayDisabled = false;
			CreateResourceList();
			ShowResourceListDetermine(recreate: false);
			isDirty = true;
		}
	}

	public void OnVesselDestroy(Vessel v)
	{
		if (v == vessel)
		{
			isDirty = true;
		}
	}

	public void OnVesselGoOffRails(Vessel v)
	{
		if (v == vessel)
		{
			isDirty = true;
		}
	}

	public void Update()
	{
		if (!appStarted)
		{
			return;
		}
		if (isCreated && (!(vessel != FlightGlobals.ActiveVessel) || !(FlightGlobals.ActiveVessel != null)))
		{
			if (isDirty && FlightGlobals.ActiveVessel != null)
			{
				vessel = FlightGlobals.ActiveVessel;
				isDirty = ShowResourceListDetermine(recreate: true);
			}
			else if (!displayDisabled && vessel != null && (vessel.state == Vessel.State.DEAD || vessel.state == Vessel.State.INACTIVE))
			{
				ClearResourceList();
				HideResourceList();
				displayDisabled = true;
				isDirty = false;
			}
		}
		else
		{
			vessel = FlightGlobals.ActiveVessel;
			CreateResourceList();
			HideResourceList();
			isDirty = false;
		}
	}

	public void CreateResourceList()
	{
		resourceHolder.Clear();
		ClearResourceList();
		if (stagePartSet == null)
		{
			stagePartSet = new PartSet(stageParts);
		}
		vessel = FlightGlobals.ActiveVessel;
		int num = 0;
		if (showStage)
		{
			stageParts.Clear();
			int num2 = vessel.currentStage - 2;
			int count = vessel.parts.Count;
			while (count-- > 0)
			{
				Part part = vessel.parts[count];
				if (part.State != PartStates.ACTIVE)
				{
					continue;
				}
				HashSet<Part>.Enumerator enumerator = part.crossfeedPartSet.GetParts().GetEnumerator();
				while (enumerator.MoveNext())
				{
					Part current = enumerator.Current;
					if (!stageParts.Contains(current) && current.inverseStage > num2)
					{
						stageParts.Add(current);
					}
				}
			}
			stagePartSet.RebuildInPlace();
			HashSet<Part>.Enumerator enumerator2 = stageParts.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				Part part = enumerator2.Current;
				if (part.Resources.Count <= 0)
				{
					continue;
				}
				int count2 = part.Resources.Count;
				while (count2-- > 0)
				{
					PartResource partResource = part.Resources[count2];
					if (partResource.isVisible)
					{
						resourceHolder.Add(partResource.info);
					}
				}
			}
		}
		else
		{
			bool flag = FlightGlobals.ActiveVessel.isEVA && FlightGlobals.ActiveVessel.evaController != null;
			int count3 = FlightGlobals.ActiveVessel.parts.Count;
			while (count3-- > 0)
			{
				PartResource partResource2 = null;
				if (flag && FlightGlobals.ActiveVessel.evaController.PropellantResource != null)
				{
					partResource2 = FlightGlobals.ActiveVessel.evaController.PropellantResource;
					resourceHolder.Add(partResource2.info);
				}
				Part part = FlightGlobals.ActiveVessel.parts[count3];
				if (part.Resources.Count <= 0)
				{
					continue;
				}
				int count4 = part.Resources.Count;
				while (count4-- > 0)
				{
					PartResource partResource3 = part.Resources[count4];
					if (partResource3.isVisible && (!flag || partResource2 == null || partResource3.info != partResource2.info))
					{
						resourceHolder.Add(partResource3.info);
					}
				}
			}
		}
		resourceList = new List<PartResourceDefinition>(resourceHolder);
		int i = 0;
		for (int count5 = resourceList.Count; i < count5; i++)
		{
			ResourceItem resourceItem = UnityEngine.Object.Instantiate(itemPrefab);
			resourceItem.gameObject.SetActive(value: false);
			appFrame.scrollList.AddItem(resourceItem.GetComponent<UIListItem>());
			num++;
			resourceItem.Setup(resourceList[i], showStage, stagePartSet);
			resourceItems.Add(resourceItem);
		}
		if (num > 0 || showStage)
		{
			options = UnityEngine.Object.Instantiate(optionsPrefab);
			options.Setup(showStage);
			options.gameObject.SetActive(value: false);
			appFrame.scrollList.AddItem(options.GetComponent<UIListItem>());
			num++;
		}
		isCreated = true;
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(ResizeList());
		}
	}

	public IEnumerator ResizeList()
	{
		yield return new WaitForEndOfFrame();
		appFrame.AutoScale();
	}

	public bool ShowResourceListDetermine(bool recreate)
	{
		if (!base.hover && hidden && !base.appLauncherButton.IsHovering)
		{
			return isDirty;
		}
		ShowResourceList(recreate);
		return !recreate;
	}

	public void ShowResourceList(bool recreate)
	{
		if (!displayDisabled)
		{
			if (recreate)
			{
				CreateResourceList();
			}
			int count = resourceItems.Count;
			for (int i = 0; i < count; i++)
			{
				resourceItems[i].gameObject.SetActive(value: true);
			}
			if (options != null)
			{
				options.gameObject.SetActive(value: true);
			}
		}
	}

	public void HideResourceList()
	{
		int count = resourceItems.Count;
		for (int i = 0; i < count; i++)
		{
			resourceItems[i].gameObject.SetActive(value: false);
		}
		if (options != null)
		{
			options.gameObject.SetActive(value: false);
		}
	}

	public void ForceHideResourceList()
	{
		int count = resourceItems.Count;
		for (int i = 0; i < count; i++)
		{
			resourceItems[i].gameObject.SetActive(value: false);
		}
		if (options != null)
		{
			options.gameObject.SetActive(value: false);
		}
	}

	public void ClearResourceList()
	{
		resourceItems.Clear();
		appFrame.scrollList.Clear(destroyElements: true);
	}

	public void ToggleStageView()
	{
		showStage = !showStage;
		CreateResourceList();
		ShowResourceListDetermine(recreate: false);
	}

	public void Refresh()
	{
		CreateResourceList();
		ShowResourceListDetermine(recreate: true);
	}
}
