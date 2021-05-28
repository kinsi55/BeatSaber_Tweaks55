using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using UnityEngine.SceneManagement;
using UnityEngine;
using IPALogger = IPA.Logging.Logger;
using System.Reflection;
using HarmonyLib;
using Tweaks55.HarmonyPatches;
using BeatSaberMarkupLanguage.Settings;

//#define OCULUS

namespace Tweaks55 {

	[Plugin(RuntimeOptions.SingleStartInit)]
	public class Plugin {
		internal static Plugin Instance { get; private set; }
		internal static IPALogger Log { get; private set; }

		public static Harmony harmony;

		[Init]
		/// <summary>
		/// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
		/// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
		/// Only use [Init] with one Constructor.
		/// </summary>Console.WriteLine
		public void Init(Config conf, IPALogger logger) {
			Instance = this;
			Log = logger;

			Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();

			BSMLSettings.instance.AddSettingsMenu("Tweaks55", "Tweaks55.Views.settings.bsml", Configuration.PluginConfig.Instance);

			harmony = new Harmony("Kinsi55.BeatSaber.Tweaks55");
			harmony.PatchAll(Assembly.GetExecutingAssembly());

			SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
		}

		private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1) {
			if(arg1.name == "MainMenu" || arg1.name == "GameCore")
				GlobalParticles.SetEnabledState();
		}
	}
}
