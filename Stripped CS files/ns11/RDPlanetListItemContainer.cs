using System;
using System.Collections.Generic;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

[RequireComponent(typeof(UIListItem))]
public class RDPlanetListItemContainer : MonoBehaviour
{
	public delegate void SelectionCallback(RDPlanetListItemContainer container, bool selected);

	public SelectionCallback selectionCallback;

	public RawImage planetRawImage;

	[NonSerialized]
	public GameObject planet;

	[NonSerialized]
	public bool cascading;

	[NonSerialized]
	public bool selected;

	[NonSerialized]
	public int hierarchy_level;

	[NonSerialized]
	public RDPlanetListItemContainer parent;

	public UIRadioButton background;

	public TextMeshProUGUI label_planetName;

	public LayoutElement layoutElement;

	public PointerEnterExitHandler hoverController;

	public List<RDPlanetListItemContainer> children = new List<RDPlanetListItemContainer>();

	[NonSerialized]
	public new string name;

	[NonSerialized]
	public string displayName;

	public float scale_original;

	public float scale_popped;

	public bool over;

	public bool isRotating;

	public float rotationStartTime;

	public float rotationTimeOffset;

	public UIList list;

	[NonSerialized]
	public Camera thumbnailCamera;

	[NonSerialized]
	public RenderTexture thumbnailRenderTexture;

	public float thumbnailCameraSize = 30f;

	public LayerMask thumbnailCameraMask;

	public int thumbnailSize = 128;

	public int renderTextureDepth = 24;

	public void OnDestroy()
	{
		background.onTrue.RemoveListener(OnTrue);
		background.onFalse.RemoveListener(OnFalse);
		hoverController.onPointerEnter.RemoveListener(OnPointerEnter);
		hoverController.onPointerExit.RemoveListener(OnPointerExit);
		UnityEngine.Object.Destroy(base.gameObject);
		int i = 0;
		for (int count = children.Count; i < count; i++)
		{
			UnityEngine.Object.Destroy(children[i]);
		}
	}

	public void Update()
	{
		if (isRotating)
		{
			RotatePlanet();
		}
	}

	public void SetLightDirection()
	{
		planet.GetComponent<Renderer>().material.SetVector("_localLightDirection", planet.transform.InverseTransformDirection(Vector3.forward).normalized);
	}

	public void Setup(string bodyName, string displayName, GameObject planet, bool cascading, float offset_pos, float scale, float container_height, int hierarchy_level, UIList list)
	{
		this.list = list;
		float magnitude = planet.GetComponent<MeshFilter>().mesh.bounds.size.magnitude;
		this.cascading = cascading;
		this.planet = planet;
		SetLightDirection();
		planet.GetComponent<Renderer>().material.SetFloat("_rimPower", 3f);
		planet.GetComponent<Renderer>().material.SetFloat("_rimBlend", 0.3f);
		name = bodyName;
		this.displayName = Localizer.Format("#autoLOC_7001301", displayName);
		scale_original = 80f / magnitude * scale;
		scale_popped = scale_original * 1.1f;
		this.hierarchy_level = hierarchy_level;
		U5Util.SetLayerRecursive(planet.gameObject, LayerMask.NameToLayer("UIAdditional"));
		planet.transform.localScale = new Vector3(scale_original, scale_original, scale_original);
		planet.transform.SetParent(base.gameObject.transform);
		planet.transform.localPosition = new Vector3(0f, 0f, 1000f);
		planet.transform.rotation = Quaternion.identity;
		SunCoronas[] componentsInChildren = planet.gameObject.GetComponentsInChildren<SunCoronas>();
		int num = componentsInChildren.Length;
		for (int i = 0; i < num; i++)
		{
			SunCoronas obj = componentsInChildren[i];
			obj.transform.SetParent(planet.transform);
			UnityEngine.Object.DestroyImmediate(obj);
		}
		layoutElement.preferredHeight = container_height;
		label_planetName.text = this.displayName;
		background.onTrue.AddListener(OnTrue);
		background.onFalse.AddListener(OnFalse);
		hoverController.onPointerEnter.AddListener(OnPointerEnter);
		hoverController.onPointerExit.AddListener(OnPointerExit);
		CreateThumbnailCamera(bodyName, base.gameObject.transform, out thumbnailCamera, thumbnailCameraSize, thumbnailCameraMask, out thumbnailRenderTexture, thumbnailSize, renderTextureDepth);
	}

