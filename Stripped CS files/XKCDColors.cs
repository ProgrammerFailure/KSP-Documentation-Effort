using System;
using UnityEngine;

public static class XKCDColors
{
	public class ColorTranslator
	{
		public static Color FromHtml(string hexString)
		{
			if (hexString.Length == 7)
			{
				return new Color((float)Convert.ToInt32(hexString.Substring(1, 2), 16) / 255f, (float)Convert.ToInt32(hexString.Substring(3, 2), 16) / 255f, (float)Convert.ToInt32(hexString.Substring(5, 2), 16) / 255f, 1f);
			}
			if (hexString.Length != 9)
			{
				throw new ArgumentException("Invalid Hex Format: " + hexString + " -- Acceptable formats are either #rrggbb or #rrggbbaa");
			}
			return new Color((float)Convert.ToInt32(hexString.Substring(1, 2), 16) / 255f, (float)Convert.ToInt32(hexString.Substring(3, 2), 16) / 255f, (float)Convert.ToInt32(hexString.Substring(5, 2), 16) / 255f, (float)Convert.ToInt32(hexString.Substring(7, 2), 16) / 255f);
		}

		public static string ToHex(Color c)
		{
			return "#" + ((int)(c.r * 255f)).ToString("x2") + ((int)(c.g * 255f)).ToString("x2") + ((int)(c.b * 255f)).ToString("x2");
		}

		public static string ToHexA(Color c)
		{
			return "#" + ((int)(c.r * 255f)).ToString("x2") + ((int)(c.g * 255f)).ToString("x2") + ((int)(c.b * 255f)).ToString("x2") + ((int)(c.a * 255f)).ToString("x2");
		}
	}

	public class HexFormat
	{
		public static string KSPBadassGreen => "#b4d455";

		public static string KSPNotSoGoodOrange => "#feb200";

		public static string KSPUIGrey => "#8a939c";

		public static string KSPUnnamedCyan => "#5fbcb8";

		public static string KSPNeutralUIGrey => "#fefefe";

		public static string CloudyBlue => "#acc2d9";

		public static string DarkPastelGreen => "#56ae57";

		public static string Dust => "#b2996e";

		public static string ElectricLime => "#a8ff04";

		public static string FreshGreen => "#69d84f";

		public static string LightEggplant => "#894585";

		public static string NastyGreen => "#70b23f";

		public static string ReallyLightBlue => "#d4ffff";

		public static string Tea => "#65ab7c";

		public static string WarmPurple => "#952e8f";

		public static string YellowishTan => "#fcfc81";

		public static string Cement => "#a5a391";

		public static string DarkGrassGreen => "#388004";

		public static string DustyTeal => "#4c9085";

		public static string GreyTeal => "#5e9b8a";

		public static string MacaroniAndCheese => "#efb435";

		public static string PinkishTan => "#d99b82";

		public static string Spruce => "#0a5f38";

		public static string StrongBlue => "#0c06f7";

		public static string ToxicGreen => "#61de2a";

		public static string WindowsBlue => "#3778bf";

		public static string BlueBlue => "#2242c7";

		public static string BlueWithAHintOfPurple => "#533cc6";

		public static string Booger => "#9bb53c";

		public static string BrightSeaGreen => "#05ffa6";

		public static string DarkGreenBlue => "#1f6357";

		public static string DeepTurquoise => "#017374";

		public static string GreenTeal => "#0cb577";

		public static string StrongPink => "#ff0789";

		public static string Bland => "#afa88b";

		public static string DeepAqua => "#08787f";

		public static string LavenderPink => "#dd85d7";

		public static string LightMossGreen => "#a6c875";

		public static string LightSeafoamGreen => "#a7ffb5";

		public static string OliveYellow => "#c2b709";

		public static string PigPink => "#e78ea5";

		public static string DeepLilac => "#966ebd";

		public static string Desert => "#ccad60";

		public static string DustyLavender => "#ac86a8";

		public static string PurpleyGrey => "#947e94";

		public static string Purply => "#983fb2";

		public static string CandyPink => "#ff63e9";

		public static string LightPastelGreen => "#b2fba5";

		public static string BoringGreen => "#63b365";

		public static string KiwiGreen => "#8ee53f";

		public static string LightGreyGreen => "#b7e1a1";

		public static string OrangePink => "#ff6f52";

		public static string TeaGreen => "#bdf8a3";

		public static string VeryLightBrown => "#d3b683";

		public static string EggShell => "#fffcc4";

		public static string EggplantPurple => "#430541";

		public static string PowderPink => "#ffb2d0";

		public static string ReddishGrey => "#997570";

		public static string BabyShitBrown => "#ad900d";

		public static string Liliac => "#c48efd";

		public static string StormyBlue => "#507b9c";

		public static string UglyBrown => "#7d7103";

		public static string Custard => "#fffd78";

		public static string DarkishPink => "#da467d";

		public static string DeepBrown => "#410200";

		public static string GreenishBeige => "#c9d179";

		public static string Manilla => "#fffa86";

		public static string OffBlue => "#5684ae";

		public static string BattleshipGrey => "#6b7c85";

		public static string BrownyGreen => "#6f6c0a";

		public static string Bruise => "#7e4071";

		public static string KelleyGreen => "#009337";

		public static string SicklyYellow => "#d0e429";

		public static string SunnyYellow => "#fff917";

		public static string Azul => "#1d5dec";

		public static string Darkgreen => "#054907";

		public static string Green_Yellow => "#b5ce08";

		public static string Lichen => "#8fb67b";

		public static string LightLightGreen => "#c8ffb0";

		public static string PaleGold => "#fdde6c";

		public static string SunYellow => "#ffdf22";

		public static string TanGreen => "#a9be70";

		public static string Burple => "#6832e3";

		public static string Butterscotch => "#fdb147";

		public static string Toupe => "#c7ac7d";

		public static string DarkCream => "#fff39a";

		public static string IndianRed => "#850e04";

		public static string LightLavendar => "#efc0fe";

		public static string PoisonGreen => "#40fd14";

		public static string BabyPukeGreen => "#b6c406";

		public static string BrightYellowGreen => "#9dff00";

		public static string CharcoalGrey => "#3c4142";

		public static string Squash => "#f2ab15";

		public static string Cinnamon => "#ac4f06";

		public static string LightPeaGreen => "#c4fe82";

		public static string RadioactiveGreen => "#2cfa1f";

		public static string RawSienna => "#9a6200";

		public static string BabyPurple => "#ca9bf7";

		public static string Cocoa => "#875f42";

		public static string LightRoyalBlue => "#3a2efe";

		public static string Orangeish => "#fd8d49";

		public static string RustBrown => "#8b3103";

		public static string SandBrown => "#cba560";

		public static string Swamp => "#698339";

		public static string TealishGreen => "#0cdc73";

		public static string BurntSiena => "#b75203";

		public static string Camo => "#7f8f4e";

		public static string DuskBlue => "#26538d";

		public static string Fern => "#63a950";

		public static string OldRose => "#c87f89";

		public static string PaleLightGreen => "#b1fc99";

		public static string PeachyPink => "#ff9a8a";

		public static string RosyPink => "#f6688e";

		public static string LightBluishGreen => "#76fda8";

		public static string LightBrightGreen => "#53fe5c";

		public static string LightNeonGreen => "#4efd54";

		public static string LightSeafoam => "#a0febf";

		public static string TiffanyBlue => "#7bf2da";

		public static string WashedOutGreen => "#bcf5a6";

		public static string BrownyOrange => "#ca6b02";

		public static string NiceBlue => "#107ab0";

		public static string Sapphire => "#2138ab";

		public static string GreyishTeal => "#719f91";

		public static string OrangeyYellow => "#fdb915";

		public static string Parchment => "#fefcaf";

		public static string Straw => "#fcf679";

		public static string VeryDarkBrown => "#1d0200";

		public static string Terracota => "#cb6843";

		public static string UglyBlue => "#31668a";

		public static string ClearBlue => "#247afd";

		public static string Creme => "#ffffb6";

		public static string FoamGreen => "#90fda9";

		public static string Grey_Green => "#86a17d";

		public static string LightGold => "#fddc5c";

		public static string SeafoamBlue => "#78d1b6";

		public static string Topaz => "#13bbaf";

		public static string VioletPink => "#fb5ffc";

		public static string Wintergreen => "#20f986";

		public static string YellowTan => "#ffe36e";

		public static string DarkFuchsia => "#9d0759";

		public static string IndigoBlue => "#3a18b1";

		public static string LightYellowishGreen => "#c2ff89";

		public static string PaleMagenta => "#d767ad";

		public static string RichPurple => "#720058";

		public static string SunflowerYellow => "#ffda03";

		public static string Green_Blue => "#01c08d";

		public static string Leather => "#ac7434";

		public static string RacingGreen => "#014600";

		public static string VividPurple => "#9900fa";

		public static string DarkRoyalBlue => "#02066f";

		public static string Hazel => "#8e7618";

		public static string MutedPink => "#d1768f";

		public static string BoogerGreen => "#96b403";

		public static string Canary => "#fdff63";

		public static string CoolGrey => "#95a3a6";

		public static string DarkTaupe => "#7f684e";

		public static string DarkishPurple => "#751973";

		public static string TrueGreen => "#089404";

		public static string CoralPink => "#ff6163";

		public static string DarkSage => "#598556";

		public static string DarkSlateBlue => "#214761";

		public static string FlatBlue => "#3c73a8";

		public static string Mushroom => "#ba9e88";

		public static string RichBlue => "#021bf9";

		public static string DirtyPurple => "#734a65";

		public static string Greenblue => "#23c48b";

		public static string IckyGreen => "#8fae22";

		public static string LightKhaki => "#e6f2a2";

		public static string WarmBlue => "#4b57db";

		public static string DarkHotPink => "#d90166";

		public static string DeepSeaBlue => "#015482";

		public static string Carmine => "#9d0216";

		public static string DarkYellowGreen => "#728f02";

		public static string PalePeach => "#ffe5ad";

		public static string PlumPurple => "#4e0550";

		public static string GoldenRod => "#f9bc08";

		public static string NeonRed => "#ff073a";

		public static string OldPink => "#c77986";

		public static string VeryPaleBlue => "#d6fffe";

		public static string BloodOrange => "#fe4b03";

		public static string Grapefruit => "#fd5956";

		public static string SandYellow => "#fce166";

		public static string ClayBrown => "#b2713d";

		public static string DarkBlueGrey => "#1f3b4d";

		public static string FlatGreen => "#699d4c";

		public static string LightGreenBlue => "#56fca2";

		public static string WarmPink => "#fb5581";

		public static string DodgerBlue => "#3e82fc";

		public static string GrossGreen => "#a0bf16";

		public static string Ice => "#d6fffa";

		public static string MetallicBlue => "#4f738e";

		public static string PaleSalmon => "#ffb19a";

		public static string SapGreen => "#5c8b15";

		public static string Algae => "#54ac68";

		public static string BlueyGrey => "#89a0b0";

		public static string GreenyGrey => "#7ea07a";

		public static string HighlighterGreen => "#1bfc06";

		public static string LightLightBlue => "#cafffb";

		public static string LightMint => "#b6ffbb";

		public static string RawUmber => "#a75e09";

		public static string VividBlue => "#152eff";

		public static string DeepLavender => "#8d5eb7";

		public static string DullTeal => "#5f9e8f";

		public static string LightGreenishBlue => "#63f7b4";

		public static string MudGreen => "#606602";

		public static string Pinky => "#fc86aa";

		public static string RedWine => "#8c0034";

		public static string ShitGreen => "#758000";

		public static string TanBrown => "#ab7e4c";

		public static string Darkblue => "#030764";

		public static string Rosa => "#fe86a4";

		public static string Lipstick => "#d5174e";

		public static string PaleMauve => "#fed0fc";

		public static string Claret => "#680018";

		public static string Dandelion => "#fedf08";

		public static string Orangered => "#fe420f";

		public static string PoopGreen => "#6f7c00";

		public static string Ruby => "#ca0147";

		public static string Dark => "#1b2431";

		public static string GreenishTurquoise => "#00fbb0";

		public static string PastelRed => "#db5856";

		public static string PissYellow => "#ddd618";

		public static string BrightCyan => "#41fdfe";

		public static string DarkCoral => "#cf524e";

		public static string AlgaeGreen => "#21c36f";

		public static string DarkishRed => "#a90308";

		public static string ReddyBrown => "#6e1005";

		public static string BlushPink => "#fe828c";

		public static string CamouflageGreen => "#4b6113";

		public static string LawnGreen => "#4da409";

		public static string Putty => "#beae8a";

		public static string VibrantBlue => "#0339f8";

		public static string DarkSand => "#a88f59";

		public static string Purple_Blue => "#5d21d0";

		public static string Saffron => "#feb209";

		public static string Twilight => "#4e518b";

		public static string WarmBrown => "#964e02";

		public static string Bluegrey => "#85a3b2";

		public static string BubbleGumPink => "#ff69af";

		public static string DuckEggBlue => "#c3fbf4";

		public static string GreenishCyan => "#2afeb7";

		public static string Petrol => "#005f6a";

		public static string Royal => "#0c1793";

		public static string Butter => "#ffff81";

		public static string DustyOrange => "#f0833a";

		public static string OffYellow => "#f1f33f";

		public static string PaleOliveGreen => "#b1d27b";

		public static string Orangish => "#fc824a";

		public static string Leaf => "#71aa34";

		public static string LightBlueGrey => "#b7c9e2";

		public static string DriedBlood => "#4b0101";

		public static string LightishPurple => "#a552e6";

		public static string RustyRed => "#af2f0d";

		public static string LavenderBlue => "#8b88f8";

		public static string LightGrassGreen => "#9af764";

		public static string LightMintGreen => "#a6fbb2";

		public static string Sunflower => "#ffc512";

		public static string Velvet => "#750851";

		public static string BrickOrange => "#c14a09";

		public static string LightishRed => "#fe2f4a";

		public static string PureBlue => "#0203e2";

		public static string TwilightBlue => "#0a437a";

		public static string VioletRed => "#a50055";

		public static string YellowyBrown => "#ae8b0c";

		public static string Carnation => "#fd798f";

		public static string MuddyYellow => "#bfac05";

		public static string DarkSeafoamGreen => "#3eaf76";

		public static string DeepRose => "#c74767";

		public static string DustyRed => "#b9484e";

		public static string Grey_Blue => "#647d8e";

		public static string LemonLime => "#bffe28";

		public static string Purple_Pink => "#d725de";

		public static string BrownYellow => "#b29705";

		public static string PurpleBrown => "#673a3f";

		public static string Wisteria => "#a87dc2";

		public static string BananaYellow => "#fafe4b";

		public static string LipstickRed => "#c0022f";

		public static string WaterBlue => "#0e87cc";

		public static string BrownGrey => "#8d8468";

		public static string VibrantPurple => "#ad03de";

		public static string BabyGreen => "#8cff9e";

		public static string BarfGreen => "#94ac02";

		public static string EggshellBlue => "#c4fff7";

		public static string SandyYellow => "#fdee73";

		public static string CoolGreen => "#33b864";

		public static string Pale => "#fff9d0";

		public static string Blue_Grey => "#758da3";

		public static string HotMagenta => "#f504c9";

		public static string Greyblue => "#77a1b5";

		public static string Purpley => "#8756e4";

		public static string BabyShitGreen => "#889717";

		public static string BrownishPink => "#c27e79";

		public static string DarkAquamarine => "#017371";

		public static string Diarrhea => "#9f8303";

		public static string LightMustard => "#f7d560";

		public static string PaleSkyBlue => "#bdf6fe";

		public static string TurtleGreen => "#75b84f";

		public static string BrightOlive => "#9cbb04";

		public static string DarkGreyBlue => "#29465b";

		public static string GreenyBrown => "#696006";

		public static string LemonGreen => "#adf802";

		public static string LightPeriwinkle => "#c1c6fc";

		public static string SeaweedGreen => "#35ad6b";

		public static string SunshineYellow => "#fffd37";

		public static string UglyPurple => "#a442a0";

		public static string MediumPink => "#f36196";

		public static string PukeBrown => "#947706";

		public static string VeryLightPink => "#fff4f2";

		public static string Viridian => "#1e9167";

		public static string Bile => "#b5c306";

		public static string FadedYellow => "#feff7f";

		public static string VeryPaleGreen => "#cffdbc";

		public static string VibrantGreen => "#0add08";

		public static string BrightLime => "#87fd05";

		public static string Spearmint => "#1ef876";

		public static string LightAquamarine => "#7bfdc7";

		public static string LightSage => "#bcecac";

		public static string Yellowgreen => "#bbf90f";

		public static string BabyPoo => "#ab9004";

		public static string DarkSeafoam => "#1fb57a";

		public static string DeepTeal => "#00555a";

		public static string Heather => "#a484ac";

		public static string RustOrange => "#c45508";

		public static string DirtyBlue => "#3f829d";

		public static string FernGreen => "#548d44";

		public static string BrightLilac => "#c95efb";

		public static string WeirdGreen => "#3ae57f";

		public static string PeacockBlue => "#016795";

		public static string AvocadoGreen => "#87a922";

		public static string FadedOrange => "#f0944d";

		public static string GrapePurple => "#5d1451";

		public static string HotGreen => "#25ff29";

		public static string LimeYellow => "#d0fe1d";

		public static string Mango => "#ffa62b";

		public static string Shamrock => "#01b44c";

		public static string Bubblegum => "#ff6cb5";

		public static string PurplishBrown => "#6b4247";

		public static string VomitYellow => "#c7c10c";

		public static string PaleCyan => "#b7fffa";

		public static string KeyLime => "#aeff6e";

		public static string TomatoRed => "#ec2d01";

		public static string Lightgreen => "#76ff7b";

		public static string Merlot => "#730039";

		public static string NightBlue => "#040348";

		public static string PurpleishPink => "#df4ec8";

		public static string Apple => "#6ecb3c";

		public static string BabyPoopGreen => "#8f9805";

		public static string GreenApple => "#5edc1f";

		public static string Heliotrope => "#d94ff5";

		public static string Yellow_Green => "#c8fd3d";

		public static string AlmostBlack => "#070d0d";

		public static string CoolBlue => "#4984b8";

		public static string LeafyGreen => "#51b73b";

