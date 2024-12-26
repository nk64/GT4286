#!/bin/sh
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }

log "Dumping PicoDrive Help"
(cd /mnt/extsd/emus/picodrive; /mnt/extsd/emus/picodrive/PicoDrive.bin > /mnt/extsd/roms/MAME/output/help_picodrive.txt 2>&1 && /bin/sync)
