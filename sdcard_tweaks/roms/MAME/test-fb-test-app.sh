#!/bin/sh

#region Boilerplate Script Header
this_file=${0}
output_dir="${GT4286UTIL_HOME}/output"
mkdir -p "${output_dir}"
logfile="${output_dir}/log.txt"; log () { echo "${this_file}: ${1}" >> "${logfile}" && /bin/sync; }
cat /dev/urandom > /dev/fb0
#endregion Boilerplate Script Header

log "Test fb-test-app"

byobb="${GT4286UTIL_HOME}/bin/busybox_arm"

if [ ! -f "${byobb}" ]; then
    log "The built-in busybox doesn't have this command."
    log "Try: https://github.com/EXALAB/Busybox-static/tree/main/busybox_arm"
    log "Put it in: ${GT4286UTIL_HOME}/bin/busybox_arm"
else
    log "BYO Busybox found: ${byobb}"

    appdir="${GT4286UTIL_HOME}/bin/fb-test-app"

    log "fill framebuffer with red"
    "${appdir}/fb-test" -r >> "${output_dir}/test-fb-test-app.txt" 2>&1 # fill framebuffer with red
    ${byobb} sleep 1

    cat /dev/urandom > /dev/fb0

    log "fill framebuffer with green"
    "${appdir}/fb-test" -g >> "${output_dir}/test-fb-test-app.txt" 2>&1 # fill framebuffer with green
    ${byobb} sleep 1

    cat /dev/urandom > /dev/fb0

    log "fill framebuffer with blue"
    "${appdir}/fb-test" -b >> "${output_dir}/test-fb-test-app.txt" 2>&1 # fill framebuffer with blue
    ${byobb} sleep 1

    cat /dev/urandom > /dev/fb0

    log "fill framebuffer with white"
    "${appdir}/fb-test" -w >> "${output_dir}/test-fb-test-app.txt" 2>&1 # fill framebuffer with white
    ${byobb} sleep 1

    cat /dev/urandom > /dev/fb0

    log "fill framebuffer with pattern 5"
    "${appdir}/fb-test" -p 5 >> "${output_dir}/test-fb-test-app.txt" 2>&1 # fill framebuffer with pattern
    ${byobb} sleep 1

    cat /dev/urandom > /dev/fb0

    # log "fill framebuffer with rectangles"
    # "${appdir}/rect"  > "${output_dir}/test-fb-test-app.txt" 2>&1
    # ${byobb} sleep 1

    log "hello world"
    "${appdir}/fb-string" 0x20 0x20 "hello world" 0xffff0000 0xff00ff00 >> "${output_dir}/test-fb-test-app.txt" 2>&1

    ${byobb} sleep 5
fi


