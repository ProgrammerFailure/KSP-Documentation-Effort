using UnityEngine;

public class GasGiantMaterialControls : MonoBehaviour
{
	public float cachedBandMovementSpeed;

	public float cachedSwirlRotationSpeed;

	public float cachedSwirlPanSpeed;

	public MeshRenderer meshRenderer;

	public bool appropriateMaterialAssignedToMesh = true;

	public double timeDivisorX = 20.0;

	public double timeDivisorY = 20.0;

	public double timeDivisorZ = 20.0;

	public Texture2D MovementControlTexture
	{
		set
		{
			meshRenderer.material.SetTexture("_MovementTexture", value);
		}
	}

	public Texture2D SwirlControlTexture
	{
		set
		{
			meshRenderer.material.SetTexture("_SwirlRotationControlTexture", value);
		}
	}

	public Texture2D ColourMap
	{
		set
		{
			meshRenderer.material.SetTexture("_CloudColorMap", value);
		}
	}

	public Texture2D ColourMap2
	{
		set
		{
			meshRenderer.material.SetTexture("_CloudColorMap2", value);
		}
	}

	public Texture2D DetailColourMap
	{
		set
		{
			meshRenderer.material.SetTexture("_DetailCloudColorMap", value);
		}
	}

	public Texture2D CloudPatternMap
	{
		set
		{
			meshRenderer.material.SetTexture("_CloudPatternTexture", value);
		}
	}

	public Texture2D NormalMap
	{
		set
		{
			meshRenderer.material.SetTexture("_NormalMap", value);
		}
	}

	public Texture2D DetailCloudPatternMap
	{
		set
		{
			meshRenderer.material.SetTexture("_DetailCloudPatternTexture", value);
		}
	}

	public Texture2D DetailNormalMap
	{
		set
		{
			meshRenderer.material.SetTexture("_DetailNormalMap", value);
		}
	}

	public float DetailTiling
	{
		get
		{
			return meshRenderer.material.GetFloat("_DetailTiling");
		}
		set
		{
			meshRenderer.material.SetFloat("_DetailTiling", value);
		}
	}

	public float NearDetailDistance
	{
		get
		{
			return meshRenderer.material.GetFloat("_NearDistanceForDetail");
		}
		set
		{
			meshRenderer.material.SetFloat("_NearDistanceForDetail", value);
		}
	}

	public float FarDetailDistance
	{
		get
		{
			return meshRenderer.material.GetFloat("_FarDistanceForDetail");
		}
		set
		{
			meshRenderer.material.SetFloat("_FarDistanceForDetail", value);
		}
	}

	public float NearDetailStrength
	{
		get
		{
			return meshRenderer.material.GetFloat("_NearDetail");
		}
		set
		{
			meshRenderer.material.SetFloat("_NearDetail", value);
		}
	}

	public float FarDetailStrength
	{
		get
		{
			return meshRenderer.material.GetFloat("_FarDetail");
		}
		set
		{
			meshRenderer.material.SetFloat("_FarDetail", value);
		}
	}

	public float BandMovementSpeed
	{
		get
		{
			return meshRenderer.material.GetFloat("_BandMovementSpeed");
		}
		set
		{
			meshRenderer.material.SetFloat("_BandMovementSpeed", Mathf.Clamp(value, -10f, 10f));
			UpdateTimeDivisors();
		}
	}

	public float SwirlRotationSpeed
	{
		get
		{
			return meshRenderer.material.GetFloat("_SwirlRotationSpeed");
		}
		set
		{
			cachedSwirlRotationSpeed = Mathf.Clamp(value, 0f, 10f);
			meshRenderer.material.SetFloat("_SwirlRotationSpeed", cachedSwirlRotationSpeed);
			UpdateTimeDivisors();
		}
	}

	public float SwirlRotationSwirliness
	{
		get
		{
			return meshRenderer.material.GetFloat("_SwirlRotationSwirliness");
		}
		set
		{
			meshRenderer.material.SetFloat("_SwirlRotationSwirliness", Mathf.Clamp(value, -10f, 10f));
		}
	}

	public float SwirlPanSpeed
	{
		get
		{
			return meshRenderer.material.GetFloat("_SwirlPanSpeed");
		}
		set
		{
			cachedSwirlPanSpeed = Mathf.Clamp(value, -10f, 10f);
			meshRenderer.material.SetFloat("_SwirlPanSpeed", cachedSwirlPanSpeed);
			UpdateTimeDivisors();
		}
	}

	public void Awake()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		if (meshRenderer == null)
		{
			Debug.Log("[GasGiantMaterialControls] Mesh Renderer not found. Destroying Self!!");
			Object.Destroy(this);
		}
	}

	public void OnEnable()
	{
		UpdateMaterialControlSettings();
	}

	public void UpdateMaterialControlSettings()
	{
		appropriateMaterialAssignedToMesh = meshRenderer.material.shader.name == "Terrain/Gas Giant";
		if (appropriateMaterialAssignedToMesh)
		{
			cachedBandMovementSpeed = BandMovementSpeed;
			cachedSwirlRotationSpeed = SwirlRotationSpeed;
			cachedSwirlPanSpeed = SwirlPanSpeed;
			UpdateTimeDivisors();
		}
	}

	public void UpdateTimeDivisors()
	{
		timeDivisorX = 5100f / Mathf.Abs(cachedBandMovementSpeed);
		timeDivisorY = 5100f / Mathf.Abs(cachedSwirlPanSpeed);
		timeDivisorZ = 1f / cachedSwirlRotationSpeed;
	}

	public void Update()
	{
		if (appropriateMaterialAssignedToMesh)
		{
			double universalTime = Planetarium.GetUniversalTime();
			float x = (float)(universalTime % timeDivisorX / 20.0);
			float y = (float)(universalTime % timeDivisorY / 20.0);
			float z = (float)(universalTime % timeDivisorZ);
			meshRenderer.material.SetVector("_GasGiantTime", new Vector4(x, y, z, 0f));
		}
	}
}
