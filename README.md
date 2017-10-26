# Windows Build Environment for ESP8266

A standalone build environment for developing custom bootloaders & firmware for ESP8266 on Windows ([ESP8266 NON OS SDK](https://github.com/espressif/ESP8266_NONOS_SDK))

### App

```
#include "app.h"

void ICACHE_FLASH_ATTR start()
{
    log("\nstarted!\n");
}
```