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

namespace BadgeKeeper.Exceptions
{
    /// <summary>
    /// Throws if some arguments is invalid
    /// </summary>
    public class BadgeKeeperException : System.Exception
    {
        /// <summary>
        /// Default c-tor
        /// </summary>
        public BadgeKeeperException(): base() { }

        /// <summary>
        /// C-tor with message information.
        /// </summary>
        /// <param name="message"></param>
        public BadgeKeeperException(string message) : base(message) { }
    }
}
