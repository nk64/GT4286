#!/bin/sh

#region Boilerplate Script Header
this_file=${0}
output_dir="${GT4286UTIL_HOME}/output"
mkdir -p "${output_dir}"
logfile="${output_dir}/log.txt"; log () { echo "${this_file}: ${1}" >> "${logfile}" && /bin/sync; }
cat /dev/urandom > /dev/fb0
#endregion Boilerplate Script Header

# Dumping the contents of /dev/kmsg is difficult because is a circular buffer and never ends.
# You have to turn off the machine off because it appears to hang.
# Then you have to run chkdsk and look for the results in one of the recovered .chk files
# It is better to use a busybox which has dmesg compiled in (builtin one doesn't have it)

# Lets try catting /dev/kmsg in the background, sleeping for a while and then killing it gently.

if [ ! -f "${byobb}" ]; then
    log "The built-in busybox doesn't have this command."
    log "Try: https://github.com/EXALAB/Busybox-static/tree/main/busybox_arm"
    log "Put it in: ${GT4286UTIL_HOME}/bin/busybox_arm"
else
    log "BYO Busybox found: ${byobb}"


    log "Running cat in the background"
    cat /dev/kmsg > "${output_dir}/debug-kmsg.txt" 2>&1 & 
    cat_pid=$!

    # kill %1 #The first job
    # kill %- #The last job
    # kill $! #As long as you don't launch any other process in the background you can use $! directly

    log "Sleeping for 5"
    ${byobb} sleep 5
    /bin/sync
    log "Kill cat with TERM"
    kill -TERM ${cat_pid}
    /bin/sync
    ${byobb} sleep 5
    log "Kill cat with 9"
    kill -9 ${cat_pid}
    /bin/sync
    log "Done"
fi