#ifndef _BICCP_H
#define _BICCP_H

#define BICCP_ERROR    0x00
#define BICCP_SUCCESS  0xFF

#define BICCP_GRP_CONF                0x01

#define BICCP_CMD_CONF_IDENT          0x01
#define BICCP_CMD_CONF_VERSION        0x02
#define BICCP_CMD_CONF_ADDR           0x10
#define BICCP_CMD_CONF_TYPE           0x11
#define BICCP_CMD_CONF_DESC           0x12
#define BICCP_CMD_CONF_SOFTRST        0xFE
#define BICCP_CMD_CONF_HARDRST        0xFF

#define BICCP_GRP_GENPURP             0x20

#define BICCP_CMD_GENPURP_IDENT       0x01
#define BICCP_CMD_GENPURP_PRESET      0x10
#define BICCP_CMD_GENPURP_OUT_STR     0x20
#define BICCP_CMD_GENPURP_OUT_END     0x2F

#define BICCP_GRP_LIGHT               0x21

#define BICCP_CMD_LIGHT_IDENT         0x01
#define BICCP_CMD_LIGHT_PRESET        0x10
#define BICCP_CMD_LIGHT_OUT_STR       0x20
#define BICCP_CMD_LIGHT_OUT_END       0x2F
#define BICCP_CMD_LIGHT_SCENSTART     0x30
#define BICCP_CMD_LIGHT_SCENSTOP      0x31

#define BICCP_SCN_LIGHT_PROGCHANGE    0x01
#define BICCP_SCN_LIGHT_TUNGON        0x02
#define BICCP_SCN_LIGHT_TUNGOFF       0x03
#define BICCP_SCN_LIGHT_BLKLED        0x10
#define BICCP_SCN_LIGHT_BLKOLD        0x11
#define BICCP_SCN_LIGHT_TRFCFRA       0x12
#define BICCP_SCN_LIGHT_TRFCDEU       0x13
#define BICCP_SCN_LIGHT_CHASER        0x14
#define BICCP_SCN_LIGHT_ARCWELDING    0x15
#define BICCP_SCN_LIGHT_CAMERAFLASH   0x16
#define BICCP_SCN_LIGHT_FIRE          0x17

#endif _BICCP_H
