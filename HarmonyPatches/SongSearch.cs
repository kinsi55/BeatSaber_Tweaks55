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
	static class BasegameSearchFulltext {
		static bool isActive = true;

		[HarmonyPriority(int.MaxValue)]
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il) {
			if(!ILUtil.CheckIL(instructions, new Dictionary<int, OpCode>() {
				{90, OpCodes.Brtrue_S},
				{93, OpCodes.Ldarg_0},
				{96, OpCodes.Callvirt},
				{104, OpCodes.Stfld}
			})) {
				Plugin.Log.Warn("basegameFulltextSearch: BeatmapLevelFilterModel.FilerBeatmapLevelPackCollectionAsync was not what I expected, not patching it");
				isActive = false;
				return instructions;
			}

			var conditional = new CodeInstruction[] {
				new CodeInstruction(OpCodes.Ldc_I4_1),
				new CodeInstruction(OpCodes.Newarr, typeof(string)),
				new CodeInstruction(OpCodes.Dup),
				new CodeInstruction(OpCodes.Ldc_I4_0)
			}.ToList();

			conditional.AddRange(instructions.ToList().GetRange(93, 4).Select(x => new CodeInstruction(x.opcode, x.operand)));

			conditional.AddRange(new CodeInstruction[] {
				new CodeInstruction(OpCodes.Stelem_Ref),

				new CodeInstruction(OpCodes.Br, ILUtil.GetLabelForIndex(104, instructions, il))
			});

			ILUtil.InsertConditionalIL(93, ref instructions, il, AccessTools.Method(typeof(BasegameSearchFulltext), nameof(__EnableFulltextSearch)), conditional);

			instructions.ElementAt(90).operand = ILUtil.GetLabelForIndex(93, instructions, il);

			return instructions;
		}
		static bool __EnableFulltextSearch() => Configuration.PluginConfig.Instance.basegameFulltextSearch;

		[HarmonyTargetMethod]
		public static IEnumerable<MethodBase> TargetMethods() {
			yield return ILUtil.GetIterator<BeatmapLevelFilterModel>("FilerBeatmapLevelPackCollectionAsync");
		}
	}
}
