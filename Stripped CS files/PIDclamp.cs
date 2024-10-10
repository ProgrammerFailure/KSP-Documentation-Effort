using System;

[Serializable]
public class PIDclamp : IPid
{
	public string name;

	public double kp;

	public double ki;

	public double kd;

	public double clamp;

	public float tuningScalar = 1f;

	public float kpScalar = 1f;

	public float kiScalar = 1f;

	public float kdScalar = 1f;

	public float clampScalar = 1f;

	public double integral;

	public double integralLast;

	public double derivative;

	public double errorLast;

	public double output;

	public bool ignoreIntegral;

	public bool clampIntegral;

	public double integralMax = 1.0;

	public PIDclamp(string name, float kp, float ki, float kd, float clamp)
	{
		this.name = name;
		this.kp = kp;
		this.ki = ki;
		this.kd = kd;
		this.clamp = clamp;
	}

	public void ClampIntegral(float limit, bool isEnabled)
	{
		clampIntegral = isEnabled;
		integralMax = limit;
	}

	public void Reinitialize(string name, float kp, float ki, float kd)
	{
		this.name = name;
		Reinitialize(kp, ki, kd);
	}

	public void Reinitialize(float kp, float ki, float kd)
	{
		this.kp = kp;
		this.ki = ki;
		this.kd = kd;
		Reset();
	}

	public void Reset()
	{
		integral = 0.0;
		errorLast = 0.0;
	}

	public void ResetIntegral()
	{
		integral = 0.0;
	}

	public double Update(double error, double dt)
	{
		integralLast = integral;
		if (!ignoreIntegral)
		{
			integral += error * dt;
			if (clampIntegral)
			{
				integral = UtilMath.Clamp(integral += error * dt, 0.0 - integralMax, integralMax);
			}
		}
		else
		{
			integral = 0.0;
		}
		derivative = (error - errorLast) / dt;
		errorLast = error;
		output = kp * (double)tuningScalar * (double)kpScalar * error + ki * (double)tuningScalar * (double)kiScalar * integral + kd * (double)tuningScalar * (double)kdScalar * derivative;
		double num = clamp * (double)clampScalar;
		if (output < 0.0 - num)
		{
			output = 0.0 - num;
			integral = integralLast;
		}
		else if (output > num)
		{
			output = num;
			integral = integralLast;
		}
		return output;
	}

	public float Update(float error, float dt)
	{
		return (float)Update((double)error, (double)dt);
	}

	public static implicit operator double(PIDclamp v)
	{
		return v.output;
	}

	public override string ToString()
	{
		return name + "#" + kp + "#" + ki + "#" + kd + "#" + clamp;
	}

	public static PIDclamp Parse(string s)
	{
		string[] array = s.Split('#');
		return new PIDclamp(array[0], float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]), float.Parse(array[4]));
	}
}
