#!/bin/sh
log "---------------------------------------------------"
log "emulator_wrapper.sh"
emulator_exe="${0}.bin"
emulator_exe_logfile="${emulator_exe}.log"

enable_hack_handle_gmenu2x_download_bug=true
enable_hack_humanise_names=true
enable_hack_custom_scripts=true
enable_hack_custom_keymaps=true
enable_hack_fbneo_sybsystems=true

#region Functions
get_file_extension()
(
    case ${1} in
      (.*.*) ext=${1##*.};;
      (.*)   ext="";;
      (*.*)  ext=${1##*.};;
      (*)    ext="";;
    esac
    echo "${ext}"
)

find_index_of_matching_entry_in_array()
{
    local value="$1"
    shift
    find_index_of_matching_entry_in_array_result=-1
    i=0
    for entry in "$@"; do
        if [ "${entry}" = "${value}" ]; then
            find_index_of_matching_entry_in_array_result=${i};
            break;
        fi
        i=$((i + 1))
    done
}

split_file_path_into_parts()
{
    # Based on this answer https://stackoverflow.com/a/3362952
    split_file_path_into_parts_directory=""
    split_file_path_into_parts_filename=""
    split_file_path_into_parts_file_ext=""
    split_file_path_into_parts_file_stem=""
    local full_name="${1}"
    split_file_path_into_parts_directory="${full_name%/*}"
    split_file_path_into_parts_filename="${full_name##*/}"
    split_file_path_into_parts_file_ext="${split_file_path_into_parts_filename##*.}"
    split_file_path_into_parts_file_stem="${split_file_path_into_parts_filename%.*}"
    split_file_path_into_parts_directory_name="${split_file_path_into_parts_directory##*/}"
}
#endregion Functions

#region Emulator related arrays
emulator_binary_path_array=(
    "/mnt/extsd/emus/fbneo/fbneo"
    "/mnt/extsd/emus/fceux/fceux"
    "/mnt/extsd/emus/gamebatte/gambatte_sdl"
    "/mnt/extsd/emus/gpsp/gpsp"
    "/mnt/extsd/emus/gbc/gambatte_sdl"
    "/mnt/extsd/emus/picodrive/PicoDrive"
    "/mnt/extsd/emus/snes9x4d/snes9x4d.dge"
    "/mnt/extsd/emus/pcsx4all/pcsx"
    "/mnt/extsd/emus/mame/fbneo"
    "/mnt/extsd/emus/temper/temper"
)

emu_class_array=(
    "FBA"
    "FC"
    "GB"
    "GBA"
    "GBC"
    "MD"
    "SFC"
    "PS1"
    "MAME"
    "PCE"
)

rom_subdir_name_array=(
    "FBA"
    "FC"
    "GB"
    "GBA"
    "GBC"
    "MD"
    "SFC"
    "PS"
    "MAME"
    "PCE"
)

keymap_filename_array=(
    "fbneo.keymap"
    "fceux.keymap"
    "gambatte.keymap"
    "gpsp.keymap"
    "gambatte.keymap"
    "pico.keymap"
    "sfc.keymap"
    "pcsx.keymap"
    "fbneo.keymap"
    "temper.keymap"
)
#endregion Emulator related arrays

log_exe=true
log "Arg0: ${0}"
log "ArgCount: ${#}"
log "ArgAll: ${*}"
log "Current Working Directory: ${PWD}"
log "Wrapped Exe: ${emulator_exe}"

if [ ${#} = 0 ]
then
    # There are no arguments or options so we are likely being called by one of
    # the scripts to dump the emulator help so don't redirect the output
    log "Executing original exe without logging: ${emulator_exe}"
    "${emulator_exe}"
    exit 0
fi

#region Key Variable setup
find_index_of_matching_entry_in_array "${0}" "${emulator_binary_path_array[@]}"

log "*** Setting up key variables for emulator ***"
builtin_rom_dir="/mnt/extsd/roms"
download_rom_dir="/mnt/extsd/download"
keymap_filename=""
emulator_class=""
custom_keymap_dir=""
rom_subdir_name=""

if [ "${find_index_of_matching_entry_in_array_result}" -ne -1 ];
then
    #log "Found ${0} at position ${find_index_of_matching_entry_in_array_result}"
    keymap_filename="${keymap_filename_array[${i}]}"
    emulator_class="${emu_class_array[${i}]}"
    emulator_binary="${emulator_binary_path_array[${i}]}"
    rom_subdir_name="${rom_subdir_name_array[${i}]}"
    custom_keymap_dir="${builtin_rom_dir}/${rom_subdir_name}/keymaps"
    log "    Emulator Class: ${emulator_class}"
    log "    Emulator Binary: ${emulator_binary}"
    log "    Rom Subdirectory Name: ${rom_subdir_name}"
    log "    Destination keymap file: ${keymap_filename}"
    log "    Custom keymap dir: ${custom_keymap_dir}"
else
    log "    No Matching Emulator Found. Aborting: ${0}"
    exit 1
fi

if [ "${emulator_class}" = "PS1" ]; then
    #pcsx is run with -cdfile parameter so lets ignore that for now
    original_rom_file_path=${2}
else
    original_rom_file_path=${1}
fi
#endregion Key Variables

#region gmenu2x bug handling
if ${enable_hack_handle_gmenu2x_download_bug}; then
    log "*** Check for gmenu2x downloaded games from the Collect or History tabs bug ***"
    # if file name starts with /mnt/extsd/roms/{path}
    # but the file doesn't exist
    # check in /mnt/extsd/download/{path} and if it exists there then
    # we are trying to play from the download history or collect tab so we need to 
    # fix up the path to the rom file to work around the bug

    if [ "${original_rom_file_path}" != "${original_rom_file_path#"${builtin_rom_dir}"/}" ]; then
        log "    '${original_rom_file_path}' starts with '${builtin_rom_dir}/'";

        if [ ! -f "${original_rom_file_path}" ]; then
            relative_path_to_game="${original_rom_file_path#"${builtin_rom_dir}"/}"
            download_game_path="/mnt/extsd/download/${relative_path_to_game}"
            log "    but '${original_rom_file_path}' does not exist so check '${download_game_path}'";

            if [ -f "${download_game_path}" ]; then
                log "'${download_game_path}' exists so we are probably trying to play a downloaded game from the history or collect tab";
                log "    Trick the emulator into loading '${download_game_path}' instead of '${original_rom_file_path}'";
                original_rom_file_path="${download_game_path}"
            fi
        else
            log "    but '${original_rom_file_path}' exists so no trickery required";
        fi

    elif [ "${original_rom_file_path}" != "${filename#"${download_rom_dir}"/}" ]; then
        log "    '${original_rom_file_path}' starts with '${download_rom_dir}/'. We are probably running directly from the download tab so no trickery required."
    else
        log "    '${original_rom_file_path}' doesn't start with '${builtin_rom_dir}' or '${download_rom_dir}/'. No trickery applied."
    fi
fi
#endregion

filename=${original_rom_file_path}

#region Handle Redirected (aka humanised) names
if ${enable_hack_humanise_names}; then
    log "*** Handle Redirected (aka humanised) names ***"
    redirected_target_rom_file_path=""
    if [ "$(get_file_extension "${filename}")" = "redir" ]; then
        redir_file_path=${original_rom_file_path}
        rom_directory="${redir_file_path%/*}"
        log "    RomDir: ${rom_directory}"
        log "    Reading Redirection File: ${redir_file_path}"
        redir_subpath=$(cat "${redir_file_path}")
        log "    Redirect to: ${redir_subpath}"
        redirected_target_rom_file_path="${rom_directory}/redir/${redir_subpath}"
        log "    Redirected Rom: '${redir_file_path}' -> '${redirected_target_rom_file_path}'"
        filename="${redirected_target_rom_file_path}"
    else
        log "    Not a .redir file: ${filename}"
    fi
fi
#endregion Handle Redirected (aka humanised) names

#region Script Handling
if ${enable_hack_custom_scripts}; then
    # Check if rom is actually a custom script to run
    log "*** Handle Scripts ***"
    if [ "$(get_file_extension "${filename}")" = "sh" ]; then
        if [ -f "${filename}" ]; then
            log "    Running script file: ${filename}"
            # shellcheck disable=SC2154 # assigned in parent script
            ${filename} >> "${logfile}" 2>&1
            /bin/sync
            exit 0
        else
            log "    Script not found: ${filename}"
            exit 1
        fi
    else
        log "    Not a script: ${filename}"
    fi
fi
#endregion Script Handling

#region Handle FBNeo Subsystem games
if ${enable_hack_fbneo_sybsystems}; then
    fbneo_subsystem=""
    if [ "${emulator_class}" = "FBA" ]; then
        split_file_path_into_parts "${filename}"
        log "    Split: ${filename}"
        log "      directory: ${split_file_path_into_parts_directory}"
        log "      filename: ${split_file_path_into_parts_filename}"
        log "      file_ext: ${split_file_path_into_parts_file_ext}"
        log "      file_stem: ${split_file_path_into_parts_file_stem}"
        log "      directory_name: ${split_file_path_into_parts_directory_name}"

        # shellcheck disable=SC3044
        typeset -l split_file_path_into_parts_directory_name_lower
        split_file_path_into_parts_directory_name_lower="${split_file_path_into_parts_directory_name}"
        if  [ "${split_file_path_into_parts_directory_name_lower}" != "fba" ] && \
            [ "${split_file_path_into_parts_directory_name_lower}" != "redir" ]
        then
            fbneo_subsystem="${split_file_path_into_parts_directory_name}"
            log "    FBA Subsystem Detected: ${fbneo_subsystem}"
        fi
    fi
fi
#endregion Handle FBNeo Subsystem games

#region Custom keymap handling
if ${enable_hack_custom_keymaps}; then
    log "*** Custom keymap handling ***"
    if [ "${keymap_filename}" != "" ];
    then
        rom_file_path=${original_rom_file_path}
        if [ "${redirected_target_rom_file_path}" != "" ]; then
            rom_file_path=${redirected_target_rom_file_path}
        fi

        game_filename="${rom_file_path##*/}"
        game_name=${game_filename%.*}

        destination_keymap_file_path="/keyremp/${keymap_filename}"

        log "    Checking for a custom keymap for '${rom_file_path}' (${game_name}) in '${custom_keymap_dir}' to copy to '${destination_keymap_file_path}'"

        found_custom_keymap_file_path=""

        custom_keymap_path_to_check="${custom_keymap_dir}/${game_name}.keymap"

        subsystem_default_keymap_path_to_check=""
        if [ "${fbneo_subsystem}" != "" ]; then
            subsystem_default_keymap_path_to_check="${custom_keymap_dir}/${fbneo_subsystem}/_default.keymap"
        fi

        default_keymap_path_to_check="${custom_keymap_dir}/_default.keymap"

        if [ "${found_custom_keymap_file_path}" = "" ]; then
            log "    Checking for game specific custom keymap: ${custom_keymap_path_to_check}"
            if [ -f "${custom_keymap_path_to_check}" ]; then
                log "    Found a custom keymap for this game: ${custom_keymap_path_to_check}"
                found_custom_keymap_file_path=${custom_keymap_path_to_check}
            else
                log "    Didn't find a custom keymap for this game: ${custom_keymap_path_to_check}"
            fi
        fi

        if [ "${found_custom_keymap_file_path}" = "" ] && [ "${subsystem_default_keymap_path_to_check}" != "" ]; then
            log "    Checking for subsystem generic custom keymap: ${subsystem_default_keymap_path_to_check}"
            if [ -f "${subsystem_default_keymap_path_to_check}" ]; then
                log "    Found a default subsystem keymap for this game: ${subsystem_default_keymap_path_to_check}"
                found_custom_keymap_file_path=${subsystem_default_keymap_path_to_check}
            else
                log "    Didn't find a default subsystem keymap for this game: ${subsystem_default_keymap_path_to_check}"
            fi
        fi

        if [ "${found_custom_keymap_file_path}" = "" ]; then
            log "    Checking for default keymap for this emulator: ${default_keymap_path_to_check}"
            if [ -f "${default_keymap_path_to_check}" ]; then
                log "    Found a default keymap for this emulator: ${default_keymap_path_to_check}"
                found_custom_keymap_file_path=${default_keymap_path_to_check}
            else
                log "    Didn't find a default keymap for this emulator: ${default_keymap_path_to_check}"
            fi
        fi

        if [ "${found_custom_keymap_file_path}" = "" ]; then
            log "    No relevant keymap file found"
        else
            log "    Copying keymap file from '${found_custom_keymap_file_path}' to '${destination_keymap_file_path}'"
            cp "${found_custom_keymap_file_path}" "${destination_keymap_file_path}"
        fi
    fi
fi
#endregion Custom keymap handling

#region testcode
# shellcheck disable=SC2050
if [ "true" = "false" ]; then
    # For Neo Geo CDZ Subsystem
    # /bios/neocdz.zip must contain two files:
    #     000-lo.lo: crc=5A86CFF2
    #     neocd.bin: crc=DF9DE490
    # The Emulator can't handle FILE lines in .cue files
    # Convert multi file .bin/.cue files to CCD
    # https://github.com/Kippykip/SBITools CUE2CCD.bat
    #"${emulator_exe}" "/mnt/extsd/roms/FBA/redir/neocdz/neocdz.zip" "-cd" "${redirected_target_rom_file_path}" >> "${emulator_exe_logfile}" 2>&1; /bin/sync

    "${emulator_exe}" "/mnt/extsd/roms/FBA/redir/neocdz/neocdz.zip" -cd "/mnt/extsd/roms/FBA/redir/neocdz/Metal Slug (Japan) (En,Ja).ccd" >> "${emulator_exe_logfile}" 2>&1; /bin/sync
    exit 0
fi
#endregion testcode

#region Emulator Parameter Preparation
log "*** Emulator Parameters ***"
log "    Setting up Parameters for '${emulator_binary}'"
if [ "${emulator_class}" = "PS1" ]; then
    set -- "-cdfile" "${filename}"
elif [ "${emulator_class}" = "FBA" ]; then
    if [ "${fbneo_subsystem}" = "" ]; then
        set -- "${filename}"
    else
        if [ "${fbneo_subsystem}" = "neocdz" ]; then
            log "    Setting up Parameters for FBA Neo Geo CDZ Subsystem Friendly Path:"
            set -- "/mnt/extsd/bios/neocdz.zip" "-cd" "${filename}"
        else
            filename="${split_file_path_into_parts_directory}/${fbneo_subsystem}_${split_file_path_into_parts_filename}"
            log "    Setting up Parameters for FBA Subsystem (${fbneo_subsystem}) friendly path:"
            set -- "${filename}"
        fi
    fi
elif [ "${emulator_class}" = "FC" ]; then
    set -- "${filename}"
elif [ "${emulator_class}" = "GB" ]; then
    set -- "${filename}"
elif [ "${emulator_class}" = "GBA" ]; then
    set -- "${filename}"
elif [ "${emulator_class}" = "GBC" ]; then
    set -- "${filename}"
elif [ "${emulator_class}" = "MD" ]; then
    set -- "${filename}"
elif [ "${emulator_class}" = "SFC" ]; then
    set -- "${filename}"
elif [ "${emulator_class}" = "MAME" ]; then
    set -- "${filename}"
elif [ "${emulator_class}" = "PCE" ]; then
    set -- "${filename}"
else
    set -- "${filename}"
fi
log "    Emulator parameters: ${*}"
#endregion Emulator Parameter Preparation

#region Run Emulator
log "*** Run Emulator ***"
# Actually run the emulator with the parameters which have been set up earlier
if [ "${log_exe}" = true ]; then
    log "    Executing wrapped exe with logging: ${emulator_exe} ${*}"
    "${emulator_exe}" "${@}" >> "${emulator_exe_logfile}" 2>&1; /bin/sync

    if false; then
        echo "    ----------------------------- logcat --------------" >> "${emulator_exe_logfile}"; /bin/sync
        /bin/logcat -d >> "${emulator_exe_logfile}"; /bin/sync
    fi

    if true; then
        echo "    ----------------------------- getprop --------------" >> "${emulator_exe_logfile}"; /bin/sync
        /bin/getprop >> "${emulator_exe_logfile}"; /bin/sync
    fi
else
    log "    Executing wrapped exe without logging: ${emulator_exe} ${*}"
    "${emulator_exe}" "${@}"
fi

/bin/sync
#endregion Run Emulator