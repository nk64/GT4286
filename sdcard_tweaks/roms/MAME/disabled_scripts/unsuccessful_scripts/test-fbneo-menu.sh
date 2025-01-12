#!/bin/sh

#region Boilerplate Script Header
this_file=${0}
output_dir="${GT4286UTIL_HOME}/output"
mkdir -p "${output_dir}"
logfile="${output_dir}/log.txt"; log () { echo "${this_file}: ${1}" >> "${logfile}" && /bin/sync; }
cat /dev/urandom > /dev/fb0
#endregion Boilerplate Script Header

## Attempt to Run FBNeo's menu system (doesn't seem to be compiled in)

log "***** Running FBNeo Menu System"
/mnt/extsd/emus/mame/fbneo.orig -menu -joy >> "${output_dir}/test-fbneo-menu.txt" 2>&1; /bin/sync
log "***** Finished Running FBNeo Menu System"
