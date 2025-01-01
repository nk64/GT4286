#!/bin/sh
mkdir /mnt/extsd/roms/MAME/output
logfile="/mnt/extsd/roms/MAME/output/log.txt"; log () { echo $1 >> $logfile && /bin/sync; }


log "FileCopy"

mkdir /mnt/extsd/roms/MAME/output/filecopy >> $logfile
mkdir /mnt/extsd/roms/MAME/output/filecopy/bin >> $logfile
mkdir /mnt/extsd/roms/MAME/output/filecopy/etc >> $logfile
mkdir /mnt/extsd/roms/MAME/output/filecopy/config >> $logfile
mkdir /mnt/extsd/roms/MAME/output/filecopy/lib >> $logfile
mkdir /mnt/extsd/roms/MAME/output/filecopy/overlay >> $logfile
mkdir /mnt/extsd/roms/MAME/output/filecopy/sbin >> $logfile
mkdir /mnt/extsd/roms/MAME/output/filecopy/tmp >> $logfile

cp -R /bin/* /mnt/extsd/roms/MAME/output/filecopy/bin >> $logfile
cp -R /etc/* /mnt/extsd/roms/MAME/output/filecopy/etc >> $logfile
cp -R /config/* /mnt/extsd/roms/MAME/output/filecopy/config >> $logfile
cp -R /data/* /mnt/extsd/roms/MAME/output/filecopy/data >> $logfile
cp -R /system/* /mnt/extsd/roms/MAME/output/filecopy/system >> $logfile
cp -R /lib/* /mnt/extsd/roms/MAME/output/filecopy/lib >> $logfile
cp -R /overlay/* /mnt/extsd/roms/MAME/output/filecopy/overlay >> $logfile
cp -R /sbin/* /mnt/extsd/roms/MAME/output/filecopy/sbin >> $logfile
cp -R /tmp/* /mnt/extsd/roms/MAME/output/filecopy/tmp >> $logfile

/bin/sync