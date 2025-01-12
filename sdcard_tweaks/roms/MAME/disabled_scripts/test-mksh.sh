#!/bin/sh

#region Boilerplate Script Header
this_file=${0}
output_dir="${GT4286UTIL_HOME}/output"
mkdir -p "${output_dir}"
logfile="${output_dir}/log.txt"; log () { echo "${this_file}: ${1}" >> "${logfile}" && /bin/sync; }
cat /dev/urandom > /dev/fb0
#endregion Boilerplate Script Header

log "Testing MKSH Arrays"

my_array=(red orange green)
value='green'

for i in "${!my_array[@]}"; do
   if [ "${my_array[${i}]}" = "${value}" ]; then
       log "found ${value} at position: ${i}"; break;
   fi
done

log "Testing MKSH Expansions"
#Upper- or lower-casing strings in bash and zsh
#https://scriptingosx.com/2019/12/upper-or-lower-casing-strings-in-bash-and-zsh/


# # No Good
# text_var="xyz"
# text_var_upper=${text_var^^}
# log "Is this upper case 1? ${text_var_upper}"

text_var="xyz"
# shellcheck disable=SC3044
typeset -u text_var_upper
text_var_upper=${text_var}
log "Is this upper case 2? ${text_var_upper}"

# # enable not found
# log "Capturing output of 'enable'"
# # shellcheck disable=SC3044
# enable -p -a > "${output_dir}/test-mksh.txt" 2>&1


/bin/sync