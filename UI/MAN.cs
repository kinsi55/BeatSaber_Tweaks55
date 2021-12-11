using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.MenuButtons;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Tweaks55.UI {
	class TweaksFlowCoordinator : FlowCoordinator {
		MAN view = null;

		protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling) {
			if(firstActivation) {
				SetTitle("Tweaks55");
				showBackButton = true;

				if(view == null)
					view = BeatSaberUI.CreateViewController<MAN>();

				ProvideInitialViewControllers(view);
			}
		}

		protected override void BackButtonWasPressed(ViewController topViewController) {
			BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this, null, ViewController.AnimationDirection.Horizontal);
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


		void ClearMenuLightColor() {
			config.menuLightColor = new UnityEngine.Color(0, 0, 0, 0);
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
				});
			}).ConfigureAwait(false);
		}
	}
}
