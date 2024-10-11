using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro.Examples;

public class ObjectSpin : MonoBehaviour
{
	public enum MotionType
	{
		Rotation,
		BackAndForth,
		Translation
	}

	public float SpinSpeed;

	public int RotationRange;

	private Transform m_transform;

	private float m_time;

	private Vector3 m_prevPOS;

	private Vector3 m_initial_Rotation;

	private Vector3 m_initial_Position;

	private Color32 m_lightColor;

	private int frames;

	public MotionType Motion;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ObjectSpin()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}
}
