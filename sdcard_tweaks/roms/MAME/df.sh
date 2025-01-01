#!/bin/sh
mkdir /mnt/extsd/roms/MAME/output
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }

log "DF"
/bin/df > /mnt/extsd/roms/MAME/output/df.txt && /bin/sync
