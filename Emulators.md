# Emulators

The UI divides Roms up by Class, there is some overlap in the systems that are emulated by each emulator and some emulators are reused in more than one class.

| Class |    Emulator Command Line     |                       Emulated System                        | Builtin Rom Dir | Download Rom Dir |
|-------|------------------------------|--------------------------------------------------------------|-----------------|------------------|
| FBA   | /emus/fbneo/fbneo            | Arcade                                                       | /roms/fba       | /download/fba    |
| FC    | /emus/fceux/fceux            | Nintendo - Nintendo Entertainment System/Famicom             | /roms/fc        | /download/fc     |
| GB    | /emus/gamebatte/gambatte_sdl | Nintendo - Gamboy                                            | /roms/gb        | /download/gb     |
| GBA   | /emus/gpsp/gpsp              | Nintendo - Gamboy Advance                                    | /roms/gba       | /download/gba    |
| GBC   | /emus/gamebatte/gambatte_sdl | Nintendo - Gamboy Color                                      | /roms/gbc       | /download/gbc    |
| MD    | /emus/picodrive/PicoDrive    | Sega - Megadrive/Genesis                                     | /roms/md        | /download/md     |
| SFC   | /emus/snes9x4d/snes9x4d.dge  | Nintendo - Super Nintendo Entertainment System/Super Famicom | /roms/sfc       | /download/sfc    |
| PS1   | /emus/pcsx4all/pcsx -cdfile  | Sony - Playstation 1                                         | /roms/ps        | /download/ps     |
| MAME  | /emus/mame/fbneo             | Arcade                                                       | /roms/mame      | /download/mame   |
| PCE   | /emus/temper/temper          | PC-Engine/TurboGrafix-16                                     | /roms/pce       | /download/pce    |

### Notes:
* All Console Emulators Except PCE (```/emus/temper/temper```) support zipped single roms.
* No emulators support 7zipped roms except ```fbneo ``` (FBA and MAME) which support both zip and 7z
* FBA and MAME class use the same emulator (```fbneo```)



