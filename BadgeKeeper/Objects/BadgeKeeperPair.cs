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

namespace BadgeKeeper.Objects
{
    /// <summary>
    /// Badge Keeper key-value pair class.
    /// </summary>
    /// <typeparam name="K">Generic key</typeparam>
    /// <typeparam name="V">Generic value</typeparam>
    public class BadgeKeeperPair<K, V>
    {
        /// <summary>
        /// Default c-tor
        /// </summary>
        public BadgeKeeperPair()
        {
        }

        /// <summary>
        /// C-tor with key and value parameters
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public BadgeKeeperPair(K key, V value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Pair key
        /// </summary>
        public K Key { get; set; }
        /// <summary>
        /// Pair value
        /// </summary>
        public V Value { get; set; }

    }
}
