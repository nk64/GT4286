# Retro Arcade Game Console (GT-4286)

Hopefully I will get my thoughts together and write up what I have learnt about this product through reverse engineering some of the emulators and exploring the SD card and the underlying OS (Linux).

Please backup your SD card before going any further.

## For now, you can find:

1. [dat](./dats/) files for the FBNeo emulator.

2. a [screenshot](./img/fbneo-hexedit-fix-p1-r-and-l-buttons.png) of a couple of bytes to patch to enable the P1-R and P1-L buttons to work in the FBNeo emulator. 
This screenshot show the changes to be made to ```/emus/fbneo/fbneo```.
Please check the SHA-256 hash of the file first and if it is not ```24B4026764C6B8F218D56222A534EDB3D21210A2C8FCDEE8A5F96505D6B85E9F``` then please don't perform this edit.

3. a [remap](./sdcard_tweaks/keyremap) file for the (patched) FBNeo emulator that orders the 6 buttons (of each player) correctly for Street Fighter 2 (maybe more).

4. some [information](./sdcard_tweaks/roms/MAME/output/) extracted from the running system

5. a [list](./dats/built-in%20roms.txt) of the built-in games.

6. some details and pictures of the [hardware](./Hardware.md)

7. some details of the [emulators](./Emulators.md)

8. some info about the [generations](./Generations.md) of SD Cards

9. [hardware mods](./Hardware%20Modifications.md) that people have done

10. [UI mods](./UI%20Modifications.md) (Background music, Font, Menu text etc)

11. [GT4286Uitl](https://github.com/nk64/GT4286/releases) which can currently:
    - identify the generation of an SD Card
    - refresh the game database from built-in and downloaded roms (you can add roms to the builtin /rom folder if you like)

12. a list of [known Issues](./Known%20Issues.md) with the console out of the box


## In the future I hope to show how to:

1. Fix the issue of FBNeo arcade games being listed as their cryptic (upto) 8 character rom names and give them human readable names..

2. replace the mame emulator (because it is an exact dupe of the fbneo emulator) with a script that can run other scripts to extract information from the running system such as:
    * directory listing to see what programs and files are available in the Linux OS
    * getting the FBNeo emulator to dump its dat files
    * process list
    * debugging information


## Exploring the OS

The key to exploring the running OS of the system (if you care) is to replace one of the emulator binaries eg ```/emus/mame/fbneo``` (because it is a duplicate emulator) with a bash style (actually [mksh](https://www.mirbsd.org/mksh.htm)) shell script.

```
#!/bin/sh
logfile="$0.log"; log () { echo $1 >> $logfile && /bin/sync; }

log "Current Working Directory: $PWD"
filename=$1
log "Rom: $filename"
```

and then loading up any rom in that emulator which will cause the script to be run instead.

Note:
* line endings must be Linux style ```LF``` not Windows Style ```CRLF```
* call ```/bin/sync``` liberally otherwise file output might not be flushed to the SD Card before you turn it off
* you can check then check the output of this script eg ```/emus/mame/fbneo.log```
* the current working directory is the emulator's folder eg ```/emu/mame```
* the path to the SDcard is ```/mnt/extsd/```
