﻿#ifdef __cplusplus
extern "C" {
#endif

#include "user_interface.h"
#include "osapi.h"
#include "espconn.h"
#include "uart.h" 

#define SSL_BUFF_SIZE 8192

extern void ICACHE_FLASH_ATTR start();

LOCAL os_timer_t app_start_timer;

LOCAL void ICACHE_FLASH_ATTR user_start_callback(void *arg)
{
	os_timer_disarm(&app_start_timer);

	espconn_secure_ca_disable(ESPCONN_BOTH);

	espconn_secure_set_size(ESPCONN_CLIENT, SSL_BUFF_SIZE);

	start();
}

// Called when init timer fires

LOCAL void ICACHE_FLASH_ATTR user_init_callback(void)
{
	os_timer_setfn(
		&app_start_timer,
		(os_timer_func_t *)user_start_callback, NULL);

	os_timer_arm(&app_start_timer, 1000, 1);
}
 

// Called by the SDK during pre-initialization

#include "ets_sys.h" 

// Called by the SDK during pre-initialization

#define SPI_FLASH_SIZE_MAP 2

#if ((SPI_FLASH_SIZE_MAP == 0) || (SPI_FLASH_SIZE_MAP == 1))
#error "The flash map is not supported"
#elif (SPI_FLASH_SIZE_MAP == 2)
#define SYSTEM_PARTITION_OTA_SIZE							0x6A000
#define SYSTEM_PARTITION_OTA_2_ADDR							0x81000
#define SYSTEM_PARTITION_RF_CAL_ADDR						0xfb000
#define SYSTEM_PARTITION_PHY_DATA_ADDR						0xfc000
#define SYSTEM_PARTITION_SYSTEM_PARAMETER_ADDR				0xfd000
#elif (SPI_FLASH_SIZE_MAP == 3)
#define SYSTEM_PARTITION_OTA_SIZE							0x6A000
#define SYSTEM_PARTITION_OTA_2_ADDR							0x81000
#define SYSTEM_PARTITION_RF_CAL_ADDR						0x1fb000
#define SYSTEM_PARTITION_PHY_DATA_ADDR						0x1fc000
#define SYSTEM_PARTITION_SYSTEM_PARAMETER_ADDR				0x1fd000
#elif (SPI_FLASH_SIZE_MAP == 4)
#define SYSTEM_PARTITION_OTA_SIZE							0x6A000
#define SYSTEM_PARTITION_OTA_2_ADDR							0x81000
#define SYSTEM_PARTITION_RF_CAL_ADDR						0x3fb000
#define SYSTEM_PARTITION_PHY_DATA_ADDR						0x3fc000
#define SYSTEM_PARTITION_SYSTEM_PARAMETER_ADDR				0x3fd000
#elif (SPI_FLASH_SIZE_MAP == 5)
#define SYSTEM_PARTITION_OTA_SIZE							0x6A000
#define SYSTEM_PARTITION_OTA_2_ADDR							0x101000
#define SYSTEM_PARTITION_RF_CAL_ADDR						0x1fb000
#define SYSTEM_PARTITION_PHY_DATA_ADDR						0x1fc000
#define SYSTEM_PARTITION_SYSTEM_PARAMETER_ADDR				0x1fd000
#elif (SPI_FLASH_SIZE_MAP == 6)
#define SYSTEM_PARTITION_OTA_SIZE							0x6A000
#define SYSTEM_PARTITION_OTA_2_ADDR							0x101000
#define SYSTEM_PARTITION_RF_CAL_ADDR						0x3fb000
#define SYSTEM_PARTITION_PHY_DATA_ADDR						0x3fc000
#define SYSTEM_PARTITION_SYSTEM_PARAMETER_ADDR				0x3fd000
#else
#error "The flash map is not supported"
#endif

static const partition_item_t at_partition_table[] = {
	{ SYSTEM_PARTITION_BOOTLOADER, 						0x0, 												0x1000},
	{ SYSTEM_PARTITION_OTA_1,   						0x1000, 											SYSTEM_PARTITION_OTA_SIZE},
	{ SYSTEM_PARTITION_OTA_2,   						SYSTEM_PARTITION_OTA_2_ADDR, 						SYSTEM_PARTITION_OTA_SIZE},
	{ SYSTEM_PARTITION_RF_CAL,  						SYSTEM_PARTITION_RF_CAL_ADDR, 						0x1000},
	{ SYSTEM_PARTITION_PHY_DATA, 						SYSTEM_PARTITION_PHY_DATA_ADDR, 					0x1000},
	{ SYSTEM_PARTITION_SYSTEM_PARAMETER, 				SYSTEM_PARTITION_SYSTEM_PARAMETER_ADDR, 			0x3000},
};

extern UartDevice UartDev;

// Called by the SDK during initialization

void ICACHE_FLASH_ATTR user_pre_init(void)
{
	UartDev.baut_rate = BIT_RATE_74880;

	uart_div_modify(UART0, UART_CLK_FREQ / (UartDev.baut_rate));

	system_set_os_print(1);

	system_phy_freq_trace_enable(1);

	if (!system_partition_table_regist(at_partition_table, sizeof(at_partition_table) / sizeof(at_partition_table[0]), SPI_FLASH_SIZE_MAP)) {
		os_printf("system_partition_table_regist fail\r\n");
		while (1);
	}
}

void ICACHE_FLASH_ATTR user_init(void)
{
	os_printf("\r\nSDK: v%s\r\n", system_get_sdk_version());
	os_printf("Free Heap: %d\r\n", system_get_free_heap_size());
	os_printf("CPU Frequency: %d MHz\r\n", system_get_cpu_freq());
	os_printf("System Chip ID: %x\r\n", system_get_chip_id());
	os_printf("SPI Flash ID: %x\r\n", spi_flash_get_id());
	os_printf("Flash Size Map: %x\r\n", system_get_flash_size_map());

	uint8 macaddr[6];
	if (!wifi_get_macaddr(STATION_IF, macaddr)) {
		os_printf("wifi_get_macaddr(STATION_IF) failed!\n");
	}
	else {
		os_printf("MAC(STATION_IF): \"%02X-%02X-%02X-%02X-%02X-%02X\"\n", MAC2STR(macaddr));
	}

	if (!wifi_get_macaddr(SOFTAP_IF, macaddr)) {
		os_printf("wifi_get_macaddr(SOFTAP_IF) failed!\n");
	}
	else {
		os_printf("MAC(SOFTAP_IF): \"%02X-%02X-%02X-%02X-%02X-%02X\"\n", MAC2STR(macaddr));
	}

	wifi_softap_dhcps_stop();

	wifi_set_opmode_current(NULL_MODE);

	os_printf("wifi_get_opmode(): \"%d\"\n", wifi_get_opmode());

	system_init_done_cb(&user_init_callback);
}

#ifdef __cplusplus
}
#endif