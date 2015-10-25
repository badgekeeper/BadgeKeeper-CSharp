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
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using BadgeKeeper.Objects;
using BadgeKeeper.Objects.Models;

namespace BadgeKeeper.Network
{
    class BadgeKeeperApiService : IBadgeKeeperApi
    {
        private static string BaseUrl = "https://api.badgekeeper.net/";

        public BadgeKeeperApiService()
        {
        }

        public void GetProjectAchievements(string projectId, bool shouldLoadIcons, Action<BadgeKeeperProject> onSuccess, Action<BadgeKeeperResponseError> onError)
        {
            string url = BaseUrl + $"api/gateway/{projectId}/get?shouldLoadIcons={ShouldLoadIcons(shouldLoadIcons)}";
            Get(url, onSuccess, onError);
        }

        public void GetUserAchievements(string projectId, string userId, bool shouldLoadIcons, Action<BadgeKeeperUserAchievement[]> onSuccess, Action<BadgeKeeperResponseError> onError)
        {
            string url = BaseUrl + $"api/gateway/{projectId}/users/get/{userId}/?shouldLoadIcons={ShouldLoadIcons(shouldLoadIcons)}";
            Get(url, onSuccess, onError);
        }

        public void PostUserVariables(string projectId, string userId, List<BadgeKeeperPair<string, double>> values, Action<BadgeKeeperUnlockedAchievement[]> onSuccess, Action<BadgeKeeperResponseError> onError)
        {
            string url = BaseUrl + $"api/gateway/{projectId}/users/post/{userId}";
            string body = ToJson(values);
            Post(url, body, onSuccess, onError);
        }

        public void IncrementUserVariables(string projectId, string userId, List<BadgeKeeperPair<string, double>> values, Action<BadgeKeeperUnlockedAchievement[]> onSuccess, Action<BadgeKeeperResponseError> onError)
        {
            string url = BaseUrl + $"api/gateway/{projectId}/users/increment/{userId}";
            string body = ToJson(values);
            Post(url, body, onSuccess, onError);
        }

        private void Get<ResponseType>(string url, Action<ResponseType> onSuccess, Action<BadgeKeeperResponseError> onError)
        {
            using (var client = new WebClient())
            {
                client.Headers.Clear();
                client.Headers.Add(HttpRequestHeader.Accept, "application/json");
                client.Encoding = Encoding.UTF8;
                // handler
                client.DownloadStringCompleted += (s, e) =>
                {
                    try
                    {
                        BadgeKeeperResponse<ResponseType> result = FromJson<BadgeKeeperResponse<ResponseType>>(e.Result);
                        if (result.Error != null)
                        {
                            onError(result.Error);
                        }
                        else
                        {
                            onSuccess(result.Result);
                        }
                    }
                    catch (Exception ex)
                    {
                        onError(new BadgeKeeperResponseError(-1, ex.Message));
                    }
                };
                // make request
                client.DownloadStringAsync(new Uri(url));
            }
        }

        private void Post<ResponseType>(string url, string body, Action<ResponseType> onSuccess, Action<BadgeKeeperResponseError> onError)
        {
            using (var client = new WebClient())
            {
                client.Headers.Clear();
                client.Headers.Add(HttpRequestHeader.Accept, "application/json");
                client.Encoding = Encoding.UTF8;
                // handler
                client.DownloadStringCompleted += (s, e) =>
                {
                    try
                    {
                        BadgeKeeperResponse<ResponseType> result = FromJson<BadgeKeeperResponse<ResponseType>>(e.Result);
                        if (result.Error != null)
                        {
                            onError(result.Error);
                        }
                        else
                        {
                            onSuccess(result.Result);
                        }
                    }
                    catch (Exception ex)
                    {
                        onError(new BadgeKeeperResponseError(-1, ex.Message));
                    }
                };
                // make request
                client.UploadStringAsync(new Uri(url), "POST", body);
            }
        }

        private static string ShouldLoadIcons(bool shouldLoadIcons)
        {
            return (shouldLoadIcons ? "true" : "false");
        }

        private static Type FromJson<Type>(string data)
        {
            if (data != null && data.Length > 0)
            {
                var serializer = new DataContractJsonSerializer(typeof(Type));
                using (var stream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(data)))
                {
                    Type result = (Type)serializer.ReadObject(stream);
                    return result;
                }
            }
            throw new Exception("Can not parse json response.");
        }

        private static string ToJson<Type>(Type data)
        {
            string result = null;
            if (data != null)
            {
                var serializer = new DataContractJsonSerializer(typeof(Type));
                using (var stream = new System.IO.MemoryStream())
                {
                    serializer.WriteObject(stream, data);
                    result = Encoding.UTF8.GetString(stream.ToArray());
                }
            }
            return result;
        }
    }
}
