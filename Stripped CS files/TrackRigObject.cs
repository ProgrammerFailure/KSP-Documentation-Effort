using UnityEngine;

public class TrackRigObject : MonoBehaviour
{
	public enum TrackMode
	{
		FixedUpdate,
		Update,
		LateUpdate
	}

	public Transform target;

	public bool keepInitialOffset;

	public bool initialized;

	public TrackMode trackingMode = TrackMode.Update;

	public Transform trf;

	public Vector3 pOff;

	public Quaternion rOff;

	public void Awake()
	{
		Initialize();
	}

	[ContextMenu("Initialize")]
	public void Initialize()
	{
		if (!(target == null))
		{
			trf = base.transform;
			pOff = trf.position - target.position;
			rOff = Quaternion.Inverse(target.rotation) * trf.rotation;
			initialized = true;
		}
	}

	public void FixedUpdate()
	{
		if (trackingMode == TrackMode.FixedUpdate)
		{
			Track();
		}
	}

	public void Update()
	{
		if (trackingMode == TrackMode.Update)
		{
			Track();
		}
	}

	public void LateUpdate()
	{
		if (trackingMode == TrackMode.LateUpdate)
		{
			Track();
		}
	}

	public void Track()
	{
		if (!initialized)
		{
			Initialize();
		}
		if (!(target == null))
		{
			if (keepInitialOffset)
			{
				trf.rotation = target.rotation * rOff;
				trf.position = target.position + trf.rotation * pOff;
			}
			else
			{
				trf.position = target.position;
				trf.rotation = target.rotation;
			}
		}
	}
}
