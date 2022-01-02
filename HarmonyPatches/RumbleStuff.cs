using HarmonyLib;
using Libraries.HM.HMLib.VR;
using System;
using System.Reflection;
using Tweaks55.Util;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class RumbleStuff {
		public static readonly HapticPresetSO _ourPreset = ScriptableObject.CreateInstance<HapticPresetSO>();

		[HarmonyPriority(int.MinValue)]
		static bool Prefix(ref HapticPresetSO ____rumblePreset) {
			if(!Config.Instance.enableCustomRumble)
				return true;

			if(_ourPreset._duration == 0f || _ourPreset._strength == 0f)
				return false;

			____rumblePreset = _ourPreset;

			return true;
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(NoteCutHapticEffect), nameof(NoteCutHapticEffect.HitNote));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
