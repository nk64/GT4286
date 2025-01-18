#!/bin/sh

#region Boilerplate Script Header
this_file=${0}
output_dir="${GT4286UTIL_HOME}/output"
mkdir -p "${output_dir}"
logfile="${output_dir}/log.txt"; log () { echo "${this_file}: ${1}" >> "${logfile}" && /bin/sync; }
cat /dev/urandom > /dev/fb0
#endregion Boilerplate Script Header

log "Test overclocking to 1000"

byobb="${GT4286UTIL_HOME}/bin/busybox_arm"

if [ ! -f "${byobb}" ]; then
    log "The built-in busybox doesn't have this command."
    log "Try: https://github.com/EXALAB/Busybox-static/tree/main/busybox_arm"
    log "Put it in: ${GT4286UTIL_HOME}/bin/busybox_arm"
else
    log "BYO Busybox found: ${byobb}"

    appdir="${GT4286UTIL_HOME}/bin/fb-test-app"

    cpuclock="${GT4286UTIL_HOME}/bin/cpuclock"
    
    ${cpuclock} 1000 > "${output_dir}/overclock.txt" 2>&1

    results=$(cat "${output_dir}/overclock.txt")

    "${appdir}/fb-string" 0x00 0x00 "${results}" 0xffffffff 0xff000000 > /dev/null 2>&1

    ${byobb} sleep 10
fi

