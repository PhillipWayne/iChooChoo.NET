#include <stdio.h>
#include <linux/i2c-dev.h>
#include <fcntl.h>
#include <string.h>
#include <sys/ioctl.h>
#include <unistd.h>
#include <errno.h>

#include "libICC.h"

int I2CSetup(int addr, uint8_t busId)
{
	char device[] = "/dev/i2c-x";
	device[9] = '0' + busId;

	int fd;

	if((fd = open(device, O_RDWR)) < 0)
		return -1;

	if(ioctl(fd, I2C_SLAVE, addr) < 0)
		return -1;

	return fd;
}

int I2CScan(uint8_t busId, uint8_t* data)
{
	int iReturn = 0;
	char device[] = "/dev/i2c-x";
	device[9] = '0' + busId;
	int fd;

	if((fd = open(device, O_RDWR)) < 0)
		return -1;

	for(int i = I2C_ADDR_MIN; i <= I2C_ADDR_MAX; i++)
	{
		if (ioctl(fd, I2C_SLAVE, i) < 0)
			continue;

		int res = i2c_smbus_write_quick(fd, I2C_SMBUS_WRITE);
		if (res >= 0)
		{
			*(data + i) = 1;
			iReturn++;
		}
	}

	return iReturn;
}

int I2CRead(int fd)
{
	union i2c_smbus_data data;

	if(i2c_smbus_access(fd, I2C_SMBUS_READ, 0, I2C_SMBUS_BYTE, &data))
		return -1;
	else
		return data.byte && 0xff;
}

int I2CReadAddr(int addr)
{
	int fd = I2CSetup(addr, 1);
	printf("I2CReadAddr : File descriptor : %d\n", fd);

	union i2c_smbus_data data;

	if(i2c_smbus_access(fd, I2C_SMBUS_READ, 0, I2C_SMBUS_BYTE, &data))
		return -1;
	else
		return data.byte && 0xff;
}

int I2CReadBlock(int fd, uint8_t* myData)
{
	return i2c_smbus_read_i2c_block_data(fd, 0xCC, PACKETSIZE, myData);
}

int I2CReadBlockAddr(int addr, uint8_t* myData)
{
	int fd = I2CSetup(addr, 1);
	printf("I2CReadBlockAddr : File descriptor : %d\n", fd);

	return i2c_smbus_read_i2c_block_data(fd, 0xCC, PACKETSIZE, myData);
}

int I2CWriteBlock(int fd, uint8_t* bfr)
{
	union i2c_smbus_data data;
	int i;
	data.block[0] = PACKETSIZE - 1;
	for(i = 0; i < PACKETSIZE - 1; i++)
		data.block[i + 1] = bfr[i + 1];

	return i2c_smbus_access(fd, I2C_SMBUS_WRITE, bfr[0], I2C_SMBUS_I2C_BLOCK_DATA, &data);
}

int I2CWriteBlockAddr(int addr, uint8_t* bfr)
{
	int fd = I2CSetup(addr, 1);
	printf("I2CWriteBlockAddr : File descriptor : %d\n", fd);

	union i2c_smbus_data data;
	int i;
	data.block[0] = PACKETSIZE - 1;
	for(i = 0; i < PACKETSIZE - 1; i++)
		data.block[i + 1] = bfr[i + 1];

	return i2c_smbus_access(fd, I2C_SMBUS_WRITE, bfr[0], I2C_SMBUS_I2C_BLOCK_DATA, &data);
}

void PrintBuffer(uint8_t* bfr, int count)
{
	printf("0x");
	for(int i = 0; i < count; i++)
	{
		printf("%02x", bfr[i]);
		if (i < (count - 1))
			printf("-");
	}
}


/*

int openBus (char* busFileName)
{
	return open (busFileName, O_RDWR);
}

int closeBus (int busHandle)
{
	return close (busHandle);
}

int writeBytes (int busHandle, int addr, byte* buf, int len)
{
	if (ioctl (busHandle, I2C_SLAVE, addr) < 0)
		return -1;

	if (write (busHandle, buf, len) != len)
		return -2;

	return len;
}

int readBytes (int busHandle, int addr, byte* buf, int len)
{
	if (ioctl (busHandle, I2C_SLAVE, addr) < 0)
		return -1;

	int n= read (busHandle, buf, len);

	return n;
}
*/
