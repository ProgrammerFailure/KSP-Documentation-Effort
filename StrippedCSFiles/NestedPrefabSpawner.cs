using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NestedPrefabSpawner : MonoBehaviour
{
	[Serializable]
	public class NestedPrefab
	{
		public GameObject prefab;

		public Transform tgtTransform;

		public bool setTags;

		private GameObject instance;

		private int defaultLayer;

		private string defaultTag;

		public GameObject Instance
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public NestedPrefab()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Spawn(LayerMask layers, string[] tags)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Despawn()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void setLayersAndTagsRecursive(Transform trf, LayerMask layers, int defaultLayer, string[] allowedTags, string defaultTag)
		{
			throw null;
		}
	}

	public enum SpawnOnEvent
	{
		None,
		Awake,
		StartPlusDelay
	}

	[CompilerGenerated]
	private sealed class _003CStart_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public NestedPrefabSpawner _003C_003E4__this;

		private int _003Ci_003E5__2;

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
		public _003CStart_003Ed__13(int _003C_003E1__state)
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

	public EventData<NestedPrefab> OnSpawn;

	public List<NestedPrefab> Prefabs;

	public SpawnOnEvent SpawnOn;

	public int startDelay;

	public LayerMask allowedLayers;

	public string[] allowedTags;

	public bool Spawned
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public NestedPrefabSpawner()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CStart_003Ed__13))]
	private IEnumerator Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Place Prefabs")]
	public void SpawnPrefabs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Commit")]
	public void CleanUp()
	{
		throw null;
	}
}
