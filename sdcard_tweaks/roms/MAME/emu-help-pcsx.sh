#!/bin/sh
mkdir /mnt/extsd/roms/MAME/output
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }

log "Dumping pcsx Help"
(cd /mnt/extsd/emus/pcsx4all; /mnt/extsd/emus/pcsx4all/pcsx.bin > /mnt/extsd/roms/MAME/output/help_pcsx.txt 2>&1 && /bin/sync)
