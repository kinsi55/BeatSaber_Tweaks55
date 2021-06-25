using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(BombNoteController), nameof(BombNoteController.Init))]
	static class BombColor {
		[HarmonyPriority(int.MaxValue)]
		static void Prefix(BombNoteController __instance) {
			if(Configuration.PluginConfig.Instance.bombColor != Color.black)
				__instance.transform.GetChild(0)?.GetComponent<Renderer>()?.material?.SetColor("_SimpleColor", Configuration.PluginConfig.Instance.bombColor);
		}
	}
}
