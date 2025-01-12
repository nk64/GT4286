# Exploring the OS

The steps below have been codified into the [Runtime Script Framework](./Runtime%20Script%20Framework.md).
Please follow the istructions there.

## Deprecated: Exploring the OS (the hard way)

The key to exploring the running OS of the system (if you care) is to replace one of the emulator binaries eg ```/emus/mame/fbneo``` with a bash style (actually [mksh](https://www.mirbsd.org/mksh.htm)) shell script and then loading up any rom in that emulator which will cause the script to be run instead.

```
#!/bin/sh
logfile="$0.log"; log () { echo $1 >> $logfile && /bin/sync; }
log "Current Working Directory: $PWD"
log "Rom: $1"
```

Note:
* line endings must be Linux style ```LF``` not Windows Style ```CRLF```
* call ```/bin/sync``` liberally otherwise file output might not be flushed to the SD Card before you turn it off
* you can check then check the output of this script eg ```/emus/mame/fbneo.log```
* the current working directory is the emulator's folder eg ```/emu/mame```
* the path to the SDcard is ```/mnt/extsd/```


## Deprecated: Exploring the OS like a Boss
1. Replace your MAME emulator binary (```/emus/mame/fbneo```) with the shell script found [here](./sdcard_tweaks/emus/mame)
2. Copy the premade shell scripts from [here](./sdcard_tweaks/roms/MAME/) to your ```/roms/MAME/``` directry.
3. Make sure you have this directory too: ```/roms/MAME/output```
4. Run the ```GT4286Util refresh-gamedb <path-to-card>``` command so that these new _rom/scripts_ show up in your game list.
5. Boot the console and run the scripts you are iterested in.
    - The menu appears to freeze for as long as the script takes to run
    - After the command has run there should be brief black screen and then it will come back to the menu
    - Some commands like ```ls``` and ```filecopy``` and ```fbneo-dat-files``` take a while to run

Script output will end up in ```/roms/MAME/output```.

## Current Scripts


### Linux/Android Debugging Commands
- `debug-dmesg`
- `debug-kmsg`
- `debug-env`
    - dump all the environment variables of the shell
- `debug-logcat`
- `debug-getprop`

### Static Info
- `os-info-static`  
    - ```cpuinfo```  
    - ```meminfo```  
    - ```mount```  

- `os-dump-flash`
    - Dump the Flash MTD devices to disk for further inspection

### Dynamic Info
- `os-info-dynamic`
    - ```ps```  
    - ```uptime```
    - ```df```  
- `os-dump-ls`
    - list the contents of the mounted file systems
- `os-dump-files`
    - Copy as much of the flash filesystem as we can.
        * Soft links don't survive the transition 

### Emulator Related
- `emu-help`
    - dump the command line help for emulators that provide it

### Other fun scripts
- `emu-fbneo-dat-files`
    - Cause the FBNeo emulator to dump out dat files for the systems it supports
- `boot-logo-disable` & `boot-logo-enable`
- `byo-busybox-help`
    - play around with a more fully features busybox
- `reboot`
    - Reboots the console
- `test-framebuffer`
- `zkdebug-disable` & `zkdebug-enable`
    - There seems to be a property that might disable debugging to some extent, this needs more investigation of the effects.

- `hello-world`
    - a simple `hello_world.c`
        ```c
        #include <stdio.h>
        int main()
        {
            printf("Hello, World!");
            return 0;
        }
        ```
    - Compiled with the Windows [Arm GNU Toolchain](https://developer.arm.com/downloads/-/arm-gnu-toolchain-downloads)
        - `arm-gnu-toolchain-14.2.rel1-mingw-w64-x86_64-arm-none-linux-gnueabihf.zip`

    1. `C:\arm-gnu-toolchain\bin>`arm-none-linux-gnueabihf-gcc.exe -static hello_world.c -o hello_world
    2. `C:\arm-gnu-toolchain\bin>`arm-none-linux-gnueabihf-strip.exe hello_world


