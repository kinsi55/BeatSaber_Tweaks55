﻿using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.MenuButtons;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Tweaks55.HarmonyPatches;
using UnityEngine;

namespace Tweaks55.UI {
	class TweaksFlowCoordinator : FlowCoordinator {
		MAN view = null;

		public override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling) {
			if(firstActivation) {
				SetTitle("Tweaks55");
				showBackButton = true;

				if(view == null)
					view = BeatSaberUI.CreateViewController<MAN>();

				ProvideInitialViewControllers(view);
			}
		}

		public override void BackButtonWasPressed(ViewController topViewController) {
			BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this, null, ViewController.AnimationDirection.Horizontal);
			Config.Instance.Changed();
		}

		public void ShowFlow() {
			var _parentFlow = BeatSaberUI.MainFlowCoordinator.YoungestChildFlowCoordinatorOrSelf();

			BeatSaberUI.PresentFlowCoordinator(_parentFlow, this);
		}

		static TweaksFlowCoordinator flow = null;

		static MenuButton theButton;

		public static void Initialize() {

			MenuButtons.instance.RegisterButton(theButton ??= new MenuButton("Tweaks55", "A bunch of settings that really should just exist in basegame!", () => {
				if(flow == null)
					flow = BeatSaberUI.CreateFlowCoordinator<TweaksFlowCoordinator>();

				flow.ShowFlow();
			}, true));
		}

		public static void Deinit() {
			if(theButton != null)
				MenuButtons.instance.UnregisterButton(theButton);
		}
	}

	[HotReload(RelativePathToLayout = @"./settings.bsml")]
	[ViewDefinition("Tweaks55.UI.settings.bsml")]
	class MAN : BSMLAutomaticViewController {
		Config config = Config.Instance;

		static bool isAprilFirst = (DateTime.Now.Month == 4) && (DateTime.Now.Day == 1);
		static bool __true = true;

		void ClearBombColor() {
			bombColor = BombColor.defaultColor;
			NotifyPropertyChanged("bombColor");
		}
		void ClearMenuLightColor() {
			menuLightColor = new Color(0, 0, 0, 0);
			NotifyPropertyChanged("menuLightColor");
		}
		void ClearWallOutlineColor() {
			config.wallBorderColor = WallOutline.defaultColor;
			NotifyPropertyChanged("wallOutlineColor");
		}

		Color bombColor {
			get => config.bombColor;
			set => config.bombColor = value.ColorWithAlpha(1);
		}
		Color menuLightColor {
			get => config.menuLightColor;
			set => config.menuLightColor = value;
		}

		Color wallOutlineColor {
			get => config.wallBorderColor;
			set => config.wallBorderColor = value.ColorWithAlpha(1);
		}


		private readonly string version = $"Version {Assembly.GetExecutingAssembly().GetName().Version.ToString(3)} by Kinsi55";

		[UIComponent("sponsorsText")] CurvedTextMeshPro sponsorsText = null;
		void OpenSponsorsLink() => Process.Start("https://github.com/sponsors/kinsi55");
		void OpenSponsorsModal() {
			sponsorsText.text = "Loading...";
			Task.Run(() => {
				string desc = "Failed to load";
				try {
					desc = (new WebClient()).DownloadString("http://kinsi.me/sponsors/bsout.php");
				} catch { }

				_ = IPA.Utilities.Async.UnityMainThreadTaskScheduler.Factory.StartNew(() => {
					sponsorsText.text = desc;
					// There is almost certainly a better way to update / correctly set the scrollbar size...
					sponsorsText.gameObject.SetActive(false);
					sponsorsText.gameObject.SetActive(true);
				});
			}).ConfigureAwait(false);
		}
	}

	public static class BsmlWrapper {
		static readonly bool hasBsml = IPA.Loader.PluginManager.GetPluginFromId("BeatSaberMarkupLanguage") != null;

		public static void EnableUI() {
			void wrap() => TweaksFlowCoordinator.Initialize();

			if(hasBsml)
				wrap();
		}
		public static void DisableUI() {
			void wrap() => TweaksFlowCoordinator.Deinit();

			if(hasBsml)
				wrap();
		}
	}
}
