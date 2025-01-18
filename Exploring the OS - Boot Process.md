# From Boot to Emulator


- `/sbin/init`
    - `/etc/init.rc`
        - `/bin/vold`
            - `/etc/vold.fstab` (mounts /mnt/extsd)
        - `/bin/ssd_init.sh`
        - `/bin/daemon`

        - `/mnt/extsd/bin/gmenu2x`
            - `execlp("/bin/sh", "/bin/sh", "-c", "/mnt/extsd/emus/fbneo/fbneo /roms/fba/sf2.zip", (char *)NULL);`
                ```
                The exec() family of functions replaces the current process image with a new process image.
                ```
                [execlp(3) man page](https://linux.die.net/man/3/execlp)


Doco about init and init.rc
* [init.README](https://android.googlesource.com/platform/system/core/+/master/init/README.md)
* [android-init-stages](https://sx.ix5.org/info/android-init-stages/)
* [newandroidbook Chapter IV - Android Runtime Services](https://newandroidbook.com/Book/servs)


## Open questions:
- What are the steps to `/mnt/etxsd/bin/gmenu2x` getting launched?
    - it is defined as the service name `game` in `/etc/init.rc`
    - **but** it is **disabled** by default
    - later when we run the `debug-getprops` User Script we can see that the `game` service among others defined in `/etc/init.rc` is started.
        ```
        [init.svc.adbd]: [running]
        [init.svc.console]: [running]
        [init.svc.daemon]: [running]
        [init.svc.game]: [running]
        [init.svc.logd]: [running]
        [init.svc.ssd]: [stopped]
        [init.svc.ueventd]: [running]
        [init.svc.vold]: [running]
        ```

    - `gmenu2x` can't possibly start until after the SD card is mounted to `/mnt/extsd`

    - So there must be something that happens after `/mnt/extsd` is mounted that causes the `game` service to be started (and not just `started` but `enabled` so that it can be automaticlly re-started after each emulator exits)

    - there is a process which is watching for the insertion and removal of the SD card.
        - When the SD card is removed, gmenu2x is killed and a "No Micro SD Card" image is displayed
        - When the SD card is re-inserted, gmenu2x is started again.
        - could it be `/bin/daemon`?
            - there is an image file in `/etc/warning.jpg` which matches the "No Micro SD Card" image that is displayed but:
                - I can't find any strings in `/bin/daemon` that refer to it or `gmenu2x` or `game`
                - I can't find any files in the OS that contain the string `warning.jpg` (nor `gmenu2x` other than `/etc/init.rc`)
                - has it been obfuscated in some way?

            - A similar image was referred to on the [odroid forum](https://forum.odroid.com/viewtopic.php?t=37879)

## /etc/init.rc
```
import /init.${ro.hardware}.rc

on early-init
    start ueventd

sysclktz 0

loglevel 8

    export PATH /sbin:/bin:
    export LD_LIBRARY_PATH /lib:/mnt/extsd/lib
    export ANDROID_ROOT /
    export ANDROID_DATA /data

on fs
    wait /dev/block/mtdblock4
    mount squashfs /dev/block/mtdblock4 /config noatime nodiratime
    mount jffs2  mtd:data /data noatime nodiratime
    wait /dev/block/mtdblock3
    mount squashfs  /dev/block/mtdblock3 /res noatime nodiratime

on post-fs-data
    chmod 0771 /data
    mkdir /data/property 0700 root root
    mkdir /data/local 0751 root root

on boot
    ifup lo
    hostname localhost
    domainname localdomain
    setrlimit 13 40 40
    chown root radio /proc/cmdline
    class_start core
    class_start main

on nonencrypted
    class_start late_start

service ueventd /sbin/ueventd
    class core
    critical

service console /bin/sh
    class core
    disabled
    console
    user root
    group log

service ssd /bin/ssd_init.sh
    class main
    oneshot
    user root

on property:ro.debuggable=1
    start console

service vold /bin/vold
    class core
    socket vold stream 0660 root mount
    ioprio be 2

on property:persist.sys.zkdebug=1
    start adbd

on property:persist.sys.zkdebug=0
    stop adbd

service zkswe /bin/zkgui
    class main
    disabled
    user root

service logd /bin/logd
    class core
    socket logd stream 0666 logd logd
    socket logdr seqpacket 0666 logd logd
    socket logdw dgram 0222 logd logd
    
service game /mnt/extsd/bin/gmenu2x
    class main
    user root
    disabled

service daemon /bin/daemon
    class main
    user root

service adbd /bin/adbd
    class core
    disabled
    socket adbd stream 660 system system

on property:sys.usb.config=adb
    write /sys/class/android_usb/android0/enable        0
    write /sys/class/android_usb/android0/idVendor      18d1
    write /sys/class/android_usb/android0/idProduct     D002
    write /sys/class/android_usb/android0/functions     ${sys.usb.config}
    write /sys/class/android_usb/android0/enable        1
    setprop sys.usb.state ${sys.usb.config}
```

## /etc/vold.fstab
```
# Mounts the first usable partition of the specified device
#/devices/platform/awsmc.3/mmc_host for sdio
dev_mount	extsd	/mnt/extsd	auto	/devices/soc0/soc/soc:sdmmc/mmc_host
dev_mount	usb1	/mnt/usb1	auto	/devices/soc0/soc/soc:Sstar-ehci-1/usb2/2-1/2-1:1.0
dev_mount	usb2	/mnt/usb2	auto	/devices/soc0/soc/soc:Sstar-ehci-2/usb1/1-1/1-1:1.0
dev_mount	usb3	/mnt/usb3	auto	/devices/platform/sunxi-ehci.1/usb1/1-1/1-1.3
```

## /etc/ssd_init.sh
```sh
#!/bin/sh

insmod /config/modules/nls_utf8.ko
insmod /config/modules/mdrv_crypto.ko

#kernel_mod_list
insmod /config/modules/mhal.ko

#misc_mod_list
insmod /config/modules/mi_common.ko
major=`cat /proc/devices | busybox awk "\\$2==\""mi"\" {print \\$1}"\n`
minor=0

insmod /config/modules/mi_sys.ko cmdQBufSize=128 logBufSize=0
if [ $? -eq 0 ]; then
    busybox mknod /dev/mi_sys c $major $minor
    let minor++
fi

insmod /config/modules/mi_gfx.ko
if [ $? -eq 0 ]; then
    busybox mknod /dev/mi_gfx c $major $minor
    let minor++
fi

insmod /config/modules/mi_divp.ko
if [ $? -eq 0 ]; then
    busybox mknod /dev/mi_divp c $major $minor
    let minor++
fi

insmod /config/modules/mi_vdec.ko
if [ $? -eq 0 ]; then
    busybox mknod /dev/mi_vdec c $major $minor
    let minor++
fi

insmod /config/modules/mi_hdmi.ko
if [ $? -eq 0 ]; then
    busybox mknod /dev/mi_hdmi c $major $minor
    let minor++
fi

insmod /config/modules/mi_ao.ko
if [ $? -eq 0 ]; then
    busybox mknod /dev/mi_ao c $major $minor
    let minor++
fi

insmod /config/modules/mi_cipher.ko
if [ $? -eq 0 ]; then
    busybox mknod /dev/mi_cipher c $major $minor
    let minor++
fi

insmod /config/modules/mi_disp.ko
if [ $? -eq 0 ]; then
    busybox mknod /dev/mi_disp c $major $minor
    let minor++
fi

insmod /config/modules/mi_ipu.ko
if [ $? -eq 0 ]; then
    busybox mknod /dev/mi_ipu c $major $minor
    let minor++
fi

insmod /config/modules/mi_ai.ko
if [ $? -eq 0 ]; then
    busybox mknod /dev/mi_ai c $major $minor
    let minor++
fi

insmod /config/modules/mi_venc.ko
if [ $? -eq 0 ]; then
    busybox mknod /dev/mi_venc c $major $minor
    let minor++
fi

insmod /config/modules/mi_panel.ko
if [ $? -eq 0 ]; then
    busybox mknod /dev/mi_panel c $major $minor
    let minor++
fi

#mi module
major=`cat /proc/devices | busybox awk "\\$2==\""mi_poll"\" {print \\$1}"`
busybox mknod /dev/mi_poll c $major 0
insmod /config/modules/fbdev.ko
#misc_mod_list_late

insmod /config/modules/cifs.ko
insmod /config/modules/ehci-hcd.ko
insmod /config/modules/usb-storage.ko
```