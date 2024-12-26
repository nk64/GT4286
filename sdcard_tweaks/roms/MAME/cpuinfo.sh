#!/bin/sh
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }
log "Dump CPU Info"
cat /proc/cpuinfo > /mnt/extsd/roms/MAME/output/cpuinfo.txt && /bin/sync
