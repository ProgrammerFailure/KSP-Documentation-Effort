using UnityEngine;

public class SpaceCenterCrew : MonoBehaviour
{
	public enum CrewType
	{
		GroundCrew,
		Mechanic,
		Scientist
	}

	public enum crewStates
	{
		Idle,
		Walking,
		Running,
		Standing
	}

	public float speed;

	public Transform[] waypoints;

	public int currentwaypoint;

	public Vector3 target;

	public Vector3 moveDirection;

	public Vector3 velocity;

	public CrewType cType;

	public crewStates state;

	public bool stateChanged;

	public string[] crewAnimations;

	public Animation _animation;

	public Rigidbody _rigidbody;

	public void CrewMovement()
	{
		if (currentwaypoint >= waypoints.Length)
		{
			return;
		}
		target = waypoints[currentwaypoint].position;
		moveDirection = target - base.transform.position;
		velocity = this.GetComponentCached(ref _rigidbody).velocity;
		if (moveDirection.magnitude < 1f)
		{
			this.GetComponentCached(ref _rigidbody).velocity = Vector3.zero;
			if (cType == CrewType.Scientist)
			{
				base.gameObject.transform.LookAt(Vector3.zero);
			}
			currentwaypoint = Random.Range(0, waypoints.Length);
			state = crewStates.Standing;
			stateChanged = true;
		}
		else
		{
			velocity = moveDirection.normalized * speed;
			this.GetComponentCached(ref _rigidbody).velocity = velocity;
			base.transform.LookAt(target);
		}
	}

	public virtual void SetAnimation()
	{
	}
}
