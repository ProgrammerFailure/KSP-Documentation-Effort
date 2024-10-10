using System;
using System.Collections.Generic;
using Expansions.Missions.Editor;
using Expansions.Missions.Flow;
using UnityEngine;

namespace Expansions.Missions;

public class MissionCheckpointValidator
{
	public static List<string> modulesAreEqualErrorList;

	public static bool ValidateCheckpoint(Mission checkpointMission, Mission mission)
	{
		return CheckCheckpointPathRecursive(checkpointMission, mission, checkpointMission.startNode, mission.startNode);
	}

	public static bool CheckCheckpointPathRecursive(Mission checkpointMission, Mission mission, MENode checkpointNode, MENode missionNode)
	{
		MENodePathInfo mENodePathInfo = checkpointMission.flow.NodePaths[checkpointNode];
		MENodePathInfo mENodePathInfo2 = mission.flow.NodePaths[missionNode];
		bool result = false;
		int i = 0;
		for (int count = mENodePathInfo.paths.Count; i < count; i++)
		{
			int j = 0;
			for (int count2 = mENodePathInfo.paths[i].Nodes.Count; j < count2; j++)
			{
				if (mENodePathInfo.paths[i].Nodes[j].HasBeenActivated)
				{
					if (i < mENodePathInfo2.paths.Count && j < mENodePathInfo2.paths[i].Nodes.Count && !IsCheckpointNodeDirty(mENodePathInfo.paths[i].Nodes[j], mENodePathInfo2.paths[i].Nodes[j]))
					{
						return CheckCheckpointPathRecursive(checkpointMission, mission, mENodePathInfo.paths[i].Nodes[j], mENodePathInfo2.paths[i].Nodes[j]);
					}
					result = true;
				}
				else
				{
					result = false;
				}
			}
		}
		return result;
	}

	public static bool IsCheckpointNodeDirty(MENode checkpointNode, MENode missionNode)
	{
		if (checkpointNode.id != missionNode.id)
		{
			return true;
		}
		if (!ModulesAreEqual(checkpointNode, missionNode))
		{
			return true;
		}
		if (checkpointNode.testGroups.Count != missionNode.testGroups.Count)
		{
			return true;
		}
		if (checkpointNode.actionModules.Count != missionNode.actionModules.Count)
		{
			return true;
		}
		int num = 0;
		int count = missionNode.testGroups.Count;
		while (true)
		{
			if (num < count)
			{
				if (checkpointNode.testGroups[num].testModules.Count != missionNode.testGroups[num].testModules.Count)
				{
					break;
				}
				int i = 0;
				for (int count2 = missionNode.testGroups[num].testModules.Count; i < count2; i++)
				{
					if (!ModulesAreEqual(checkpointNode.testGroups[num].testModules[i], missionNode.testGroups[num].testModules[i]))
					{
						return true;
					}
				}
				num++;
				continue;
			}
			int num2 = 0;
			int count3 = missionNode.actionModules.Count;
			while (true)
			{
				if (num2 < count3)
				{
					if (!ModulesAreEqual(checkpointNode.actionModules[num2], missionNode.actionModules[num2]))
					{
						break;
					}
					num2++;
					continue;
				}
				return false;
			}
			return true;
		}
		return true;
	}

	public static bool ModulesAreEqual(object checkpointModule, object missionModule)
	{
		if (checkpointModule.GetType() != missionModule.GetType())
		{
			return false;
		}
		BaseAPFieldList baseAPFieldList = new BaseAPFieldList(checkpointModule);
		BaseAPFieldList baseAPFieldList2 = new BaseAPFieldList(missionModule);
		int i = 0;
		for (int count = baseAPFieldList2.Count; i < count; i++)
		{
			switch (baseAPFieldList[i].Attribute.checkpointValidation)
			{
			case CheckpointValidationType.Equals:
				try
				{
					if (!baseAPFieldList[i].GetValue().Equals(baseAPFieldList2[i].GetValue()))
					{
						return false;
					}
				}
				catch
				{
					if (modulesAreEqualErrorList == null)
					{
						modulesAreEqualErrorList = new List<string>();
					}
					if (!modulesAreEqualErrorList.Contains(missionModule.GetType().Name))
					{
						modulesAreEqualErrorList.Add(missionModule.GetType().Name);
						Debug.LogWarningFormat("[Checkpoint Comparator] Unable to compare Modules of type {0}. Enesure there is an OnValidateCheckpointValue method defined or an Equals method.", missionModule.GetType().Name);
					}
				}
				break;
			case CheckpointValidationType.CustomMethod:
				if (baseAPFieldList[i].CompareValuesForCheckpoint != null && !(bool)baseAPFieldList[i].CompareValuesForCheckpoint.Invoke(missionModule, new object[1] { baseAPFieldList[i].GetValue() }))
				{
					return false;
				}
				break;
			case CheckpointValidationType.Controls:
				if (!ModulesAreEqual(baseAPFieldList[i].GetValue(), baseAPFieldList2[i].GetValue()))
				{
					return false;
				}
				break;
			}
		}
		return true;
	}

	public static bool CompareStructLists<T>(List<T> list1, List<T> list2) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
	{
		return CompareAnyLists(list1, list2);
	}

	public static bool CompareObjectLists<T>(List<T> list1, List<T> list2)
	{
		if (list1 != null && list2 != null)
		{
			return CompareAnyLists(list1, list2);
		}
		return false;
	}

	public static bool CompareAnyLists<T>(List<T> list1, List<T> list2)
	{
		if (list1.Count != list2.Count)
		{
			return false;
		}
		int num = 0;
		while (true)
		{
			if (num < list1.Count)
			{
				if (!list1[num].Equals(list2[num]))
				{
					break;
				}
				num++;
				continue;
			}
			return true;
		}
		return false;
	}
}
