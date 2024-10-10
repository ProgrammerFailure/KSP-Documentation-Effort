using UnityEngine;

[RequireComponent(typeof(Detonator))]
[AddComponentMenu("Detonator/Smoke")]
public class DetonatorSmoke : DetonatorComponent
{
	public const float _baseSize = 1f;

	public const float _baseDuration = 8f;

	public Color _baseColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

	public const float _baseDamping = 0.1300004f;

	public float _scaledDuration;

	public GameObject _smokeA;

	public DetonatorBurstEmitter _smokeAEmitter;

	public Material smokeAMaterial;

	public GameObject _smokeB;

	public DetonatorBurstEmitter _smokeBEmitter;

	public Material smokeBMaterial;

	public bool drawSmokeA = true;

	public bool drawSmokeB = true;

	public override void Init()
	{
		FillMaterials(wipe: false);
		BuildSmokeA();
		BuildSmokeB();
	}

	public void FillMaterials(bool wipe)
	{
		if (!smokeAMaterial || wipe)
		{
			smokeAMaterial = MyDetonator().smokeAMaterial;
		}
		if (!smokeBMaterial || wipe)
		{
			smokeBMaterial = MyDetonator().smokeBMaterial;
		}
	}

	public void BuildSmokeA()
	{
		_smokeA = new GameObject("SmokeA");
		_smokeAEmitter = _smokeA.AddComponent<DetonatorBurstEmitter>();
		_smokeA.transform.parent = base.transform;
		_smokeA.transform.localPosition = localPosition;
		_smokeA.transform.localRotation = Quaternion.identity;
		_smokeAEmitter.material = smokeAMaterial;
		_smokeAEmitter.exponentialGrowth = false;
		_smokeAEmitter.sizeGrow = 0.095f;
		_smokeAEmitter.useWorldSpace = MyDetonator().useWorldSpace;
		_smokeAEmitter.upwardsBias = MyDetonator().upwardsBias;
	}

	public void UpdateSmokeA()
	{
		_smokeA.transform.localPosition = Vector3.Scale(localPosition, new Vector3(size, size, size));
		_smokeA.transform.LookAt(Camera.main.transform);
		_smokeA.transform.localPosition = -(Vector3.forward * -1.5f);
		_smokeAEmitter.color = base.color;
		_smokeAEmitter.duration = duration * 0.5f;
		_smokeAEmitter.durationVariation = 0f;
		_smokeAEmitter.timeScale = timeScale;
		_smokeAEmitter.count = 4f;
		_smokeAEmitter.particleSize = 25f;
		_smokeAEmitter.sizeVariation = 3f;
		_smokeAEmitter.velocity = velocity;
		_smokeAEmitter.startRadius = 10f;
		_smokeAEmitter.size = size;
		_smokeAEmitter.useExplicitColorAnimation = true;
		_smokeAEmitter.explodeDelayMin = explodeDelayMin;
		_smokeAEmitter.explodeDelayMax = explodeDelayMax;
		Color color = new Color(0.2f, 0.2f, 0.2f, 0.4f);
		Color color2 = new Color(0.2f, 0.2f, 0.2f, 0.7f);
		Color color3 = new Color(0.2f, 0.2f, 0.2f, 0.4f);
		Color color4 = new Color(0.2f, 0.2f, 0.2f, 0f);
		_smokeAEmitter.colorAnimation[0] = color;
		_smokeAEmitter.colorAnimation[1] = color2;
		_smokeAEmitter.colorAnimation[2] = color2;
		_smokeAEmitter.colorAnimation[3] = color3;
		_smokeAEmitter.colorAnimation[4] = color4;
	}

	public void BuildSmokeB()
	{
		_smokeB = new GameObject("SmokeB");
		_smokeBEmitter = _smokeB.AddComponent<DetonatorBurstEmitter>();
		_smokeB.transform.parent = base.transform;
		_smokeB.transform.localPosition = localPosition;
		_smokeB.transform.localRotation = Quaternion.identity;
		_smokeBEmitter.material = smokeBMaterial;
		_smokeBEmitter.exponentialGrowth = false;
		_smokeBEmitter.sizeGrow = 0.095f;
		_smokeBEmitter.useWorldSpace = MyDetonator().useWorldSpace;
		_smokeBEmitter.upwardsBias = MyDetonator().upwardsBias;
	}

	public void UpdateSmokeB()
	{
		_smokeB.transform.localPosition = Vector3.Scale(localPosition, new Vector3(size, size, size));
		_smokeB.transform.LookAt(Camera.main.transform);
		_smokeB.transform.localPosition = -(Vector3.forward * -1f);
		_smokeBEmitter.color = base.color;
		_smokeBEmitter.duration = duration * 0.5f;
		_smokeBEmitter.durationVariation = 0f;
		_smokeBEmitter.count = 2f;
		_smokeBEmitter.particleSize = 25f;
		_smokeBEmitter.sizeVariation = 3f;
		_smokeBEmitter.velocity = velocity;
		_smokeBEmitter.startRadius = 10f;
		_smokeBEmitter.size = size;
		_smokeBEmitter.useExplicitColorAnimation = true;
		_smokeBEmitter.explodeDelayMin = explodeDelayMin;
		_smokeBEmitter.explodeDelayMax = explodeDelayMax;
		Color color = new Color(0.2f, 0.2f, 0.2f, 0.4f);
		Color color2 = new Color(0.2f, 0.2f, 0.2f, 0.7f);
		Color color3 = new Color(0.2f, 0.2f, 0.2f, 0.4f);
		Color color4 = new Color(0.2f, 0.2f, 0.2f, 0f);
		_smokeBEmitter.colorAnimation[0] = color;
		_smokeBEmitter.colorAnimation[1] = color2;
		_smokeBEmitter.colorAnimation[2] = color2;
		_smokeBEmitter.colorAnimation[3] = color3;
		_smokeBEmitter.colorAnimation[4] = color4;
	}

	public void Reset()
	{
		FillMaterials(wipe: true);
		on = true;
		size = 1f;
		duration = 8f;
		explodeDelayMin = 0f;
		explodeDelayMax = 0f;
		color = _baseColor;
		velocity = new Vector3(3f, 3f, 3f);
	}

	public override void Explode()
	{
		if (!(detailThreshold > detail) && on)
		{
			UpdateSmokeA();
			UpdateSmokeB();
			if (drawSmokeA)
			{
				_smokeAEmitter.Explode();
			}
			if (drawSmokeB)
			{
				_smokeBEmitter.Explode();
			}
		}
	}
}
