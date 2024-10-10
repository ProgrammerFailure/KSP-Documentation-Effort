using ns11;
using UnityEngine;

public class TutorialDuna : TutorialScenario
{
	public string stateName = "welcome";

	public bool complete;

	public TutorialPage welcome;

	public TutorialPage grappleDeploy;

	public TutorialPage BeginApproach;

	public TutorialPage asteroidCatched;

	public TutorialPage showtime;

	public TutorialPage alignment;

	public TutorialPage attitude3;

	public TutorialPage lowerPe3;

	public TutorialPage closeToAsteroid;

	public TutorialPage conclusion;

	public TutorialPage conclusion2;

	public TutorialPage conclusion3;

	public TutorialPage conclusion4;

	public override void OnAssetSetup()
	{
		instructorPrefabName = "Instructor_Gene";
	}

	public override void OnTutorialSetup()
	{
		welcome = new TutorialPage("welcome");
		welcome.windowTitle = "Interplanetary Travel";
		welcome.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		welcome.dialog = new MultiOptionDialog(welcome.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel("So you've gotten to the Mun already, right? That's good, but it's easy, we're going to go over a more complex procedure today, getting to another planet directly from a low Kerbin orbit. The difficulty of the task depends a lot on the planet you intend to visit. One obvious reason is that farther Celestial Bodies need more dV to be reached, but you also have to take the CB's characteristics into account, such as atmosphere, gravity and the like. \n\nSo when choosing a planet to visit your first stop should be the Tracking Station's Knowledge Base. Here you can learn more about each CB and see if you're up to the task, but for today there's no need because I know where we're going: Duna.", expandW: false, expandH: true), new DialogGUIButton("Next", delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(welcome);
		grappleDeploy = new TutorialPage("grappleDeploy");
		grappleDeploy.windowTitle = "Duna";
		grappleDeploy.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_nodB);
		};
		grappleDeploy.dialog = new MultiOptionDialog(grappleDeploy.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel("Duna, the red dot, the bringer of contrast. Let's demystify this CB, shall we?\n\nOpen Map view, change focus to Duna ([Tab] to iterate). Access the Knowledge Base when Duna is highlighted via th planete icon on the right of the screen. \n\nAs you can see one of the reasons why we chose Duna for this particular mission is because of its conditions, it has a quite thin atmosphere and it has a pretty low gravity compared to Kerbin, so it should be a simple enough mission for even a newcomer.", expandW: false, expandH: true), new DialogGUIButton("Next", delegate
		{
			Tutorial.GoToNextPage();
		}, delegate
		{
			Transform transform = Object.FindObjectOfType<KnowledgeBase>().transform.Find("KnowledgeContainer");
			GameObject gameObject = null;
			int childCount = transform.childCount;
			while (childCount-- > 0)
			{
				Transform child = transform.GetChild(childCount);
				if (child.name == "Planets")
				{
					gameObject = child.gameObject;
				}
			}
			return (gameObject != null && gameObject.activeInHierarchy && PlanetariumCamera.fetch.target.GetName() == "Duna") ? true : false;
		}, dismissOnSelect: true));
		Tutorial.AddPage(grappleDeploy);
		BeginApproach = new TutorialPage("BeginApproach");
		BeginApproach.windowTitle = "Oberth effect";
		BeginApproach.OnEnter = delegate
		{
			instructor.PlayEmoteRepeating(instructor.anim_idle_lookAround, 5f);
		};
		BeginApproach.dialog = new MultiOptionDialog(BeginApproach.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel("It's time for a little lecture, in order to make our transfer to Duna, or any other CB for that matter, we're going to use something called the Oberth effect.\n\nThe Periapsis is the lowest point in an orbit, and the Apoapsis the highest one, remember? Well, think about your orbit as a hill for a moment, if you're going uphill it's harder than going downhill, right? This is because your kinetic energy is lower when you're traveling to a higher point, so going back to orbits the point of highest kinetic energy of the orbit is the Periapsis, because it's where you're about to change from downhill to uphill.\n\nYou may ask yourself what all this has to do with getting to Duna, well back to the hill analogy, you get tired less if you run downhill, you use less energy, it's the same here, you burn less fuel if your kinetic energy is at its highest.", expandW: false, expandH: true), new DialogGUIButton("Next", delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(BeginApproach);
		asteroidCatched = new TutorialPage("asteroidCatched");
		asteroidCatched.windowTitle = "Phase Angles";
		asteroidCatched.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_idle_lookAround);
		};
		asteroidCatched.dialog = new MultiOptionDialog(asteroidCatched.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel("When setting up a transfer orbit using the Oberth effect you usually have to be very patient, it's all about being in the right place at the right time, sometimes it can take weeks, months or even years.\n\nIf you look at your mission timer you can see we spawned you close to the transfer window. Outside of training I recommend you speed up time in the Tracking Station if you don't want to wait that long. But how do you know exactly how long you have to wait? Well, that's complicated. The simplest way to guide you is with something called phase angles between the Kerbin and the CB you want to go to taking the Sun as reference. The phase angle to use on Duna is about 45 degrees. Calculating phase angles is beyond the scope of this tutorial, they're not too hard to find pre-calculated in \"external resources\".", expandW: false, expandH: true), new DialogGUIButton("Next", delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(asteroidCatched);
		showtime = new TutorialPage("showtime");
		showtime.windowTitle = "Interplanetary transfer orbit";
		showtime.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_idle_lookAround);
		};
		showtime.dialog = new MultiOptionDialog(showtime.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel("So let's get into action, press [" + GameSettings.CAMERA_RESET.name + "] to focus your ship, create a maneuver node around the point of \"sunrise\" in Kerbin and pull the prograde handle until you intersect Duna's orbit, move the node around Kerbin until you get an encounter node with Duna’s SOI.\n\n Following the same Oberth effect lecture, try getting the encounter as close as possible to Duna's Apoapsis, so its kinetic energy is at the lowest and you have less trouble catching up to it, this is optional, but it's the most efficient way to do it.", expandW: false, expandH: true), new DialogGUIButton("Next", delegate
		{
			Tutorial.GoToNextPage();
		}, delegate
		{
			bool flag = false;
			if (FlightGlobals.ActiveVessel.patchedConicSolver != null)
			{
				int count = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count;
				while (count-- > 0)
				{
					ManeuverNode maneuverNode = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[count];
					if (!(maneuverNode.double_0 < Planetarium.GetUniversalTime()) && ((maneuverNode.patch != null && maneuverNode.patch.referenceBody != null && maneuverNode.patch.referenceBody.name == "Duna") || (maneuverNode.nextPatch != null && maneuverNode.nextPatch.referenceBody != null && maneuverNode.nextPatch.referenceBody.name == "Duna") || (maneuverNode.nextPatch.nextPatch != null && maneuverNode.nextPatch.nextPatch.referenceBody != null && maneuverNode.nextPatch.nextPatch.referenceBody.name == "Duna") || (maneuverNode.nextPatch.nextPatch.nextPatch != null && maneuverNode.nextPatch.nextPatch.nextPatch.referenceBody != null && maneuverNode.nextPatch.nextPatch.nextPatch.referenceBody.name == "Duna")))
					{
						flag = true;
						break;
					}
				}
			}
			return flag ? true : false;
		}, dismissOnSelect: true));
		Tutorial.AddPage(showtime);
		alignment = new TutorialPage("alignment");
		alignment.windowTitle = "Correction maneuver";
		alignment.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_idle_lookAround);
		};
		alignment.dialog = new MultiOptionDialog(alignment.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel("That's right! That trajectory looks great. It's going to be quite a burn, some 2 - 3 minutes most likely so make sure you plan your times right, remember that you want to burn half before and half after the node for efficiency.\n\nAfter the burn you may or may not get exactly the desired result, it's okay, it's quite an amazing feat to set a interplanetary landing course in just one maneuver, even for the more skilled. So we're going to set up a correction node, since your orbit and Duna's are not exactly aligned I suggest you put it on the Descending Node and adjust your Normal and Radial vectors slightly. \nThis adjustment node is crucial to ensure a successful landing on Duna, so take your time setting it up, when you're happy with the results press Next.", expandW: false, expandH: true), new DialogGUIButton("Next", delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(alignment);
		attitude3 = new TutorialPage("attitude3");
		attitude3.windowTitle = "Encounter";
		attitude3.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_nodA);
		};
		attitude3.dialog = new MultiOptionDialog(attitude3.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel("Execute that burn, if you do everything correctly you should see the encounter node even after removing the maneuver node. If not I recommend you set up another correction maneuver as soon as possible, the closer you are to the Celestial Body you want to reach the more dV it takes to correct the course.\n\nBesides that all that's left is to wait until you reach your close encounter, if you want to go crazy with the Time Warp go ahead, but make sure to stop it a little before the encounter node, you could miss your encounter altogether if you're not careful.", expandW: false, expandH: true));
		attitude3.SetAdvanceCondition((KFSMState KFSMEvent) => FlightGlobals.ActiveVessel.orbitDriver.referenceBody.name == "Duna");
		Tutorial.AddPage(attitude3);
		lowerPe3 = new TutorialPage("lowerPe3");
		lowerPe3.windowTitle = "More corrections";
		lowerPe3.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		lowerPe3.dialog = new MultiOptionDialog(lowerPe3.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel("You are now on Duna's close proximity, but you're not done yet, a few things could have gone a little wrong.\n\nOne thing that happens often is that Ike, Duna's satellite, is in the way, your trajectory leads to an Ike encounter. This is common because Ike's SOI is actually bigger than Duna's. If this happens create a maneuver node as close as possible to your current position and move the Radial vector a little away from Duna, burn and wait until you are close to Ike's orbit, then create another maneuver to get back closer to Duna.\n\nIn any case create a node and pull the retrograde vector to create a stable low orbit around Duna, this will give us a better chance of success. This is not mandatory, if you're full of daring and/or can't spare the fuel you could set a collision course and proceed to land.", expandW: false, expandH: true), new DialogGUIButton("Next", delegate
		{
			Tutorial.GoToNextPage();
		}, () => (FlightGlobals.ActiveVessel.orbit.eccentricity <= 0.2 || FlightGlobals.ActiveVessel.situation == Vessel.Situations.LANDED) ? true : false, dismissOnSelect: true));
		Tutorial.AddPage(lowerPe3);
		closeToAsteroid = new TutorialPage("closeToAsteroid");
		closeToAsteroid.windowTitle = "Great!";
		closeToAsteroid.OnEnter = delegate
		{
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		closeToAsteroid.dialog = new MultiOptionDialog(closeToAsteroid.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(() => (FlightGlobals.ActiveVessel.situation == Vessel.Situations.LANDED) ? "You landed at Duna! Excelent! \nAll the very same principles you used here apply to traveling to any other planet or Celestial Body in General, so you are definitely ready to go on and conquer space. \n\nI'm really proud of you, I know you have potential." : "Well done! You are in a great position to just lower your orbit and start descending on duna.\n\n When landing remember to deploy your ship's legs by right clicking them and clicking the button and aerobraking burning retrograde to reduce your speed. In a planet such as Duna a parachute helps somewhat, but it won't be enough to break your fall.\n\n All the very same principles you used here apply to traveling to any other planet or Celestial Body in General, so you are definitely ready to go on and conquer space. \n\nI'm really proud of you, I know you have potential.", expandW: false, expandH: true), new DialogGUIButton("Finish", delegate
		{
			CompleteTutorial();
		}, dismissOnSelect: true));
		Tutorial.AddPage(closeToAsteroid);
		Tutorial.StartTutorial(welcome);
	}

	public override void OnOnDestroy()
	{
		InputLockManager.RemoveControlLock("docking");
	}
}
