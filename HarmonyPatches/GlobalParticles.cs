using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	static class GlobalParticles {
		public static void SetEnabledState() {
			if(Configuration.PluginConfig.Instance == null) {
				SetEnabledState(true);
				return;
			}

			SetEnabledState(!Configuration.PluginConfig.Instance.disableGlobalParticles);
		}

		public static void SetEnabledState(bool enabled) {
			foreach(var x in Resources.FindObjectsOfTypeAll<ParticleSystem>())
				if(x.name == "DustPS") x.gameObject.SetActive(enabled);
		}
	}
}
