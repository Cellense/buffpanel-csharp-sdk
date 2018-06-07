# BuffPanel C# SDK

This repository contains an implementation with a simple interface designed to easily communicate with the REST API of the BuffPanel service from within your Unity Game.

For more information on the BuffPanel service, please visit our [website](http://buffpanel.com/), and read our [help pages](http://buffpanel.com/help/introduction).

## Installation

1.  Download project
2.  Use prebuilt library **BuffPanelSDK.dll** located in **prebuilt** folder (Visual Studio and mono builds are available) **OR** build the project, which should output the **BuffPanelSDK.dll** library.
3.  In your project right click on **References -> Add Reference... -> Browse ->** select add **BuffPanelSDK.dll**.

You can optionally include the source code files within the 'BuffPanelSDK' directory directly in your project.

### Setup tracking

Include the following line of code into your project's code. It should be executed **at the startup of the game**, **once per game session** for tracking to work correctly.

```
BuffPanel.BuffPanel.Track(game_token, is_existing_player, attributes, [logger]);
```

- `game_token` is a *string* value, that's generated by the BuffPanel platform during the registration process and is used to identify your game.
- `is_existing_player` is a *boolean* value, that lets us know whether the current player has already run the game before (*true*) or is a new player (*false*).
- `attributes` are a *Dictionary<string, string>* value, that carry any additional information valuable for you, for example purchased DLC list
- `logger` is an optional `BuffPanel.Logging.Logger` parameter used for logging additional information about BuffPanel activity

Former `player_token` is no longer used, instead universally unique identifier (UUID) is generated and persisted on player's machine implicitly.

## How it works?

When the `Track` method is called, the SDK sends an HTTP request to the REST API of the BuffPanel servers.

Upon recieving this event the BuffPanel service identifies your game using the submitted `game_token` and internally attempts to attribute the player's **run event** to any of the running **campaigns** for your game.

## Tracking Downloadable Content (DLC)

To track information about the DLC a player purchased in the past we use the `attributes` parameter. The tracking call's structure allows for tracking of either **Steam** DLC or any other DLC purchase as long as two information are passed to BuffPanel SDK:
- a unique DLC identifier for each DLC purchase
- UNIX timestamp of each DLC purchase

### Steam DLC purchase tracking

To acquire the needed information from **Steamworks SDK** you can:
1. Get the [total DLC count for the player](https://partner.steamgames.com/doc/api/ISteamApps#GetDLCCount)
2. Iterate through [all purchased DLC](https://partner.steamgames.com/doc/api/ISteamApps#BGetDLCDataByIndex), listing the entries in the `attributes` parameter format as
```
{ "dlc_<appId>": "<purchaseTimestamp>" }
```
3. Fill in the [DLC purchase UNIX timestamp](https://partner.steamgames.com/doc/api/ISteamApps#GetEarliestPurchaseUnixTime) for all the items
4. Track a run using the method containing these pair values in `attributes` parameter
```
BuffPanel.BuffPanel.Track(game_token, is_existing_player, attributes, [logger]);
```

#### Example call:

```
BuffPanel.BuffPanel.Track(
  "tetris_ultimate_4000", // buffpanel game token
  true, // whether this is an existing player
  new Dictionary<string, string>() {
    { "dlc_657540", "1506599038" },
    { "dlc_643660", "1511176765" },
    { "dlc_907480", 1511076765.ToString() }
  } // "attributes" containing the DLC tracking information
);
```

### Tracking DLC purchase from another store

To integrate other stores and distribution platforms you use, please contact us at contact-us@buffpanel.com.

### Logging

By default BuffPanel does not log anything. You can turn on logging by specifying logger instance in `Track` call.

```
BuffPanel.BuffPanel.Track(game_token, is_existing_player, attributes, new BuffPanel.Logging.ConsoleLogger());
```

`ConsoleLogger` logs all activity by default to console. You can create your own custom logger by implementing `BuffPanel.Logging.Logger` interface
