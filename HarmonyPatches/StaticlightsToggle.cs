using HarmonyLib;
using HMUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(PlayerSettingsPanelController), nameof(PlayerSettingsPanelController.SetLayout))]
	static class StaticlightsToggle {
		static PlayerSettingsPanelController instance;
		static EnvironmentEffectsFilterPresetDropdown toggle1;
		static EnvironmentEffectsFilterPresetDropdown toggle2;

		static GameObject replaceLabel;
		static ToggleWithCallbacks replaceToggle;

		[HarmonyPriority(int.MinValue)]
		static void Postfix(
			PlayerSettingsPanelController __instance,
			EnvironmentEffectsFilterPresetDropdown ____environmentEffectsFilterDefaultPresetDropdown,
			EnvironmentEffectsFilterPresetDropdown ____environmentEffectsFilterExpertPlusPresetDropdown
		) {
			if(__instance.transform.parent.name == "PlayerSettingsViewController" || instance != null)
				return;

			instance = __instance;
			toggle1 = ____environmentEffectsFilterDefaultPresetDropdown;
			toggle2 = ____environmentEffectsFilterExpertPlusPresetDropdown;

			Setup(Configuration.PluginConfig.Instance.staticLightsToggle);
		}

		public static void ToggleEffectState(bool setStatic) {
			var theEffect = setStatic ? EnvironmentEffectsFilterPreset.NoEffects : EnvironmentEffectsFilterPreset.AllEffects;

			toggle1.SelectCellWithLightReductionAmount(theEffect);
			toggle2.SelectCellWithLightReductionAmount(theEffect);

			instance.SetIsDirty();
		}

		static IEnumerator InitEffectState(bool setStatic) {
			yield return new WaitForSeconds(.01f);

			ToggleEffectState(setStatic);
		}

		public static void Setup(bool enable) {
			if(instance == null)
				return;

			// Here we go... Get the playeroptions container
			var container = instance.transform.Find("ViewPort/Content/CommonSection");

			container.GetComponent<VerticalLayoutGroup>().enabled = true;

			GameObject disableNext(EnvironmentEffectsFilterPresetDropdown e, bool active) {
				var s = e.transform.parent.GetSiblingIndex();

				var l = e.transform.parent.parent.GetChild(s + 1).gameObject;

				if(l.name == "-")
					l.SetActive(active);

				return e.transform.parent.gameObject;
			}

			disableNext(toggle2, !enable).SetActive(!enable);

			var existingTableSettingRow = toggle1.transform.parent;
			var ogLabel = existingTableSettingRow.Find("Label").gameObject;

			if(replaceLabel == null) {
				replaceLabel = GameObject.Instantiate(ogLabel, existingTableSettingRow);
				GameObject.Destroy(replaceLabel.GetComponents<MonoBehaviour>().First(l => l.GetType().Name == "LocalizedTextMeshProUGUI"));
				replaceLabel.GetComponent<CurvedTextMeshPro>().text = "Static Lights";
			}

			if(replaceToggle == null) {
				replaceToggle = GameObject.Instantiate(container.GetComponentInChildren<ToggleWithCallbacks>(), existingTableSettingRow);

				replaceToggle.onValueChanged.RemoveAllListeners();

				replaceToggle.onValueChanged.AddListener(ToggleEffectState);
			}


			ogLabel.SetActive(!enable);
			existingTableSettingRow.Find("SimpleTextDropDown").gameObject.SetActive(!enable);
			replaceLabel.SetActive(enable);
			replaceToggle.gameObject.SetActive(enable);

			if(enable) {
				var targetState = instance.playerSpecificSettings.environmentEffectsFilterDefaultPreset == EnvironmentEffectsFilterPreset.NoEffects &&
				instance.playerSpecificSettings.environmentEffectsFilterExpertPlusPreset == EnvironmentEffectsFilterPreset.NoEffects;

				replaceToggle.isOn = targetState;
				SharedCoroutineStarter.instance.StartCoroutine(InitEffectState(targetState));
			}
		}
	}
}
