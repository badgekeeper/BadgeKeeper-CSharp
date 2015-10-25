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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadgeKeeper
{
    public class BadgeKeeper
    {
        private static BadgeKeeper _instance;

        private string _userId;
        private string _projectId;
        private bool _shouldLoadIcons;

        private BadgeKeeper() { }

        public static BadgeKeeper Instance()
        {
            if (_instance == null)
            {
                _instance = new BadgeKeeper();
            }
            return _instance;
        }
        
        public static void SetUserId(string userId)
        {
            Instance()._userId = userId;
        }

        public static void SetProjectId(string projectId)
        {
            Instance()._projectId = projectId;
        }

        public static void SetShouldLoadIcons(bool shouldLoadIcons)
        {
            Instance()._shouldLoadIcons = shouldLoadIcons;
        }
    }
}