		public static string MustardBrown => "#ac7e04";

		public static string Dusk => "#4e5481";

		public static string DullBrown => "#876e4b";

		public static string FrogGreen => "#58bc08";

		public static string VividGreen => "#2fef10";

		public static string BrightLightGreen => "#2dfe54";

		public static string FluroGreen => "#0aff02";

		public static string Kiwi => "#9cef43";

		public static string Seaweed => "#18d17b";

		public static string NavyGreen => "#35530a";

		public static string UltramarineBlue => "#1805db";

		public static string Iris => "#6258c4";

		public static string PastelOrange => "#ff964f";

		public static string YellowishOrange => "#ffab0f";

		public static string Perrywinkle => "#8f8ce7";

		public static string Tealish => "#24bca8";

		public static string DarkPlum => "#3f012c";

		public static string Pear => "#cbf85f";

		public static string PinkishOrange => "#ff724c";

		public static string MidnightPurple => "#280137";

		public static string LightUrple => "#b36ff6";

		public static string DarkMint => "#48c072";

		public static string GreenishTan => "#bccb7a";

		public static string LightBurgundy => "#a8415b";

		public static string TurquoiseBlue => "#06b1c4";

		public static string UglyPink => "#cd7584";

		public static string Sandy => "#f1da7a";

		public static string ElectricPink => "#ff0490";

		public static string MutedPurple => "#805b87";

		public static string MidGreen => "#50a747";

		public static string Greyish => "#a8a495";

		public static string NeonYellow => "#cfff04";

		public static string Banana => "#ffff7e";

		public static string CarnationPink => "#ff7fa7";

		public static string Tomato => "#ef4026";

		public static string Sea => "#3c9992";

		public static string MuddyBrown => "#886806";

		public static string TurquoiseGreen => "#04f489";

		public static string Buff => "#fef69e";

		public static string Fawn => "#cfaf7b";

		public static string MutedBlue => "#3b719f";

		public static string PaleRose => "#fdc1c5";

		public static string DarkMintGreen => "#20c073";

		public static string Amethyst => "#9b5fc0";

		public static string Blue_Green => "#0f9b8e";

		public static string Chestnut => "#742802";

		public static string SickGreen => "#9db92c";

		public static string Pea => "#a4bf20";

		public static string RustyOrange => "#cd5909";

		public static string Stone => "#ada587";

		public static string RoseRed => "#be013c";

		public static string PaleAqua => "#b8ffeb";

		public static string DeepOrange => "#dc4d01";

		public static string Earth => "#a2653e";

		public static string MossyGreen => "#638b27";

		public static string GrassyGreen => "#419c03";

		public static string PaleLimeGreen => "#b1ff65";

		public static string LightGreyBlue => "#9dbcd4";

		public static string PaleGrey => "#fdfdfe";

		public static string Asparagus => "#77ab56";

		public static string Blueberry => "#464196";

		public static string PurpleRed => "#990147";

		public static string PaleLime => "#befd73";

		public static string GreenishTeal => "#32bf84";

		public static string Caramel => "#af6f09";

		public static string DeepMagenta => "#a0025c";

		public static string LightPeach => "#ffd8b1";

		public static string MilkChocolate => "#7f4e1e";

		public static string Ocher => "#bf9b0c";

		public static string OffGreen => "#6ba353";

		public static string PurplyPink => "#f075e6";

		public static string Lightblue => "#7bc8f6";

		public static string DuskyBlue => "#475f94";

		public static string Golden => "#f5bf03";

		public static string LightBeige => "#fffeb6";

		public static string ButterYellow => "#fffd74";

		public static string DuskyPurple => "#895b7b";

		public static string FrenchBlue => "#436bad";

		public static string UglyYellow => "#d0c101";

		public static string GreenyYellow => "#c6f808";

		public static string OrangishRed => "#f43605";

		public static string ShamrockGreen => "#02c14d";

		public static string OrangishBrown => "#b25f03";

		public static string TreeGreen => "#2a7e19";

		public static string DeepViolet => "#490648";

		public static string Gunmetal => "#536267";

		public static string Blue_Purple => "#5a06ef";

		public static string Cherry => "#cf0234";

		public static string SandyBrown => "#c4a661";

		public static string WarmGrey => "#978a84";

		public static string DarkIndigo => "#1f0954";

		public static string Midnight => "#03012d";

		public static string BlueyGreen => "#2bb179";

		public static string GreyPink => "#c3909b";

		public static string SoftPurple => "#a66fb5";

		public static string Blood => "#770001";

		public static string BrownRed => "#922b05";

		public static string MediumGrey => "#7d7f7c";

		public static string Berry => "#990f4b";

		public static string Poo => "#8f7303";

		public static string PurpleyPink => "#c83cb9";

		public static string LightSalmon => "#fea993";

		public static string Snot => "#acbb0d";

		public static string EasterPurple => "#c071fe";

		public static string LightYellowGreen => "#ccfd7f";

		public static string DarkNavyBlue => "#00022e";

		public static string Drab => "#828344";

		public static string LightRose => "#ffc5cb";

		public static string Rouge => "#ab1239";

		public static string PurplishRed => "#b0054b";

		public static string SlimeGreen => "#99cc04";

		public static string BabyPoop => "#937c00";

		public static string IrishGreen => "#019529";

		public static string Pink_Purple => "#ef1de7";

		public static string DarkNavy => "#000435";

		public static string GreenyBlue => "#42b395";

		public static string LightPlum => "#9d5783";

		public static string PinkishGrey => "#c8aca9";

		public static string DirtyOrange => "#c87606";

		public static string RustRed => "#aa2704";

		public static string PaleLilac => "#e4cbff";

		public static string OrangeyRed => "#fa4224";

		public static string PrimaryBlue => "#0804f9";

		public static string KermitGreen => "#5cb200";

		public static string BrownishPurple => "#76424e";

		public static string MurkyGreen => "#6c7a0e";

		public static string Wheat => "#fbdd7e";

		public static string VeryDarkPurple => "#2a0134";

		public static string BottleGreen => "#044a05";

		public static string Watermelon => "#fd4659";

		public static string DeepSkyBlue => "#0d75f8";

		public static string FireEngineRed => "#fe0002";

		public static string YellowOchre => "#cb9d06";

		public static string PumpkinOrange => "#fb7d07";

		public static string PaleOlive => "#b9cc81";

		public static string LightLilac => "#edc8ff";

		public static string LightishGreen => "#61e160";

		public static string CarolinaBlue => "#8ab8fe";

		public static string Mulberry => "#920a4e";

		public static string ShockingPink => "#fe02a2";

		public static string Auburn => "#9a3001";

		public static string BrightLimeGreen => "#65fe08";

		public static string Celadon => "#befdb7";

		public static string PinkishBrown => "#b17261";

		public static string PooBrown => "#885f01";

		public static string BrightSkyBlue => "#02ccfe";

		public static string Celery => "#c1fd95";

		public static string DirtBrown => "#836539";

		public static string Strawberry => "#fb2943";

		public static string DarkLime => "#84b701";

		public static string Copper => "#b66325";

		public static string MediumBrown => "#7f5112";

		public static string MutedGreen => "#5fa052";

		public static string RobinSEgg => "#6dedfd";

		public static string BrightAqua => "#0bf9ea";

		public static string BrightLavender => "#c760ff";

		public static string Ivory => "#ffffcb";

		public static string VeryLightPurple => "#f6cefc";

		public static string LightNavy => "#155084";

		public static string PinkRed => "#f5054f";

		public static string OliveBrown => "#645403";

		public static string PoopBrown => "#7a5901";

		public static string MustardGreen => "#a8b504";

		public static string OceanGreen => "#3d9973";

		public static string VeryDarkBlue => "#000133";

		public static string DustyGreen => "#76a973";

		public static string LightNavyBlue => "#2e5a88";

		public static string MintyGreen => "#0bf77d";

		public static string Adobe => "#bd6c48";

		public static string Barney => "#ac1db8";

		public static string JadeGreen => "#2baf6a";

		public static string BrightLightBlue => "#26f7fd";

		public static string LightLime => "#aefd6c";

		public static string DarkKhaki => "#9b8f55";

		public static string OrangeYellow => "#ffad01";

		public static string Ocre => "#c69c04";

		public static string Maize => "#f4d054";

		public static string FadedPink => "#de9dac";

		public static string BritishRacingGreen => "#05480d";

		public static string Sandstone => "#c9ae74";

		public static string MudBrown => "#60460f";

		public static string LightSeaGreen => "#98f6b0";

		public static string RobinEggBlue => "#8af1fe";

		public static string AquaMarine => "#2ee8bb";

		public static string DarkSeaGreen => "#11875d";

		public static string SoftPink => "#fdb0c0";

		public static string OrangeyBrown => "#b16002";

		public static string CherryRed => "#f7022a";

		public static string BurntYellow => "#d5ab09";

		public static string BrownishGrey => "#86775f";

		public static string Camel => "#c69f59";

		public static string PurplishGrey => "#7a687f";

		public static string Marine => "#042e60";

		public static string GreyishPink => "#c88d94";

		public static string PaleTurquoise => "#a5fbd5";

		public static string PastelYellow => "#fffe71";

		public static string BlueyPurple => "#6241c7";

		public static string CanaryYellow => "#fffe40";

		public static string FadedRed => "#d3494e";

		public static string Sepia => "#985e2b";

		public static string Coffee => "#a6814c";

		public static string BrightMagenta => "#ff08e8";

		public static string Mocha => "#9d7651";

		public static string Ecru => "#feffca";

		public static string Purpleish => "#98568d";

		public static string Cranberry => "#9e003a";

		public static string DarkishGreen => "#287c37";

		public static string BrownOrange => "#b96902";

		public static string DuskyRose => "#ba6873";

		public static string Melon => "#ff7855";

		public static string SicklyGreen => "#94b21c";

		public static string Silver => "#c5c9c7";

		public static string PurplyBlue => "#661aee";

		public static string PurpleishBlue => "#6140ef";

		public static string HospitalGreen => "#9be5aa";

		public static string ShitBrown => "#7b5804";

		public static string MidBlue => "#276ab3";

		public static string Amber => "#feb308";

		public static string EasterGreen => "#8cfd7e";

		public static string SoftBlue => "#6488ea";

		public static string CeruleanBlue => "#056eee";

		public static string GoldenBrown => "#b27a01";

		public static string BrightTurquoise => "#0ffef9";

		public static string RedPink => "#fa2a55";

		public static string RedPurple => "#820747";

		public static string GreyishBrown => "#7a6a4f";

		public static string Vermillion => "#f4320c";

		public static string Russet => "#a13905";

		public static string SteelGrey => "#6f828a";

		public static string LighterPurple => "#a55af4";

		public static string BrightViolet => "#ad0afd";

		public static string PrussianBlue => "#004577";

		public static string SlateGreen => "#658d6d";

		public static string DirtyPink => "#ca7b80";

		public static string DarkBlueGreen => "#005249";

		public static string Pine => "#2b5d34";

		public static string YellowyGreen => "#bff128";

		public static string DarkGold => "#b59410";

		public static string Bluish => "#2976bb";

		public static string DarkishBlue => "#014182";

		public static string DullRed => "#bb3f3f";

		public static string PinkyRed => "#fc2647";

		public static string Bronze => "#a87900";

		public static string PaleTeal => "#82cbb2";

		public static string MilitaryGreen => "#667c3e";

		public static string BarbiePink => "#fe46a5";

		public static string BubblegumPink => "#fe83cc";

		public static string PeaSoupGreen => "#94a617";

		public static string DarkMustard => "#a88905";

		public static string Shit => "#7f5f00";

		public static string MediumPurple => "#9e43a2";

		public static string VeryDarkGreen => "#062e03";

		public static string Dirt => "#8a6e45";

		public static string DuskyPink => "#cc7a8b";

		public static string RedViolet => "#9e0168";

		public static string LemonYellow => "#fdff38";

		public static string Pistachio => "#c0fa8b";

		public static string DullYellow => "#eedc5b";

		public static string DarkLimeGreen => "#7ebd01";

		public static string DenimBlue => "#3b5b92";

		public static string TealBlue => "#01889f";

		public static string LightishBlue => "#3d7afd";

		public static string PurpleyBlue => "#5f34e7";

		public static string LightIndigo => "#6d5acf";

		public static string SwampGreen => "#748500";

		public static string BrownGreen => "#706c11";

		public static string DarkMaroon => "#3c0008";

		public static string HotPurple => "#cb00f5";

		public static string DarkForestGreen => "#002d04";

		public static string FadedBlue => "#658cbb";

		public static string DrabGreen => "#749551";

		public static string LightLimeGreen => "#b9ff66";

		public static string SnotGreen => "#9dc100";

		public static string Yellowish => "#faee66";

		public static string LightBlueGreen => "#7efbb3";

		public static string Bordeaux => "#7b002c";

		public static string LightMauve => "#c292a1";

		public static string Ocean => "#017b92";

		public static string Marigold => "#fcc006";

		public static string MuddyGreen => "#657432";

		public static string DullOrange => "#d8863b";

		public static string Steel => "#738595";

		public static string ElectricPurple => "#aa23ff";

		public static string FluorescentGreen => "#08ff08";

		public static string YellowishBrown => "#9b7a01";

		public static string Blush => "#f29e8e";

		public static string SoftGreen => "#6fc276";

		public static string BrightOrange => "#ff5b00";

		public static string Lemon => "#fdff52";

		public static string PurpleGrey => "#866f85";

		public static string AcidGreen => "#8ffe09";

		public static string PaleLavender => "#eecffe";

		public static string VioletBlue => "#510ac9";

		public static string LightForestGreen => "#4f9153";

		public static string BurntRed => "#9f2305";

		public static string KhakiGreen => "#728639";

		public static string Cerise => "#de0c62";

		public static string FadedPurple => "#916e99";

		public static string Apricot => "#ffb16d";

		public static string DarkOliveGreen => "#3c4d03";

		public static string GreyBrown => "#7f7053";

		public static string GreenGrey => "#77926f";

		public static string TrueBlue => "#010fcc";

		public static string PaleViolet => "#ceaefa";

		public static string PeriwinkleBlue => "#8f99fb";

		public static string LightSkyBlue => "#c6fcff";

		public static string Blurple => "#5539cc";

		public static string GreenBrown => "#544e03";

		public static string Bluegreen => "#017a79";

		public static string BrightTeal => "#01f9c6";

		public static string BrownishYellow => "#c9b003";

		public static string PeaSoup => "#929901";

		public static string Forest => "#0b5509";

		public static string BarneyPurple => "#a00498";

		public static string Ultramarine => "#2000b1";

		public static string Purplish => "#94568c";

		public static string PukeYellow => "#c2be0e";

		public static string BluishGrey => "#748b97";

		public static string DarkPeriwinkle => "#665fd1";

		public static string DarkLilac => "#9c6da5";

		public static string Reddish => "#c44240";

		public static string LightMaroon => "#a24857";

		public static string DustyPurple => "#825f87";

		public static string TerraCotta => "#c9643b";

		public static string Avocado => "#90b134";

		public static string MarineBlue => "#01386a";

		public static string TealGreen => "#25a36f";

		public static string SlateGrey => "#59656d";

		public static string LighterGreen => "#75fd63";

		public static string ElectricGreen => "#21fc0d";

		public static string DustyBlue => "#5a86ad";

		public static string GoldenYellow => "#fec615";

		public static string BrightYellow => "#fffd01";

		public static string LightLavender => "#dfc5fe";

		public static string Umber => "#b26400";

		public static string Poop => "#7f5e00";

		public static string DarkPeach => "#de7e5d";

		public static string JungleGreen => "#048243";

		public static string Eggshell => "#ffffd4";

		public static string Denim => "#3b638c";

		public static string YellowBrown => "#b79400";

		public static string DullPurple => "#84597e";

		public static string ChocolateBrown => "#411900";

		public static string WineRed => "#7b0323";

		public static string NeonBlue => "#04d9ff";

		public static string DirtyGreen => "#667e2c";

		public static string LightTan => "#fbeeac";

		public static string IceBlue => "#d7fffe";

		public static string CadetBlue => "#4e7496";

		public static string DarkMauve => "#874c62";

		public static string VeryLightBlue => "#d5ffff";

		public static string GreyPurple => "#826d8c";

		public static string PastelPink => "#ffbacd";

		public static string VeryLightGreen => "#d1ffbd";

		public static string DarkSkyBlue => "#448ee4";

		public static string Evergreen => "#05472a";

		public static string DullPink => "#d5869d";

		public static string Aubergine => "#3d0734";

		public static string Mahogany => "#4a0100";

		public static string ReddishOrange => "#f8481c";

		public static string DeepGreen => "#02590f";

		public static string VomitGreen => "#89a203";

		public static string PurplePink => "#e03fd8";

		public static string DustyPink => "#d58a94";

		public static string FadedGreen => "#7bb274";

		public static string CamoGreen => "#526525";

		public static string PinkyPurple => "#c94cbe";

		public static string PinkPurple => "#db4bda";

		public static string BrownishRed => "#9e3623";

		public static string DarkRose => "#b5485d";

		public static string Mud => "#735c12";

		public static string Brownish => "#9c6d57";

		public static string EmeraldGreen => "#028f1e";

		public static string PaleBrown => "#b1916e";

		public static string DullBlue => "#49759c";

		public static string BurntUmber => "#a0450e";

		public static string MediumGreen => "#39ad48";

		public static string Clay => "#b66a50";

		public static string LightAqua => "#8cffdb";

		public static string LightOliveGreen => "#a4be5c";

		public static string BrownishOrange => "#cb7723";

		public static string DarkAqua => "#05696b";

		public static string PurplishPink => "#ce5dae";

		public static string DarkSalmon => "#c85a53";

		public static string GreenishGrey => "#96ae8d";

		public static string Jade => "#1fa774";

		public static string UglyGreen => "#7a9703";

		public static string DarkBeige => "#ac9362";

		public static string Emerald => "#01a049";

		public static string PaleRed => "#d9544d";

		public static string LightMagenta => "#fa5ff7";

		public static string Sky => "#82cafc";

		public static string LightCyan => "#acfffc";

		public static string YellowOrange => "#fcb001";

		public static string ReddishPurple => "#910951";

