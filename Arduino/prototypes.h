#ifndef _PROTOTYPES_H
#define _PROTOTYPES_H

#include <arduino.h>

void I2CReceive(int byteCount);
void I2CSend();
int GetFreeActionSlot();
#ifdef DEBUG
void PrintBuffer(byte* buf, int count);
#endif

//// CRC
unsigned short ComputeCRC(byte* b);
int CheckCRC(byte* b);
void PutCRC(byte* b);

/// EEPROM_CONFIG
void EEPROM_Erase();
void EEPROM_ReadSettings();
int EEPROM_writeI2Caddr(byte addr);
int EEPROM_writeType(byte type);
int EEPROM_writeDesc(char* desc);
void EEPROM_writeVersion();

/// modCONF
uint32_t processCommandConf();
void processConfIdent();
void processConfVersion();
void processConfAddress();
void processConfType();
void processConfDescription();
uint32_t processConfSoftReset();
uint32_t processConfHardReset();

//// modGENPURP
void processCommandGenPurp();
void processGenPurpIdent();
void processGenPurpPreset();
void processGenPurpSet();
void setupGenPurp();
void loopGenPurp();
void emerGenPurp();

//// modLIGHT
void processCommandLight();
void processLightIdent();
void processLightPreset();
void processLightScenario();
void processLightScenarioStop();
void processLightSet();
void processLightSetDimmable();
void setupLight();
void loopLight();
void emerLight();

#endif _PROTOTYPES_H

