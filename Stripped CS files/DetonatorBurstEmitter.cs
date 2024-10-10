using UnityEngine;

public class DetonatorBurstEmitter : DetonatorComponent
{
	public ParticleSystem _particleSystem;

	public float _baseDamping = 0.1300004f;

	public float _baseSize = 1f;

	public Color _baseColor = Color.white;

	public float damping = 1f;

	public float startRadius = 1f;

	public float maxScreenSize = 2f;

	public bool explodeOnAwake;

	public bool oneShot = true;

	public float sizeVariation;

	public float particleSize = 1f;

	public float count = 1f;

	public float sizeGrow = 20f;

	public bool exponentialGrowth = true;

	public float durationVariation;

	public bool useWorldSpace = true;

	public float upwardsBias;

	public float angularVelocity = 20f;

	public bool randomRotation = true;

	public ParticleSystemRenderMode renderModeNewSystem;

	public bool useExplicitColorAnimation;

	public Color[] colorAnimation = new Color[5];

	public bool _delayedExplosionStarted;

	public float _explodeDelay;

	public Material material;

	public float _emitTime;

	public float speed = 3f;

	public float initFraction = 0.1f;

	public static float epsilon = 0.01f;

	public float _tmpParticleSize;

	public Vector3 _tmpPos;

	public Vector3 _thisPos;

	public float _tmpDuration;

	public float _tmpCount;

	public float _scaledDuration;

	public float _scaledDurationVariation;

	public float _scaledStartRadius;

	public float _scaledColor;

	public float _tmpAngularVelocity;

	public override void Init()
	{
		MonoBehaviour.print("UNUSED");
	}

