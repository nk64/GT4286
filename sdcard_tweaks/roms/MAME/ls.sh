#!/bin/sh
mkdir /mnt/extsd/roms/MAME/output
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }

log "Dumping a directory listing"
ls -lAF -R / > /mnt/extsd/roms/MAME/output/ls.txt && /bin/sync
