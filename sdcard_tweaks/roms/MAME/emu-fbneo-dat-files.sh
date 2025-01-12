#!/bin/sh

#region Boilerplate Script Header
this_file=${0}
output_dir="${GT4286UTIL_HOME}/output"
mkdir -p "${output_dir}"
logfile="${output_dir}/log.txt"; log () { echo "${this_file}: ${1}" >> "${logfile}" && /bin/sync; }
cat /dev/urandom > /dev/fb0
#endregion Boilerplate Script Header

### Dump FBNeo Data Files
log "Dumping FBNeo Data Files"

potential_exe=(
    "/mnt/extsd/emus/fbneo/fbneo.bin"
    "/mnt/extsd/emus/fbneo/fbneo"
)

exe="";
for i in "${potential_exe[@]}"; do if [ -f "${i}" ]; then exe="${i}"; break; fi; done

if [ "${exe}" = "" ];
then
    log "No emulator found. Bailing out";
    exit 1;
fi

mkdir -p "${output_dir}/fbneo-dat-files"
(cd "${output_dir}/fbneo-dat-files" && ${exe} -dat >> "${output_dir}/emu-fbneo-dat-files.txt" 2>&1; /bin/sync)
