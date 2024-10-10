using System.Collections.Generic;
using UnityEngine;

public interface IAxisFieldLimits
{
	Callback<AxisFieldLimit> LimitsChanged { get; set; }

	bool HasAxisFieldLimits();

	bool HasAxisFieldLimit(string fieldName);

	List<AxisFieldLimit> GetAxisFieldLimits();

	AxisFieldLimit GetAxisFieldLimit(string fieldName);

	Vector2 GetHardLimits(string fieldName);

	Vector2 GetSoftLimits(string fieldName);

	void SetHardLimits(string fieldName, Vector2 newLimits);

	void SetSoftLimits(string fieldName, Vector2 newLimits);
}
