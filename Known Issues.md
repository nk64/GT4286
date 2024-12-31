# Known issues with the stock console


## There is no MAME
MAME is shown as a separate emulator but it is just a copy of the FBNeo (aka FBA) emulator.  
(let's repurpose it)


## Non working game entries appear in the Download tab
If you add roms to the appropriate emulator sub dir in /downloads, duplicate entries for some games might appear after saving a game and rebooting the console.

This is because while most emulators save their game state in the emulator folder, some emulators eg GB, GBC, MD save their state in the rom folder and the next time the console is rebooted it will rescan the download folder and add a non working entry to the list. You will end up with a non working entry for every save state slot you use.


## FBNeo/MAME ```P1-L``` and ```P1-R``` buttons don't work
FBNeo emulator (and it's clone MAME) have a misconfiguration which means that the ```P1-L``` and ```P1-R``` buttons have no effect, which impacts many fighting games.  
(this is fixable by patching emulator and adding a keyremap files)


## FBNeo//MAME Arcade roms names are cryptic
FBNeo games are listed as their cryptic rom .zip file name which is ugly by comparison to the other emulators which can have nice human readable names. (See page 10 in the [manual](https://media.jaycar.com.au/product/resources/GT4286_manualMain_130153.pdf) for an example of this).  
(we can do something about this)


## Emulators are stretched to full screen
Most emulators are fully stretched to fill the screen. It would be nice to be able to control the scaling especially for Gameboy games.  
(no luck so far)


## Downloaded games don't load via the History and Collection tabs
If you add games to the ```/download``` folders and then play them and add them to your Collection when you select them from the Collection or History tabs they don't load. This is not a problem for the builtin games.