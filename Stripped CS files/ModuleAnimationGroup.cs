using System;
using System.Collections.Generic;
using ns9;
using UnityEngine;

public class ModuleAnimationGroup : PartModule, IMultipleDragCube
{
	public List<IAnimatedModule> _Modules;

	[KSPField(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001352")]
	public string animationStatus = "";

	[KSPField]
	public string activeAnimationName = "";

	[KSPField]
	public bool alwaysActive;

	[KSPField]
	public bool autoDeploy;

	[KSPField]
	public bool suppressActionsEvents;

	[KSPField]
	public bool stopActiveAnimIfDisabled;

	[KSPField]
	public bool displayActions = true;

	[KSPField(isPersistant = true)]
	public bool isDeployed;

	[KSPField]
	public string deactivateAnimationName = "";

	[KSPField]
	public string deployAnimationName = "";

	[KSPField]
	public string deployActionName = "#autoLOC_6001333";

	[KSPField]
	public string retractActionName = "#autoLOC_6001339";

	[KSPField]
	public string toggleActionName = "#autoLOC_6001329";

	[KSPField]
	public string moduleType = "#autoLOC_6002383";

	[KSPField]
	public string deployEffectName = "Deploy";

	[KSPField]
	public string activeEffectName = "Active";

	[KSPField]
	public string retractEffectName = "Retract";

	[KSPField]
	public bool realTimeAnimation;

	public float animationStartTime;

	public float pauseStartTime;

	public float savedAnimationSpeed;

	public string savedAnimationName;

	public bool _lateSetupComplete;

	public bool _animationLock;

	public Action _postAnimationMethod;

	public static string cacheAutoLOC_215840;

	public Animation DeployAnimation { get; set; }

	public Animation ActiveAnimation { get; set; }

	public Animation DeactivateAnimation { get; set; }

	public bool IsMultipleCubesActive => true;

	[KSPEvent(unfocusedRange = 3f, active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001333")]
	public void DeployModule()
	{
		base.part.Effect(deployEffectName, 1f);
		SetDeployedState(1, resetAnimationTime: true);
	}

	[KSPEvent(unfocusedRange = 3f, active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001339")]
	public void RetractModule()
	{
		base.part.Effect(activeEffectName, 0f);
		base.part.Effect(retractEffectName, 1f);
		SetRetractedState(-1, resetAnimationTime: true);
	}

	[KSPAction("#autoLOC_6001333")]
	public void DeployModuleAction(KSPActionParam param)
	{
		if (!isDeployed)
		{
			DeployModule();
		}
	}

	[KSPAction("#autoLOC_6001339")]
	public void RetractModuleAction(KSPActionParam param)
	{
		if (isDeployed)
		{
			RetractModule();
		}
	}

	[KSPAction("#autoLOC_6001329")]
	public void ToggleModuleAction(KSPActionParam param)
	{
		if (isDeployed)
		{
			RetractModule();
		}
		else
		{
			DeployModule();
		}
	}

	public virtual void Start()
	{
		FindModules();
		FindAnimations();
		StopAnimations();
		CheckAnimationState();
		if (deployAnimationName != string.Empty)
		{
			DeployAnimation[deployAnimationName].layer = 3;
		}
		if (activeAnimationName != string.Empty)
		{
			ActiveAnimation[activeAnimationName].layer = 4;
		}
		if (deactivateAnimationName != string.Empty)
		{
			DeactivateAnimation[deactivateAnimationName].layer = 4;
		}
		if (autoDeploy || suppressActionsEvents)
		{
			base.Events["DeployModule"].active = false;
			base.Events["RetractModule"].active = false;
			base.Actions["DeployModuleAction"].active = false;
			base.Actions["RetractModuleAction"].active = false;
			base.Actions["ToggleModuleAction"].active = false;
			if (autoDeploy)
			{
				isDeployed = true;
				suppressActionsEvents = true;
			}
		}
		if (!displayActions)
		{
			base.Actions["DeployModuleAction"].active = false;
			base.Actions["RetractModuleAction"].active = false;
			base.Actions["ToggleModuleAction"].active = false;
		}
		Setup();
	}

	public override void OnStart(StartState state)
	{
		base.OnStart(state);
		GameEvents.onGamePause.Add(OnPause);
		GameEvents.onGameUnpause.Add(OnUnpause);
	}

	public override void OnInventoryModeDisable()
	{
		base.OnInventoryModeDisable();
		if (HighLogic.LoadedSceneIsFlight && !isDeployed)
		{
			SetRetractedState(-10000, resetAnimationTime: true);
		}
		if (base.part.protoPartSnapshot != null)
		{
			ProtoPartModuleSnapshot protoPartModuleSnapshot = base.part.protoPartSnapshot.FindModule("ModuleAnimationGroup");
			if (protoPartModuleSnapshot != null && protoPartModuleSnapshot.moduleValues != null)
			{
				protoPartModuleSnapshot.moduleValues.SetValue("isDeployed", isDeployed.ToString());
			}
		}
	}

	public override void OnStoredInInventory(ModuleInventoryPart moduleInventoryPart)
	{
		base.OnStoredInInventory(moduleInventoryPart);
		if (base.vessel == null)
		{
			SetRetractedState(-10000, resetAnimationTime: true);
		}
		isDeployed = false;
	}

	public virtual void OnDestroy()
	{
		GameEvents.onGamePause.Remove(OnPause);
		GameEvents.onGameUnpause.Remove(OnUnpause);
	}

	public void FindAnimations()
	{
		if (DeployAnimation == null)
		{
			DeployAnimation = ((deployAnimationName == string.Empty) ? null : base.part.FindModelAnimators(deployAnimationName)[0]);
		}
		if (ActiveAnimation == null)
		{
			ActiveAnimation = ((activeAnimationName == string.Empty) ? null : base.part.FindModelAnimators(activeAnimationName)[0]);
		}
		if (DeactivateAnimation == null)
		{
			DeactivateAnimation = ((deactivateAnimationName == string.Empty) ? null : base.part.FindModelAnimators(deactivateAnimationName)[0]);
		}
	}

	public void StopAnimations()
	{
		Animation[] array = base.part.FindModelAnimators();
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			array[i].Stop();
		}
	}

	public void LateUpdate()
	{
		if (!_lateSetupComplete)
		{
			LateSetup();
		}
	}

	public void LateSetup()
	{
		_lateSetupComplete = true;
		if (isDeployed)
		{
			EnableModules();
		}
		else
		{
			DisableModules();
		}
		MonoUtilities.RefreshPartContextWindow(base.part);
	}

	public void Setup()
	{
		base.Events["DeployModule"].guiName = Localizer.Format(deployActionName, moduleType);
		base.Events["RetractModule"].guiName = Localizer.Format(retractActionName, moduleType);
		base.Actions["DeployModuleAction"].guiName = Localizer.Format(deployActionName, moduleType);
		base.Actions["RetractModuleAction"].guiName = Localizer.Format(retractActionName, moduleType);
		base.Actions["ToggleModuleAction"].guiName = Localizer.Format(toggleActionName, moduleType);
	}

	public override void OnLoad(ConfigNode node)
	{
		FindModules();
		FindAnimations();
		CheckAnimationState();
	}

	public void Update()
	{
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return;
		}
		try
		{
			if (base.vessel != null)
			{
				if (_animationLock)
				{
					if (!DeployAnimation.IsPlaying(deployAnimationName))
					{
						_animationLock = false;
						base.Fields["animationStatus"].guiActive = false;
						_postAnimationMethod();
						GameEvents.OnAnimationGroupRetractComplete.Fire(this);
					}
				}
				else if (isDeployed && base.part != null && base.part.partInfo != null && base.part.partInfo.name != "groundAnchor")
				{
					CheckForActivity();
				}
				UpdateAnimationTime();
			}
			base.OnUpdate();
		}
		catch (Exception)
		{
			MonoBehaviour.print("[RESOURCES] - ERROR IN OnUpdate of ModuleAnimationGroup");
		}
	}

	public virtual void UpdateAnimationTime()
	{
		if (!realTimeAnimation || !(Time.unscaledDeltaTime > Time.deltaTime))
		{
			return;
		}
		AnimationState currentPlayingAnimationState = GetCurrentPlayingAnimationState();
		if (currentPlayingAnimationState != null)
		{
			if (currentPlayingAnimationState.speed > 0f)
			{
				currentPlayingAnimationState.time = (Time.unscaledTime - animationStartTime) / currentPlayingAnimationState.speed;
			}
			else if (currentPlayingAnimationState.speed < 0f)
			{
				currentPlayingAnimationState.time = currentPlayingAnimationState.length + (Time.unscaledTime - animationStartTime) / currentPlayingAnimationState.speed;
			}
		}
	}

	public virtual AnimationState GetCurrentPlayingAnimationState()
	{
		AnimationState result = null;
		if (DeployAnimation != null && savedAnimationName == deployAnimationName)
		{
			result = DeployAnimation[deployAnimationName];
		}
		if (DeactivateAnimation != null && savedAnimationName == deactivateAnimationName)
		{
			result = DeactivateAnimation[deactivateAnimationName];
		}
		if (ActiveAnimation != null && savedAnimationName == activeAnimationName)
		{
			result = ActiveAnimation[activeAnimationName];
		}
		return result;
	}

	public virtual void OnPause()
	{
		if (realTimeAnimation)
		{
			pauseStartTime = Time.unscaledTime;
			AnimationState currentPlayingAnimationState = GetCurrentPlayingAnimationState();
			if (currentPlayingAnimationState != null)
			{
				currentPlayingAnimationState.speed = 0f;
			}
		}
	}

	public virtual void OnUnpause()
	{
		if (realTimeAnimation)
		{
			animationStartTime += Time.unscaledTime - pauseStartTime;
			AnimationState currentPlayingAnimationState = GetCurrentPlayingAnimationState();
			if (currentPlayingAnimationState != null)
			{
				currentPlayingAnimationState.speed = savedAnimationSpeed;
			}
		}
	}

	public void CheckAnimationState()
	{
		if (isDeployed)
		{
			SetDeployedState(1000, resetAnimationTime: false);
		}
		else
		{
			SetRetractedState(-1000, resetAnimationTime: false);
		}
		ToggleEmmitters(state: false);
	}

	public void FindModules()
	{
		if (base.vessel != null)
		{
			_Modules = base.part.FindModulesImplementing<IAnimatedModule>();
		}
	}

	public void CheckForActivity()
	{
		int count = _Modules.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				if ((_Modules[num].ModuleIsActive() || alwaysActive) && isDeployed)
				{
					break;
				}
				num++;
				continue;
			}
			ToggleActiveState(-1, state: false);
			return;
		}
		ToggleActiveState(1, state: true);
	}

	public void ToggleActiveState(int speed, bool state)
	{
		if (HighLogic.LoadedSceneIsEditor)
		{
			speed *= 1000;
		}
		try
		{
			if (activeAnimationName != string.Empty && !ActiveAnimation.isPlaying && state)
			{
				base.part.Effect(activeEffectName, 1f);
				savedAnimationSpeed = speed;
				animationStartTime = Time.unscaledTime;
				savedAnimationName = activeAnimationName;
				ActiveAnimation[activeAnimationName].speed = speed;
				ActiveAnimation.Play(activeAnimationName);
				if (speed > 0)
				{
					SetDragCubes("ACTIVE_A");
				}
				else
				{
					SetDragCubes("ACTIVE_B");
				}
				ToggleEmmitters(state);
			}
			else if (deactivateAnimationName != string.Empty && !DeactivateAnimation.isPlaying && !state)
			{
				savedAnimationSpeed = speed;
				animationStartTime = Time.unscaledTime;
				savedAnimationName = deactivateAnimationName;
				DeactivateAnimation[deactivateAnimationName].speed = speed;
				DeactivateAnimation.Play(deactivateAnimationName);
				if (speed > 0)
				{
					SetDragCubes("DEACTIVATE_A");
				}
				else
				{
					SetDragCubes("DEACTIVATE_B");
				}
				ToggleEmmitters(state);
			}
			else
			{
				ToggleEmmitters(state);
			}
			if (activeAnimationName != string.Empty && stopActiveAnimIfDisabled && ActiveAnimation.isPlaying && !state)
			{
				base.part.Effect(activeEffectName, 0f);
				ActiveAnimation.Stop();
			}
		}
		catch (Exception)
		{
			MonoBehaviour.print("[RESOURCES] ERROR in ToggleActiveState of ModuleAnimationGroup");
		}
	}

	public void ToggleEmmitters(bool state)
	{
		KSPParticleEmitter[] componentsInChildren = base.part.GetComponentsInChildren<KSPParticleEmitter>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			componentsInChildren[i].emit = state;
			componentsInChildren[i].enabled = state;
		}
	}

	public void SetRetractedState(int speed, bool resetAnimationTime)
	{
		if (HighLogic.LoadedSceneIsEditor)
		{
			speed *= 1000;
		}
		isDeployed = false;
		base.Events["RetractModule"].active = false;
		DisableModules();
		GameEvents.OnAnimationGroupStateChanged.Fire(this, data1: false);
		MonoUtilities.RefreshPartContextWindow(base.part);
		if (deployAnimationName != string.Empty)
		{
			PlayDeployAnimation(speed, RetractComplete, resetAnimationTime);
			SetDragCubes("DEPLOY_A");
		}
		else
		{
			RetractComplete();
		}
	}

	public void DeployComplete()
	{
		if (!suppressActionsEvents)
		{
			base.Events["RetractModule"].active = true;
		}
		EnableModules();
		GameEvents.OnAnimationGroupStateChanged.Fire(this, data1: true);
		MonoUtilities.RefreshPartContextWindow(base.part);
	}

	public void RetractComplete()
	{
		if (!suppressActionsEvents)
		{
			base.Events["DeployModule"].active = true;
		}
	}

	public void SetDeployedState(int speed, bool resetAnimationTime)
	{
		if (HighLogic.LoadedSceneIsEditor)
		{
			speed *= 1000;
		}
		isDeployed = true;
		base.Events["DeployModule"].active = false;
		if (deployAnimationName != string.Empty)
		{
			PlayDeployAnimation(speed, DeployComplete, resetAnimationTime);
			SetDragCubes("DEPLOY_B");
		}
		else
		{
			DeployComplete();
		}
	}

	public void SetDragCubes(string cube)
	{
		if (!string.IsNullOrEmpty(deployAnimationName))
		{
			base.part.DragCubes.SetCubeWeight("DEPLOY_A", 0f);
			base.part.DragCubes.SetCubeWeight("DEPLOY_B", 0f);
		}
		if (!string.IsNullOrEmpty(deactivateAnimationName))
		{
			base.part.DragCubes.SetCubeWeight("DEACTIVATE_A", 0f);
			base.part.DragCubes.SetCubeWeight("DEACTIVATE_B", 0f);
		}
		if (!string.IsNullOrEmpty(activeAnimationName))
		{
			base.part.DragCubes.SetCubeWeight("ACTIVE_A", 0f);
			base.part.DragCubes.SetCubeWeight("ACTIVE_B", 0f);
		}
		base.part.DragCubes.SetCubeWeight(cube, 1f);
	}

	public void PlayDeployAnimation(int speed, Action postAnimate, bool resetAnimationTime)
	{
		if (speed < 0)
		{
			if (activeAnimationName != string.Empty)
			{
				ActiveAnimation.Stop(activeAnimationName);
			}
			if (deactivateAnimationName != string.Empty)
			{
				DeactivateAnimation.Stop(deactivateAnimationName);
			}
			if (resetAnimationTime && deployAnimationName != string.Empty)
			{
				DeployAnimation[deployAnimationName].time = DeployAnimation[deployAnimationName].length;
			}
		}
		if (deployAnimationName != string.Empty)
		{
			if (HighLogic.LoadedSceneIsFlight)
			{
				_animationLock = true;
				animationStatus = cacheAutoLOC_215840;
				base.Fields["animationStatus"].guiActive = true;
				_postAnimationMethod = postAnimate;
			}
			else
			{
				postAnimate();
			}
			if (resetAnimationTime)
			{
				savedAnimationSpeed = speed;
				animationStartTime = Time.unscaledTime;
				savedAnimationName = deployAnimationName;
			}
			DeployAnimation[deployAnimationName].speed = speed;
			DeployAnimation.Play(deployAnimationName);
		}
	}

	public void DisableModules()
	{
		if (!(base.vessel == null) && _Modules != null)
		{
			int i = 0;
			for (int count = _Modules.Count; i < count; i++)
			{
				_Modules[i].DisableModule();
			}
			ToggleEmmitters(state: false);
		}
	}

	public void EnableModules()
	{
		if (base.vessel == null || _Modules == null)
		{
			return;
		}
		int i = 0;
		for (int count = _Modules.Count; i < count; i++)
		{
			IAnimatedModule animatedModule = _Modules[i];
			if (animatedModule.IsSituationValid())
			{
				animatedModule.EnableModule();
			}
		}
	}

	public string[] GetDragCubeNames()
	{
		List<string> list = new List<string>();
		if (!string.IsNullOrEmpty(deployAnimationName))
		{
			list.Add("DEPLOY_A");
			list.Add("DEPLOY_B");
		}
		if (!string.IsNullOrEmpty(deactivateAnimationName))
		{
			list.Add("DEACTIVATE_A");
			list.Add("DEACTIVATE_B");
		}
		if (!string.IsNullOrEmpty(activeAnimationName))
		{
			list.Add("ACTIVE_A");
			list.Add("ACTIVE_B");
		}
		return list.ToArray();
	}

	public void AssumeDragCubePosition(string name)
	{
		switch (name)
		{
		case "ACTIVE_B":
			if (DeployAnimation != null)
			{
				ActiveAnimation[activeAnimationName].normalizedTime = 1f;
				ActiveAnimation[activeAnimationName].normalizedSpeed = 0f;
				ActiveAnimation[activeAnimationName].enabled = true;
				ActiveAnimation[activeAnimationName].weight = 1f;
				ActiveAnimation.Play(activeAnimationName);
			}
			break;
		case "ACTIVE_A":
			if (DeployAnimation != null)
			{
				ActiveAnimation[activeAnimationName].normalizedTime = 0f;
				ActiveAnimation[activeAnimationName].normalizedSpeed = 0f;
				ActiveAnimation[activeAnimationName].enabled = true;
				ActiveAnimation[activeAnimationName].weight = 1f;
				ActiveAnimation.Play(activeAnimationName);
			}
			break;
		case "DEACTIVATE_B":
			if (DeployAnimation != null)
			{
				DeactivateAnimation[deactivateAnimationName].normalizedTime = 1f;
				DeactivateAnimation[deactivateAnimationName].normalizedSpeed = 0f;
				DeactivateAnimation[deactivateAnimationName].enabled = true;
				DeactivateAnimation[deactivateAnimationName].weight = 1f;
				DeactivateAnimation.Play(deactivateAnimationName);
			}
			break;
		case "DEACTIVATE_A":
			if (DeployAnimation != null)
			{
				DeactivateAnimation[deactivateAnimationName].normalizedTime = 0f;
				DeactivateAnimation[deactivateAnimationName].normalizedSpeed = 0f;
				DeactivateAnimation[deactivateAnimationName].enabled = true;
				DeactivateAnimation[deactivateAnimationName].weight = 1f;
				DeactivateAnimation.Play(deactivateAnimationName);
			}
			break;
		case "DEPLOY_B":
			if (DeployAnimation != null)
			{
				DeployAnimation[deployAnimationName].normalizedTime = 1f;
				DeployAnimation[deployAnimationName].normalizedSpeed = 0f;
				DeployAnimation[deployAnimationName].enabled = true;
				DeployAnimation[deployAnimationName].weight = 1f;
				DeployAnimation.Play(deployAnimationName);
			}
			break;
		case "DEPLOY_A":
			if (DeployAnimation != null)
			{
				DeployAnimation[deployAnimationName].normalizedTime = 0f;
				DeployAnimation[deployAnimationName].normalizedSpeed = 0f;
				DeployAnimation[deployAnimationName].enabled = true;
				DeployAnimation[deployAnimationName].weight = 1f;
				DeployAnimation.Play(deployAnimationName);
			}
			break;
		}
	}

	public bool UsesProceduralDragCubes()
	{
		return false;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_215840 = Localizer.Format("#autoLOC_215840");
	}
}
