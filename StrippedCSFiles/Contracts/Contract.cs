using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Contracts.Agents;
using KSP.UI.Screens;
using KSPAchievements;
using UnityEngine;

namespace Contracts;

public class Contract : IContractParameterHost
{
	public enum ContractPrestige
	{
		[Description("#autoLOC_7001024")]
		Trivial,
		[Description("#autoLOC_7001025")]
		Significant,
		[Description("#autoLOC_7001026")]
		Exceptional
	}

	public enum State
	{
		[Description("#autoLOC_900713")]
		Generated,
		[Description("#autoLOC_900337")]
		Offered,
		[Description("#autoLOC_900714")]
		OfferExpired,
		[Description("#autoLOC_900716")]
		Declined,
		[Description("#autoLOC_900711")]
		Cancelled,
		[Description("#autoLOC_900336")]
		Active,
		[Description("#autoLOC_900710")]
		Completed,
		[Description("#autoLOC_900715")]
		DeadlineExpired,
		[Description("#autoLOC_900708")]
		Failed,
		[Description("#autoLOC_8003155")]
		Withdrawn
	}

	public enum Viewed
	{
		Unseen,
		Seen,
		Read
	}

	public enum DeadlineType
	{
		Fixed,
		Floating,
		None
	}

	protected enum ProgressState
	{
		Unreached,
		Reached,
		Incomplete,
		Complete
	}

	[CompilerGenerated]
	private sealed class _003Cget_AllParameters_003Ed__109 : IEnumerable<ContractParameter>, IEnumerable, IEnumerator<ContractParameter>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ContractParameter _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Contract _003C_003E4__this;

		private IEnumerator<ContractParameter> _003CparameterEnumerator_003E5__2;

		private int _003CiC_003E5__3;

		private int _003Ci_003E5__4;

