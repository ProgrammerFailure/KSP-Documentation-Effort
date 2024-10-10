using System;
using System.Collections;
using UnityEngine;

public class kerbalExpressionSystem : MonoBehaviour
{
	public ProtoCrewMember protoCrewMember;

	public Part evaPart;

	public Animator animator;

	public string expressionParameterName = "Expression";

	public string varianceParameterName = "Variance";

	public string secondaryVarianceParameterName = "SecondaryVariance";

	public string hasHelmetName = "hasHelmet";

	public string idleBoolName = "isIdle";

	public string idleFloatName = "IdleFloatController";

	public Kerbal kerbal;

	public KerbalEVA kerbalEVA;

	public float panicLevel;

	public float wheeLevel;

	public float fearFactor;

	public float expression;

	public float expressionLerpRate = 1f;

	public float idleThreshold = 0.2f;

	public int idleRandomNumberLimitHelmet = 11;

	public int idleRandomNumberLimitNoHelmet = 6;

	public float varianceLerpRate = 1f;

	public float varianceUpdateInterval = 1f;

	public float varianceTimer;

	public float secondaryVarianceLerpRate = 1f;

	public float secondaryVarianceUpdateInterval = 1f;

	public float secondaryVarianceTimer;

	public float idleTimer;

	public float idleUpdateInterval = 10f;

	public int idlePlayChance = 10;

	public float flight_velocity;

	public float flight_angularV;

	public float flight_gee;

	public int expressionSampleSize = 20;

	public int currentExpressionCacheIndex;

	public float[] expressionSampleCache;

	public bool isIVAController;

	public bool isBadass;

	public float IdleFloat;

	public bool isIdle;

	public bool hasHelmet;

	public float courage;

	public float stupidity;

	public float variance;

	public float secondaryVariance;

	public float targetVariance;

	public float targetSecondaryVariance;

	public float targetExpression;

	public float normalizedExpression;

	public int expressionHash;

	public int varianceHash;

	public int secondaryVarianceHash;

	public int idleBoolHash;

	public int idleFloatHash;

	public int hasHelmetHash;

	public bool running;

	public bool isJetpackOn;

	public float newIdleWheeLevel;

	public float newIdleFearFactor;

	public float startExpressionTime;

	public bool useNewIdleExpressions;

	public static bool showInactive = true;

	public virtual void Awake()
	{
		running = false;
		expressionSampleCache = new float[expressionSampleSize];
		GameEvents.onPartExplode.Add(explosionReaction);
	}

	public virtual void OnDestroy()
	{
		GameEvents.onPartExplode.Remove(explosionReaction);
	}

	public virtual void explosionReaction(GameEvents.ExplosionReaction er)
	{
		fearFactor = er.magnitude * 100f - er.distance;
	}

	public virtual void Start()
	{
		float num = 0f;
		wheeLevel = 0f;
		float num2 = num;
		num = 0f;
		panicLevel = num2;
		fearFactor = num;
		flight_gee = 1f;
		hasHelmet = true;
		if (evaPart == null)
		{
			isIVAController = true;
		}
		else
		{
			isIVAController = false;
		}
		animator = GetComponent<Animator>();
		if (isIVAController && animator != null)
		{
			animator.SetLayerWeight(1, 1f);
		}
		expressionHash = Animator.StringToHash(expressionParameterName);
		varianceHash = Animator.StringToHash(varianceParameterName);
		secondaryVarianceHash = Animator.StringToHash(secondaryVarianceParameterName);
		idleBoolHash = Animator.StringToHash(idleBoolName);
		idleFloatHash = Animator.StringToHash(idleFloatName);
		hasHelmetHash = Animator.StringToHash(hasHelmetName);
		if (isIVAController)
		{
			if (kerbal != null)
			{
				protoCrewMember = kerbal.protoCrewMember;
				hasHelmet = kerbal.showHelmet;
			}
		}
		else if (evaPart != null && evaPart.protoModuleCrew.Count > 0)
		{
			protoCrewMember = evaPart.protoModuleCrew[0];
		}
		if (protoCrewMember != null && animator != null)
		{
			isBadass = protoCrewMember.isBadass;
			courage = protoCrewMember.courage;
			stupidity = protoCrewMember.stupidity;
			running = true;
		}
		else
		{
			Debug.Log("Failed to locate protocrewMember, or Animator Component! unable to start expression AI");
			running = false;
		}
	}

