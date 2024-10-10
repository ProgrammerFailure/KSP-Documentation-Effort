using UnityEngine;

public class Scientist : SpaceCenterCrew
{
	public void Start()
	{
		crewAnimations = new string[16]
		{
			"idle", "idle_b", "idle_c", "idle_d", "idle_e", "idle_f", "idle_g", "notes_a", "notes_b", "notes_c",
			"notes_d", "reading_a", "reading_b", "reading_c", "wkC_forward01", "wkC_run01"
		};
		currentwaypoint = Random.Range(0, waypoints.Length);
		cType = CrewType.Scientist;
		state = crewStates.Idle;
		this.GetComponentCached(ref _animation).Play(crewAnimations[Random.Range(0, 7)]);
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
				state = crewStates.Walking;
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
		case crewStates.Standing:
			if (!this.GetComponentCached(ref _animation).isPlaying)
			{
				state = crewStates.Idle;
				stateChanged = true;
			}
			break;
		case crewStates.Running:
			break;
		}
	}

	public override void SetAnimation()
	{
		switch (state)
		{
		case crewStates.Idle:
			this.GetComponentCached(ref _animation).CrossFade(crewAnimations[Random.Range(0, 7)]);
			stateChanged = false;
			break;
		case crewStates.Walking:
			speed = 0.7f;
			this.GetComponentCached(ref _animation).CrossFadeQueued(crewAnimations[14], 0.2f, QueueMode.CompleteOthers);
			stateChanged = false;
			break;
		case crewStates.Running:
			speed = 1.6f;
			this.GetComponentCached(ref _animation).CrossFadeQueued(crewAnimations[15], 0.2f, QueueMode.CompleteOthers);
			stateChanged = false;
			break;
		case crewStates.Standing:
			this.GetComponentCached(ref _animation).CrossFade(crewAnimations[Random.Range(7, 14)]);
			stateChanged = false;
			break;
		}
	}
}
