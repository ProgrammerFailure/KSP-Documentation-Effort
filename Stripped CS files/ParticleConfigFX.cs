using System;
using UnityEngine;

[EffectDefinition("PARTICLE_CONFIG")]
public class ParticleConfigFX : EffectBehaviour
{
	[Serializable]
	public class PFXMaterial
	{
		public enum MaterialType
		{
			Additive,
			AlphaBlended
		}

		[Persistent]
		public MaterialType type;

		[Persistent]
		public Color color = new Color(1f, 1f, 1f, 1f);

		[Persistent]
		public string texture = "";

		public Material CreateMaterial()
		{
			Material material = null;
			switch (type)
			{
			case MaterialType.AlphaBlended:
				material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended"));
				break;
			case MaterialType.Additive:
				material = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
				break;
			}
			material.color = color;
			Texture2D texture2D = GameDatabase.Instance.GetTexture(texture, asNormalMap: false);
			if (texture2D != null)
			{
				material.mainTexture = texture2D;
			}
			else
			{
				Debug.LogError("Cannot assign texture to ParticleFX");
			}
			return material;
		}
	}

	public GameObject pHost;

	public ParticleSystem ps;

	[Persistent]
	public float power;

	[Persistent]
	public bool useWorldSpace = true;

	[Persistent]
	public Vector3 ellipsoid = Vector3.one;

	public FXCurve minSize = new FXCurve("minSize", 0.1f);

	public FXCurve maxSize = new FXCurve("maxSize", 0.1f);

	public FXCurve minEnergy = new FXCurve("minEnergy", 2f);

	public FXCurve maxEnergy = new FXCurve("maxEnergy", 3f);

	public FXCurve minEmission = new FXCurve("minEmission", 80f);

	public FXCurve maxEmission = new FXCurve("maxEmission", 100f);

	[Persistent]
	public Vector3 worldVelocity = Vector3.zero;

	[Persistent]
	public Vector3 localVelocity = Vector3.zero;

	[Persistent]
	public Vector3 rndVelocity = Vector3.zero;

	[Persistent]
	public float emitterVelocityScale = 0.05f;

	[Persistent]
	public Vector3 tangentVelocity = Vector3.zero;

	[Persistent]
	public float angularVelocity;

	[Persistent]
	public float rndAngularVelocity = 0.05f;

	[Persistent]
	public bool rndRotation;

	[Persistent]
	public bool oneShot;

	[Persistent]
	public bool doesAnimateColor = true;

	[Persistent]
	public Color[] colorAnimation = new Color[5]
	{
		new Color(1f, 1f, 1f, dC * 10f),
		new Color(1f, 1f, 1f, dC * 180f),
		new Color(1f, 1f, 1f, 1f),
		new Color(1f, 1f, 1f, dC * 180f),
		new Color(1f, 1f, 1f, dC * 10f)
	};

	public static float dC = 0.003921569f;

	[Persistent]
	public Vector3 worldRotationAxis = Vector3.zero;

	[Persistent]
	public Vector3 localRotationAxis = Vector3.zero;

	public FXCurve sizeGrow = new FXCurve("sizeGrow", 0f);

	[Persistent]
	public Vector3 rndForce = Vector3.zero;

	[Persistent]
	public Vector3 force = Vector3.zero;

	[Persistent]
	public float damping = 1f;

	[Persistent]
	public bool autodestruct;

	[Persistent]
	public bool castShadows;

	[Persistent]
	public bool recieveShadows;

	public FXCurve lengthScale = new FXCurve("lengthScale", 0f);

	[Persistent]
	public float velocityScale;

	[Persistent]
	public float maxParticleSize = 0.25f;

	[Persistent]
	public ParticleSystemRenderMode particleRenderModeNewSystem;

	[Persistent]
	public int uvAnimationXTile = 1;

	[Persistent]
	public int uvAnimationYTile = 1;

	[Persistent]
	public int uvAnimationCycles = 1;

	[Persistent]
	public PFXMaterial material = new PFXMaterial();

	public void InitializeComponents()
	{
		power = 0f;
		if (ps == null)
		{
			ps = pHost.AddComponent<ParticleSystem>();
		}
		InitializeParticleSystem();
		UpdateComponents();
	}

