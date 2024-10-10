using UnityEngine;

[RequireComponent(typeof(Detonator))]
[AddComponentMenu("Detonator/Fireball")]
public class DetonatorFireball : DetonatorComponent
{
	public float _baseSize = 1f;

	public float _baseDuration = 3f;

	public Color _baseColor = new Color(1f, 0.423f, 0f, 0.5f);

	public float _scaledDuration;

	public GameObject _fireballA;

	public DetonatorBurstEmitter _fireballAEmitter;

	public Material fireballAMaterial;

	public GameObject _fireballB;

	public DetonatorBurstEmitter _fireballBEmitter;

	public Material fireballBMaterial;

	public GameObject _fireShadow;

	public DetonatorBurstEmitter _fireShadowEmitter;

	public Material fireShadowMaterial;

	public bool drawFireballA = true;

	public bool drawFireballB = true;

	public bool drawFireShadow = true;

	public Color _detailAdjustedColor;

	public override void Init()
	{
		FillMaterials(wipe: false);
		BuildFireballA();
		BuildFireballB();
		BuildFireShadow();
	}

	public void FillMaterials(bool wipe)
	{
		if (!fireballAMaterial || wipe)
		{
			fireballAMaterial = MyDetonator().fireballAMaterial;
		}
		if (!fireballBMaterial || wipe)
		{
			fireballBMaterial = MyDetonator().fireballBMaterial;
		}
		if (!fireShadowMaterial || wipe)
		{
			if ((double)Random.value > 0.5)
			{
				fireShadowMaterial = MyDetonator().smokeAMaterial;
			}
			else
			{
				fireShadowMaterial = MyDetonator().smokeBMaterial;
			}
		}
	}

	public void BuildFireballA()
	{
		_fireballA = new GameObject("FireballA");
		_fireballAEmitter = _fireballA.AddComponent<DetonatorBurstEmitter>();
		_fireballA.transform.parent = base.transform;
		_fireballA.transform.localRotation = Quaternion.identity;
		_fireballAEmitter.material = fireballAMaterial;
		_fireballAEmitter.useWorldSpace = MyDetonator().useWorldSpace;
		_fireballAEmitter.upwardsBias = MyDetonator().upwardsBias;
	}

	public void UpdateFireballA()
	{
		_fireballA.transform.localPosition = Vector3.Scale(localPosition, new Vector3(size, size, size));
		_fireballAEmitter.color = base.color;
		_fireballAEmitter.duration = duration * 0.5f;
		_fireballAEmitter.durationVariation = duration * 0.5f;
		_fireballAEmitter.count = 2f;
		_fireballAEmitter.timeScale = timeScale;
		_fireballAEmitter.detail = detail;
		_fireballAEmitter.particleSize = 14f;
		_fireballAEmitter.sizeVariation = 3f;
		_fireballAEmitter.velocity = velocity;
		_fireballAEmitter.startRadius = 4f;
		_fireballAEmitter.size = size;
		_fireballAEmitter.useExplicitColorAnimation = true;
		Color b = new Color(1f, 1f, 1f, 0.5f);
		Color b2 = new Color(0.6f, 0.15f, 0.15f, 0.3f);
		Color color = new Color(0.1f, 0.2f, 0.45f, 0f);
		_fireballAEmitter.colorAnimation[0] = Color.Lerp(base.color, b, 0.8f);
		_fireballAEmitter.colorAnimation[1] = Color.Lerp(base.color, b, 0.5f);
		_fireballAEmitter.colorAnimation[2] = base.color;
		_fireballAEmitter.colorAnimation[3] = Color.Lerp(base.color, b2, 0.7f);
		_fireballAEmitter.colorAnimation[4] = color;
		_fireballAEmitter.explodeDelayMin = explodeDelayMin;
		_fireballAEmitter.explodeDelayMax = explodeDelayMax;
	}

	public void BuildFireballB()
	{
		_fireballB = new GameObject("FireballB");
		_fireballBEmitter = _fireballB.AddComponent<DetonatorBurstEmitter>();
		_fireballB.transform.parent = base.transform;
		_fireballB.transform.localRotation = Quaternion.identity;
		_fireballBEmitter.material = fireballBMaterial;
		_fireballBEmitter.useWorldSpace = MyDetonator().useWorldSpace;
		_fireballBEmitter.upwardsBias = MyDetonator().upwardsBias;
	}

