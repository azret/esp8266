# Windows Build Environment for ESP8266

A standalone build environment for developing custom bootloaders & firmware for ESP8266 on Windows.

```c
#include "app.h"

void ICACHE_FLASH_ATTR start()
{
    log("\nstarted!\n");
}
```

Run **BUILD.bat** to compile and flash the app.

### Links

- ([ESP8266 NON OS SDK](https://github.com/espressif/ESP8266_NONOS_SDK))
