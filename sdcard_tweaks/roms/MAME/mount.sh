#!/bin/sh
mkdir /mnt/extsd/roms/MAME/output
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }

log "Dumping mount points"
/bin/mount > /mnt/extsd/roms/MAME/output/mount.txt && /bin/sync
