using UnityEngine;

[AddComponentMenu("Detonator/Detonator")]
public class Detonator : MonoBehaviour
{
	public static float _baseSize = 30f;

	public static Color _baseColor = new Color(1f, 0.423f, 0f, 0.5f);

	public static float _baseDuration = 3f;

	public float size = 10f;

	public Color color = _baseColor;

	public bool explodeOnStart = true;

	public float duration = _baseDuration;

	public float detail = 1f;

	public float upwardsBias;

	public float destroyTime = 7f;

	public bool useWorldSpace = true;

	public Vector3 direction = Vector3.zero;

	public Material fireballAMaterial;

	public Material fireballBMaterial;

	public Material smokeAMaterial;

	public Material smokeBMaterial;

	public Material shockwaveMaterial;

	public Material sparksMaterial;

	public Material glowMaterial;

	public Material heatwaveMaterial;

	public Component[] components;

	public DetonatorFireball _fireball;

	public DetonatorSparks _sparks;

	public DetonatorShockwave _shockwave;

	public DetonatorSmoke _smoke;

	public DetonatorGlow _glow;

	public DetonatorLight _light;

	public DetonatorForce _force;

	public DetonatorHeatwave _heatwave;

	public bool autoCreateFireball = true;

	public bool autoCreateSparks = true;

	public bool autoCreateShockwave = true;

	public bool autoCreateSmoke = true;

	public bool autoCreateGlow = true;

	public bool autoCreateLight = true;

	public bool autoCreateForce = true;

	public bool autoCreateHeatwave;

	public float _lastExplosionTime = 1000f;

	public bool _firstComponentUpdate = true;

	public Component[] _subDetonators;

	public static Material defaultFireballAMaterial;

	public static Material defaultFireballBMaterial;

	public static Material defaultSmokeAMaterial;

	public static Material defaultSmokeBMaterial;

	public static Material defaultShockwaveMaterial;

	public static Material defaultSparksMaterial;

	public static Material defaultGlowMaterial;

	public static Material defaultHeatwaveMaterial;

	public void Awake()
	{
		FillDefaultMaterials();
		components = GetComponents(typeof(DetonatorComponent));
		Component[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			DetonatorComponent detonatorComponent = (DetonatorComponent)array[i];
			if (detonatorComponent is DetonatorFireball)
			{
				_fireball = detonatorComponent as DetonatorFireball;
			}
			if (detonatorComponent is DetonatorSparks)
			{
				_sparks = detonatorComponent as DetonatorSparks;
			}
			if (detonatorComponent is DetonatorShockwave)
			{
				_shockwave = detonatorComponent as DetonatorShockwave;
			}
			if (detonatorComponent is DetonatorSmoke)
			{
				_smoke = detonatorComponent as DetonatorSmoke;
			}
			if (detonatorComponent is DetonatorGlow)
			{
				_glow = detonatorComponent as DetonatorGlow;
			}
			if (detonatorComponent is DetonatorLight)
			{
				_light = detonatorComponent as DetonatorLight;
			}
			if (detonatorComponent is DetonatorForce)
			{
				_force = detonatorComponent as DetonatorForce;
			}
			if (detonatorComponent is DetonatorHeatwave)
			{
				_heatwave = detonatorComponent as DetonatorHeatwave;
			}
		}
		if (!_fireball && autoCreateFireball)
		{
			_fireball = base.gameObject.AddComponent<DetonatorFireball>();
			_fireball.Reset();
		}
		if (!_smoke && autoCreateSmoke)
		{
			_smoke = base.gameObject.AddComponent<DetonatorSmoke>();
			_smoke.Reset();
		}
		if (!_sparks && autoCreateSparks)
		{
			_sparks = base.gameObject.AddComponent<DetonatorSparks>();
			_sparks.Reset();
		}
		if (!_shockwave && autoCreateShockwave)
		{
			_shockwave = base.gameObject.AddComponent<DetonatorShockwave>();
			_shockwave.Reset();
		}
		if (!_glow && autoCreateGlow)
		{
			_glow = base.gameObject.AddComponent<DetonatorGlow>();
			_glow.Reset();
		}
		if (!_light && autoCreateLight)
		{
			_light = base.gameObject.AddComponent<DetonatorLight>();
			_light.Reset();
		}
		if (!_force && autoCreateForce)
		{
			_force = base.gameObject.AddComponent<DetonatorForce>();
			_force.Reset();
		}
		if (!_heatwave && autoCreateHeatwave && SystemInfo.supportsImageEffects)
		{
			_heatwave = base.gameObject.AddComponent<DetonatorHeatwave>();
			_heatwave.Reset();
		}
		components = GetComponents(typeof(DetonatorComponent));
	}