		ContractParameter IEnumerator<ContractParameter>.Current
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
		public _003Cget_AllParameters_003Ed__109(int _003C_003E1__state)
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		IEnumerator<ContractParameter> IEnumerable<ContractParameter>.GetEnumerator()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw null;
		}
	}

	[SerializeField]
	protected ContractPrestige prestige;

	[SerializeField]
	protected DeadlineType expiryType;

	[SerializeField]
	protected DeadlineType deadlineType;

	[SerializeField]
	protected Agent agent;

	[SerializeField]
	private State state;

	[SerializeField]
	private Viewed viewed;

	[SerializeField]
	protected double dateExpire;

	[SerializeField]
	protected double dateAccepted;

	[SerializeField]
	protected double dateDeadline;

	[SerializeField]
	protected double dateFinished;

	[SerializeField]
	private long contractID;

	private Guid contractGuid;

	[SerializeField]
	private int missionSeed;

	public double TimeExpiry;

	public double TimeDeadline;

	public double FundsAdvance;

	public double FundsCompletion;

	public double FundsFailure;

	public float ScienceCompletion;

	public float ReputationCompletion;

	public float ReputationFailure;

	public bool AutoAccept;

	public bool IgnoresWeight;

	private List<string> keywords;

	private List<string> keywordsRequired;

	[SerializeField]
	private List<ContractParameter> parameters;

	public EventData<State> OnStateChange;

	public EventData<Viewed> OnViewedChange;

	public static int contractsInExistance
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

	public ContractPrestige Prestige
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string Title
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string Synopsys
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string Description
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string Notes
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Agent Agent
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public State ContractState
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string LocalizedContractState
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Viewed ContractViewed
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double DateExpire
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double DateAccepted
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double DateDeadline
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double DateFinished
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public long ContractID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Guid ContractGuid
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int MissionSeed
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<string> Keywords
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<string> KeywordsRequired
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public IContractParameterHost Parent
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Contract Root
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int ParameterCount
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ContractParameter this[int index]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ContractParameter this[string id]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ContractParameter this[Type type]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public IEnumerable<ContractParameter> AllParameters
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[IteratorStateMachine(typeof(_003Cget_AllParameters_003Ed__109))]
		get
		{
			throw null;
		}
	}

	protected static double GameTime
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Contract()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	~Contract()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetExpiry()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetExpiry(int minDays, int maxDays)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetExpiry(float minDays, float maxDays)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetDeadlineDays(float days, CelestialBody body = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetDeadlineYears(float years, CelestialBody body = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetFunds(float advance, float completion, CelestialBody body = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetFunds(float advance, float completion, float failure, CelestialBody body = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetReputation(float completion, float failure, CelestialBody body = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetReputation(float completion, CelestialBody body = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetScience(float completion, CelestialBody body = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float GetDestinationWeight(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool AddKeywords(params string[] keywords)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool AddKeywordsRequired(params string[] keywords)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ContractParameter AddParameter(ContractParameter parameter, string id = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ParameterStateUpdate(ContractParameter p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ContractParameter GetParameter(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ContractParameter GetParameter(string id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ContractParameter GetParameter(Type type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T GetParameter<T>(string id = null) where T : ContractParameter
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveParameter(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveParameter(string id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveParameter(Type type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveParameter(ContractParameter parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnParameterStateChange(ContractParameter p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanBeCancelled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanBeDeclined()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanBeFailed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool MeetRequirements()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual List<CelestialBody> GetWeightBodies()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string GetSynopsys()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string GetDescription()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string GetTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string GetNotes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool Generate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnRegister()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnUnregister()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnReset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void AwardAdvance()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void AwardCompletion()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void PenalizeCancellation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void PenalizeFailure()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnOffered()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnOfferExpired()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnAccepted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnCancelled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnCompleted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDeclined()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnFailed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDeadlineExpired()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnFinished()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnGenerateFailed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnWithdrawn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnSeen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnRead()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string GetHashString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string MessageOffered()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string MessageOfferExpired()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string MessageCancelled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string MessageAccepted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string MessageDeadlineExpired()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string MessageFailed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string MessageCompleted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string MessageAdvances()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string MessageRewards()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string MessageFailurePenalties()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string MessageCancellationPenalties(double fundsPenalty, float repPenalty)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string MissionControlTextRich()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected string MissionNotes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string ParameterNotes(ContractParameter p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected string MissionParameter(ContractParameter parameter, int indent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupSeed(int seed = -1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupID()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SendStateMessage(string title, string message, MessageSystemButton.MessageButtonColor color, MessageSystemButton.ButtonIcons icon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Offer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Accept()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Decline()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Cancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Withdraw()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Fail()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Complete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetState(State newState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string StateMsgAddition(string addition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsFinished()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Register()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Unregister()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Kill()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetViewed(Viewed viewed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Contract Generate(Type contractType, ContractPrestige difficulty, int seed, State state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Contract Load(Contract contract, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GenerateFailed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static List<CelestialBody> GetBodies(bool includeKerbin, bool includeSun)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static List<CelestialBody> GetBodies_Reached(bool includeKerbin, bool includeSun)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static List<CelestialBody> GetBodies_NotReached(bool includeKerbin, bool includeSun)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static List<CelestialBody> GetBodies_InComplete(bool includeKerbin, bool includeSun, string notComplete)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static List<CelestialBody> GetBodies_Complete(bool includeKerbin, bool includeSun, string complete)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static int CountBodies_Complete(bool includeKerbin, bool includeSun, string nodeComplete)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static int CountBodies_Reached(bool includeKerbin, bool includeSun)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static List<CelestialBody> GetBodies(string nodeName, ProgressState nodeState, Func<CelestialBody, bool> where = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddBodyNodes(ProgressState nodeState, string nodeName, List<CelestialBody> bodies, CelestialBodySubtree tree, Func<CelestialBody, bool> where = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static List<CelestialBody> GetBodies(ProgressState bodyState, Func<CelestialBody, bool> where = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddBodyTrees(ProgressState bodyState, List<CelestialBodySubtree> bodies, CelestialBodySubtree tree)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static List<CelestialBody> GetBodies(ProgressState bodyState, string nodeName, ProgressState nodeState, Func<CelestialBody, bool> where = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddBodyTrees(ProgressState bodyState, string nodeName, ProgressState nodeState, List<CelestialBodySubtree> bodies, CelestialBodySubtree tree)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static List<CelestialBody> GetBodies_NextUnreached(int depth, Func<CelestialBody, bool> where = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected CelestialBody WeightedBodyChoice(IList<CelestialBody> bodies, System.Random generator = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddNextUnreachedBodyTrees(ref int depth, List<CelestialBodySubtree> bodies, CelestialBodySubtree tree, Func<CelestialBody, bool> where)
	{
		throw null;
	}
}
