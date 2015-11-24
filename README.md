# Badge Keeper

Badge Keeper Android library will help add achievement system to your app easily.

More information you can find here: [Badge Keeper official site](https://badgekeeper.net)

Test your project from web browser here: [Badge Keeper Api](https://api.badgekeeper.net/swagger/ui/index)

[![Build Status](https://travis-ci.org/badgekeeper/BadgeKeeper-CSharp.svg?branch=master)](https://travis-ci.org/badgekeeper/BadgeKeeper-CSharp)
[![Version](https://img.shields.io/nuget/v/BadgeKeeper.svg)](https://www.nuget.org/packages/BadgeKeeper/)
[![License](https://img.shields.io/badge/license-Apache%20License%2C%20Version%202.0-blue.svg)](http://www.apache.org/licenses/LICENSE-2.0)

## Getting Started

Using [Nuget](https://www.nuget.org/packages/BadgeKeeper/):
```
Install-Package BadgeKeeper
```

## Usage

### Basic Initialization

```csharp
BadgeKeeper.SetProjectId("Project Id from admin panel");
BadgeKeeper.SetUserId("Your client id");
BadgeKeeper.SetShouldLoadIcons(true); // default is false
```

That's all settings that need to be configured.

### Lambda or Actions

Badge Keeper uses callback system to get results. You can choose which one is better for your: lambda or actions.
To simplify code blocks we will continue using lambda below.

### Get project achievements (no userId required)

```csharp
BadgeKeeper.GetProjectAchievements(
  (BadgeKeeperAchievement[] achievements) =>
  {
    // Put logic here
  },
  
  (BadgeKeeperResponseError error) => {
    // Put logic here
  });
```

### Get user achievements

```csharp
BadgeKeeper.GetUserAchievements(
  (BadgeKeeperUserAchievement[] achievements) =>
  {
    // Put logic here
  },
  
  (BadgeKeeperResponseError error) => {
    // Put logic here
  });
```

### Post user variables and validate completed achievements

```csharp
BadgeKeeper.PreparePostKeyWithValue("x", 0);

BadgeKeeper.PostPreparedValues(
  (BadgeKeeperUnlockedAchievement[] achievements) =>
  {
    // Put logic here
  },
  
  (BadgeKeeperResponseError error) => {
    // Put logic here
  });
```

### Increment user variables and validate completed achievements

```csharp
BadgeKeeper.PrepareIncrementKeyWithValue("x", 0);

BadgeKeeper.IncrementPreparedValues(
  (BadgeKeeperUnlockedAchievement[] achievements) =>
  {
    // Put logic here
  },
  
  (BadgeKeeperResponseError error) => {
    // Put logic here
  });
```

## Requirements

* We support all .Net versions since 3.5 (Including universal applications, Xamarin, Asp.Net).

## License

	Copyright 2015 Badge Keeper

	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at

    	http://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.