		public static string ReddishPink => "#fe2c54";

		public static string Orchid => "#c875c4";

		public static string DirtyYellow => "#cdc50a";

		public static string OrangeRed => "#fd411e";

		public static string DeepRed => "#9a0200";

		public static string OrangeBrown => "#be6400";

		public static string CobaltBlue => "#030aa7";

		public static string NeonPink => "#fe019a";

		public static string RosePink => "#f7879a";

		public static string GreyishPurple => "#887191";

		public static string Raspberry => "#b00149";

		public static string AquaGreen => "#12e193";

		public static string SalmonPink => "#fe7b7c";

		public static string Tangerine => "#ff9408";

		public static string BrownishGreen => "#6a6e09";

		public static string RedBrown => "#8b2e16";

		public static string GreenishBrown => "#696112";

		public static string Pumpkin => "#e17701";

		public static string PineGreen => "#0a481e";

		public static string Charcoal => "#343837";

		public static string BabyPink => "#ffb7ce";

		public static string Cornflower => "#6a79f7";

		public static string BlueViolet => "#5d06e9";

		public static string Chocolate => "#3d1c02";

		public static string GreyishGreen => "#82a67d";

		public static string Scarlet => "#be0119";

		public static string GreenYellow => "#c9ff27";

		public static string DarkOlive => "#373e02";

		public static string Sienna => "#a9561e";

		public static string PastelPurple => "#caa0ff";

		public static string Terracotta => "#ca6641";

		public static string AquaBlue => "#02d8e9";

		public static string SageGreen => "#88b378";

		public static string BloodRed => "#980002";

		public static string DeepPink => "#cb0162";

		public static string Grass => "#5cac2d";

		public static string Moss => "#769958";

		public static string PastelBlue => "#a2bffe";

		public static string BluishGreen => "#10a674";

		public static string GreenBlue => "#06b48b";

		public static string DarkTan => "#af884a";

		public static string GreenishBlue => "#0b8b87";

		public static string PaleOrange => "#ffa756";

		public static string Vomit => "#a2a415";

		public static string ForrestGreen => "#154406";

		public static string DarkLavender => "#856798";

		public static string DarkViolet => "#34013f";

		public static string PurpleBlue => "#632de9";

		public static string DarkCyan => "#0a888a";

		public static string OliveDrab => "#6f7632";

		public static string Pinkish => "#d46a7e";

		public static string Cobalt => "#1e488f";

		public static string NeonPurple => "#bc13fe";

		public static string LightTurquoise => "#7ef4cc";

		public static string AppleGreen => "#76cd26";

		public static string DullGreen => "#74a662";

		public static string Wine => "#80013f";

		public static string PowderBlue => "#b1d1fc";

		public static string OffWhite => "#ffffe4";

		public static string ElectricBlue => "#0652ff";

		public static string DarkTurquoise => "#045c5a";

		public static string BluePurple => "#5729ce";

		public static string Azure => "#069af3";

		public static string BrightRed => "#ff000d";

		public static string PinkishRed => "#f10c45";

		public static string CornflowerBlue => "#5170d7";

		public static string LightOlive => "#acbf69";

		public static string Grape => "#6c3461";

		public static string GreyishBlue => "#5e819d";

		public static string PurplishBlue => "#601ef9";

		public static string YellowishGreen => "#b0dd16";

		public static string GreenishYellow => "#cdfd02";

		public static string MediumBlue => "#2c6fbb";

		public static string DustyRose => "#c0737a";

		public static string LightViolet => "#d6b4fc";

		public static string MidnightBlue => "#020035";

		public static string BluishPurple => "#703be7";

		public static string RedOrange => "#fd3c06";

		public static string DarkMagenta => "#960056";

		public static string Greenish => "#40a368";

		public static string OceanBlue => "#03719c";

		public static string Coral => "#fc5a50";

		public static string Cream => "#ffffc2";

		public static string ReddishBrown => "#7f2b0a";

		public static string BurntSienna => "#b04e0f";

		public static string Brick => "#a03623";

		public static string Sage => "#87ae73";

		public static string GreyGreen => "#789b73";

		public static string White => "#ffffff";

		public static string RobinSEggBlue => "#98eff9";

		public static string MossGreen => "#658b38";

		public static string SteelBlue => "#5a7d9a";

		public static string Eggplant => "#380835";

		public static string LightYellow => "#fffe7a";

		public static string LeafGreen => "#5ca904";

		public static string LightGrey => "#d8dcd6";

		public static string Puke => "#a5a502";

		public static string PinkishPurple => "#d648d7";

		public static string SeaBlue => "#047495";

		public static string PalePurple => "#b790d4";

		public static string SlateBlue => "#5b7c99";

		public static string BlueGrey => "#607c8e";

		public static string HunterGreen => "#0b4008";

		public static string Fuchsia => "#ed0dd9";

		public static string Crimson => "#8c000f";

		public static string PaleYellow => "#ffff84";

		public static string Ochre => "#bf9005";

		public static string MustardYellow => "#d2bd0a";

		public static string LightRed => "#ff474c";

		public static string Cerulean => "#0485d1";

		public static string PalePink => "#ffcfdc";

		public static string DeepBlue => "#040273";

		public static string Rust => "#a83c09";

		public static string LightTeal => "#90e4c1";

		public static string Slate => "#516572";

		public static string Goldenrod => "#fac205";

		public static string DarkYellow => "#d5b60a";

		public static string DarkGrey => "#363737";

		public static string ArmyGreen => "#4b5d16";

		public static string GreyBlue => "#6b8ba4";

		public static string Seafoam => "#80f9ad";

		public static string Puce => "#a57e52";

		public static string SpringGreen => "#a9f971";

		public static string DarkOrange => "#c65102";

		public static string Sand => "#e2ca76";

		public static string PastelGreen => "#b0ff9d";

		public static string Mint => "#9ffeb0";

		public static string LightOrange => "#fdaa48";

		public static string BrightPink => "#fe01b1";

		public static string Chartreuse => "#c1f80a";

		public static string DeepPurple => "#36013f";

		public static string DarkBrown => "#341c02";

		public static string Taupe => "#b9a281";

		public static string PeaGreen => "#8eab12";

		public static string PukeGreen => "#9aae07";

		public static string KellyGreen => "#02ab2e";

		public static string SeafoamGreen => "#7af9ab";

		public static string BlueGreen => "#137e6d";

		public static string Khaki => "#aaa662";

		public static string Burgundy => "#610023";

		public static string DarkTeal => "#014d4e";

		public static string BrickRed => "#8f1402";

		public static string RoyalPurple => "#4b006e";

		public static string Plum => "#580f41";

		public static string MintGreen => "#8fff9f";

		public static string Gold => "#dbb40c";

		public static string BabyBlue => "#a2cffe";

		public static string YellowGreen => "#c0fb2d";

		public static string BrightPurple => "#be03fd";

		public static string DarkRed => "#840000";

		public static string PaleBlue => "#d0fefe";

		public static string GrassGreen => "#3f9b0b";

		public static string Navy => "#01153e";

		public static string Aquamarine => "#04d8b2";

		public static string BurntOrange => "#c04e01";

		public static string NeonGreen => "#0cff0c";

		public static string BrightBlue => "#0165fc";

		public static string Rose => "#cf6275";

		public static string LightPink => "#ffd1df";

		public static string Mustard => "#ceb301";

		public static string Indigo => "#380282";

		public static string Lime => "#aaff32";

		public static string SeaGreen => "#53fca1";

		public static string Periwinkle => "#8e82fe";

		public static string DarkPink => "#cb416b";

		public static string OliveGreen => "#677a04";

		public static string Peach => "#ffb07c";

		public static string PaleGreen => "#c7fdb5";

		public static string LightBrown => "#ad8150";

		public static string HotPink => "#ff028d";

		public static string Black => "#000000";

		public static string Lilac => "#cea2fd";

		public static string NavyBlue => "#001146";

		public static string RoyalBlue => "#0504aa";

		public static string Beige => "#e6daa6";

		public static string Salmon => "#ff796c";

		public static string Olive => "#6e750e";

		public static string Maroon => "#650021";

		public static string BrightGreen => "#01ff07";

		public static string DarkPurple => "#35063e";

		public static string Mauve => "#ae7181";

		public static string ForestGreen => "#06470c";

		public static string Aqua => "#13eac9";

		public static string Cyan => "#00ffff";

		public static string Tan => "#d1b26f";

		public static string DarkBlue => "#00035b";

		public static string Lavender => "#c79fef";

		public static string Turquoise => "#06c2ac";

		public static string DarkGreen => "#033500";

		public static string Violet => "#9a0eea";

		public static string LightPurple => "#bf77f6";

		public static string LimeGreen => "#89fe05";

		public static string Grey => "#929591";

		public static string SkyBlue => "#75bbfd";

		public static string Yellow => "#ffff14";

		public static string Magenta => "#c20078";

		public static string LightGreen => "#96f97b";

		public static string Orange => "#f97306";

		public static string Teal => "#029386";

		public static string LightBlue => "#95d0fc";

		public static string Red => "#e50000";

		public static string Brown => "#653700";

		public static string Pink => "#ff81c0";

		public static string Blue => "#0343df";

		public static string Green => "#15b01a";

		public static string Purple => "#7e1e9c";
	}

	[Obsolete("This has been replaced by the Color.A(float) extension method.")]
	public static float NextColorAlpha = 1f;

	public static Color KSPBadassGreen => new Color(0.7215686f, 0.792156f, 38f / 85f, 1f);

	public static Color KSPNotSoGoodOrange => new Color(1f, 0.694f, 0f, 1f);

	public static Color KSPUIGrey => new Color(0.2122261f, 0.2260669f, 0.2399077f, 1f);

	public static Color KSPUnnamedCyan => new Color(0.3720415f, 63f / 85f, 0.7257529f, 1f);

	public static Color KSPNeutralUIGrey => new Color(27f / 34f, 0.778234f, 0.7182093f, 1f);

	public static Color KSPMellowYellow => new Color(1f, 0.94f, 0f, 1f);

	public static Color KSPYellowishGreen => new Color(0.396f, 0.96f, 0.3215f, 1f);

	public static Color KSPMETitle => new Color(84f / 85f, 0.7960784f, 0.2666667f, 1f);

	public static Color CloudyBlue => new Color(0.6745098f, 0.7607843f, 0.8509804f, 1f);

	public static Color DarkPastelGreen => new Color(0.3372549f, 0.682353f, 0.3411765f, 1f);

	public static Color Dust => new Color(0.6980392f, 0.6f, 0.4313726f, 1f);

	public static Color ElectricLime => new Color(0.6588235f, 1f, 0.01568628f, 1f);

	public static Color FreshGreen => new Color(0.4117647f, 0.8470588f, 0.3098039f, 1f);

	public static Color LightEggplant => new Color(0.5372549f, 0.2705882f, 0.5215687f, 1f);

	public static Color NastyGreen => new Color(0.4392157f, 0.6980392f, 0.2470588f, 1f);

	public static Color ReallyLightBlue => new Color(0.8313726f, 1f, 1f, 1f);

	public static Color Tea => new Color(0.3960784f, 0.6705883f, 0.4862745f, 1f);

	public static Color WarmPurple => new Color(0.5843138f, 0.1803922f, 0.5607843f, 1f);

	public static Color YellowishTan => new Color(84f / 85f, 84f / 85f, 43f / 85f, 1f);

	public static Color Cement => new Color(0.6470588f, 0.6392157f, 29f / 51f, 1f);

	public static Color DarkGrassGreen => new Color(0.2196078f, 0.5019608f, 0.01568628f, 1f);

	public static Color DustyTeal => new Color(0.2980392f, 48f / 85f, 0.5215687f, 1f);

	public static Color GreyTeal => new Color(0.3686275f, 0.6078432f, 46f / 85f, 1f);

	public static Color MacaroniAndCheese => new Color(0.9372549f, 0.7058824f, 0.2078431f, 1f);

	public static Color PinkishTan => new Color(0.8509804f, 0.6078432f, 0.509804f, 1f);

	public static Color Spruce => new Color(0.03921569f, 0.372549f, 0.2196078f, 1f);

	public static Color StrongBlue => new Color(0.04705882f, 0.02352941f, 0.9686275f, 1f);

	public static Color ToxicGreen => new Color(0.3803922f, 0.8705882f, 0.1647059f, 1f);

	public static Color WindowsBlue => new Color(0.2156863f, 0.4705882f, 0.7490196f, 1f);

	public static Color BlueBlue => new Color(0.1333333f, 0.2588235f, 0.7803922f, 1f);

	public static Color BlueWithAHintOfPurple => new Color(0.3254902f, 0.2352941f, 66f / 85f, 1f);

	public static Color Booger => new Color(0.6078432f, 0.7098039f, 0.2352941f, 1f);

	public static Color BrightSeaGreen => new Color(0.01960784f, 1f, 0.6509804f, 1f);

	public static Color DarkGreenBlue => new Color(0.1215686f, 33f / 85f, 0.3411765f, 1f);

	public static Color DeepTurquoise => new Color(0.003921569f, 23f / 51f, 0.454902f, 1f);

	public static Color GreenTeal => new Color(0.04705882f, 0.7098039f, 0.4666667f, 1f);

	public static Color StrongPink => new Color(1f, 0.02745098f, 0.5372549f, 1f);

	public static Color Bland => new Color(35f / 51f, 0.6588235f, 0.5450981f, 1f);

	public static Color DeepAqua => new Color(0.03137255f, 0.4705882f, 0.4980392f, 1f);

	public static Color LavenderPink => new Color(13f / 15f, 0.5215687f, 0.8431373f, 1f);

	public static Color LightMossGreen => new Color(0.6509804f, 0.7843137f, 0.4588235f, 1f);

	public static Color LightSeafoamGreen => new Color(0.654902f, 1f, 0.7098039f, 1f);

	public static Color OliveYellow => new Color(0.7607843f, 61f / 85f, 3f / 85f, 1f);

	public static Color PigPink => new Color(0.9058824f, 0.5568628f, 0.6470588f, 1f);

	public static Color DeepLilac => new Color(0.5882353f, 0.4313726f, 63f / 85f, 1f);

	public static Color Desert => new Color(0.8f, 0.6784314f, 32f / 85f, 1f);

	public static Color DustyLavender => new Color(0.6745098f, 0.5254902f, 0.6588235f, 1f);

	public static Color PurpleyGrey => new Color(0.5803922f, 0.4941176f, 0.5803922f, 1f);

	public static Color Purply => new Color(0.5960785f, 0.2470588f, 0.6980392f, 1f);

	public static Color CandyPink => new Color(1f, 33f / 85f, 0.9137255f, 1f);

	public static Color LightPastelGreen => new Color(0.6980392f, 0.9843137f, 0.6470588f, 1f);

	public static Color BoringGreen => new Color(33f / 85f, 0.7019608f, 0.3960784f, 1f);

	public static Color KiwiGreen => new Color(0.5568628f, 0.8980392f, 0.2470588f, 1f);

	public static Color LightGreyGreen => new Color(61f / 85f, 0.8823529f, 0.6313726f, 1f);

	public static Color OrangePink => new Color(1f, 0.4352941f, 0.3215686f, 1f);

	public static Color TeaGreen => new Color(63f / 85f, 0.972549f, 0.6392157f, 1f);

	public static Color VeryLightBrown => new Color(0.827451f, 0.7137255f, 0.5137255f, 1f);

	public static Color EggShell => new Color(1f, 84f / 85f, 0.7686275f, 1f);

	public static Color EggplantPurple => new Color(0.2627451f, 0.01960784f, 0.254902f, 1f);

	public static Color PowderPink => new Color(1f, 0.6980392f, 0.8156863f, 1f);

	public static Color ReddishGrey => new Color(0.6f, 0.4588235f, 0.4392157f, 1f);

	public static Color BabyShitBrown => new Color(0.6784314f, 48f / 85f, 0.05098039f, 1f);

	public static Color Liliac => new Color(0.7686275f, 0.5568628f, 0.9921569f, 1f);

	public static Color StormyBlue => new Color(16f / 51f, 0.4823529f, 52f / 85f, 1f);

	public static Color UglyBrown => new Color(0.4901961f, 0.4431373f, 0.01176471f, 1f);

	public static Color Custard => new Color(1f, 0.9921569f, 0.4705882f, 1f);

	public static Color DarkishPink => new Color(0.854902f, 0.2745098f, 0.4901961f, 1f);

	public static Color DeepBrown => new Color(0.254902f, 0.007843138f, 0f, 1f);

	public static Color GreenishBeige => new Color(67f / 85f, 0.8196079f, 0.4745098f, 1f);

	public static Color Manilla => new Color(1f, 0.9803922f, 0.5254902f, 1f);

	public static Color OffBlue => new Color(0.3372549f, 44f / 85f, 0.682353f, 1f);

	public static Color BattleshipGrey => new Color(0.4196078f, 0.4862745f, 0.5215687f, 1f);

	public static Color BrownyGreen => new Color(0.4352941f, 0.4235294f, 0.03921569f, 1f);

	public static Color Bruise => new Color(0.4941176f, 0.2509804f, 0.4431373f, 1f);

	public static Color KelleyGreen => new Color(0f, 49f / 85f, 0.2156863f, 1f);

	public static Color SicklyYellow => new Color(0.8156863f, 0.8941177f, 0.1607843f, 1f);

	public static Color SunnyYellow => new Color(1f, 83f / 85f, 0.09019608f, 1f);

	public static Color Azul => new Color(0.1137255f, 31f / 85f, 0.9254902f, 1f);

	public static Color Darkgreen => new Color(0.01960784f, 0.2862745f, 0.02745098f, 1f);

	public static Color Green_Yellow => new Color(0.7098039f, 0.8078431f, 0.03137255f, 1f);

	public static Color Lichen => new Color(0.5607843f, 0.7137255f, 0.4823529f, 1f);

	public static Color LightLightGreen => new Color(0.7843137f, 1f, 0.6901961f, 1f);

	public static Color PaleGold => new Color(0.9921569f, 0.8705882f, 0.4235294f, 1f);

	public static Color SunYellow => new Color(1f, 0.8745098f, 0.1333333f, 1f);

	public static Color TanGreen => new Color(0.6627451f, 0.7450981f, 0.4392157f, 1f);

