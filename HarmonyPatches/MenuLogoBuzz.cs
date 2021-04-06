using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {

    [HarmonyPatch(typeof(FlickeringNeonSign), nameof(FlickeringNeonSign.SetOn))]
    class FlickeringNeonSignPatch {
		[HarmonyPriority(int.MaxValue)]
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il) {
			if(instructions.ElementAt(52).opcode != OpCodes.Ldarg_0) {
				Plugin.Log.Warn("muteMenuLogoBuzz: FlickeringNeonSign.SetOn:52 was not what I expected, not patching it");
			} else {
				ILUtil.DynamicPrefix(52, ref instructions, il, AccessTools.Method(typeof(FlickeringNeonSignPatch), nameof(__DisableBuzz)));
			}
			return instructions;
		}

		static bool __DisableBuzz() => !Configuration.PluginConfig.Instance.muteMenuLogoBuzz;
	}
}
