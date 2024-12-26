#!/bin/sh
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }

log "Dumping gpSP Help"
(cd /mnt/extsd/emus/gpsp; /mnt/extsd/emus/gpsp/gpsp.bin > /mnt/extsd/roms/MAME/output/help_gpsp.txt 2>&1 && /bin/sync)
