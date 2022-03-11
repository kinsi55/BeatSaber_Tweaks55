
using IPA.Config.Stores;
using System;
using System.Runtime.CompilerServices;
using Tweaks55.HarmonyPatches;
using UnityEngine;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace Tweaks55 {
	internal class Config {
		public static Config Instance { get; set; }

		public Color bombColor = Color.black;
		public Color wallOutlineColor { get; set; } = Color.white;
		public bool disableDebris { get; set; } = false;
		public bool disableSliceScore { get; set; } = false;
		public bool transparentWalls { get; set; } = false;

		public bool disableComboBreakEffect { get; set; } = false;
		public bool disableWallRumbleAndParticles { get; set; } = false;
		public bool disableSaberClash { get; set; } = false;
		public bool disableBurnMarks { get; set; } = false;
		public bool disableCampaignFireworks { get; set; } = false;


		public bool disableCutParticles { get; set; } = false;
		public virtual bool disableGlobalParticles { get; set; } = false;
		public virtual bool disableBombExplosion { get; set; } = false;


		public bool disableHealthWarning { get; set; } = false;
		public bool basegameFulltextSearch { get; set; } = false;
		public bool keepGameSettingsOnCancel { get; set; } = false;
		public float scrollSpeedMultiplier { get; set; } = 1f;

		public bool disableFakeWallBloom { get; set; } = false;
		public bool disableBeatLines { get; set; } = false;


		public virtual bool staticLightsToggle { get; set; } = false;
		public bool disableRawScore { get; set; } = false;
		public virtual Color menuLightColor { get; set; } = new Color(0, 0, 0, 0);







		public bool enableCustomRumble { get; set; } = false;
		public virtual float cutRumbleStrength { get; set; } = 0.075f;
		public virtual float cutRumbleDuration { get; set; } = 0.08f;

		/// <summary>
		/// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
		/// </summary>
		public virtual void Changed() => ApplyValues();

		public void ApplyValues() {
			// Initially the default bomb color was 0;0;0;0, ths will correct that fault if it made its way into the config
			if(bombColor.a == 0f)
				bombColor.a = 1;

			RumbleStuff._ourPreset._duration = cutRumbleDuration;
			RumbleStuff._ourPreset._strength = cutRumbleStrength;

			if(!Plugin.enabled)
				return;

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
		}
	}
}
