using System;
using System.Collections.Generic;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class MissionEditorHistory : MonoBehaviour
{
	public delegate void ActionCallback(ConfigNode data, HistoryType type);

	public class HistoryAction
	{
		public IMEHistoryTarget target;

		public ConfigNode data;

		public ActionCallback callback;

		public bool destroyOnClear;

		public MonoBehaviour[] destroyTargets;

		public Guid StateId { get; set; }

		public HistoryAction(IMEHistoryTarget target, ActionCallback callback, bool destroyOnClear, Guid currentHistoryStateID, params MonoBehaviour[] destroyTargets)
		{
			this.target = target;
			this.callback = callback;
			data = target.GetState();
			this.destroyOnClear = destroyOnClear;
			StateId = currentHistoryStateID;
			this.destroyTargets = destroyTargets;
		}

		public HistoryAction(HistoryAction action)
		{
			target = action.target;
			callback = action.callback;
			data = target.GetState();
			destroyOnClear = action.destroyOnClear;
			StateId = action.StateId;
			destroyTargets = action.destroyTargets;
		}
	}

	public class ActionStack
	{
		public List<List<HistoryAction>> items;

		public int limit;

		public int lastFramePush = -1;

		public int Count => items.Count;

		public ActionStack(int stackLimit = -1)
		{
			items = new List<List<HistoryAction>>();
			limit = ((stackLimit < 0) ? int.MaxValue : stackLimit);
		}

		public void Push(HistoryAction item)
		{
			if (lastFramePush != Time.frameCount)
			{
				if (items.Count >= limit)
				{
					items.RemoveAt(0);
				}
				items.Add(new List<HistoryAction> { item });
				lastFramePush = Time.frameCount;
			}
			else
			{
				items[items.Count - 1].Add(item);
			}
		}

		public List<HistoryAction> Peek()
		{
			if (items.Count > 0)
			{
				return items[items.Count - 1];
			}
			return null;
		}

		public List<HistoryAction> Pop()
		{
			if (items.Count > 0)
			{
				List<HistoryAction> result = items[items.Count - 1];
				items.RemoveAt(items.Count - 1);
				return result;
			}
			return null;
		}

		public void Clear()
		{
			lastFramePush = -1;
			items.Clear();
		}
	}

	public ActionStack UndoActions;

	public ActionStack RedoActions;

	public static MissionEditorHistory Instance { get; set; }

	public bool ActionInProgress { get; set; }

	public static bool HasHistory => Instance.UndoActions.Count > 0;

	public static Guid HistoryStateId
	{
		get
		{
			if (MissionEditorLogic.Instance != null && MissionEditorLogic.Instance.EditorMission != null)
			{
				return MissionEditorLogic.Instance.EditorMission.historyId;
			}
			return default(Guid);
		}
		set
		{
			MissionEditorLogic.Instance.EditorMission.historyId = value;
		}
	}

	public void Awake()
	{
		if ((bool)Instance)
		{
			UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		Instance = this;
		UndoActions = new ActionStack();
		RedoActions = new ActionStack();
	}

	public static void PushUndoAction(IMEHistoryTarget target, ActionCallback callback)
	{
		PushUndoAction(target, callback, false);
	}

	public static void PushUndoAction(IMEHistoryTarget target, ActionCallback callback, bool destroyOnClear, params MonoBehaviour[] destroyTargets)
	{
		if (!Instance.ActionInProgress && !MissionEditorLogic.Instance.EditorMission.IsTutorialMission)
		{
			Instance.UndoActions.Push(new HistoryAction(target, callback, destroyOnClear, HistoryStateId, destroyTargets));
			HistoryStateId = Guid.NewGuid();
			Instance.ClearRedoActions();
		}
	}

	public static void Undo()
	{
		if (Instance.UndoActions.Count <= 0)
		{
			return;
		}
		Instance.ActionInProgress = true;
		try
		{
			List<HistoryAction> list = Instance.UndoActions.Pop();
			for (int i = 0; i < list.Count; i++)
			{
				HistoryAction historyAction = list[i];
				Instance.RedoActions.Push(new HistoryAction(historyAction));
				historyAction.callback(historyAction.data, HistoryType.Undo);
				SelectAffectedObject(historyAction);
				HistoryStateId = historyAction.StateId;
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			Debug.LogError("Exception caught during undo! Flushing undo cache to keep editor working!");
			Clear();
		}
		Instance.ActionInProgress = false;
	}

	public static void Redo()
	{
		if (Instance.RedoActions.Count <= 0)
		{
			return;
		}
		Instance.ActionInProgress = true;
		try
		{
			List<HistoryAction> list = Instance.RedoActions.Pop();
			for (int i = 0; i < list.Count; i++)
			{
				HistoryAction historyAction = list[i];
				Instance.UndoActions.Push(new HistoryAction(historyAction));
				historyAction.callback(historyAction.data, HistoryType.Redo);
				SelectAffectedObject(historyAction);
				HistoryStateId = historyAction.StateId;
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			Debug.LogError("Exception caught during redo! Flushing undo cache to keep editor working!");
			Clear();
		}
		Instance.ActionInProgress = false;
	}

	public static void SelectAffectedObject(HistoryAction action)
	{
		if (action.target is MEGUINode)
		{
			((MEGUINode)action.target).Select(deselectOtherNodes: true);
		}
		else if (action.target is MEGUIParameter)
		{
			object fieldHost = ((MEGUIParameter)action.target).FieldHost;
			if (fieldHost is MEGUINode)
			{
				((MEGUINode)fieldHost).Select(deselectOtherNodes: true);
			}
			else if (fieldHost is MEGUIConnector && MissionEditorLogic.Instance != null)
			{
				MissionEditorLogic.Instance.ConnectorSelectionChange((MEGUIConnector)fieldHost);
			}
		}
	}

	public static void Clear()
	{
		Instance.UndoActions.Clear();
		Instance.ClearRedoActions();
	}

	public void ClearRedoActions()
	{
		while (RedoActions.Count > 0)
		{
			List<HistoryAction> list = RedoActions.Pop();
			for (int i = 0; i < list.Count; i++)
			{
				HistoryAction historyAction = list[i];
				if (!historyAction.destroyOnClear)
				{
					continue;
				}
				if (historyAction.destroyTargets != null && historyAction.destroyTargets.Length != 0)
				{
					int j = 0;
					for (int num = historyAction.destroyTargets.Length; j < num; j++)
					{
						UnityEngine.Object.Destroy(historyAction.destroyTargets[j].gameObject);
					}
				}
				else
				{
					UnityEngine.Object.Destroy(((MonoBehaviour)historyAction.target).gameObject);
				}
			}
		}
	}
}
