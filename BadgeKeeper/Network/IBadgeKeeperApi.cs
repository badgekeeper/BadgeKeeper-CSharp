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
using System.Collections.Generic;
using BadgeKeeper.Objects;
using BadgeKeeper.Objects.Models;

namespace BadgeKeeper.Network
{
    public interface IBadgeKeeperApi
    {
        void GetProjectAchievements(string projectId, bool shouldLoadIcons, Action<BadgeKeeperProject> onSuccess, Action<BadgeKeeperResponseError> onError);
        void GetUserAchievements(string projectId, string userId, bool shouldLoadIcons, Action<BadgeKeeperUserAchievement[]> onSuccess, Action<BadgeKeeperResponseError> onError);
        void PostUserVariables(string projectId, string userId, List<BadgeKeeperPair<string, double>> values, Action<BadgeKeeperUnlockedAchievement[]> onSuccess, Action<BadgeKeeperResponseError> onError);
        void IncrementUserVariables(string projectId, string userId, List<BadgeKeeperPair<string, double>> values, Action<BadgeKeeperUnlockedAchievement[]> onSuccess, Action<BadgeKeeperResponseError> onError);
    }
}
