/****************************************************************************
 *   FILENAME: BufferedFileWriter.cpp
 *   Copyright (c) 2020 EmbedHead Design; All Rights Reserved
 *   PURPOSE: Boost file write performance by adding buffering.
 *   DEPENDENCIES, LIMITATIONS, & DESIGN NOTES:
 *       See .h file.
 ****************************************************************************/

#include "BufferedFileWriter.h"
#include <stdint.h>
#include <stdbool.h>
#include <string.h>
#include "debugIO.h"
#include "FS.h"


BufferedFileWriter::BufferedFileWriter(void)
{
    bytesWrittenTotal = 0;
    file = NULL;
    writeEndPtr = buff + sizeof(buff);
    clear();
}

BufferedFileWriter::~BufferedFileWriter(void)
{
    if (NULL != file) {
        flush();
        file = NULL;
    }
}

// Connect to (opened for write) file.  Clear buffer.  
// Must NOT reset bytes written count (see file header), because code may close and
// re-open (append to) the same file in order to force an update of the directory entry.
void BufferedFileWriter::setFile(FS_FILE *_file)
{
    file = _file;
    clear();
}

// Return number of bytes in the buffer.
size_t BufferedFileWriter::bufferCount(void)
{
    return (size_t)(writePtr - buff);
}

void BufferedFileWriter::resetBytesWrittenTotal(void)
{
    bytesWrittenTotal = 0;
}

// Return total bytes written (including bytes still residing in buffer, not yet flushed
// to file) since initialization or last clear().
size_t BufferedFileWriter::getBytesWrittenTotal(void)
{
    return bytesWrittenTotal;
}

// Clear buffer before first use, or to reinitialize.
// Must NOT zero the total count of bytes written (see file header comments).
// May be called repeatedly.
void BufferedFileWriter::clear()
{
    writePtr = buff;
}

// Flush write buffer to disk.
// After the last write, call flush().
// Returns FS_FWrite() return code, or WriteFail if setFile() was not called with a non-NULL file pointer.
uint32_t BufferedFileWriter::flush(void)
{
    uint32_t retval = 0;
    if (NULL == file) {
        retval = WriteNoFile;
    } else if (writePtr > buff) {
        setDebug4(true);
        retval = FS_FWrite(buff, (size_t)(writePtr - buff), 1, file);
        setDebug4(false);
        writePtr = buff;
    }
    return retval;
}

// Write data to disk buffer with specified length (handles binary).
// When the disk buffer is full, flush disk buffer to disk.
// After the last write, user must call flush().
// Returns FS_FWrite() return code if buffer is flushed, 0 otherwise.
uint32_t BufferedFileWriter::write(const char *source,    // Buffer of data to write to disk buffer / disk
        size_t nChars)         // Count of bytes just written to buffer.
{
    uint32_t retval = 0;

    if (NULL == file) {
        retval = WriteNoFile;
    } else {
        while (nChars > 0) {
            *writePtr++ = *source++;
            --nChars;
            ++bytesWrittenTotal;
            // If full:  flush to disk and set write pointer back to beginning of buff.
            if (writePtr >= writeEndPtr) {
                retval = flush();
            }
        }
    }
    return retval;
}

// Write string to disk buffer, length from strlen().  Writes 0 bytes for NULL string.
// When the disk buffer is full, flush disk buffer to disk.
// After the last write, user must call flush().
// If buffer is flushed, returns FS_FWrite() return code (or 0xffff if setFile() was not called with a
// non-NULL file pointer), 0 otherwise.
uint32_t BufferedFileWriter::writeStr(const char *string)
{
    uint32_t retval = 0;
    if (NULL != string) {
        retval = write(string, strlen(string));
    }
    return retval;
}

// Logging-style printf formatting into disk buffer via fixed-length line buffer.
// Writes 0 bytes for NULL fmt.
// When the disk buffer is full, flush disk buffer to disk.
// After the last write, user must call flush().
// Returns true on success (includes file has been set), false if unable to write or file has not been set.
bool BufferedFileWriter::logPrintf(const char *fmt, ...)
{
    va_list vl;
    va_start(vl, fmt);
    int nChars = vsnprintf(lineBuff, LineBuffSize, fmt, vl);
    va_end(vl);
    write(lineBuff, nChars);
}
