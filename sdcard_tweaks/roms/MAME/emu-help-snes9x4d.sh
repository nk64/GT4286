#!/bin/sh
mkdir /mnt/extsd/roms/MAME/output
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }

log "Dumping snes9x4d Help"
(cd /mnt/extsd/emus/snes9x4d; /mnt/extsd/emus/snes9x4d/snes9x4d.dge.bin > /mnt/extsd/roms/MAME/output/help_snes9x4d.txt 2>&1 && /bin/sync)
