using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.DebugToolbar.Screens.Cheats;

public class CheatsObjectThrower : MonoBehaviour
{
	[Serializable]
	public class ObjectType
	{
		public string name;

		public string displayName;

		public GameObject obj;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ObjectType()
		{
			throw null;
		}
	}

	public List<ObjectType> objectTypes;

	public Button objectPrev;

	public Button objectNext;

	public TextMeshProUGUI objectText;

	public float sizeMin;

	public float sizeMax;

	public float sizeDefault;

	public Slider sizeSlider;

	public TextMeshProUGUI sizeText;

	public float massMin;

	public float massMax;

	public float massDefault;

	public Slider massSlider;

	public TextMeshProUGUI massText;

	public float speedMin;

	public float speedMax;

	public float speedDefault;

	public Slider speedSlider;

	public TextMeshProUGUI speedText;

	public Toggle armedToggle;

	public TextMeshProUGUI armedText;

	private int objectTypeSelected;

	private float sizeValue;

	private float massValue;

	private float speedValue;

	private bool isArmed;

	public static float projectileDuration;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CheatsObjectThrower()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static CheatsObjectThrower()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnObjectPrev()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnObjectNext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSizeSet(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSpeedSet(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMassSet(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnArmedToggle(bool armed)
	{
		throw null;
	}
}
