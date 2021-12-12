using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using System;
using System.Reflection;
using Tweaks55.HarmonyPatches;
using Tweaks55.UI;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace Tweaks55 {

	[Plugin(RuntimeOptions.DynamicInit)]
	public class Plugin {
		internal static Plugin Instance { get; private set; }
		internal static IPALogger Log { get; private set; }

		internal static bool enabled { get; private set; } = true;

		public static Harmony harmony;

		[Init]
		/// <summary>
		/// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
		/// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
		/// Only use [Init] with one Constructor.
		/// </summary>Console.WriteLine
		public void Init(IPA.Config.Config conf, IPALogger logger) {
			Instance = this;
			Log = logger;

			Config.Instance = conf.Generated<Config>();

			harmony = new Harmony("Kinsi55.BeatSaber.Tweaks55");
		}

		private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1) {
			if(arg1.name == "MainMenu" || arg1.name == "GameCore")
				GlobalParticles.SetEnabledState();
		}

		public static Exception PatchFailed(string name, Exception ex) {
			if(ex != null) {
				Plugin.Log.Warn($"{name} Tweak failed to setup! Ignoring");
				Plugin.Log.Debug(ex);
			}
			return null;
		}

		[OnEnable]
		public void OnEnable() {
			enabled = true;
			harmony.PatchAll(Assembly.GetExecutingAssembly());

			Config.Instance.Changed();
			TweaksFlowCoordinator.Initialize();

			SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
		}

		/// <summary>
		/// Called when the plugin is disabled and on Beat Saber quit. It is important to clean up any Harmony patches, GameObjects, and Monobehaviours here.
		/// The game should be left in a state as if the plugin was never started.
		/// Methods marked [OnDisable] must return void or Task.
		/// </summary>
		[OnDisable]
		public void OnDisable() {
			enabled = false;

			harmony.UnpatchSelf();
			TweaksFlowCoordinator.Deinit();
			GlobalParticles.SetEnabledState(true);
			MenuLightColor.SetColor(new UnityEngine.Color(0, 0, 0, 0));
			StaticlightsToggle.Setup(false);

			SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
		}
	}
}
