using UnityEngine;

[RequireComponent(typeof(Detonator))]
public class DetonatorCloudRing : DetonatorComponent
{
	public float _baseSize = 1f;

	public float _baseDuration = 5f;

	public Vector3 _baseVelocity = new Vector3(155f, 5f, 155f);

	public Color _baseColor = Color.white;

	public Vector3 _baseForce = new Vector3(0.162f, 2.56f, 0f);

	public GameObject _cloudRing;

	public DetonatorBurstEmitter _cloudRingEmitter;

	public Material cloudRingMaterial;

	public override void Init()
	{
		FillMaterials(wipe: false);
		BuildCloudRing();
	}

	public void FillMaterials(bool wipe)
	{
		if (!cloudRingMaterial || wipe)
		{
			cloudRingMaterial = MyDetonator().smokeBMaterial;
		}
	}

	public void BuildCloudRing()
	{
		_cloudRing = new GameObject("CloudRing");
		_cloudRingEmitter = _cloudRing.AddComponent<DetonatorBurstEmitter>();
		_cloudRing.transform.parent = base.transform;
		_cloudRing.transform.localPosition = localPosition;
		_cloudRingEmitter.material = cloudRingMaterial;
		_cloudRingEmitter.useExplicitColorAnimation = true;
	}

	public void UpdateCloudRing()
	{
		_cloudRing.transform.localPosition = Vector3.Scale(localPosition, new Vector3(size, size, size));
		_cloudRingEmitter.color = base.color;
		_cloudRingEmitter.duration = duration;
		_cloudRingEmitter.durationVariation = duration / 4f;
		_cloudRingEmitter.count = (int)(detail * 50f);
		_cloudRingEmitter.particleSize = 10f;
		_cloudRingEmitter.sizeVariation = 2f;
		_cloudRingEmitter.velocity = velocity;
		_cloudRingEmitter.startRadius = 3f;
		_cloudRingEmitter.size = size;
		_cloudRingEmitter.force = force;
		_cloudRingEmitter.explodeDelayMin = explodeDelayMin;
		_cloudRingEmitter.explodeDelayMax = explodeDelayMax;
		Color color = Color.Lerp(base.color, new Color(0.2f, 0.2f, 0.2f, 0.6f), 0.5f);
		Color color2 = new Color(0.2f, 0.2f, 0.2f, 0.5f);
		Color color3 = new Color(0.2f, 0.2f, 0.2f, 0.3f);
		Color color4 = new Color(0.2f, 0.2f, 0.2f, 0f);
		_cloudRingEmitter.colorAnimation[0] = color;
		_cloudRingEmitter.colorAnimation[1] = color2;
		_cloudRingEmitter.colorAnimation[2] = color2;
		_cloudRingEmitter.colorAnimation[3] = color3;
		_cloudRingEmitter.colorAnimation[4] = color4;
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
		velocity = _baseVelocity;
		force = _baseForce;
	}

	public override void Explode()
	{
		if (on)
		{
			UpdateCloudRing();
			_cloudRingEmitter.Explode();
		}
	}
}
