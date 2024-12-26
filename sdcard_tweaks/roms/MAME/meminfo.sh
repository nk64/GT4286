#!/bin/sh
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }
log "Memory Info"
cat /proc/meminfo > /mnt/extsd/roms/MAME/output/meminfo.txt && /bin/sync