## Class: FBA
Emulator: [FBNeo](https://github.com/finalburnneo/FBNeo)  
Version string: FBNeo v1.0.0.01  
Binary: ```/emus/fbneo/fbneo```  

The emulator derives from source code from somewhere around this era: [https://github.com/finalburnneo/FBNeo/commit/e5b867384117fdf0a05f9d312b63bdc51d8c6c80](https://github.com/finalburnneo/FBNeo/commit/e5b867384117fdf0a05f9d312b63bdc51d8c6c80)

### Emulated Systems:
According to the dat files extracted from the emulator, the following systems are potentially supported:
* Arcade
* ColecoVision
* Sony MSX
* Neo Geo Pocket
* Neogeo
* Nintendo Entertainment System
* Nintendo Famicom Disk System
* PC Engine
* PC Engine SuperGrafx
* Sega Game Gear
* Sega Master System
* Sega MegaDrive (AKA Sega Genesis)
* Sega SG-1000
* Sinclair ZX Spectrum
* TurboGrafx-16

### Command line help (run /emus/fbneo/fbneo without parameters)
```
Usage: /mnt/extsd/emus/mame/fbneo [-cd] [-joy] [-menu] [-novsync] [-integerscale] [-fullscreen] [-dat] [-autosave] [-nearest] [-linear] [-best] <romname>
Note the -menu switch does not require a romname
e.g.: /mnt/extsd/emus/mame/fbneo.orig mslug
e.g.: /mnt/extsd/emus/mame/fbneo.orig -menu -joy
For NeoCD games:
/mnt/extsd/emus/mame/fbneo.orig neocdz -cd path/to/ccd/filename.cue (or .ccd)
Usage is restricted by the license at https://raw.githubusercontent.com/finalburnneo/FBNeo/master/src/license.txt
```

### Notes:
* This emulator has a bug in it's mapping causing the P1 R and P1 L buttons not to respond. This can be fixed with a 6 byte [patch](./img/fbneo-hexedit-fix-p1-r-and-l-buttons.png).
* A keyremap can be found [here](./sdcard_tweaks/keyremap/) which makes (at least) Street Fighter II work
* the -dat command line options works and generates [dat files](./dats/fbneo%20dat%20files/)
* the -menu command line option doesn't do anything.



## Class: FC
Emulator: [FCEUX](https://fceux.com/)  
Version string: FCEUX 2.2.3-interim svn  
Binary: ```/emus/fceux/fceux```  

### Emulated Systems:
The website claims support for the following systems:
* Nintendo Entertainment System
* Famicom
* Famicom Disk System
* Dendy



## Class: SFC
Emulator: [snes9x4d](https://github.com/m45t3r/snes9x4d)  
Binary: ```/emus/snes9x4d/snes9x4d.dge```  

### Emulated Systems:
* Super Nintendo Entertainment System
* Super Famicom

### Command line help (run /emus/snes9x4d/snes9x4d.dge without parameters)
```
usage: snes9x <options> <rom image filename>
```



## Class: GB, GBC
Emulator: gambatte  
Version string: Gambatte SDL SVN r309  
Binary: ```/emus/gamebatte/gambatte_sdl```  

The emulator derives from source code from somewhere around this era:
[gambatte_src-0.5.0-wip2v2](https://sourceforge.net/projects/gambatte/files/gambatte/0.5.0-wip2/)  

### Emulated Systems:
* Nintendo Gameboy
* Nintendo Gameboy Color

### Command line help (run /emus/gamebatte/gambatte_sdl without parameters)
```
Usage: gambatte_sdl [OPTION]... romfile
      --gba-cgb            GBA CGB mode

      --force-dmg          Force DMG mode

      --multicart-compat   Support certain multicart ROM images by not strictly respecting ROM header MBC type

  -f, --full-screen        Start in full screen mode

  -i, --input KEYS         Use the 8 given input KEYS for respectively
                           START SELECT A B UP DOWN LEFT RIGHT

  -l, --latency N          Use audio buffer latency of N ms
                           16 <= N <= 5000, default: 133

      --list-keys          List valid input KEYS

  -p, --periods N          Use N audio buffer periods
                           1 <= N <= 32, default: 4

  -r, --sample-rate N      Use audio sample rate of N Hz
                           4000 <= N <= 192000, default: 48000

      --resampler N        Use audio resampler number N
                           0 = Fast [default]
                           1 = High quality (CIC + sinc chain)
                           2 = Very high quality (CIC + sinc chain)
                           3 = Highest quality (CIC + sinc chain)

  -s, --scale N            Scale video output by an integer factor of N

  -v, --video-filter N     Use video filter number N
                           0 = None
                           1 = Bicubic Catmull-Rom spline 2x
                           2 = Bicubic Catmull-Rom spline 3x
                           3 = Kreed's 2xSaI
                           4 = MaxSt's hq2x
                           5 = MaxSt's hq3x

  -y, --yuv-overlay        Use YUV overlay for (usually faster) scaling
```



## Class: GBA
Emulator: [gpsp](https://gpsp-dev.blogspot.com/)  
Binary: ```/emus/gpsp/gpsp```  

### Emulated Systems:
* Nintendo Gameboy Advance



## Class: MD
Emulator: [PicoDrive](https://github.com/notaz/picodrive)  
Binary: ```/emus/picodrive/PicoDrive```  

### Emulated Systems:
* Sega Megadrive
* Sega Genesis
* Sega Master System
### Untested but potentially emulated systems
* Sega Mega-CD
* Sega 32X



## Class: PS1
Emulator: PCSX or PCSX4ALL  
Binary: ```/emus/pcsx4all/pcsx```  
Command line: ```/emus/pcsx4all/pcsx -cdfile```

### Emulated Systems:
* Sony Playstation 1

### Notes:
* Converting rom files to suit is a pain.



## Class: MAME
Emulator: [FBNeo](https://github.com/finalburnneo/FBNeo)  
Version: v1.0.0.01  
Binary: ```/emus/mame/fbneo```  

### Notes:
* It is just a copy of the FBNeo emulator with no added value



## Class: PCE
Emulator: [Temper](https://cajas.us/mirrors/trimui/temper/)  
Binary: ```/emus/temper/temper```  

### Emulated Systems:
* PC-Engine
* TurboGrafix-16