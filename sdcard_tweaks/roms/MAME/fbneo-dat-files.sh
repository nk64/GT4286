#!/bin/sh
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }

### Dump FBNeo Data Files
log "Dumping FBNeo Data Files"

mkdir /mnt/extsd/roms/MAME/output/fbneo-dat-files
(cd /mnt/extsd/roms/MAME/output/fbneo-dat-files && /mnt/extsd/emus/fbneo/fbneo.bin -dat >> /mnt/extsd/roms/MAME/output/fbneo-dat-files.txt 2>&1 && /bin/sync)

