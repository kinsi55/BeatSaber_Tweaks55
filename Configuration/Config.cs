
using IPA.Config.Stores;
using System;
using System.Runtime.CompilerServices;
using Tweaks55.HarmonyPatches;
using UnityEngine;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace Tweaks55 {
	internal class Config {
		public static Config Instance;

		public Color bombColor = BombColor.defaultColor;
		public Color wallBorderColor = WallOutline.defaultColor;
		public bool disableDebris = false;
		public bool disableSliceScore = false;
		public bool transparentWalls = false;

		public bool disableComboBreakEffect = false;
		public bool disableWallRumbleAndParticles = false;
		public bool disableSaberClash = false;
		public bool disableBurnMarks = false;
		public bool disableCampaignFireworks = false;


		public bool disableCutParticles = false;
		public virtual bool disableGlobalParticles { get; set; } = false;
		public bool disableBombExplosion = false;


		public bool disableHealthWarning = false;
		public bool keepGameSettingsOnCancel = false;
		public float scrollSpeedMultiplier = 1f;

		public bool disableFakeWallBloom = false;
		public bool disableBeatLines = false;
		public bool disableCameraNoise = false;


		public virtual bool staticLightsToggle { get; set; } = false;
		public bool disableRawScore = false;
		public virtual Color menuLightColor { get; set; } = new Color(0, 0, 0, 0);







		public bool enableCustomRumble = false;
		public float cutRumbleStrength = 1f;
		public float rumbleChainElementsStrength = 1f;
		public float rumbleArcsStrength = 1f;

		/// <summary>
		/// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
		/// </summary>
		public virtual void Changed() => ApplyValues();

		public void ApplyValues() {
			if(!Plugin.enabled)
				return;

			// Migration from old default (<0.3.7). TODO: Remove later
			if(bombColor == Color.black)
				bombColor = BombColor.defaultColor;

			try {
				MenuLightColor.SetColor(menuLightColor);
			} catch(Exception ex) {
				Plugin.Log.Warn(string.Format("MenuLightColor.SetColor failed: {0}", ex));
			}

			try {
				StaticlightsToggle.Setup(staticLightsToggle);
			} catch(Exception ex) {
				Plugin.Log.Warn(string.Format("StaticlightsToggle.Setup failed: {0}", ex));
			}

			try {
				GlobalParticles.SetEnabledState(!disableGlobalParticles);
			} catch(Exception ex) {
				Plugin.Log.Warn(string.Format("GlobalParticles.SetEnabledState failed: {0}", ex));
			}

			WallOutline.realBorderColor = wallBorderColor;
			WallOutline.realBorderColor.a = Mathf.Max(wallBorderColor.r, wallBorderColor.g, wallBorderColor.b) / 5f;

			WallOutline.fakeBorderColor = wallBorderColor;
			WallOutline.fakeBorderColor.a = 0.6f + (WallOutline.realBorderColor.a * 2f);

			WallOutline.enabled = wallBorderColor != WallOutline.defaultColor;

			Rumblez();
			void Rumblez() {
				CutRumble.normalPreset._duration = Math.Min(0.2f, CutRumble.DURATION_NORMAL * cutRumbleStrength);
				CutRumble.normalPreset._strength = CutRumble.STRENGTH_NORMAL * Math.Min(1, (cutRumbleStrength * 1.2f));

				CutRumble.weakPreset._duration = Math.Min(0.2f, CutRumble.DURATION_WEAK * rumbleChainElementsStrength);
				CutRumble.weakPreset._strength = CutRumble.STRENGTH_WEAK * Math.Min(1, (rumbleChainElementsStrength * 1.2f));

				ArcRumble.preset._duration = Math.Min(0.05f, ArcRumble.DURATION_NORMAL * rumbleArcsStrength);
				ArcRumble.preset._strength = ArcRumble.STRENGTH_NORMAL * Math.Min(1, rumbleArcsStrength * 1.2f);
			}
		}

		public  string PercentageFormatter(float x) => x.ToString("0%");
	}
}
