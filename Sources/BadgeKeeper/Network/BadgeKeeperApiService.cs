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
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using BadgeKeeper.Objects;
using BadgeKeeper.Objects.Models;

#if !(PORTABLE40 || PORTABLE)
using System.Net;
#else
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
#endif

namespace BadgeKeeper.Network
{
    class BadgeKeeperApiService : IBadgeKeeperApi
    {
        private static string BaseUrl = "https://api.badgekeeper.net/";
#if (PORTABLE40 || PORTABLE)
        private HttpClient _client = new HttpClient();
#endif

        public BadgeKeeperApiService()
        {
#if (PORTABLE40 || PORTABLE)
            _client = new HttpClient();
            _client.Timeout = TimeSpan.FromSeconds(30);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
#endif
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
#if !(PORTABLE40 || PORTABLE)
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
                        ProcessNonPortableResponse(e.Result, onSuccess, onError);
                    }
                    catch (Exception ex)
                    {
                        onError(new BadgeKeeperResponseError(-1, ex.Message));
                    }
                };
                // make request
                client.DownloadStringAsync(new Uri(url));
            }
#else
            _client.GetAsync(url).ContinueWith(task => ProcessPortableResponse(task, onSuccess, onError), TaskScheduler.FromCurrentSynchronizationContext());
#endif
        }

        private void Post<ResponseType>(string url, string body, Action<ResponseType> onSuccess, Action<BadgeKeeperResponseError> onError)
        {
#if !(PORTABLE40 || PORTABLE)
            using (var client = new WebClient())
            {
                client.Headers.Clear();
                client.Headers.Add(HttpRequestHeader.Accept, "application/json");
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.Encoding = Encoding.UTF8;
                // handler
                client.UploadStringCompleted += (s, e) =>
                {
                    try
                    {
                        ProcessNonPortableResponse(e.Result, onSuccess, onError);
                    }
                    catch (Exception ex)
                    {
                        onError(new BadgeKeeperResponseError(-1, ex.Message));
                    }
                };
                // make request
                client.UploadStringAsync(new Uri(url), "POST", body);
            }
#else
            StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
            _client.PostAsync(url, content).ContinueWith(task => ProcessPortableResponse(task, onSuccess, onError), TaskScheduler.FromCurrentSynchronizationContext());
#endif
        }

#if (PORTABLE40 || PORTABLE)
        /// <summary>
        /// Process response message for portable library
        /// </summary>
        /// <typeparam name="ResponseType"></typeparam>
        /// <param name="task">Response task result</param>
        /// <param name="onSuccess">Success callback</param>
        /// <param name="onError">Error callback</param>
        private void ProcessPortableResponse<ResponseType>(Task<HttpResponseMessage> task, Action<ResponseType> onSuccess, Action<BadgeKeeperResponseError> onError)
        {
            if (task.IsFaulted)
            {
                onError(new BadgeKeeperResponseError(-1, task.Exception != null ? task.Exception.Message : "Internal server error."));
            }
            else if (task.IsCanceled)
            {
                onError(new BadgeKeeperResponseError(-2, "Request cancelled."));
            }
            else
            {
                HttpResponseMessage message = task.Result;
                if (task.Result.IsSuccessStatusCode)
                {
                    task.Result.Content.ReadAsStringAsync().ContinueWith(content =>
                    {
                        var result = FromJson<BadgeKeeperResponse<ResponseType>>(content.Result);
                        if (result.Error != null)
                        {
                            onError(result.Error);
                        }
                        else
                        {
                            onSuccess(result.Result);
                        }
                    });
                }
                else
                {
                    onError(new BadgeKeeperResponseError(-1, "Internal server error."));
                }
            }
        }
#else
        /// <summary>
        /// Process response message for non portable library
        /// </summary>
        /// <typeparam name="ResponseType"></typeparam>
        /// <param name="response">Response string for request</param>
        /// <param name="onSuccess">Success callback</param>
        /// <param name="onError">Error callback</param>
        private void ProcessNonPortableResponse<ResponseType>(string response, Action<ResponseType> onSuccess, Action<BadgeKeeperResponseError> onError)
        {
            try
            {
                BadgeKeeperResponse<ResponseType> result = FromJson<BadgeKeeperResponse<ResponseType>>(response);
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
        }
#endif

        private static string ShouldLoadIcons(bool shouldLoadIcons)
        {
            return (shouldLoadIcons ? "true" : "false");
        }

        private static Type FromJson<Type>(string data)
        {
            if (!string.IsNullOrEmpty(data))
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
                    var bytes = stream.ToArray();
                    result = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                }
            }
            return result;
        }
    }
}
