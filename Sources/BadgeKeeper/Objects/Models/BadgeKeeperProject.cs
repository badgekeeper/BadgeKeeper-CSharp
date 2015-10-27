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

namespace BadgeKeeper.Objects.Models
{
    /// <summary>
    /// Contains all information about Project in Badge Keeper service.
    /// </summary>
    public class BadgeKeeperProject
    {
        /// <summary>
        /// Badge Keeper project title.
        /// </summary>
        public readonly string Title;
        /// <summary>
        /// Badge Keeper project description.
        /// </summary>
        public readonly string Description;
        /// <summary>
        /// Badge Keeper project icon.
        /// </summary>
        public readonly string Icon;
        /// <summary>
        /// Array of Badge Keeper achievements.
        /// </summary>
        public readonly BadgeKeeperAchievement[] Achievements;
    }
}