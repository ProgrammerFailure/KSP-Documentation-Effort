using System.Collections.Generic;

namespace PreFlightTests;

public interface IDesignConcern
{
	event Callback<bool> testResultChanged;

	bool Test();

	List<Part> GetAffectedParts();

	string GetConcernTitle();

	string GetConcernDescription();

	bool GetPreviousResult();

	EditorFacilities GetEditorFacilities();

	DesignConcernSeverity GetSeverity();
}
