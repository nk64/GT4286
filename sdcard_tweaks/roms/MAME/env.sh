#!/bin/sh
mkdir /mnt/extsd/roms/MAME/output
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }

### Dump Environment and Shell Variables
log "Dumping Envinoment and Shell Variables"
set > /mnt/extsd/roms/MAME/output/env.txt 2>&1 && /bin/sync