	public void Awake()
	{
		_particleSystem = base.gameObject.AddComponent<ParticleSystem>();
		_particleSystem.hideFlags = HideFlags.HideAndDontSave;
		ParticleSystem.LimitVelocityOverLifetimeModule limitVelocityOverLifetime = _particleSystem.limitVelocityOverLifetime;
		limitVelocityOverLifetime.enabled = true;
		limitVelocityOverLifetime.dampen = _baseDamping;
		ParticleSystem.MainModule main = _particleSystem.main;
		main.playOnAwake = false;
		_particleSystem.Stop();
		ParticleSystemRenderer component = _particleSystem.GetComponent<ParticleSystemRenderer>();
		component.maxParticleSize = maxScreenSize;
		component.material = material;
		component.material.color = Color.white;
		ParticleSystem.MinMaxCurve startLifetime = main.startLifetime;
		ParticleSystem.SizeOverLifetimeModule sizeOverLifetime = _particleSystem.sizeOverLifetime;
		sizeOverLifetime.enabled = true;
		float valueEnd = (1f + sizeGrow) * startLifetime.constantMax;
		sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, valueEnd));
		if (explodeOnAwake)
		{
			Explode();
		}
	}

	public void Update()
	{
		float num = sizeGrow;
		if (exponentialGrowth)
		{
			float num2 = Time.time - _emitTime;
			float num3 = SizeFunction(num2 - epsilon);
			num = (SizeFunction(num2) / num3 - 1f) / epsilon;
		}
		else
		{
			num = sizeGrow;
		}
		ParticleSystem.MinMaxCurve startLifetime = _particleSystem.main.startLifetime;
		ParticleSystem.SizeOverLifetimeModule sizeOverLifetime = _particleSystem.sizeOverLifetime;
		float valueEnd = (1f + num) * startLifetime.constantMax;
		sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, valueEnd));
		if (_delayedExplosionStarted)
		{
			_explodeDelay -= Time.deltaTime;
			if (_explodeDelay <= 0f)
			{
				Explode();
			}
		}
	}

	public float SizeFunction(float elapsedTime)
	{
		float num = 1f - 1f / (1f + elapsedTime * speed);
		return initFraction + (1f - initFraction) * num;
	}

	public void Reset()
	{
		size = _baseSize;
		color = _baseColor;
		damping = _baseDamping;
	}

	public override void Explode()
	{
		if (!on)
		{
			return;
		}
		ParticleSystem.MainModule main = _particleSystem.main;
		main.simulationSpace = (useWorldSpace ? ParticleSystemSimulationSpace.World : ParticleSystemSimulationSpace.Local);
		_scaledDuration = timeScale * duration;
		_scaledDurationVariation = timeScale * durationVariation;
		_scaledStartRadius = size * startRadius;
		ParticleSystemRenderer component = _particleSystem.GetComponent<ParticleSystemRenderer>();
		component.renderMode = renderModeNewSystem;
		if (!_delayedExplosionStarted)
		{
			_explodeDelay = explodeDelayMin + Random.value * (explodeDelayMax - explodeDelayMin);
		}
		if (_explodeDelay <= 0f)
		{
			Color[] array = new Color[5];
			ParticleSystem.ColorOverLifetimeModule colorOverLifetime = _particleSystem.colorOverLifetime;
			colorOverLifetime.enabled = true;
			Gradient gradient = new Gradient();
			if (useExplicitColorAnimation)
			{
				array[0] = colorAnimation[0];
				array[1] = colorAnimation[1];
				array[2] = colorAnimation[2];
				array[3] = colorAnimation[3];
				array[4] = colorAnimation[4];
			}
			else
			{
				array[0] = new Color(color.r, color.g, color.b, color.a * 0.7f);
				array[1] = new Color(color.r, color.g, color.b, color.a * 1f);
				array[2] = new Color(color.r, color.g, color.b, color.a * 0.5f);
				array[3] = new Color(color.r, color.g, color.b, color.a * 0.3f);
				array[4] = new Color(color.r, color.g, color.b, color.a * 0f);
			}
			gradient.SetKeys(new GradientColorKey[5]
			{
				new GradientColorKey(array[0], 0f),
				new GradientColorKey(array[1], 0.25f),
				new GradientColorKey(array[2], 0.5f),
				new GradientColorKey(array[3], 0.75f),
				new GradientColorKey(array[4], 1f)
			}, new GradientAlphaKey[5]
			{
				new GradientAlphaKey(array[0].a, 0f),
				new GradientAlphaKey(array[1].a, 0.25f),
				new GradientAlphaKey(array[2].a, 0.5f),
				new GradientAlphaKey(array[3].a, 0.75f),
				new GradientAlphaKey(array[4].a, 1f)
			});
			ParticleSystem.MinMaxGradient minMaxGradient = new ParticleSystem.MinMaxGradient(gradient);
			colorOverLifetime.color = minMaxGradient;
			component.material = material;
			ParticleSystem.ForceOverLifetimeModule forceOverLifetime = _particleSystem.forceOverLifetime;
			forceOverLifetime.enabled = true;
			ParticleSystem.MinMaxCurve x = forceOverLifetime.x;
			ParticleSystem.MinMaxCurve y = forceOverLifetime.y;
			ParticleSystem.MinMaxCurve z = forceOverLifetime.z;
			x.mode = ParticleSystemCurveMode.TwoConstants;
			y.mode = ParticleSystemCurveMode.TwoConstants;
			z.mode = ParticleSystemCurveMode.TwoConstants;
			x.constantMin = force.x;
			y.constantMin = force.y;
			z.constantMin = force.z;
			x.constantMax = force.x;
			y.constantMax = force.y;
			z.constantMax = force.z;
			_tmpCount = count * detail;
			if (_tmpCount < 1f)
			{
				_tmpCount = 1f;
			}
			if (main.simulationSpace == ParticleSystemSimulationSpace.World)
			{
				_thisPos = base.gameObject.transform.position;
			}
			else
			{
				_thisPos = new Vector3(0f, 0f, 0f);
			}
			for (int i = 1; (float)i <= _tmpCount; i++)
			{
				_tmpPos = Vector3.Scale(Random.insideUnitSphere, new Vector3(_scaledStartRadius, _scaledStartRadius, _scaledStartRadius));
				_tmpPos = _thisPos + _tmpPos;
				if (randomRotation)
				{
					_tmpAngularVelocity = Random.Range(-1f, 1f) * angularVelocity;
				}
				else
				{
					_tmpAngularVelocity = angularVelocity;
				}
				_tmpParticleSize = size * (particleSize + Random.value * sizeVariation);
				_tmpDuration = _scaledDuration + Random.value * _scaledDurationVariation;
				_particleSystem.Emit(_tmpPos, new Vector3(_tmpAngularVelocity, _tmpAngularVelocity, _tmpAngularVelocity), _tmpParticleSize, _tmpDuration, color);
			}
			_emitTime = Time.time;
			_delayedExplosionStarted = false;
			_explodeDelay = 0f;
		}
		else
		{
			_delayedExplosionStarted = true;
		}
	}
}