	public void FillDefaultMaterials()
	{
		if (!fireballAMaterial)
		{
			fireballAMaterial = DefaultFireballAMaterial();
		}
		if (!fireballBMaterial)
		{
			fireballBMaterial = DefaultFireballBMaterial();
		}
		if (!smokeAMaterial)
		{
			smokeAMaterial = DefaultSmokeAMaterial();
		}
		if (!smokeBMaterial)
		{
			smokeBMaterial = DefaultSmokeBMaterial();
		}
		if (!shockwaveMaterial)
		{
			shockwaveMaterial = DefaultShockwaveMaterial();
		}
		if (!sparksMaterial)
		{
			sparksMaterial = DefaultSparksMaterial();
		}
		if (!glowMaterial)
		{
			glowMaterial = DefaultGlowMaterial();
		}
		if (!heatwaveMaterial)
		{
			heatwaveMaterial = DefaultHeatwaveMaterial();
		}
	}

	public void Start()
	{
		if (explodeOnStart)
		{
			UpdateComponents();
			Explode();
		}
	}

	public void Update()
	{
		if (destroyTime > 0f && _lastExplosionTime + destroyTime <= Time.time)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void UpdateComponents()
	{
		Component[] array;
		if (_firstComponentUpdate)
		{
			array = components;
			for (int i = 0; i < array.Length; i++)
			{
				DetonatorComponent obj = (DetonatorComponent)array[i];
				obj.Init();
				obj.SetStartValues();
			}
			_firstComponentUpdate = false;
		}
		if (_firstComponentUpdate)
		{
			return;
		}
		array = components;
		for (int i = 0; i < array.Length; i++)
		{
			DetonatorComponent detonatorComponent = (DetonatorComponent)array[i];
			if (detonatorComponent.detonatorControlled)
			{
				detonatorComponent.size = detonatorComponent.startSize * (size / _baseSize);
				detonatorComponent.timeScale = duration / _baseDuration;
				detonatorComponent.detail = detonatorComponent.startDetail * detail;
				detonatorComponent.force = detonatorComponent.startForce * (size / _baseSize) + direction * (size / _baseSize);
				detonatorComponent.velocity = detonatorComponent.startVelocity * (size / _baseSize) + direction * (size / _baseSize);
				detonatorComponent.color = Color.Lerp(detonatorComponent.startColor, color, color.a);
			}
		}
	}

	public void Explode()
	{
		_lastExplosionTime = Time.time;
		Component[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			DetonatorComponent obj = (DetonatorComponent)array[i];
			UpdateComponents();
			obj.Explode();
		}
	}

	public void Reset()
	{
		size = 10f;
		color = _baseColor;
		duration = _baseDuration;
		FillDefaultMaterials();
	}

	public static Material DefaultFireballAMaterial()
	{
		if (defaultFireballAMaterial != null)
		{
			return defaultFireballAMaterial;
		}
		defaultFireballAMaterial = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
		defaultFireballAMaterial.name = "FireballA-Default";
		Texture2D mainTexture = Resources.Load("Detonator/Textures/Fireball") as Texture2D;
		defaultFireballAMaterial.SetColor("_TintColor", Color.white);
		defaultFireballAMaterial.mainTexture = mainTexture;
		defaultFireballAMaterial.mainTextureScale = new Vector2(0.5f, 1f);
		return defaultFireballAMaterial;
	}

	public static Material DefaultFireballBMaterial()
	{
		if (defaultFireballBMaterial != null)
		{
			return defaultFireballBMaterial;
		}
		defaultFireballBMaterial = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
		defaultFireballBMaterial.name = "FireballB-Default";
		Texture2D mainTexture = Resources.Load("Detonator/Textures/Fireball") as Texture2D;
		defaultFireballBMaterial.SetColor("_TintColor", Color.white);
		defaultFireballBMaterial.mainTexture = mainTexture;
		defaultFireballBMaterial.mainTextureScale = new Vector2(0.5f, 1f);
		defaultFireballBMaterial.mainTextureOffset = new Vector2(0.5f, 0f);
		return defaultFireballBMaterial;
	}

	public static Material DefaultSmokeAMaterial()
	{
		if (defaultSmokeAMaterial != null)
		{
			return defaultSmokeAMaterial;
		}
		defaultSmokeAMaterial = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended"));
		defaultSmokeAMaterial.name = "SmokeA-Default";
		Texture2D mainTexture = Resources.Load("Detonator/Textures/Smoke") as Texture2D;
		defaultSmokeAMaterial.SetColor("_TintColor", Color.white);
		defaultSmokeAMaterial.mainTexture = mainTexture;
		defaultSmokeAMaterial.mainTextureScale = new Vector2(0.5f, 1f);
		return defaultSmokeAMaterial;
	}

	public static Material DefaultSmokeBMaterial()
	{
		if (defaultSmokeBMaterial != null)
		{
			return defaultSmokeBMaterial;
		}
		defaultSmokeBMaterial = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended"));
		defaultSmokeBMaterial.name = "SmokeB-Default";
		Texture2D mainTexture = Resources.Load("Detonator/Textures/Smoke") as Texture2D;
		defaultSmokeBMaterial.SetColor("_TintColor", Color.white);
		defaultSmokeBMaterial.mainTexture = mainTexture;
		defaultSmokeBMaterial.mainTextureScale = new Vector2(0.5f, 1f);
		defaultSmokeBMaterial.mainTextureOffset = new Vector2(0.5f, 0f);
		return defaultSmokeBMaterial;
	}

	public static Material DefaultSparksMaterial()
	{
		if (defaultSparksMaterial != null)
		{
			return defaultSparksMaterial;
		}
		defaultSparksMaterial = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
		defaultSparksMaterial.name = "Sparks-Default";
		Texture2D mainTexture = Resources.Load("Detonator/Textures/GlowDot") as Texture2D;
		defaultSparksMaterial.SetColor("_TintColor", Color.white);
		defaultSparksMaterial.mainTexture = mainTexture;
		return defaultSparksMaterial;
	}

	public static Material DefaultShockwaveMaterial()
	{
		if (defaultShockwaveMaterial != null)
		{
			return defaultShockwaveMaterial;
		}
		defaultShockwaveMaterial = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
		defaultShockwaveMaterial.name = "Shockwave-Default";
		Texture2D mainTexture = Resources.Load("Detonator/Textures/Shockwave") as Texture2D;
		defaultShockwaveMaterial.SetColor("_TintColor", new Color(0.1f, 0.1f, 0.1f, 1f));
		defaultShockwaveMaterial.mainTexture = mainTexture;
		return defaultShockwaveMaterial;
	}

	public static Material DefaultGlowMaterial()
	{
		if (defaultGlowMaterial != null)
		{
			return defaultGlowMaterial;
		}
		defaultGlowMaterial = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
		defaultGlowMaterial.name = "Glow-Default";
		Texture2D mainTexture = Resources.Load("Detonator/Textures/Glow") as Texture2D;
		defaultGlowMaterial.SetColor("_TintColor", Color.white);
		defaultGlowMaterial.mainTexture = mainTexture;
		return defaultGlowMaterial;
	}

	public static Material DefaultHeatwaveMaterial()
	{
		if (SystemInfo.supportsImageEffects)
		{
			if (defaultHeatwaveMaterial != null)
			{
				return defaultHeatwaveMaterial;
			}
			defaultHeatwaveMaterial = new Material(Shader.Find("HeatDistort"));
			defaultHeatwaveMaterial.name = "Heatwave-Default";
			Texture2D value = Resources.Load("Detonator/Textures/Heatwave") as Texture2D;
			defaultHeatwaveMaterial.SetTexture("_BumpMap", value);
			return defaultHeatwaveMaterial;
		}
		return null;
	}
}
