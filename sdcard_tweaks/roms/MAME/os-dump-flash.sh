#!/bin/sh

#region Boilerplate Script Header
this_file=${0}
output_dir="${GT4286UTIL_HOME}/output"
mkdir -p "${output_dir}"
logfile="${output_dir}/log.txt"; log () { echo "${this_file}: ${1}" >> "${logfile}" && /bin/sync; }
cat /dev/urandom > /dev/fb0
#endregion Boilerplate Script Header

byobb="${GT4286UTIL_HOME}/bin/busybox_arm"

dest="${output_dir}/os-dump-flash"
log "Dumping Flash to ${dest}"

if [ ! -f "${byobb}" ]; then
    log "The built-in busybox doesn't have this command."
    log "Try: https://github.com/EXALAB/Busybox-static/tree/main/busybox_arm"
    log "Put it in: ${GT4286UTIL_HOME}/bin/busybox_arm"
else
    log "BYO Busybox found: ${byobb}"
    log "Dumping MTD Devices"

    mkdir -p "${dest}"
    rm -f "${output_dir}/os-dump-flash.txt"

    devices=("0" "1" "2" "3" "4" "5" "6")
    for i in "${devices[@]}"; do
        log "Dumping MTD Device #${i} to "
        ${byobb} nanddump "/dev/mtd/mtd${i}" -f "${dest}/mtd${i}.img" >> "${output_dir}/os-dump-flash.txt" 2>&1; /bin/sync
    done
fi



