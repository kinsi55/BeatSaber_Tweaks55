
using IPA.Config.Stores;
using System.Runtime.CompilerServices;
using Tweaks55.HarmonyPatches;
using UnityEngine;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace Tweaks55 {

	internal class Config {
		public static Config Instance { get; set; }
		public virtual bool disableWallRumbleAndParticles { get; set; } = false;
		public virtual bool disableSaberClash { get; set; } = false;
		public virtual bool disableCutParticles { get; set; } = false;
		public virtual bool disableGlobalParticles { get; set; } = false;
		public virtual bool disableDebris { get; set; } = false;
		public virtual bool disableSliceScore { get; set; } = false;


		public virtual bool disableIngameMapEditor { get; set; } = false;
		public virtual bool keepGameSettingsOnCancel { get; set; } = false;
		public virtual bool disableHealthWarning { get; set; } = false;



		public virtual bool disableFakeWallBloom { get; set; } = false;
		public virtual bool staticLightsToggle { get; set; } = false;
		public virtual bool disableBeatLines { get; set; } = false;
		public virtual bool disableComboBreakEffect { get; set; } = false;
		public virtual bool disableCampaignFireworks { get; set; } = false;
		public virtual bool disableBurnMarks { get; set; } = false;

		internal Color bombColor = Color.black;
		public virtual Color menuLightColor { get; set; } = new Color(0, 0, 0, 0);


		public virtual bool basegameFulltextSearch { get; set; } = false;

		public virtual float scrollSpeedMultiplier { get; set; } = 1f;



		public virtual bool enableCustomRumble { get; set; } = false;
		public virtual float cutRumbleStrength { get; set; } = 0.075f;
		public virtual float cutRumbleDuration { get; set; } = 0.08f;

		/// <summary>
		/// This is called whenever BSIPA reads the config from disk (including when file changes are detected).
		/// </summary>
		public virtual void OnReload() {
			// Do stuff after config is read from disk.
		}

		/// <summary>
		/// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
		/// </summary>
		public virtual void Changed() {
			if(!Plugin.enabled)
				return;

			// Do stuff when the config is changed.
			StaticlightsToggle.Setup(staticLightsToggle);
			GlobalParticles.SetEnabledState(!disableGlobalParticles);

			// Initially the default bomb color was 0;0;0;0, ths will correct that fault if it made its way into the config
			if(bombColor.a == 0f)
				bombColor.a = 1;

			MenuLightColor.SetColor(menuLightColor);
		}

		/// <summary>
		/// Call this to have BSIPA copy the values from <paramref name="other"/> into this config.
		/// </summary>
		public virtual void CopyFrom(Config other) {
			// This instance's members populated from other
		}
	}
}
