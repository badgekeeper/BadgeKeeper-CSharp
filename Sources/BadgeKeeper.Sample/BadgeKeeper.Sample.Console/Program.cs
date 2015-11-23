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
using System.Linq;
using BadgeKeeper.Network;
using BadgeKeeper.Objects.Models;

namespace BadgeKeeper.Sample.Console
{
    class Program
    {
        private static bool isLoading = false;
        private static string userId = "";

        private static int posts = 0;
        private static int increments = 0;

        private static Action<BadgeKeeperResponseError> onError = (BadgeKeeperResponseError error) =>
        {
            System.Console.WriteLine(error.Message);
            isLoading = false;
        };

        private static Action<BadgeKeeperAchievement[]> onProject = (BadgeKeeperAchievement[] achievements) =>
        {
            string text = ProjectAchievementsToString(achievements);
            System.Console.WriteLine();
            System.Console.WriteLine(text);
            System.Console.WriteLine();
            isLoading = false;
        };

        private static Action<BadgeKeeperUserAchievement[]> onUser = (BadgeKeeperUserAchievement[] achievements) =>
        {
            string text = UserAchievementsToString(achievements);
            System.Console.WriteLine();
            System.Console.WriteLine(text);
            System.Console.WriteLine();
            isLoading = false;
        };

        private static Action<BadgeKeeperUnlockedAchievement[]> onUnlocked = (BadgeKeeperUnlockedAchievement[] achievements) =>
        {
            string text = UnlockedAchievementsToString(achievements);
            System.Console.WriteLine();
            System.Console.WriteLine(text);
            System.Console.WriteLine();
            isLoading = false;
        };

        static void Main(string[] args)
        {
            BadgeKeeper.SetProjectId("a93a3a6d-d5f3-4b5c-b153-538063af6121");

            do
            {
                if (isLoading)
                {
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }
                System.Console.WriteLine("Input action:");
                System.Console.WriteLine("1 - Get project achievements");
                System.Console.WriteLine("2 - Set user id");
                System.Console.WriteLine("3 - Get user achievements");
                System.Console.WriteLine("4 - Post user value");
                System.Console.WriteLine("5 - Increment user value");
                System.Console.WriteLine("6 - Close application");

                System.Console.Write("Choose action: ");
                string line = System.Console.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    int value = 0;
                    if (int.TryParse(line, out value))
                    {
                        switch (value)
                        {
                            case 1:
                                {
                                    LoadProjectAchievements();
                                    break;
                                }
                            case 2:
                                {
                                    SetUserId();
                                    break;
                                }
                            case 3:
                                {
                                    LoadUserAchievements();
                                    break;
                                }
                            case 4:
                                {
                                    PostUserVariables();
                                    break;
                                }
                            case 5:
                                {
                                    IncrementUserVariables();
                                    break;
                                }
                            case 6: return;
                        }
                    }
                }
            } while (true);
        }

        private static void LoadProjectAchievements()
        {
            isLoading = true;
            BadgeKeeper.GetProjectAchievements(onProject, onError);
        }

        private static void SetUserId()
        {
            do
            {
                System.Console.Write("Input user id: ");
                userId = System.Console.ReadLine();
            } while (string.IsNullOrEmpty(userId));
            BadgeKeeper.SetUserId(userId);
        }

        private static void LoadUserAchievements()
        {
            if (string.IsNullOrEmpty(userId))
            {
                System.Console.WriteLine("User Id is empty. Set User Id and try again.");
                return;
            }
            isLoading = true;
            BadgeKeeper.GetUserAchievements(onUser, onError);
        }

        private static void PostUserVariables()
        {
            if (string.IsNullOrEmpty(userId))
            {
                System.Console.WriteLine("User Id is empty. Set User Id and try again.");
                return;
            }

            isLoading = true;
            ++posts;
            BadgeKeeper.PreparePostKeyWithValue("x", posts);
            BadgeKeeper.PostPreparedValues(onUnlocked, onError);
        }

        private static void IncrementUserVariables()
        {
            if (string.IsNullOrEmpty(userId))
            {
                System.Console.WriteLine("User Id is empty. Set User Id and try again.");
                return;
            }

            isLoading = true;
            ++increments;
            BadgeKeeper.PrepareIncrementKeyWithValue("x", increments);
            BadgeKeeper.IncrementPreparedValues(onUnlocked, onError);
        }
        
        public static string ProjectAchievementsToString(BadgeKeeperAchievement[] achievements)
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
            return text;
        }

        public static string UserAchievementsToString(BadgeKeeperUserAchievement[] achievements)
        {
            string text = "Achievements: [";
            foreach (var achievement in achievements)
            {
                string atext = $"{{ \"Title\": \"{achievement.DisplayName}\", \"Description\": \"{achievement.Description}\", \"IsUnlocked\": \"{achievement.IsUnlocked}\" }}";
                if (achievements.Last() != achievement)
                {
                    atext += ", ";
                }
                text += atext;
            }
            text += "]";
            return text;
        }

        public static string UnlockedAchievementsToString(BadgeKeeperUnlockedAchievement[] achievements)
        {
            string text = "Achievements: [";
            foreach (var achievement in achievements)
            {
                string rtext = "[ ";
                foreach (var reward in achievement.Rewards)
                {
                    rtext += $"{{ \"Name\": \"{reward.Name}\", \"Value\": \"{reward.Value}\" }}";
                    if (achievement.Rewards.Last() != reward)
                    {
                        rtext += ", ";
                    }
                }
                rtext += " ]";

                string atext = $"{{ \"Title\": \"{achievement.DisplayName}\", \"Description\": \"{achievement.Description}\", \"IsUnlocked\": \"{achievement.IsUnlocked}\", \"Rewards\": {rtext} }}";
                if (achievements.Last() != achievement)
                {
                    atext += ", ";
                }
                text += atext;
            }
            text += "]";
            return text;
        }
    }
}
