#ifdef __cplusplus
extern "C" {
#endif

#include "app.h"

    extern bool ICACHE_FLASH_ATTR serv(uint32 port);

    LOCAL bool ICACHE_FLASH_ATTR wifi_set_softap(const char* ssid, const char* pwd) {

        if (ssid && os_strlen(ssid) > 0 && (os_strlen(ssid) > 31 || os_strlen(ssid) < 3)) {
            os_printf("invalid \"ssid\"!\n");
            return false;
        }

        if (pwd && os_strlen(pwd) > 0 && (os_strlen(pwd) > 63 || os_strlen(pwd) < 8)) {
            os_printf("invalid \"pwd\"!\n");
            return false;
        }

        struct softap_config conf;

        os_memset(
            &conf,
            0, sizeof(conf));

        wifi_softap_dhcps_stop();

        if (ssid && os_strlen(ssid) > 0) {

            conf.channel = 1;
            conf.ssid_hidden = 0;

            conf.ssid_len = os_strlen(ssid);

            os_strcpy(
                (char*)(conf.ssid),
                ssid);

            conf.max_connection = 4;
            conf.beacon_interval = 100;

            if (!pwd || os_strlen(pwd) == 0) {
                conf.authmode = AUTH_OPEN;
                *conf.password = 0;
            }
            else {
                conf.authmode = AUTH_WPA2_PSK;
                os_strcpy((char*)(conf.password), pwd);
            }

        }
        else {

            ssid = 0;

        }

        bool res = wifi_softap_set_config_current(&conf);

        if (!res) {
            os_printf(
                "wifi_softap_set_config_current(\"%s\") failed!\n", ssid);

            return false;
        }

        if (!ssid) {
            return true;
        }

        if (wifi_softap_dhcps_status() != DHCP_STARTED) {

            if (!wifi_softap_dhcps_start() || (wifi_softap_dhcps_status() != DHCP_STARTED)) {

                os_printf("wifi_softap_dhcps_start() failed!\n");

                os_memset(
                    &conf, 0, sizeof(conf));

                wifi_softap_set_config_current(&conf);

                return false;

            }

        }

        struct ip_info inf;

        if (!wifi_get_ip_info(SOFTAP_IF, &inf)) {

            os_printf(
                "wifi_get_ip_info() failed!\n");

            return false;

        }

        os_printf("ap: %u.%u.%u.%u\n", (inf.ip.addr >> (8 * 0)) & 0xff, (inf.ip.addr >> (8 * 1)) & 0xff, (inf.ip.addr >> (8 * 2)) & 0xff, (inf.ip.addr >> (8 * 3)) & 0xff);

        return true;

    }

    LOCAL void ICACHE_FLASH_ATTR wifi_event_handler(void* arg) {

        System_Event_t* e = (System_Event_t*)(arg);

        switch (e->event) {

        case EVENT_SOFTAPMODE_PROBEREQRECVED:

            os_printf("EVENT_SOFTAPMODE_PROBEREQRECVED: \"%02X-%02X-%02X-%02X-%02X-%02X\", rssi: %d\n",
                MAC2STR(e->event_info.ap_probereqrecved.mac),
                e->event_info.ap_probereqrecved.rssi);

            break;

        case EVENT_SOFTAPMODE_STACONNECTED:

            os_printf("EVENT_SOFTAPMODE_STACONNECTED: \"%02X-%02X-%02X-%02X-%02X-%02X\"\n", MAC2STR(e->event_info.sta_connected.mac));
            os_printf("aid: %d\n", e->event_info.sta_connected.aid);

            break;

        case EVENT_STAMODE_CONNECTED:

            os_printf("EVENT_STAMODE_CONNECTED\n");
            break;

        case EVENT_STAMODE_DISCONNECTED:

            os_printf("EVENT_STAMODE_DISCONNECTED\n");
            break;

        case EVENT_STAMODE_GOT_IP:

            os_printf("EVENT_STAMODE_GOT_IP\n");
            break;

        case EVENT_OPMODE_CHANGED:

            os_printf("EVENT_OPMODE_CHANGED: %d\n", e->event_info.opmode_changed.new_opmode);

            struct ip_info inf;

            os_memset(
                &inf,
                0, sizeof(inf));

            if (wifi_get_ip_info(SOFTAP_IF, &inf)) {
                if (inf.ip.addr != 0) {
                    os_printf("listen: %d.%d.%d.%d:80\n", ip2str4(inf.ip.addr));
                    if (!serv(80)) {
                        os_printf("serv(%d) failed!\n", 80);
                    }
                }
            }

            break;

        default:

            os_printf("evt %d\n", e->event);
            break;

        }
    }

    void ICACHE_FLASH_ATTR start()
    {
        // LED on GPIO2
 
        ETS_GPIO_INTR_DISABLE();        
        PIN_FUNC_SELECT(PERIPHS_IO_MUX_GPIO2_U, FUNC_GPIO2);        
        PIN_PULLUP_DIS(PERIPHS_IO_MUX_GPIO2_U);        
        GPIO_REG_WRITE(GPIO_ENABLE_W1TS_ADDRESS, 1 << 2);
        ETS_GPIO_INTR_ENABLE();

        os_printf("\n");

        /**
        */

        uint8 macaddr[6];

        if (!wifi_get_macaddr(STATION_IF, macaddr)) {

            os_printf(
                "wifi_get_macaddr(STATION_IF) failed!\n");

            return;

        }

        os_printf(
            "MAC(STATION_IF): \"%02X-%02X-%02X-%02X-%02X-%02X\"\n", MAC2STR(macaddr));

        /**
        */

        if (!wifi_get_macaddr(SOFTAP_IF, macaddr)) {

            os_printf(
                "wifi_get_macaddr(SOFTAP_IF) failed!\n");

            return;

        }

        os_printf(
            "MAC(SOFTAP_IF): \"%02X-%02X-%02X-%02X-%02X-%02X\"\n", MAC2STR(macaddr));

        /**
        */

        os_printf("\n");

        wifi_set_event_handler_cb((wifi_event_handler_cb_t)wifi_event_handler);

        if (!wifi_set_opmode_current(SOFTAP_MODE))
        {
            os_printf(
                "wifi_set_opmode_current(SOFTAP_MODE) failed!\n");

            return;
        }

        if (!wifi_set_softap(SAP_FACTORY_SSID, SAP_FACTORY_PWD))
        {
            os_printf(
                "wifi_set_softap(SOFTAP_MODE) failed!\n");

            return;
        }

        os_printf("\n\nstarted!\n\n");
    }

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
     


// LOCAL os_timer_t timer;
// 
// void ICACHE_FLASH_ATTR	ADC_TEST(void* p)
// {
//     os_timer_disarm(&timer);
// 
//     os_timer_setfn(&timer, ADC_TEST, NULL);
// 
//     /*
//     if (digitalRead(3)) {
// 
//         digitalWrite(3, LOW);
// 
//         os_printf("digitalWrite(%d, LOW)\n", 3);
// 
//     }
//     else {
// 
//         digitalWrite(3, HIGH);
// 
//         os_printf("digitalWrite(%d, HIGH)\n", 3);
// 
//     }
//     */
// 
//     os_timer_arm(&timer, 1000, 1);
// }

// void ICACHE_FLASH_ATTR start()
// {
//     os_printf("\n");
// 
//     // =================================================
// 
//     gpio_init(); 
// 
//     // LED on GPIO3
// 
//     ETS_GPIO_INTR_DISABLE();
// 
//     PIN_FUNC_SELECT(PERIPHS_IO_MUX_U0RXD_U, FUNC_GPIO3);
// 
//     PIN_PULLUP_DIS(PERIPHS_IO_MUX_U0RXD_U);
// 
//     GPIO_REG_WRITE(GPIO_ENABLE_W1TS_ADDRESS, 1 << 3);
// 
//     ETS_GPIO_INTR_ENABLE();
// 
// 
//     // LED on GPIO5
// 
//     ETS_GPIO_INTR_DISABLE();
// 
//     PIN_FUNC_SELECT(PERIPHS_IO_MUX_GPIO5_U, FUNC_GPIO5);
// 
//     PIN_PULLUP_DIS(PERIPHS_IO_MUX_GPIO5_U);
// 
//     GPIO_REG_WRITE(GPIO_ENABLE_W1TS_ADDRESS, 1 << 5);
// 
//     ETS_GPIO_INTR_ENABLE();
// 
// 
//     // LED on GPIO2
// 
//     ETS_GPIO_INTR_DISABLE();
// 
//     PIN_FUNC_SELECT(PERIPHS_IO_MUX_GPIO2_U, FUNC_GPIO2);
// 
//     PIN_PULLUP_DIS(PERIPHS_IO_MUX_GPIO2_U);
// 
//     GPIO_REG_WRITE(GPIO_ENABLE_W1TS_ADDRESS, 1 << 2);
// 
//     ETS_GPIO_INTR_ENABLE();
// 
//     // =================================================
// 
//     digitalWrite(2, HIGH);
//     digitalWrite(5, HIGH);
// 
//     ADC_TEST(NULL);
// 
//     os_printf("\n\nstarted!\n\n");
// }

#ifdef __cplusplus
}
#endif