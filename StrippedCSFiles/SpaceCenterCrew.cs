using System.Runtime.CompilerServices;
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

	protected Vector3 target;

	protected Vector3 moveDirection;

	protected Vector3 velocity;

	protected CrewType cType;

	protected crewStates state;

	protected bool stateChanged;

	protected string[] crewAnimations;

	protected Animation _animation;

	protected Rigidbody _rigidbody;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SpaceCenterCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CrewMovement()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetAnimation()
	{
		throw null;
	}
}
