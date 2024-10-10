using System;
using System.Collections.Generic;
using UnityEngine;

namespace ns2;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupInputLock : MonoBehaviour
{
	public static bool disableAllLocking;

	[SerializeField]
	public List<string> inputLockMask = new List<string>();

	public List<string> defaultMask = new List<string>();

	public ulong lockMask;

	public bool lockMaskDirty = true;

	public CanvasGroup canvasGroup;

	public List<string> InputLockMask
	{
		get
		{
			lockMaskDirty = true;
			return inputLockMask;
		}
	}

	public ulong LockMask
	{
		get
		{
			return lockMask;
		}
		set
		{
			lockMask = value;
			lockMaskDirty = false;
		}
	}

	public void UpdateLockMask()
	{
		lockMask = 0uL;
		int i = 0;
		for (int count = inputLockMask.Count; i < count; i++)
		{
			try
			{
				ControlTypes controlTypes = (ControlTypes)Enum.Parse(typeof(ControlTypes), inputLockMask[i], ignoreCase: true);
				lockMask |= (ulong)controlTypes;
			}
			catch (Exception ex)
			{
				Debug.Log("CanvasGroupInputLock '" + base.gameObject.name + "': " + ex.Message);
			}
		}
		Debug.Log(base.gameObject.name + " MASK: " + lockMask);
		lockMaskDirty = false;
	}

	public void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		InitializeDefaultMask();
		GameEvents.onInputLocksModified.Add(OnInputLocksModified);
	}

	public void InitializeDefaultMask()
	{
		defaultMask.Clear();
		for (int i = 0; i < inputLockMask.Count; i++)
		{
			defaultMask.Add(inputLockMask[i]);
		}
	}

	public void SetInputMaskToDefault()
	{
		SetInputMask(defaultMask);
	}

	public void SetInputMask(List<string> newMask)
	{
		inputLockMask.Clear();
		for (int i = 0; i < newMask.Count; i++)
		{
			inputLockMask.Add(newMask[i]);
		}
		UpdateLockMask();
	}

	public void OnDestroy()
	{
		GameEvents.onInputLocksModified.Remove(OnInputLocksModified);
	}

	public void Start()
	{
		if (!disableAllLocking)
		{
			UpdateLockMask();
			canvasGroup.blocksRaycasts = (InputLockManager.lockMask & lockMask) == 0L;
		}
	}

	public void OnInputLocksModified(GameEvents.FromToAction<ControlTypes, ControlTypes> fromTo)
	{
		if (!disableAllLocking)
		{
			if (lockMaskDirty)
			{
				UpdateLockMask();
			}
			canvasGroup.blocksRaycasts = ((ulong)fromTo.to & lockMask) == 0L;
		}
	}

	public static string ToBits(ControlTypes lockMask)
	{
		return ToBits((ulong)lockMask);
	}

	public static string ToBits(ulong lockMask)
	{
		string text = Convert.ToString((long)lockMask, 2);
		if (text.Length < 64)
		{
			text = new string('0', 64 - text.Length) + text;
		}
		return text;
	}
}