	public static Color Burple => new Color(0.4078431f, 0.1960784f, 0.8901961f, 1f);

	public static Color Butterscotch => new Color(0.9921569f, 0.6941177f, 0.2784314f, 1f);

	public static Color Toupe => new Color(0.7803922f, 0.6745098f, 0.4901961f, 1f);

	public static Color DarkCream => new Color(1f, 81f / 85f, 0.6039216f, 1f);

	public static Color IndianRed => new Color(0.5215687f, 0.05490196f, 0.01568628f, 1f);

	public static Color LightLavendar => new Color(0.9372549f, 64f / 85f, 0.9960784f, 1f);

	public static Color PoisonGreen => new Color(0.2509804f, 0.9921569f, 0.07843138f, 1f);

	public static Color BabyPukeGreen => new Color(0.7137255f, 0.7686275f, 0.02352941f, 1f);

	public static Color BrightYellowGreen => new Color(0.6156863f, 1f, 0f, 1f);

	public static Color CharcoalGrey => new Color(0.2352941f, 0.254902f, 0.2588235f, 1f);

	public static Color Squash => new Color(0.9490196f, 0.6705883f, 7f / 85f, 1f);

	public static Color Cinnamon => new Color(0.6745098f, 0.3098039f, 0.02352941f, 1f);

	public static Color LightPeaGreen => new Color(0.7686275f, 0.9960784f, 0.509804f, 1f);

	public static Color RadioactiveGreen => new Color(0.172549f, 0.9803922f, 0.1215686f, 1f);

	public static Color RawSienna => new Color(0.6039216f, 0.3843137f, 0f, 1f);

	public static Color BabyPurple => new Color(0.7921569f, 0.6078432f, 0.9686275f, 1f);

	public static Color Cocoa => new Color(0.5294118f, 0.372549f, 0.2588235f, 1f);

	public static Color LightRoyalBlue => new Color(0.227451f, 0.1803922f, 0.9960784f, 1f);

	public static Color Orangeish => new Color(0.9921569f, 47f / 85f, 0.2862745f, 1f);

	public static Color RustBrown => new Color(0.5450981f, 0.1921569f, 0.01176471f, 1f);

	public static Color SandBrown => new Color(0.7960784f, 0.6470588f, 32f / 85f, 1f);

	public static Color Swamp => new Color(0.4117647f, 0.5137255f, 0.2235294f, 1f);

	public static Color TealishGreen => new Color(0.04705882f, 44f / 51f, 23f / 51f, 1f);

	public static Color BurntSiena => new Color(61f / 85f, 0.3215686f, 0.01176471f, 1f);

	public static Color Camo => new Color(0.4980392f, 0.5607843f, 0.3058824f, 1f);

	public static Color DuskBlue => new Color(0.1490196f, 0.3254902f, 47f / 85f, 1f);

	public static Color Fern => new Color(33f / 85f, 0.6627451f, 16f / 51f, 1f);

	public static Color OldRose => new Color(0.7843137f, 0.4980392f, 0.5372549f, 1f);

	public static Color PaleLightGreen => new Color(0.6941177f, 84f / 85f, 0.6f, 1f);

	public static Color PeachyPink => new Color(1f, 0.6039216f, 46f / 85f, 1f);

	public static Color RosyPink => new Color(82f / 85f, 0.4078431f, 0.5568628f, 1f);

	public static Color LightBluishGreen => new Color(0.4627451f, 0.9921569f, 0.6588235f, 1f);

	public static Color LightBrightGreen => new Color(0.3254902f, 0.9960784f, 0.3607843f, 1f);

	public static Color LightNeonGreen => new Color(0.3058824f, 0.9921569f, 0.3294118f, 1f);

	public static Color LightSeafoam => new Color(32f / 51f, 0.9960784f, 0.7490196f, 1f);

	public static Color TiffanyBlue => new Color(0.4823529f, 0.9490196f, 0.854902f, 1f);

	public static Color WashedOutGreen => new Color(0.7372549f, 49f / 51f, 0.6509804f, 1f);

	public static Color BrownyOrange => new Color(0.7921569f, 0.4196078f, 0.007843138f, 1f);

	public static Color NiceBlue => new Color(0.0627451f, 0.4784314f, 0.6901961f, 1f);

	public static Color Sapphire => new Color(0.1294118f, 0.2196078f, 0.6705883f, 1f);

	public static Color GreyishTeal => new Color(0.4431373f, 0.6235294f, 29f / 51f, 1f);

	public static Color OrangeyYellow => new Color(0.9921569f, 37f / 51f, 7f / 85f, 1f);

	public static Color Parchment => new Color(0.9960784f, 84f / 85f, 35f / 51f, 1f);

	public static Color Straw => new Color(84f / 85f, 82f / 85f, 0.4745098f, 1f);

	public static Color VeryDarkBrown => new Color(0.1137255f, 0.007843138f, 0f, 1f);

	public static Color Terracota => new Color(0.7960784f, 0.4078431f, 0.2627451f, 1f);

	public static Color UglyBlue => new Color(0.1921569f, 0.4f, 46f / 85f, 1f);

	public static Color ClearBlue => new Color(0.1411765f, 0.4784314f, 0.9921569f, 1f);

	public static Color Creme => new Color(1f, 1f, 0.7137255f, 1f);

	public static Color FoamGreen => new Color(48f / 85f, 0.9921569f, 0.6627451f, 1f);

	public static Color Grey_Green => new Color(0.5254902f, 0.6313726f, 0.4901961f, 1f);

	public static Color LightGold => new Color(0.9921569f, 44f / 51f, 0.3607843f, 1f);

	public static Color SeafoamBlue => new Color(0.4705882f, 0.8196079f, 0.7137255f, 1f);

	public static Color Topaz => new Color(0.07450981f, 0.7333333f, 35f / 51f, 1f);

	public static Color VioletPink => new Color(0.9843137f, 0.372549f, 84f / 85f, 1f);

	public static Color Wintergreen => new Color(0.1254902f, 83f / 85f, 0.5254902f, 1f);

	public static Color YellowTan => new Color(1f, 0.8901961f, 0.4313726f, 1f);

	public static Color DarkFuchsia => new Color(0.6156863f, 0.02745098f, 0.3490196f, 1f);

	public static Color IndigoBlue => new Color(0.227451f, 8f / 85f, 0.6941177f, 1f);

	public static Color LightYellowishGreen => new Color(0.7607843f, 1f, 0.5372549f, 1f);

	public static Color PaleMagenta => new Color(0.8431373f, 0.4039216f, 0.6784314f, 1f);

	public static Color RichPurple => new Color(0.4470588f, 0f, 0.345098f, 1f);

	public static Color SunflowerYellow => new Color(1f, 0.854902f, 0.01176471f, 1f);

	public static Color Green_Blue => new Color(0.003921569f, 64f / 85f, 47f / 85f, 1f);

	public static Color Leather => new Color(0.6745098f, 0.454902f, 0.2039216f, 1f);

	public static Color RacingGreen => new Color(0.003921569f, 0.2745098f, 0f, 1f);

	public static Color VividPurple => new Color(0.6f, 0f, 0.9803922f, 1f);

	public static Color DarkRoyalBlue => new Color(0.007843138f, 0.02352941f, 0.4352941f, 1f);

	public static Color Hazel => new Color(0.5568628f, 0.4627451f, 8f / 85f, 1f);

	public static Color MutedPink => new Color(0.8196079f, 0.4627451f, 0.5607843f, 1f);

	public static Color BoogerGreen => new Color(0.5882353f, 0.7058824f, 0.01176471f, 1f);

	public static Color Canary => new Color(0.9921569f, 1f, 33f / 85f, 1f);

	public static Color CoolGrey => new Color(0.5843138f, 0.6392157f, 0.6509804f, 1f);

	public static Color DarkTaupe => new Color(0.4980392f, 0.4078431f, 0.3058824f, 1f);

	public static Color DarkishPurple => new Color(0.4588235f, 5f / 51f, 23f / 51f, 1f);

	public static Color TrueGreen => new Color(0.03137255f, 0.5803922f, 0.01568628f, 1f);

	public static Color CoralPink => new Color(1f, 0.3803922f, 33f / 85f, 1f);

	public static Color DarkSage => new Color(0.3490196f, 0.5215687f, 0.3372549f, 1f);

	public static Color DarkSlateBlue => new Color(0.1294118f, 0.2784314f, 0.3803922f, 1f);

	public static Color FlatBlue => new Color(0.2352941f, 23f / 51f, 0.6588235f, 1f);

	public static Color Mushroom => new Color(62f / 85f, 0.6196079f, 0.5333334f, 1f);

	public static Color RichBlue => new Color(0.007843138f, 0.1058824f, 83f / 85f, 1f);

	public static Color DirtyPurple => new Color(23f / 51f, 0.2901961f, 0.3960784f, 1f);

	public static Color Greenblue => new Color(0.1372549f, 0.7686275f, 0.5450981f, 1f);

	public static Color IckyGreen => new Color(0.5607843f, 0.682353f, 0.1333333f, 1f);

	public static Color LightKhaki => new Color(46f / 51f, 0.9490196f, 0.6352941f, 1f);

	public static Color WarmBlue => new Color(0.2941177f, 0.3411765f, 0.8588235f, 1f);

	public static Color DarkHotPink => new Color(0.8509804f, 0.003921569f, 0.4f, 1f);

	public static Color DeepSeaBlue => new Color(0.003921569f, 0.3294118f, 0.509804f, 1f);

	public static Color Carmine => new Color(0.6156863f, 0.007843138f, 0.08627451f, 1f);

	public static Color DarkYellowGreen => new Color(0.4470588f, 0.5607843f, 0.007843138f, 1f);

	public static Color PalePeach => new Color(1f, 0.8980392f, 0.6784314f, 1f);

	public static Color PlumPurple => new Color(0.3058824f, 0.01960784f, 16f / 51f, 1f);

	public static Color GoldenRod => new Color(83f / 85f, 0.7372549f, 0.03137255f, 1f);

	public static Color NeonRed => new Color(1f, 0.02745098f, 0.227451f, 1f);

	public static Color OldPink => new Color(0.7803922f, 0.4745098f, 0.5254902f, 1f);

	public static Color VeryPaleBlue => new Color(0.8392157f, 1f, 0.9960784f, 1f);

	public static Color BloodOrange => new Color(0.9960784f, 0.2941177f, 0.01176471f, 1f);

	public static Color Grapefruit => new Color(0.9921569f, 0.3490196f, 0.3372549f, 1f);

	public static Color SandYellow => new Color(84f / 85f, 0.8823529f, 0.4f, 1f);

	public static Color ClayBrown => new Color(0.6980392f, 0.4431373f, 0.2392157f, 1f);

	public static Color DarkBlueGrey => new Color(0.1215686f, 0.2313726f, 0.3019608f, 1f);

	public static Color FlatGreen => new Color(0.4117647f, 0.6156863f, 0.2980392f, 1f);

	public static Color LightGreenBlue => new Color(0.3372549f, 84f / 85f, 0.6352941f, 1f);

	public static Color WarmPink => new Color(0.9843137f, 0.3333333f, 43f / 85f, 1f);

	public static Color DodgerBlue => new Color(0.2431373f, 0.509804f, 84f / 85f, 1f);

	public static Color GrossGreen => new Color(32f / 51f, 0.7490196f, 0.08627451f, 1f);

	public static Color Ice => new Color(0.8392157f, 1f, 0.9803922f, 1f);

	public static Color MetallicBlue => new Color(0.3098039f, 23f / 51f, 0.5568628f, 1f);

	public static Color PaleSalmon => new Color(1f, 0.6941177f, 0.6039216f, 1f);

	public static Color SapGreen => new Color(0.3607843f, 0.5450981f, 7f / 85f, 1f);

	public static Color Algae => new Color(0.3294118f, 0.6745098f, 0.4078431f, 1f);

	public static Color BlueyGrey => new Color(0.5372549f, 32f / 51f, 0.6901961f, 1f);

	public static Color GreenyGrey => new Color(0.4941176f, 32f / 51f, 0.4784314f, 1f);

	public static Color HighlighterGreen => new Color(0.1058824f, 84f / 85f, 0.02352941f, 1f);

	public static Color LightLightBlue => new Color(0.7921569f, 1f, 0.9843137f, 1f);

	public static Color LightMint => new Color(0.7137255f, 1f, 0.7333333f, 1f);

	public static Color RawUmber => new Color(0.654902f, 0.3686275f, 3f / 85f, 1f);

	public static Color VividBlue => new Color(7f / 85f, 0.1803922f, 1f, 1f);

	public static Color DeepLavender => new Color(47f / 85f, 0.3686275f, 61f / 85f, 1f);

	public static Color DullTeal => new Color(0.372549f, 0.6196079f, 0.5607843f, 1f);

	public static Color LightGreenishBlue => new Color(33f / 85f, 0.9686275f, 0.7058824f, 1f);

	public static Color MudGreen => new Color(32f / 85f, 0.4f, 0.007843138f, 1f);

	public static Color Pinky => new Color(84f / 85f, 0.5254902f, 2f / 3f, 1f);

	public static Color RedWine => new Color(0.5490196f, 0f, 0.2039216f, 1f);

	public static Color ShitGreen => new Color(0.4588235f, 0.5019608f, 0f, 1f);

	public static Color TanBrown => new Color(0.6705883f, 0.4941176f, 0.2980392f, 1f);

	public static Color Darkblue => new Color(0.01176471f, 0.02745098f, 0.3921569f, 1f);

	public static Color Rosa => new Color(0.9960784f, 0.5254902f, 0.6431373f, 1f);

	public static Color Lipstick => new Color(71f / 85f, 0.09019608f, 0.3058824f, 1f);

	public static Color PaleMauve => new Color(0.9960784f, 0.8156863f, 84f / 85f, 1f);

	public static Color Claret => new Color(0.4078431f, 0f, 8f / 85f, 1f);

	public static Color Dandelion => new Color(0.9960784f, 0.8745098f, 0.03137255f, 1f);

	public static Color Orangered => new Color(0.9960784f, 0.2588235f, 1f / 17f, 1f);

	public static Color PoopGreen => new Color(0.4352941f, 0.4862745f, 0f, 1f);

	public static Color Ruby => new Color(0.7921569f, 0.003921569f, 0.2784314f, 1f);

	public static Color Dark => new Color(0.1058824f, 0.1411765f, 0.1921569f, 1f);

	public static Color GreenishTurquoise => new Color(0f, 0.9843137f, 0.6901961f, 1f);

	public static Color PastelRed => new Color(0.8588235f, 0.345098f, 0.3372549f, 1f);

	public static Color PissYellow => new Color(13f / 15f, 0.8392157f, 8f / 85f, 1f);

	public static Color BrightCyan => new Color(0.254902f, 0.9921569f, 0.9960784f, 1f);

	public static Color DarkCoral => new Color(69f / 85f, 0.3215686f, 0.3058824f, 1f);

	public static Color AlgaeGreen => new Color(0.1294118f, 0.7647059f, 0.4352941f, 1f);

	public static Color DarkishRed => new Color(0.6627451f, 0.01176471f, 0.03137255f, 1f);

	public static Color ReddyBrown => new Color(0.4313726f, 0.0627451f, 0.01960784f, 1f);

	public static Color BlushPink => new Color(0.9960784f, 0.509804f, 0.5490196f, 1f);

	public static Color CamouflageGreen => new Color(0.2941177f, 0.3803922f, 0.07450981f, 1f);

	public static Color LawnGreen => new Color(0.3019608f, 0.6431373f, 3f / 85f, 1f);

	public static Color Putty => new Color(0.7450981f, 0.682353f, 46f / 85f, 1f);

	public static Color VibrantBlue => new Color(0.01176471f, 0.2235294f, 0.972549f, 1f);

	public static Color DarkSand => new Color(0.6588235f, 0.5607843f, 0.3490196f, 1f);

	public static Color Purple_Blue => new Color(31f / 85f, 0.1294118f, 0.8156863f, 1f);

	public static Color Saffron => new Color(0.9960784f, 0.6980392f, 3f / 85f, 1f);

	public static Color Twilight => new Color(0.3058824f, 0.3176471f, 0.5450981f, 1f);

	public static Color WarmBrown => new Color(0.5882353f, 0.3058824f, 0.007843138f, 1f);

	public static Color Bluegrey => new Color(0.5215687f, 0.6392157f, 0.6980392f, 1f);

	public static Color BubbleGumPink => new Color(1f, 0.4117647f, 35f / 51f, 1f);

	public static Color DuckEggBlue => new Color(0.7647059f, 0.9843137f, 0.9568627f, 1f);

	public static Color GreenishCyan => new Color(0.1647059f, 0.9960784f, 61f / 85f, 1f);

	public static Color Petrol => new Color(0f, 0.372549f, 0.4156863f, 1f);

	public static Color Royal => new Color(0.04705882f, 0.09019608f, 49f / 85f, 1f);

	public static Color Butter => new Color(1f, 1f, 43f / 85f, 1f);

	public static Color DustyOrange => new Color(0.9411765f, 0.5137255f, 0.227451f, 1f);

	public static Color OffYellow => new Color((float)Math.E * 105f / 302f, 81f / 85f, 0.2470588f, 1f);

	public static Color PaleOliveGreen => new Color(0.6941177f, 0.8235294f, 0.4823529f, 1f);

	public static Color Orangish => new Color(84f / 85f, 0.509804f, 0.2901961f, 1f);

	public static Color Leaf => new Color(0.4431373f, 2f / 3f, 0.2039216f, 1f);

	public static Color LightBlueGrey => new Color(61f / 85f, 67f / 85f, 0.8862745f, 1f);

	public static Color DriedBlood => new Color(0.2941177f, 0.003921569f, 0.003921569f, 1f);

	public static Color LightishPurple => new Color(0.6470588f, 0.3215686f, 46f / 51f, 1f);

	public static Color RustyRed => new Color(35f / 51f, 0.1843137f, 0.05098039f, 1f);

	public static Color LavenderBlue => new Color(0.5450981f, 0.5333334f, 0.972549f, 1f);

	public static Color LightGrassGreen => new Color(0.6039216f, 0.9686275f, 0.3921569f, 1f);

	public static Color LightMintGreen => new Color(0.6509804f, 0.9843137f, 0.6980392f, 1f);

