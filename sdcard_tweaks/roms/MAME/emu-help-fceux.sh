#!/bin/sh
mkdir /mnt/extsd/roms/MAME/output
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }

log "Dumping FCEux Help"
(cd /mnt/extsd/emus/fceux; /mnt/extsd/emus/fceux/fceux.bin > /mnt/extsd/roms/MAME/output/help_fceux.txt 2>&1 && /bin/sync)
