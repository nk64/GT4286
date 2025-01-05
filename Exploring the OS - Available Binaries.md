# Available Binaries

```
/sbin:
-rwxr-xr-x    1 0        0            55980 Feb  7  2023 init*
lrwxrwxrwx    1 0        0               14 Feb  7  2023 insmod -> ../bin/busybox*
lrwxrwxrwx    1 0        0               14 Feb  7  2023 lsmod -> ../bin/busybox*
lrwxrwxrwx    1 0        0               14 Feb  7  2023 rmmod -> ../bin/busybox*
lrwxrwxrwx    1 0        0                4 Feb  7  2023 ueventd -> init*
```

```
/bin:
-rwxr-xr-x    1 0        0            91972 Feb  7  2023 adbd*
-rwxr-xr-x    1 0        0            87384 Feb  7  2023 busybox*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 cat -> busybox*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 chmod -> busybox*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 chown -> busybox*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 cp -> busybox*
-rwxrwxr-x    1 0        0           111612 Feb  7  2023 daemon*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 date -> busybox*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 df -> busybox*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 echo -> busybox*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 fsync -> busybox*
-rwxr-xr-x    1 0        0            26968 Feb  7  2023 getevent*
-rwxr-xr-x    1 0        0             5464 Feb  7  2023 getprop*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 kill -> busybox*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 ln -> busybox*
-rwxr-xr-x    1 0        0            17808 Feb  7  2023 logcat*
-rwxr-xr-x    1 0        0            58760 Feb  7  2023 logd*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 ls -> busybox*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 mkdir -> busybox*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 mknod -> busybox*
-rwxr-xr-x    1 0        0           210688 Feb  7  2023 mksh*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 mount -> busybox*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 mv -> busybox*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 ps -> busybox*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 pwd -> busybox*
-rwxr-xr-x    1 0        0             5464 Feb  7  2023 reboot*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 rm -> busybox*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 rmdir -> busybox*
-rwxr-xr-x    1 0        0             5464 Feb  7  2023 setprop*
lrwxrwxrwx    1 0        0                4 Feb  7  2023 sh -> mksh*
-rwxrwxr-x    1 0        0             1976 Feb  7  2023 ssd_init.sh*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 sync -> busybox*
-rwxr-xr-x    1 0        0             5468 Feb  7  2023 test_fb*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 touch -> busybox*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 umount -> busybox*
lrwxrwxrwx    1 0        0                7 Feb  7  2023 vi -> busybox*
-rwxr-xr-x    1 0        0            54820 Feb  7  2023 vold*
```


The following commands appear to be compiled in to /bin/busybox but are missing a corresponding soft link in /bin or /sbin:  
- awk
- dirname
- install
- top
- unlink

```/data``` is a writeable filesystem that we could use as location to set up some links if we want to use these tools