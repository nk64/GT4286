#!/bin/sh
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }
log "LS"
ls -lAF -R / > /mnt/extsd/roms/MAME/output/ls.txt && /bin/sync
