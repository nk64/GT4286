#!/bin/sh
export GT4286UTIL_HOME=/mnt/extsd/_gt4286util
this_file=${0}
logfile="${this_file}.log"; log () { echo "${this_file}: ${1}" >> "${logfile}" && /bin/sync; }

log "---------------------------------------------------"
log "Arg0: $0"
log "ArgAll: ${*}"
log "Current Working Directory: ${PWD}"

wrapper_script="${GT4286UTIL_HOME}/scripts/emulator_wrapper.sh"

log "Wrapper Script: ${wrapper_script}"

if [ -f "${wrapper_script}" ]
then
    log "Sourcing wrapper script: ${wrapper_script}"
    # shellcheck disable=SC1090
    . "${wrapper_script}" >> "${logfile}" 2>&1 && /bin/sync
else
    log "Wrapper script not found: ${wrapper_script}"
    exit 1
fi

/bin/sync