	public void UpdateFireballB()
	{
		_fireballB.transform.localPosition = Vector3.Scale(localPosition, new Vector3(size, size, size));
		_fireballBEmitter.color = base.color;
		_fireballBEmitter.duration = duration * 0.5f;
		_fireballBEmitter.durationVariation = duration * 0.5f;
		_fireballBEmitter.count = 2f;
		_fireballBEmitter.timeScale = timeScale;
		_fireballBEmitter.detail = detail;
		_fireballBEmitter.particleSize = 10f;
		_fireballBEmitter.sizeVariation = 6f;
		_fireballBEmitter.velocity = velocity;
		_fireballBEmitter.startRadius = 4f;
		_fireballBEmitter.size = size;
		_fireballBEmitter.useExplicitColorAnimation = true;
		Color b = new Color(1f, 1f, 1f, 0.5f);
		Color b2 = new Color(0.6f, 0.15f, 0.15f, 0.3f);
		Color color = new Color(0.1f, 0.2f, 0.45f, 0f);
		_fireballBEmitter.colorAnimation[0] = Color.Lerp(base.color, b, 0.8f);
		_fireballBEmitter.colorAnimation[1] = Color.Lerp(base.color, b, 0.5f);
		_fireballBEmitter.colorAnimation[2] = base.color;
		_fireballBEmitter.colorAnimation[3] = Color.Lerp(base.color, b2, 0.7f);
		_fireballBEmitter.colorAnimation[4] = color;
		_fireballBEmitter.explodeDelayMin = explodeDelayMin;
		_fireballBEmitter.explodeDelayMax = explodeDelayMax;
	}

	public void BuildFireShadow()
	{
		_fireShadow = new GameObject("FireShadow");
		_fireShadowEmitter = _fireShadow.AddComponent<DetonatorBurstEmitter>();
		_fireShadow.transform.parent = base.transform;
		_fireShadow.transform.localRotation = Quaternion.identity;
		_fireShadowEmitter.material = fireShadowMaterial;
		_fireShadowEmitter.useWorldSpace = MyDetonator().useWorldSpace;
		_fireShadowEmitter.upwardsBias = MyDetonator().upwardsBias;
	}

	public void UpdateFireShadow()
	{
		_fireShadow.transform.localPosition = Vector3.Scale(localPosition, new Vector3(size, size, size));
		_fireShadow.transform.LookAt(Camera.main.transform);
		_fireShadow.transform.localPosition = -(Vector3.forward * 1f);
		_fireShadowEmitter.color = new Color(0.1f, 0.1f, 0.1f, 0.6f);
		_fireShadowEmitter.duration = duration * 0.5f;
		_fireShadowEmitter.durationVariation = duration * 0.5f;
		_fireShadowEmitter.timeScale = timeScale;
		_fireShadowEmitter.detail = 1f;
		_fireShadowEmitter.particleSize = 13f;
		_fireShadowEmitter.velocity = velocity;
		_fireShadowEmitter.sizeVariation = 1f;
		_fireShadowEmitter.count = 4f;
		_fireShadowEmitter.startRadius = 6f;
		_fireShadowEmitter.size = size;
		_fireShadowEmitter.explodeDelayMin = explodeDelayMin;
		_fireShadowEmitter.explodeDelayMax = explodeDelayMax;
	}

	public void Reset()
	{
		FillMaterials(wipe: true);
		on = true;
		size = _baseSize;
		duration = _baseDuration;
		explodeDelayMin = 0f;
		explodeDelayMax = 0f;
		color = _baseColor;
	}

	public override void Explode()
	{
		if (!(detailThreshold > detail) && on)
		{
			UpdateFireballA();
			UpdateFireballB();
			UpdateFireShadow();
			if (drawFireballA)
			{
				_fireballAEmitter.Explode();
			}
			if (drawFireballB)
			{
				_fireballBEmitter.Explode();
			}
			if (drawFireShadow)
			{
				_fireShadowEmitter.Explode();
			}
		}
	}
}
