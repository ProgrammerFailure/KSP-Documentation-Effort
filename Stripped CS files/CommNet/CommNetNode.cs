using UnityEngine;

namespace CommNet;

public class CommNetNode : MonoBehaviour
{
	public CommNode comm;

	public bool networkInitialised;

	public CommNode Comm
	{
		get
		{
			return comm;
		}
		set
		{
			comm = value;
		}
	}

	public virtual void Start()
	{
		comm = new CommNode(base.transform);
		comm.OnNetworkPreUpdate = OnNetworkPreUpdate;
		comm.OnNetworkPostUpdate = OnNetworkPostUpdate;
		if (CommNetNetwork.Initialized)
		{
			networkInitialised = true;
			CommNetNetwork.Add(comm);
		}
		else
		{
			networkInitialised = false;
		}
		GameEvents.CommNet.OnNetworkInitialized.Add(OnNetworkInitialized);
	}

	public virtual void OnDestroy()
	{
		if (networkInitialised)
		{
			networkInitialised = false;
			CommNetNetwork.Remove(comm);
		}
		GameEvents.CommNet.OnNetworkInitialized.Remove(OnNetworkInitialized);
	}

	public virtual void OnNetworkInitialized()
	{
		networkInitialised = true;
		CommNetNetwork.Add(comm);
	}

	public virtual void OnNetworkPreUpdate()
	{
	}

	public virtual void OnNetworkPostUpdate()
	{
	}
}
