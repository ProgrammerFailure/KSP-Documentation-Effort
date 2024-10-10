using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

public class Wheel : Block
{
	public TireFriction tireFriction;

	public float radius;

	public float mass;

	public float float_0;

	public float invI;

	public float load;

	public float grip;

	public Vector2 vector2_0;

	public Vector2 Fext;

	public float m_brakeTorque;

	public float float_1;

	public Vector2 vector2_1;

	public float w;

	public float float_2;

	public Vector2 vector2_2;

	public float Tr;

	public bool isResting;

	public float m_forwardSlip;

	public Vector2 m_tireForce;

	public float m_Ty;

	public bool m_isAdherent;

	public Vector2 m_adherentSlip;

	public Vector2 m_adherentForce;

	public float m_inTd;

	public float m_inTb;

	public TireFriction.ContactPatch m_contact = new TireFriction.ContactPatch();

	public float debug1;

	public float debug2;

	public float debug3;

	public float inertia => float_0;

	public float angularVelocity => w;

	public float driveTorque => m_inTd;

	public float brakeTorque => m_inTb;

	public TireFriction.ContactPatch contactPatch => m_contact;

	public void RecalculateConstants()
	{
		float_0 = mass * radius * radius * 0.5f;
		if (float_0 < 0.01f)
		{
			float_0 = 0.01f;
		}
		invI = 1f / float_0;
	}

	public float AddBrakeTorque(float brakeTorque)
	{
		m_brakeTorque += brakeTorque;
		if (m_brakeTorque < 0f)
		{
			m_brakeTorque = 0f;
		}
		return m_brakeTorque;
	}

	public void ResetBrakeTorque()
	{
		m_brakeTorque = 0f;
	}

	public void RecalculateVars()
	{
		w = float_1 * invI;
	}

	public override void Initialize()
	{
		SetInputs(1);
		SetOutputs(0);
		tireFriction = new TireFriction();
	}

	public override void GetState(ref State state_0)
	{
		state_0.float_0 = float_1;
		state_0.vector2_0 = Vector2.zero;
		state_0.Lr = 0f;
		isResting = true;
	}

	public override void SetSubstepState(State state_0)
	{
		float_1 = state_0.float_0;
		vector2_1 = state_0.vector2_0;
		RecalculateVars();
	}

	public override void ComputeStateUpstream()
	{
		m_forwardSlip = (float)((double)w * (double)radius - (double)vector2_0.y);
		Vector2 slip = new Vector2(vector2_0.x, m_forwardSlip);
		m_contact.load = load;
		m_contact.slip = slip;
		m_contact.groundGrip = grip;
		m_tireForce = tireFriction.GetForce(m_contact);
		m_Ty = m_tireForce.y * radius;
		m_isAdherent = tireFriction.IsAdherentSlip(m_contact);
		if (m_isAdherent)
		{
			m_adherentSlip.x = slip.x;
			m_adherentSlip.y = tireFriction.GetAdherentSlipForward(m_contact);
			m_contact.slip = m_adherentSlip;
			m_contact.slip.y *= Mathf.Sign(slip.y);
			m_adherentForce = tireFriction.GetAdherentForce(m_contact);
			float num = m_adherentForce.y * radius;
			if (MathUtility.FastAbs(m_Ty) < MathUtility.FastAbs(num))
			{
				m_Ty = num;
			}
		}
		else
		{
			m_adherentSlip = Vector2.zero;
			m_adherentForce = Vector2.zero;
		}
		Connection connection = base.inputs[0];
		if (connection != null)
		{
			connection.float_0 = float_1;
			connection.float_1 = float_0;
			connection.Tr = Tr;
		}
	}