	public virtual void FixedUpdate()
	{
		if (FlightGlobals.ActiveVessel != null)
		{
			flight_angularV = Mathf.Clamp((float)FlightGlobals.ship_angularVelocity.magnitude, 0f, 100f);
			flight_velocity = (float)FlightGlobals.ship_velocity.magnitude;
			flight_gee = ((FlightGlobals.ship_verticalSpeed < 0.0) ? ((float)FlightGlobals.ship_geeForce) : 1f);
		}
		varianceTimer += Time.deltaTime;
		if (varianceTimer >= varianceUpdateInterval)
		{
			varianceTimer = 0f;
			targetVariance = UnityEngine.Random.Range(0f, 1f);
		}
		variance = Mathf.Lerp(variance, targetVariance, Time.deltaTime * varianceLerpRate);
		secondaryVarianceTimer += Time.deltaTime;
		if (secondaryVarianceTimer >= secondaryVarianceUpdateInterval)
		{
			secondaryVarianceTimer = 0f;
			targetSecondaryVariance = UnityEngine.Random.Range(0f, 1f);
		}
		secondaryVariance = Mathf.Lerp(secondaryVariance, targetSecondaryVariance, Time.deltaTime * secondaryVarianceLerpRate);
		if (!isIVAController)
		{
			return;
		}
		if (Mathf.Abs(expression) <= 0.06f)
		{
			idleTimer += Time.deltaTime;
			if (idleTimer >= idleUpdateInterval)
			{
				idleTimer = 0f;
				isIdle = true;
				if (UnityEngine.Random.Range(0, 100) <= idlePlayChance)
				{
					IdleFloat = (hasHelmet ? UnityEngine.Random.Range(1, idleRandomNumberLimitHelmet) : UnityEngine.Random.Range(0, idleRandomNumberLimitHelmet));
				}
				else
				{
					IdleFloat = 0f;
				}
			}
		}
		else
		{
			isIdle = false;
		}
	}

	public virtual void Update()
	{
		if (running && (showInactive || (protoCrewMember != null && !protoCrewMember.inactive)))
		{
			UpdateExpressionAI();
			animator.SetFloat(expressionHash, expression);
			animator.SetFloat(varianceHash, variance);
			animator.SetFloat(secondaryVarianceHash, secondaryVariance);
			if (isIVAController)
			{
				animator.SetBool(idleBoolHash, isIdle);
				animator.SetFloat(idleFloatHash, IdleFloat);
				animator.SetBool(hasHelmetHash, hasHelmet);
			}
		}
	}

	public virtual void UpdateExpressionAI()
	{
		if (isIVAController)
		{
			IVAUpdate();
		}
		else
		{
			EVAUpdate();
		}
	}

	public virtual void IVAUpdate()
	{
		wheeLevel = 0f;
		wheeLevel += (1f - Mathf.Clamp01(Mathf.Abs(flight_gee))) * 0.6f;
		wheeLevel += (isBadass ? (flight_velocity / 100f) : 0f);
		wheeLevel += Mathf.Clamp01(flight_angularV / 40f) * stupidity;
		wheeLevel -= fearFactor;
		wheeLevel *= 1f + stupidity * 2f;
		panicLevel = (isBadass ? 0f : (flight_velocity / 100f * (1f - stupidity)));
		panicLevel += (isBadass ? (flight_angularV / 100f) : (flight_angularV / 50f));
		if (protoCrewMember != null)
		{
			panicLevel += (float)(Math.Sqrt(Math.Max(0.0, protoCrewMember.GExperiencedNormalized - (isBadass ? 0.75 : 0.5))) * 0.5);
		}
		panicLevel += fearFactor / (1f + stupidity);
		fearFactor = ((panicLevel < 0.3f) ? (fearFactor * (1f - courage * 0.5f)) : (fearFactor * (1f - courage * 0.2f)));
		wheeLevel = Mathf.Clamp01(wheeLevel);
		panicLevel = Mathf.Clamp01(panicLevel);
		targetExpression = (wheeLevel * 0.5f - panicLevel * 0.5f) * 2f;
		expressionSampleCache[currentExpressionCacheIndex] = targetExpression;
		currentExpressionCacheIndex = (currentExpressionCacheIndex + 1) % expressionSampleSize;
		for (int i = 0; i < expressionSampleSize; i++)
		{
			normalizedExpression += expressionSampleCache[i];
		}
		normalizedExpression /= expressionSampleSize;
		expression = Mathf.Lerp(expression, normalizedExpression, Time.deltaTime * expressionLerpRate);
		normalizedExpression = 0f;
	}

