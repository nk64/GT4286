#!/bin/sh

#region Boilerplate Script Header
this_file=${0}
output_dir="${GT4286UTIL_HOME}/output"
mkdir -p "${output_dir}"
logfile="${output_dir}/log.txt"; log () { echo "${this_file}: ${1}" >> "${logfile}" && /bin/sync; }
cat /dev/urandom > /dev/fb0
#endregion Boilerplate Script Header

dest="${output_dir}/os-dump-files"
log "Copying OS Files to ${dest}"

dirs_of_interest=(
    "bin"
    "etc"
    "data"
    "config"
    "lib"
    "overlay"
    "sbin"
    "system"
    "tmp"
)

log "Copying: /"
mkdir -p "${dest}" >> "${logfile}" 2>&1; /bin/sync
cp /* "${dest}" >> "${logfile}" 2>&1; /bin/sync

for i in "${dirs_of_interest[@]}";
do
    log "Copying: ${i}"
    mkdir -p "${dest}/${i}" >> "${logfile}" 2>&1; /bin/sync
    cp -R "/${i}" "${dest}" >> "${logfile}" 2>&1; /bin/sync
done
