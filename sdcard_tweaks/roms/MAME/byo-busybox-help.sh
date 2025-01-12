#!/bin/sh

#region Boilerplate Script Header
this_file=${0}
output_dir="${GT4286UTIL_HOME}/output"
mkdir -p "${output_dir}"
logfile="${output_dir}/log.txt"; log () { echo "${this_file}: ${1}" >> "${logfile}" && /bin/sync; }
cat /dev/urandom > /dev/fb0
#endregion Boilerplate Script Header

# log "Running built-in busybox"
# # The builtin busybox doesn't seem to have the --list command nor have any help compiled in
# "/bin/busybox" > "${output_dir}/busybox-builtin.txt" 2>&1; /bin/sync
# "/bin/busybox" --list > "${output_dir}/busybox-builtin-list.txt" 2>&1; /bin/sync

# log "Running busybox-armv7l (1.21.1)"
# # https://busybox.net/downloads/binaries/1.21.1//busybox-armv7l
# "${GT4286UTIL_HOME}/bin/busybox-armv7l" > "${output_dir}/byo-busybox-busybox-armv7l.txt" 2>&1; /bin/sync
# "${GT4286UTIL_HOME}/bin/busybox-armv7l" --list > "${output_dir}/byo-busybox-busybox-armv7l-list.txt" 2>&1; /bin/sync

log "Running busybox_arm (1.31.1)"
# https://github.com/EXALAB/Busybox-static/tree/main/busybox_arm
"${GT4286UTIL_HOME}/bin/busybox_arm" > "${output_dir}/byo-busybox-busybox_arm.txt" 2>&1; /bin/sync
"${GT4286UTIL_HOME}/bin/busybox_arm" --list > "${output_dir}/byo-busybox-busybox_arm-list.txt" 2>&1; /bin/sync

set_up_byo_busybox_apps()
{
    log "Setting up BYO Busybox Apps"
    mkdir -p "/tmp/bin";
    #"${GT4286UTIL_HOME}/bin/busybox-armv7l" --install -s "/tmp/bin"
    "${GT4286UTIL_HOME}/bin/busybox_arm" --install -s "/tmp/bin"
}

set_up_byo_busybox_apps

ls -laF /tmp/bin > "${output_dir}/byo-busybox-ls.txt"

log "Deleting BYO Busybox apps"
rm -rf /tmp/bin
