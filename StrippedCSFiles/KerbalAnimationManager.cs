using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[Serializable]
public class KerbalAnimationManager
{
	public KerbalAnimationState idle;

	public KerbalAnimationState suspendedIdle;

	public KerbalAnimationState walkFwd;

	public KerbalAnimationState run;

	public KerbalAnimationState strafeLeft;

	public KerbalAnimationState strafeRight;

	public KerbalAnimationState walkBack;

	public KerbalAnimationState turnLeft;

	public KerbalAnimationState turnRight;

	public KerbalAnimationState packExtend;

	public KerbalAnimationState packStow;

	public KerbalAnimationState standUpFaceDown;

	public KerbalAnimationState standUpFaceUp;

	public KerbalAnimationState JumpFwdStart;

	public KerbalAnimationState JumpFwdEnd;

	public KerbalAnimationState JumpStillStart;

	public KerbalAnimationState JumpStillEnd;

	public KerbalAnimationState swimIdle;

	public KerbalAnimationState swimFwd;

	public KerbalAnimationState swimUpFaceDown;

	public KerbalAnimationState swimUpFaceUp;

	public KerbalAnimationState walkLowGee;

	public KerbalAnimationState walkLowGeeSuspendedLeft;

	public KerbalAnimationState walkLowGeeSuspendedRight;

	public KerbalAnimationState walkLowGeeLeft;

	public KerbalAnimationState walkLowGeeRight;

	public KerbalAnimationState ladderGrabGrounded;

	public KerbalAnimationState ladderGrabSuspended;

	public KerbalAnimationState ladderIdle;

	public KerbalAnimationState ladderClimb;

	public KerbalAnimationState ladderDescend;

	public KerbalAnimationState ladderPushOff;

	public KerbalAnimationState ladderRelease;

	public KerbalAnimationState ladderLeanFwd;

	public KerbalAnimationState ladderLeanBack;

	public KerbalAnimationState ladderLeanLeft;

	public KerbalAnimationState ladderLeanRight;

	public KerbalAnimationState flagPlant;

	public KerbalAnimationState seatIdle;

	public KerbalAnimationState clamber;

	public KerbalAnimationState chuteIdle;

	public KerbalAnimationState chuteLeanForward;

	public KerbalAnimationState chuteLeanBackward;

	public KerbalAnimationState chuteLeanLeft;

	public KerbalAnimationState chuteLeanRight;

	public KerbalAnimationState idle_a;

	public KerbalAnimationState idle_b;

	public KerbalAnimationState idle_c;

	public KerbalAnimationState idle_d;

	public KerbalAnimationState idle_f;

	public KerbalAnimationState idle_g;

	public KerbalAnimationState idle_i;

	public KerbalAnimationState controlpanel_a;

	public KerbalAnimationState controlpanel_b;

	public KerbalAnimationState controlpanel_c;

	public List<KerbalAnimationState> controlPanelAnims;

	public int controlPanelAnimSelector;

	public KerbalAnimationState rockSample;

	public KerbalAnimationState weld;

	public KerbalAnimationState weldGunDraw;

	public KerbalAnimationState weldGunPutAway;

	public KerbalAnimationState weldGunLift;

	public KerbalAnimationState weldSuspended;

	public KerbalAnimationState playingGolf;

	public KerbalAnimationState smashBanana;

	public KerbalAnimationState spinWingnut;

	public KerbalAnimationState weld_aim_up;

	public KerbalAnimationState weld_aim_down;

	public int[] syncLayers;

	public KerbalEVA evaController;

	public List<KerbalAnimationState> RandomIdleAnims;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalAnimationManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Start(KerbalEVA evaController)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void setupAnimation(string astName, KerbalAnimationState kas)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SyncAnimationLayers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual KerbalAnimationState[] GetAllAnimations()
	{
		throw null;
	}
}
