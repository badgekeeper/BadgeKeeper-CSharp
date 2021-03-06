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

using System.Runtime.Serialization;

namespace BadgeKeeper.Objects.Models
{
    /// <summary>
    /// Present general element in Badge Keeper service - achievement.
    /// </summary>
    [DataContract]
    public class BadgeKeeperAchievement
    {
        /// <summary>
        /// Title of Achievement.
        /// </summary>
        [DataMember]
        public readonly string DisplayName;
        /// <summary>
        /// Description of Achievement.
        /// </summary>
        [DataMember]
        public readonly string Description;
        /// <summary>
        /// Base64 string of unlocked icon for achievement if exist.
        /// </summary>
        [DataMember]
        public readonly string UnlockedIcon;
        /// <summary>
        /// Base64 string of locked icon for achievement if exist.
        /// </summary>
        [DataMember]
        public readonly string LockedIcon;
    }
}
