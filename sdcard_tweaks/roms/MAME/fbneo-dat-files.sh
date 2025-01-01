#!/bin/sh
mkdir /mnt/extsd/roms/MAME/output
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }

### Dump FBNeo Data Files
log "Dumping FBNeo Data Files"

potential_exe=(
    "/mnt/extsd/emus/fbneo/fbneo.bin"
    "/mnt/extsd/emus/fbneo/fbneo"
)

exe=""; for i in "${potential_exe[@]}"; do if [[ -f $i ]]; then exe="$i"; break; fi; done

if [[ "$exe" == "" ]];
then
    log "No emulator found. Bailing out";
    exit 1;
fi

mkdir /mnt/extsd/roms/MAME/output/fbneo-dat-files
(cd /mnt/extsd/roms/MAME/output/fbneo-dat-files && $exe -dat >> /mnt/extsd/roms/MAME/output/fbneo-dat-files.txt 2>&1 && /bin/sync)
