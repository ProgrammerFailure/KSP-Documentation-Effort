using UnityEngine;

public class MechanicHammer : SpaceCenterCrew
{
	public int hitState;

	public int loop;

	public void Start()
	{
		crewAnimations = new string[10] { "idle", "idle_a", "idle_b", "idle_c", "idle_d", "right_hit_a_start", "right_hit_a_mid", "right_hit_a_end", "wkC_forward01", "wkC_run01" };
		currentwaypoint = Random.Range(0, waypoints.Length);
		cType = CrewType.Mechanic;
		state = crewStates.Idle;
		this.GetComponentCached(ref _animation).Play(crewAnimations[Random.Range(0, 5)]);
		stateChanged = false;
		hitState = 0;
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
			if (hitState == 0)
			{
				hitState = 1;
			}
			else if (hitState == 1)
			{
				if (loop > 3)
				{
					hitState = 2;
					loop = 0;
				}
				else
				{
					this.GetComponentCached(ref _animation).CrossFade(crewAnimations[6]);
					loop++;
				}
			}
			else if (hitState == 2)
			{
				hitState = 0;
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
			this.GetComponentCached(ref _animation).CrossFade(crewAnimations[Random.Range(0, 5)]);
			stateChanged = false;
			break;
		case crewStates.Walking:
			speed = 0.7f;
			this.GetComponentCached(ref _animation).CrossFadeQueued(crewAnimations[8], 0.2f, QueueMode.CompleteOthers);
			stateChanged = false;
			break;
		case crewStates.Running:
			speed = 1.6f;
			this.GetComponentCached(ref _animation).CrossFadeQueued(crewAnimations[9], 0.2f, QueueMode.CompleteOthers);
			stateChanged = false;
			break;
		case crewStates.Standing:
			if (hitState == 0)
			{
				this.GetComponentCached(ref _animation).CrossFade(crewAnimations[5]);
			}
			else if (hitState == 1)
			{
				this.GetComponentCached(ref _animation).CrossFade(crewAnimations[6]);
			}
			else
			{
				this.GetComponentCached(ref _animation).CrossFade(crewAnimations[7]);
			}
			stateChanged = false;
			break;
		}
	}
}
