﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(ComboUIController), nameof(ComboUIController.HandleComboBreakingEventHappened))]
	class ComboBreakEffect {
		[HarmonyPriority(int.MaxValue)]
		static void Postfix(Animator ____animator) {
			if(!Configuration.PluginConfig.Instance.disableComboBreakEffect)
				return;

			____animator.speed = 69420f;
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed("ComboBreakEffect", ex);
	}
}