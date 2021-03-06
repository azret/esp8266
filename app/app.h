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
#include "gpio.h"	

#define SAP_FACTORY_SSID "Alpha"
#define SAP_FACTORY_PWD ""

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

#define ip2str4(addr) (uint8_t)(addr & 0xFF), (uint8_t)((addr >> 8) & 0xFF), (uint8_t)((addr >> 16) & 0xFF), (uint8_t)((addr >> 24) & 0xFF)

#ifdef __cplusplus
}
#endif
#endif /* __APP_H__ */