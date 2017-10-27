#ifdef __cplusplus
extern "C" {
#endif

#define ssl_en 0

#include "app.h"

LOCAL void ICACHE_FLASH_ATTR writ(struct espconn *ptrespconn, char *pbuf, uint16 length) {

    log("%s\n", pbuf);

    espconn_sent(
        ptrespconn,
        (uint8*)pbuf,
        length);

}

LOCAL void ICACHE_FLASH_ATTR resp(void *arg) {

    struct espconn *ptrespconn = (struct espconn*)arg;

    const char *content =
        "{\r\n"
        "   status: \"OK\"\r\n"
        "}";

	char headers[256];

    os_memset(
        headers,
        0, sizeof(headers));

    os_sprintf(headers + os_strlen((const char *)headers), "HTTP/1.0 200 OK\r\n");
    os_sprintf(headers + os_strlen((const char *)headers), "Server: lwIP\r\n");
    os_sprintf(headers + os_strlen((const char *)headers), "Pragma: no-cache\r\n");
    os_sprintf(headers + os_strlen((const char *)headers), "Content-Type: text/plain\r\n");
    os_sprintf(headers + os_strlen((const char *)headers), "Content-Length: %d\r\n", os_strlen(content));
    os_sprintf(headers + os_strlen((const char *)headers), "\r\n");

    uint16 len = os_strlen((const char *)headers) + os_strlen(content);

    uint8 * pbuf =
        (uint8 *)os_malloc(len + 1);

    os_memset(
        pbuf,
        0, len + 1);

    if (pbuf) {

        os_memcpy(pbuf + os_strlen((const char*)pbuf), headers, os_strlen(headers));
        os_memcpy(pbuf + os_strlen((const char*)pbuf), content, os_strlen(content));

        writ(ptrespconn, (char*)pbuf, len);

        os_free(pbuf);
    }
    
}

LOCAL const char* STATUS2STR(uint8 st) {

#define STATION_STOPPED 0xFF
    switch (st)
    {
        case STATION_STOPPED: return "disconnected";
        case STATION_IDLE: return "disconnected";
        case STATION_CONNECTING: return "connecting";
        case STATION_WRONG_PASSWORD: return "failed";
        case STATION_NO_AP_FOUND: return "failed";
        case STATION_CONNECT_FAIL: return "failed";
        case STATION_GOT_IP: return "connected";
        default: return "unknown";
    }

}

LOCAL void ICACHE_FLASH_ATTR statu(void *arg) {

    uint8 wifi_station_macaddr[6];

    if (!wifi_get_macaddr(STATION_IF, wifi_station_macaddr)) {

        os_memset(
            wifi_station_macaddr,
            0, sizeof(wifi_station_macaddr));
    }

    struct ip_info wifi_station_ip_info;

    if (!wifi_get_ip_info(STATION_IF, &wifi_station_ip_info)) {

        os_memset(
            &wifi_station_ip_info,
            0, sizeof(wifi_station_ip_info));
    }

    struct station_config wifi_station_settings;

    if (!wifi_station_get_config(&wifi_station_settings)) {

        os_memset(
            &wifi_station_settings,
            0, sizeof(wifi_station_settings));
    }

    struct espconn *ptrespconn = (struct espconn*)arg;

    uint8 * content =
        (uint8 *)os_malloc(1024);

    *content = 0;

    uint8 st = wifi_station_get_connect_status();

    os_sprintf((char*)(content + os_strlen((const char*)content)),
        "{\r\n"
        "   station: {\r\n"
        "       mac: \"%02X-%02X-%02X-%02X-%02X-%02X\",\r\n"
        "       ssid: \"%s\",\r\n"
        "       ip: \"%d.%d.%d.%d\",\r\n"
        "       status: \"%s\"\r\n"
        "   }\r\n"
        "}",
        MAC2STR(wifi_station_macaddr),
        wifi_station_settings.ssid,
        ip2str4(wifi_station_ip_info.ip.addr),
        STATUS2STR(st)
    );

    char headers[256];

    os_memset(
        headers,
        0, sizeof(headers));

    os_sprintf(headers + os_strlen((const char*)headers), "HTTP/1.0 200 OK\r\n");
    os_sprintf(headers + os_strlen((const char*)headers), "Server: SDK/%s\r\n", system_get_sdk_version());
    os_sprintf(headers + os_strlen((const char*)headers), "Pragma: no-cache\r\n");
    os_sprintf(headers + os_strlen((const char*)headers), "Content-Type: application/json\r\n");
    os_sprintf(headers + os_strlen((const char*)headers), "Content-Length: %d\r\n", os_strlen((const char *)content));
    os_sprintf(headers + os_strlen((const char*)headers), "\r\n");

    uint16 len = os_strlen((const char*)headers) + os_strlen((const char*)content);

    uint8 * pbuf =
        (uint8 *)os_malloc(len + 1);

    os_memset(
        pbuf,
        0, len + 1);

    if (pbuf) {

        os_memcpy(pbuf + os_strlen((const char*)pbuf), headers, os_strlen((const char*)headers));
        os_memcpy(pbuf + os_strlen((const char*)pbuf), content, os_strlen((const char*)content));

        writ(ptrespconn, (char*)pbuf, len);
        os_free(pbuf);
    }

    os_free(content);

}

LOCAL void ICACHE_FLASH_ATTR err(void *arg) {

    struct espconn *ptrespconn = (struct espconn*)arg;

    char headers[256];

    os_memset(
        headers,
        0, sizeof(headers));

    os_sprintf(headers + os_strlen((const char *)headers), "HTTP/1.0 404 Not Found\r\n");
    os_sprintf(headers + os_strlen((const char *)headers), "Server: SDK/%s\r\n", system_get_sdk_version());
    os_sprintf(headers + os_strlen((const char *)headers), "Content-Length: 0\r\n");
    os_sprintf(headers + os_strlen((const char *)headers), "\r\n");

    uint16 len = os_strlen((const char *)headers);

    uint8 * pbuf =
        (uint8 *)os_malloc(len + 1);

    os_memset(
        pbuf,
        0, len + 1);

    if (pbuf) {

        os_memcpy(pbuf + os_strlen((char*)pbuf), headers, os_strlen(headers));

        writ(ptrespconn, (char*)pbuf, len);

        os_free(pbuf);

    }

}

LOCAL os_timer_t *restart_10ms;

LOCAL void ICACHE_FLASH_ATTR restart_10ms_cb(void *arg)
{
	system_restart();
}

LOCAL void ICACHE_FLASH_ATTR recv(void *arg, char *precv, unsigned short length) {

    struct espconn *ptrespconn = (struct espconn*)arg;

    log("[recv] %d.%d.%d.%d:%d\n", ptrespconn->proto.tcp->remote_ip[0],
        ptrespconn->proto.tcp->remote_ip[1], ptrespconn->proto.tcp->remote_ip[2],
        ptrespconn->proto.tcp->remote_ip[3], ptrespconn->proto.tcp->remote_port);

    log("%u\n", length);
    log("%s\n", precv);

    char* get = (char *)os_strstr(precv, "GET / HTTP/1.");

    if (get) {
		
        statu(arg);
        return;

    }

    char* restart = (char *)os_strstr(precv, "GET /restart HTTP/1.");

	if (!restart) {
		restart = (char *)os_strstr(precv, "POST /GET HTTP/1.");
	}

    if (restart) {

        if (restart_10ms == NULL) {
            restart_10ms = (os_timer_t *)os_malloc(sizeof(os_timer_t));
        }

        os_timer_disarm(restart_10ms);
        os_timer_setfn(restart_10ms, (os_timer_func_t *)restart_10ms_cb, NULL);
        os_timer_arm(restart_10ms, 10, 0);  // delay 10ms, then do

        statu(arg);

        return;

    }

    char * post = (char *)os_strstr(precv, "POST / HTTP/1.");

    if (post) {

        err(arg);
        return;

    }

    err(arg);

}

LOCAL void ICACHE_FLASH_ATTR recon(void *arg, sint8 err) {

    struct espconn *pesp_conn = (struct espconn*)arg;

    log("recon %d.%d.%d.%d:%d err %d\n", pesp_conn->proto.tcp->remote_ip[0],
        pesp_conn->proto.tcp->remote_ip[1], pesp_conn->proto.tcp->remote_ip[2],
        pesp_conn->proto.tcp->remote_ip[3], pesp_conn->proto.tcp->remote_port, err);

}

LOCAL void ICACHE_FLASH_ATTR discon(void *arg) {

    struct espconn *pesp_conn = (struct espconn*)arg;

    log("discon %d.%d.%d.%d:%d\n", pesp_conn->proto.tcp->remote_ip[0],
        pesp_conn->proto.tcp->remote_ip[1], pesp_conn->proto.tcp->remote_ip[2],
        pesp_conn->proto.tcp->remote_ip[3], pesp_conn->proto.tcp->remote_port);

}

LOCAL void ICACHE_FLASH_ATTR accept(void *arg) {

    struct espconn *pesp_conn = (struct espconn*)arg;

    espconn_regist_recvcb(pesp_conn, recv);
    espconn_regist_reconcb(pesp_conn, recon);
    espconn_regist_disconcb(pesp_conn, discon);

    log("accept %d.%d.%d.%d:%d\n", pesp_conn->proto.tcp->remote_ip[0],
        pesp_conn->proto.tcp->remote_ip[1], pesp_conn->proto.tcp->remote_ip[2],
        pesp_conn->proto.tcp->remote_ip[3], pesp_conn->proto.tcp->remote_port);

}

bool ICACHE_FLASH_ATTR serv(uint32 port) {

    uint8 mod = wifi_get_opmode();

    if (mod != STATION_MODE && mod != SOFTAP_MODE && mod != STATIONAP_MODE) {
        log("wifi mode is not configured!");
        return false;
    }

    LOCAL struct espconn esp_conn;
    LOCAL esp_tcp esptcp;

    esp_conn.type = ESPCONN_TCP;
    esp_conn.state = ESPCONN_NONE;
    esp_conn.proto.tcp = &esptcp;
    esp_conn.proto.tcp->local_port = port;
    espconn_regist_connectcb(&esp_conn, accept);
    espconn_accept(&esp_conn);

    log("serv %d.%d.%d.%d:%d\n", esp_conn.proto.tcp->local_ip[0],
        esp_conn.proto.tcp->local_ip[1], esp_conn.proto.tcp->local_ip[2],
        esp_conn.proto.tcp->local_ip[3], esp_conn.proto.tcp->local_port);

    return true;

}

#ifdef __cplusplus
}
#endif