	public static Color Sunflower => new Color(1f, 0.772549f, 6f / 85f, 1f);

	public static Color Velvet => new Color(0.4588235f, 0.03137255f, 0.3176471f, 1f);

	public static Color BrickOrange => new Color(0.7568628f, 0.2901961f, 3f / 85f, 1f);

	public static Color LightishRed => new Color(0.9960784f, 0.1843137f, 0.2901961f, 1f);

	public static Color PureBlue => new Color(0.007843138f, 0.01176471f, 0.8862745f, 1f);

	public static Color TwilightBlue => new Color(0.03921569f, 0.2627451f, 0.4784314f, 1f);

	public static Color VioletRed => new Color(0.6470588f, 0f, 0.3333333f, 1f);

	public static Color YellowyBrown => new Color(0.682353f, 0.5450981f, 0.04705882f, 1f);

	public static Color Carnation => new Color(0.9921569f, 0.4745098f, 0.5607843f, 1f);

	public static Color MuddyYellow => new Color(0.7490196f, 0.6745098f, 0.01960784f, 1f);

	public static Color DarkSeafoamGreen => new Color(0.2431373f, 35f / 51f, 0.4627451f, 1f);

	public static Color DeepRose => new Color(0.7803922f, 0.2784314f, 0.4039216f, 1f);

	public static Color DustyRed => new Color(37f / 51f, 0.282353f, 0.3058824f, 1f);

	public static Color Grey_Blue => new Color(0.3921569f, 0.4901961f, 0.5568628f, 1f);

	public static Color LemonLime => new Color(0.7490196f, 0.9960784f, 0.1568628f, 1f);

	public static Color Purple_Pink => new Color(0.8431373f, 0.145098f, 0.8705882f, 1f);

	public static Color BrownYellow => new Color(0.6980392f, 0.5921569f, 0.01960784f, 1f);

	public static Color PurpleBrown => new Color(0.4039216f, 0.227451f, 0.2470588f, 1f);

	public static Color Wisteria => new Color(0.6588235f, 0.4901961f, 0.7607843f, 1f);

	public static Color BananaYellow => new Color(0.9803922f, 0.9960784f, 0.2941177f, 1f);

	public static Color LipstickRed => new Color(64f / 85f, 0.007843138f, 0.1843137f, 1f);

	public static Color WaterBlue => new Color(0.05490196f, 0.5294118f, 0.8f, 1f);

	public static Color BrownGrey => new Color(47f / 85f, 44f / 85f, 0.4078431f, 1f);

	public static Color VibrantPurple => new Color(0.6784314f, 0.01176471f, 0.8705882f, 1f);

	public static Color BabyGreen => new Color(0.5490196f, 1f, 0.6196079f, 1f);

	public static Color BarfGreen => new Color(0.5803922f, 0.6745098f, 0.007843138f, 1f);

	public static Color EggshellBlue => new Color(0.7686275f, 1f, 0.9686275f, 1f);

	public static Color SandyYellow => new Color(0.9921569f, 0.9333333f, 23f / 51f, 1f);

	public static Color CoolGreen => new Color(0.2f, 0.7215686f, 0.3921569f, 1f);

	public static Color Pale => new Color(1f, 83f / 85f, 0.8156863f, 1f);

	public static Color Blue_Grey => new Color(0.4588235f, 47f / 85f, 0.6392157f, 1f);

	public static Color HotMagenta => new Color(49f / 51f, 0.01568628f, 67f / 85f, 1f);

	public static Color Greyblue => new Color(0.4666667f, 0.6313726f, 0.7098039f, 1f);

	public static Color Purpley => new Color(0.5294118f, 0.3372549f, 0.8941177f, 1f);

	public static Color BabyShitGreen => new Color(0.5333334f, 0.5921569f, 0.09019608f, 1f);

	public static Color BrownishPink => new Color(0.7607843f, 0.4941176f, 0.4745098f, 1f);

	public static Color DarkAquamarine => new Color(0.003921569f, 23f / 51f, 0.4431373f, 1f);

	public static Color Diarrhea => new Color(0.6235294f, 0.5137255f, 0.01176471f, 1f);

	public static Color LightMustard => new Color(0.9686275f, 71f / 85f, 32f / 85f, 1f);

	public static Color PaleSkyBlue => new Color(63f / 85f, 82f / 85f, 0.9960784f, 1f);

	public static Color TurtleGreen => new Color(0.4588235f, 0.7215686f, 0.3098039f, 1f);

	public static Color BrightOlive => new Color(52f / 85f, 0.7333333f, 0.01568628f, 1f);

	public static Color DarkGreyBlue => new Color(0.1607843f, 0.2745098f, 0.3568628f, 1f);

	public static Color GreenyBrown => new Color(0.4117647f, 32f / 85f, 0.02352941f, 1f);

	public static Color LemonGreen => new Color(0.6784314f, 0.972549f, 0.007843138f, 1f);

	public static Color LightPeriwinkle => new Color(0.7568628f, 66f / 85f, 84f / 85f, 1f);

	public static Color SeaweedGreen => new Color(0.2078431f, 0.6784314f, 0.4196078f, 1f);

	public static Color SunshineYellow => new Color(1f, 0.9921569f, 0.2156863f, 1f);

	public static Color UglyPurple => new Color(0.6431373f, 0.2588235f, 32f / 51f, 1f);

	public static Color MediumPink => new Color(81f / 85f, 0.3803922f, 0.5882353f, 1f);

	public static Color PukeBrown => new Color(0.5803922f, 0.4666667f, 0.02352941f, 1f);

	public static Color VeryLightPink => new Color(1f, 0.9568627f, 0.9490196f, 1f);

	public static Color Viridian => new Color(0.1176471f, 29f / 51f, 0.4039216f, 1f);

	public static Color Bile => new Color(0.7098039f, 0.7647059f, 0.02352941f, 1f);

	public static Color FadedYellow => new Color(0.9960784f, 1f, 0.4980392f, 1f);

	public static Color VeryPaleGreen => new Color(69f / 85f, 0.9921569f, 0.7372549f, 1f);

	public static Color VibrantGreen => new Color(0.03921569f, 13f / 15f, 0.03137255f, 1f);

	public static Color BrightLime => new Color(0.5294118f, 0.9921569f, 0.01960784f, 1f);

	public static Color Spearmint => new Color(0.1176471f, 0.972549f, 0.4627451f, 1f);

	public static Color LightAquamarine => new Color(0.4823529f, 0.9921569f, 0.7803922f, 1f);

	public static Color LightSage => new Color(0.7372549f, 0.9254902f, 0.6745098f, 1f);

	public static Color Yellowgreen => new Color(0.7333333f, 83f / 85f, 1f / 17f, 1f);

	public static Color BabyPoo => new Color(0.6705883f, 48f / 85f, 0.01568628f, 1f);

	public static Color DarkSeafoam => new Color(0.1215686f, 0.7098039f, 0.4784314f, 1f);

	public static Color DeepTeal => new Color(0f, 0.3333333f, 0.3529412f, 1f);

	public static Color Heather => new Color(0.6431373f, 44f / 85f, 0.6745098f, 1f);

	public static Color RustOrange => new Color(0.7686275f, 0.3333333f, 0.03137255f, 1f);

	public static Color DirtyBlue => new Color(0.2470588f, 0.509804f, 0.6156863f, 1f);

	public static Color FernGreen => new Color(0.3294118f, 47f / 85f, 0.2666667f, 1f);

	public static Color BrightLilac => new Color(67f / 85f, 0.3686275f, 0.9843137f, 1f);

	public static Color WeirdGreen => new Color(0.227451f, 0.8980392f, 0.4980392f, 1f);

	public static Color PeacockBlue => new Color(0.003921569f, 0.4039216f, 0.5843138f, 1f);

	public static Color AvocadoGreen => new Color(0.5294118f, 0.6627451f, 0.1333333f, 1f);

	public static Color FadedOrange => new Color(0.9411765f, 0.5803922f, 0.3019608f, 1f);

	public static Color GrapePurple => new Color(31f / 85f, 0.07843138f, 0.3176471f, 1f);

	public static Color HotGreen => new Color(0.145098f, 1f, 0.1607843f, 1f);

	public static Color LimeYellow => new Color(0.8156863f, 0.9960784f, 0.1137255f, 1f);

	public static Color Mango => new Color(1f, 0.6509804f, 0.1686275f, 1f);

	public static Color Shamrock => new Color(0.003921569f, 0.7058824f, 0.2980392f, 1f);

	public static Color Bubblegum => new Color(1f, 0.4235294f, 0.7098039f, 1f);

	public static Color PurplishBrown => new Color(0.4196078f, 0.2588235f, 0.2784314f, 1f);

	public static Color VomitYellow => new Color(0.7803922f, 0.7568628f, 0.04705882f, 1f);

	public static Color PaleCyan => new Color(61f / 85f, 1f, 0.9803922f, 1f);

	public static Color KeyLime => new Color(0.682353f, 1f, 0.4313726f, 1f);

	public static Color TomatoRed => new Color(0.9254902f, 0.1764706f, 0.003921569f, 1f);

	public static Color Lightgreen => new Color(0.4627451f, 1f, 0.4823529f, 1f);

	public static Color Merlot => new Color(23f / 51f, 0f, 0.2235294f, 1f);

	public static Color NightBlue => new Color(0.01568628f, 0.01176471f, 0.282353f, 1f);

	public static Color PurpleishPink => new Color(0.8745098f, 0.3058824f, 0.7843137f, 1f);

	public static Color Apple => new Color(0.4313726f, 0.7960784f, 0.2352941f, 1f);

	public static Color BabyPoopGreen => new Color(0.5607843f, 0.5960785f, 0.01960784f, 1f);

	public static Color GreenApple => new Color(0.3686275f, 44f / 51f, 0.1215686f, 1f);

	public static Color Heliotrope => new Color(0.8509804f, 0.3098039f, 49f / 51f, 1f);

	public static Color Yellow_Green => new Color(0.7843137f, 0.9921569f, 0.2392157f, 1f);

	public static Color AlmostBlack => new Color(0.02745098f, 0.05098039f, 0.05098039f, 1f);

	public static Color CoolBlue => new Color(0.2862745f, 44f / 85f, 0.7215686f, 1f);

	public static Color LeafyGreen => new Color(0.3176471f, 61f / 85f, 0.2313726f, 1f);

	public static Color MustardBrown => new Color(0.6745098f, 0.4941176f, 0.01568628f, 1f);

	public static Color Dusk => new Color(0.3058824f, 0.3294118f, 43f / 85f, 1f);

	public static Color DullBrown => new Color(0.5294118f, 0.4313726f, 0.2941177f, 1f);

	public static Color FrogGreen => new Color(0.345098f, 0.7372549f, 0.03137255f, 1f);

	public static Color VividGreen => new Color(0.1843137f, 0.9372549f, 0.0627451f, 1f);

	public static Color BrightLightGreen => new Color(0.1764706f, 0.9960784f, 0.3294118f, 1f);

	public static Color FluroGreen => new Color(0.03921569f, 1f, 0.007843138f, 1f);

	public static Color Kiwi => new Color(52f / 85f, 0.9372549f, 0.2627451f, 1f);

	public static Color Seaweed => new Color(8f / 85f, 0.8196079f, 0.4823529f, 1f);

	public static Color NavyGreen => new Color(0.2078431f, 0.3254902f, 0.03921569f, 1f);

	public static Color UltramarineBlue => new Color(8f / 85f, 0.01960784f, 0.8588235f, 1f);

	public static Color Iris => new Color(0.3843137f, 0.345098f, 0.7686275f, 1f);

	public static Color PastelOrange => new Color(1f, 0.5882353f, 0.3098039f, 1f);

	public static Color YellowishOrange => new Color(1f, 0.6705883f, 1f / 17f, 1f);

	public static Color Perrywinkle => new Color(0.5607843f, 0.5490196f, 0.9058824f, 1f);

	public static Color Tealish => new Color(0.1411765f, 0.7372549f, 0.6588235f, 1f);

	public static Color DarkPlum => new Color(0.2470588f, 0.003921569f, 0.172549f, 1f);

	public static Color Pear => new Color(0.7960784f, 0.972549f, 0.372549f, 1f);

	public static Color PinkishOrange => new Color(1f, 0.4470588f, 0.2980392f, 1f);

	public static Color MidnightPurple => new Color(0.1568628f, 0.003921569f, 0.2156863f, 1f);

	public static Color LightUrple => new Color(0.7019608f, 0.4352941f, 82f / 85f, 1f);

	public static Color DarkMint => new Color(0.282353f, 64f / 85f, 0.4470588f, 1f);

	public static Color GreenishTan => new Color(0.7372549f, 0.7960784f, 0.4784314f, 1f);

	public static Color LightBurgundy => new Color(0.6588235f, 0.254902f, 0.3568628f, 1f);

	public static Color TurquoiseBlue => new Color(0.02352941f, 0.6941177f, 0.7686275f, 1f);

	public static Color UglyPink => new Color(41f / 51f, 0.4588235f, 44f / 85f, 1f);

	public static Color Sandy => new Color((float)Math.E * 105f / 302f, 0.854902f, 0.4784314f, 1f);

	public static Color ElectricPink => new Color(1f, 0.01568628f, 48f / 85f, 1f);

	public static Color MutedPurple => new Color(0.5019608f, 0.3568628f, 0.5294118f, 1f);

	public static Color MidGreen => new Color(16f / 51f, 0.654902f, 0.2784314f, 1f);

	public static Color Greyish => new Color(0.6588235f, 0.6431373f, 0.5843138f, 1f);

	public static Color NeonYellow => new Color(69f / 85f, 1f, 0.01568628f, 1f);

	public static Color Banana => new Color(1f, 1f, 0.4941176f, 1f);

	public static Color CarnationPink => new Color(1f, 0.4980392f, 0.654902f, 1f);

	public static Color Tomato => new Color(0.9372549f, 0.2509804f, 0.1490196f, 1f);

	public static Color Sea => new Color(0.2352941f, 0.6f, 0.572549f, 1f);

	public static Color MuddyBrown => new Color(0.5333334f, 0.4078431f, 0.02352941f, 1f);

	public static Color TurquoiseGreen => new Color(0.01568628f, 0.9568627f, 0.5372549f, 1f);

	public static Color Buff => new Color(0.9960784f, 82f / 85f, 0.6196079f, 1f);

	public static Color Fawn => new Color(69f / 85f, 35f / 51f, 0.4823529f, 1f);

	public static Color MutedBlue => new Color(0.2313726f, 0.4431373f, 0.6235294f, 1f);

	public static Color PaleRose => new Color(0.9921569f, 0.7568628f, 0.772549f, 1f);

	public static Color DarkMintGreen => new Color(0.1254902f, 64f / 85f, 23f / 51f, 1f);

	public static Color Amethyst => new Color(0.6078432f, 0.372549f, 64f / 85f, 1f);

	public static Color Blue_Green => new Color(1f / 17f, 0.6078432f, 0.5568628f, 1f);

	public static Color Chestnut => new Color(0.454902f, 0.1568628f, 0.007843138f, 1f);

	public static Color SickGreen => new Color(0.6156863f, 37f / 51f, 0.172549f, 1f);

	public static Color Pea => new Color(0.6431373f, 0.7490196f, 0.1254902f, 1f);

	public static Color RustyOrange => new Color(41f / 51f, 0.3490196f, 3f / 85f, 1f);

	public static Color Stone => new Color(0.6784314f, 0.6470588f, 0.5294118f, 1f);

	public static Color RoseRed => new Color(0.7450981f, 0.003921569f, 0.2352941f, 1f);

	public static Color PaleAqua => new Color(0.7215686f, 1f, 0.9215686f, 1f);

	public static Color DeepOrange => new Color(44f / 51f, 0.3019608f, 0.003921569f, 1f);

	public static Color Earth => new Color(0.6352941f, 0.3960784f, 0.2431373f, 1f);

	public static Color MossyGreen => new Color(33f / 85f, 0.5450981f, 0.1529412f, 1f);

	public static Color GrassyGreen => new Color(0.254902f, 52f / 85f, 0.01176471f, 1f);

	public static Color PaleLimeGreen => new Color(0.6941177f, 1f, 0.3960784f, 1f);

	public static Color LightGreyBlue => new Color(0.6156863f, 0.7372549f, 0.8313726f, 1f);

	public static Color PaleGrey => new Color(0.9921569f, 0.9921569f, 0.9960784f, 1f);

	public static Color Asparagus => new Color(0.4666667f, 0.6705883f, 0.3372549f, 1f);

	public static Color Blueberry => new Color(0.2745098f, 0.254902f, 0.5882353f, 1f);

	public static Color PurpleRed => new Color(0.6f, 0.003921569f, 0.2784314f, 1f);

	public static Color PaleLime => new Color(0.7450981f, 0.9921569f, 23f / 51f, 1f);

	public static Color GreenishTeal => new Color(0.1960784f, 0.7490196f, 44f / 85f, 1f);

	public static Color Caramel => new Color(35f / 51f, 0.4352941f, 3f / 85f, 1f);

	public static Color DeepMagenta => new Color(32f / 51f, 0.007843138f, 0.3607843f, 1f);

	public static Color LightPeach => new Color(1f, 0.8470588f, 0.6941177f, 1f);

	public static Color MilkChocolate => new Color(0.4980392f, 0.3058824f, 0.1176471f, 1f);

	public static Color Ocher => new Color(0.7490196f, 0.6078432f, 0.04705882f, 1f);

	public static Color OffGreen => new Color(0.4196078f, 0.6392157f, 0.3254902f, 1f);

	public static Color PurplyPink => new Color(0.9411765f, 0.4588235f, 46f / 51f, 1f);

	public static Color Lightblue => new Color(0.4823529f, 0.7843137f, 82f / 85f, 1f);

	public static Color DuskyBlue => new Color(0.2784314f, 0.372549f, 0.5803922f, 1f);

	public static Color Golden => new Color(49f / 51f, 0.7490196f, 0.01176471f, 1f);

	public static Color LightBeige => new Color(1f, 0.9960784f, 0.7137255f, 1f);

	public static Color ButterYellow => new Color(1f, 0.9921569f, 0.454902f, 1f);