	public virtual void EVAUpdate()
	{
		string text = "null";
		if (kerbalEVA != null && kerbalEVA.Ready)
		{
			text = kerbalEVA.fsm.CurrentState.name;
			isJetpackOn = kerbalEVA.JetpackDeployed;
		}
		wheeLevel = 0f;
		wheeLevel += (1f - Mathf.Clamp01(Mathf.Abs(flight_gee))) * 0.6f;
		wheeLevel += (isBadass ? (flight_velocity / 100f) : 0f);
		wheeLevel += Mathf.Clamp01(flight_angularV / 40f) * stupidity;
		wheeLevel -= fearFactor;
		if (isJetpackOn)
		{
			wheeLevel += 0.3f;
		}
		wheeLevel *= 1f + stupidity * 2f;
		panicLevel = (isBadass ? 0f : (flight_velocity / 100f * (1f - stupidity)));
		panicLevel += (isBadass ? (flight_angularV / 100f) : (flight_angularV / 50f));
		panicLevel += fearFactor / (1f + stupidity);
		if (protoCrewMember != null)
		{
			panicLevel += (float)(Math.Sqrt(Math.Max(0.0, protoCrewMember.GExperiencedNormalized - (isBadass ? 0.75 : 0.5))) * 0.5);
		}
		switch (text)
		{
		case "Idle_b (Grounded)":
			if (useNewIdleExpressions)
			{
				if (newIdleWheeLevel > 0f)
				{
					wheeLevel = newIdleWheeLevel;
				}
				if (newIdleFearFactor > 0f)
				{
					fearFactor = newIdleFearFactor;
				}
			}
			break;
		case "Ragdoll":
			if (!isBadass)
			{
				fearFactor = Mathf.Clamp(0.4f * ((float)evaPart.vessel.verticalSpeed + (float)evaPart.vessel.horizontalSrfSpeed), 0f, 1f);
			}
			else
			{
				fearFactor = Mathf.Clamp(0.1f * ((float)evaPart.vessel.verticalSpeed + (float)evaPart.vessel.horizontalSrfSpeed), 0f, 1f);
			}
			break;
		case "Fall":
			if (!isBadass)
			{
				fearFactor = Mathf.Clamp(((float)evaPart.vessel.verticalSpeed + (float)evaPart.vessel.horizontalSrfSpeed) / 100f, 0f, 1f);
			}
			else
			{
				wheeLevel += Mathf.Clamp(((float)evaPart.vessel.verticalSpeed + (float)evaPart.vessel.horizontalSrfSpeed) / 100f, 0f, 1f);
			}
			break;
		case "Idle (Grounded)":
			if (protoCrewMember.gender == ProtoCrewMember.Gender.Male)
			{
				wheeLevel = 0.35f;
				panicLevel = 0f;
				variance = 0f;
				secondaryVariance = 0f;
			}
			else
			{
				wheeLevel = 0.1f;
				panicLevel = 0f;
				variance = 0.1f;
				secondaryVariance = 0.9f;
			}
			break;
		}
		fearFactor = ((panicLevel < 0.3f) ? (fearFactor * (1f - courage * 0.5f)) : (fearFactor * (1f - courage * 0.2f)));
		wheeLevel = Mathf.Clamp01(wheeLevel);
		panicLevel = Mathf.Clamp01(panicLevel);
		targetExpression = (wheeLevel * 0.5f - panicLevel * 0.5f) * 2f;
		expression = Mathf.Lerp(expression, targetExpression, Time.deltaTime * expressionLerpRate);
	}

	public IEnumerator SetNewIdleExpression()
	{
		yield return new WaitForSeconds(startExpressionTime);
		useNewIdleExpressions = true;
	}
}
