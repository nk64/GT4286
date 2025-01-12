#!/bin/sh

#region Boilerplate Script Header
this_file=${0}
output_dir="${GT4286UTIL_HOME}/output"
mkdir -p "${output_dir}"
logfile="${output_dir}/log.txt"; log () { echo "${this_file}: ${1}" >> "${logfile}" && /bin/sync; }
cat /dev/urandom > /dev/fb0
#endregion Boilerplate Script Header

## I don't know what the appropriate parameters are

log "Test the test_fb program found at /bin/test_fb"
/bin/test_fb /dev/fb0 5000 0x123456 > "${output_dir}/test-test_fb.txt" 2>&1; /bin/sync
