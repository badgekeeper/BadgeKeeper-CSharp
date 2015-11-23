using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BadgeKeeper.Objects.Models;
using BadgeKeeper.Network;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BadgeKeeper.Sample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private TextBox _responseTextBox;

        public MainPage()
        {
            this.InitializeComponent();
            _responseTextBox = (TextBox)FindName("ResponseTextBox");

            BadgeKeeper.SetProjectId("a93a3a6d-d5f3-4b5c-b153-538063af6121");
        }

        private void GetProjectAchievementsClick(object sender, RoutedEventArgs e)
        {
            SetLoading(true);
            SetResponseText("");

            BadgeKeeper.GetProjectAchievements(
                (BadgeKeeperAchievement[] achievements) =>
                {
                    string text = "Achievements: [";
                    foreach (var achievement in achievements)
                    {
                        string atext = $"{{ \"Title\": \"{achievement.DisplayName}\", \"Description\": \"{achievement.Description}\" }}";
                        if (achievements.Last() != achievement)
                        {
                            atext += ", ";
                        }
                        text += atext;
                    }
                    text += "]";

                    SetResponseText(text);
                    SetLoading(false);
                },
                (BadgeKeeperResponseError error) =>
                {
                    DidReceiveResponseError(error);
                });
        }
        
        private void GetUserAchievementsClick(object sender, RoutedEventArgs e)
        {

        }

        private void PostUserValuesClick(object sender, RoutedEventArgs e)
        {

        }

        private void IncrementUserValuesClick(object sender, RoutedEventArgs e)
        {

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

        private void SetLoading(bool isLoading)
        {
            var userIdTextBox = (TextBox)FindName("UserIdTextBox");

            if (isLoading)
            {
                userIdTextBox.IsEnabled = false;

                //self.loginTextField.enabled = NO;
                //self.postVariablesButton.enabled = NO;
                //self.incrementVariablesButton.enabled = NO;
                //self.getProjectAchievementsButton.enabled = NO;
                //self.getUserAchievementsButton.enabled = NO;
                //[self.activityIndicatorView startAnimating];

            }
            else
            {
                userIdTextBox.IsEnabled = true;

                //self.loginTextField.enabled = YES; 
                //self.postVariablesButton.enabled = YES; 
                //self.incrementVariablesButton.enabled = YES; 
                //self.getProjectAchievementsButton.enabled = YES; 
                //self.getUserAchievementsButton.enabled = YES; 
                //[self.activityIndicatorView stopAnimating];
            }
        }
    }
}
