# Capturing emulator output

1. Pick an [emulator](https://github.com/nk64/GT4286/blob/main/Emulators.md) to study eg ```/emus/gamebatte/gambatte_sdl``` (note that there is a misspelling in the directory name of this emulator) (also note that ```/emus/gbc``` is unused)
2. Rename the emulator by adding ```.bin``` to the filename eg. ```/emus/gamebatte/gambatte_sdl``` becomes ```/emus/gamebatte/gambatte_sdl.bin```
3. Then replace emulator with the shell script below.
4. After you play a game in that emulator you can find
    - the script output in eg. ```/emus/gamebatte/gambatte_sdl.log```
    - the emulator output in eg. ```/emus/gamebatte/gambatte_sdl.bin.log```

```
#!/bin/sh
logfile="$0.log"; log () { echo $1 >> $logfile && /bin/sync; }
wrapped_exe="$0.bin"
wrapped_exe_logfile="$wrapped_exe.log"
log_exe=true

log "---------------------------------------------------"
log "Arg0: $0"
log "ArgAll: $@"
log "Current Working Directory: $PWD"
log "Wrapped Exe: $wrapped_exe"
log "Executing wrapped exe: $wrapped_exe"

$wrapped_exe "$@" > $wrapped_exe_logfile 2>&1 && /bin/sync
```


Note:
* line endings must be Linux style ```LF``` not Windows Style ```CRLF```
* call ```/bin/sync``` liberally otherwise file output might not be flushed to the SD Card before you turn it off
* the current working directory is the emulator's folder eg ```/emu/mame```
* the path to the SDcard is ```/mnt/extsd/```
