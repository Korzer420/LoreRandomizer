# LoreRandomizer
Hollow Knight Randomizer 4 connection that adds a bunch of lore related checks.

## Settings

### Use Custom Lore
Gives all lore items (randomized by Lore Tablets) a unique name and a new sprite (by kadala <3)
If lore tablets are not randomized, this will place the "enhanced" lore items at their vanilla locations instead.

### Randomize NPC
Randomizes certain NPC and their respective dialogue. (21 locations + 21 items)
The focus is on NPC which mostly exists for lore purposes. After you obtained their check and reload the room, the behave normally.
This includes:
- Bardoon
- Bretta* (in Fungal Wastes)
- Dung Defender (after the fight in his arena)
- Emilitia
- Fluke Hermit
- Godseeker*
- Gravedigger
- Grasshopper (the ghosts next to Cornifer in QG)
- Hidden Moth (in the Shrine of Believers)
- Joni
- Mask Maker
- Marissa (<3)
- Menderbug Diary
- Midwife
- Millibelle*
- Moss Prophet
- Myla (Best girl <3)
- Poggy (Ghost in the small elevator room to Pleasure House)
- The White Lady
- Vespa (the ghost at Hive Knight)
- Willoh

Npc marked with "*" have special properties:
- Bretta: Her house unlocks after obtaining her DIALOGUE ITEM, not talking to her in Fungal Wastes.
- Godseeker: Requires a room reload after freeing her to be able to obtain the item.
- Millibelle: Will not talk to you if you have Grimmchild equip (Vanilla behaviour?)

**Ghost NPC are killable even if their check hasn't been obtained yet. Reloading the room will spawn a shiny in their place then.**

### Randomize Dream Nail Dialogue
Randomizes "major" dream nail dialogues with their locations (19 locations + 19 items)
This includes:
- Aspid Queen Corpse
- Ancient Nailsmith Golem
- Crystalized Shaman*
- Dashmaster Statue
- Dreamshield Statue (yes really)
- Dryyas Corpse
- Grimm Summoner Corpse*
- Hopper Dummy (next to Oro)
- Ismas Corpse
- Kings Mould Machine (in the hidden workshop)
- Mine Golem (which holds Crystal Heart)
- Overgrown Shaman
- Pale King*
- Radiance Statue (at the Top of Crystal Peak)
- Shade Golem* (with AND without Void Heart)
- Shriek Statue (By Abyss Shriek)
- Shroom "King" Corpse
- Snail Shaman Tomb (next to Soul Eater)

Locations with a "*" have special properties:
- Crystalized Shaman and Pale King: Spawn a shiny, if they have been destroyed. (Requires a room reload, if you forgot to check the dream nail dialogue first)
- Grimm Summoner Corpse: The nightmare lantern cannot be activated by normal means. Obtaining the Grimm summoner dream nail check, automatically unlocks Grimm (Grimmchild still works)
- Shade Golem: Has two checks, depending on if you have Void Heart or not. If you have Void Heart but not obtained the first one yet, the first one takes priority. It requires a room reload to obtain the second item then.

### Randomize Point of Interest
Adds additional checks to interesting point around Hallownest. (19 locations + 19 items)
This includes:
- City Fountain
- Beast Den "Altar" (?)
- Stag Nest Egg
- The Vitruvian Grub
- The dreamer tablet (which gives access to the dream nail sequence)
- The giant corpse in Queens Garden
- Traitors Grave
- Lore Tablet Record Bela (missing lore tablet, placed in soul sanctum... somewhere)
- Weaver Seal
- The machine in Grimms Tent
- Elder Hu Grave
- Xero Grave
- Marmu Grave
- White Palace Nursery
- No Eyes Statue
- Galien Corpse
- Markoth Corpse
- Gorb Grave
- Grimm Summoner Corpse

### Randomize Travellers
Randomizes Quirrel, Zote, Tiso, Cloth and Hornet encounters. (34 locations + 34 items)
If you choose "Vanilla" as the travel order, the npc will only appear once you collected enough of their dialogue checks (which exactly it is, doesn't matter)
This does include special encounters:
- Hornet will not appear in Kingdom's Edge.
- Zote will not appear in the colosseum.
- Quirrel will not appear in the Uumuu fight.
- Hornet will not appear in the THK fight, increasing true ending requirements by the Hornet stages.

### Randomize Elderbug Rewards
Adds a egg shop like location to Elderbug, which requires lore items to get the items.
A tablet is added next to Elderbug to preview the items he has.
As "lore items" count every item from the pools above and the normal lore tablets/custom lore.
**You can only activate this if at least one pool of the above is randomized!**

### Randomize Shrine of Believers
Adds up to 28 lore tablets in the Shrine of Believers which can be destroyed to obtain their item once their condition is met.
These are bingo like goals that the player must solve. The reward is displayed when reading.
Activating this will likely scarce shops.
**Does NOT add items by itself!**

### Cursed Reading
Randomizes the ability to read lore tablets includes the colo entry tablets as well as all "Point of Interest" locations and Menderbugs Diary.
The mod tries to display a message that the player can't read when they try. If that fails the interaction is removed entirely.

If you use "Randomize Shrine of Believers", the locations are only in logic if you have read even though you can obtain the items regardless if you have it or not.

### Cursed Listening
Randomizes the ability to talk to npc and therefore travelling with the stag and entering shops.
The mod tries to display a message that the player can't listen when they try. If that fails the interaction is removed entirely.
**If you kill a ghost npc before obtaining their check, the shiny will only spawn if you have this ability!**

**Currently it is possible to talk to Iselda once, because of unknown reason**

## Other Mods

### Lore Master
This mod is the (rando only) successor to Lore Master and not compatible with it. If you have played with it before, this mods checks may seem familiar. Here is a list of changes that the mod has to lore master:
- Powers, the treasures and the black egg temple condition have been removed completely. Although the power names will still be displayed for certain checks.
- Define Refs was also removed.
- The option to use the custom lore sprites (by kadala <3) and their name is now an extra option.
- Bretta instead of her diary is randomized. (And her check control if her house is open or not.)
- Godseeker, Millibelle and the Hidden Moth are new in the Randomize NPC pool.
- Randomize Dream Warrior location has merged with Randomize Point of Interest.
- Elderbug Rewards now function like the egg shop and does no longer add items.
- Myla is now killable after obtaining her item :c
- Ghosts and the crystal shaman can be "killed" regardless if you have obtained their check. Reentering the room will spawn the item back (if you've met the condition).
- Menderbug is no longer invincible :c
- The traveller order "shuffled" is removed and "Everywhere" got renamed to "None".
- Hornet stages are added to "Randomize Traveller".
- Traveller order "vanilla" now affects special encounter as well.

### Curse Randomizer
- The reading and listening ability items can be mimicked by curses.

### Condensed Spoiler Logger
- Adds the reading and listening ability to the log if used.
- Adds the traveller item locations to the log if randomize traveller is used and the travel order is "Vanilla".

### RandoSettingsManager
- Supported.
