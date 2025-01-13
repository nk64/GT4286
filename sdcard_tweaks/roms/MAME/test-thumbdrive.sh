#!/bin/sh

#region Boilerplate Script Header
this_file=${0}
output_dir="${GT4286UTIL_HOME}/output"
mkdir -p "${output_dir}"
logfile="${output_dir}/log.txt"; log () { echo "${this_file}: ${1}" >> "${logfile}" && /bin/sync; }
cat /dev/urandom > /dev/fb0
#endregion Boilerplate Script Header


byobb="${GT4286UTIL_HOME}/bin/busybox_arm"

if [ ! -f "${byobb}" ]; then
    log "The built-in busybox doesn't have this command."
    log "Try: https://github.com/EXALAB/Busybox-static/tree/main/busybox_arm"
    log "Put it in: ${GT4286UTIL_HOME}/bin/busybox_arm"
else
    log "BYO Busybox found: ${byobb}"

    cat /proc/partitions >> "${output_dir}/test-thumbdrive-partitions.txt" 2>&1; /bin/sync
    ${byobb} blkid >> "${output_dir}/test-thumbdrive-lsblk.txt" 2>&1; /bin/sync

    log "Mounting thumb drive"
    {
        mkdir /tmp/exthdd
        mount /dev/block/sda /tmp/exthdd
        ls -laF /tmp/exthdd
        cat /tmp/exthdd/test.txt
        umount /tmp/exthdd
        rm -rf /tmp/exthdd
    }  >> "${output_dir}/test-thumbdrive-mount.txt" 2>&1; /bin/sync

    log "Done"
fi