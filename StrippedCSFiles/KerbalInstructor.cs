using System.Runtime.CompilerServices;

public class KerbalInstructor : KerbalInstructorBase
{
	public CharacterAnimationState anim_idle;

	public CharacterAnimationState anim_idle_lookAround;

	public CharacterAnimationState anim_idle_sigh;

	public CharacterAnimationState anim_idle_wonder;

	public CharacterAnimationState anim_true_thumbUp;

	public CharacterAnimationState anim_true_thumbsUp;

	public CharacterAnimationState anim_true_nodA;

	public CharacterAnimationState anim_true_nodB;

	public CharacterAnimationState anim_true_smileA;

	public CharacterAnimationState anim_true_smileB;

	public CharacterAnimationState anim_false_disappointed;

	public CharacterAnimationState anim_false_disagreeA;

	public CharacterAnimationState anim_false_disagreeB;

	public CharacterAnimationState anim_false_disagreeC;

	public CharacterAnimationState anim_false_sadA;

	private float rptInterval;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalInstructor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private new void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PlayEmote(CharacterAnimationState st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PlayEmoteRepeating(CharacterAnimationState st, float repeatInterval)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StopRepeatingEmote()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RepeatEmote()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PlayEmoteQueued(CharacterAnimationState st, CharacterAnimationState fallbackAnim)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PlayEmote(CharacterAnimationState st, CharacterAnimationState fallbackAnim, bool playSound = true)
	{
		throw null;
	}
}
