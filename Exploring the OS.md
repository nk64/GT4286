
## Exploring the OS (fundamentals)

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


## Exploring the OS like a boss
1. Replace your MAME emulator binary (```/emus/mame/fbneo```) with the shell script found [here](./sdcard_tweaks/emus/mame)
2. Copy the premade shell scripts from [here](./sdcard_tweaks/roms/MAME/) to your ```/roms/MAME/``` directry.
3. Make sure you have this directory too: ```/roms/MAME/output```
4. Run the ```GT4286Util refreshgamedb <path-to-sd-card>``` command so that these new _rom/scripts_ show up in your game list.
5. Boot the console and run the scripts you are iterested in.
    - The menu appears to freeze for as long as the script takes to run
    - After the command has run there should be brief black screen and then it will come back to the menu
    - Some commands like ```ls``` and ```filecopy``` and ```fbneo-dat-files``` take a while to run

Script output will end up in ```/roms/MAME/output```.

## Current Scripts

### Standard Linux Commands
```cpuinfo```  

```meminfo```  

```mount```  

```ps```  

```df```  

```env```  
dump all the environment variables of the shell

```ls```  
list the contents of the mounted file systems

```touch```  
create a file if it doesn't exist. This is not particularly useful but it was the first command I ran blind before I knew what binaries we had available to us.

### Emulator command line help
```emu-help-fbneo```  

```emu-help-fceux```  

```emu-help-gambatte```  

```emu-help-pcsx```  

```emu-help-snes9x4d```  


### Other fun scripts
```fbneo-dat-files```  
Cause the FBNeo emulator to dump out dat files for the systems it supports

```filecopy```  
Copy as much of the flash filesystem as we can.
* Soft links don't survive the transition 
* Refer to the output of ```ls``` for the available ```busybox``` commands

```logcat```  
some interesting debuging info