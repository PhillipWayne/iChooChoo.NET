#pragma once

#include <stdint.h>

#define I2C_ADDR_MIN 0x08
#define I2C_ADDR_MAX 0x77

#define PACKETSIZE 20

//#define byte unsigned char

#ifdef __cplusplus
extern "C" {
#endif

int I2CSetup(int addr, uint8_t busId);

int I2CScan(uint8_t busId, uint8_t* data);

int I2CRead(int fd);

int I2CReadAddr(int addr);

int I2CReadBlock(int fd, uint8_t* myData);

int I2CReadBlockAddr(int addr, uint8_t* myData);

int I2CWriteBlock(int fd, uint8_t* bfr);

int I2CWriteBlockAddr(int addr, uint8_t* bfr);

void PrintBuffer(uint8_t* bfr, int count);

/*extern int openBus (char*);
extern int closeBus (int);
extern int writeBytes (int, int, byte*, int);
extern int readBytes (int, int, byte*, int);
*/

#ifdef __cplusplus
}
#endif
