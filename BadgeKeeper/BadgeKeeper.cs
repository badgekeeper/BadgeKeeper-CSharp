﻿// Copyright 2015 Badge Keeper
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
using BadgeKeeper.Network;

namespace BadgeKeeper
{
    public class BadgeKeeper
    {
        // Singleton instance
        private static BadgeKeeper _instance;

        // Service properties
        private string _projectId;
        private string _userId;
        private bool _shouldLoadIcons = false;

        private IBadgeKeeperApi _apiService;

        // Temporary storage
        private Dictionary<string, List<BadgeKeeperPair<string, double>>> postVariables = new Dictionary<string, List<BadgeKeeperPair<string, double>>>();
        private Dictionary<string, List<BadgeKeeperPair<string, double>>> incrementVariables = new Dictionary<string, List<BadgeKeeperPair<string, double>>>();

        private BadgeKeeper()
        {
            _apiService = new BadgeKeeperApiService();
        }

        public static BadgeKeeper Instance()
        {
            if (_instance == null)
            {
                _instance = new BadgeKeeper();
            }
            return _instance;
        }

        /// <summary>
        /// Setup Project Id for Badge Keeper instance. You can find Project Id in admin panel.
        /// </summary>
        /// <param name="projectId">Project Id from Badge Keeper admin panel.</param>
        public static void SetProjectId(string projectId)
        {
            Instance()._projectId = projectId;
        }

        /// <summary>
        /// Setup User Id for Badge Keeper instance. This is client unique id in your system.
        /// </summary>
        /// <param name="userId">Unique client Id in your system.</param>
        public static void SetUserId(string userId)
        {
            Instance()._userId = userId;
        }

        /// <summary>
        /// When sets to true load achievement icons from Badge Keeper service
        /// with base64 encoded images for unloked and locked states.
        /// Sets this parameter to false can reduce traffic and you will get response faster.
        /// </summary>
        /// <param name="shouldLoadIcons">Should we request images from Badge Keeper service or not.</param>
        public static void SetShouldLoadIcons(bool shouldLoadIcons)
        {
            Instance()._shouldLoadIcons = shouldLoadIcons;
        }

        /// <summary>
        /// Requests all project achievements list.
        /// Check that Project Id and callback configured.
        /// </summary>
        /// <returns>Array of BadgeKeeperAchievement.</returns>
        public static void GetProjectAchievements(Action<BadgeKeeperAchievement[]> onSuccess, Action<BadgeKeeperResponseError> onError)
        {
            Instance().CheckProjectParameters();
            Action<BadgeKeeperProject> onProjectReceived = (BadgeKeeperProject project) =>
            {
                onSuccess(project.Achievements);
            };
            Api().GetProjectAchievements(Instance()._projectId, Instance()._shouldLoadIcons, onProjectReceived, onError);
        }

        /// <summary>
        /// Requests all project achievements list async.
        /// Check that Project Id and callback configured.
        /// </summary>
        /// <returns>Array of BadgeKeeperAchievement.</returns>
        public static void GetProjectAchievementsAsync(Action<BadgeKeeperUserAchievement[]> onSuccess, Action<BadgeKeeperResponseError> onError)
        {
            Instance().CheckParameters();
            Api().GetUserAchievements(Instance()._projectId, Instance()._userId, Instance()._shouldLoadIcons, onSuccess, onError);
        }

        private static IBadgeKeeperApi Api()
        {
            return Instance()._apiService;
        }

        // Configuration validation
        private void CheckParameters()
        {
            CheckProjectParameters();
            if (_userId == null || _userId.Length <= 0)
            {
                throw new Exceptions.BadgeKeeperException("User Id property is not configured.");
            }
        }

        private void CheckProjectParameters()
        {
            if (_projectId == null || _projectId.Length <= 0)
            {
                throw new Exceptions.BadgeKeeperException("Project Id property is not configured.");
            }
        }
    }
}
