#!/bin/sh

#region Boilerplate Script Header
this_file=${0}
output_dir="${GT4286UTIL_HOME}/output"
mkdir -p "${output_dir}"
logfile="${output_dir}/log.txt"; log () { echo "${this_file}: ${1}" >> "${logfile}" && /bin/sync; }
cat /dev/urandom > /dev/fb0
#endregion Boilerplate Script Header

byobb="${GT4286UTIL_HOME}/bin/busybox_arm"

log "Dumping the process list"
# /bin/ps > "${output_dir}/os-info-dynamic-ps.txt"; /bin/sync
${byobb} ps -ef -o pid,ppid,pgid,user,group,comm,args > "${output_dir}/os-info-dynamic-ps.txt" 2>&1; /bin/sync

log "Dumping /proc/uptime"
cat /proc/uptime > "${output_dir}/os-info-dynamic-proc-uptime.txt"; /bin/sync

log "Disk Free (df)"
/bin/df > "${output_dir}/os-info-dynamic-df.txt"; /bin/sync

