#ifdef __cplusplus
extern "C" {
#endif

#include "user_interface.h"
#include "osapi.h"
#include "espconn.h"

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

// uart_register.h"

#define UART_RXD_INV (BIT(19))
#define UART_CTS_INV (BIT(20))
#define UART_RTS_INV (BIT(23))
#define UART_TXD_INV (BIT(22))

// uart.h"

#define UART0   0
#define UART1   1

typedef enum {
	FIVE_BITS = 0x0,
	SIX_BITS = 0x1,
	SEVEN_BITS = 0x2,
	EIGHT_BITS = 0x3
} UartBitsNum4Char;

typedef enum {
	ONE_STOP_BIT = 0x1,
	ONE_HALF_STOP_BIT = 0x2,
	TWO_STOP_BIT = 0x3
} UartStopBitsNum;

typedef enum {
	NONE_BITS = 0x2,
	ODD_BITS = 1,
	EVEN_BITS = 0
} UartParityMode;

typedef enum {
	STICK_PARITY_DIS = 0,
	STICK_PARITY_EN = 1
} UartExistParity;

typedef enum {
	UART_None_Inverse = 0x0,
	UART_Rxd_Inverse = UART_RXD_INV,
	UART_CTS_Inverse = UART_CTS_INV,
	UART_Txd_Inverse = UART_TXD_INV,
	UART_RTS_Inverse = UART_RTS_INV,
} UART_LineLevelInverse;

typedef enum {
	BIT_RATE_300 = 300,
	BIT_RATE_600 = 600,
	BIT_RATE_1200 = 1200,
	BIT_RATE_2400 = 2400,
	BIT_RATE_4800 = 4800,
	BIT_RATE_9600 = 9600,
	BIT_RATE_19200 = 19200,
	BIT_RATE_38400 = 38400,
	BIT_RATE_57600 = 57600,
	BIT_RATE_74880 = 74880,
	BIT_RATE_115200 = 115200,
	BIT_RATE_230400 = 230400,
	BIT_RATE_460800 = 460800,
	BIT_RATE_921600 = 921600,
	BIT_RATE_1843200 = 1843200,
	BIT_RATE_3686400 = 3686400,
} UartBautRate;

typedef enum {
	NONE_CTRL,
	HARDWARE_CTRL,
	XON_XOFF_CTRL
} UartFlowCtrl;

typedef enum {
	USART_HardwareFlowControl_None = 0x0,
	USART_HardwareFlowControl_RTS = 0x1,
	USART_HardwareFlowControl_CTS = 0x2,
	USART_HardwareFlowControl_CTS_RTS = 0x3
} UART_HwFlowCtrl;

typedef enum {
	EMPTY,
	UNDER_WRITE,
	WRITE_OVER
} RcvMsgBuffState;

typedef struct {
	uint32 RcvBuffSize;
	uint8 *pRcvMsgBuff;
	uint8 *pWritePos;
	uint8 *pReadPos;
	uint8 TrigLvl;
	RcvMsgBuffState  BuffState;
} RcvMsgBuff;

typedef struct {
	uint32   TrxBuffSize;
	uint8   *pTrxBuff;
} TrxMsgBuff;

typedef enum {
	BAUD_RATE_DET,
	WAIT_SYNC_FRM,
	SRCH_MSG_HEAD,
	RCV_MSG_BODY,
	RCV_ESC_CHAR,
} RcvMsgState;

typedef struct {
	UartBautRate baut_rate;
	UartBitsNum4Char data_bits;
	UartExistParity  exist_parity;
	UartParityMode parity;
	UartStopBitsNum stop_bits;
	UartFlowCtrl  flow_ctrl;
	RcvMsgBuff  rcv_buff;
	TrxMsgBuff trx_buff;
	RcvMsgState rcv_state;
	int received;
	int buff_uart_no;
} UartDevice;

extern UartDevice UartDev;

// Called by the SDK during pre-initialization

void user_rf_pre_init(void)
{
	system_set_os_print(1);

	// This is a better place to init the UART to capture the SDK os_printf messages

	UartDev.baut_rate = BIT_RATE_74880;

	uart_div_modify(UART0, UART_CLK_FREQ / (UartDev.baut_rate));
	uart_div_modify(UART1, UART_CLK_FREQ / (UartDev.baut_rate));

	os_printf("rf_pre_init();\n");
}

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

// Called by the SDK during initialization

void ICACHE_FLASH_ATTR user_pre_init(void)
{
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

	system_init_done_cb(&user_init_callback);
}

#ifdef __cplusplus
}
#endif