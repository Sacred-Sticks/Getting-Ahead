# EventBus Documentation

The `EventBus` script is a `ScriptableObject` that provides a simple event system in Unity. It allows you to create events that can be subscribed to and triggered from various components in your game. This script is part of the `Kickstarter.Events` namespace.

## Class: EventBus

### Fields

- `public Action<EventArgs> Event`: A public field of type `Action<EventArgs>`, representing the event to be subscribed to and triggered. This field can be accessed and modified from other classes.

### Methods

#### `public void CallEvent(EventArgs e)`

Triggers the event by invoking the `Event` delegate and passing the provided `EventArgs` object.

- `parameters`: An instance of the `EventArgs` class or a derived class representing the arguments to be passed to the event subscribers.

---

The `EventBus` script provides a convenient way to create and manage events in your Unity project. By adding the `Event` field to your custom `EventBus` ScriptableObject and calling the `CallEvent` method, you can trigger the event and notify any subscribed listeners. This allows for decoupling and flexible communication between different components of your game.

Remember to create and configure instances of the `EventBus` script as `ScriptableObject` assets in your project. You can then reference these assets in your components and subscribe to the events they expose.
