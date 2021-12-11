using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(MissionObjectiveGameUIView), nameof(MissionObjectiveGameUIView.RefreshIcon))]
	static class CampaignFireworks {
		[HarmonyPriority(int.MinValue)]
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il) {
			if(instructions.ElementAt(41).opcode != OpCodes.Ldarg_0) {
				Plugin.Log.Warn("disableCampaignFireworks: MissionObjectiveGameUIView.RefreshIcon:IL41 was not what I expected, not patching it");
			} else {
				ILUtil.DynamicPrefix(41, ref instructions, il, AccessTools.Method(typeof(CampaignFireworks), nameof(__DisableCampaignFireworks)));
			}
			return instructions;
		}

		static bool __DisableCampaignFireworks() => !Config.Instance.disableCampaignFireworks;

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed("CampaignFireworks", ex);
	}
}
