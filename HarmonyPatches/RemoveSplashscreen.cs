using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	class YeetSplash {
		[HarmonyPriority(int.MaxValue)]
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
			//for(int i = 49; i < 75; i++)
			//	instructions.ElementAt(i).opcode = OpCodes.Nop;

			return instructions;
		}

		[HarmonyTargetMethod]
		public static IEnumerable<MethodBase> TargetMethods() {
			yield return ILUtil.GetIterator<AppInit>("StartCoroutine");
		}
	}
}