	public override void EvaluateTorqueDownstream()
	{
		m_inTd = base.inputs[0]?.outTd ?? 0f;
		m_inTb = m_brakeTorque;
		float num = m_inTb * Mathf.Sign(0f - float_1);
		float tsum = m_inTd + m_Ty + num;
		float num2 = (0f - float_1) / Solver.deltaTime;
		float num3 = (float)((double)vector2_0.y / (double)radius - (double)w) * float_0 / Solver.deltaTime;
		float num4 = MathUtility.FastAbs(num2);
		float num5 = MathUtility.FastAbs(num3);
		if (!(num4 < num5) && MathUtility.FastAbs(num5 - num4) >= 1E-06f)
		{
			tsum = SolveSymmetricConstraint(tsum, m_Ty, num3);
			tsum = SolveSymmetricConstraint(tsum, num, num2);
		}
		else
		{
			tsum = SolveSymmetricConstraint(tsum, num, num2);
			tsum = SolveSymmetricConstraint(tsum, m_Ty, num3);
		}
		float_2 = tsum;
		Tr = float_2 - m_inTd;
		float num6 = Mathf.Clamp(Fext.y * radius - m_inTd, 0f - m_inTb, m_inTb) + m_inTd;
		float num9;
		if (num3 <= 0f)
		{
			float num7 = Mathf.Max(m_Ty, num3);
			if (num6 < 0f)
			{
				float num8 = m_Ty * Mathf.InverseLerp(m_adherentSlip.y * Mathf.Sign(m_forwardSlip), 0f, m_forwardSlip);
				if (num6 < num8)
				{
					num6 = num8;
				}
				num9 = num6 - num7;
			}
			else
			{
				num9 = Mathf.Clamp(num6, 0f - num7, 0f - m_Ty);
			}
		}
		else
		{
			float num7 = Mathf.Min(m_Ty, num3);
			if (num6 > 0f)
			{
				float num10 = m_Ty * Mathf.InverseLerp(m_adherentSlip.y * Mathf.Sign(m_forwardSlip), 0f, m_forwardSlip);
				if (num6 > num10)
				{
					num6 = num10;
				}
				num9 = num6 - num7;
			}
			else
			{
				num9 = Mathf.Clamp(num6, 0f - m_Ty, 0f - num7);
			}
		}
		vector2_2.y = num9 / radius;
		float x;
		if (m_isAdherent)
		{
			m_adherentSlip.y = m_adherentSlip.y * vector2_2.y / m_adherentForce.y;
			m_contact.slip = m_adherentSlip;
			m_adherentSlip.x = tireFriction.GetAdherentSlipSideways(m_contact);
			m_contact.slip = m_adherentSlip;
			m_adherentForce = tireFriction.GetAdherentForce(m_contact);
			x = m_adherentForce.x;
		}
		else
		{
			x = m_tireForce.x;
		}
		x = MathUtility.FastAbs(x);
		vector2_2.x = Mathf.Clamp(Fext.x, 0f - x, x);
		isResting &= x > MathUtility.FastAbs(Fext.x);
		isResting &= m_inTb > MathUtility.FastAbs(Fext.y * radius - m_inTd);
	}

	public override void GetSubstepDerivative(ref Derivative derivative_0)
	{
		derivative_0.float_0 = float_2;
		derivative_0.vector2_0 = vector2_2;
		derivative_0.Tr = Tr;
	}

	public override void SetState(State state_0)
	{
		float_1 = state_0.float_0;
		vector2_2 = state_0.vector2_0 / Solver.deltaTime;
		Tr = state_0.Lr / Solver.deltaTime;
		RecalculateVars();
		m_brakeTorque = 0f;
	}

	public static float SolveSymmetricConstraint(float Tsum, float float_3, float Tmax)
	{
		bool flag = Tsum >= 0f;
		bool flag2 = Tmax >= 0f;
		float num = (flag ? Tsum : (0f - Tsum));
		float num2 = (flag2 ? Tmax : (0f - Tmax));
		if (Tmax == 0f || (flag == flag2 && num > num2))
		{
			float t = Mathf.InverseLerp(0f, Tsum * 0.5f, float_3);
			Tsum = MathUtility.UnclampedLerp(Tsum, Tmax, t);
		}
		return Tsum;
	}
}
