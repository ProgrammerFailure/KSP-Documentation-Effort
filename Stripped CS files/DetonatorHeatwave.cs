using UnityEngine;

[RequireComponent(typeof(Detonator))]
[AddComponentMenu("Detonator/Heatwave (Pro Only)")]
public class DetonatorHeatwave : DetonatorComponent
{
	public GameObject _heatwave;

	public float s;

	public float _startSize;

	public float _maxSize;

	public float _baseDuration = 0.25f;

	public bool _delayedExplosionStarted;

	public float _explodeDelay;

	public float zOffset = 0.5f;

	public float distortion = 64f;

	public float _elapsedTime;

	public float _normalizedTime;

	public Material heatwaveMaterial;

	public Material _material;

	public override void Init()
	{
	}

	public void Update()
	{
		if (_delayedExplosionStarted)
		{
			_explodeDelay -= Time.deltaTime;
			if (_explodeDelay <= 0f)
			{
				Explode();
			}
		}
		if ((bool)_heatwave)
		{
			_heatwave.transform.rotation = Quaternion.FromToRotation(Vector3.up, Camera.main.transform.position - _heatwave.transform.position);
			_heatwave.transform.localPosition = localPosition + Vector3.forward * zOffset;
			_elapsedTime += Time.deltaTime;
			_normalizedTime = _elapsedTime / duration;
			s = Mathf.Lerp(_startSize, _maxSize, _normalizedTime);
			_heatwave.GetComponent<Renderer>().material.SetFloat("_BumpAmt", (1f - _normalizedTime) * distortion);
			_heatwave.gameObject.transform.localScale = new Vector3(s, s, s);
			if (_elapsedTime > duration)
			{
				Object.Destroy(_heatwave.gameObject);
			}
		}
	}

	public override void Explode()
	{
		if (!SystemInfo.supportsImageEffects || detailThreshold > detail || !on)
		{
			return;
		}
		if (!_delayedExplosionStarted)
		{
			_explodeDelay = explodeDelayMin + Random.value * (explodeDelayMax - explodeDelayMin);
		}
		if (_explodeDelay <= 0f)
		{
			_startSize = 0f;
			_maxSize = size * 10f;
			_material = new Material(Shader.Find("HeatDistort"));
			_heatwave = GameObject.CreatePrimitive(PrimitiveType.Plane);
			Object.Destroy(_heatwave.GetComponent(typeof(MeshCollider)));
			if (!heatwaveMaterial)
			{
				heatwaveMaterial = MyDetonator().heatwaveMaterial;
			}
			_material.CopyPropertiesFromMaterial(heatwaveMaterial);
			_heatwave.GetComponent<Renderer>().material = _material;
			_heatwave.transform.parent = base.transform;
			_delayedExplosionStarted = false;
			_explodeDelay = 0f;
		}
		else
		{
			_delayedExplosionStarted = true;
		}
	}

	public void Reset()
	{
		duration = _baseDuration;
	}
}