	public static Color DuskyPurple => new Color(0.5372549f, 0.3568628f, 0.4823529f, 1f);

	public static Color FrenchBlue => new Color(0.2627451f, 0.4196078f, 0.6784314f, 1f);

	public static Color UglyYellow => new Color(0.8156863f, 0.7568628f, 0.003921569f, 1f);

	public static Color GreenyYellow => new Color(66f / 85f, 0.972549f, 0.03137255f, 1f);

	public static Color OrangishRed => new Color(0.9568627f, 0.2117647f, 0.01960784f, 1f);

	public static Color ShamrockGreen => new Color(0.007843138f, 0.7568628f, 0.3019608f, 1f);

	public static Color OrangishBrown => new Color(0.6980392f, 0.372549f, 0.01176471f, 1f);

	public static Color TreeGreen => new Color(0.1647059f, 0.4941176f, 5f / 51f, 1f);

	public static Color DeepViolet => new Color(0.2862745f, 0.02352941f, 0.282353f, 1f);

	public static Color Gunmetal => new Color(0.3254902f, 0.3843137f, 0.4039216f, 1f);

	public static Color Blue_Purple => new Color(0.3529412f, 0.02352941f, 0.9372549f, 1f);

	public static Color Cherry => new Color(69f / 85f, 0.007843138f, 0.2039216f, 1f);

	public static Color SandyBrown => new Color(0.7686275f, 0.6509804f, 0.3803922f, 1f);

	public static Color WarmGrey => new Color(0.5921569f, 46f / 85f, 44f / 85f, 1f);

	public static Color DarkIndigo => new Color(0.1215686f, 3f / 85f, 0.3294118f, 1f);

	public static Color Midnight => new Color(0.01176471f, 0.003921569f, 0.1764706f, 1f);

	public static Color BlueyGreen => new Color(0.1686275f, 0.6941177f, 0.4745098f, 1f);

	public static Color GreyPink => new Color(0.7647059f, 48f / 85f, 0.6078432f, 1f);

	public static Color SoftPurple => new Color(0.6509804f, 0.4352941f, 0.7098039f, 1f);

	public static Color Blood => new Color(0.4666667f, 0f, 0.003921569f, 1f);

	public static Color BrownRed => new Color(0.572549f, 0.1686275f, 0.01960784f, 1f);

	public static Color MediumGrey => new Color(0.4901961f, 0.4980392f, 0.4862745f, 1f);

	public static Color Berry => new Color(0.6f, 1f / 17f, 0.2941177f, 1f);

	public static Color Poo => new Color(0.5607843f, 23f / 51f, 0.01176471f, 1f);

	public static Color PurpleyPink => new Color(0.7843137f, 0.2352941f, 37f / 51f, 1f);

	public static Color LightSalmon => new Color(0.9960784f, 0.6627451f, 49f / 85f, 1f);

	public static Color Snot => new Color(0.6745098f, 0.7333333f, 0.05098039f, 1f);

	public static Color EasterPurple => new Color(64f / 85f, 0.4431373f, 0.9960784f, 1f);

	public static Color LightYellowGreen => new Color(0.8f, 0.9921569f, 0.4980392f, 1f);

	public static Color DarkNavyBlue => new Color(0f, 0.007843138f, 0.1803922f, 1f);

	public static Color Drab => new Color(0.509804f, 0.5137255f, 0.2666667f, 1f);

	public static Color LightRose => new Color(1f, 0.772549f, 0.7960784f, 1f);

	public static Color Rouge => new Color(0.6705883f, 6f / 85f, 0.2235294f, 1f);

	public static Color PurplishRed => new Color(0.6901961f, 0.01960784f, 0.2941177f, 1f);

	public static Color SlimeGreen => new Color(0.6f, 0.8f, 0.01568628f, 1f);

	public static Color BabyPoop => new Color(49f / 85f, 0.4862745f, 0f, 1f);

	public static Color IrishGreen => new Color(0.003921569f, 0.5843138f, 0.1607843f, 1f);

	public static Color Pink_Purple => new Color(0.9372549f, 0.1137255f, 0.9058824f, 1f);

	public static Color DarkNavy => new Color(0f, 0.01568628f, 0.2078431f, 1f);

	public static Color GreenyBlue => new Color(0.2588235f, 0.7019608f, 0.5843138f, 1f);

	public static Color LightPlum => new Color(0.6156863f, 0.3411765f, 0.5137255f, 1f);

	public static Color PinkishGrey => new Color(0.7843137f, 0.6745098f, 0.6627451f, 1f);

	public static Color DirtyOrange => new Color(0.7843137f, 0.4627451f, 0.02352941f, 1f);

	public static Color RustRed => new Color(2f / 3f, 0.1529412f, 0.01568628f, 1f);

	public static Color PaleLilac => new Color(0.8941177f, 0.7960784f, 1f, 1f);

	public static Color OrangeyRed => new Color(0.9803922f, 0.2588235f, 0.1411765f, 1f);

	public static Color PrimaryBlue => new Color(0.03137255f, 0.01568628f, 83f / 85f, 1f);

	public static Color KermitGreen => new Color(0.3607843f, 0.6980392f, 0f, 1f);

	public static Color BrownishPurple => new Color(0.4627451f, 0.2588235f, 0.3058824f, 1f);

	public static Color MurkyGreen => new Color(0.4235294f, 0.4784314f, 0.05490196f, 1f);

	public static Color Wheat => new Color(0.9843137f, 13f / 15f, 0.4941176f, 1f);

	public static Color VeryDarkPurple => new Color(0.1647059f, 0.003921569f, 0.2039216f, 1f);

	public static Color BottleGreen => new Color(0.01568628f, 0.2901961f, 0.01960784f, 1f);

	public static Color Watermelon => new Color(0.9921569f, 0.2745098f, 0.3490196f, 1f);

	public static Color DeepSkyBlue => new Color(0.05098039f, 0.4588235f, 0.972549f, 1f);

	public static Color FireEngineRed => new Color(0.9960784f, 0f, 0.007843138f, 1f);

	public static Color YellowOchre => new Color(0.7960784f, 0.6156863f, 0.02352941f, 1f);

	public static Color PumpkinOrange => new Color(0.9843137f, 0.4901961f, 0.02745098f, 1f);

	public static Color PaleOlive => new Color(37f / 51f, 0.8f, 43f / 85f, 1f);

	public static Color LightLilac => new Color(0.9294118f, 0.7843137f, 1f, 1f);

	public static Color LightishGreen => new Color(0.3803922f, 0.8823529f, 32f / 85f, 1f);

	public static Color CarolinaBlue => new Color(46f / 85f, 0.7215686f, 0.9960784f, 1f);

	public static Color Mulberry => new Color(0.572549f, 0.03921569f, 0.3058824f, 1f);

	public static Color ShockingPink => new Color(0.9960784f, 0.007843138f, 0.6352941f, 1f);

	public static Color Auburn => new Color(0.6039216f, 16f / 85f, 0.003921569f, 1f);

	public static Color BrightLimeGreen => new Color(0.3960784f, 0.9960784f, 0.03137255f, 1f);

	public static Color Celadon => new Color(0.7450981f, 0.9921569f, 61f / 85f, 1f);

	public static Color PinkishBrown => new Color(0.6941177f, 0.4470588f, 0.3803922f, 1f);

	public static Color PooBrown => new Color(0.5333334f, 0.372549f, 0.003921569f, 1f);

	public static Color BrightSkyBlue => new Color(0.007843138f, 0.8f, 0.9960784f, 1f);

	public static Color Celery => new Color(0.7568628f, 0.9921569f, 0.5843138f, 1f);

	public static Color DirtBrown => new Color(0.5137255f, 0.3960784f, 0.2235294f, 1f);

	public static Color Strawberry => new Color(0.9843137f, 0.1607843f, 0.2627451f, 1f);

	public static Color DarkLime => new Color(44f / 85f, 61f / 85f, 0.003921569f, 1f);

	public static Color Copper => new Color(0.7137255f, 33f / 85f, 0.145098f, 1f);

	public static Color MediumBrown => new Color(0.4980392f, 0.3176471f, 6f / 85f, 1f);

	public static Color MutedGreen => new Color(0.372549f, 32f / 51f, 0.3215686f, 1f);

	public static Color RobinSEgg => new Color(0.427451f, 0.9294118f, 0.9921569f, 1f);

	public static Color BrightAqua => new Color(0.04313726f, 83f / 85f, 0.9176471f, 1f);

	public static Color BrightLavender => new Color(0.7803922f, 32f / 85f, 1f, 1f);

	public static Color Ivory => new Color(1f, 1f, 0.7960784f, 1f);

	public static Color VeryLightPurple => new Color(82f / 85f, 0.8078431f, 84f / 85f, 1f);

	public static Color LightNavy => new Color(7f / 85f, 16f / 51f, 44f / 85f, 1f);

	public static Color PinkRed => new Color(49f / 51f, 0.01960784f, 0.3098039f, 1f);

	public static Color OliveBrown => new Color(0.3921569f, 0.3294118f, 0.01176471f, 1f);

	public static Color PoopBrown => new Color(0.4784314f, 0.3490196f, 0.003921569f, 1f);

	public static Color MustardGreen => new Color(0.6588235f, 0.7098039f, 0.01568628f, 1f);

	public static Color OceanGreen => new Color(0.2392157f, 0.6f, 23f / 51f, 1f);

	public static Color VeryDarkBlue => new Color(0f, 0.003921569f, 0.2f, 1f);

	public static Color DustyGreen => new Color(0.4627451f, 0.6627451f, 23f / 51f, 1f);

	public static Color LightNavyBlue => new Color(0.1803922f, 0.3529412f, 0.5333334f, 1f);

	public static Color MintyGreen => new Color(0.04313726f, 0.9686275f, 0.4901961f, 1f);

	public static Color Adobe => new Color(63f / 85f, 0.4235294f, 0.282353f, 1f);

	public static Color Barney => new Color(0.6745098f, 0.1137255f, 0.7215686f, 1f);

	public static Color JadeGreen => new Color(0.1686275f, 35f / 51f, 0.4156863f, 1f);

	public static Color BrightLightBlue => new Color(0.1490196f, 0.9686275f, 0.9921569f, 1f);

	public static Color LightLime => new Color(0.682353f, 0.9921569f, 0.4235294f, 1f);

	public static Color DarkKhaki => new Color(0.6078432f, 0.5607843f, 0.3333333f, 1f);

	public static Color OrangeYellow => new Color(1f, 0.6784314f, 0.003921569f, 1f);

	public static Color Ocre => new Color(66f / 85f, 52f / 85f, 0.01568628f, 1f);

	public static Color Maize => new Color(0.9568627f, 0.8156863f, 0.3294118f, 1f);

	public static Color FadedPink => new Color(0.8705882f, 0.6156863f, 0.6745098f, 1f);

	public static Color BritishRacingGreen => new Color(0.01960784f, 0.282353f, 0.05098039f, 1f);

	public static Color Sandstone => new Color(67f / 85f, 0.682353f, 0.454902f, 1f);

	public static Color MudBrown => new Color(32f / 85f, 0.2745098f, 1f / 17f, 1f);

	public static Color LightSeaGreen => new Color(0.5960785f, 82f / 85f, 0.6901961f, 1f);

	public static Color RobinEggBlue => new Color(46f / 85f, (float)Math.E * 105f / 302f, 0.9960784f, 1f);

	public static Color AquaMarine => new Color(0.1803922f, 0.9098039f, 0.7333333f, 1f);

	public static Color DarkSeaGreen => new Color(1f / 15f, 0.5294118f, 31f / 85f, 1f);

	public static Color SoftPink => new Color(0.9921569f, 0.6901961f, 64f / 85f, 1f);

	public static Color OrangeyBrown => new Color(0.6941177f, 32f / 85f, 0.007843138f, 1f);

	public static Color CherryRed => new Color(0.9686275f, 0.007843138f, 0.1647059f, 1f);

	public static Color BurntYellow => new Color(71f / 85f, 0.6705883f, 3f / 85f, 1f);

	public static Color BrownishGrey => new Color(0.5254902f, 0.4666667f, 0.372549f, 1f);

	public static Color Camel => new Color(66f / 85f, 0.6235294f, 0.3490196f, 1f);

	public static Color PurplishGrey => new Color(0.4784314f, 0.4078431f, 0.4980392f, 1f);

	public static Color Marine => new Color(0.01568628f, 0.1803922f, 32f / 85f, 1f);

	public static Color GreyishPink => new Color(0.7843137f, 47f / 85f, 0.5803922f, 1f);

	public static Color PaleTurquoise => new Color(0.6470588f, 0.9843137f, 71f / 85f, 1f);

	public static Color PastelYellow => new Color(1f, 0.9960784f, 0.4431373f, 1f);

	public static Color BlueyPurple => new Color(0.3843137f, 0.254902f, 0.7803922f, 1f);

	public static Color CanaryYellow => new Color(1f, 0.9960784f, 0.2509804f, 1f);

	public static Color FadedRed => new Color(0.827451f, 0.2862745f, 0.3058824f, 1f);

	public static Color Sepia => new Color(0.5960785f, 0.3686275f, 0.1686275f, 1f);

	public static Color Coffee => new Color(0.6509804f, 43f / 85f, 0.2980392f, 1f);

	public static Color BrightMagenta => new Color(1f, 0.03137255f, 0.9098039f, 1f);

	public static Color Mocha => new Color(0.6156863f, 0.4627451f, 0.3176471f, 1f);

	public static Color Ecru => new Color(0.9960784f, 1f, 0.7921569f, 1f);

	public static Color Purpleish => new Color(0.5960785f, 0.3372549f, 47f / 85f, 1f);

	public static Color Cranberry => new Color(0.6196079f, 0f, 0.227451f, 1f);

	public static Color DarkishGreen => new Color(0.1568628f, 0.4862745f, 0.2156863f, 1f);

	public static Color BrownOrange => new Color(37f / 51f, 0.4117647f, 0.007843138f, 1f);

	public static Color DuskyRose => new Color(62f / 85f, 0.4078431f, 23f / 51f, 1f);

	public static Color Melon => new Color(1f, 0.4705882f, 0.3333333f, 1f);

	public static Color SicklyGreen => new Color(0.5803922f, 0.6980392f, 0.1098039f, 1f);

	public static Color Silver => new Color(0.772549f, 67f / 85f, 0.7803922f, 1f);

	public static Color PurplyBlue => new Color(0.4f, 0.1019608f, 0.9333333f, 1f);

	public static Color PurpleishBlue => new Color(0.3803922f, 0.2509804f, 0.9372549f, 1f);

	public static Color HospitalGreen => new Color(0.6078432f, 0.8980392f, 2f / 3f, 1f);

	public static Color ShitBrown => new Color(0.4823529f, 0.345098f, 0.01568628f, 1f);

	public static Color MidBlue => new Color(0.1529412f, 0.4156863f, 0.7019608f, 1f);

	public static Color Amber => new Color(0.9960784f, 0.7019608f, 0.03137255f, 1f);

	public static Color EasterGreen => new Color(0.5490196f, 0.9921569f, 0.4941176f, 1f);

	public static Color SoftBlue => new Color(0.3921569f, 0.5333334f, 0.9176471f, 1f);

	public static Color CeruleanBlue => new Color(0.01960784f, 0.4313726f, 0.9333333f, 1f);

	public static Color GoldenBrown => new Color(0.6980392f, 0.4784314f, 0.003921569f, 1f);

	public static Color BrightTurquoise => new Color(1f / 17f, 0.9960784f, 83f / 85f, 1f);

	public static Color RedPink => new Color(0.9803922f, 0.1647059f, 0.3333333f, 1f);

	public static Color RedPurple => new Color(0.509804f, 0.02745098f, 0.2784314f, 1f);

	public static Color GreyishBrown => new Color(0.4784314f, 0.4156863f, 0.3098039f, 1f);

	public static Color Vermillion => new Color(0.9568627f, 0.1960784f, 0.04705882f, 1f);

	public static Color Russet => new Color(0.6313726f, 0.2235294f, 0.01960784f, 1f);

	public static Color SteelGrey => new Color(0.4352941f, 0.509804f, 46f / 85f, 1f);

	public static Color LighterPurple => new Color(0.6470588f, 0.3529412f, 0.9568627f, 1f);

	public static Color BrightViolet => new Color(0.6784314f, 0.03921569f, 0.9921569f, 1f);

	public static Color PrussianBlue => new Color(0f, 0.2705882f, 0.4666667f, 1f);

	public static Color SlateGreen => new Color(0.3960784f, 47f / 85f, 0.427451f, 1f);

	public static Color DirtyPink => new Color(0.7921569f, 0.4823529f, 0.5019608f, 1f);

	public static Color DarkBlueGreen => new Color(0f, 0.3215686f, 0.2862745f, 1f);

	public static Color Pine => new Color(0.1686275f, 31f / 85f, 0.2039216f, 1f);

	public static Color YellowyGreen => new Color(0.7490196f, (float)Math.E * 105f / 302f, 0.1568628f, 1f);

	public static Color DarkGold => new Color(0.7098039f, 0.5803922f, 0.0627451f, 1f);

	public static Color Bluish => new Color(0.1607843f, 0.4627451f, 0.7333333f, 1f);

	public static Color DarkishBlue => new Color(0.003921569f, 0.254902f, 0.509804f, 1f);

	public static Color DullRed => new Color(0.7333333f, 0.2470588f, 0.2470588f, 1f);

	public static Color PinkyRed => new Color(84f / 85f, 0.1490196f, 0.2784314f, 1f);

	public static Color Bronze => new Color(0.6588235f, 0.4745098f, 0f, 1f);

	public static Color PaleTeal => new Color(0.509804f, 0.7960784f, 0.6980392f, 1f);

	public static Color MilitaryGreen => new Color(0.4f, 0.4862745f, 0.2431373f, 1f);

	public static Color BarbiePink => new Color(0.9960784f, 0.2745098f, 0.6470588f, 1f);

	public static Color BubblegumPink => new Color(0.9960784f, 0.5137255f, 0.8f, 1f);

	public static Color PeaSoupGreen => new Color(0.5803922f, 0.6509804f, 0.09019608f, 1f);

	public static Color DarkMustard => new Color(0.6588235f, 0.5372549f, 0.01960784f, 1f);

