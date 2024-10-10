using System;
using System.IO;
using ns11;
using UnityEngine;

public static class CraftThumbnail
{
	public static float camFov = 30f;

	public static float camDist = 0f;

	public static Camera snapshotCamera;

	public static RenderTexture renderTexture;

	public static Texture2D thumbTexture;

	public static EventData<ShipConstruct, string, byte[]> OnSnapshotCapture = new EventData<ShipConstruct, string, byte[]>("OnSnapshotCapture");

	public static void CreateRenderTextures(int resolution)
	{
		if (renderTexture != null)
		{
			renderTexture.Release();
		}
		renderTexture = new RenderTexture(resolution, resolution, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
		renderTexture.Create();
		thumbTexture = new Texture2D(resolution, resolution, TextureFormat.ARGB32, mipChain: false);
	}

	public static void TakeStockSnaphot(ShipConstruct ship, int resolution, string facilityName, bool expansion, string craftPath, float elevation = 45f, float azimuth = 45f, float pitch = 45f, float hdg = 45f, float fovFactor = 1f)
	{
		string text = "";
		string relativePath = KSPUtil.GetRelativePath(craftPath, KSPUtil.ApplicationRootPath);
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(craftPath);
		string[] separator = new string[1] { "Ships/" };
		string[] array = relativePath.Split(separator, StringSplitOptions.None);
		text = ((!expansion || array.Length == 0) ? ("Ships/@thumbs/" + facilityName) : (array[0] + "Ships/@thumbs/" + facilityName));
		TakeSnaphot(ship, resolution, text, fileNameWithoutExtension, elevation, azimuth, pitch, hdg, fovFactor);
	}

	public static void TakeSnaphot(ShipConstruct ship, int resolution, string folderPath, string craftName, float elevation = 45f, float azimuth = 45f, float pitch = 45f, float hdg = 45f, float fovFactor = 1f)
	{
		GameObject gameObject = new GameObject("SnapshotCamera");
		snapshotCamera = gameObject.AddComponent<Camera>();
		snapshotCamera.clearFlags = CameraClearFlags.Color;
		snapshotCamera.backgroundColor = Color.clear;
		snapshotCamera.fieldOfView = camFov;
		snapshotCamera.cullingMask = 1;
		snapshotCamera.enabled = false;
		snapshotCamera.allowHDR = false;
		Light light = gameObject.AddComponent<Light>();
		light.renderingLayerMask = 1;
		light.type = LightType.Spot;
		light.range = 100f;
		light.intensity = 0.5f;
		camDist = KSPCameraUtil.GetDistanceToFit(ShipConstruction.CalculateCraftSize(ship), camFov * fovFactor);
		snapshotCamera.transform.position = ShipConstruction.FindCraftCenter(ship, excludeClamps: true) + Quaternion.AngleAxis(azimuth, Vector3.up) * Quaternion.AngleAxis(elevation, Vector3.right) * (Vector3.back * camDist);
		snapshotCamera.transform.rotation = Quaternion.AngleAxis(hdg, Vector3.up) * Quaternion.AngleAxis(pitch, Vector3.right);
		if (thumbTexture == null)
		{
			CreateRenderTextures(resolution);
		}
		thumbTexture = ShipConstruction.RenderCamera(snapshotCamera, resolution, resolution, 24, RenderTextureReadWrite.Default);
		byte[] array = thumbTexture.EncodeToPNG();
		string text = KSPUtil.ApplicationRootPath + folderPath;
		if (craftName.Contains("/"))
		{
			int num = craftName.LastIndexOf("/");
			text = text + "/" + craftName.Substring(0, num);
			craftName = craftName.Substring(num + 1);
		}
		craftName = KSPUtil.SanitizeString(craftName, '_', replaceEmpty: true);
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		if (Directory.Exists(text))
		{
			string text2 = text + "/" + craftName + ".png";
			try
			{
				File.WriteAllBytes(text2, array);
				OnSnapshotCapture.Fire(ship, text2, array);
			}
			catch (Exception ex)
			{
				Debug.LogError("[Thumbnail]: Error writing thumbnail with path " + text2 + ". Message: " + ex);
			}
		}
		else
		{
			Debug.LogError("[Thumbnail]: Error creating directory " + text);
		}
		UnityEngine.Object.Destroy(gameObject);
	}

	public static Texture2D TakePartSnaphot(string partName, StoredPart sPart, int resolution, string folderPath, out string fullFileName, float elevation = 15f, float azimuth = 25f, float pitch = 15f, float hdg = 25f, float fovFactor = 1f, int variantIndex = -1)
	{
		return TakePartSnapshot(partName, sPart, null, resolution, folderPath, out fullFileName, elevation, azimuth, pitch, hdg, fovFactor, variantIndex);
	}

	public static Texture2D TakePartSnapshot(string partName, StoredPart sPart, Part part, int resolution, string folderPath, out string fullFileName, float elevation = 15f, float azimuth = 25f, float pitch = 15f, float hdg = 25f, float fovFactor = 1f, int variantIndex = -1)
	{
		GameObject gameObject = new GameObject("SnapshotCamera");
		snapshotCamera = gameObject.AddComponent<Camera>();
		snapshotCamera.clearFlags = CameraClearFlags.Color;
		snapshotCamera.backgroundColor = Color.clear;
		snapshotCamera.fieldOfView = camFov;
		snapshotCamera.cullingMask = ((!HighLogic.LoadedSceneIsFlight) ? 1 : 536870912);
		snapshotCamera.enabled = false;
		snapshotCamera.orthographic = true;
		snapshotCamera.orthographicSize = 0.75f;
		snapshotCamera.allowHDR = false;
		Light light = gameObject.AddComponent<Light>();
		light.renderingLayerMask = ((!HighLogic.LoadedSceneIsFlight) ? 1 : 536870912);
		light.type = LightType.Spot;
		light.range = 100f;
		light.intensity = 0.5f;
		AvailablePart availablePart = null;
		ProtoPartSnapshot protoPartSnapshot = null;
		StoredPart val = null;
		if (UIPartActionControllerInventory.Instance.CurrentInventorySlotHovered != null)
		{
			UIPartActionControllerInventory.Instance.CurrentInventorySlotHovered.inventoryPartActionUI.inventoryPartModule.storedParts.TryGetValue(UIPartActionControllerInventory.Instance.CurrentInventorySlotHovered.slotIndex, out val);
			if (val != null)
			{
				protoPartSnapshot = val.snapshot;
				availablePart = protoPartSnapshot.partInfo;
			}
		}
		if (sPart != null)
		{
			if (sPart != null && val == null)
			{
				val = sPart;
				protoPartSnapshot = val.snapshot;
				availablePart = protoPartSnapshot.partInfo;
			}
			if (availablePart == null)
			{
				availablePart = PartLoader.getPartInfoByName(partName);
			}
		}
		else if (part != null)
		{
			availablePart = part.partInfo;
		}
		if (availablePart == null)
		{
			fullFileName = "";
			thumbTexture = null;
			return thumbTexture;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(availablePart.iconPrefab);
		gameObject2.SetActive(value: true);
		if (HighLogic.LoadedSceneIsFlight)
		{
			gameObject2.layer = 29;
			gameObject2.SetLayerRecursive(29);
		}
		Material[] array = EditorPartIcon.CreateMaterialArray(gameObject2, includeInactiveRenderers: true);
		IThumbnailSetup thumbNailSetupIface = GetThumbNailSetupIface(availablePart);
		if (variantIndex > -1)
		{
			ModulePartVariants.ApplyVariant(null, gameObject2.transform, availablePart.Variants[variantIndex], array, skipShader: false, variantIndex);
		}
		int num = array.Length;
		while (num-- > 0)
		{
			if (!array[num].shader.name.Contains("ScreenSpaceMask"))
			{
				if (array[num].shader.name == "KSP/Bumped Specular (Mapped)")
				{
					array[num].shader = Shader.Find("KSP/ScreenSpaceMaskSpecular");
				}
				else if (array[num].shader.name.Contains("Bumped"))
				{
					array[num].shader = Shader.Find("KSP/ScreenSpaceMaskBumped");
				}
				else if (array[num].shader.name.Contains("KSP/Alpha/CutoffBackground"))
				{
					array[num].shader = Shader.Find("KSP/ScreenSpaceMaskAlphaCutoffBackground");
				}
				else if (array[num].shader.name == "KSP/Unlit")
				{
					array[num].shader = Shader.Find("KSP/ScreenSpaceMaskUnlit");
				}
				else
				{
					array[num].shader = Shader.Find("KSP/ScreenSpaceMask");
				}
			}
			array[num].enableInstancing = false;
		}
		thumbNailSetupIface?.AssumeSnapshotPosition(gameObject2, protoPartSnapshot);
		Vector3 size = PartGeometryUtil.MergeBounds(PartGeometryUtil.GetPartRendererBounds(availablePart.partPrefab), availablePart.partPrefab.transform.root).size;
		camDist = KSPCameraUtil.GetDistanceToFit(Mathf.Max(Mathf.Max(size.x, size.y), size.z), camFov * fovFactor, resolution);
		snapshotCamera.transform.position = Quaternion.AngleAxis(azimuth, Vector3.up) * Quaternion.AngleAxis(elevation, Vector3.right) * (Vector3.back * camDist);
		snapshotCamera.transform.rotation = Quaternion.AngleAxis(hdg, Vector3.up) * Quaternion.AngleAxis(pitch, Vector3.right);
		gameObject2.transform.SetParent(snapshotCamera.transform);
		snapshotCamera.transform.Translate(0f, -1000f, -250f);
		if (thumbTexture == null)
		{
			CreateRenderTextures(resolution);
		}
		thumbTexture = ShipConstruction.RenderCamera(snapshotCamera, resolution, resolution, 24, RenderTextureReadWrite.Default);
		byte[] bytes = thumbTexture.EncodeToPNG();
		string text = KSPUtil.ApplicationRootPath + "GameData/" + folderPath;
		if (!text.Contains("Parts/@thumbs"))
		{
			text = KSPUtil.ApplicationRootPath + "@thumbs/Parts/";
		}
		string text2 = ((variantIndex > -1) ? variantIndex.ToString() : "");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		fullFileName = "";
		if (Directory.Exists(text))
		{
			fullFileName = text + partName + "_icon" + text2;
			if (thumbNailSetupIface != null && sPart != null)
			{
				fullFileName += thumbNailSetupIface.ThumbSuffix(sPart.snapshot);
			}
			try
			{
				File.WriteAllBytes(fullFileName + ".png", bytes);
			}
			catch (Exception ex)
			{
				Debug.LogError("[Thumbnail]: Error writing thumbnail with path " + fullFileName + ". Message: " + ex);
			}
		}
		else
		{
			Debug.LogError("[Thumbnail]: Error creating directory " + text);
		}
		UnityEngine.Object.Destroy(gameObject);
		UnityEngine.Object.Destroy(gameObject2);
		return thumbTexture;
	}

	public static string GetPartIconTexturePath(Part part, out int variantIdx)
	{
		variantIdx = -1;
		if (part == null)
		{
			return string.Empty;
		}
		ModulePartVariants modulePartVariants = part.FindModuleImplementing<ModulePartVariants>();
		if (modulePartVariants != null)
		{
			variantIdx = modulePartVariants.GetCurrentVariantIndex();
		}
		string text = ((variantIdx > -1) ? variantIdx.ToString() : "");
		int num = part.partInfo.partUrl.LastIndexOf("Parts/");
		string text2 = "";
		text2 = ((num <= 0) ? (KSPUtil.ApplicationRootPath + "@thumbs/Parts/" + part.partInfo.name + "_icon" + text) : (part.partInfo.partUrl.Substring(0, part.partInfo.partUrl.LastIndexOf("Parts/") + 6) + "@thumbs/" + part.partInfo.name + "_icon" + text));
		IThumbnailSetup thumbNailSetupIface = GetThumbNailSetupIface(part.partInfo);
		if (thumbNailSetupIface != null && part.protoPartSnapshot != null)
		{
			text2 += thumbNailSetupIface.ThumbSuffix(part.protoPartSnapshot);
		}
		return text2;
	}

	public static IThumbnailSetup GetThumbNailSetupIface(AvailablePart availablePart)
	{
		IThumbnailSetup result = null;
		int count = availablePart.partPrefab.Modules.Count;
		while (count-- > 0)
		{
			object obj = availablePart.partPrefab.Modules[count];
			Type[] interfaces = obj.GetType().GetInterfaces();
			for (int i = 0; i < interfaces.Length; i++)
			{
				if (interfaces[i] == typeof(IThumbnailSetup))
				{
					result = obj as IThumbnailSetup;
					break;
				}
			}
		}
		return result;
	}
}
