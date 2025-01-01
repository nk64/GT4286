#!/bin/sh
mkdir /mnt/extsd/roms/MAME/output
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }

log "Dumping FBNeo Help"
(cd /mnt/extsd/emus/fbneo; /mnt/extsd/emus/fbneo/fbneo.bin > /mnt/extsd/roms/MAME/output/help_fbneo.txt 2>&1 && /bin/sync)
