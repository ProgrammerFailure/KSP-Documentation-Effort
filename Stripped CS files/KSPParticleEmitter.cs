using System;
using UnityEngine;
using UnityEngine.Rendering;

[AddComponentMenu("KSP/Particle Emitter")]
[ExecuteInEditMode]
public class KSPParticleEmitter : MonoBehaviour
{
	public enum EmissionShape
	{
		Ellipsoid,
		Ellipse,
		Sphere,
		Ring,
		Cuboid,
		Plane,
		Line,
		Point
	}

	[SerializeField]
	public ParticleSystem ps;

	public bool emit;

	public EmissionShape shape;

	public Vector3 shape3D;

	public Vector2 shape2D;

	public float shape1D;

	public Color color;

	public bool useWorldSpace;

	public float minSize;

	public float maxSize;

	public float minEnergy;

	public float maxEnergy;

	public int minEmission;

	public int maxEmission;

	public Vector3 worldVelocity;

	public Vector3 localVelocity;

	public Vector3 rndVelocity;

	public float emitterVelocityScale;

	public float angularVelocity;

	public float rndAngularVelocity;

	public bool rndRotation;

	public bool doesAnimateColor;

	public Color[] colorAnimation;

	public Vector3 worldRotationAxis;

	public Vector3 localRotationAxis;

	public float sizeGrow;

	public Vector3 rndForce;

	public Vector3 force;

	public float damping;

	public bool castShadows;

	public bool recieveShadows;

	public float lengthScale;

	public float velocityScale;

	public float maxParticleSize;

	public ParticleSystemRenderMode particleRenderMode;

	public int uvAnimationXTile;

	public int uvAnimationYTile;

	public int uvAnimationCycles;

	public Material material;

	public bool dirty = true;

	public int framesAlive;

	public float particlesThisFrame;

	public float amountPerSec;

	public float overflow;

	public int itr;

	public Vector3 position;

	public float posFloat;

	public Vector3 velocity;

	public float rotation;

	public void EnsureParticleSystemCreated()
	{
		if (ps == null)
		{
			ps = base.gameObject.GetComponent<ParticleSystem>();
			if (ps == null)
			{
				ps = base.gameObject.AddComponent<ParticleSystem>();
			}
		}
		ParticleSystem.EmissionModule emission = ps.emission;
		emission.enabled = false;
	}

	public void Reset()
	{
		StopCoroutine("EmitCoroutine");
		emit = true;
		shape = EmissionShape.Ellipsoid;
		shape3D = Vector3.one;
		shape2D = Vector2.one;
		shape1D = 1f;
		color = Color.white;
		useWorldSpace = true;
		minSize = 0.1f;
		maxSize = 0.25f;
		minEnergy = 2f;
		maxEnergy = 3f;
		minEmission = 80;
		maxEmission = 100;
		worldVelocity = Vector3.zero;
		localVelocity = Vector3.zero;
		rndVelocity = Vector3.zero;
		emitterVelocityScale = 0.05f;
		angularVelocity = 0f;
		rndAngularVelocity = 0.05f;
		rndRotation = false;
		doesAnimateColor = true;
		colorAnimation = new Color[5]
		{
			new Color(1f, 1f, 1f, 2f / 51f),
			new Color(1f, 1f, 1f, 0.7058824f),
			new Color(1f, 1f, 1f, 1f),
			new Color(1f, 1f, 1f, 0.7058824f),
			new Color(1f, 1f, 1f, 2f / 51f)
		};
		worldRotationAxis = Vector3.zero;
		localRotationAxis = Vector3.zero;
		sizeGrow = 0f;
		rndForce = Vector3.zero;
		force = Vector3.zero;
		damping = 1f;
		castShadows = false;
		recieveShadows = false;
		lengthScale = 0f;
		velocityScale = 0f;
		maxParticleSize = 0.25f;
		particleRenderMode = ParticleSystemRenderMode.Billboard;
		uvAnimationXTile = 1;
		uvAnimationYTile = 1;
		uvAnimationCycles = 1;
		EnsureParticleSystemCreated();
		SetDirty();
	}

