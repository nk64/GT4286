#!/bin/sh

#region Boilerplate Script Header
this_file=${0}
output_dir="${GT4286UTIL_HOME}/output"
mkdir -p "${output_dir}"
logfile="${output_dir}/log.txt"; log () { echo "${this_file}: ${1}" >> "${logfile}" && /bin/sync; }
cat /dev/urandom > /dev/fb0
#endregion Boilerplate Script Header

log "Running a test awk program"

{
    echo "----"
    /bin/busybox awk "BEGIN { print \"Don't Panic!\" }"
} > "${output_dir}/test-awk.txt" 2>&1; /bin/sync