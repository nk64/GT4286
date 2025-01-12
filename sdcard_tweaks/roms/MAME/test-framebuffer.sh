#!/bin/sh

#region Boilerplate Script Header
this_file=${0}
output_dir="${GT4286UTIL_HOME}/output"
mkdir -p "${output_dir}"
logfile="${output_dir}/log.txt"; log () { echo "${this_file}: ${1}" >> "${logfile}" && /bin/sync; }
#endregion Boilerplate Script Header

log "Random into Framebuffer"

byobb="${GT4286UTIL_HOME}/bin/busybox_arm"

if [ ! -f "${byobb}" ]; then
    log "The built-in busybox doesn't have this command."
    log "Try: https://github.com/EXALAB/Busybox-static/tree/main/busybox_arm"
    log "Put it in: ${GT4286UTIL_HOME}/bin/busybox_arm"
else
    log "BYO Busybox found: ${byobb}"

    log "Snapshot current Framebuffer"
    cat /dev/fb0 > "${GT4286UTIL_HOME}/res/dump_of_fb.raw"

    cat /dev/urandom > /dev/fb0
    ${byobb} sleep 1

    log "Restore Framebuffer snapshot"
    cat "${GT4286UTIL_HOME}/res/dump_of_fb.raw" > /dev/fb0
    ${byobb} sleep 1

    cat /dev/urandom > /dev/fb0
    ${byobb} sleep 1

    ${byobb} bzcat "${GT4286UTIL_HOME}/res/BGRAx1280x1440x32_red.raw.bz2" > /dev/fb0
    ${byobb} sleep 1
    ${byobb} bzcat "${GT4286UTIL_HOME}/res/BGRAx1280x1440x32_green.raw.bz2" > /dev/fb0
    ${byobb} sleep 1
    ${byobb} bzcat "${GT4286UTIL_HOME}/res/BGRAx1280x1440x32_blue.raw.bz2" > /dev/fb0

    cat /dev/urandom > /dev/fb0
    #${byobb} sleep 5
fi


