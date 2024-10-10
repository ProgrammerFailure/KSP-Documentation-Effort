using UnityEngine;

public class GroundCrew : SpaceCenterCrew
{
	public int loop;

	public int currentAnimation;

	public void Start()
	{
		crewAnimations = new string[19]
		{
			"idle_a", "idle_b", "idle_c", "idle_d", "idle_e", "idle_f", "idle_g", "idle_h", "animA", "animB",
			"animC", "animD", "animE", "animF", "animG", "animH", "animI", "wkC_casual", "wkC_run"
		};
		currentwaypoint = Random.Range(0, waypoints.Length);
		cType = CrewType.GroundCrew;
		state = crewStates.Idle;
		this.GetComponentCached(ref _animation).Play(crewAnimations[Random.Range(0, 8)]);
		stateChanged = false;
		loop = 0;
		currentAnimation = 0;
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
				state = crewStates.Standing;
				stateChanged = true;
			}
			break;
		case crewStates.Standing:
			if (!this.GetComponentCached(ref _animation).isPlaying)
			{
				if (loop > 2)
				{
					state = crewStates.Idle;
					stateChanged = true;
					loop = 0;
				}
				else
				{
					this.GetComponentCached(ref _animation).CrossFade(crewAnimations[currentAnimation]);
					loop++;
				}
			}
			break;
		case crewStates.Walking:
		case crewStates.Running:
			break;
		}
	}

	public override void SetAnimation()
	{
		switch (state)
		{
		case crewStates.Idle:
			this.GetComponentCached(ref _animation).CrossFade(crewAnimations[Random.Range(0, 8)]);
			stateChanged = false;
			break;
		case crewStates.Walking:
			speed = 0.7f;
			this.GetComponentCached(ref _animation).CrossFadeQueued(crewAnimations[17], 0.2f, QueueMode.CompleteOthers);
			stateChanged = false;
			break;
		case crewStates.Running:
			speed = 1.6f;
			this.GetComponentCached(ref _animation).CrossFadeQueued(crewAnimations[18], 0.2f, QueueMode.CompleteOthers);
			stateChanged = false;
			break;
		case crewStates.Standing:
			currentAnimation = Random.Range(8, 17);
			this.GetComponentCached(ref _animation).CrossFade(crewAnimations[currentAnimation]);
			stateChanged = false;
			break;
		}
	}
}
