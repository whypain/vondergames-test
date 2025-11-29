# vondergames-test

## Development Time Stamps (Not including bug fixing)

| System Name | Expected Time | Actual Time |
|-------------|---------------|-------------|
| Inventory System | 4 hours | 4.5 hours |
| Crafting System | 2 hours | 1 hour |
| Time Hop System | 2 hours | N/A |
| Combat System | 3 hours | N/A |
| Total | 11 hours | N/A |

## Breakdown
### Inventory System
| System Name | Time Taken |
|-------------|---------------|
| UI + Adding Item + Stacking Logic  | 1 hour |
| Basic Organizing Feature + Removing Item | 1 hour |
| Inventory Bar | 1.5 hour |
| Consumable Items | 1 hour |
| Total | 4.5 hours |

### Crafting System
| System Name | Time Taken |
|-------------|---------------|
| UI + CraftingSystem | 1 hour |
| Total | 1 hour |

### Bugs
| Bug Name | Time to fix |
|-------------|---------------|
| Inventory bugs | 1 hour |
| Inventory Bar bugs | 1 hour |
| CraftingSytem bugs | 1 hour |
| Total | 3 hours |

### Inventory Bugs
* Player can swap item from crafting result
* Remove item remove the item in the wrong slot

### Inventory Bar Bugs
* Player can drag item in the bar while Inventory is inactive


### CraftingSystem Bugs
* Player can craft when Inventory is full
* Player can swap items in inventory with the crafting result slot

## Features Left
* Usable Chest
* Crafting Station
* Time Hop System
* Combat System

**Estimate time needed to finish**: 
* ~16 hours, including bug fixing
* ~9 hours, not including bug fixing

**Next Steps**:
* Reuse scripts from CraftingController and Inventory to create the Crafting Station
* Reuse scripts from Inventory to create function chest
* Make TimeManager Singleton that keep tracks of days, current time of day, and manage the time hopping mechanic
* Make Player Movement Controller
* Make Weapon class and create Wand class that inherits Weapon
* Create Character abstract class with health system
* Create Weapon field in Player and add Attack function which uses the weapon
* Player and Slime inherits Character
* Implement basic state machine and behaviours for Slime
