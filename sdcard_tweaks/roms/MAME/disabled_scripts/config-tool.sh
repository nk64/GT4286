#!/bin/sh

#region Boilerplate Script Header
this_file=${0}
output_dir="${GT4286UTIL_HOME}/output"
mkdir -p "${output_dir}"
logfile="${output_dir}/log.txt"; log () { echo "${this_file}: ${1}" >> "${logfile}" && /bin/sync; }
cat /dev/urandom > /dev/fb0
#endregion Boilerplate Script Header


# # no help
# log "Running /config/config_tool --help"
# /config/config_tool --help  > "${output_dir}/config_tool_help.txt"; /bin/sync

log "Running /config/dump_config"
/config/dump_config > "${output_dir}/config_tool_dump_config.txt"; /bin/sync

log "Running /config/dump_mmap"
/config/dump_mmap > "${output_dir}/config_tool_dump_mmap.txt"; /bin/sync
