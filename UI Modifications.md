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