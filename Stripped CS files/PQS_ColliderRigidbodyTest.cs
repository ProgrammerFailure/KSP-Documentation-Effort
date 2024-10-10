using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PQS_ColliderRigidbodyTest : MonoBehaviour
{
	public float force;

	public GClass4 rootSphere;

	public float sphereGravity;

	public Vector3 initPos;

	public Quaternion initRot;

	public Rigidbody _rigidbody;

	public float fps;

	public void Reset()
	{
		force = 100f;
		sphereGravity = 0f - (float)PhysicsGlobals.GravitationalAcceleration;
	}

	public void Start()
	{
		this.GetComponentCached(ref _rigidbody).useGravity = false;
		initPos = base.transform.position;
		initRot = base.transform.rotation;
	}

	public void FixedUpdate()
	{
		if (Input.GetMouseButton(0))
		{
			Vector3 normalized = (base.transform.position - Camera.main.transform.position).normalized;
			this.GetComponentCached(ref _rigidbody).AddForce(normalized * force);
		}
		if (Input.GetMouseButton(2))
		{
			this.GetComponentCached(ref _rigidbody).velocity = Vector3.zero;
			this.GetComponentCached(ref _rigidbody).angularVelocity = Vector3.zero;
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			base.transform.position = initPos;
			base.transform.rotation = initRot;
			this.GetComponentCached(ref _rigidbody).velocity = Vector3.zero;
			this.GetComponentCached(ref _rigidbody).angularVelocity = Vector3.zero;
		}
		this.GetComponentCached(ref _rigidbody).AddForce(sphereGravity * (base.transform.position - rootSphere.transform.position).normalized);
	}

	public void Update()
	{
		fps = Mathf.Lerp(fps, 1f / Time.deltaTime, 10f * Time.deltaTime);
	}
}
