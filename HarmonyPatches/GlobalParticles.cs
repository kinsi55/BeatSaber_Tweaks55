using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	static class GlobalParticles {
		public static void SetEnabledState() {
			if(Config.Instance == null) {
				SetEnabledState(true);
				return;
			}

			SetEnabledState(!Config.Instance.disableGlobalParticles);
		}

		public static void SetEnabledState(bool enabled) {
			foreach(var x in Resources.FindObjectsOfTypeAll<ParticleSystem>())
				if(x.name == "DustPS") x.gameObject.SetActive(enabled);
		}
	}
}
