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
    /// <summary>
    /// Interface of Badge Keeper API service.
    /// </summary>
    public interface IBadgeKeeperApi
    {
        /// <summary>
        /// Get project information from Badge Keeper services.
        /// </summary>
        /// <param name="projectId">Project Id from Badge Keeper admin panel.</param>
        /// <param name="shouldLoadIcons">If True - Badge keeper will return base64 icon string.</param>
        /// <param name="onSuccess">Callback for success request.</param>
        /// <param name="onError">Callback for error request.</param>
        void GetProjectAchievements(string projectId, bool shouldLoadIcons, Action<BadgeKeeperProject> onSuccess, Action<BadgeKeeperResponseError> onError);
        /// <summary>
        /// Get array of user achievements from Badge Keeper services.
        /// </summary>
        /// <param name="projectId">Project Id from Badge Keeper admin panel.</param>
        /// <param name="userId">Unique User Id in your service.</param>
        /// <param name="shouldLoadIcons">If True - Badge keeper will return base64 icon string.</param>
        /// <param name="onSuccess">Callback for success request.</param>
        /// <param name="onError">Callback for error request.</param>
        void GetUserAchievements(string projectId, string userId, bool shouldLoadIcons, Action<BadgeKeeperUserAchievement[]> onSuccess, Action<BadgeKeeperResponseError> onError);
        /// <summary>
        /// Sends all prepared values to server to increase them and validate achievements completion.
        /// Before sending values must be prepared via PrepareIncrementValue method calls.
        /// </summary>
        /// <param name="projectId">Project Id from Badge Keeper admin panel.</param>
        /// <param name="userId">Unique User Id in your service.</param>
        /// <param name="values">Prepared list of values that will be set on Badge Keeper service.</param>
        /// <param name="onSuccess">Callback for success request.</param>
        /// <param name="onError">Callback for error request.</param>
        void PostUserVariables(string projectId, string userId, List<BadgeKeeperPair<string, double>> values, Action<BadgeKeeperUnlockedAchievement[]> onSuccess, Action<BadgeKeeperResponseError> onError);
        /// <summary>
        /// Sends all prepared values to server to overwrite them and validate achievements completion.
        /// Before sending values must be prepared via PreparePostValue method calls.
        /// </summary>
        /// <param name="projectId">Project Id from Badge Keeper admin panel.</param>
        /// <param name="userId">Unique User Id in your service.</param>
        /// <param name="values">Prepared list of values that will be incremented on Badge Keeper service.</param>
        /// <param name="onSuccess">Callback for success request.</param>
        /// <param name="onError">Callback for error request.</param>
        void IncrementUserVariables(string projectId, string userId, List<BadgeKeeperPair<string, double>> values, Action<BadgeKeeperUnlockedAchievement[]> onSuccess, Action<BadgeKeeperResponseError> onError);
    }
}
