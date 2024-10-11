using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class test_particles : MonoBehaviour
{
	public float power;

	public float powerVariation;

	public float throttleSpeed;

	private List<float> flameInitSizeValuesNewSystem;

	private List<float> flameInitLifeValuesNewSystem;

	private List<float> smokeInitSizeValuesNewSystem;

	private List<float> smokeInitLifeValuesNewSystem;

	public List<ParticleSystem> flameoutList;

	public List<ParticleSystem> smokeList;

	public List<ParticleSystem> flameList;

	public bool flameEnabled;

	public bool smokeEnabled;

	public Text throttleText;

	public Text smokeText;

	public Text flameText;

	public Camera testcamera;

	public float camMoveSpeed;

	public float camRotateSpeed;

	private Vector3 dragOrigin;

	private Vector3 camMove;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public test_particles()
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
	private void MouseDrag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPower(float pwr)
	{
		throw null;
	}
}
