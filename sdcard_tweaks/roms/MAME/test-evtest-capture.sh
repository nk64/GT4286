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

    for file_path in /dev/input/event*; do
        file_name="${file_path##*/}"
        log "Capture events from  '${file_path}' to '${output_dir}/test-evtest-${file_name}.txt'"
        "${evtest}" --grab "${file_path}" > "${output_dir}/test-evtest-${file_name}.txt" 2>&1 &
    done

    ${byobb} bzcat "${GT4286UTIL_HOME}/res/BGRAx1280x1440x32_green.raw.bz2" > /dev/fb0

    log "Sleeping for 20"
    ${byobb} sleep 20
    /bin/sync

    ${byobb} bzcat "${GT4286UTIL_HOME}/res/BGRAx1280x1440x32_blue.raw.bz2" > /dev/fb0

    ## Job Control ##
    # kill %1 # The first job
    # kill %- # The last job
    # kill $! # As long as you don't launch any other process in the background you can use $! directly
    # JOBS=$(jobs -p)
    # for pid in "${JOBS[@]}"; do
    #     kill -TERM "${pid}";
    # done


    JOBS=$(jobs -p)
    #log "Kill event capture Jobs: ${JOBS[*]}"

    for pid in ${JOBS}; do
        log "Kill event capture Job: ${pid}"
        kill -TERM "${pid}";
    done
    ${byobb} sleep 1

    JOBS=$(jobs -p)
    for pid in ${JOBS}; do
        log "Unkilled Job: ${pid}"
    done

    /bin/sync
    
    ${byobb} bzcat "${GT4286UTIL_HOME}/res/BGRAx1280x1440x32_red.raw.bz2" > /dev/fb0
    log "Done"

fi