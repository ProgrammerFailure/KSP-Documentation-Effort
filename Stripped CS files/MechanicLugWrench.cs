using UnityEngine;

public class MechanicLugWrench : SpaceCenterCrew
{
	public int lugState;

	public int loop;

	public void Start()
	{
		crewAnimations = new string[6] { "idle_right_lugDrill", "right_lug_a_start", "right_lug_a_mid", "right_lug_a_end", "wkC_lugDrill_right_forward01", "wkC_run01" };
		currentwaypoint = Random.Range(0, waypoints.Length);
		cType = CrewType.Mechanic;
		state = crewStates.Idle;
		this.GetComponentCached(ref _animation).Play(crewAnimations[0]);
		stateChanged = false;
		lugState = 0;
		loop = 0;
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
			if (this.GetComponentCached(ref _animation).isPlaying)
			{
				break;
			}
			stateChanged = true;
			if (lugState == 0)
			{
				lugState = 1;
			}
			else if (lugState == 1)
			{
				if (loop > 3)
				{
					lugState = 2;
					loop = 0;
				}
				else
				{
					this.GetComponentCached(ref _animation).CrossFade(crewAnimations[2]);
					loop++;
				}
			}
			else if (lugState == 2)
			{
				lugState = 0;
				state = crewStates.Idle;
			}
			break;
		}
	}

	public override void SetAnimation()
	{
		switch (state)
		{
		case crewStates.Idle:
			this.GetComponentCached(ref _animation).CrossFade(crewAnimations[0]);
			stateChanged = false;
			break;
		case crewStates.Walking:
			speed = 0.7f;
			this.GetComponentCached(ref _animation).CrossFadeQueued(crewAnimations[4], 0.2f, QueueMode.CompleteOthers);
			stateChanged = false;
			break;
		case crewStates.Running:
			speed = 1.6f;
			this.GetComponentCached(ref _animation).CrossFadeQueued(crewAnimations[5], 0.2f, QueueMode.CompleteOthers);
			stateChanged = false;
			break;
		case crewStates.Standing:
			if (lugState == 0)
			{
				this.GetComponentCached(ref _animation).CrossFade(crewAnimations[1]);
			}
			else if (lugState == 1)
			{
				this.GetComponentCached(ref _animation).CrossFade(crewAnimations[2]);
			}
			else
			{
				this.GetComponentCached(ref _animation).CrossFade(crewAnimations[3]);
			}
			stateChanged = false;
			break;
		}
	}
}
