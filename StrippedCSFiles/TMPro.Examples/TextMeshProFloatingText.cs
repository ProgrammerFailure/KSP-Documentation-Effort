using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro.Examples;

public class TextMeshProFloatingText : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CDisplayTextMeshProFloatingText_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TextMeshProFloatingText _003C_003E4__this;

		private float _003CCountDuration_003E5__2;

		private float _003Cstarting_Count_003E5__3;

		private float _003Ccurrent_Count_003E5__4;

		private Vector3 _003Cstart_pos_003E5__5;

		private Color32 _003Cstart_color_003E5__6;

		private float _003Calpha_003E5__7;

		private float _003CfadeDuration_003E5__8;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CDisplayTextMeshProFloatingText_003Ed__12(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CDisplayTextMeshFloatingText_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TextMeshProFloatingText _003C_003E4__this;

		private float _003CCountDuration_003E5__2;

		private float _003Cstarting_Count_003E5__3;

		private float _003Ccurrent_Count_003E5__4;

		private Vector3 _003Cstart_pos_003E5__5;

		private Color32 _003Cstart_color_003E5__6;

		private float _003Calpha_003E5__7;

		private float _003CfadeDuration_003E5__8;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CDisplayTextMeshFloatingText_003Ed__13(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	public Font TheFont;

	private GameObject m_floatingText;

	private TextMeshPro m_textMeshPro;

	private TextMesh m_textMesh;

	private Transform m_transform;

	private Transform m_floatingText_Transform;

	private Transform m_cameraTransform;

	private Vector3 lastPOS;

	private Quaternion lastRotation;

	public int SpawnType;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TextMeshProFloatingText()
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
	[IteratorStateMachine(typeof(_003CDisplayTextMeshProFloatingText_003Ed__12))]
	public IEnumerator DisplayTextMeshProFloatingText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CDisplayTextMeshFloatingText_003Ed__13))]
	public IEnumerator DisplayTextMeshFloatingText()
	{
		throw null;
	}
}
