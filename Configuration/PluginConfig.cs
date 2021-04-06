
using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using Tweaks55.HarmonyPatches;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace Tweaks55.Configuration {

	internal class PluginConfig {
		public static PluginConfig Instance { get; set; }
		public virtual bool disableWallRumbleAndParticles { get; set; } = false;
		public virtual bool disableSaberClash { get; set; } = false;
		public virtual bool disableCutParticles { get; set; } = false;
		public virtual bool disableGlobalParticles { get; set; } = false;
		public virtual bool disableDebris { get; set; } = false;


		public virtual bool disableIngameMapEditor { get; set; } = false;
		public virtual bool muteMenuLogoBuzz { get; set; } = false;
		public virtual bool keepGameSettingsOnCancel { get; set; } = false;
		public virtual bool disableHealthWarning { get; set; } = false;


		public virtual bool disableFakeWallBloom { get; set; } = false;
		public virtual bool staticLightsToggle { get; set; } = false;
		public virtual bool wallsInZenMode { get; set; } = false;


		public virtual bool basegameFulltextSearch { get; set; } = false;


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
			// Do stuff when the config is changed.
			StaticlightsToggle.Setup(staticLightsToggle);
			GlobalParticles.SetEnabledState(!disableGlobalParticles);
		}

		/// <summary>
		/// Call this to have BSIPA copy the values from <paramref name="other"/> into this config.
		/// </summary>
		public virtual void CopyFrom(PluginConfig other) {
			// This instance's members populated from other
		}
	}
}
