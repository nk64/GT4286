#!/bin/sh
mkdir /mnt/extsd/roms/MAME/output
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }

log "Touching a file"
/bin/touch /mnt/extsd/roms/MAME/output/touch.txt && /bin/sync

