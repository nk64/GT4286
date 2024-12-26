#!/bin/sh
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }

log "Dumping temper Help"
(cd /mnt/extsd/emus/temper; /mnt/extsd/emus/temper/temper.bin > /mnt/extsd/roms/MAME/output/help_temper.txt 2>&1 && /bin/sync)
