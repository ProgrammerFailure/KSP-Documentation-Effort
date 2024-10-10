using System;
using UnityEngine;

namespace Expansions.Serenity.RobotArmFX;

[Serializable]
public abstract class RobotArmScannerFX : ScriptableObject, IConfigNode
{
	public string className = string.Empty;

	public float effectStartTime;

	public float effectStopTime = 1f;

	public bool IsReady { get; set; }

	public Part Part { get; set; }

	public RobotArmScannerFX()
	{
		IsReady = false;
	}

	public virtual void OnStart(Part part)
	{
		Part = part;
	}

	public abstract void OnEffectStart();

	public abstract void OnUpdate(float animationTime, float distanceFromSurface, Vector3 instrumentTargetPosition);

	public abstract void OnEffectStop();

	public virtual void Load(ConfigNode node)
	{
		node.TryGetValue("className", ref className);
		node.TryGetValue("effectStartTime", ref effectStartTime);
		node.TryGetValue("effectStopTime", ref effectStopTime);
	}

	public virtual void Save(ConfigNode node)
	{
		node.AddValue("className", className);
		node.AddValue("effectStartTime", effectStartTime);
		node.AddValue("effectStopTime", effectStopTime);
	}

	public static RobotArmScannerFX CreateInstanceOfRobotArmScannerFX(string className)
	{
		Type classByName = AssemblyLoader.GetClassByName(typeof(RobotArmScannerFX), className);
		ScriptableObject scriptableObject = null;
		if (classByName != null)
		{
			scriptableObject = ScriptableObject.CreateInstance(classByName);
		}
		return scriptableObject as RobotArmScannerFX;
	}
}
