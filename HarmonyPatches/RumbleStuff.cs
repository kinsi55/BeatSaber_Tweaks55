using HarmonyLib;
using Libraries.HM.HMLib.VR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(NoteCutHapticEffect), MethodType.Constructor)]
	class RumbleStuff {
		[HarmonyPriority(int.MaxValue)]
		static void Postfix(HapticPresetSO ____rumblePreset) {
			if(____rumblePreset == null || !Configuration.PluginConfig.Instance.enableCustomRumble)
				return;

			____rumblePreset._duration = Configuration.PluginConfig.Instance.cutRumbleDuration;
			____rumblePreset._strength = Configuration.PluginConfig.Instance.cutRumbleStrength;
		}
	}
}
