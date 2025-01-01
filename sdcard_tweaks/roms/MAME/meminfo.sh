#!/bin/sh
mkdir /mnt/extsd/roms/MAME/output
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }

log "Dumping Memory Info"
cat /proc/meminfo > /mnt/extsd/roms/MAME/output/meminfo.txt && /bin/sync
