public interface IDiscoverable
{
	DiscoveryInfo DiscoveryInfo { get; }

	string RevealName();

	string RevealDisplayName();

	double RevealSpeed();

	double RevealAltitude();

	string RevealSituationString();

	string RevealType();

	float RevealMass();
}