	public void InitializeParticleSystem()
	{
		ParticleSystem.MainModule main = ps.main;
		ParticleSystemRenderer component = pHost.GetComponent<ParticleSystemRenderer>();
		ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = ps.velocityOverLifetime;
		velocityOverLifetime.enabled = true;
		main.simulationSpace = (useWorldSpace ? ParticleSystemSimulationSpace.World : ParticleSystemSimulationSpace.Local);
		velocityOverLifetime.x = localVelocity.x;
		velocityOverLifetime.y = localVelocity.y;
		velocityOverLifetime.z = localVelocity.z;
		ParticleSystem.MinMaxCurve z = new ParticleSystem.MinMaxCurve
		{
			constantMin = angularVelocity / 57.29578f,
			constantMax = (angularVelocity + rndAngularVelocity) / 57.29578f,
			mode = ParticleSystemCurveMode.TwoConstants
		};
		ParticleSystem.RotationOverLifetimeModule rotationOverLifetime = ps.rotationOverLifetime;
		rotationOverLifetime.enabled = true;
		rotationOverLifetime.z = z;
		if (doesAnimateColor)
		{
			ParticleSystem.ColorOverLifetimeModule colorOverLifetime = ps.colorOverLifetime;
			colorOverLifetime.enabled = true;
			Gradient gradient = new Gradient();
			gradient.SetKeys(new GradientColorKey[5]
			{
				new GradientColorKey(colorAnimation[0], 0f),
				new GradientColorKey(colorAnimation[1], 0.25f),
				new GradientColorKey(colorAnimation[2], 0.5f),
				new GradientColorKey(colorAnimation[3], 0.75f),
				new GradientColorKey(colorAnimation[4], 1f)
			}, new GradientAlphaKey[5]
			{
				new GradientAlphaKey(colorAnimation[0].a, 0f),
				new GradientAlphaKey(colorAnimation[1].a, 0.25f),
				new GradientAlphaKey(colorAnimation[2].a, 0.5f),
				new GradientAlphaKey(colorAnimation[3].a, 0.75f),
				new GradientAlphaKey(colorAnimation[4].a, 1f)
			});
			ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient(gradient);
			colorOverLifetime.color = color;
		}
		ParticleSystem.ForceOverLifetimeModule forceOverLifetime = ps.forceOverLifetime;
		forceOverLifetime.enabled = true;
		ParticleSystem.MinMaxCurve x = default(ParticleSystem.MinMaxCurve);
		ParticleSystem.MinMaxCurve y = default(ParticleSystem.MinMaxCurve);
		ParticleSystem.MinMaxCurve z2 = default(ParticleSystem.MinMaxCurve);
		x.mode = ParticleSystemCurveMode.TwoConstants;
		y.mode = ParticleSystemCurveMode.TwoConstants;
		z2.mode = ParticleSystemCurveMode.TwoConstants;
		x.constantMin = force.x - rndForce.x / 2f;
		y.constantMin = force.y - rndForce.y / 2f;
		z2.constantMin = force.z - rndForce.z / 2f;
		x.constantMax = force.x + rndForce.x / 2f;
		y.constantMax = force.y + rndForce.y / 2f;
		z2.constantMax = force.z + rndForce.z / 2f;
		forceOverLifetime.x = x;
		forceOverLifetime.y = y;
		forceOverLifetime.z = z2;
		ParticleSystem.LimitVelocityOverLifetimeModule limitVelocityOverLifetime = ps.limitVelocityOverLifetime;
		limitVelocityOverLifetime.enabled = true;
		limitVelocityOverLifetime.dampen = damping;
		component.renderMode = particleRenderModeNewSystem;
		component.velocityScale = velocityScale;
		component.maxParticleSize = maxParticleSize;
		if (uvAnimationXTile != 1 || uvAnimationYTile != 1)
		{
			ParticleSystem.TextureSheetAnimationModule textureSheetAnimation = ps.textureSheetAnimation;
			textureSheetAnimation.enabled = true;
			textureSheetAnimation.numTilesX = uvAnimationXTile;
			textureSheetAnimation.numTilesY = uvAnimationYTile;
			textureSheetAnimation.frameOverTime = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 0f, 1f, 1f));
		}
		component.material = material.CreateMaterial();
	}

	public void UpdateComponents()
	{
		ParticleSystem.MainModule main = ps.main;
		ParticleSystem.MinMaxCurve startSize = main.startSize;
		startSize.constantMin = minSize.Value(power);
		startSize.constantMax = maxSize.Value(power);
		startSize.mode = ParticleSystemCurveMode.TwoConstants;
		main.startSize = startSize;
		ParticleSystem.MinMaxCurve startLifetime = main.startLifetime;
		startLifetime.constantMin = minEnergy.Value(power);
		startLifetime.constantMax = maxEnergy.Value(power);
		startLifetime.mode = ParticleSystemCurveMode.TwoConstants;
		main.startLifetime = startLifetime;
		ParticleSystem.SizeOverLifetimeModule sizeOverLifetime = ps.sizeOverLifetime;
		sizeOverLifetime.enabled = true;
		float valueEnd = (1f + sizeGrow.Value(power)) * maxEnergy.Value(power);
		sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, valueEnd));
		pHost.GetComponent<ParticleSystemRenderer>().lengthScale = lengthScale.Value(power);
	}

	public override void OnLoad(ConfigNode node)
	{
		ConfigNode.LoadObjectFromConfig(this, node);
		string value = node.GetValue("transform");
		if (value != null)
		{
			Transform transform = KSPUtil.FindInPartModel(hostPart.transform, value);
			if (transform != null)
			{
				pHost = transform.gameObject;
			}
		}
		minSize.Load("minSize", node);
		maxSize.Load("maxSize", node);
		minEnergy.Load("minEnergy", node);
		maxEnergy.Load("maxEnergy", node);
		minEmission.Load("minEmission", node);
		maxEmission.Load("maxEmission", node);
	}

	public override void OnSave(ConfigNode node)
	{
		ConfigNode configNode = ConfigNode.CreateConfigFromObject(this);
		configNode.name = "PARTICLE";
		configNode.CopyTo(node);
		minSize.Save(node);
		maxSize.Save(node);
		minEnergy.Save(node);
		maxEnergy.Save(node);
		minEmission.Save(node);
		maxEmission.Save(node);
	}

	public override void OnInitialize()
	{
		InitializeComponents();
	}

	public override void OnEvent(float power)
	{
		if (!oneShot)
		{
			power = Mathf.Clamp01(power);
			UpdateComponents();
		}
	}

	public override void OnEvent()
	{
		if (oneShot)
		{
			power = 1f;
			UpdateComponents();
			ps.Emit(Mathf.FloorToInt(UnityEngine.Random.Range(minEmission, maxEmission)));
		}
	}
}
