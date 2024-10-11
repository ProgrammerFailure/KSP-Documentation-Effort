using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace EdyCommonTools;

public class RotationController : MonoBehaviour
{
	public delegate void OnRotationFinished();

	[Serializable]
	public class Rotation
	{
		public enum Axis
		{
			Up,
			Right,
			Forward
		}

		public enum Mode
		{
			Disabled,
			Free,
			LookAtTarget,
			LookAtTargetCenter
		}

		public float angle;

		public Mode mode;

		public float targetOffset;

		public bool damped;

		public float damping;

		public bool clamped;

		public float minAngle;

		public float maxAngle;

		private float m_currentAngle;

		private float m_currentTarget;

		private Axis m_axis;

		private Vector3 m_axisVector;

		public Quaternion rotation
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public bool isRotating
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Rotation(Axis axis)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Rotation(Axis axis, Mode rotationMode)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Rotation(Axis axis, Mode rotationMode, float min, float max)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void ResetAngle(float newAngle)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Update(float deltaTime, Transform self, Transform target, Renderer targetRenderer, Transform reference)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private float AngleToTarget(Vector3 pos, Vector3 targetPos, Transform reference)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Vector3 GetAxisVector(Axis axis)
		{
			throw null;
		}
	}

	public Transform target;

	public bool rotateInWorldSpace;

	public bool invertHVorder;

	public Rotation horizontal;

	public Rotation vertical;

	[FormerlySerializedAs("pitch")]
	public Rotation roll;

	public OnRotationFinished onRotationFinished;

	private Transform m_trans;

	private Transform m_cachedTarget;

	private Renderer m_targetRenderer;

	private bool m_rotating;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RotationController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateTransform()
	{
		throw null;
	}
}
