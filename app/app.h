#ifndef __APP_H__
#define __APP_H__

#ifdef __cplusplus
extern "C" {
#endif

#define VERBOSE

#include "ip_addr.h"
#include "user_config.h"
#include "c_types.h"
#include "os_type.h" // TIMER
#include "osapi.h"
#include "mem.h"
#include "user_interface.h"
#include "espconn.h"
#include "pwm.h"
	
#define INPUT             0x00
#define INPUT_PULLUP      0x02
#define INPUT_PULLDOWN_16 0x04 // PULLDOWN only possible for pin16
#define OUTPUT            0x01
#define OUTPUT_OPEN_DRAIN 0x03
#define WAKEUP_PULLUP     0x05
#define WAKEUP_PULLDOWN   0x07
#define SPECIAL           0xF8 //defaults to the usable BUSes uart0rx/tx uart1tx and hspi
#define FUNCTION_0        0x08
#define FUNCTION_1        0x18
#define FUNCTION_2        0x28
#define FUNCTION_3        0x38
#define FUNCTION_4        0x48

// #include "esp8266_peri.h"
// 
// #include "i2s.h"

#define ip2str4(addr) (uint8_t)(addr & 0xFF), (uint8_t)((addr >> 8) & 0xFF), (uint8_t)((addr >> 16) & 0xFF), (uint8_t)((addr >> 24) & 0xFF)

#ifdef VERBOSE
#define log(...) os_printf( __VA_ARGS__ )
#define DBG log
#else
#define log(...)
#endif

#if defined(ARDUINO)
#ifndef DEBUG_ESP_CORE
#define DEBUG_ESP_CORE
#endif
#define DEBUG_WIFI(...) log( __VA_ARGS__ )
#else
#ifndef DEBUG_WIFI
#define DEBUG_WIFI(...)
#endif
#endif
	
#ifdef __cplusplus
}
#endif

#endif /* __APP_H__ */