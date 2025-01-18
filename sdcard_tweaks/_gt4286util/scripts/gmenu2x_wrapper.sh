#!/bin/sh
wrapped_exe="$0.bin"
wrapped_exe_logfile="${wrapped_exe}.log"

log_exe=false

#log "---------------------------------------------------"
#log "Arg0: $0"
#log "ArgAll: ${*}"
#log "Current Working Directory: ${PWD}"
log "Wrapped Exe: ${wrapped_exe}"
log "Executing wrapped exe: ${wrapped_exe}"

enable_hack_gmenu2x_enable_disable_boot_logo=true
enable_hack_gmenu2x_set_default_startup_screen=true

#region Enable / Disable Boot Logo animation and music
if ${enable_hack_gmenu2x_enable_disable_boot_logo}; then
    show_boot_logo=$(/bin/getprop "persist.gt4286.show_boot_logo")
    log "Property: persist.gt4286.show_boot_logo=${show_boot_logo}"

    if [ "${show_boot_logo}" = "false" ];
    then
        log "Setting prop.logoboot=1"
        /bin/setprop "prop.logoboot" "1"
    fi
fi
#endregion Enable / Disable Boot Logo animation and music

#region Set Start Screen
if ${enable_hack_gmenu2x_set_default_startup_screen}; then
    gmenu2x_skip_default_screen_handling_property_name="prop.gt4286.gmenu2x_run_once"

    gmenu2x_skip_default_screen_handling_property_value=$(/bin/getprop "${gmenu2x_skip_default_screen_handling_property_name}")
    log "Property: ${gmenu2x_skip_default_screen_handling_property_name}=${gmenu2x_skip_default_screen_handling_property_value}"

    if [ "${gmenu2x_skip_default_screen_handling_property_value}" != "true" ];
    then
        ## List ##
        # /bin/setprop "prop.title" "ALL"
        # /bin/setprop "prop.kind" "0"
        # /bin/setprop "prop.selection" "0" # position in list (0=first entry in list)

        ## Class ##
        # /bin/setprop "prop.title" "GAMELIST"
        # /bin/setprop "prop.gameclassselection" "8"  # Class 0=FBA, 1=FC, 2=GB, 3=GBA, 4=GBC, 5=MD, 6=SFC, 7=PS1, 8=MAME, 9=PCE
        # /bin/setprop "prop.selection" "0" # position in list (0=first entry in list)

        ## History ##
        # /bin/setprop "prop.title" "ALL"
        # /bin/setprop "prop.kind" "2"
        # /bin/setprop "prop.selection" "0" # position in list (0=first entry in list)

        ## Collect ##
        # /bin/setprop "prop.title" "ALL"
        # /bin/setprop "prop.kind" "3"
        # /bin/setprop "prop.selection" "0" # position in list (0=first entry in list)

        ## Search ##
        # /bin/setprop "prop.title" "SEARCHFTU"
        # /bin/setprop "prop.inputstr" "home alone" # the text that has been entered into the search
        # /bin/setprop "prop.keysel" "0"      # ??
        # /bin/setprop "prop.numorletter" "1" # 0=Numeric Keyboard, 1=Alphabet Keyboard
        # /bin/setprop "prop.selection" "0"   # position in list (0=first entry in list)

        # We set a session variable (that doesn't persist across reboots)
        # to prevent jumping to the default tab and location) after the first time.
        log "Setting: ${gmenu2x_skip_default_screen_handling_property_name}=true"
        /bin/setprop "${gmenu2x_skip_default_screen_handling_property_name}" "true"
    fi
fi
#endregion Set Start Screen

if [ "${log_exe}" = "true" ];
then
    log "Executing wrapped exe with logging: ${wrapped_exe}"
    ${wrapped_exe} "$@" >> "${wrapped_exe_logfile}" 2>&1; /bin/sync
else
    log "Executing wrapped exe without logging: ${wrapped_exe}"
    ${wrapped_exe} "$@" > /dev/null 2>&1
fi

#log "After gmenu2x. How did we get here. Did we invoke kill? or reboot?"