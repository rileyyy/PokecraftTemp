TransitionZone works very simply to load or unload a new map defined in the `MapToLoad` and `MapToUnload` properties respectively.
It uses StickNodeName (a Node2D) on both maps to know where they should be attached to each other.
The TransitionZone finds, and packs the map before adding it to the List of loaded maps.

Before collision:
![image](https://github.com/user-attachments/assets/5e7e2bdc-be3a-4414-a23f-ae61c75e7c2d)

After collision:
<img width="1052" alt="image" src="https://github.com/user-attachments/assets/5335604b-e4b3-4e71-bfb4-6310f4310e67" />

This was the first step to figure out, as Azeroth is absolutely massive, and therefore needs to be loaded in managable chunks.
It also reduces the demand that sections "fit" into spaces. This way I can prioritize the feel of a zone over the "true" size to make all maps mesh together correctly.
