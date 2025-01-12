#!/bin/sh

#region Boilerplate Script Header
this_file=${0}
output_dir="${GT4286UTIL_HOME}/output"
mkdir -p "${output_dir}"
logfile="${output_dir}/log.txt"; log () { echo "${this_file}: ${1}" >> "${logfile}" && /bin/sync; }
cat /dev/urandom > /dev/fb0
#endregion Boilerplate Script Header

log "Dumping CPU Info"
cat /proc/cpuinfo > "${output_dir}/os-info-static-proc-cpuinfo.txt"; /bin/sync

log "Dumping Memory Info"
cat /proc/meminfo > "${output_dir}/os-info-static-proc-meminfo.txt"; /bin/sync

log "Dumping /proc/version"
cat /proc/version > "${output_dir}/os-info-static-proc-version.txt"; /bin/sync

log "Dumping /proc/mtd"
cat /proc/mtd > "${output_dir}/os-info-static-proc-mtd.txt"; /bin/sync

log "Dumping mount points"
/bin/mount > "${output_dir}/os-info-static-mount.txt"; /bin/sync

if false; then
    ## None of these result in any data yet

    log "About to dump many things in /proc/sys"
    contents=$(cat "${/sys/proc/kernel/osrelease}")
    log "/sys/proc/kernel/osrelease: ${contents}"

    contents=$(cat "${/proc/sys/kernel/domainname}")
    log "/proc/sys/kernel/domainname: ${contents}"

    contents=$(cat "${/proc/sys/kernel/hostname}")
    log "/proc/sys/kernel/hostname: ${contents}"

    contents=$(cat "${/proc/sys/kernel/modprobe}")
    log "/proc/sys/kernel/modprobe: ${contents}"

    contents=$(cat "${/proc/sys/kernel/modules_disabled}")
    log "/proc/sys/kernel/modules_disabled: ${contents}"

    contents=$(cat "${/proc/sys/kernel/osrelease}")
    log "/proc/sys/kernel/osrelease: ${contents}"

    contents=$(cat "${/proc/sys/kernel/ostype}")
    log "/proc/sys/kernel/ostype: ${contents}"

    contents=$(cat "${/proc/sys/kernel/version}")
    log "/proc/sys/kernel/version: ${contents}"

    contents=$(cat "${/sys/kernel/notes}")
    log "/sys/kernel/notes: ${contents}"
fi

byobb="${GT4286UTIL_HOME}/bin/busybox_arm"

if [ ! -f "${byobb}" ]; then
    log "The built-in busybox doesn't have these commands."
    log "Try: https://github.com/EXALAB/Busybox-static/tree/main/busybox_arm"
    log "Put it in: ${GT4286UTIL_HOME}/bin/busybox_arm"
else
    log "BYO Busybox found: ${byobb}"
    log "Running lsusb"
    ${byobb} lsusb > "${output_dir}/os-info-static-lsusb.txt" 2>&1; /bin/sync

    log "Running lsmod"
    ${byobb} lsmod > "${output_dir}/os-info-static-lsmod.txt" 2>&1; /bin/sync

    log "Dumping framebuffer settings"
    ${byobb} fbset > "${output_dir}/os-info-static-fbset.txt" 2>&1; /bin/sync
fi
