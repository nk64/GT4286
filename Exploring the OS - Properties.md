# The Property System

This section is a very incomplete work in progress.

TODO: Explain the property system

Note: The property name/key must be 31 characters or less in length

## ```/bin/getprop``` & ```/bin/setprop```
```
usage: setprop <key> <value>
usage: getprop
usage: getprop <key>
```

### getprop (listing all properties)
|           Property            |               Value               |
|-------------------------------|-----------------------------------|
| init.svc.adbd                 | running                           |
| init.svc.console              | running                           |
| init.svc.daemon               | running                           |
| init.svc.game                 | running                           |
| init.svc.logd                 | running                           |
| init.svc.ssd                  | stopped                           |
| init.svc.ueventd              | running                           |
| init.svc.vold                 | running                           |
| ro.baseband                   | unknown                           |
| ro.bootloader                 | unknown                           |
| ro.bootmode                   | unknown                           |
| ro.build.description          | zkos                              |
| ro.build.display.id           | zkos,flythings                    |
| ro.build.fingerprint          | ZKOS,flythings(see www.zkswe.com) |
| ro.build.git                  | 2a96044                           |
| ro.build.version.codename     | flythings                         |
| ro.build.version.release      | flythingsV2.1                     |
| ro.build.version.sdk          | flythings                         |
| ro.debuggable                 | 1                                 |
| ro.factorytest                | 0                                 |
| ro.firmware                   | ssd20x_develop                    |
| ro.hardware                   | sstarsoc(flatteneddevicetree)     |
| ro.product.board              | swaio                             |
| ro.product.device             | swaio                             |
| ro.product.manufacturer       | zkswe                             |
| ro.product.model              | Zkswe_SSD20X_SPINOR               |
| ro.product.name               | swaio                             |
| ro.revision                   | 0                                 |
| ro.serialno                   |                                   |
| ro.system.version             | 2.6.2                             |
| service.adb.tcp.port          | 5555                              |
| sys.usb.config                | adb                               |
| sys.usb.state                 | adb                               |
| sys.zkupgrade.force           | 1                                 |


## Properties whos name starts with ```persist``` persist across reboots
|           Property            |               Value               |
|-------------------------------|-----------------------------------|
| persist.sys.lang              | 2                                 |
| persist.sys.usb.config        | adb                               |
| persist.sys.zkdebug           | 1                                 |


## gmenu2x uses these properties to remember where it was at after the emulator runs and then quits
|      Property Name      | Property Type |                 Known Values                  |                         Description                         |
|-------------------------|---------------|-----------------------------------------------|-------------------------------------------------------------|
| prop.logoboot           | Integer       | 0, 1                                          | Whether the Boot animation has been played                  |
| prop.title              | String        | 0 , ALL, DOWNLOAD, GAMELIST, SEARCHFTU        | The Main screen tab that was active                         |
| prop.kind               | Integer       | 0, 2, 3                 | when prop.title="ALL": 0=List, 2=History, 3=Collect                                        |
| prop.selection          | Integer       |                                               | 0 based index of the currently selected game in the list    |
| prop.gameclassselection | Integer       | 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 | 0 based index of the selected game Class: 0=FBA, 1=FC, 2=GB, 3=GBA, 4=GBC, 5=MD, 6=SFC, 7=PS1, 8=MAME, 9=PCE |
| prop.inputstr           | String        |                                               | The text that has been entered in the search screen         |
| prop.keysel             | Integer       |                                               | ???                                                         |
| prop.numorletter        | Integer       | 0, 1       | The type of the keyboard in the search screen: 0=Numeric Keyboard, 1=Alphabet Keyboard               |


## We can force gmenu2x to open at a specific screen

```bash
    ## List ##
    /bin/setprop "prop.title" "ALL"
    /bin/setprop "prop.kind" "0"
    /bin/setprop "prop.selection" "0" # position in list (0=first entry in list)

    ## Class ##
    /bin/setprop "prop.title" "GAMELIST"
    # Class 0=FBA, 1=FC, 2=GB, 3=GBA, 4=GBC, 5=MD, 6=SFC, 7=PS1, 8=MAME, 9=PCE
    /bin/setprop "prop.gameclassselection" "0"
    /bin/setprop "prop.selection" "0" # position in list (0=first entry in list)

    ## History ##
    /bin/setprop "prop.title" "ALL"
    /bin/setprop "prop.kind" "2"
    /bin/setprop "prop.selection" "0" # position in list (0=first entry in list)

    ## Collect ##
    /bin/setprop "prop.title" "ALL"
    /bin/setprop "prop.kind" "3"
    /bin/setprop "prop.selection" "0" # position in list (0=first entry in list)

    ## Search ##
    /bin/setprop "prop.title" "SEARCHFTU"
    /bin/setprop "prop.inputstr" "street fi" # the text that has been entered into the search
    /bin/setprop "prop.keysel" "0"      # ??
    /bin/setprop "prop.numorletter" "1" # 0=Numeric Keyboard, 1=Alphabet Keyboard
    /bin/setprop "prop.selection" "0"   # position in list (0=first entry in list)
```