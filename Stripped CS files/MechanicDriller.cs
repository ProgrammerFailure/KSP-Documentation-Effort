using UnityEngine;

public class MechanicDriller : SpaceCenterCrew
{
	public void Start()
	{
		crewAnimations = new string[8] { "idle", "idle_a", "idle_b", "idle_c", "idle_d", "left_drill_a", "wkC_lugDrill_right_forward01", "wkC_run01" };
		currentwaypoint = Random.Range(0, waypoints.Length);
		cType = CrewType.Mechanic;
		state = crewStates.Idle;
		this.GetComponentCached(ref _animation).Play(crewAnimations[Random.Range(0, 5)]);
		stateChanged = false;
	}

	public void Update()
	{
		if (stateChanged)
		{
			SetAnimation();
		}
		switch (state)
		{
		case crewStates.Idle:
			if (!this.GetComponentCached(ref _animation).isPlaying)
			{
				if (Random.Range(0, 2) == 0)
				{
					state = crewStates.Walking;
				}
				else
				{
					state = crewStates.Running;
				}
				stateChanged = true;
			}
			break;
		case crewStates.Walking:
			if (waypoints.Length != 0)
			{
				CrewMovement();
				break;
			}
			state = crewStates.Standing;
			stateChanged = true;
			break;
		case crewStates.Running:
			if (waypoints.Length != 0)
			{
				CrewMovement();
				break;
			}
			state = crewStates.Standing;
			stateChanged = true;
			break;
		case crewStates.Standing:
			if (!this.GetComponentCached(ref _animation).isPlaying)
			{
				state = crewStates.Idle;
				stateChanged = true;
			}
			break;
		}
	}

	public override void SetAnimation()
	{
		switch (state)
		{
		case crewStates.Idle:
			this.GetComponentCached(ref _animation).CrossFade(crewAnimations[Random.Range(0, 5)]);
			stateChanged = false;
			break;
		case crewStates.Walking:
			speed = 0.7f;
			this.GetComponentCached(ref _animation).CrossFadeQueued(crewAnimations[6], 0.2f, QueueMode.CompleteOthers);
			stateChanged = false;
			break;
		case crewStates.Running:
			speed = 1.6f;
			this.GetComponentCached(ref _animation).CrossFadeQueued(crewAnimations[7], 0.2f, QueueMode.CompleteOthers);
			stateChanged = false;
			break;
		case crewStates.Standing:
			this.GetComponentCached(ref _animation).CrossFade(crewAnimations[5]);
			stateChanged = false;
			break;
		}
	}
}
