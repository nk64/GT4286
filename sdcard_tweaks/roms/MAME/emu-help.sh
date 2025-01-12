#!/bin/sh

#region Boilerplate Script Header
this_file=${0}
output_dir="${GT4286UTIL_HOME}/output"
mkdir -p "${output_dir}"
logfile="${output_dir}/log.txt"; log () { echo "${this_file}: ${1}" >> "${logfile}" && /bin/sync; }
cat /dev/urandom > /dev/fb0
#endregion Boilerplate Script Header

cat /dev/urandom > /dev/fb0

log "Dumping FBNeo Help"
(cd /mnt/extsd/emus/fbneo && /mnt/extsd/emus/fbneo/fbneo.bin > "${output_dir}/help_fbneo.txt" 2>&1; /bin/sync)

cat /dev/urandom > /dev/fb0
log "Dumping Gambatte Help"
(cd /mnt/extsd/emus/gamebatte && /mnt/extsd/emus/gamebatte/gambatte_sdl.bin > "${output_dir}/help_gambatte.txt" 2>&1; /bin/sync)

cat /dev/urandom > /dev/fb0
log "Dumping snes9x4d Help"
(cd /mnt/extsd/emus/snes9x4d && /mnt/extsd/emus/snes9x4d/snes9x4d.dge.bin > "${output_dir}/help_snes9x4d.txt" 2>&1; /bin/sync)


## The following emulators don't report anything useful as far as command line help
cat /dev/urandom > /dev/fb0
log "Dumping FCEux Help"
(cd /mnt/extsd/emus/fceux && /mnt/extsd/emus/fceux/fceux.bin > "${output_dir}/help_fceux.txt" 2>&1; /bin/sync)

cat /dev/urandom > /dev/fb0
if false; then
    ## The following emulators don't report anything useful as far as command line help
    ## furthermore they end up in the GUI menu and you need to close them again
    log "Dumping gpSP Help"
    (cd /mnt/extsd/emus/gpsp && /mnt/extsd/emus/gpsp/gpsp.bin > "${output_dir}/help_gpsp.txt" 2>&1; /bin/sync)

    log "Dumping pcsx Help"
    (cd /mnt/extsd/emus/pcsx4all && /mnt/extsd/emus/pcsx4all/pcsx.bin > "${output_dir}/help_pcsx.txt" 2>&1; /bin/sync)

    log "Dumping PicoDrive Help"
    (cd /mnt/extsd/emus/picodrive && /mnt/extsd/emus/picodrive/PicoDrive.bin > "${output_dir}/help_picodrive.txt" 2>&1; /bin/sync)

    log "Dumping temper Help"
    (cd /mnt/extsd/emus/temper && /mnt/extsd/emus/temper/temper.bin > "${output_dir}/help_temper.txt" 2>&1; /bin/sync)
fi




