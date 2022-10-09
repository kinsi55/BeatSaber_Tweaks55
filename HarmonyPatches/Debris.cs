using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Tweaks55.Util;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class Debris {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Config.Instance.disableDebris;

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(NoteDebrisSpawner), nameof(NoteDebrisSpawner.SpawnDebris));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}

	// Game will pre-create 40 debris objects - If debris is disabled, we might as well not do that.
	[HarmonyPatch]
	static class DebrisPrefab {
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
			var x = new CodeMatcher(instructions);
			var m = new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(DebrisPrefab), nameof(NoInitialSizeIfDebrisDisabled)));

			x.MatchForward(true,
				new CodeMatch(OpCodes.Ldc_I4_S),
				new CodeMatch(x => (x.operand as MethodInfo)?.Name == "WithInitialSize")
			).Repeat(x => {
				x.InsertAndAdvance(m);
			});

			return x.InstructionEnumeration();
		}

		static int NoInitialSizeIfDebrisDisabled(int inSize) {
			if(Config.Instance.disableDebris)
				return 0;

			return inSize;
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(NoteDebrisPoolInstaller), "InstallBindings");
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
