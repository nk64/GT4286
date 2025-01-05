# The Property System

This section is a very incomplete work in progress.

TODO: Explain the property system


## Property System: ```/bin/getprop``` & ```/bin/setprop```
```
usage: setprop <key> <value>
usage: getprop
usage: getprop <key>
```

# getprop (listing all properties)

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


## I think that gmenu2x uses these to remember where it was at after the emulator runs and then quits
|      Property Name      | Property Type |                 Known Values                  |                         Description                         |
|-------------------------|---------------|-----------------------------------------------|-------------------------------------------------------------|
| prop.gameclassselection | Integer       |                                               | The number of the Selected game Class                       |
| prop.inputstr           | String        |                                               | Maybe the text that has been entered in the search screen   |
| prop.keysel             | Integer       |                                               | ?cursel                                                     |
| prop.kind               | Integer       |                                               | ?mListType                                                  |
| prop.logoboot           | Integer       | 0, 1                                          | Whether the Boot animation has been played                  |
| prop.numorletter        | Integer       |                                               | ?_ZNK6ZKBase9isVisibleEv(mListViewKeyBoardPtr);              |
| prop.selection          | Integer       |                                               | Maybe the number of the currently selected game in the list |
| persist.sys.lang        | Integer       | 1-?                                           | System Language                                             |
| prop.title              | String        | 0 , ALL, DOWNLOAD, GAMELIST, SEARCHFTU             | The Main screen tab that was active                         |

