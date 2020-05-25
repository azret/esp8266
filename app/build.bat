@ECHO OFF

@echo off&setlocal

for %%i in ("%~dp0..") do set "root=%%~fi"

@SET xtensa=%root%\bin\xtensa-lx106-elf-gcc
@SET sdk=D:\espressif\ESP8266_NONOS_SDK\
@SET app=%root%\app
@SET lib=%root%\lib
@SET include=%root%\include
@SET esptool=%root%\bin\esptool.exe
@SET watch=%root%\bin\watch.exe

@SET elf=%app%
@IF %elf:~-1%==\ SET elf=%elf:~0,-1%
@SET elf=%elf%\.elf
@IF exist "%elf%" (
    @del /S /F /Q "%elf%" >nul
)
@IF not exist "%elf%" (
    @md "%elf%"
)

@IF not exist "%elf%/user_config.h" (
    @echo // user_config.h > "%elf%/user_config.h"
)

@SET flags=-c -Wall -Wextra -Os -g -Wno-unused-variable -Wno-unused-value -Wno-unused-parameter -Wno-unused-function -Wno-parentheses -Wpointer-arith -Wno-implicit-function-declaration -Wl,-EL -fno-inline-functions -fno-exceptions -nostdlib -mlongcalls -mtext-section-literals -falign-functions=4 -MMD -std=gnu99 -ffunction-sections -fdata-sections

"%xtensa%\bin\xtensa-lx106-elf-gcc" -D__ets__ -DICACHE_FLASH -U__STRICT_ANSI__ "-I%include%" "-I%sdk%\include" "-I%sdk%\lwip\include" %flags% -DF_CPU=80000000L -DESP8266 "-I%app%" "-I%app%\include" "-I%elf%" "%app%\boot.c" -o "%app%\.elf\boot.c.o"
@IF %ERRORLEVEL% GTR 0 @GOTO FAIL

"%xtensa%\bin\xtensa-lx106-elf-gcc" -D__ets__ -DICACHE_FLASH -U__STRICT_ANSI__ "-I%include%" "-I%sdk%\include" "-I%sdk%\lwip\include" %flags% -DF_CPU=80000000L -DESP8266 "-I%app%" "-I%app%\include" "-I%elf%" "%app%\app.c" -o "%app%\.elf\app.c.o"
@IF %ERRORLEVEL% GTR 0 @GOTO FAIL

"%xtensa%\bin\xtensa-lx106-elf-gcc" -D__ets__ -DICACHE_FLASH -U__STRICT_ANSI__ "-I%include%" "-I%sdk%\include" "-I%sdk%\lwip\include" %flags% -DF_CPU=80000000L -DESP8266 "-I%app%" "-I%app%\include" "-I%elf%" "%app%\serv.c" -o "%app%\.elf\serv.c.o"
@IF %ERRORLEVEL% GTR 0 @GOTO FAIL

"%xtensa%\bin\xtensa-lx106-elf-gcc" -g -Wall -Wextra -Os -nostdlib -Wl,--no-check-sections  -Wl,-static "-L%lib%" "-L%sdk%\lib" "-L%sdk%\ld" "-Teagle.app.v6.ld" -Wl,--gc-sections  -o "%app%\.elf\app.elf" -Wl,--start-group  "%app%\.elf\boot.c.o" "%app%\.elf\app.c.o" "%app%\.elf\serv.c.o" -lc -lgcc -lhal -lphy -lpp -lnet80211 -lwpa -lcrypto -lmain -lssl  -llwip -Wl,--end-group
@IF %ERRORLEVEL% GTR 0 @GOTO FAIL

@SET FLASH=1

"%esptool%" -v -bz 1M -eo "%app%\.elf\app.elf" -bo "%app%\.elf\0x00000.bin" -bs .data -bs .rodata -bs .bss -bs .text -bc -ec -eo "%app%\.elf\app.elf" -es .irom0.text "%app%\.elf\0x10000.bin" -ec
@IF %ERRORLEVEL% GTR 0 @GOTO FAIL

@IF [%FLASH%] EQU [1] (

    "%esptool%" -v -cb 921600 -cp COM3 -cf "%app%\.elf\0x00000.bin" -ca 0x10000 -cf "%app%\.elf\0x10000.bin"
    @IF %ERRORLEVEL% GTR 0 @GOTO FAIL

    "%watch%" --port COM3 --baud 74880"
)

:Fail
:End
