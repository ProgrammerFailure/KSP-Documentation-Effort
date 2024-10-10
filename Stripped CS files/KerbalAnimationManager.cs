using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

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

	public List<KerbalAnimationState> controlPanelAnims = new List<KerbalAnimationState>();

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

	public List<KerbalAnimationState> RandomIdleAnims = new List<KerbalAnimationState>();

	public virtual void Start(KerbalEVA evaController)
	{
		this.evaController = evaController;
		evaController.GetComponent<Animation>().wrapMode = WrapMode.Loop;
		KerbalAnimationState[] allAnimations = GetAllAnimations();
		int num = allAnimations.Length;
		for (int i = 0; i < num; i++)
		{
			KerbalAnimationState kerbalAnimationState = allAnimations[i];
			setupAnimation(kerbalAnimationState.animationName, kerbalAnimationState);
			if (kerbalAnimationState.hasJpVariant)
			{
				setupAnimation("jp_" + kerbalAnimationState.animationName, kerbalAnimationState);
			}
			kerbalAnimationState.SetManager(this);
		}
		SyncAnimationLayers();
		RandomIdleAnims.Add(idle_a);
		RandomIdleAnims.Add(idle_b);
		RandomIdleAnims.Add(idle_c);
		RandomIdleAnims.Add(idle_d);
		RandomIdleAnims.Add(idle_f);
		RandomIdleAnims.Add(idle_g);
		RandomIdleAnims.Add(idle_i);
		controlPanelAnims.Add(controlpanel_a);
		controlPanelAnims.Add(controlpanel_b);
		controlPanelAnims.Add(controlpanel_c);
	}

	public virtual void setupAnimation(string astName, KerbalAnimationState kas)
	{
		AnimationState animationState = evaController.GetComponent<Animation>()[astName];
		if (animationState == null)
		{
			Debug.LogWarning("[KEVA Animation Error]: Animation state " + astName + ", " + kas.animationName + " not found in animation component");
			return;
		}
		animationState.layer = kas.layer;
		animationState.wrapMode = kas.wrapMode;
		animationState.blendMode = kas.blendMode;
		animationState.weight = kas.weight;
		int num = kas.addMixingTransforms.Length;
		for (int i = 0; i < num; i++)
		{
			animationState.AddMixingTransform(kas.addMixingTransforms[i], kas.addRecursive);
		}
		num = kas.specificMixingTransforms.Length;
		for (int j = 0; j < num; j++)
		{
			animationState.AddMixingTransform(kas.specificMixingTransforms[j].transform, kas.specificMixingTransforms[j].isRecursive);
		}
		num = kas.excludeMixingTransforms.Length;
		for (int k = 0; k < num; k++)
		{
			animationState.RemoveMixingTransform(kas.excludeMixingTransforms[k]);
		}
	}

	public virtual void SyncAnimationLayers()
	{
		int num = syncLayers.Length;
		while (num-- > 0)
		{
			evaController.GetComponent<Animation>().SyncLayer(syncLayers[num]);
		}
	}

	public virtual KerbalAnimationState[] GetAllAnimations()
	{
		List<KerbalAnimationState> list = new List<KerbalAnimationState>();
		FieldInfo[] fields = GetType().GetFields();
		int num = fields.Length;
		for (int i = 0; i < num; i++)
		{
			FieldInfo fieldInfo = fields[i];
			if (!(fieldInfo.FieldType != typeof(KerbalAnimationState)))
			{
				list.Add((KerbalAnimationState)GetType().InvokeMember(fieldInfo.Name, BindingFlags.GetField, null, this, new object[0]));
			}
		}
		return list.ToArray();
	}
}
