using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[Serializable]
public class RnDCommsStream
{
	[CompilerGenerated]
	private sealed class _003CtimeoutCoroutine_003Ed__14 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public RnDCommsStream _003C_003E4__this;

		public ProtoVessel source;

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
		public _003CtimeoutCoroutine_003Ed__14(int _003C_003E1__state)
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

	public ScienceSubject subject;

	public float fileSize;

	public float timeout;

	public float xmitValue;

	public float scienceValueRatio;

	public bool xmitIncomplete;

	private float dataIn;

	private float dataOut;

	private float UTofLastTransmit;

	private bool timedOut;

	private ResearchAndDevelopment host;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RnDCommsStream(ScienceSubject subject, float fileSize, float timeout, float xmitValue, bool xmitIncomplete, ResearchAndDevelopment RDInstance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RnDCommsStream(ScienceSubject subject, float fileSize, float timeout, float xmitValue, float scienceValueRatio, bool xmitIncomplete, ResearchAndDevelopment RDInstance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StreamData(float dataAmount, ProtoVessel source)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CtimeoutCoroutine_003Ed__14))]
	private IEnumerator timeoutCoroutine(ProtoVessel source)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void submitStreamData(ProtoVessel source)
	{
		throw null;
	}
}
