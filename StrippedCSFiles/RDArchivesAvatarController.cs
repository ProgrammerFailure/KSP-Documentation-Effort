using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RDArchivesAvatarController : MonoBehaviour
{
	public class AvatarState
	{
		public string name;

		public List<CharacterAnimationState> states;

		public List<string> comments;

		public Vector2 thoughtVector;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AvatarState(string name, Vector2 thoughtVector)
		{
			throw null;
		}
	}

	public KerbalInstructor avatar;

	public float rangeWeight;

	private float scoreRange;

	private string currentComment;

	private AvatarState idle;

	private AvatarState curious;

	private AvatarState halfway;

	private AvatarState satisfied;

	private AvatarState unsure;

	private List<AvatarState> states;

	public string Comment
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RDArchivesAvatarController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 getThoughtVector(float curiousness, float certainty)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 getThoughtVector(float score, float range, int samples)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string SetAvatarState(float score, float minScore, float maxScore, int samples)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string SetAvatarState(Vector2 thoughtVector)
	{
		throw null;
	}
}
