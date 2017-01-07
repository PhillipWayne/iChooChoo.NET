#ifndef _ICHOOCHOO_H
#define _ICHOOCHOO_H

#define PROG_VERSION_MAJ  0x00 // 0
#define PROG_VERSION_MIN  0x28 //40
#define PROG_VERSION_BLD  0x00 // 0

#define MODTYPE_GENPURP   0x20
#define MODTYPE_LIGHT     0x21

#define PACKETSIZE 20
#define DESCSIZE 14
#define MAXOUTPUTS 16
#define MAXACTIONS 10

// I2C Addresses goes from 0x08 to 0x77 = 112 total addresses
#define I2C_ADDR_MIN 0x08
#define I2C_ADDR_MAX 0x77

#endif _ICHOOCHOO_H
