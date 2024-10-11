using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class MissionEditorHistory : MonoBehaviour
{
	public delegate void ActionCallback(ConfigNode data, HistoryType type);

	protected class HistoryAction
	{
		public IMEHistoryTarget target;

		public ConfigNode data;

		public ActionCallback callback;

		public bool destroyOnClear;

		public MonoBehaviour[] destroyTargets;

		public Guid StateId
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
		public HistoryAction(IMEHistoryTarget target, ActionCallback callback, bool destroyOnClear, Guid currentHistoryStateID, params MonoBehaviour[] destroyTargets)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public HistoryAction(HistoryAction action)
		{
			throw null;
		}
	}

	protected class ActionStack
	{
		private List<List<HistoryAction>> items;

		private int limit;

		private int lastFramePush;

		public int Count
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ActionStack(int stackLimit = -1)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Push(HistoryAction item)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public List<HistoryAction> Peek()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public List<HistoryAction> Pop()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Clear()
		{
			throw null;
		}
	}

	protected ActionStack UndoActions;

	protected ActionStack RedoActions;

	public static MissionEditorHistory Instance
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

	public bool ActionInProgress
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public static bool HasHistory
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Guid HistoryStateId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionEditorHistory()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void PushUndoAction(IMEHistoryTarget target, ActionCallback callback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void PushUndoAction(IMEHistoryTarget target, ActionCallback callback, bool destroyOnClear, params MonoBehaviour[] destroyTargets)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Undo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Redo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SelectAffectedObject(HistoryAction action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Clear()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ClearRedoActions()
	{
		throw null;
	}
}
