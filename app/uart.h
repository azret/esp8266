#ifndef __UART_H__
#define __UART_H__

#ifdef __cplusplus
extern "C" {
#endif

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
		uint8* pRcvMsgBuff;
		uint8* pWritePos;
		uint8* pReadPos;
		uint8 TrigLvl;
		RcvMsgBuffState  BuffState;
	} RcvMsgBuff;

	typedef struct {
		uint32   TrxBuffSize;
		uint8* pTrxBuff;
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


#ifdef __cplusplus
}
#endif
#endif /* __UART_H__ */