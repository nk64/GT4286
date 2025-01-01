#!/bin/sh
mkdir /mnt/extsd/roms/MAME/output
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }

log "Dumping CPU Info"
cat /proc/cpuinfo > /mnt/extsd/roms/MAME/output/cpuinfo.txt && /bin/sync
