using UnityEngine;

[AddComponentMenu("Detonator/Force")]
[RequireComponent(typeof(Detonator))]
public class DetonatorForce : DetonatorComponent
{
	public float _baseRadius = 50f;

	public float _basePower = 4000f;

	public float _scaledRange;

	public float _scaledIntensity;

	public bool _delayedExplosionStarted;

	public float _explodeDelay;

	public float radius;

	public float power;

	public GameObject fireObject;

	public float fireObjectLife;

	public Collider[] _colliders;

	public GameObject _tempFireObject;

	public Vector3 _explosionPosition;

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
	}

	public override void Explode()
	{
		if (!on || detailThreshold > detail)
		{
			return;
		}
		if (!_delayedExplosionStarted)
		{
			_explodeDelay = explodeDelayMin + Random.value * (explodeDelayMax - explodeDelayMin);
		}
		if (_explodeDelay <= 0f)
		{
			_explosionPosition = base.transform.position;
			_colliders = Physics.OverlapSphere(_explosionPosition, radius);
			Collider[] colliders = _colliders;
			int num = 0;
			while (true)
			{
				if (num < colliders.Length)
				{
					Collider collider = colliders[num];
					if ((bool)collider && (bool)collider.GetComponent<Rigidbody>())
					{
						collider.GetComponent<Rigidbody>().AddExplosionForce(power * size, _explosionPosition, radius * size, 4f * MyDetonator().upwardsBias * size);
						SendMessage("OnDetonatorForceHit", null, SendMessageOptions.DontRequireReceiver);
						if ((bool)fireObject)
						{
							if ((bool)collider.transform.Find(fireObject.name + "(Clone)"))
							{
								break;
							}
							_tempFireObject = Object.Instantiate(fireObject, base.transform.position, base.transform.rotation);
							_tempFireObject.transform.parent = collider.transform;
							_tempFireObject.transform.localPosition = new Vector3(0f, 0f, 0f);
							if ((bool)_tempFireObject.GetComponent<ParticleSystem>())
							{
								_tempFireObject.GetComponent<ParticleSystem>().Play();
								Object.Destroy(_tempFireObject, fireObjectLife);
							}
						}
					}
					num++;
					continue;
				}
				_delayedExplosionStarted = false;
				_explodeDelay = 0f;
				break;
			}
		}
		else
		{
			_delayedExplosionStarted = true;
		}
	}

	public void Reset()
	{
		radius = _baseRadius;
		power = _basePower;
	}
}