	public static Color Shit => new Color(0.4980392f, 0.372549f, 0f, 1f);

	public static Color MediumPurple => new Color(0.6196079f, 0.2627451f, 0.6352941f, 1f);

	public static Color VeryDarkGreen => new Color(0.02352941f, 0.1803922f, 0.01176471f, 1f);

	public static Color Dirt => new Color(46f / 85f, 0.4313726f, 0.2705882f, 1f);

	public static Color DuskyPink => new Color(0.8f, 0.4784314f, 0.5450981f, 1f);

	public static Color RedViolet => new Color(0.6196079f, 0.003921569f, 0.4078431f, 1f);

	public static Color LemonYellow => new Color(0.9921569f, 1f, 0.2196078f, 1f);

	public static Color Pistachio => new Color(64f / 85f, 0.9803922f, 0.5450981f, 1f);

	public static Color DullYellow => new Color(0.9333333f, 44f / 51f, 0.3568628f, 1f);

	public static Color DarkLimeGreen => new Color(0.4941176f, 63f / 85f, 0.003921569f, 1f);

	public static Color DenimBlue => new Color(0.2313726f, 0.3568628f, 0.572549f, 1f);

	public static Color TealBlue => new Color(0.003921569f, 0.5333334f, 0.6235294f, 1f);

	public static Color LightishBlue => new Color(0.2392157f, 0.4784314f, 0.9921569f, 1f);

	public static Color PurpleyBlue => new Color(0.372549f, 0.2039216f, 0.9058824f, 1f);

	public static Color LightIndigo => new Color(0.427451f, 0.3529412f, 69f / 85f, 1f);

	public static Color SwampGreen => new Color(0.454902f, 0.5215687f, 0f, 1f);

	public static Color BrownGreen => new Color(0.4392157f, 0.4235294f, 1f / 15f, 1f);

	public static Color DarkMaroon => new Color(0.2352941f, 0f, 0.03137255f, 1f);

	public static Color HotPurple => new Color(0.7960784f, 0f, 49f / 51f, 1f);

	public static Color DarkForestGreen => new Color(0f, 0.1764706f, 0.01568628f, 1f);

	public static Color FadedBlue => new Color(0.3960784f, 0.5490196f, 0.7333333f, 1f);

	public static Color DrabGreen => new Color(0.454902f, 0.5843138f, 0.3176471f, 1f);

	public static Color LightLimeGreen => new Color(37f / 51f, 1f, 0.4f, 1f);

	public static Color SnotGreen => new Color(0.6156863f, 0.7568628f, 0f, 1f);

	public static Color Yellowish => new Color(0.9803922f, 0.9333333f, 0.4f, 1f);

	public static Color LightBlueGreen => new Color(0.4941176f, 0.9843137f, 0.7019608f, 1f);

	public static Color Bordeaux => new Color(0.4823529f, 0f, 0.172549f, 1f);

	public static Color LightMauve => new Color(0.7607843f, 0.572549f, 0.6313726f, 1f);

	public static Color Ocean => new Color(0.003921569f, 0.4823529f, 0.572549f, 1f);

	public static Color Marigold => new Color(84f / 85f, 64f / 85f, 0.02352941f, 1f);

	public static Color MuddyGreen => new Color(0.3960784f, 0.454902f, 0.1960784f, 1f);

	public static Color DullOrange => new Color(0.8470588f, 0.5254902f, 0.2313726f, 1f);

	public static Color Steel => new Color(23f / 51f, 0.5215687f, 0.5843138f, 1f);

	public static Color ElectricPurple => new Color(2f / 3f, 0.1372549f, 1f, 1f);

	public static Color FluorescentGreen => new Color(0.03137255f, 1f, 0.03137255f, 1f);

	public static Color YellowishBrown => new Color(0.6078432f, 0.4784314f, 0.003921569f, 1f);

	public static Color Blush => new Color(0.9490196f, 0.6196079f, 0.5568628f, 1f);

	public static Color SoftGreen => new Color(0.4352941f, 0.7607843f, 0.4627451f, 1f);

	public static Color BrightOrange => new Color(1f, 0.3568628f, 0f, 1f);

	public static Color Lemon => new Color(0.9921569f, 1f, 0.3215686f, 1f);

	public static Color PurpleGrey => new Color(0.5254902f, 0.4352941f, 0.5215687f, 1f);

	public static Color AcidGreen => new Color(0.5607843f, 0.9960784f, 3f / 85f, 1f);

	public static Color PaleLavender => new Color(0.9333333f, 69f / 85f, 0.9960784f, 1f);

	public static Color VioletBlue => new Color(0.3176471f, 0.03921569f, 67f / 85f, 1f);

	public static Color LightForestGreen => new Color(0.3098039f, 29f / 51f, 0.3254902f, 1f);

	public static Color BurntRed => new Color(0.6235294f, 0.1372549f, 0.01960784f, 1f);

	public static Color KhakiGreen => new Color(0.4470588f, 0.5254902f, 0.2235294f, 1f);

	public static Color Cerise => new Color(0.8705882f, 0.04705882f, 0.3843137f, 1f);

	public static Color FadedPurple => new Color(29f / 51f, 0.4313726f, 0.6f, 1f);

	public static Color Apricot => new Color(1f, 0.6941177f, 0.427451f, 1f);

	public static Color DarkOliveGreen => new Color(0.2352941f, 0.3019608f, 0.01176471f, 1f);

	public static Color GreyBrown => new Color(0.4980392f, 0.4392157f, 0.3254902f, 1f);

	public static Color GreenGrey => new Color(0.4666667f, 0.572549f, 0.4352941f, 1f);

	public static Color TrueBlue => new Color(0.003921569f, 1f / 17f, 0.8f, 1f);

	public static Color PaleViolet => new Color(0.8078431f, 0.682353f, 0.9803922f, 1f);

	public static Color PeriwinkleBlue => new Color(0.5607843f, 0.6f, 0.9843137f, 1f);

	public static Color LightSkyBlue => new Color(66f / 85f, 84f / 85f, 1f, 1f);

	public static Color Blurple => new Color(0.3333333f, 0.2235294f, 0.8f, 1f);

	public static Color GreenBrown => new Color(0.3294118f, 0.3058824f, 0.01176471f, 1f);

	public static Color Bluegreen => new Color(0.003921569f, 0.4784314f, 0.4745098f, 1f);

	public static Color BrightTeal => new Color(0.003921569f, 83f / 85f, 66f / 85f, 1f);

	public static Color BrownishYellow => new Color(67f / 85f, 0.6901961f, 0.01176471f, 1f);

	public static Color PeaSoup => new Color(0.572549f, 0.6f, 0.003921569f, 1f);

	public static Color Forest => new Color(0.04313726f, 0.3333333f, 3f / 85f, 1f);

	public static Color BarneyPurple => new Color(32f / 51f, 0.01568628f, 0.5960785f, 1f);

	public static Color Ultramarine => new Color(0.1254902f, 0f, 0.6941177f, 1f);

	public static Color Purplish => new Color(0.5803922f, 0.3372549f, 0.5490196f, 1f);

	public static Color PukeYellow => new Color(0.7607843f, 0.7450981f, 0.05490196f, 1f);

	public static Color BluishGrey => new Color(0.454902f, 0.5450981f, 0.5921569f, 1f);

	public static Color DarkPeriwinkle => new Color(0.4f, 0.372549f, 0.8196079f, 1f);

	public static Color DarkLilac => new Color(52f / 85f, 0.427451f, 0.6470588f, 1f);

	public static Color Reddish => new Color(0.7686275f, 0.2588235f, 0.2509804f, 1f);

	public static Color LightMaroon => new Color(0.6352941f, 0.282353f, 0.3411765f, 1f);

	public static Color DustyPurple => new Color(0.509804f, 0.372549f, 0.5294118f, 1f);

	public static Color TerraCotta => new Color(67f / 85f, 0.3921569f, 0.2313726f, 1f);

	public static Color Avocado => new Color(48f / 85f, 0.6941177f, 0.2039216f, 1f);

	public static Color MarineBlue => new Color(0.003921569f, 0.2196078f, 0.4156863f, 1f);

	public static Color TealGreen => new Color(0.145098f, 0.6392157f, 0.4352941f, 1f);

	public static Color SlateGrey => new Color(0.3490196f, 0.3960784f, 0.427451f, 1f);

	public static Color LighterGreen => new Color(0.4588235f, 0.9921569f, 33f / 85f, 1f);

	public static Color ElectricGreen => new Color(0.1294118f, 84f / 85f, 0.05098039f, 1f);

	public static Color DustyBlue => new Color(0.3529412f, 0.5254902f, 0.6784314f, 1f);

	public static Color GoldenYellow => new Color(0.9960784f, 66f / 85f, 7f / 85f, 1f);

	public static Color BrightYellow => new Color(1f, 0.9921569f, 0.003921569f, 1f);

	public static Color LightLavender => new Color(0.8745098f, 0.772549f, 0.9960784f, 1f);

	public static Color Umber => new Color(0.6980392f, 0.3921569f, 0f, 1f);

	public static Color Poop => new Color(0.4980392f, 0.3686275f, 0f, 1f);

	public static Color DarkPeach => new Color(0.8705882f, 0.4941176f, 31f / 85f, 1f);

	public static Color JungleGreen => new Color(0.01568628f, 0.509804f, 0.2627451f, 1f);

	public static Color Eggshell => new Color(1f, 1f, 0.8313726f, 1f);

	public static Color Denim => new Color(0.2313726f, 33f / 85f, 0.5490196f, 1f);

	public static Color YellowBrown => new Color(61f / 85f, 0.5803922f, 0f, 1f);

	public static Color DullPurple => new Color(44f / 85f, 0.3490196f, 0.4941176f, 1f);

	public static Color ChocolateBrown => new Color(0.254902f, 5f / 51f, 0f, 1f);

	public static Color WineRed => new Color(0.4823529f, 0.01176471f, 0.1372549f, 1f);

	public static Color NeonBlue => new Color(0.01568628f, 0.8509804f, 1f, 1f);

	public static Color DirtyGreen => new Color(0.4f, 0.4941176f, 0.172549f, 1f);

	public static Color LightTan => new Color(0.9843137f, 0.9333333f, 0.6745098f, 1f);

	public static Color IceBlue => new Color(0.8431373f, 1f, 0.9960784f, 1f);

	public static Color CadetBlue => new Color(0.3058824f, 0.454902f, 0.5882353f, 1f);

	public static Color DarkMauve => new Color(0.5294118f, 0.2980392f, 0.3843137f, 1f);

	public static Color VeryLightBlue => new Color(71f / 85f, 1f, 1f, 1f);

	public static Color GreyPurple => new Color(0.509804f, 0.427451f, 0.5490196f, 1f);

	public static Color PastelPink => new Color(1f, 62f / 85f, 41f / 51f, 1f);

	public static Color VeryLightGreen => new Color(0.8196079f, 1f, 63f / 85f, 1f);

	public static Color DarkSkyBlue => new Color(0.2666667f, 0.5568628f, 0.8941177f, 1f);

	public static Color Evergreen => new Color(0.01960784f, 0.2784314f, 0.1647059f, 1f);

	public static Color DullPink => new Color(71f / 85f, 0.5254902f, 0.6156863f, 1f);

	public static Color Aubergine => new Color(0.2392157f, 0.02745098f, 0.2039216f, 1f);

	public static Color Mahogany => new Color(0.2901961f, 0.003921569f, 0f, 1f);

	public static Color ReddishOrange => new Color(0.972549f, 0.282353f, 0.1098039f, 1f);

	public static Color DeepGreen => new Color(0.007843138f, 0.3490196f, 1f / 17f, 1f);

	public static Color VomitGreen => new Color(0.5372549f, 0.6352941f, 0.01176471f, 1f);

	public static Color PurplePink => new Color(0.8784314f, 0.2470588f, 0.8470588f, 1f);

	public static Color DustyPink => new Color(71f / 85f, 46f / 85f, 0.5803922f, 1f);

	public static Color FadedGreen => new Color(0.4823529f, 0.6980392f, 0.454902f, 1f);

	public static Color CamoGreen => new Color(0.3215686f, 0.3960784f, 0.145098f, 1f);

	public static Color PinkyPurple => new Color(67f / 85f, 0.2980392f, 0.7450981f, 1f);

	public static Color PinkPurple => new Color(0.8588235f, 0.2941177f, 0.854902f, 1f);

	public static Color BrownishRed => new Color(0.6196079f, 0.2117647f, 0.1372549f, 1f);

	public static Color DarkRose => new Color(0.7098039f, 0.282353f, 31f / 85f, 1f);

	public static Color Mud => new Color(23f / 51f, 0.3607843f, 6f / 85f, 1f);

	public static Color Brownish => new Color(52f / 85f, 0.427451f, 0.3411765f, 1f);

	public static Color EmeraldGreen => new Color(0.007843138f, 0.5607843f, 0.1176471f, 1f);

	public static Color PaleBrown => new Color(0.6941177f, 29f / 51f, 0.4313726f, 1f);

	public static Color DullBlue => new Color(0.2862745f, 0.4588235f, 52f / 85f, 1f);

	public static Color BurntUmber => new Color(32f / 51f, 0.2705882f, 0.05490196f, 1f);

	public static Color MediumGreen => new Color(0.2235294f, 0.6784314f, 0.282353f, 1f);

	public static Color Clay => new Color(0.7137255f, 0.4156863f, 16f / 51f, 1f);

	public static Color LightAqua => new Color(0.5490196f, 1f, 0.8588235f, 1f);

	public static Color LightOliveGreen => new Color(0.6431373f, 0.7450981f, 0.3607843f, 1f);

	public static Color BrownishOrange => new Color(0.7960784f, 0.4666667f, 0.1372549f, 1f);

	public static Color DarkAqua => new Color(0.01960784f, 0.4117647f, 0.4196078f, 1f);

	public static Color PurplishPink => new Color(0.8078431f, 31f / 85f, 0.682353f, 1f);

	public static Color DarkSalmon => new Color(0.7843137f, 0.3529412f, 0.3254902f, 1f);

	public static Color GreenishGrey => new Color(0.5882353f, 0.682353f, 47f / 85f, 1f);

	public static Color Jade => new Color(0.1215686f, 0.654902f, 0.454902f, 1f);

	public static Color UglyGreen => new Color(0.4784314f, 0.5921569f, 0.01176471f, 1f);

	public static Color DarkBeige => new Color(0.6745098f, 49f / 85f, 0.3843137f, 1f);

	public static Color Emerald => new Color(0.003921569f, 32f / 51f, 0.2862745f, 1f);

	public static Color PaleRed => new Color(0.8509804f, 0.3294118f, 0.3019608f, 1f);

	public static Color LightMagenta => new Color(0.9803922f, 0.372549f, 0.9686275f, 1f);

	public static Color Sky => new Color(0.509804f, 0.7921569f, 84f / 85f, 1f);

	public static Color LightCyan => new Color(0.6745098f, 1f, 84f / 85f, 1f);

	public static Color YellowOrange => new Color(84f / 85f, 0.6901961f, 0.003921569f, 1f);

	public static Color ReddishPurple => new Color(29f / 51f, 3f / 85f, 0.3176471f, 1f);

	public static Color ReddishPink => new Color(0.9960784f, 0.172549f, 0.3294118f, 1f);

	public static Color Orchid => new Color(0.7843137f, 0.4588235f, 0.7686275f, 1f);

	public static Color DirtyYellow => new Color(41f / 51f, 0.772549f, 0.03921569f, 1f);

	public static Color OrangeRed => new Color(0.9921569f, 0.254902f, 0.1176471f, 1f);

	public static Color DeepRed => new Color(0.6039216f, 0.007843138f, 0f, 1f);

	public static Color OrangeBrown => new Color(0.7450981f, 0.3921569f, 0f, 1f);

	public static Color CobaltBlue => new Color(0.01176471f, 0.03921569f, 0.654902f, 1f);

	public static Color NeonPink => new Color(0.9960784f, 0.003921569f, 0.6039216f, 1f);

	public static Color RosePink => new Color(0.9686275f, 0.5294118f, 0.6039216f, 1f);

	public static Color GreyishPurple => new Color(0.5333334f, 0.4431373f, 29f / 51f, 1f);

	public static Color Raspberry => new Color(0.6901961f, 0.003921569f, 0.2862745f, 1f);

	public static Color AquaGreen => new Color(6f / 85f, 0.8823529f, 49f / 85f, 1f);

	public static Color SalmonPink => new Color(0.9960784f, 0.4823529f, 0.4862745f, 1f);

	public static Color Tangerine => new Color(1f, 0.5803922f, 0.03137255f, 1f);

	public static Color BrownishGreen => new Color(0.4156863f, 0.4313726f, 3f / 85f, 1f);

	public static Color RedBrown => new Color(0.5450981f, 0.1803922f, 0.08627451f, 1f);

	public static Color GreenishBrown => new Color(0.4117647f, 0.3803922f, 6f / 85f, 1f);

	public static Color Pumpkin => new Color(0.8823529f, 0.4666667f, 0.003921569f, 1f);

	public static Color PineGreen => new Color(0.03921569f, 0.282353f, 0.1176471f, 1f);

	public static Color Charcoal => new Color(0.2039216f, 0.2196078f, 0.2156863f, 1f);

	public static Color BabyPink => new Color(1f, 61f / 85f, 0.8078431f, 1f);

	public static Color Cornflower => new Color(0.4156863f, 0.4745098f, 0.9686275f, 1f);

	public static Color BlueViolet => new Color(31f / 85f, 0.02352941f, 0.9137255f, 1f);

	public static Color Chocolate => new Color(0.2392157f, 0.1098039f, 0.007843138f, 1f);

	public static Color GreyishGreen => new Color(0.509804f, 0.6509804f, 0.4901961f, 1f);

	public static Color Scarlet => new Color(0.7450981f, 0.003921569f, 5f / 51f, 1f);

	public static Color GreenYellow => new Color(67f / 85f, 1f, 0.1529412f, 1f);

	public static Color DarkOlive => new Color(0.2156863f, 0.2431373f, 0.007843138f, 1f);

	public static Color Sienna => new Color(0.6627451f, 0.3372549f, 0.1176471f, 1f);

	public static Color PastelPurple => new Color(0.7921569f, 32f / 51f, 1f, 1f);

