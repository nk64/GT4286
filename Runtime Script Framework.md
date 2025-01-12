# Runtime Script Framework

## Description
Consolidate all the emulators to run through one main script (and gmenu2x to another).
These script perform a variaety of hacks

gmenu2x:
* enable_disable_boot_logo
* set_default_startup_screen

emulator hacks:
* handle_gmenu2x_download_bug
* humanise_names
* custom_scripts
* custom_keymaps
* fbneo_subsystems

## Instructions (WIP)
⚠️ Backup your SD Card before proceding!

1. Rename all original emulator binaries with a `.bin` extension. eg `/emus/fbneo/fbneo` -> `/emus/fbneo/fbneo.bin`
1. Rename the original `gmenu2x` with a `.bin` extension. eg `/bin/gmenu2x` -> `/bin/gmenu2x.bin`
2. Copy the `_gt4286util` from [here](./sdcard_tweaks/) to the root of your SD Card.
    - It contains an improved busybox in `_gt4286util/bin`

3. Copy and rename `/_gt4286util/_template_gmenu2x.sh` to `/bin/gmenu2x`
4. Copy and rename `/_gt4286util/_template_emu.sh` to `/emus/{emudir}/{emuname}` eg:
    -  `/_gt4286util/_template_emu.sh` -> `/emus/fbneo/fbneo`
    -  `/_gt4286util/_template_emu.sh` -> `/emus/fceux/fceux`
    -  `/_gt4286util/_template_emu.sh` -> `/emus/gamebatte/gambatte_sdl`
    -  `/_gt4286util/_template_emu.sh` -> `/emus/gpsp/gpsp`
    -  `/_gt4286util/_template_emu.sh` -> `/emus/gbc/gambatte_sdl`
    -  `/_gt4286util/_template_emu.sh` -> `/emus/picodrive/PicoDrive`
    -  `/_gt4286util/_template_emu.sh` -> `/emus/snes9x4d/snes9x4d.dge`
    -  `/_gt4286util/_template_emu.sh` -> `/emus/pcsx4all/pcsx`
    -  `/_gt4286util/_template_emu.sh` -> `/emus/mame/fbneo`
    -  `/_gt4286util/_template_emu.sh` -> `/emus/temper/temper`

5. Copy the scripts from [here](./sdcard_tweaks/roms/MAME/) to `/roms/MAME`
    - most of these scripts are for use by those interested in exploring the internals but the following might be useful to the casual user:
        - boot-logo-disable.sh
        - boot-logo-enable.sh


## `gmenu2x` hacks
* `enable_disable_boot_logo`
    - use the `boot-logo-disable.sh` and `boot-logo-enable.sh` MAME scripts to toggle the boot logo playback as desired.
* `set_default_startup_screen`
    - review the `/_gt4286util/gmenu2x_wrapper.sh` script and [property doco](./Exploring%20the%20OS%20-%20Properties.md) for the appropriate properties to set to have your desired screen be the default

## emulator hacks:
* `handle_gmenu2x_download_bug`
    - fixes a [known issue](./Known%20Issues.md) where downloaded games don't load via the History and Collection tabs.

* `humanise_names`
    - Includes the hack from [Humanise Names](./Humanise%20Names.md)

* `custom_scripts`
    - Scripts can be placed in any of the (built-in) rom directories (default to `/roms/MAME` becase the MAME emulator is a clone). They will show up in the games list (after running `GT4286Util refresh-gamedb <path-to-card>`)
    - This is a formalisation of [Exploring the OS like a Boss](./Exploring%20the%20OS.md)

* `custom_keymaps`  

    Search for custom keymap files in order and copy to the appropriate file name for the emulator in `/keyremaps`:

    Examples:
    * FBNeo:
        1. game specific keymap eg. `/roms/FBA/keymaps/sf2.keymap`
        2. emulator default keymap eg. `/roms/FBA/keymaps/_default.keymap`

    * FBNeo Subsystems:
        1. game specific keymap eg. `/roms/FBA/keymaps/cv/heist.keymap`
        2. subsystem default keymap eg. `/roms/FBA/keymaps/cv/_default_.keymap`
        3. emulator default keymap eg. `/roms/FBA/keymaps/_default.keymap`

    * Other Emulators:
        1. game specific keymap eg. `/roms/{class}/keymaps/My Example Game.keymap`
        2. emulator default keymap eg. `/roms/{class}/keymaps/_default.keymap`

* `fbneo_subsystems`
    - Refer to the [details](./Emulators%20-%20fbneo.md) of supported subsystems
