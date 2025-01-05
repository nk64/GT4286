# Emulators - FBNeo

As well as emulating arcade games, FBNeo supports some consoles. The list of games supported by each console is hardcoded into the emulator so the rom files must be named correctly with the correct contents.
Use the `dat` files from [here](./dats/fbneo%20dat%20files/) and a rom manger to straighten out your roms before continuing.

You will need to know the `prefixcode` for the console system:
* ColecoVision: `cv`
* Sony MSX: `msx`
* SNK Neo Geo Pocket: `ngp`
* SNK Neo Geo Pocket Color: `ngpc`
* Nintendo Entertainment System: `nes`
* Nintendo Famicom Disk System: `fds`
* NEC PC Engine: `pce`
* NEC PC Engine SuperGrafx: `sgx`
* Sega Game Gear: `gg`
* Sega Master System: `sms`
* Sega MegaDrive (AKA Sega Genesis): `md`
* Sega SG-1000: `sg1k`
* Sinclair ZX Spectrum: `spec`
* NEC TurboGrafx-16: `tg`


The original FBNeo emulator requires you to run the emulator like so: `fbneo {prefix}_{gamename}` where `gamename` is the zip file without the `zip|7z` extension.

But the GT4286 fbneo is run like: `fbneo {romdir}/{gamename}.zip` eg `fbneo /roms/FBA/sf2.zip`.  
Internally the emulator splits the single parameter up into the `romdir=/roms/FBA` and `gamename=sf2`.  
It adds `{romdir}` to it's list of places to search for files and then starts the emulation with the game driver for `{gamename}`. The emulator then looks for `{gamename}.zip` (or `.7z`) in `{romdir}`.

We will need to work hard to trick fbneo into loading console games.


## The lesser way (proof of concept)
This procedure only requires that you can run `GT4286Util refresh-gamedb <path-to-card>`

1. For example, picking a console and game at random:  
    Console: ColecoVision (`cv`)  
    Game Name: `heist`  
    Game Description: `The Heist`  
    Rom File: `heist.zip`  

2. put the ColecoVision rom `heist.zip` in to `/roms/FBA`
3. create an empty file called `cv_heist.zip` in `/roms/FBA`
4. run `GT4286Util refresh-gamedb <path-to-card>`
5. You will find in your game list two entries
    - `cv_heist`
    - `heist`
6. If you select `cv_heist` FBNeo should load the ColecoVision version of `The Heist`.
7. The game entry `heist` doesn't do anything.

## The better way
This procedure required that the [Humanise Names](./Humanise%20Names.md) procedure has been applied and that you can run `GT4286Util refresh-gamedb <path-to-card>`

1. Using to ColecoVision game "The Heist" from the example in `The lesser way`  
    Console: ColecoVision (`cv`)  
    Game Name: `heist`  
    Game Description: `The Heist`  
    Rom File: `heist.zip`  

2. Make a new directory for the console code `/roms/FBA/redir/cv`.
3. Put the ColecoVision rom `heist.zip` in `/roms/FBA/redir/cv`.
4. Create a descriptive name file `[CV] The Heist.redir` in `/roms/FBA` with the single line contents:
    ```
    cv/cv_heist.zip
    ```
5. Note the deliberate discrepancy between the file name in the `.redir` file (`cv_heist.zip`) and the actual zip file (`heist.zip`) on disk.
6. Run `GT4286Util refresh-gamedb <path-to-card>`.
7. You will find in your game list the new entry:
    - `[CV] The Heist`
8. If you select `[CV] The Heist` FBNeo should load the ColecoVision version of `The Heist`.

