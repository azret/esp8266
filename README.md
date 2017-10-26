# Windows Build Environment for ESP8266

## app\build.bat

```
esptool v0.4.8 - (c) 2014 Ch. Klippel <ck@atelier-klippel.de>
setting flash size from 512K to 1M
using ELF file "D:\Vie d'Artiste\esp8266\app\.elf\app.elf"
created structure for binimage "D:\Vie d'Artiste\esp8266\app\.elf\0x00000.bin" with entry address 0x40100004
added segment #0 to binimage for address 0x3FFE8000 with size 0x000007E4
added section .data at 0x3FFE8000 size 0x000007E4
set bimage entry to 0x40100004
added segment #1 to binimage for address 0x3FFE87F0 with size 0x00000EAC
added section .rodata at 0x3FFE87F0 size 0x00000EAC
set bimage entry to 0x40100004
added segment #2 to binimage for address 0x3FFE96A0 with size 0x00006540
added section .bss at 0x3FFE96A0 size 0x00006540
set bimage entry to 0x40100004
added segment #3 to binimage for address 0x40100000 with size 0x00006AC8
added section .text at 0x40100000 size 0x00006AC8
set bimage entry to 0x40100004
saved binimage file, total size is 59088 bytes, checksum byte is 0xD2
using ELF file "D:\Vie d'Artiste\esp8266\app\.elf\app.elf"
saved section ".irom0.text" to file "D:\Vie d'Artiste\esp8266\app\.elf\0x10000.bin"
esptool v0.4.8 - (c) 2014 Ch. Klippel <ck@atelier-klippel.de>
opening bootloader
resetting board
trying to connect
trying to connect
Uploading 59088 bytes from to flash at 0x00000000
..........................................................
Uploading 248748 bytes from to flash at 0x00010000
...................................................................................................................................................................................................................................................
starting app without reboot
closing bootloader

SDK ver: 2.1.0(116b762) compiled @ May  5 2017 16:08:55
phy ver: 1134_0, pp ver: 10.2

user_init();
mode : null

started!
```
