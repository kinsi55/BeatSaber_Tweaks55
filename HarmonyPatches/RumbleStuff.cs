using HarmonyLib;
using Libraries.HM.HMLib.VR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(NoteCutHapticEffect), nameof(NoteCutHapticEffect.HitNote))]
	class RumbleStuff {
		static HapticPresetSO _ourPreset = ScriptableObject.CreateInstance<HapticPresetSO>();

		[HarmonyPriority(int.MinValue)]
		static bool Prefix(ref HapticPresetSO ____rumblePreset) {
			if(!Configuration.PluginConfig.Instance.enableCustomRumble)
				return true;

			_ourPreset._duration = Configuration.PluginConfig.Instance.cutRumbleDuration;
			_ourPreset._strength = Configuration.PluginConfig.Instance.cutRumbleStrength;

			if(_ourPreset._duration == 0f || _ourPreset._strength == 0f)
				return false;

			if(____rumblePreset == _ourPreset)
				return true;

			____rumblePreset = _ourPreset;

			return true;
		}
	}
}
