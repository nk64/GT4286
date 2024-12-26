#!/bin/sh

#echo "KMSG" >> /mnt/extsd/emus/gamebatte/log.txt && /bin/sync
#cat /dev/kmsg > /mnt/extsd/emus/gamebatte/kmsg.txt && /bin/sync
#echo "KMSG" >> /mnt/extsd/emus/gamebatte/log.txt && /bin/sync
#cat /proc/kmsg > /mnt/extsd/emus/gamebatte/kmsg.txt && /bin/sync

# echo "fbviewer" >> /mnt/extsd/emus/gamebatte/log.txt && /bin/sync
# /mnt/extsd/fbviewer /mnt/extsd/emus/gambatte/menubg.png  > /mnt/extsd/emus/gamebatte/fbviewer.txt 2> /mnt/extsd/emus/gamebatte/fbviewer.err
# /bin/sync

# echo "test_fb" >> /mnt/extsd/emus/gamebatte/log.txt && /bin/sync
# /bin/test_fb /dev/fb0 5000 0x123456 > /mnt/extsd/emus/gamebatte/test_fb.txt 2> /mnt/extsd/emus/gamebatte/test_fb.err
# /bin/sync

# Attempt to Run FBNeo's menu system (unfortunatly not compiled in)
#log "***** Running FBNeo Menu System"
#/mnt/extsd/emus/mame/fbneo.orig -menu -joy >> $logfile 2>&1 && /bin/sync
#log "***** Finished Running FBNeo Menu System"
