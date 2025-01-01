#!/bin/sh
mkdir /mnt/extsd/roms/MAME/output
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }

log "Dumping the process list"
/bin/ps > /mnt/extsd/roms/MAME/output/ps.txt && /bin/sync
