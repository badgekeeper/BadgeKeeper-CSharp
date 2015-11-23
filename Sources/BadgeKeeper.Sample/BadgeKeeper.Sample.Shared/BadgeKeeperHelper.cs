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

using System.Linq;
using BadgeKeeper.Objects.Models;

namespace BadgeKeeper.Sample
{
    public class BadgeKeeperHelper
    {
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
