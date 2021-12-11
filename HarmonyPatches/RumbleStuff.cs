using HarmonyLib;
using Libraries.HM.HMLib.VR;
using System;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(NoteCutHapticEffect), nameof(NoteCutHapticEffect.HitNote))]
	static class RumbleStuff {
		static HapticPresetSO _ourPreset = ScriptableObject.CreateInstance<HapticPresetSO>();

		[HarmonyPriority(int.MinValue)]
		static bool Prefix(ref HapticPresetSO ____rumblePreset) {
			if(!Config.Instance.enableCustomRumble)
				return true;

			_ourPreset._duration = Config.Instance.cutRumbleDuration;
			_ourPreset._strength = Config.Instance.cutRumbleStrength;

			if(_ourPreset._duration == 0f || _ourPreset._strength == 0f)
				return false;

			____rumblePreset = _ourPreset;

			return true;
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed("RumbleStuff", ex);
	}
}
