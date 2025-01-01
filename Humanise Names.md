# Humanise FBNeo Arcade Game Names

Roms for all emulators other than FBNeo/MAME will show up in the game list as whatever name the rom file name is (minus the file extension).

For example the ```/roms/FC/Carpenter brother (thump-thump).nes``` will show up in the game menu as ```Carpenter brother (thump-thump)``` and if you don't like the name you can rename the file to eg. ```/roms/FC/Carpenter Brother.nes``` and after refreshing the game db using ```GT4286Util``` it will appear as ```Carpenter Brother```.

The names of those roms will come down to personal choice or whatever rom management tool you choose to use to curate your rom collection and are out of scope for ```GT4286Util```.

Arcade games are made up of many rom chips and the standard packaging method is Zip or 7z archive files. These are typically named compactly (and crypticly) in DOS 8.3 format eg ```4in1.zip```, ```b2b.zip```, ```whp.zip```

The list of supported game names are hardcoded into emulators like FBNeo and MAME and they cannot be freely changed.

For example:
|     File Name      | Game Name |                 Game Description                 |
|--------------------|-----------|--------------------------------------------------|
| /roms/FBA/4in1.zip | 4in1      | 4 Fun in 1 [Bootleg]                             |
| /roms/FBA/b2b.zip  | b2b       | Bang Bang Busters (2010 NCI release) [Prototype] |
| /roms/FBA/whp.zip  | whp       | World Heroes Perfect                             |

The human readable and descriptive name is also hardcoded in the emulator and would normally be shown by the emulator's menuing system 
But the GT4286 unfortunatly only shows the cryptic name in the game list.

I had hoped that the ```fileenglishscan``` column in the game.db would be a good place to add the descriptive name, but that only influences searching eg now you could search for ```buster``` and get ```Bang Bang Busters```. But what is shown in the game list is the original cryptic name ```b2b```


## Rube Goldberg Solution - Manual Steps (until GT4286Util is improved)
⚠️ Backup your SD Card before proceding!  
⚠️ Read all the steps before starting so you know what you are in for.  

1. Rename the FBNeo emulator by adding ```.bin``` to the filename eg. ```/emus/fbneo/fbneo``` becomes ```/emus/fbneo/fbneo.bin```

2. Copy the shell script ```fbneo.script_with_rom_name_redirection``` found [here](./sdcard_tweaks/emus/fbneo) into the ```/emus/fbneo``` directory and rename it to ```fbneo```.  
The end result is that you must have the following two files:
    - ```/emus/fbneo/fbneo.bin``` (The real emulator)
    - ```/emus/fbneo/fbneo``` (The shell script, that does some manipulation of the rom parameter and then calls the real emulator)

3. Make a new directory ```/roms/FBA/redir```

4. Move ```/roms/FBA/b2b.zip``` to ```/roms/FBA/redir```

5. Create a text file called ```/roms/FBA/Bang Bang Busters (2010 NCI release) [Protoype].redir``` containing *only* the original zip file name:
    ```b2b.zip```

6. Rename the associated image ```/roms/FBA/image/b2b.jpg``` to match the new name ```/roms/FBA/image/Bang Bang Busters (2010 NCI release) [Protoype].jpg```

7. Run the ```GT4286Util refresh-gamedb <path-to-card>``` command so that the new _'rom'_ shows up in your game list.

8. Boot the console and:
    - confirm that in the FBA class you no longer have ```b2b``` but instead have ```Bang Bang Busters (2010 NCI release) [Protoype]```.
    - confirm that the screenshot is displayed
    - confirm that the game runs
    - confirm that other games that haven't been redirected still work
    - confirm that you can search for ```busters```

9. Now repeat Steps 4, 5 and 6 for all the files that you care about in ```/roms/FBA``` followed finally by Step 7.


## Semi Automated Method - 2 Manual Steps + GT4286Util doing the bulk of the work
⚠️ Backup your SD Card before proceding!  
⚠️ Read all the steps before starting so you know what you are in for.  

1. Rename the FBNeo emulator by adding ```.bin``` to the filename eg. ```/emus/fbneo/fbneo``` becomes ```/emus/fbneo/fbneo.bin```

2. Copy the shell script ```fbneo.script_with_rom_name_redirection``` found [here](./sdcard_tweaks/emus/fbneo) into the ```/emus/fbneo``` directory and rename it to ```fbneo```.  
The end result is that you must have the following two files:
    - ```/emus/fbneo/fbneo.bin``` (The real emulator)
    - ```/emus/fbneo/fbneo``` (The shell script, that does some manipulation of the rom parameter and then calls the real emulator)

3. Run ```GT4286Util humanise-game-names <path-to-card>``` to humanise your ```/roms/FBA``` folder (```/roms/MAME```, ```/download/FBA``` and ```/download/MAME``` are currently not supported)

4. Run the ```GT4286Util refresh-gamedb <path-to-card>``` command so that the new _'roms'_ show up in your game list.

