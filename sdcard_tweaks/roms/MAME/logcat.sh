#!/bin/sh
mkdir /mnt/extsd/roms/MAME/output
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }

log "Running LogCat"
/bin/logcat -d > /mnt/extsd/roms/MAME/output/logcat.txt && /bin/sync
