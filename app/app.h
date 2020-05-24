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