using HarmonyLib;
using System;
using System.Reflection;
using Tweaks55.Util;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	class ComboBreakEffect {
		[HarmonyPriority(int.MaxValue)]
		static void Postfix(Animator ____animator) {
			if(!Config.Instance.disableComboBreakEffect)
				return;

			____animator.speed = 69420f;
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(ComboUIController), nameof(ComboUIController.Start));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
