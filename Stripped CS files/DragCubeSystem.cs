using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public class DragCubeSystem : MonoBehaviour
{
	[SerializeField]
	public int resolution = 512;

	[SerializeField]
	public AnimationCurve dragCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

	[SerializeField]
	public string cameraLayer = "DragRender";

	[SerializeField]
	public float proceduralDragUpdateInterval = 1f;

	[SerializeField]
	public Shader dragShader;

	[SerializeField]
	public Shader dragShaderBumped;

	[SerializeField]
	public bool debugOutputTextures;

	[SerializeField]
	public string debugHaltOnPartName = "";

	[SerializeField]
	public string debugHaltOnPartCubeName = "Default";

	[SerializeField]
	public GameObject testPart;

	public Camera aeroCamera;

	public float aeroCameraSize;

	public int cameraLayerInt;

	public RenderTexture renderTexture;

	public Texture2D aeroTexture;

	public static float aeroCameraOffset = 0.1f;

	public static float aeroCameraDoubleOffset = aeroCameraOffset * 2f;

	public Coroutine testRoutine;

	[SerializeField]
	public Part testPartInstance;

	public static int debugProceduralTextureID = 0;

	[SerializeField]
	public bool debugProcedural;

	public static DragCubeSystem Instance { get; set; }

	public float ProceduralDragUpdateInterval
	{
		get
		{
			return proceduralDragUpdateInterval;
		}
		set
		{
			proceduralDragUpdateInterval = value;
		}
	}

	public void Awake()
	{
		Instance = this;
		CreateAeroTextures();
		CreateAeroCamera();
		if (debugOutputTextures && !Directory.Exists("DragTextures"))
		{
			Directory.CreateDirectory("DragTextures");
		}
		dragShaderBumped = Shader.Find("Aerodynamics/DragRender");
		dragShader = Shader.Find("Aerodynamics/DragRenderNoBump");
	}

	public void OnDestroy()
	{
		if (renderTexture != null)
		{
			renderTexture.DiscardContents();
			Object.Destroy(renderTexture);
		}
		Object.Destroy(aeroTexture);
		if (aeroCamera != null)
		{
			Object.Destroy(aeroCamera.gameObject);
		}
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void CreateAeroCamera()
	{
		GameObject gameObject = new GameObject("AeroCamera");
		aeroCamera = gameObject.AddComponent<Camera>();
		aeroCamera.clearFlags = CameraClearFlags.Color;
		aeroCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
		aeroCamera.orthographic = true;
		aeroCamera.orthographicSize = 1f;
		aeroCamera.aspect = 1f;
		aeroCamera.nearClipPlane = 0.1f;
		aeroCamera.farClipPlane = 50f;
		aeroCamera.targetTexture = renderTexture;
		aeroCamera.enabled = false;
		aeroCamera.allowHDR = false;
		cameraLayerInt = LayerMask.NameToLayer(cameraLayer);
		aeroCamera.cullingMask = 1 << cameraLayerInt;
		aeroCamera.transform.parent = base.transform;
	}

	public void CreateAeroTextures()
	{
		renderTexture = new RenderTexture(resolution, resolution, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
		renderTexture.Create();
		aeroTexture = new Texture2D(resolution, resolution, TextureFormat.ARGB32, mipChain: false);
	}

	[ContextMenu("Test Render")]
	public void TestRender()
	{
		if (!Directory.Exists("DragTextures"))
		{
			Directory.CreateDirectory("DragTextures");
		}
		if (debugOutputTextures && testPart != null && testRoutine == null)
		{
			testPartInstance = testPart.GetComponent<Part>();
			if (testPartInstance != null)
			{
				SetupPartForRender(testPartInstance, testPart.gameObject);
				ConfigNode node = new ConfigNode("DRAG_CUBE_TEST");
				List<ModuleDragModifier> list = new List<ModuleDragModifier>();
				List<ModuleDragAreaModifier> list2 = new List<ModuleDragAreaModifier>();
				testRoutine = StartCoroutine(RenderDragCubes(testPart.gameObject, testPartInstance, node, null, null, null, list.ToArray(), list2.ToArray(), destroyObject: false));
			}
		}
	}

	public IEnumerator RenderDragCubesCoroutine(Part p, ConfigNode dragConfig)
	{
		bool flag = false;
		ModuleJettison moduleJettison = null;
		ModulePartVariants modulePartVariants = null;
		IMultipleDragCube moduleJettison2 = null;
		List<IMultipleDragCube> list = new List<IMultipleDragCube>();
		List<ModuleDragModifier> list2 = new List<ModuleDragModifier>();
		List<ModuleDragAreaModifier> list3 = new List<ModuleDragAreaModifier>();
		Part part = Object.Instantiate(p, Vector3.zero, Quaternion.identity);
		GameObject gameObject = part.gameObject;
		SetupPartForRender(part, gameObject);
		PartModule[] componentsInChildren = gameObject.GetComponentsInChildren<PartModule>(includeInactive: true);
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			if (!(componentsInChildren[i] is IMultipleDragCube { IsMultipleCubesActive: not false } multipleDragCube))
			{
				continue;
			}
			if (moduleJettison == null)
			{
				moduleJettison = componentsInChildren[i] as ModuleJettison;
				if (moduleJettison != null)
				{
					moduleJettison2 = multipleDragCube;
				}
			}
			if (modulePartVariants == null)
			{
				modulePartVariants = componentsInChildren[i] as ModulePartVariants;
			}
			if (!multipleDragCube.UsesProceduralDragCubes())
			{
				list.Add(multipleDragCube);
				continue;
			}
			flag = true;
			list = null;
			break;
		}
		if (!flag && list != null && list.Count > 2)
		{
			Debug.LogWarning("DragCubeSystem: Part '" + p.partInfo.name + "' has more than two IMultipleDragCube part modules. You should consider procedural drag cubes.");
		}
		int j = 0;
		for (int num2 = componentsInChildren.Length; j < num2; j++)
		{
			PartModule partModule = componentsInChildren[j];
			if (partModule is ModuleDragModifier)
			{
				list2.Add(partModule as ModuleDragModifier);
			}
			else if (partModule is ModuleDragAreaModifier)
			{
				list3.Add(partModule as ModuleDragAreaModifier);
			}
		}
		if (flag)
		{
			Debug.Log("DragCubeSystem: Part '" + p.partInfo.name + "' has defined a procedural drag cube setup");
			dragConfig.AddValue("procedural", "True");
			Object.DestroyImmediate(gameObject);
			yield break;
		}
		if (modulePartVariants != null && moduleJettison != null)
		{
			modulePartVariants.moduleJettison = moduleJettison;
			part.variants = modulePartVariants;
			moduleJettison.SetVariants();
		}
		Debug.Log("DragCubeSystem: Creating drag cubes for part '" + p.partInfo.name + "'");
		yield return StartCoroutine(RenderDragCubes(gameObject, p, dragConfig, list, moduleJettison2, modulePartVariants, list2.ToArray(), list3.ToArray()));
	}

	public float GetCdModifier(string name, ModuleDragModifier[] modifiers)
	{
		if (modifiers != null && modifiers.Length != 0)
		{
			int num = 0;
			int num2 = modifiers.Length;
			while (true)
			{
				if (num < num2)
				{
					if (modifiers[num].dragCubeName == name)
					{
						break;
					}
					num++;
					continue;
				}
				return 1f;
			}
			return modifiers[num].dragModifier;
		}
		return 1f;
	}

	public float GetAreaModifier(string name, ModuleDragAreaModifier[] modifiers)
	{
		if (modifiers != null && modifiers.Length != 0)
		{
			int num = 0;
			int num2 = modifiers.Length;
			while (true)
			{
				if (num < num2)
				{
					if (modifiers[num].dragCubeName == name)
					{
						break;
					}
					num++;
					continue;
				}
				return 1f;
			}
			return modifiers[num].areaModifier;
		}
		return 1f;
	}

	public IEnumerator RenderDragCubes(GameObject partObject, Part partPrefab, ConfigNode node, List<IMultipleDragCube> multipleInterfaces, IMultipleDragCube moduleJettison, ModulePartVariants modulePartVariants, ModuleDragModifier[] dragModifiers, ModuleDragAreaModifier[] areaModifiers, bool destroyObject = true)
	{
		string[] positionNames = null;
		int interfaceCount = 1;
		if (multipleInterfaces != null)
		{
			if (multipleInterfaces.Count > 0)
			{
				interfaceCount = multipleInterfaces.Count;
			}
			else
			{
				multipleInterfaces = null;
			}
		}
		for (int interfaceIdx = 0; interfaceIdx < interfaceCount; interfaceIdx++)
		{
			if (multipleInterfaces != null)
			{
				positionNames = multipleInterfaces[interfaceIdx].GetDragCubeNames();
				if (multipleInterfaces[interfaceIdx] == moduleJettison && modulePartVariants != null && partPrefab.baseVariant != null)
				{
					modulePartVariants.SetVariant(partPrefab.baseVariant.Name);
				}
			}
			int positionCount;
			if (positionNames != null)
			{
				positionCount = positionNames.Length;
			}
			else
			{
				positionCount = 1;
				positionNames = new string[1] { "Default" };
			}
			int j = 0;
			while (j < positionCount)
			{
				DragCube cube = new DragCube
				{
					Name = positionNames[j]
				};
				multipleInterfaces?[interfaceIdx].AssumeDragCubePosition(positionNames[j]);
				yield return null;
				Bounds partBounds = CalculatePartBounds(partObject);
				int num;
				for (int i = 0; i < 6; i = num)
				{
					SetAeroCamera((DragCube.DragFace)i, partBounds);
					UpdateAeroTexture();
					if (debugOutputTextures)
					{
						byte[] bytes = aeroTexture.EncodeToPNG();
						string[] obj = new string[7]
						{
							"DragTextures/",
							partPrefab.partInfo.name,
							"_",
							positionNames[j],
							"_",
							null,
							null
						};
						DragCube.DragFace dragFace = (DragCube.DragFace)i;
						obj[5] = dragFace.ToString();
						obj[6] = ".png";
						File.WriteAllBytes(string.Concat(obj), bytes);
					}
					float area = 0f;
					float drag = 0f;
					float depth = 0f;
					CalculateAerodynamics(aeroCamera, out area, out drag, out depth);
					cube.Area[i] = area * GetAreaModifier(positionNames[j], areaModifiers);
					cube.Drag[i] = drag * GetCdModifier(positionNames[j], dragModifiers);
					cube.Depth[i] = depth;
					if (debugHaltOnPartName == partPrefab.partInfo.name && (debugHaltOnPartCubeName == positionNames[j] || string.IsNullOrEmpty(debugHaltOnPartCubeName)))
					{
						string[] obj2 = new string[6] { "DragCubeSystem: Pausing at ", debugHaltOnPartName, " ", debugHaltOnPartCubeName, " ", null };
						DragCube.DragFace dragFace = (DragCube.DragFace)i;
						obj2[5] = dragFace.ToString();
						Debug.LogError(string.Concat(obj2));
						Debug.Break();
						Debug.DebugBreak();
						yield return null;
					}
					num = i + 1;
				}
				cube.Center = partBounds.center;
				cube.Size = partBounds.size;
				partPrefab.DragCubes.Cubes.Add(cube);
				node.AddValue("cube", cube.SaveToString());
				num = j + 1;
				j = num;
			}
		}
		if (destroyObject)
		{
			CleanUpPartMaterials(partObject);
			partObject.gameObject.transform.position = new Vector3(0f, 1000f, 0f);
			partObject.gameObject.SetActive(value: false);
			Object.DestroyImmediate(partObject.gameObject);
		}
		yield return null;
	}

	public Bounds CalculatePartBounds(GameObject part)
	{
		Bounds result = default(Bounds);
		bool flag = false;
		Renderer[] componentsInChildren = part.gameObject.GetComponentsInChildren<Renderer>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			if (!(componentsInChildren[i] as ParticleSystemRenderer != null))
			{
				if (flag)
				{
					result.Encapsulate(componentsInChildren[i].bounds);
					continue;
				}
				result = componentsInChildren[i].bounds;
				flag = true;
			}
		}
		Part component = part.GetComponent<Part>();
		if (component != null)
		{
			result.size *= component.boundsMultiplier;
		}
		return result;
	}

	public void SetupPartForRender(Part part, GameObject partObject)
	{
		part.enabled = false;
		part.SetMirror(Vector3.one);
		partObject.name += " Drag Rendering Clone";
		partObject.SetActive(value: true);
		for (int i = 0; i < part.children.Count; i++)
		{
			if (part.children[i].partTransform.parent == part.partTransform)
			{
				Object.DestroyImmediate(part.children[i].gameObject);
			}
		}
		MonoBehaviour[] componentsInChildren = partObject.GetComponentsInChildren<MonoBehaviour>(includeInactive: true);
		int j = 0;
		for (int num = componentsInChildren.Length; j < num; j++)
		{
			componentsInChildren[j].enabled = false;
		}
		Collider[] componentsInChildren2 = partObject.GetComponentsInChildren<Collider>(includeInactive: true);
		int k = 0;
		for (int num2 = componentsInChildren2.Length; k < num2; k++)
		{
			componentsInChildren2[k].enabled = false;
		}
		partObject.gameObject.SetLayerRecursive(cameraLayerInt, 2);
		Renderer[] componentsInChildren3 = partObject.GetComponentsInChildren<Renderer>();
		int num3 = componentsInChildren3.Length;
		while (num3-- > 0)
		{
			if (!(componentsInChildren3[num3] == null) && componentsInChildren3[num3].gameObject.CompareTag("Drag_Hidden"))
			{
				Object.DestroyImmediate(componentsInChildren3[num3]);
			}
		}
		Renderer[] componentsInChildren4 = partObject.GetComponentsInChildren<Renderer>();
		int l = 0;
		for (int num4 = componentsInChildren4.Length; l < num4; l++)
		{
			Renderer renderer = componentsInChildren4[l];
			renderer.shadowCastingMode = ShadowCastingMode.Off;
			renderer.receiveShadows = false;
			Material[] materials = renderer.materials;
			int m = 0;
			for (int num5 = materials.Length; m < num5; m++)
			{
				Material material = materials[m];
				if (!(material == null))
				{
					Material material2 = null;
					material2 = ((!material.HasProperty("_BumpMap")) ? new Material(dragShader) : new Material(dragShaderBumped));
					if (SetMainTexture(material) && material.HasProperty("_MainTex"))
					{
						material2.SetTexture("_MainTex", material.GetTexture("_MainTex"));
						material2.SetTextureOffset("_MainTex", material.GetTextureOffset("_MainTex"));
						material2.SetTextureScale("_MainTex", material.GetTextureScale("_MainTex"));
					}
					if (material.HasProperty("_BumpMap"))
					{
						material2.SetTexture("_BumpMap", material.GetTexture("_BumpMap"));
						material2.SetTextureOffset("_BumpMap", material.GetTextureOffset("_BumpMap"));
						material2.SetTextureScale("_BumpMap", material.GetTextureScale("_BumpMap"));
					}
					materials[m] = material2;
				}
			}
			renderer.materials = materials;
		}
		ParticleSystem[] componentsInChildren5 = partObject.GetComponentsInChildren<ParticleSystem>();
		int n = 0;
		for (int num6 = componentsInChildren5.Length; n < num6; n++)
		{
			FXPrefab component = componentsInChildren5[n].GetComponent<FXPrefab>();
			if (component != null)
			{
				Object.DestroyImmediate(component);
			}
			Object.DestroyImmediate(componentsInChildren5[n]);
		}
	}

	public bool SetMainTexture(Material mat)
	{
		return mat.shader.name.Contains("Alpha");
	}

	public void CleanUpPartMaterials(GameObject part)
	{
		MeshRenderer[] componentsInChildren = part.GetComponentsInChildren<MeshRenderer>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			Material[] sharedMaterials = componentsInChildren[i].sharedMaterials;
			int j = 0;
			for (int num2 = sharedMaterials.Length; j < num2; j++)
			{
				if (!(sharedMaterials[j] == null))
				{
					Object.Destroy(sharedMaterials[j]);
				}
			}
		}
	}

	public float SetAeroCamera(DragCube.DragFace direction, Bounds partBounds)
	{
		float planarSize = GetPlanarSize(direction, partBounds);
		float num = GetPlanarDepth(direction, partBounds) + aeroCameraDoubleOffset;
		Vector3 planarDirection = GetPlanarDirection(direction);
		aeroCamera.transform.position = GetPlanarCenter(direction, partBounds);
		aeroCamera.transform.rotation = Quaternion.LookRotation(planarDirection);
		aeroCameraSize = planarSize / 2f;
		aeroCamera.orthographicSize = aeroCameraSize;
		aeroCamera.nearClipPlane = 0f;
		aeroCamera.farClipPlane = num;
		return num;
	}

	public float GetPlanarSize(DragCube.DragFace direction, Bounds partBounds)
	{
		switch (direction)
		{
		case DragCube.DragFace.const_0:
		case DragCube.DragFace.const_1:
			return Mathf.Max(partBounds.size.z, partBounds.size.y);
		case DragCube.DragFace.const_2:
		case DragCube.DragFace.const_3:
			return Mathf.Max(partBounds.size.x, partBounds.size.z);
		default:
			return Mathf.Max(partBounds.size.x, partBounds.size.y);
		}
	}

	public float GetPlanarDepth(DragCube.DragFace direction, Bounds partBounds)
	{
		return direction switch
		{
			DragCube.DragFace.const_0 => partBounds.size.x, 
			DragCube.DragFace.const_1 => partBounds.size.x, 
			DragCube.DragFace.const_2 => partBounds.size.y, 
			DragCube.DragFace.const_3 => partBounds.size.y, 
			DragCube.DragFace.const_4 => partBounds.size.z, 
			_ => partBounds.size.z, 
		};
	}

	public Vector3 GetPlanarCenter(DragCube.DragFace direction, Bounds partBounds)
	{
		Vector3 zero = Vector3.zero;
		return direction switch
		{
			DragCube.DragFace.const_0 => new Vector3(partBounds.max.x + aeroCameraOffset, partBounds.center.y, partBounds.center.z), 
			DragCube.DragFace.const_1 => new Vector3(partBounds.min.x - aeroCameraOffset, partBounds.center.y, partBounds.center.z), 
			DragCube.DragFace.const_2 => new Vector3(partBounds.center.x, partBounds.max.y + aeroCameraOffset, partBounds.center.z), 
			DragCube.DragFace.const_3 => new Vector3(partBounds.center.x, partBounds.min.y - aeroCameraOffset, partBounds.center.z), 
			DragCube.DragFace.const_5 => new Vector3(partBounds.center.x, partBounds.center.y, partBounds.min.z - aeroCameraOffset), 
			_ => new Vector3(partBounds.center.x, partBounds.center.y, partBounds.max.z + aeroCameraOffset), 
		};
	}

	public Vector3 GetPlanarDirection(DragCube.DragFace direction)
	{
		return direction switch
		{
			DragCube.DragFace.const_0 => Vector3.left, 
			DragCube.DragFace.const_1 => Vector3.right, 
			DragCube.DragFace.const_2 => Vector3.down, 
			DragCube.DragFace.const_3 => Vector3.up, 
			DragCube.DragFace.const_5 => Vector3.forward, 
			_ => Vector3.back, 
		};
	}

	public void UpdateAeroTexture()
	{
		aeroCamera.Render();
		RenderTexture.active = renderTexture;
		aeroTexture.ReadPixels(new Rect(0f, 0f, resolution, resolution), 0, 0);
		aeroTexture.Apply();
		RenderTexture.active = null;
	}

	public void CalculateAerodynamics(Camera aeroCamera, out float area, out float drag, out float depth)
	{
		Color32[] pixels = aeroTexture.GetPixels32();
		float num = Mathf.Pow(2f * aeroCameraSize / (float)resolution, 2f);
		int num2 = 0;
		drag = 0f;
		area = 0f;
		depth = 0f;
		int i = 0;
		for (int num3 = pixels.Length; i < num3; i++)
		{
			Color32 color = pixels[i];
			if (color.a != 0)
			{
				if (color.r > 0)
				{
					float time = (float)(int)color.r / 255f;
					drag += dragCurve.Evaluate(time);
				}
				if (color.g > 0)
				{
					float b = Mathf.Lerp(aeroCamera.nearClipPlane, aeroCamera.farClipPlane, (float)(int)color.g / 255f);
					depth = Mathf.Max(depth, b);
				}
				area += num;
				num2++;
			}
		}
		if (num2 > 0)
		{
			drag /= num2;
		}
	}

	public IEnumerator SetupDragCubeCoroutine(Part p)
	{
		ConfigNode dragConfig2 = PartLoader.Instance.GetDatabaseConfig(p, "DRAG_CUBE");
		if (dragConfig2 == null || !p.DragCubes.LoadCubes(dragConfig2))
		{
			Debug.LogFormat("[DragCubeSystem]: Drag cubes not found or cannot be read for part {0}. Generating New drag cubes.", p.partName);
			dragConfig2 = new ConfigNode("DRAG_CUBE");
			yield return StartCoroutine(Instance.RenderDragCubesCoroutine(p, dragConfig2));
			if (dragConfig2 != null)
			{
				PartLoader.Instance.SetDatabaseConfig(p, dragConfig2);
			}
		}
	}

	public IEnumerator SetupDragCubeCoroutine(Part p, ConfigNode dragConfig)
	{
		if (dragConfig == null)
		{
			dragConfig = new ConfigNode("DRAG_CUBE");
		}
		yield return StartCoroutine(Instance.RenderDragCubesCoroutine(p, dragConfig));
	}

	public DragCube RenderProceduralDragCube(Part p)
	{
		if (debugProcedural)
		{
			Debug.Log("DragCubeSystem: Rendering procedural drag for " + p.partInfo.name);
		}
		DragCube dragCube = new DragCube();
		Part part = Object.Instantiate(p, Vector3.zero, Quaternion.identity);
		GameObject gameObject = part.gameObject;
		bool flag;
		if (flag = part.GetComponent<Vessel>() != null)
		{
			part.vessel.mapObject = null;
		}
		SetupPartForRender(part, gameObject);
		Bounds bounds = CalculatePartBounds(gameObject);
		if (debugProcedural)
		{
			Debug.LogError("Rendering drag cubes for  texture " + p.gameObject.name, p.gameObject);
			Debug.LogError(bounds);
		}
		for (int i = 0; i < 6; i++)
		{
			SetAeroCamera((DragCube.DragFace)i, bounds);
			UpdateAeroTexture();
			float area = 0f;
			float drag = 0f;
			float depth = 0f;
			if (debugProcedural)
			{
				byte[] bytes = aeroTexture.EncodeToPNG();
				string[] obj = new string[5]
				{
					"DragTexturesProc/DragTex",
					debugProceduralTextureID.ToString("D2"),
					"_",
					null,
					null
				};
				DragCube.DragFace dragFace = (DragCube.DragFace)i;
				obj[3] = dragFace.ToString();
				obj[4] = ".png";
				File.WriteAllBytes(string.Concat(obj), bytes);
			}
			CalculateAerodynamics(aeroCamera, out area, out drag, out depth);
			dragCube.Area[i] = area;
			dragCube.Drag[i] = drag;
			dragCube.Depth[i] = depth;
			if (debugProcedural)
			{
				object[] array = new object[5];
				DragCube.DragFace dragFace = (DragCube.DragFace)i;
				array[0] = dragFace.ToString();
				array[1] = " ";
				array[2] = area;
				array[3] = " ";
				array[4] = drag;
				Debug.LogError(string.Concat(array));
			}
		}
		dragCube.Center = bounds.center;
		dragCube.Size = bounds.size;
		if (debugProcedural)
		{
			if (debugProceduralTextureID > 10)
			{
				debugProceduralTextureID = 0;
			}
			debugProceduralTextureID++;
		}
		gameObject.SetActive(value: false);
		Object.Destroy(gameObject);
		if (flag)
		{
			FlightCamera.fetch.CycleCameraHighlighter();
		}
		return dragCube;
	}

	public void LoadDragCubes(Part p)
	{
		ConfigNode databaseConfig = PartLoader.Instance.GetDatabaseConfig(p, "DRAG_CUBE");
		if (databaseConfig != null)
		{
			p.DragCubes.LoadCubes(databaseConfig);
		}
		else if (PartLoader.getPartInfoByName(p.name) != null && PartLoader.getPartInfoByName(p.name).partPrefab.GetComponent<Part>() != null)
		{
			DragCubeList dragCubes = PartLoader.getPartInfoByName(p.name).partPrefab.GetComponent<Part>().DragCubes;
			p.DragCubes.LoadCubes(dragCubes);
		}
		else
		{
			Debug.LogError("DragCubeSystem: Can't find database config for " + p.partInfo.name);
		}
	}
}
