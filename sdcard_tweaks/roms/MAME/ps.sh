#!/bin/sh
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }
log "PS"
/bin/ps -aux > /mnt/extsd/roms/MAME/output/ps.txt && /bin/sync
