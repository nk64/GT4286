#!/bin/sh
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }

log "Dumping Gambatte Help"
(cd /mnt/extsd/emus/gamebatte; /mnt/extsd/emus/gamebatte/gambatte_sdl.bin > /mnt/extsd/roms/MAME/output/help_gambatte.txt 2>&1 && /bin/sync)
