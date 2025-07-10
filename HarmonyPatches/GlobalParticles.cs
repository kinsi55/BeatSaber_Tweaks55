using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	static class GlobalParticles {
		static bool lastKnownState = true;

		public static void SetEnabledState() {
			if(Config.Instance == null) {
				SetEnabledState(true);
				return;
			}

			SetEnabledState(!Config.Instance.disableGlobalParticles);
		}

		public static void SetEnabledState(bool enabled) {
			if(!lastKnownState || !enabled) {
				foreach(var x in Resources.FindObjectsOfTypeAll<ParticleSystem>())
					if(x.name == "DustPS" || x.name == "DustBritney") x.gameObject.SetActive(enabled);
			}

			lastKnownState = enabled;
		}
	}
}
