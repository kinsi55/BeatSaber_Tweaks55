using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(DefaultScenesTransitionsFromInit), nameof(DefaultScenesTransitionsFromInit.TransitionToNextScene))]
	class PatchHealthWarning {
		[HarmonyPriority(int.MinValue)]
		static void Prefix(ref bool goStraightToMenu) {
			if(Configuration.PluginConfig.Instance.disableHealthWarning)
				goStraightToMenu = true;
		}
	}
}
