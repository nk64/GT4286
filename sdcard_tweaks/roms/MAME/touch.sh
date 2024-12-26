#!/bin/sh
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }
log "Touch"
/bin/touch /mnt/extsd/roms/MAME/output/touch.txt && /bin/sync

