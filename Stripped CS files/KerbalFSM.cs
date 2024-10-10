using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KerbalFSM
{
	public List<KFSMState> States;

	public KFSMState currentState;

	public KFSMEvent lastEvent;

	public KFSMState lastState;

	public bool fsmStarted;

	public string currentStateName;

	public string lastEventName;

	public bool DebugBreakOnStateChange;

	public Callback<KFSMState, KFSMState, KFSMEvent> OnStateChange = delegate
	{
	};

	public Callback<KFSMEvent> OnEventCalled = delegate
	{
	};

	public string lastCurrentState = "";

	public KFSMState CurrentState => currentState;

	public KFSMEvent LastEvent => lastEvent;

	public KFSMState LastState => lastState;

	public bool Started => fsmStarted;

	public double TimeAtCurrentState => (double)Time.time - CurrentState.TimeAtStateEnter;

	public int FramesInCurrentState => Time.frameCount - CurrentState.FrameCountAtStateEnter;

	public KerbalFSM()
	{
		States = new List<KFSMState>();
		fsmStarted = false;
	}

	public void AddState(KFSMState st)
	{
		States.Add(st);
	}

	public void AddEvent(KFSMEvent ev, params KFSMState[] toStates)
	{
		int num = toStates.Length;
		KFSMState kFSMState;
		bool flag;
		do
		{
			if (num-- <= 0)
			{
				return;
			}
			kFSMState = toStates[num];
			flag = true;
			int count = States.Count;
			while (count-- > 0)
			{
				if (States[count] == toStates[num])
				{
					flag = false;
					kFSMState.AddEvent(ev);
				}
			}
		}
		while (!flag);
		throw new Exception(string.Concat("FSM Error: Cannot add events to state ", kFSMState, ". It is not registered in this FSM (or is null)"));
	}

	public void AddEventExcluding(KFSMEvent ev, params KFSMState[] excStates)
	{
		int count = States.Count;
		while (count-- > 0)
		{
			bool flag = true;
			int num = excStates.Length;
			while (num-- > 0)
			{
				if (excStates[num] == States[count])
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				States[count].AddEvent(ev);
			}
		}
	}

	public void RunEvent(KFSMEvent evt)
	{
		if (!fsmStarted)
		{
			throw new Exception("FSM Error: Cannot switch states before starting. Please call StartFSM() before running the FSM.");
		}
		if (!currentState.StateEvents.Contains(evt))
		{
			Debug.Log("Event " + evt.name + " not assigned to state " + currentState.name);
			return;
		}
		OnEventCalled(evt);
		evt.OnEvent();
		KFSMState goToStateOnEvent = evt.GoToStateOnEvent;
		lastEvent = evt;
		lastEventName = evt.name;
		currentStateName = goToStateOnEvent.name;
		currentState.OnLeave(goToStateOnEvent);
		lastState = currentState;
		currentState = goToStateOnEvent;
		currentState.TimeAtStateEnter = Time.time;
		currentState.FrameCountAtStateEnter = Time.frameCount;
		OnStateChange(lastState, goToStateOnEvent, evt);
		currentState.OnEnter(lastState);
		if (DebugBreakOnStateChange)
		{
			Debug.Break();
		}
	}

	public void StartFSM(string initialStateName)
	{
		int num = 0;
		int count = States.Count;
		KFSMState kFSMState;
		while (true)
		{
			if (num < count)
			{
				kFSMState = States[num];
				if (kFSMState.name == initialStateName)
				{
					break;
				}
				num++;
				continue;
			}
			throw new Exception("[KFSM ERROR]: Cannot Start. Could not find a state called " + initialStateName + " in states list");
		}
		StartFSM(kFSMState);
	}

	public void StartFSM(KFSMState initialState)
	{
		currentState = initialState;
		currentState.OnEnter(null);
		currentState.TimeAtStateEnter = Time.time;
		currentState.FrameCountAtStateEnter = Time.frameCount;
		currentStateName = currentState.name;
		fsmStarted = true;
	}

	public void UpdateFSM()
	{
		if (fsmStarted)
		{
			if (currentStateName != lastCurrentState)
			{
				lastCurrentState = currentStateName;
			}
			currentState.OnUpdate();
			updateFSM(KFSMUpdateMode.UPDATE);
		}
	}

	public void FixedUpdateFSM()
	{
		if (fsmStarted)
		{
			currentState.OnFixedUpdate();
			updateFSM(KFSMUpdateMode.FIXEDUPDATE);
		}
	}

	public void LateUpdateFSM()
	{
		if (fsmStarted)
		{
			currentState.OnLateUpdate();
			updateFSM(KFSMUpdateMode.LATEUPDATE);
		}
	}

	public void updateFSM(KFSMUpdateMode mode)
	{
		for (int i = 0; i < currentState.StateEvents.Count; i++)
		{
			KFSMEvent kFSMEvent = currentState.StateEvents[i];
			if (kFSMEvent == null)
			{
				Debug.LogWarningFormat("[KerbalFSM]: Null Event in current State '{0}' encountered.", currentStateName);
			}
			else if (kFSMEvent.updateMode == mode && kFSMEvent.OnCheckCondition(currentState))
			{
				RunEvent(kFSMEvent);
			}
		}
	}
}
