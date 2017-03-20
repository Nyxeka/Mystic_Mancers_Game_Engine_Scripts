# Mystic_Mancers_Game_Engine_Scripts
A collection of unity scripts that I created while programming the game Mystic Mancers and the Crystal of Considerable Power, a 2.5D platformer (still WIP)
I'll be updating this as I make additions and changes to the engine.

The plan was to make the setup mostly component-based, where the users in my dev group could just drag and drop components/scripts into game-objects in unity, and thereby modularily create player characters and enemies using custom "abilities", "ai", "controllers", resources (mana/energy/etc), health, etc...

Special Note:
Also included is an animation handler script in the works that speeds up development by grabbing animations right from an imported fbx file, and putting them into a single, shared animator controller. The idea was that if you have a fairly complex animator controller, and you have multiple players/enmemies/etc... that are going to be using the same animation state machine, you just create one animator controller, and then simply drag and drop your Imported Asset from the game-world into the script, and let the script handle the rest. Currently, the script requires that you designate the animation clips on import, but soon (after the project is submitted, and we aren't on a time-limit), I'll be adding support for external fbx animations, and maybe some other solutions that I can find. The script is also hard-coded to function with the main character assets functionality in mind, and modularity will be handled post-submission as well.
