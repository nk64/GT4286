# Input Devices 

The GT4286 comes with the following Input devices

```
Available devices:
  /dev/input/event0: mcu_ts
  /dev/input/event1: mcu_ts
  /dev/input/event69: Twin USB Gamepad
  /dev/input/event70: Twin USB Gamepad
```

Event Test Utility:
- `/_gt4286util/bin/evtest`
    ```
    USAGE:
    All Capabilities mode:
      evtest [--allcaps]
        --allcaps  dump the capabilites of all event devices

    Capture mode:
      evtest [--grab] /dev/input/eventX
        --grab  grab the device for exclusive access

    Query mode: (check exit code)
      evtest --query /dev/input/eventX <type> <value>

    <type> is one of: EV_KEY, EV_SW, EV_LED, EV_SND
    <value> can either be a numerical value, or the textual name of the key/switch/LED/sound being queried (e.g. SW_DOCK).
    ```
- User Script to [Dump Capabilities](./sdcard_tweaks/roms/MAME/test-evtest-dump-capabilities.sh)
    ```
    evtest="${GT4286UTIL_HOME}/bin/evtest"
    "${evtest}" --allcaps > "${output_dir}/test-evtest-dump-capabilities.txt" 2>&1
    ```

- User Script to [Capture Events](./sdcard_tweaks/roms/MAME/test-evtest-capture.sh)
    * Script will capture input for 20 seconds (from green screen to blue screen)

[Linux Kernel Event Codes](https://www.kernel.org/doc/Documentation/input/event-codes.txt)