	public static Color Terracotta => new Color(0.7921569f, 0.4f, 0.254902f, 1f);

	public static Color AquaBlue => new Color(0.007843138f, 0.8470588f, 0.9137255f, 1f);

	public static Color SageGreen => new Color(0.5333334f, 0.7019608f, 0.4705882f, 1f);

	public static Color BloodRed => new Color(0.5960785f, 0f, 0.007843138f, 1f);

	public static Color DeepPink => new Color(0.7960784f, 0.003921569f, 0.3843137f, 1f);

	public static Color Grass => new Color(0.3607843f, 0.6745098f, 0.1764706f, 1f);

	public static Color Moss => new Color(0.4627451f, 0.6f, 0.345098f, 1f);

	public static Color PastelBlue => new Color(0.6352941f, 0.7490196f, 0.9960784f, 1f);

	public static Color BluishGreen => new Color(0.0627451f, 0.6509804f, 0.454902f, 1f);

	public static Color GreenBlue => new Color(0.02352941f, 0.7058824f, 0.5450981f, 1f);

	public static Color DarkTan => new Color(35f / 51f, 0.5333334f, 0.2901961f, 1f);

	public static Color GreenishBlue => new Color(0.04313726f, 0.5450981f, 0.5294118f, 1f);

	public static Color PaleOrange => new Color(1f, 0.654902f, 0.3372549f, 1f);

	public static Color Vomit => new Color(0.6352941f, 0.6431373f, 7f / 85f, 1f);

	public static Color ForrestGreen => new Color(7f / 85f, 0.2666667f, 0.02352941f, 1f);

	public static Color DarkLavender => new Color(0.5215687f, 0.4039216f, 0.5960785f, 1f);

	public static Color DarkViolet => new Color(0.2039216f, 0.003921569f, 0.2470588f, 1f);

	public static Color PurpleBlue => new Color(33f / 85f, 0.1764706f, 0.9137255f, 1f);

	public static Color DarkCyan => new Color(0.03921569f, 0.5333334f, 46f / 85f, 1f);

	public static Color OliveDrab => new Color(0.4352941f, 0.4627451f, 0.1960784f, 1f);

	public static Color Pinkish => new Color(0.8313726f, 0.4156863f, 0.4941176f, 1f);

	public static Color Cobalt => new Color(0.1176471f, 0.282353f, 0.5607843f, 1f);

	public static Color NeonPurple => new Color(0.7372549f, 0.07450981f, 0.9960784f, 1f);

	public static Color LightTurquoise => new Color(0.4941176f, 0.9568627f, 0.8f, 1f);

	public static Color AppleGreen => new Color(0.4627451f, 41f / 51f, 0.1490196f, 1f);

	public static Color DullGreen => new Color(0.454902f, 0.6509804f, 0.3843137f, 1f);

	public static Color Wine => new Color(0.5019608f, 0.003921569f, 0.2470588f, 1f);

	public static Color PowderBlue => new Color(0.6941177f, 0.8196079f, 84f / 85f, 1f);

	public static Color OffWhite => new Color(1f, 1f, 0.8941177f, 1f);

	public static Color ElectricBlue => new Color(0.02352941f, 0.3215686f, 1f, 1f);

	public static Color DarkTurquoise => new Color(0.01568628f, 0.3607843f, 0.3529412f, 1f);

	public static Color BluePurple => new Color(0.3411765f, 0.1607843f, 0.8078431f, 1f);

	public static Color Azure => new Color(0.02352941f, 0.6039216f, 81f / 85f, 1f);

	public static Color BrightRed => new Color(1f, 0f, 0.05098039f, 1f);

	public static Color PinkishRed => new Color((float)Math.E * 105f / 302f, 0.04705882f, 0.2705882f, 1f);

	public static Color CornflowerBlue => new Color(0.3176471f, 0.4392157f, 0.8431373f, 1f);

	public static Color LightOlive => new Color(0.6745098f, 0.7490196f, 0.4117647f, 1f);

	public static Color Grape => new Color(0.4235294f, 0.2039216f, 0.3803922f, 1f);

	public static Color GreyishBlue => new Color(0.3686275f, 43f / 85f, 0.6156863f, 1f);

	public static Color PurplishBlue => new Color(32f / 85f, 0.1176471f, 83f / 85f, 1f);

	public static Color YellowishGreen => new Color(0.6901961f, 13f / 15f, 0.08627451f, 1f);

	public static Color GreenishYellow => new Color(41f / 51f, 0.9921569f, 0.007843138f, 1f);

	public static Color MediumBlue => new Color(0.172549f, 0.4352941f, 0.7333333f, 1f);

	public static Color DustyRose => new Color(64f / 85f, 23f / 51f, 0.4784314f, 1f);

	public static Color LightViolet => new Color(0.8392157f, 0.7058824f, 84f / 85f, 1f);

	public static Color MidnightBlue => new Color(0.007843138f, 0f, 0.2078431f, 1f);

	public static Color BluishPurple => new Color(0.4392157f, 0.2313726f, 0.9058824f, 1f);

	public static Color RedOrange => new Color(0.9921569f, 0.2352941f, 0.02352941f, 1f);

	public static Color DarkMagenta => new Color(0.5882353f, 0f, 0.3372549f, 1f);

	public static Color Greenish => new Color(0.2509804f, 0.6392157f, 0.4078431f, 1f);

	public static Color OceanBlue => new Color(0.01176471f, 0.4431373f, 52f / 85f, 1f);

	public static Color Coral => new Color(84f / 85f, 0.3529412f, 16f / 51f, 1f);

	public static Color Cream => new Color(1f, 1f, 0.7607843f, 1f);

	public static Color ReddishBrown => new Color(0.4980392f, 0.1686275f, 0.03921569f, 1f);

	public static Color BurntSienna => new Color(0.6901961f, 0.3058824f, 1f / 17f, 1f);

	public static Color Brick => new Color(32f / 51f, 0.2117647f, 0.1372549f, 1f);

	public static Color Sage => new Color(0.5294118f, 0.682353f, 23f / 51f, 1f);

	public static Color GreyGreen => new Color(0.4705882f, 0.6078432f, 23f / 51f, 1f);

	public static Color White => new Color(1f, 1f, 1f, 1f);

	public static Color RobinSEggBlue => new Color(0.5960785f, 0.9372549f, 83f / 85f, 1f);

	public static Color MossGreen => new Color(0.3960784f, 0.5450981f, 0.2196078f, 1f);

	public static Color SteelBlue => new Color(0.3529412f, 0.4901961f, 0.6039216f, 1f);

	public static Color Eggplant => new Color(0.2196078f, 0.03137255f, 0.2078431f, 1f);

	public static Color LightYellow => new Color(1f, 0.9960784f, 0.4784314f, 1f);

	public static Color LeafGreen => new Color(0.3607843f, 0.6627451f, 0.01568628f, 1f);

	public static Color LightGrey => new Color(0.8470588f, 44f / 51f, 0.8392157f, 1f);

	public static Color Puke => new Color(0.6470588f, 0.6470588f, 0.007843138f, 1f);

	public static Color PinkishPurple => new Color(0.8392157f, 0.282353f, 0.8431373f, 1f);

	public static Color SeaBlue => new Color(0.01568628f, 0.454902f, 0.5843138f, 1f);

	public static Color PalePurple => new Color(61f / 85f, 48f / 85f, 0.8313726f, 1f);

	public static Color SlateBlue => new Color(0.3568628f, 0.4862745f, 0.6f, 1f);

	public static Color BlueGrey => new Color(32f / 85f, 0.4862745f, 0.5568628f, 1f);

	public static Color HunterGreen => new Color(0.04313726f, 0.2509804f, 0.03137255f, 1f);

	public static Color Fuchsia => new Color(0.9294118f, 0.05098039f, 0.8509804f, 1f);

	public static Color Crimson => new Color(0.5490196f, 0f, 1f / 17f, 1f);

	public static Color PaleYellow => new Color(1f, 1f, 44f / 85f, 1f);

	public static Color Ochre => new Color(0.7490196f, 48f / 85f, 0.01960784f, 1f);

	public static Color MustardYellow => new Color(0.8235294f, 63f / 85f, 0.03921569f, 1f);

	public static Color LightRed => new Color(1f, 0.2784314f, 0.2980392f, 1f);

	public static Color Cerulean => new Color(0.01568628f, 0.5215687f, 0.8196079f, 1f);

	public static Color PalePink => new Color(1f, 69f / 85f, 44f / 51f, 1f);

	public static Color DeepBlue => new Color(0.01568628f, 0.007843138f, 23f / 51f, 1f);

	public static Color Rust => new Color(0.6588235f, 0.2352941f, 3f / 85f, 1f);

	public static Color LightTeal => new Color(48f / 85f, 0.8941177f, 0.7568628f, 1f);

	public static Color Slate => new Color(0.3176471f, 0.3960784f, 0.4470588f, 1f);

	public static Color Goldenrod => new Color(0.9803922f, 0.7607843f, 0.01960784f, 1f);

	public static Color DarkYellow => new Color(71f / 85f, 0.7137255f, 0.03921569f, 1f);

	public static Color DarkGrey => new Color(0.2117647f, 0.2156863f, 0.2156863f, 1f);

	public static Color ArmyGreen => new Color(0.2941177f, 31f / 85f, 0.08627451f, 1f);

	public static Color GreyBlue => new Color(0.4196078f, 0.5450981f, 0.6431373f, 1f);

	public static Color Seafoam => new Color(0.5019608f, 83f / 85f, 0.6784314f, 1f);

	public static Color Puce => new Color(0.6470588f, 0.4941176f, 0.3215686f, 1f);

	public static Color SpringGreen => new Color(0.6627451f, 83f / 85f, 0.4431373f, 1f);

	public static Color DarkOrange => new Color(66f / 85f, 0.3176471f, 0.007843138f, 1f);

	public static Color Sand => new Color(0.8862745f, 0.7921569f, 0.4627451f, 1f);

	public static Color PastelGreen => new Color(0.6901961f, 1f, 0.6156863f, 1f);

	public static Color Mint => new Color(0.6235294f, 0.9960784f, 0.6901961f, 1f);

	public static Color LightOrange => new Color(0.9921569f, 2f / 3f, 0.282353f, 1f);

	public static Color BrightPink => new Color(0.9960784f, 0.003921569f, 0.6941177f, 1f);

	public static Color Chartreuse => new Color(0.7568628f, 0.972549f, 0.03921569f, 1f);

	public static Color DeepPurple => new Color(0.2117647f, 0.003921569f, 0.2470588f, 1f);

	public static Color DarkBrown => new Color(0.2039216f, 0.1098039f, 0.007843138f, 1f);

	public static Color Taupe => new Color(37f / 51f, 0.6352941f, 43f / 85f, 1f);

	public static Color PeaGreen => new Color(0.5568628f, 0.6705883f, 6f / 85f, 1f);

	public static Color PukeGreen => new Color(0.6039216f, 0.682353f, 0.02745098f, 1f);

	public static Color KellyGreen => new Color(0.007843138f, 0.6705883f, 0.1803922f, 1f);

	public static Color SeafoamGreen => new Color(0.4784314f, 83f / 85f, 0.6705883f, 1f);

	public static Color BlueGreen => new Color(0.07450981f, 0.4941176f, 0.427451f, 1f);

	public static Color Khaki => new Color(2f / 3f, 0.6509804f, 0.3843137f, 1f);

	public static Color Burgundy => new Color(0.3803922f, 0f, 0.1372549f, 1f);

	public static Color DarkTeal => new Color(0.003921569f, 0.3019608f, 0.3058824f, 1f);

	public static Color BrickRed => new Color(0.5607843f, 0.07843138f, 0.007843138f, 1f);

	public static Color RoyalPurple => new Color(0.2941177f, 0f, 0.4313726f, 1f);

	public static Color Plum => new Color(0.345098f, 1f / 17f, 0.254902f, 1f);

	public static Color MintGreen => new Color(0.5607843f, 1f, 0.6235294f, 1f);

	public static Color Gold => new Color(0.8588235f, 0.7058824f, 0.04705882f, 1f);

	public static Color BabyBlue => new Color(0.6352941f, 69f / 85f, 0.9960784f, 1f);

	public static Color YellowGreen => new Color(64f / 85f, 0.9843137f, 0.1764706f, 1f);

	public static Color BrightPurple => new Color(0.7450981f, 0.01176471f, 0.9921569f, 1f);

	public static Color DarkRed => new Color(44f / 85f, 0f, 0f, 1f);

	public static Color PaleBlue => new Color(0.8156863f, 0.9960784f, 0.9960784f, 1f);

	public static Color GrassGreen => new Color(0.2470588f, 0.6078432f, 0.04313726f, 1f);

	public static Color Navy => new Color(0.003921569f, 7f / 85f, 0.2431373f, 1f);

	public static Color Aquamarine => new Color(0.01568628f, 0.8470588f, 0.6980392f, 1f);

	public static Color BurntOrange => new Color(64f / 85f, 0.3058824f, 0.003921569f, 1f);

	public static Color NeonGreen => new Color(0.04705882f, 1f, 0.04705882f, 1f);

	public static Color BrightBlue => new Color(0.003921569f, 0.3960784f, 84f / 85f, 1f);

	public static Color Rose => new Color(69f / 85f, 0.3843137f, 0.4588235f, 1f);

	public static Color LightPink => new Color(1f, 0.8196079f, 0.8745098f, 1f);

	public static Color Mustard => new Color(0.8078431f, 0.7019608f, 0.003921569f, 1f);

	public static Color Indigo => new Color(0.2196078f, 0.007843138f, 0.509804f, 1f);

	public static Color Lime => new Color(2f / 3f, 1f, 0.1960784f, 1f);

	public static Color SeaGreen => new Color(0.3254902f, 84f / 85f, 0.6313726f, 1f);

	public static Color Periwinkle => new Color(0.5568628f, 0.509804f, 0.9960784f, 1f);

	public static Color DarkPink => new Color(0.7960784f, 0.254902f, 0.4196078f, 1f);

	public static Color OliveGreen => new Color(0.4039216f, 0.4784314f, 0.01568628f, 1f);

	public static Color Peach => new Color(1f, 0.6901961f, 0.4862745f, 1f);

	public static Color PaleGreen => new Color(0.7803922f, 0.9921569f, 0.7098039f, 1f);

	public static Color LightBrown => new Color(0.6784314f, 43f / 85f, 16f / 51f, 1f);

	public static Color HotPink => new Color(1f, 0.007843138f, 47f / 85f, 1f);

	public static Color Black => new Color(0f, 0f, 0f, 1f);

	public static Color Lilac => new Color(0.8078431f, 0.6352941f, 0.9921569f, 1f);

	public static Color NavyBlue => new Color(0f, 1f / 15f, 0.2745098f, 1f);

	public static Color RoyalBlue => new Color(0.01960784f, 0.01568628f, 2f / 3f, 1f);

	public static Color Beige => new Color(46f / 51f, 0.854902f, 0.6509804f, 1f);

	public static Color Salmon => new Color(1f, 0.4745098f, 0.4235294f, 1f);

	public static Color Olive => new Color(0.4313726f, 0.4588235f, 0.05490196f, 1f);

	public static Color Maroon => new Color(0.3960784f, 0f, 0.1294118f, 1f);

	public static Color BrightGreen => new Color(0.003921569f, 1f, 0.02745098f, 1f);

	public static Color DarkPurple => new Color(0.2078431f, 0.02352941f, 0.2431373f, 1f);

	public static Color Mauve => new Color(0.682353f, 0.4431373f, 43f / 85f, 1f);

	public static Color ForestGreen => new Color(0.02352941f, 0.2784314f, 0.04705882f, 1f);

	public static Color Aqua => new Color(0.07450981f, 0.9176471f, 67f / 85f, 1f);

	public static Color Cyan => new Color(0f, 1f, 1f, 1f);

	public static Color Tan => new Color(0.8196079f, 0.6980392f, 0.4352941f, 1f);

	public static Color DarkBlue => new Color(0f, 0.01176471f, 0.3568628f, 1f);

	public static Color Lavender => new Color(0.7803922f, 0.6235294f, 0.9372549f, 1f);

	public static Color Turquoise => new Color(0.02352941f, 0.7607843f, 0.6745098f, 1f);

	public static Color DarkGreen => new Color(0.01176471f, 0.2078431f, 0f, 1f);

	public static Color Violet => new Color(0.6039216f, 0.05490196f, 0.9176471f, 1f);

	public static Color LightPurple => new Color(0.7490196f, 0.4666667f, 82f / 85f, 1f);

	public static Color LimeGreen => new Color(0.5372549f, 0.9960784f, 0.01960784f, 1f);

	public static Color Grey => new Color(0.572549f, 0.5843138f, 29f / 51f, 1f);

	public static Color SkyBlue => new Color(0.4588235f, 0.7333333f, 0.9921569f, 1f);

	public static Color Yellow => new Color(1f, 1f, 0.07843138f, 1f);

	public static Color Magenta => new Color(0.7607843f, 0f, 0.4705882f, 1f);

	public static Color LightGreen => new Color(0.5882353f, 83f / 85f, 0.4823529f, 1f);

	public static Color Orange => new Color(83f / 85f, 23f / 51f, 0.02352941f, 1f);

	public static Color Teal => new Color(0.007843138f, 49f / 85f, 0.5254902f, 1f);

	public static Color LightBlue => new Color(0.5843138f, 0.8156863f, 84f / 85f, 1f);

	public static Color Red => new Color(0.8980392f, 0f, 0f, 1f);

	public static Color Brown => new Color(0.3960784f, 0.2156863f, 0f, 1f);

	public static Color Pink => new Color(1f, 43f / 85f, 64f / 85f, 1f);

	public static Color Blue => new Color(0.01176471f, 0.2627451f, 0.8745098f, 1f);

	public static Color Green => new Color(7f / 85f, 0.6901961f, 0.1019608f, 1f);

	public static Color Purple => new Color(0.4941176f, 0.1176471f, 52f / 85f, 1f);

	public static Color smethod_0(this Color c, float a)
	{
		c.a = a;
		return c;
	}

	public static Color32 smethod_1(this Color32 c, byte a)
	{
		c.a = a;
		return c;
	}
}
