﻿using HarmonyLib;
using System;
using System.Reflection;
using Tweaks55.Util;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class PatchHealthWarning {
		[HarmonyPriority(int.MinValue)]
		static void Prefix(ref bool goStraightToMenu) {
			if(Config.Instance.disableHealthWarning)
				goStraightToMenu = true;
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(DefaultScenesTransitionsFromInit), nameof(DefaultScenesTransitionsFromInit.TransitionToNextScene));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
