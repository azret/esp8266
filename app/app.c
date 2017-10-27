#ifdef __cplusplus
extern "C" {
#endif

#include "app.h"

#define SAP_FACTORY_SSID "Alpha"
#define SAP_FACTORY_PWD ""

extern bool ICACHE_FLASH_ATTR serv(uint32 port);
	
LOCAL bool ICACHE_FLASH_ATTR wifi_set_softap(const char* ssid, const char* pwd) {

    if (ssid && os_strlen(ssid) > 0 && (os_strlen(ssid) > 31 || os_strlen(ssid) < 3)) {
        log("invalid \"ssid\"!\n");
        return false;
    }

    if (pwd && os_strlen(pwd) > 0 && (os_strlen(pwd) > 63 || os_strlen(pwd) < 8)) {
        log("invalid \"pwd\"!\n");
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

    } else {

        ssid =0;

    }

    bool res = wifi_softap_set_config_current(&conf);

    if (!res) {
        log(
            "wifi_softap_set_config_current(\"%s\") failed!\n", ssid);

        return false;
    }

    if (!ssid) {
        return true;
    }

    if (wifi_softap_dhcps_status() != DHCP_STARTED) {

        if (!wifi_softap_dhcps_start() || (wifi_softap_dhcps_status() != DHCP_STARTED)) {

            log("wifi_softap_dhcps_start() failed!\n");

            os_memset(
                &conf, 0, sizeof(conf));

            wifi_softap_set_config_current(&conf);

            return false;

        }

    }

    struct ip_info inf;

    if (!wifi_get_ip_info(SOFTAP_IF, &inf)) {

        log(
            "wifi_get_ip_info() failed!\n");

        return false;

    }

    log("ap: %u.%u.%u.%u\n", (inf.ip.addr >> (8 * 0)) & 0xff, (inf.ip.addr >> (8 * 1)) & 0xff, (inf.ip.addr >> (8 * 2)) & 0xff, (inf.ip.addr >> (8 * 3)) & 0xff);

    return true;

}

LOCAL void ICACHE_FLASH_ATTR wifi_event_handler(void* arg) {

	System_Event_t* e = (System_Event_t*)(arg);

	switch (e->event) {

	case EVENT_SOFTAPMODE_PROBEREQRECVED:

		log("EVENT_SOFTAPMODE_PROBEREQRECVED: \"%02X-%02X-%02X-%02X-%02X-%02X\", rssi: %d\n",
			MAC2STR(e->event_info.ap_probereqrecved.mac),
			e->event_info.ap_probereqrecved.rssi);

		break;

	case EVENT_SOFTAPMODE_STACONNECTED:

		log("EVENT_SOFTAPMODE_STACONNECTED: \"%02X-%02X-%02X-%02X-%02X-%02X\"\n", MAC2STR(e->event_info.sta_connected.mac));
		log("aid: %d\n", e->event_info.sta_connected.aid);

		break;

	case EVENT_STAMODE_CONNECTED:

		log("EVENT_STAMODE_CONNECTED\n");
		break;

	case EVENT_STAMODE_DISCONNECTED:

		log("EVENT_STAMODE_DISCONNECTED\n");
		break;

	case EVENT_STAMODE_GOT_IP:

		log("EVENT_STAMODE_GOT_IP\n");
		break;

	case EVENT_OPMODE_CHANGED:

		log("EVENT_OPMODE_CHANGED: %d\n", e->event_info.opmode_changed.new_opmode);

		struct ip_info inf;

		os_memset(
			&inf,
			0, sizeof(inf));

		if (wifi_get_ip_info(SOFTAP_IF, &inf)) {
			if (inf.ip.addr != 0) {
				log("listen: %d.%d.%d.%d:80\n", ip2str4(inf.ip.addr));
				if (!serv(80)) {
					log("serv(%d) failed!\n", 80);
				}
			}
		}

		break;
		
	default:

		log("evt %d\n", e->event);
		break;

	}
}

void ICACHE_FLASH_ATTR start() 
{
	log("\n");

	/**
	*/

	uint8 macaddr[6];

	if (!wifi_get_macaddr(STATION_IF, macaddr)) {

		log(
			"wifi_get_macaddr(STATION_IF) failed!\n");

		return;

	}

	log(
		"MAC(STATION_IF): \"%02X-%02X-%02X-%02X-%02X-%02X\"\n", MAC2STR(macaddr));

	/**
	*/

	if (!wifi_get_macaddr(SOFTAP_IF, macaddr)) {

		log(
			"wifi_get_macaddr(SOFTAP_IF) failed!\n");

		return;

	}

	log(
		"MAC(SOFTAP_IF): \"%02X-%02X-%02X-%02X-%02X-%02X\"\n", MAC2STR(macaddr));

	/**
	*/

	log("\n");

	wifi_set_event_handler_cb((wifi_event_handler_cb_t)wifi_event_handler);

	if (!wifi_set_opmode_current(SOFTAP_MODE))
	{
		log(
			"wifi_set_opmode_current(SOFTAP_MODE) failed!\n");

		return;
	}

	if (!wifi_set_softap(SAP_FACTORY_SSID, SAP_FACTORY_PWD))
	{
		log(
			"wifi_set_softap(SOFTAP_MODE) failed!\n");

		return;
	}

    log("\nstarted!\n");
}

#ifdef __cplusplus
}
#endif