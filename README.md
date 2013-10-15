# SharpHue

An open-source .NET library for the Philips Hue lighting system.


## Current Features

* Add a new user to the Bridge whitelist
* Retrieve all lights and light state information
* Set new light state information
* Set light state for multiple lights or all lights
* Discover new lights


## Planned Features

* Schedule management (retrieve, add, edit, delete schedules)
* Group management (retrieve, add, edit, delete groups)
* Whitelist management (retrieve and delete users)


## Examples

### Configuration initialization

First, you must initialize the API configuration, so that the API knows your username and/or device IP. There are three ways to do this.

#### 1. Register new user

```cs
Configruation.AddUser();
```

This will first attempt to locate your bridge device automatically using Philips' discovery service, then register a new user with the Bridge. **NOTE:** You must press the button on the bridge before you call this method, and once you press the button you have 30 seconds to call this method.

After this method returns, if there were no errors or exceptions, the `Configuration.Username` property will contain the newly registered username, in case you need to refer to it.

#### 2. Initialize with existing user

```cs
Configuration.Initialize("Username Here");
```

This will first attempt to locate your bridge device automatically using Philips' discovery service, then initialize the API with the specified preexisting username.

#### 3. Initialize with existing user and manually-defined IP

```cs
Configuration.Initialize("Username Here", IPAddress.Parse("192.68.x.x"));
```

This will initialize the API with the specified preexisting username, and will explicitly use the IP address given, bypassing the Philips' discovery service.

### Retrieving Lights

```cs
LightCollection lights = new LightCollection();
```

This will retrieve all lights and their associated light state information. If you already have a `LightCollection` and want to refresh it, simply call `lights.Refresh()`.

Once you have a light collection, you can reference lights either by index:

```cs
lights[1]
```

Or by name:

```cs
lights["Kitchen Light 1"]
```

Lights also contain state information. To get the current hue of light 2:

```cs
lights[2].State.Hue
```

**NOTE:** Accessing a `LightCollection` by index is **one-based**, not **zero-based** like most arrays. Attempting to retrieve `lights[0]` will return `null`.

### Enumerating Lights

`LightCollection` implements `IReadOnlyCollection<T>` using `Light` as its generic parameter, so if you have a `LightCollection`, you can do anything you can do with other enumerators, including LINQ:

```cs
var UpstairsLightsOn = (from l in lights
                        where l.Name.StartsWith("2F")
                        where l.State.IsOn
                        select l).ToList();
```

### Setting Light State

#### Using JSON

To update the state of a light, you can pass in a `JObject` directly with your own state information:

```cs
var state = new JObject(new JProperty("on", true));
lights[3].SetState(state);
```

Or even just write your own JSON:

```cs
var json = JObject.Parse("{on: true, ct: 137, bri: 255, effect: \"colorloop\"}");
lights[5].SetState(json);
```

Using a `JObject` is required for basic JSON validation (so that a non-JSON string isn't passed in and sent to the device).

#### Using `LightStateBuilder`

An easier way to build a new state is to use the provided `LightStateBuilder` class:

```cs
// Build a new light state
LightStateBuilder builder = new LightStateBuilder()
                            .TurnOn()
                            .Saturation(128)
                            .Brightness(128)
                            .Effect(LightEffect.ColorLoop);
                            
// Apply the state to one or more lights
lights[1].SetState(builder);
lights[4].SetState(builder);
```

You use the light state builder by chaining various methods, then pass the builder into whichever light(s) you want to update using `SetState()`.

#### Using `LightStateBuilder` Exclusively

You can also just use a LightStateBuilder directly, to apply a new state to one or more lights:

```cs
  new LightStateBuilder()
      .For(lights[1], lights[3]) // Specifies one or more lights which this new state is for
// Or .For(lights, 1, 3)
      .TurnOn()                  // Turns on the light(s)
      .ColorTemperature(137)     // Sets the color temperature to 6500K
      .Brightness(255)           // Set the brightness to maximum
      .Apply();                  // Send the light state that was just built to the lights specified in .For()
```

You can also apply a new state to every single light using `.ForAll()`:

```cs
// Party Mode
new LightStateBuilder()
    .ForAll()
    .TurnOn()
    .Effect(LightEffect.ColorLoop)
    .Brightness(255)
    .Apply();
```

Order matters! Always call `.Apply()` last, because you cannot call any more methods after Apply (it returns `void`). Also, if you are using `.Apply()`, be sure to call either `.For()` or `.ForAll()`, otherwise an exception will be thrown.

## Contributing

Report bugs, request features, or fork it, code it yourself, and send me a pull request!

## Credits

Uses the [Newtonsoft.Json](http://json.codeplex.com/) library for JSON handling.
