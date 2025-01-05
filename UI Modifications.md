# UI Modifications


## Change the font
[Original Post](https://github.com/nk64/GT4286/discussions/10) by horsiezoo

1. Copy a TTF font (eg) ```c:\windows\fonts\comic.ttf``` to the ```/ui``` folder on the SD Card 
2. Edit ```/EasyUI.cfg``` and change:  
    ```"font":"/mnt/extsd/ui/PingFang.ttf"```  
    to  
    ```"font":"/mnt/extsd/ui/comic.ttf"```


## Background music
[Original Post](https://github.com/nk64/GT4286/discussions/5#discussioncomment-11683578) by horsiezoo

Replace ```/ui/bg_music.wav``` with a wav file of your choice.


## Disabling the background music
You can disable the repetitive background music by renaming ```/ui/logo_music.wav``` to ```/ui/logo_music.wav.disabled``` but doing that seems to mean that a normal keytones don't work.

So if you want the annoying beeps but you don't want the background music, then you would need to make or find a wav file with a bit of silence and save it as ```/ui/logo_music.wav``` (untested feedback requested).


## Properly muting the 'Keytone' sounds
1. Use the stock menu to visit the ```Settings Page``` and the go to ```Keytone Setting``` and select ```mute```.
2. Now you don't have annoying beep sounds, but you instead have an annoying fraction of a second of silence interrupting your background music on every joystick and button input.
3. Rename ```/ui/mute.wav``` to ```/ui/mute.wav.disabled``` on your SD Card.


## Disabling the boot logo and boot music cleanly
If you find the time it takes for the boot animation annoying, you can speed up your boot process.

1. Wrap your ```/bin/gmenu2x``` file in a shell script in by following the instructions to [Capturing emulator output](./Capturing%20emulator%20output.md) (except subsituting ```/bin/gmenu2x``` for ```/emus/gamebatte/gambatte_sdl``` of course) 
2. Add the following lines after the last ```log``` statement
```
log "Setting prop.logoboot=1"
/bin/setprop "prop.logoboot" "1"
```
3. Now your GT2486 should get to the menu more quickly by skipping the boot logo and music


## Change the headings and words of the menu system
The main menu text can be changed by editing this file: ```/tr/en_US.json```

The in-emulator menu text is baked into the following images:  
```/menu/2/menubg1.png```  
```/menu/2/menubg.png```

These images might be able to be rebuilt from:
- ```/ui/io_bg.png```
- ```/ui/frame1.png```
- ```/ui/frame2.png```
and some others (joystick, select, start)

Specifically I would like to change:  
- ```Save Progress Bar``` -> ```State Slot```  
- ```Save Progress``` -> ```Save State```  
- ```Read PRogress``` -> ```Load State```  


## Game preview images
The preview images for the installed roms are stored in the ```/roms/{class}/image``` directory

The images do not appear to need to be a specific resolution. They will be scaled to fill the preview box so trial and error will be needed to figure out the ideal height and width. (feedback desired)

They can be ```jpg```, ```png``` or animated ```gifs``` (maybe other formats too), they just need to be named with a ```.jpg``` extension.

The image file needs to match the rom file name for it to be displayed  
Like: ```/roms/FBA/sf2.zip``` -> ```/roms/FBA/image/sf2.jpg```

or if you have applied the [humanise names](./Humanise%20Names.md) trick...  
Like: ```/roms/FBA/Street Fighter II.redir``` -> ```/roms/FBA/image/Street Fighter II.jpg```



## Adding a default preview image for games that don't have one
Add a ```default.jpg``` image to ```/roms/{class}/image``` and any games (in that class) which are missing an image will get this default image.

