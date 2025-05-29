# UdonSharp Examples

This repository contains various examples of UdonSharp scripts for VRChat worlds. Each example is designed to demonstrate different features and functionalities of UdonSharp, with a focus on counter implementations using different networking approaches.

## Table of Contents

- [UdonSharp Examples](#udonsharp-examples)
  - [Table of Contents](#table-of-contents)
  - [Examples](#examples)
    - [01 LocalCounter](#01-localcounter)
    - [02 OwnerCounter](#02-ownercounter)
    - [03 OwnerOnlyCounter](#03-owneronlycounter)
    - [04 WorldCounter](#04-worldcounter)
  - [Contributing](#contributing)
  - [License](#license)

## Examples

### [01 LocalCounter](01_LocalCounter/LocalCounter.cs)

A purely local counter implementation that only affects the client interacting with it. This counter has no network events or synchronization capabilities - each player sees their own independent counter value. Ideal for personal tracking metrics or simple UI interactions that don't need to be shared.

### [02 OwnerCounter](02_OwnerCounter/OwnerCounter.cs)

A networked counter where a designated owner maintains the state variable. All players can interact with the counter interface, but interactions from non-owners send network events to the owner, who performs the actual variable update and synchronization. This architecture is **recommended for most use cases** as it prevents race conditions while maintaining full interactivity.

### [03 OwnerOnlyCounter](03_OwnerOnlyCounter/OwnerOnlyCounter.cs)

A restricted access counter where only the object's owner can increment the value. The UI automatically disables interaction capabilities for non-owners while still displaying the synchronized value across all clients. This pattern is useful when you want to restrict control to specific players (like world creators or designated moderators) while maintaining value synchronization.

### [04 WorldCounter](04_WorldCounter/WorldCounter.cs)

A fully interactive counter that can be incremented by any player in the world. When interacted with, the player automatically becomes the owner of the object and updates the shared value. While simple to implement, this approach is **not recommended for production use** due to potential race conditions when multiple players interact simultaneously. Consider using the OwnerCounter pattern instead for more reliable synchronization.

## Contributing

If you have suggestions for new examples or improvements to existing ones, feel free to open an issue or submit a pull request. Contributions are welcome!

## License

MIT
