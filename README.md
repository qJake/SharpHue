SharpHue
========

An open-source .NET library for the Philips Hue lighting system.

##Current Features

* Add a new user to the Bridge whitelist
* Retrieve all lights and light state information
* Set new light state information

## Planned Features

* Schedule management (retrieve, add, edit, delete schedules)
* Group management (retrieve, add, edit, delete groups)
* Whitelist management (retrieve and delete users)

## Examples


### Configuration initialization

First, you must initialize the API configuration, so that the API knows your username and/or device IP. There are three ways to do this.

#### Register new user

```
Configruation.AddUser();
```

This will first attempt to locate your bridge device automatically using Philips' discovery service, then register a new user with the Bridge. **NOTE:** You must press the button on the bridge before you call this method, and once you press the button you have 30 seconds to call this method.

#### Initialize with existing user

```
Configuration.Initialize("Username Here");
```

This will first attempt to locate your bridge device automatically using Philips' discovery service, then initialize the API with the specified preexisting username.

#### Initialize with existing user and manually-defined IP

```
Configuration.Initialize("Username Here", IPAddress.Parse("192.68.x.x"));
```

This will initialize the API with the specified preexisting username, and will explicitly use the IP address given, bypassing the Philips' discovery service.

### Retrieving Lights

```
LightCollection lights = new LightCollection();
```

This will retrieve all lights and their associated light state information.

Once you have a light collection, you can reference lights either by index:

```
lights[1]
```

Or by name:

```
lights["Kitchen Light 1"]
```

Lights also contain state information. To get the current hue of light 2:

```
lights[2].Hue
```

### Setting Light State

To update the state of a light, you can either pass in a `JObject` directly with your own state information, or use the provided `LightStateBuilder` class:

```
LightStateBuilder builder = new LightStateBuilder()
                            .TurnOn()
                            .Saturation(128)
                            .Brightness(128)
                            .Effect(LightEffect.ColorLoop);
lights[1].SetState(builder);
```

You use the light state builder by chaining various methods, then pass the builder into whichever light(s) you want to update using `SetState()`.
