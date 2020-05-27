#ifdef __cplusplus
extern "C" {
#endif

#include "app.h"

#define HIGH 1
#define LOW 0

    /*LOCAL int GPIO_PIN_REGISTERS[16] = {

        PERIPHS_IO_MUX_GPIO0_U,
        PERIPHS_IO_MUX_U0TXD_U,
        PERIPHS_IO_MUX_GPIO2_U,
        PERIPHS_IO_MUX_U0RXD_U,
        PERIPHS_IO_MUX_GPIO4_U,
        PERIPHS_IO_MUX_GPIO5_U,
        PERIPHS_IO_MUX_SD_CLK_U,
        PERIPHS_IO_MUX_SD_DATA0_U,
        PERIPHS_IO_MUX_SD_DATA1_U,
        PERIPHS_IO_MUX_SD_DATA2_U,
        PERIPHS_IO_MUX_SD_DATA3_U,
        PERIPHS_IO_MUX_SD_CMD_U,
        PERIPHS_IO_MUX_MTDI_U,
        PERIPHS_IO_MUX_MTCK_U,
        PERIPHS_IO_MUX_MTMS_U,
        PERIPHS_IO_MUX_MTDO_U

    };

    LOCAL void ICACHE_FLASH_ATTR
        pinMode(uint8_t pin, uint8_t mode, uint8_t pullup) {
        if ((0x1 << pin) & 0b110101) {
            PIN_FUNC_SELECT(GPIO_PIN_REGISTERS[pin], 0);
        }
        else {
            PIN_FUNC_SELECT(GPIO_PIN_REGISTERS[pin], 3);
        }

        if (pullup)
            PIN_PULLUP_EN(GPIO_PIN_REGISTERS[pin]);
        else
            PIN_PULLUP_DIS(GPIO_PIN_REGISTERS[pin]);

        if (mode) {
            GPIO_REG_WRITE(GPIO_ENABLE_W1TC_ADDRESS, 1 << pin);
        }
        else {
            GPIO_REG_WRITE(GPIO_ENABLE_W1TS_ADDRESS, 1 << pin);
        }
    }
    */
     
 LOCAL void ICACHE_FLASH_ATTR
     digitalWrite(uint8_t pin, uint8_t state) {
     if (state) {
         GPIO_REG_WRITE(GPIO_OUT_W1TS_ADDRESS, 1 << pin); // set GPIO pin high
     }
     else {
         GPIO_REG_WRITE(GPIO_OUT_W1TC_ADDRESS, 1 << pin); // set GPIO pin low
     }
 }

 LOCAL int ICACHE_FLASH_ATTR
     digitalRead(uint8_t pin) {
     return (GPIO_REG_READ(GPIO_OUT_ADDRESS) >> pin) & 1;
 }


LOCAL os_timer_t timer;

void ICACHE_FLASH_ATTR	ADC_TEST(void* p)
{
    os_timer_disarm(&timer);

    os_timer_setfn(&timer, ADC_TEST, NULL);

    /*
    if (digitalRead(3)) {

        digitalWrite(3, LOW);

        os_printf("digitalWrite(%d, LOW)\n", 3);

    }
    else {

        digitalWrite(3, HIGH);

        os_printf("digitalWrite(%d, HIGH)\n", 3);

    }
    */

    os_timer_arm(&timer, 1000, 1);
}

void ICACHE_FLASH_ATTR start()
{
    os_printf("\n");

    // =================================================

    gpio_init(); 

    // LED on GPIO3

    ETS_GPIO_INTR_DISABLE();

    PIN_FUNC_SELECT(PERIPHS_IO_MUX_U0RXD_U, FUNC_GPIO3);

    PIN_PULLUP_DIS(PERIPHS_IO_MUX_U0RXD_U);

    GPIO_REG_WRITE(GPIO_ENABLE_W1TS_ADDRESS, 1 << 3);

    ETS_GPIO_INTR_ENABLE();


    // LED on GPIO5

    ETS_GPIO_INTR_DISABLE();

    PIN_FUNC_SELECT(PERIPHS_IO_MUX_GPIO5_U, FUNC_GPIO5);

    PIN_PULLUP_DIS(PERIPHS_IO_MUX_GPIO5_U);

    GPIO_REG_WRITE(GPIO_ENABLE_W1TS_ADDRESS, 1 << 5);

    ETS_GPIO_INTR_ENABLE();


    // LED on GPIO2

    ETS_GPIO_INTR_DISABLE();

    PIN_FUNC_SELECT(PERIPHS_IO_MUX_GPIO2_U, FUNC_GPIO2);

    PIN_PULLUP_DIS(PERIPHS_IO_MUX_GPIO2_U);

    GPIO_REG_WRITE(GPIO_ENABLE_W1TS_ADDRESS, 1 << 2);

    ETS_GPIO_INTR_ENABLE();

    // =================================================

    digitalWrite(2, HIGH);
    digitalWrite(5, HIGH);

    ADC_TEST(NULL);

    os_printf("\n\nstarted!\n\n");
}

#ifdef __cplusplus
}
#endif