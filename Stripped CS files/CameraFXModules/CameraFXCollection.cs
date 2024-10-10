using System;
using System.Collections.Generic;
using UnityEngine;

namespace CameraFXModules;

[Serializable]
public class CameraFXCollection
{
	public List<CameraFXModule> fxModules;

	public int fxModuleCount;

	public Vector3 p;

	public Quaternion r;

	public int Count => fxModuleCount;

	public CameraFXModule this[int index]
	{
		get
		{
			if (index < 0 || index >= fxModuleCount)
			{
				throw new ArgumentOutOfRangeException();
			}
			return fxModules[index];
		}
	}

	public CameraFXCollection()
	{
		fxModules = new List<CameraFXModule>();
		fxModuleCount = 0;
	}

	public Vector3 GetLocalPositionFX(Vector3 p0, float multiplier, Views viewMask)
	{
		p = p0;
		fxModuleCount = fxModules.Count;
		for (int i = 0; i < fxModuleCount; i++)
		{
			if (fxModules[i].IsActive())
			{
				p = fxModules[i].UpdateLocalPosition(p0, p, multiplier, viewMask);
			}
		}
		return p;
	}

	public Quaternion GetLocalRotationFX(Quaternion r0, float multiplier, Views viewMask)
	{
		r = r0;
		fxModuleCount = fxModules.Count;
		for (int i = 0; i < fxModuleCount; i++)
		{
			if (fxModules[i].IsActive())
			{
				r = fxModules[i].UpdateLocalRotation(r0, r, multiplier, viewMask);
			}
		}
		return r;
	}

	public void AddFX(CameraFXModule fx)
	{
		fxModules.Add(fx);
		fxModuleCount = fxModules.Count;
		fx.OnFXAdded(this);
	}

	public void InsertFX(CameraFXModule fx, int at)
	{
		fxModules.Insert(at, fx);
		fxModuleCount = fxModules.Count;
		fx.OnFXAdded(this);
	}

	public void RemoveFX(CameraFXModule fx)
	{
		fxModules.Remove(fx);
		fxModuleCount = fxModules.Count;
		fx.OnFXRemoved(this);
	}

	public void Clear()
	{
		int count = fxModules.Count;
		for (int i = 0; i < count; i++)
		{
			fxModules[i].OnFXRemoved(this);
		}
		fxModules.Clear();
		fxModuleCount = 0;
	}

	public int IndexOf(CameraFXModule fx)
	{
		return fxModules.IndexOf(fx);
	}

	public bool Contains(CameraFXModule fx)
	{
		return fxModules.Contains(fx);
	}
}
