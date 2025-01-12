#!/bin/sh

#region Boilerplate Script Header
this_file=${0}
output_dir="${GT4286UTIL_HOME}/output"
mkdir -p "${output_dir}"
logfile="${output_dir}/log.txt"; log () { echo "${this_file}: ${1}" >> "${logfile}" && /bin/sync; }
cat /dev/urandom > /dev/fb0
#endregion Boilerplate Script Header

## None of these commands results in any output whatsoever.
## All the help must have been compiled out of them for size.

log "Dumping mksh help"
/bin/mksh --help > "${output_dir}/mksh-help.txt"; /bin/sync
/bin/sh --help > "${output_dir}/sh-help.txt"; /bin/sync

log "Dumping busybox help"
help_file="${output_dir}/busybox-help.txt"
/bin/busybox > "${output_dir}/busybox-help.txt"; /bin/sync
/sbin/insmod --help >> "${help_file}"; /bin/sync
/sbin/lsmod  --help >> "${help_file}"; /bin/sync
/sbin/rmmod  --help >> "${help_file}"; /bin/sync
/bin/cat     --help >> "${help_file}"; /bin/sync
/bin/chmod   --help >> "${help_file}"; /bin/sync
/bin/chown   --help >> "${help_file}"; /bin/sync
/bin/cp      --help >> "${help_file}"; /bin/sync
/bin/date    --help >> "${help_file}"; /bin/sync
/bin/df      --help >> "${help_file}"; /bin/sync
/bin/echo    --help >> "${help_file}"; /bin/sync
/bin/fsync   --help >> "${help_file}"; /bin/sync
/bin/kill    --help >> "${help_file}"; /bin/sync
/bin/ln      --help >> "${help_file}"; /bin/sync
/bin/ls      --help >> "${help_file}"; /bin/sync
/bin/mkdir   --help >> "${help_file}"; /bin/sync
/bin/mknod   --help >> "${help_file}"; /bin/sync
/bin/mount   --help >> "${help_file}"; /bin/sync
/bin/mv      --help >> "${help_file}"; /bin/sync
/bin/ps      --help >> "${help_file}"; /bin/sync
/bin/pwd     --help >> "${help_file}"; /bin/sync
/bin/rm      --help >> "${help_file}"; /bin/sync
/bin/rmdir   --help >> "${help_file}"; /bin/sync
/bin/sync    --help >> "${help_file}"; /bin/sync
/bin/touch   --help >> "${help_file}"; /bin/sync
/bin/umount  --help >> "${help_file}"; /bin/sync
/bin/vi      --help >> "${help_file}"; /bin/sync

#The following commands appear to be compiled in to /bin/busybox but are missing a corresponding soft link in /bin or /sbin:
#  awk, dirname, install, top, unlink

log "Linking up busybox extras"
mkdir -p /tmp/bin
ln -s /bin/busybox /tmp/bin/awk
ln -s /bin/busybox /tmp/bin/dirname
ln -s /bin/busybox /tmp/bin/install
ln -s /bin/busybox /tmp/bin/top
ln -s /bin/busybox /tmp/bin/unlink

log "Linking up busybox extras help"
help_file="${output_dir}/busybox-extras-help.txt"
/tmp/bin/awk     --help >  "${help_file}"; /bin/sync
/tmp/bin/dirname --help >> "${help_file}"; /bin/sync
/tmp/bin/install --help >> "${help_file}"; /bin/sync
/tmp/bin/top     --help >> "${help_file}"; /bin/sync
/tmp/bin/unlink  --help >> "${help_file}"; /bin/sync

log "Deleting busybox extras"
rm -rf /tmp/bin
