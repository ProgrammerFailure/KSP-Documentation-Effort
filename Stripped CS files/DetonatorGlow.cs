using UnityEngine;

[RequireComponent(typeof(Detonator))]
[AddComponentMenu("Detonator/Glow")]
public class DetonatorGlow : DetonatorComponent
{
	public float _baseSize = 1f;

	public float _baseDuration = 3f;

	public Vector3 _baseVelocity = new Vector3(0f, 0f, 0f);

	public Color _baseColor = Color.black;

	public float _scaledDuration;

	public GameObject _glow;

	public DetonatorBurstEmitter _glowEmitter;

	public Material glowMaterial;

	public override void Init()
	{
		FillMaterials(wipe: false);
		BuildGlow();
	}

	public void FillMaterials(bool wipe)
	{
		if (!glowMaterial || wipe)
		{
			glowMaterial = MyDetonator().glowMaterial;
		}
	}

	public void BuildGlow()
	{
		_glow = new GameObject("Glow");
		_glowEmitter = _glow.AddComponent<DetonatorBurstEmitter>();
		_glow.transform.parent = base.transform;
		_glow.transform.localPosition = localPosition;
		_glowEmitter.material = glowMaterial;
		_glowEmitter.exponentialGrowth = false;
		_glowEmitter.useExplicitColorAnimation = true;
		_glowEmitter.useWorldSpace = MyDetonator().useWorldSpace;
	}

	public void UpdateGlow()
	{
		_glow.transform.localPosition = Vector3.Scale(localPosition, new Vector3(size, size, size));
		_glowEmitter.color = base.color;
		_glowEmitter.duration = duration;
		_glowEmitter.timeScale = timeScale;
		_glowEmitter.count = 1f;
		_glowEmitter.particleSize = 65f;
		_glowEmitter.sizeVariation = 0f;
		_glowEmitter.velocity = new Vector3(0f, 0f, 0f);
		_glowEmitter.startRadius = 0f;
		_glowEmitter.sizeGrow = 0f;
		_glowEmitter.size = size;
		_glowEmitter.explodeDelayMin = explodeDelayMin;
		_glowEmitter.explodeDelayMax = explodeDelayMax;
		Color color = Color.Lerp(base.color, new Color(0.5f, 0.1f, 0.1f, 1f), 0.5f);
		color.a = 0.9f;
		Color color2 = Color.Lerp(base.color, new Color(0.6f, 0.3f, 0.3f, 1f), 0.5f);
		color2.a = 0.8f;
		Color color3 = Color.Lerp(base.color, new Color(0.7f, 0.3f, 0.3f, 1f), 0.5f);
		color3.a = 0.5f;
		Color color4 = Color.Lerp(base.color, new Color(0.4f, 0.3f, 0.4f, 1f), 0.5f);
		color4.a = 0.2f;
		Color color5 = new Color(0.1f, 0.1f, 0.4f, 0f);
		_glowEmitter.colorAnimation[0] = color;
		_glowEmitter.colorAnimation[1] = color2;
		_glowEmitter.colorAnimation[2] = color3;
		_glowEmitter.colorAnimation[3] = color4;
		_glowEmitter.colorAnimation[4] = color5;
	}

	public void Update()
	{
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
	}

	public override void Explode()
	{
		if (!(detailThreshold > detail) && on)
		{
			UpdateGlow();
			_glowEmitter.Explode();
		}
	}
}