	public void Awake()
	{
		if (GetComponent<KSPParticleEmitter>() != this)
		{
			Debug.LogError("Cannot have more than one KSPParticleEmitter on a single GameObject");
			UnityEngine.Object.DestroyImmediate(this);
		}
		else
		{
			EnsureParticleSystemCreated();
			SetDirty();
		}
	}

	public void SetDirty()
	{
		dirty = true;
	}

	public void SetupProperties()
	{
		if (!dirty)
		{
			return;
		}
		if (ps == null)
		{
			EnsureParticleSystemCreated();
			if (ps == null)
			{
				Debug.Log("[KSPParticleEmitter]: Cannot Setup, ParticleSystem is null.");
				return;
			}
		}
		ParticleSystem.MainModule main = ps.main;
		main.simulationSpace = (useWorldSpace ? ParticleSystemSimulationSpace.World : ParticleSystemSimulationSpace.Local);
		ParticleSystem.RotationOverLifetimeModule rotationOverLifetime = ps.rotationOverLifetime;
		rotationOverLifetime.enabled = true;
		ParticleSystem.MinMaxCurve z = default(ParticleSystem.MinMaxCurve);
		z.constantMin = angularVelocity / 57.29578f;
		z.constantMax = (angularVelocity + rndAngularVelocity) / 57.29578f;
		z.mode = ParticleSystemCurveMode.TwoConstants;
		rotationOverLifetime.z = z;
		ParticleSystem.ColorOverLifetimeModule colorOverLifetime = ps.colorOverLifetime;
		colorOverLifetime.enabled = doesAnimateColor;
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
		ParticleSystem.MinMaxGradient minMaxGradient = new ParticleSystem.MinMaxGradient(gradient);
		colorOverLifetime.color = minMaxGradient;
		ParticleSystem.SizeOverLifetimeModule sizeOverLifetime = ps.sizeOverLifetime;
		sizeOverLifetime.enabled = true;
		float p = Mathf.Lerp(minEnergy, maxEnergy, 0.5f);
		float valueEnd = 1f * Mathf.Pow(1f + sizeGrow, p);
		sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.EaseInOut(0f, 1f, 1f, valueEnd));
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
		ParticleSystemRenderer component = ps.GetComponent<ParticleSystemRenderer>();
		if (castShadows)
		{
			component.shadowCastingMode = ShadowCastingMode.On;
		}
		else
		{
			component.shadowCastingMode = ShadowCastingMode.Off;
		}
		component.receiveShadows = recieveShadows;
		component.lengthScale = lengthScale;
		component.velocityScale = velocityScale;
		component.maxParticleSize = maxParticleSize;
		component.renderMode = particleRenderMode;
		if (uvAnimationXTile != 1 || uvAnimationYTile != 1)
		{
			ParticleSystem.TextureSheetAnimationModule textureSheetAnimation = ps.textureSheetAnimation;
			textureSheetAnimation.enabled = true;
			textureSheetAnimation.numTilesX = uvAnimationXTile;
			textureSheetAnimation.numTilesY = uvAnimationYTile;
			textureSheetAnimation.frameOverTime = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 0f, 1f, 1f));
		}
		component.material = material;
		dirty = false;
	}

	public void Update()
	{
		framesAlive++;
		if (framesAlive < 2)
		{
			return;
		}
		SetupProperties();
		if (maxEmission < 0 || !emit)
		{
			return;
		}
		if (amountPerSec == 0f)
		{
			amountPerSec = UnityEngine.Random.Range(minEmission, maxEmission);
		}
		particlesThisFrame = amountPerSec * Time.deltaTime + overflow;
		if (particlesThisFrame < 1f)
		{
			overflow += particlesThisFrame;
			return;
		}
		itr = (int)particlesThisFrame;
		particlesThisFrame -= itr;
		for (overflow = 0f; itr >= 0; itr--)
		{
			EmitParticle();
		}
		amountPerSec = UnityEngine.Random.Range(minEmission, maxEmission);
	}

	public void EmitParticle()
	{
		if (ps == null)
		{
			EnsureParticleSystemCreated();
			if (ps == null)
			{
				Debug.Log("[KSPParticleEmitter]: Cannot Emit, ParticleSystem is null.");
				return;
			}
		}
		switch (shape)
		{
		case EmissionShape.Ellipsoid:
			position = UnityEngine.Random.insideUnitSphere;
			position.Scale(shape3D);
			break;
		case EmissionShape.Ellipse:
			position = UnityEngine.Random.insideUnitCircle;
			position.x *= shape2D.x;
			position.z = position.y * shape2D.y;
			position.y = 0f;
			break;
		case EmissionShape.Sphere:
			position = UnityEngine.Random.insideUnitSphere * shape1D;
			break;
		case EmissionShape.Ring:
			posFloat = UnityEngine.Random.Range(0f, (float)Math.PI * 2f);
			position = new Vector3(Mathf.Sin(posFloat) * shape2D.x, 0f, Mathf.Cos(posFloat) * shape2D.y);
			break;
		case EmissionShape.Cuboid:
			position = new Vector3(UnityEngine.Random.Range(0f - shape3D.x, shape3D.x), UnityEngine.Random.Range(0f - shape3D.y, shape3D.y), UnityEngine.Random.Range(0f - shape3D.z, shape3D.z));
			break;
		case EmissionShape.Plane:
			position = new Vector3(UnityEngine.Random.Range(0f - shape2D.x, shape2D.x), 0f, UnityEngine.Random.Range(0f - shape2D.y, shape2D.y));
			break;
		case EmissionShape.Line:
			position = new Vector3(UnityEngine.Random.Range(0f - shape1D, shape1D) * 0.5f, 0f, 0f);
			break;
		case EmissionShape.Point:
			position = Vector3.zero;
			break;
		}
		ParticleSystem.MainModule main = ps.main;
		main.simulationSpace = (useWorldSpace ? ParticleSystemSimulationSpace.World : ParticleSystemSimulationSpace.Local);
		if (useWorldSpace)
		{
			position = base.transform.TransformPoint(position);
			velocity = worldVelocity + base.transform.TransformDirection(localVelocity + new Vector3(UnityEngine.Random.Range(0f - rndVelocity.x, rndVelocity.x), UnityEngine.Random.Range(0f - rndVelocity.y, rndVelocity.y), UnityEngine.Random.Range(0f - rndVelocity.z, rndVelocity.z)));
		}
		else
		{
			velocity = localVelocity + new Vector3(UnityEngine.Random.Range(0f - rndVelocity.x, rndVelocity.x), UnityEngine.Random.Range(0f - rndVelocity.y, rndVelocity.y), UnityEngine.Random.Range(0f - rndVelocity.z, rndVelocity.z)) + base.transform.InverseTransformDirection(worldVelocity);
		}
		rotation = (rndRotation ? (UnityEngine.Random.value * 360f) : 0f);
		if (framesAlive > 2)
		{
			ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
			emitParams.position = position;
			emitParams.velocity = velocity;
			emitParams.startSize = UnityEngine.Random.Range(minSize, maxSize);
			emitParams.startLifetime = UnityEngine.Random.Range(minEnergy, maxEnergy);
			if (doesAnimateColor)
			{
				emitParams.startColor = new Color(color.r, color.g, color.b, 1f);
			}
			else
			{
				emitParams.startColor = new Color(color.r, color.g, color.b, color.a);
			}
			emitParams.rotation = rotation;
			if (Application.isPlaying)
			{
				ps.Emit(emitParams, 1);
			}
			else
			{
				ps.Simulate(Time.fixedDeltaTime, withChildren: true, restart: false, fixedTimeStep: true);
			}
		}
	}

	public void Emit()
	{
		SetupProperties();
		int num = UnityEngine.Random.Range(minEmission, maxEmission);
		for (int i = 0; i < num; i++)
		{
			EmitParticle();
		}
	}
}
