using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/PQuadSphere_Cache")]
public class PQSCache : MonoBehaviour
{
	public class PQSGlobalPresetList
	{
		public List<PQSPreset> presets;

		public string preset;

		public int presetIndex;

		public string version;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PQSGlobalPresetList()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Load(ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetPreset(int presetIndex)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetPreset(string presetName)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Save(ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PQSSpherePreset GetPreset(string pqsName)
		{
			throw null;
		}
	}

	public class PQSPreset
	{
		public string name;

		public List<PQSSpherePreset> spherePresets;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PQSPreset()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Load(ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Save(ConfigNode node)
		{
			throw null;
		}
	}

	public class PQSSpherePreset
	{
		public string name;

		public double minDistance;

		public int minSubdivision;

		public int maxSubdivision;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PQSSpherePreset()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PQSSpherePreset(string name, double minDistance, int minSubdivision, int maxSubdivision)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Load(ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Save(ConfigNode node)
		{
			throw null;
		}
	}

	public delegate void OnEvent();

	[CompilerGenerated]
	private sealed class _003CCoroutineCreatePQCache_003Ed__30 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PQSCache _003C_003E4__this;

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
		public _003CCoroutineCreatePQCache_003Ed__30(int _003C_003E1__state)
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

	public static PQSCache Instance;

	public static PQSGlobalPresetList PresetList;

	public static int lastCompatibleMajor;

	public static int lastCompatibleMinor;

	public static int lastCompatibleRev;

	public int cachePQInitialCount;

	public int cachePQIncreasePerFrame;

	public bool cachePQCoroutine;

	private bool cacheBeingCreated;

	[HideInInspector]
	public bool cacheReady;

	[HideInInspector]
	public Stack<PQ> cachePQUnassigned;

	public int cachePQAssignedCount;

	public int cachePQUnassignedCount;

	public int cachePQTotalCount;

	public OnEvent onCacheReady;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PQSCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void LoadPresetList(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SavePresetList(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CreateDefaultPresetList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CreateCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void IncreasePQCache(int addCount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQ GetQuad()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DestroyQuad(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CCoroutineCreatePQCache_003Ed__30))]
	protected IEnumerator CoroutineCreatePQCache()
	{
		throw null;
	}
}
