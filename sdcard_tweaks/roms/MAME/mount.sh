#!/bin/sh
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }
log "Mount"
/bin/mount > /mnt/extsd/roms/MAME/output/mount.txt && /bin/sync
