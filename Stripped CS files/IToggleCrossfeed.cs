public interface IToggleCrossfeed
{
	bool CrossfeedToggleableEditor();

	bool CrossfeedToggleableFlight();

	bool CrossfeedRequiresTech();

	string CrossfeedTech();

	bool CrossfeedHasTech();
}
