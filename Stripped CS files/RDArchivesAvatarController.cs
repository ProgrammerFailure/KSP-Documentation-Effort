using System.Collections.Generic;
using ns9;
using UnityEngine;

public class RDArchivesAvatarController : MonoBehaviour
{
	public class AvatarState
	{
		public string name;

		public List<CharacterAnimationState> states;

		public List<string> comments;

		public Vector2 thoughtVector;

		public AvatarState(string name, Vector2 thoughtVector)
		{
			this.name = name;
			states = new List<CharacterAnimationState>();
			comments = new List<string>();
			this.thoughtVector = thoughtVector;
		}
	}

	public KerbalInstructor avatar;

	public float rangeWeight = 1f;

	public float scoreRange = 0.5f;

	public string currentComment;

	public AvatarState idle;

	public AvatarState curious;

	public AvatarState halfway;

	public AvatarState satisfied;

	public AvatarState unsure;

	public List<AvatarState> states;

	public string Comment => currentComment;

	public void Start()
	{
		states = new List<AvatarState>();
		idle = new AvatarState("idle", getThoughtVector(0f, -1f));
		idle.states.Add(avatar.anim_idle);
		idle.comments.Add(Localizer.Format("#autoLOC_417557"));
		states.Add(idle);
		curious = new AvatarState("curious", getThoughtVector(-1f, 0f));
		curious.states.Add(avatar.anim_true_smileB);
		curious.comments.Add(Localizer.Format("#autoLOC_417563"));
		states.Add(curious);
		halfway = new AvatarState("half-way", getThoughtVector(0f, 0f));
		halfway.states.Add(avatar.anim_true_nodA);
		halfway.comments.Add(Localizer.Format("#autoLOC_417568"));
		states.Add(halfway);
		satisfied = new AvatarState("satisfied", getThoughtVector(1f, 0f));
		satisfied.states.Add(avatar.anim_true_smileA);
		satisfied.comments.Add(Localizer.Format("#autoLOC_417573"));
		states.Add(satisfied);
		unsure = new AvatarState("unsure", getThoughtVector(0f, 1f));
		unsure.states.Add(avatar.anim_idle_lookAround);
		unsure.comments.Add(Localizer.Format("#autoLOC_417579"));
		states.Add(unsure);
	}

	public Vector2 getThoughtVector(float curiousness, float certainty)
	{
		return new Vector2(curiousness, certainty);
	}

	public Vector2 getThoughtVector(float score, float range, int samples)
	{
		float num = score * 2f - 1f;
		float num2 = ((samples >= 1) ? range : (-1f));
		return new Vector2(num * (1f - Mathf.Abs(num2) * rangeWeight), num2);
	}

	public string SetAvatarState(float score, float minScore, float maxScore, int samples)
	{
		scoreRange = maxScore - minScore;
		Vector2 thoughtVector = getThoughtVector(score, scoreRange, samples);
		return SetAvatarState(thoughtVector);
	}

	public string SetAvatarState(Vector2 thoughtVector)
	{
		AvatarState avatarState = states[0];
		float num = float.MaxValue;
		int i = 0;
		for (int count = states.Count; i < count; i++)
		{
			float sqrMagnitude = (states[i].thoughtVector - thoughtVector).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				avatarState = states[i];
				num = sqrMagnitude;
			}
		}
		avatar.PlayEmote(avatarState.states[Random.Range(0, avatarState.states.Count)]);
		currentComment = avatarState.comments[Random.Range(0, avatarState.comments.Count)];
		return currentComment;
	}
}
