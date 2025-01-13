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

    evtest="${GT4286UTIL_HOME}/bin/evtest"

    # From logcat:
    # D/zkgui   (  936): Open key:/dev/input/event0 
    # D/zkgui   (  936): open [/dev/input/event0] [12] sucess
    # D/zkgui   (  936): get mastrt fd [/dev/input/event0]
    # D/zkgui   (  936): Open key:/dev/input/event1 
    # D/zkgui   (  936): open [/dev/input/event1] [13] sucess
    # D/zkgui   (  936): Open key:/dev/input/event6 
    # D/zkgui   (  936): Open key:/dev/input/event8 
    # D/zkgui   (  936): Open key:/dev/input/event67 
    # D/zkgui   (  936): Open key:/dev/input/event68 
    # D/zkgui   (  936): Open key:/dev/input/event69 
    # D/zkgui   (  936): open [/dev/input/event69] [14] sucess
    # D/zkgui   (  936): get mastrt fd [/dev/input/event69]
    # D/zkgui   (  936): Open key:/dev/input/event70 
    # D/zkgui   (  936): open [/dev/input/event70] [15] sucess


    ${byobb} bzcat "${GT4286UTIL_HOME}/res/BGRAx1280x1440x32_blue.raw.bz2" > /dev/fb0

    "${evtest}" --grab /dev/input/event0 > "${output_dir}/test-evtest-event0.txt" 2>&1 &
    pid1=$!
    "${evtest}" --grab /dev/input/event1 > "${output_dir}/test-evtest-event1.txt" 2>&1 &
    pid2=$!
    "${evtest}" --grab /dev/input/event69 > "${output_dir}/test-evtest-event69.txt" 2>&1 &
    pid3=$!
    "${evtest}" --grab /dev/input/event70 > "${output_dir}/test-evtest-event70.txt" 2>&1 &
    pid4=$!

    ${byobb} bzcat "${GT4286UTIL_HOME}/res/BGRAx1280x1440x32_green.raw.bz2" > /dev/fb0

    log "Sleeping for 20"
    ${byobb} sleep 20
    /bin/sync

    ${byobb} bzcat "${GT4286UTIL_HOME}/res/BGRAx1280x1440x32_blue.raw.bz2" > /dev/fb0

    # kill %1 # The first job
    # kill %- # The last job
    # kill $! # As long as you don't launch any other process in the background you can use $! directly

    log "Kill event capture"
    kill -TERM ${pid1}
    kill -TERM ${pid2}
    kill -TERM ${pid3}
    kill -TERM ${pid4}
    /bin/sync
    
    ${byobb} bzcat "${GT4286UTIL_HOME}/res/BGRAx1280x1440x32_red.raw.bz2" > /dev/fb0
    log "Done"
fi