	public void CreateThumbnailCamera(string bodyName, Transform anchor, out Camera camRef, float camSize, LayerMask layerMask, out RenderTexture rtRef, int rtSize, int rtDepth)
	{
		camRef = new GameObject("planetCam_" + bodyName).AddComponent<Camera>();
		camRef.orthographic = true;
		camRef.orthographicSize = camSize;
		camRef.cullingMask = layerMask.value;
		camRef.farClipPlane = 295f;
		camRef.clearFlags = CameraClearFlags.Color;
		camRef.backgroundColor = Color.clear;
		camRef.transform.SetParent(anchor);
		camRef.transform.localPosition = new Vector3(0f, 0f, 900f);
		rtRef = new RenderTexture(rtSize, rtSize, rtDepth, RenderTextureFormat.ARGB32);
		camRef.targetTexture = rtRef;
		planetRawImage.texture = rtRef;
	}

	public void SetSelectionCallback(SelectionCallback del)
	{
		selectionCallback = del;
	}

	public void OnTrue(PointerEventData data, UIRadioButton.CallType callType)
	{
		selected = true;
		selectionCallback(this, selected: true);
		StartRotation();
	}

	public void OnFalse(PointerEventData data, UIRadioButton.CallType callType)
	{
		if (callType == UIRadioButton.CallType.USER)
		{
			selected = false;
			selectionCallback(this, selected: false);
			StopRotation();
		}
	}

	public void OnPointerEnter(PointerEventData data)
	{
		if (!over)
		{
			PopOutPlanet();
			over = true;
			StartRotation();
		}
	}

	public void OnPointerExit(PointerEventData data)
	{
		if (over)
		{
			PopInPlanet();
			over = false;
			StopRotation();
		}
	}

	public void StartRotation()
	{
		if (!isRotating && !planet.name.StartsWith("Sun"))
		{
			rotationStartTime = Time.realtimeSinceStartup;
			isRotating = true;
		}
	}

	public void StopRotation()
	{
		if (isRotating)
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			rotationTimeOffset += realtimeSinceStartup - rotationStartTime;
			rotationStartTime = realtimeSinceStartup;
			RotatePlanet();
			isRotating = false;
		}
	}

	public void RotatePlanet()
	{
		SetLightDirection();
		planet.transform.rotation = Quaternion.identity;
		planet.transform.Rotate(Vector3.up, 75f * (Time.realtimeSinceStartup - rotationStartTime + rotationTimeOffset));
	}

	public void PopOutPlanet()
	{
		planet.transform.localScale = new Vector3(scale_popped, scale_popped, scale_popped);
	}

	public void PopInPlanet()
	{
		planet.transform.localScale = new Vector3(scale_original, scale_original, scale_original);
	}

	public void AddChild(RDPlanetListItemContainer child)
	{
		children.Add(child);
		child.parent = this;
	}

	public RDPlanetListItemContainer GetParentInHierarchy(int hierarchy_level)
	{
		if (parent == null)
		{
			return null;
		}
		if (this.hierarchy_level == hierarchy_level)
		{
			return this;
		}
		return parent.GetParentInHierarchy(hierarchy_level);
	}

	public void HideChildren()
	{
		int count = children.Count;
		for (int i = 0; i < count; i++)
		{
			children[i].Hide();
		}
	}

	public void ShowChildren()
	{
		int num = list.GetIndex(GetComponent<UIListItem>());
		int count = children.Count;
		for (int i = 0; i < count; i++)
		{
			children[i].Show(++num);
		}
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	public void Show(int index)
	{
		base.gameObject.SetActive(value: true);
	}
}
