// Copyright 2015 Badge Keeper
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using System.Threading.Tasks;
using BadgeKeeper.Network;
using BadgeKeeper.Objects.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BadgeKeeper.Sample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private TextBox _responseTextBox;
        private TextBox _userIdTextBox;
        private Button _getProjectButton;
        private Button _getUserButton;
        private Button _postButton;
        private Button _incrementButton;

        private static int posts = 0;
        private static int increments = 0;

        private Action<BadgeKeeperResponseError> _onError;
        private Action<BadgeKeeperUnlockedAchievement[]> _onUnlocked;
        private Action<BadgeKeeperAchievement[]> _onProject;
        private Action<BadgeKeeperUserAchievement[]> _onUser;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            _userIdTextBox = (TextBox)FindName("UserIdTextBox");
            _responseTextBox = (TextBox)FindName("ResponseTextBox");
            _getProjectButton = (Button)FindName("GetProjectButton");
            _getUserButton = (Button)FindName("GetAchievementsButton");
            _postButton = (Button)FindName("PostVariablesButton");
            _incrementButton = (Button)FindName("IncrementVariablesButton");

            _onError = (BadgeKeeperResponseError error) =>
            {
                DidReceiveResponseError(error);
            };

            _onUnlocked = (BadgeKeeperUnlockedAchievement[] achievements) =>
            {
                string text = BadgeKeeperHelper.UnlockedAchievementsToString(achievements);
                SetResponseText(text);
                SetLoading(false);
            };

            _onProject = (BadgeKeeperAchievement[] achievements) =>
            {
                string text = BadgeKeeperHelper.ProjectAchievementsToString(achievements);
                SetResponseText(text);
                SetLoading(false);
            };

            _onUser = (BadgeKeeperUserAchievement[] achievements) =>
            {
                string text = BadgeKeeperHelper.UserAchievementsToString(achievements);
                SetResponseText(text);
                SetLoading(false);
            };

            BadgeKeeper.SetProjectId("a93a3a6d-d5f3-4b5c-b153-538063af6121");
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void GetProjectAchievementsClick(object sender, RoutedEventArgs e)
        {
            SetLoading(true);
            BadgeKeeper.GetProjectAchievements(_onProject, _onError);
        }

        private async void GetUserAchievementsClick(object sender, RoutedEventArgs e)
        {
            bool result = await IsLoginValid();
            if (result)
            {
                SetLoading(true);
                BadgeKeeper.SetUserId(_userIdTextBox.Text);
                BadgeKeeper.GetUserAchievements(_onUser, _onError);
            }
        }

        private async void PostUserValuesClick(object sender, RoutedEventArgs e)
        {
            bool result = await IsLoginValid();
            if (result)
            {
                ++posts;

                SetLoading(true);
                BadgeKeeper.SetUserId(_userIdTextBox.Text);
                BadgeKeeper.PreparePostKeyWithValue("x", posts);
                BadgeKeeper.PostPreparedValues(_onUnlocked, _onError);
            }
        }

        private async void IncrementUserValuesClick(object sender, RoutedEventArgs e)
        {
            bool result = await IsLoginValid();
            if (result)
            {
                ++increments;

                SetLoading(true);
                BadgeKeeper.SetUserId(_userIdTextBox.Text);
                BadgeKeeper.PrepareIncrementKeyWithValue("x", increments);
                BadgeKeeper.IncrementPreparedValues(_onUnlocked, _onError);
            }
        }

        private void DidReceiveResponseError(BadgeKeeperResponseError error)
        {
            string text = $"Error: [\"Code\": {error.Code}, \"Message\": \"{error.Message}\"";
            SetResponseText(text);
            SetLoading(false);
        }

        private void SetResponseText(string text)
        {
            _responseTextBox.Text = text;
        }

        private async Task<bool> IsLoginValid()
        {
            string login = _userIdTextBox.Text;
            if (string.IsNullOrEmpty(login))
            {
                var dialog = new MessageDialog("You should specify your User ID first!");
                await dialog.ShowAsync();
                return false;
            }
            return true;
        }

        private void SetLoading(bool isLoading)
        {
            _userIdTextBox.IsEnabled = !isLoading;
            _getProjectButton.IsEnabled = !isLoading;
            _getUserButton.IsEnabled = !isLoading;
            _postButton.IsEnabled = !isLoading;
            _incrementButton.IsEnabled = !isLoading;
            _responseTextBox.IsEnabled = !isLoading;

            if (isLoading)
            {
                SetResponseText("Loading data from server ...");
            }
        }
    }
}
