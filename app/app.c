#ifdef __cplusplus
extern "C" {
#endif

#include "app.h"

#define HIGH 1
#define LOW 0

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

    if (digitalRead(2)) {

        digitalWrite(2, LOW);
        digitalWrite(5, LOW);

        os_printf("digitalWrite(%d, LOW)\n", 2);
        os_printf("digitalWrite(%d, LOW)\n", 5);

    }
    else {

        digitalWrite(2, HIGH);
        digitalWrite(5, HIGH);

        os_printf("digitalWrite(%d, HIGH)\n", 2);
        os_printf("digitalWrite(%d, HIGH)\n", 5);

    }

    os_timer_arm(&timer, 1000, 1);
}

void ICACHE_FLASH_ATTR start()
{
    log("\n");

    // =================================================

    gpio_init(); 

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

    log("\n\nstarted!\n\n");
}

#ifdef __cplusplus
}
#endif