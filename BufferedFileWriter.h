/****************************************************************************
 *   FILENAME: BufferedFileWriter.h
 *   Copyright (c) 2020 EmbedHead Design; All Rights Reserved
 *   PURPOSE: Boost file write performance by adding buffering.
 *   DEPENDENCIES, LIMITATIONS, & DESIGN NOTES:
 *       NOT a circular buffer:  accumulates bytes until either the buffer is full, which
 *       triggers an automatic flush(), or until flush() is called.  Then the buffer contents
 *       are written to media and the buffer is cleared.
 *
 *       In one context using Segger emFile with an SD card, FS_FWrite() has a great deal of 
 *       overhead.  In one example, using a 4k buffer for file writes reduced time to write
 *       10,000 lines from many minutes to under two seconds.  Using this class allows minimizing
 *        the number of FS_FWrite() calls.
 *
 *       Counts the bytes written.
 *       Current usage includes writing logfiles, and we track the number of bytes written
 *       to the file to avoid the (roughly 100ms) very expensive file size check needed to
 *       determine whether to roll-over to a new logfile (when a file size limit is exceeded).
 *
 *       The (same) logfiles are repeatedly closed (to flush to disk) and reopened in order to
 *       avoid data loss by ensuring that:
 *        - data is written to media, and
 *        - the media directory is updated to record the existence of the new data.
 *       The count of bytes written to a file must be reset when opening a new file and
 *       NOT be reset when re-opening the same file after a close / re-open.  Thus resetting
 *       the byte count is a separate operation from setting the file.
 *
 *       Suggest using only static instances of this class in order to keep the (large)
 *       data and line buffers off of the stack.
 *
 ****************************************************************************/

#ifndef BUFFERED_FILE_WRITER_H
#define BUFFERED_FILE_WRITER_H

#include <stdarg.h>
#include <stdint.h>
#include <stdio.h>
#include "debugIO.h"
#include "FS.h"

class BufferedFileWriter
{
public:
    enum WriteResult {
        WriteNoFile = UINT32_MAX      // Unable to write because file pointer hasn't been set
    };

    // Write buffer size; made constant to allow static allocation.
    static const size_t BufferSize = 4096;
    // Line buffer size; made constant to allow static allocation.
    static const size_t LineBuffSize = 2048;

    BufferedFileWriter(void);

    virtual ~BufferedFileWriter(void);

    // Connect to ((re-)opened for write) file.  Clear buffer.
    // Must NOT reset bytes written count (see file header), because code may close and
    // re-open (append to) the same file in order to update the directory entry.
    void setFile(FS_FILE *_file);

    // Return number of bytes buffered.
    size_t bufferCount(void);

    // Return bytes written (including buffer) since initialization or last resetBytesWrittenTotal().
    size_t getBytesWrittenTotal(void);

    // Reset count of bytes written.
    void resetBytesWrittenTotal(void);

    // Clear buffer.  
    // Must NOT zero the total count of bytes written (see file header comments).
    void clear(void);

    // Flush disk buffer to disk.
    // After the last write, call flush().
    // Returns FS_FWrite() return code,
    // or WriteNoFile if setFile() was never called, or was last called with a NULL file pointer.
    uint32_t flush(void);

    // Write (binary) data to disk buffer with specified length.
    // When the disk buffer is full, flush disk buffer to disk.
    // After the last write, user must call flush().
    // If buffer is flushed, returns FS_FWrite() return code (number of bytes written),
    // or WriteNoFile if setFile() was never called, or was last called with a NULL file pointer),
    // 0 otherwise.
    uint32_t write(const char *source,    // Source buffer of data to write to disk buffer / disk
            size_t nChars);         // Count of bytes just written to buffer.

    // Write string to disk buffer, length from strlen().  Writes 0 bytes for NULL string.
    // When the disk buffer is full, flush disk buffer to disk.
    // After the last write, user must call flush().
    // If buffer is flushed, returns FS_FWrite() return code,
    // If buffer is flushed, returns FS_FWrite() return code (number of bytes written),
    // or WriteNoFile if setFile() was never called, or was last called with a NULL file pointer),
    // 0 otherwise.
    uint32_t writeStr(const char *string);

    // vprintf formatting into disk buffer.
    // When the disk buffer is full, flush disk buffer to disk.
    // After the last write, user must call flush().
    // Return code is number of bytes written,
    // or WriteNoFile if setFile() was never called or was last called with a NULL file pointer.
    int vprintf(const char * fmt, va_list arglist)
            _ATTRIBUTE ((__format__ (__printf__, 2, 0)));

private:
    // Block copy-ctor, assignment operator.
    BufferedFileWriter(const BufferedFileWriter &obj);
    BufferedFileWriter& operator=(const BufferedFileWriter& obj);

    // File data buffer
    char        buff[BufferSize];
    // printf line buffer
    char        lineBuff[LineBuffSize + 1];
    FS_FILE *   file;
    char *      writePtr;
    const char * writeEndPtr;
    // Bytes written total, including those still in the buffer and those flushed to the file,
    // since initialization or the last resetBytesWrittenTotal() call.
    size_t      bytesWrittenTotal;
};

#endif //ndef BUFFERED_FILE_WRITER_H
